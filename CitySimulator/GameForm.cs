﻿using System;
using System.Diagnostics;
using CitySimulator.Tools;
using SFML.Graphics;
using SFML.Window;

namespace CitySimulator {
    internal class GameForm : IDisposable{
        private readonly RenderWindow _window;
        private readonly SfmlCityRenderer _renderer;
        private Vector2f _lastMousePos;

        private Tool _tool;
        private readonly SoundManager _soundManager = new SoundManager();
        private readonly Game _game;

        internal GameForm() {
            // Request a 24-bits depth buffer when creating the window
            var contextSettings = new ContextSettings {
                DepthBits = 24
            };

            // Create the main window
            _window = new RenderWindow(new VideoMode(640, 480), "Silver Street Simulator", Styles.Default, contextSettings);
            _window.SetActive();

            // Setup event handlers
            _window.Closed += OnClosed;
            _window.Resized += OnResized;

            _window.MouseWheelMoved += OnMouseWheelMoved;
            _window.MouseMoved += OnMouseMoved;

            _window.MouseButtonPressed += OnMouseButtonPressed;
            _window.MouseButtonReleased += OMouseButtonReleased;
            _window.KeyPressed += OnKeyPressed;
            
            Random rnd = new Random();

            _game = new Game(rnd.Next());

            _renderer = new SfmlCityRenderer(_game.City);

        }

        private void OnKeyPressed(object sender, KeyEventArgs e) {
            switch (e.Code) {
                case var k when (k >= Keyboard.Key.Num1 && e.Code <= Keyboard.Key.Num9): {
                        var id = k - Keyboard.Key.Num1;
                        try {
                            _tool = new BuildZoneTool(_soundManager, _game.ZoneManager[id]);
                        } catch (IndexOutOfRangeException) {
                            _tool = null;
                        }
                    }
                    break;
                case Keyboard.Key.R:
                    _tool = new BuildRoadTool(_soundManager, _game.ZoneManager[4]);
                    break;
                case Keyboard.Key.B:
                    _tool = new BuildozerTool(_soundManager, _game.ZoneManager[4]);
                    break;
            }
        }

        private void OMouseButtonReleased(object sender, MouseButtonEventArgs e) {
            if (e.Button == Mouse.Button.Left) {
                _tool?.MouseUp(_game, _renderer.View, new Vector2f(e.X, e.Y));
            }
        }

        private void OnMouseButtonPressed(object sender, MouseButtonEventArgs e) {
            if (e.Button == Mouse.Button.Left) {
                _tool?.MouseDown(_game, _renderer.View, new Vector2f(e.X, e.Y));
            }
        }

        internal void GameLoop() { 
            // Start the game loop
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            while (_window.IsOpen()) {
                // Process events
                _window.DispatchEvents();

                var timeMs = stopwatch.ElapsedMilliseconds;
                stopwatch.Restart();

                Update(timeMs);

                _renderer.Draw(_window, RenderStates.Default);

                // Finally, display the rendered frame on screen
                _window.Display();
            }
        }
        
        private void Update(long timeMs) {
            _game.Update(timeMs);

            _window.SetTitle($"Silver Street Simulator - {_game.Money:C} +- {_game.Income:C}");
        }

        private void OnMouseMoved(object sender, MouseMoveEventArgs e) {
            var mouseDrag = _lastMousePos - new Vector2f(e.X, e.Y);
            _lastMousePos = new Vector2f(e.X, e.Y);

            if (Mouse.IsButtonPressed(Mouse.Button.Right)) {
                _renderer.View.TopLeftPos += (mouseDrag * _renderer.View.Zoom);
            } if (Mouse.IsButtonPressed(Mouse.Button.Left)) {
                _tool.MouseDrag(_game, _renderer.View, new Vector2f(e.X, e.Y));
            }
        }

        private void OnMouseWheelMoved(object sender, MouseWheelEventArgs e) {
            if (e.Delta > 0) {
                Zoom(0.5f, new Vector2f(e.X,e.Y));
            } else {
                Zoom(2f, new Vector2f(e.X, e.Y));
            }
        }

        /// <summary>
        /// Function called when the window is closed
        /// </summary>
        private void OnClosed(object sender, EventArgs e) {
            _window.Close();
        }

        private void Zoom(float f, Vector2f mousePos) {
            var mousePosWorld = _renderer.View.ScreenPxToWorldPx(mousePos);
            _renderer.View.Zoom *= f;
            var mousePosNew = _renderer.View.WorldPxToScreenPx(mousePosWorld);

            _renderer.View.TopLeftPos += (mousePosNew - mousePos);
        }

        /// <summary>
        /// Function called when the window is resized
        /// </summary>
        private void OnResized(object sender, SizeEventArgs e) {
            var view = _window.GetView();
            view.Center = _window.Size.ToVector2F() / 2;
            view.Size = _window.Size.ToVector2F();
            _window.SetView(view);
        }

        public void Dispose() {
            _window?.Dispose();
        }
    }
}

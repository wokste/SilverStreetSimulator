using System;
using System.Deployment.Application;
using System.Diagnostics;
using CitySimulator.Tools;
using SFML.Graphics;
using SFML.Window;

namespace CitySimulator {
    internal class GameForm {
        private readonly RenderWindow _window;
        private readonly CityMap _city;
        private readonly SfmlCityRenderer _renderer;
        private Vector2f _lastMousePos;

        private BuildZoneTool _tool = null;
        private readonly ZoneManager _zoneManager = new ZoneManager();
        public double Money = 10000;

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

            _zoneManager.Load(@"D:\AppData\Local\CitySimulator\Assets\buildings.xml");
            _city = new CityGenerator().GenerateCity();
            _renderer = new SfmlCityRenderer(_city);

        }

        private void OnKeyPressed(object sender, KeyEventArgs e) {
            switch (e.Code) {
                case Keyboard.Key.Num1:
                    _tool = new BuildZoneTool(_zoneManager[0]);
                    break;
                case Keyboard.Key.Num2:
                    _tool = new BuildZoneTool(_zoneManager[1]);
                    break;
                case Keyboard.Key.Num3:
                    _tool = new BuildZoneTool(_zoneManager[2]);
                    break;
            }
        }

        private void OMouseButtonReleased(object sender, MouseButtonEventArgs e) {
            if (e.Button == Mouse.Button.Left) {
                _tool?.MouseUp(this, _city, GetWorldCoordinates(e.X, e.Y));
            }
        }

        private void OnMouseButtonPressed(object sender, MouseButtonEventArgs e) {
            if (e.Button == Mouse.Button.Left) {
                _tool?.MouseDown(this, _city, GetWorldCoordinates(e.X, e.Y));
            }
        }

        private Vector2i GetWorldCoordinates(int mouseX, int mouseY) {
            var vecScreenPx = new Vector2f(mouseX, mouseY);
            var vecWorldPx = _renderer.View.ScreenPxToWorldPx(vecScreenPx);
            var vecWens = _renderer.View.WorldPxToWens(vecWorldPx);
            return vecWens;
        }

        internal void GameLoop() { 
            // Start the game loop
            Stopwatch stopwatch = new Stopwatch();
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
            var population = 0;
            var jobs = 0;

            for (int x = 0; x < _city.Width; x++) {
                for (int y = 0; y < _city.Height; y++) {
                    var building = _city.Terrain[x, y].Building;
                    if (building != null) {
                        population += building.Type.Population;
                        jobs += building.Type.Jobs;
                    }
                }
            }

            var workers = Math.Min(0.7f * population, jobs);
            var income = workers * 0.1;

            Money += income * timeMs / 1000;

            _window.SetTitle($"Silver Street Simulator - {Money:C} +- {income:C}");
        }

        private void OnMouseMoved(object sender, MouseMoveEventArgs e) {
            var mouseDrag = _lastMousePos - new Vector2f(e.X, e.Y);
            _lastMousePos = new Vector2f(e.X, e.Y);

            if (Mouse.IsButtonPressed(Mouse.Button.Right)) {
                _renderer.View.TopLeftPos += (mouseDrag * _renderer.View.Zoom);
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
    }
}

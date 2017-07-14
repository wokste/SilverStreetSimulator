using System;
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

        internal GameForm() {
            // Request a 24-bits depth buffer when creating the window
            var contextSettings = new ContextSettings {
                DepthBits = 24
            };

            // Create the main window
            _window = new RenderWindow(new VideoMode(640, 480), "Silver Street Simulator", Styles.Default, contextSettings);

            // Make it the active window for OpenGL calls
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
                if (_tool != null) {
                    _tool.MouseUp(_city, GetWorldCoordinates(e.X, e.Y));
                }
            }
        }

        private void OnMouseButtonPressed(object sender, MouseButtonEventArgs e) {
            if (e.Button == Mouse.Button.Left) {
                if (_tool != null) {
                    _tool.MouseDown(_city, GetWorldCoordinates(e.X, e.Y));
                }
            }
        }

        private Vector2i GetWorldCoordinates(int mouseX, int mouseY) {

            var vecScreenPx = new Vector2f(mouseX, mouseY);

            var view = _window.GetView();
            var vecWorldPx = vecScreenPx + view.Center - view.Size / 2;
            _window.SetView(view);

            IsometricView isoView = new IsometricView();

            var vecWens = isoView.PxToWens(vecWorldPx);
            return new Vector2i((int)vecWens.X, (int)vecWens.Y);
        }

        internal void GameLoop() { 
            // Start the game loop
            while (_window.IsOpen()) {
                // Process events
                _window.DispatchEvents();

                _renderer.Draw(_window, RenderStates.Default);

                // Finally, display the rendered frame on screen
                _window.Display();
            }
        }
        
        private void OnMouseMoved(object sender, MouseMoveEventArgs e) {
            var mouseDrag = _lastMousePos - new Vector2f(e.X, e.Y);
            _lastMousePos = new Vector2f(e.X, e.Y);

            if (Mouse.IsButtonPressed(Mouse.Button.Right)) {
                var view = _window.GetView();
                view.Move(mouseDrag);
                _window.SetView(view);
            }
        }

        private void OnMouseWheelMoved(object sender, MouseWheelEventArgs e) {
            if (e.Delta > 0) {
                Zoom(0.5f);
            } else {
                Zoom(2f);
            }
        }

        /// <summary>
        /// Function called when the window is closed
        /// </summary>
        private void OnClosed(object sender, EventArgs e) {
            _window.Close();
        }

        private void Zoom(float f) {
            var view = _window.GetView();
            view.Zoom(f);
            _window.SetView(view);
        }

        /// <summary>
        /// Function called when the window is resized
        /// </summary>
        private void OnResized(object sender, SizeEventArgs e) {
            var view = _window.GetView();
            view.Size = _window.Size.ToVector2F();
            _window.SetView(view);
        }
    }
}

using System;
using SFML.Graphics;
using SFML.Window;

namespace CitySimulator {
    internal class GameForm {
        private readonly RenderWindow _window;
        private readonly CityMap _city;
        private readonly SfmlCityRenderer _renderer;

        internal GameForm() {
            // Request a 24-bits depth buffer when creating the window
            var contextSettings = new ContextSettings {
                DepthBits = 24
            };

            // Create the main window
            _window = new RenderWindow(new VideoMode(640, 480), "CityMap Simulator", Styles.Default, contextSettings);

            // Make it the active window for OpenGL calls
            _window.SetActive();

            // Setup event handlers
            _window.Closed += OnClosed;
            _window.KeyPressed += OnKeyPressed;
            _window.Resized += OnResized;

            _city = new CityGenerator().GenerateCity();
            _renderer = new SfmlCityRenderer(_city);
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

        /// <summary>
        /// Function called when the window is closed
        /// </summary>
        private void OnClosed(object sender, EventArgs e) {
            _window.Close();
        }

        /// <summary>
        /// Function called when a key is pressed
        /// </summary>
        private void OnKeyPressed(object sender, KeyEventArgs e) {
            switch (e.Code) {
                case Keyboard.Key.Escape:
                    _window.Close();
                    break;
                case Keyboard.Key.PageDown:
                    Zoom(0.5f);
                    break;
                case Keyboard.Key.PageUp:
                    Zoom(2.0f);
                    break;
                case Keyboard.Key.Up:
                    Pan(0,-1);
                    break;
                case Keyboard.Key.Down:
                    Pan(0, 1);
                    break;
                case Keyboard.Key.Left:
                    Pan(-1, 0);
                    break;
                case Keyboard.Key.Right:
                    Pan(1, 0);
                    break;
            }
        }

        private void Zoom(float f) {
            var view = _window.GetView();
            view.Zoom(f);
            _window.SetView(view);
        }

        private void Pan(float x, float y) {
            var view = _window.GetView();
            view.Move(new Vector2f(x,y) * 16);
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

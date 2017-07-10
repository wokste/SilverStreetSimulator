using System;
using SFML.Graphics;
using SFML.Window;

namespace CitySimulator {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main() {
            // Request a 24-bits depth buffer when creating the window
            var contextSettings = new ContextSettings {
                DepthBits = 24
            };

            // Create the main window
            var window = new RenderWindow(new VideoMode(640, 480), "City Simulator", Styles.Default, contextSettings);

            // Make it the active window for OpenGL calls
            window.SetActive();

            // Setup event handlers
            window.Closed += OnClosed;
            window.KeyPressed += OnKeyPressed;
            window.Resized += OnResized;

            var city = new City(256,256);
            var renderer = new SfmlCityRenderer(city);


            // Start the game loop
            while (window.IsOpen()) {
                // Process events
                window.DispatchEvents();

                renderer.Draw(window, RenderStates.Default);

                // Finally, display the rendered frame on screen
                window.Display();
            }
        }

        /// <summary>
        /// Function called when the window is closed
        /// </summary>
        static void OnClosed(object sender, EventArgs e) {
            var window = (RenderWindow)sender;
            window.Close();
        }

        /// <summary>
        /// Function called when a key is pressed
        /// </summary>
        static void OnKeyPressed(object sender, KeyEventArgs e) {
            var window = (RenderWindow)sender;
            switch (e.Code) {
                case Keyboard.Key.Escape:
                    window.Close();
                    break;
                case Keyboard.Key.PageDown:
                    Zoom(window, 0.5f);
                    break;
                case Keyboard.Key.PageUp:
                    Zoom(window, 2.0f);
                    break;
            }   
        }

        private static void Zoom(RenderWindow window, float f) {
            var view = window.GetView();
            view.Zoom(f);
            window.SetView(view);
        }

        /// <summary>
        /// Function called when the window is resized
        /// </summary>
        static void OnResized(object sender, SizeEventArgs e) {
            var window = (RenderWindow)sender;
            var view = window.GetView();
            view.Size = window.Size.ToVector2F();
            window.SetView(view);
        }
    }
}
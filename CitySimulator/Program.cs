using System;
using CitySimulator;
using SFML.Graphics;
using SFML.Window;

namespace window {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main() {
            // Request a 24-bits depth buffer when creating the window
            ContextSettings contextSettings = new ContextSettings();
            contextSettings.DepthBits = 24;

            // Create the main window
            RenderWindow window = new RenderWindow(new VideoMode(640, 480), "City Simulator", Styles.Default, contextSettings);

            // Make it the active window for OpenGL calls
            window.SetActive();

            // Setup event handlers
            window.Closed += new EventHandler(OnClosed);
            window.KeyPressed += new EventHandler<KeyEventArgs>(OnKeyPressed);
            window.Resized += new EventHandler<SizeEventArgs>(OnResized);

            City city = new City(32,32);
            SfmlCityRenderer renderer = new SfmlCityRenderer(city);


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
            Window window = (Window)sender;
            window.Close();
        }

        /// <summary>
        /// Function called when a key is pressed
        /// </summary>
        static void OnKeyPressed(object sender, KeyEventArgs e) {
            Window window = (Window)sender;
            if (e.Code == Keyboard.Key.Escape)
                window.Close();
        }

        /// <summary>
        /// Function called when the window is resized
        /// </summary>
        static void OnResized(object sender, SizeEventArgs e) {
            //GL.Viewport(0, 0, (int)e.Width, (int)e.Height);
        }
    }
}
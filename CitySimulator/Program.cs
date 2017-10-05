using System;
using System.Runtime.Remoting.Messaging;

namespace CitySimulator {
    static class Program {
        public static string AssetsFolder = "D:\\projects\\CitySimulatorData\\Assets\\";
        public static string SavesFolder = "D:\\projects\\CitySimulatorData\\Saves\\";
        
        public static Cache<string, Texture> TextureCache = new Cache<string, Texture>(s => new Texture(s));

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main() {
            //var frm = new GameForm();
            //frm.GameLoop();

            using (var game = new TkGameWindow()) {
                game.Run(30.0);
            }
        }
    }
}
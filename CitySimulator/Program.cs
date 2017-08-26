namespace CitySimulator {
    static class Program {
        public static string AssetsFolder = "D:\\projects\\CitySimulatorData\\Assets\\";
        public static string SavesFolder = "D:\\projects\\CitySimulatorData\\Saves\\";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main() {
            var frm = new GameForm();
            frm.GameLoop();
        }
    }
}
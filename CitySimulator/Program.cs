namespace CitySimulator {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main() {
            var frm = new GameForm();
            frm.GameLoop();
        }
    }
}
using System;

namespace CitySimulator {
    class Game {
        public double Money = 10000;
        public CityMap City;
        public readonly ZoneManager ZoneManager;
        public GrowthSimulator GrowthSimulator;

        public double Income { get; internal set; }

        public Game(int seed) {
            ZoneManager = new ZoneManager();
            ZoneManager.Load($"{Program.AssetsFolder}buildings.xml");

            var cityGenerator = new CityGenerator(seed);
            City = cityGenerator.GenerateCity();

            GrowthSimulator = new GrowthSimulator(City, ZoneManager);
        }

        public void Update(long timeMs) {
            GrowthSimulator.Update(timeMs);

            Income = CalculateIncome();
            Money += Income * timeMs / 1000;
        }

        private double CalculateIncome() {
            var population = 0;
            var jobs = 0;

            for (var x = 0; x < City.Width; x++) {
                for (var y = 0; y < City.Height; y++) {
                    var building = City.Terrain[x, y].Building;
                    if (building != null) {
                        population += building.Type.Population;
                        jobs += building.Type.Jobs;
                    }
                }
            }

            var workers = Math.Min(0.7f * population, jobs);
            return workers * 0.1;
        }
    }
}

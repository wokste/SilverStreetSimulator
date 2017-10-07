using System;

namespace CitySimulator {
    class Game {
        public double Money = 10000;
        public CityMap City;
        public GrowthSimulator GrowthSimulator;
        
        public double Income { get; internal set; }
        public ZoneManager ZoneManager = new ZoneManager();

        public Game(int seed) {
            ZoneManager.Load($"{Program.AssetsFolder}buildings.xml");

            var cityGenerator = new CityGenerator(seed);
            City = cityGenerator.GenerateCity(this);

            GrowthSimulator = new GrowthSimulator(City, ZoneManager);
        }

        public void Update() {
            GrowthSimulator.Update();

            Income = CalculateIncome();
            Money += Income;
        }

        private double CalculateIncome() {
            var population = 0;
            var jobs = 0;

            for (var x = 0; x < City.SizeX; x++) {
                for (var y = 0; y < City.SizeY; y++) {
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

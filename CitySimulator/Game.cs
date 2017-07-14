using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitySimulator {
    class Game {
        public double Money = 10000;
        public CityMap City;

        public double Income { get; internal set; }

        public Game(int seed) {
            var gen = new CityGenerator(seed);
            City = gen.GenerateCity();
        }

        public void Update(long timeMs) {
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

using System;
using SFML.Window;

namespace CitySimulator {
    class CityGenerator {
        private readonly Random _rnd;

        internal CityGenerator(int seed) {
            _rnd = new Random(seed);
        }

        internal CityMap GenerateCity() {
            var cityMap = new CityMap(128, 128);

            GenerateTerrain(cityMap);

            return cityMap;
        }


        private void GenerateTerrain(CityMap cityMap) {
            var heightMap = new PerlinNoise {
                Seed = _rnd.Next(),
                Scale = 16
            };

            var vegitationMap = new PerlinNoise {
                Seed = _rnd.Next(),
                Scale = 3
            };

            for (var x = 0; x < cityMap.Width; x++) {
                for (var y = 0; y < cityMap.Height; y++) {
                    var height = heightMap.Get(x, y);
                    var vegitation = vegitationMap.Get(x, y);

                    if (height < -0.5f) {
                        cityMap.Terrain[x, y].Terrain = 2;
                    } else if (vegitation < -0.5f) {
                        cityMap.Terrain[x, y].Terrain = 1;
                    } else if (vegitation > 0.5f) {
                        cityMap.Terrain[x, y].Terrain = 3;
                    } else {
                        cityMap.Terrain[x, y].Terrain = 0;
                    }
                }
            }
        }
    }
}

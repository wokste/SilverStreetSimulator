using System;
using SFML.Window;

namespace CitySimulator {
    class CityGenerator {
        private readonly Random _rnd = new Random();

        internal CityMap GenerateCity() {
            var cityMap = new CityMap(256, 256);

            GenerateTerrain(cityMap);
            CreateVillage(cityMap);

            return cityMap;
        }


        private void GenerateTerrain(CityMap cityMap) {
            var heightMap = new PerlinNoise {
                Seed = _rnd.Next(),
                Scale = 42
            };

            var vegitationMap = new PerlinNoise {
                Seed = _rnd.Next(),
                Scale = 8
            };

            for (var x = 0; x < cityMap.Width; x++) {
                for (var y = 0; y < cityMap.Height; y++) {
                    var height = heightMap.Get(x, y);
                    var vegitation = vegitationMap.Get(x, y);

                    if (height < -0.5f) {
                        cityMap.Terrain[x, y] = 3;
                    } else if (vegitation < -0.25f) {
                        cityMap.Terrain[x, y] = 2;
                    } else if (vegitation > 0.75f) {
                        cityMap.Terrain[x, y] = 4;
                    } else {
                        cityMap.Terrain[x, y] = 1;
                    }
                }
            }
        }

        private void CreateVillage(CityMap cityMap) {
            BuildingType residential = new BuildingType();

            for (int i = 0; i < 25; i++) {
                cityMap.PlaceBuilding(residential, new Vector2i(_rnd.Next(cityMap.Width - 1), _rnd.Next(cityMap.Height - 1)));
            }
        }

        
    }
}

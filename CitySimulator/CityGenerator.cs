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
            for (var x = 0; x < cityMap.Width; x++) {
                for (var y = 0; y < cityMap.Height; y++) {
                    cityMap.Terrain[x, y] = _rnd.Next(1, 5);
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

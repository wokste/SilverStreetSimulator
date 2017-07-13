using System;
using SFML.Window;

namespace CitySimulator {
    class CityGenerator {
        private readonly Random _rnd = new Random();

        internal CityMap GenerateCity() {
            var cityMap = new CityMap(128, 128);

            GenerateTerrain(cityMap);
            CreateVillage(cityMap);

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

        private void CreateVillage(CityMap cityMap) {
            ZoneManager zoneManager = new ZoneManager();
            zoneManager.Load(@"D:\AppData\Local\CitySimulator\Assets\buildings.xml");

            for (var i = 0; i < 25; i++) {
                var x = _rnd.Next(cityMap.Width - 1);
                var y = _rnd.Next(cityMap.Height - 1);

                var zone = zoneManager[_rnd.Next(3)];

                for (var dx = -4; dx <= 4; dx++) {
                    for (var dy = -4; dy <= 4; dy ++) {
                        cityMap.PlaceBuilding(new Vector2i(x + dx, y + dy), zone.GetRandom(_rnd));
                    }
                }
            }
        }
    }
}

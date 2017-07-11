using System;
using System.Collections.Generic;
using SFML.Graphics;
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
                        cityMap.Terrain[x, y] = 2;
                    } else if (vegitation < -0.5f) {
                        cityMap.Terrain[x, y] = 1;
                    } else if (vegitation > 0.5f) {
                        cityMap.Terrain[x, y] = 3;
                    } else {
                        cityMap.Terrain[x, y] = 0;
                    }
                }
            }
        }

        private void CreateVillage(CityMap cityMap) {
            List<BuildingType> resSmall = new List<BuildingType>();
            List<BuildingType> resLarge = new List<BuildingType>();

            resSmall.Add(new BuildingType {
                Size = new Vector2i(1,1),
                TextureRect = new IntRect(0,0,64,64)
            });
            resSmall.Add(new BuildingType {
                Size = new Vector2i(1, 1),
                TextureRect = new IntRect(64, 0, 64, 64)
            });
            resSmall.Add(new BuildingType {
                Size = new Vector2i(1, 1),
                TextureRect = new IntRect(128, 0, 64, 64)
            });
            resLarge.Add(new BuildingType {
                Size = new Vector2i(1, 1),
                TextureRect = new IntRect(0, 64, 64, 64)
            });
            resLarge.Add(new BuildingType {
                Size = new Vector2i(1, 1),
                TextureRect = new IntRect(64, 64, 64, 64)
            });
            resLarge.Add(new BuildingType {
                Size = new Vector2i(1, 1),
                TextureRect = new IntRect(128, 64, 64, 64)
            });
            resLarge.Add(new BuildingType {
                Size = new Vector2i(1, 1),
                TextureRect = new IntRect(192, 64, 64, 64)
            });
            resLarge.Add(new BuildingType {
                Size = new Vector2i(2, 2),
                TextureRect = new IntRect(0, 128, 128, 128)
            });

            var x = _rnd.Next(cityMap.Width - 1);
            var y = _rnd.Next(cityMap.Height - 1);

            for (var dx = -8; dx <= 8; dx++) {
                for (var dy = -1; dy <= 1; dy += 2) {
                    var buildingType = Math.Abs(dx) < _rnd.Next(3, 5) ? resLarge[_rnd.Next(resLarge.Count)] : resSmall[_rnd.Next(resSmall.Count)];
                    cityMap.PlaceBuilding(buildingType, new Vector2i(x + dx, y + dy));
                }
            }
        }
    }
}

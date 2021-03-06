﻿using System;
using System.Drawing;
using System.Linq;
using CitySimulator.MeshGeneration;

namespace CitySimulator {
    class CityGenerator {
        private readonly Random _rnd;

        internal CityGenerator(int seed) {
            _rnd = new Random(seed);
        }

        internal CityMap GenerateCity(Game game) {
            var cityMap = new CityMap(128, 128);

            GenerateTerrain(game, cityMap);

            return cityMap;
        }

        private void GenerateTerrain(Game game, CityMap cityMap) {
            var heightMap = new PerlinNoise {
                Seed = _rnd.Next(),
                Scale = 16
            };

            var vegitationMap = new PerlinNoise {
                Seed = _rnd.Next(),
                Scale = 3
            };
            
            for (var x = 0; x < cityMap.SizeX + 1; x++) {
                for (var y = 0; y < cityMap.SizeY + 1; y++) {
                    cityMap.HeightMap.Height[x, y] = heightMap.Get(x - 0.5f, y - 0.5f) * 8.0f;
                }
            }

            for (var x = 0; x < cityMap.SizeX; x++)
            {
                for (var y = 0; y < cityMap.SizeY; y++)
                {
                    var height = heightMap.Get(x, y);
                    var vegitation = vegitationMap.Get(x, y);

                    if (height < -0.5f)
                    {
                        cityMap.Terrain[x, y].Terrain = 0;
                    }
                    else
                    {
                        cityMap.Terrain[x, y].Terrain = (vegitation > 0.5f) ? 1 : 2;
                        
                        if (IsValidLoc(cityMap, x, y) && _rnd.NextDouble() < 0.6)
                        {
                            cityMap.PlaceBuilding(new Point(x, y), game.ZoneManager[_rnd.Next(0, 3)].GetRandom(_rnd));
                        }
                    }
                }
            }
        }

        private bool IsValidLoc(CityMap cityMap, int x, int y)
        {
            var heights = new[] {cityMap.HeightMap.Height[x, y], cityMap.HeightMap.Height[x + 1, y], cityMap.HeightMap.Height[x, y+1], cityMap.HeightMap.Height[x + 1, y+1] };
            var min = heights.Min();
            var max = heights.Max();

            return min > -1.5 && (max - min) < 0.5;
        }
    }
}

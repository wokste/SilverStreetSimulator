using System;
using System.Collections.Generic;
using SFML.Window;

namespace CitySimulator {
    class City {
        internal int[,] Terrain;

        internal List<Building> Buildings = new List<Building>();
        internal int Width;
        internal int Height;

        internal City(int width, int height) {
            Width = width;
            Height = height;

            Terrain = new int[Width, Height];

            for (var x = 0; x < Width; x++) {
                for (var y = 0; y < Height; y++) {
                    Terrain[x, y] = 1;
                }
            }

            BuildingType residential = new BuildingType();
            Random rnd = new Random();

            for (int i = 0; i < 25; i++) {
                PlaceBuilding(residential, new Vector2i(rnd.Next(Width - 1), rnd.Next(Height - 1)));
            }

        }

        private void PlaceBuilding(BuildingType type, Vector2i position) {
            Building building = new Building {
                Type = type,
                Position = position
            };
            Buildings.Add(building);
        }
    }
}

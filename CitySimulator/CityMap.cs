using System.Collections.Generic;
using SFML.Window;

namespace CitySimulator {
    class CityMap {
        internal int[,] Terrain;

        internal List<Building> Buildings = new List<Building>();
        internal int Width;
        internal int Height;

        internal CityMap(int width, int height) {
            Width = width;
            Height = height;

            Terrain = new int[Width, Height];
        }

        internal bool PlaceBuilding(BuildingType type, Vector2i position) {
            Building building = new Building {
                Type = type,
                Position = position
            };
            Buildings.Add(building);

            return true;
        }
    }
}

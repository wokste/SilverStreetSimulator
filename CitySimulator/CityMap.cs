﻿using SFML.Window;

namespace CitySimulator {
    class CityMap {
        internal Tile[,] Terrain;
        
        internal int Width;
        internal int Height;

        internal CityMap(int width, int height) {
            Width = width;
            Height = height;

            Terrain = new Tile[Width, Height];
        }

        internal bool IsFreeArea(Vector2i pos, Vector2i size) {
            if (pos.X < 0 || pos.Y < 0 || pos.X + size.X >= Width || pos.Y + size.Y >= Height) {
                return false;
            }

            for (var x = pos.X; x < pos.X + size.X; x++) {
                for (var y = pos.Y; y < pos.Y + size.Y; y++) {
                    var tile = Terrain[x, y];
                    if (tile.Terrain == 2) {
                        return false;
                    }

                    if (tile.Building != null) {
                        return false;
                    }
                }
            }
            return true;
        }

        internal bool PlaceBuilding(Vector2i position, BuildingType type) {
            if (!IsFreeArea(position, type.Size)) {
                return false;
            }

            var building = new Building {
                Type = type
            };

            Terrain[position.X, position.Y].Building = building;
            return true;
        }

        internal struct Tile {
            internal int Terrain;
            internal Building Building;
        }
    }
}

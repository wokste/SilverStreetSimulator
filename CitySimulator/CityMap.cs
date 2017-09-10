using SFML.Window;

namespace CitySimulator {
    class CityMap {
        internal Tile[,] Terrain;
        
        internal readonly int Width;
        internal readonly int Height;

        internal CityMap(int width, int height) {
            Width = width;
            Height = height;

            Terrain = new Tile[Width, Height];

            for (var x = 0; x < Width; x++){
                for (var y = 0; y < Height; y++){
                    Terrain[x, y].ZoneId = -1;
                }
            }
        }

        internal bool IsFreeArea(Vector2i pos) {
            if (pos.X < 0 || pos.Y < 0 || pos.X >= Width || pos.Y >= Height) {
                return false;
            }
            
            var tile = Terrain[pos.X, pos.Y];
            if (tile.Terrain == 2) {
                return false;
            }
            
            return true;
        }

        internal bool PlaceBuilding(Vector2i position, BuildingType type) {
            if (!IsFreeArea(position)) {
                //return false;
            }

            var building = new Building {
                Type = type
            };

            Terrain[position.X, position.Y].Building = building;
            return true;
        }
        
        internal bool IsRoad(int x, int y) {
            if (x < 0 || y < 0 || y >= Width || y >= Height) {
                return false;
            }

            return Terrain[x, y].ZoneId == 4;
        }

        internal struct Tile {
            internal int Terrain;
            internal int ZoneId;
            internal Building Building;
        }
    }
}

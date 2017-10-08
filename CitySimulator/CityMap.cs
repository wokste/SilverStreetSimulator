using System;
using System.Drawing;
using CitySimulator.MeshGeneration;
using OpenTK;

namespace CitySimulator {
    class CityMap
    {
        internal HeightMap HeightMap;

        internal Tile[,] Terrain;
        
        internal readonly int SizeX;
        internal readonly int SizeY;

        internal CityMap(int sizeX, int sizeY) {
            SizeX = sizeX;
            SizeY = sizeY;

            Terrain = new Tile[SizeX, SizeY];
            HeightMap = new HeightMap(SizeX, SizeY);

            for (var x = 0; x < SizeX; x++){
                for (var y = 0; y < SizeY; y++){
                    Terrain[x, y].ZoneId = -1;
                }
            }
        }

        internal bool IsFreeArea(Point pos) {
            if (pos.X < 0 || pos.Y < 0 || pos.X >= SizeX || pos.Y >= SizeY) {
                return false;
            }
            
            var tile = Terrain[pos.X, pos.Y];
            if (tile.Terrain == 0) {
                return false;
            }
            
            return true;
        }

        internal bool PlaceBuilding(Point position, BuildingType type) {
            if (!IsFreeArea(position)) {
                //return false;
            }

            var pos = new Vector2(position.X, position.Y);

            var building = new Building {
                Type = type,
                Mesh = null,
                Pos  = pos
            };

            Terrain[position.X, position.Y].Building = building;
            return true;
        }
        
        internal bool IsRoad(int x, int y) {
            if (x < 0 || y < 0 || y >= SizeX || y >= SizeY) {
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

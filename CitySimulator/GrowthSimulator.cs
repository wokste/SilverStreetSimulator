using SFML.Window;
using System;

namespace CitySimulator
{
    class GrowthSimulator
    {
        CityMap _map;
        ZoneManager _zoneManager;
        Random _rnd = new Random();

        internal GrowthSimulator(CityMap map, ZoneManager zoneManager) {
            _map = map;
            _zoneManager = zoneManager;
        }

        internal void Update(long timeMs)
        {
            for (var x = 0; x < _map.Width; x++)
            {
                for (var y = 0; y < _map.Height; y++)
                {
                    var tile = _map.Terrain[x, y];
                    if (tile.ZoneId < 0)
                        continue;

                    var Zone = _zoneManager[tile.ZoneId];

                    if (_rnd.Next(1, 1000) < timeMs)
                    {
                        _map.PlaceBuilding(new Vector2i(x, y), Zone.GetRandom(_rnd));
                    }
                }
            }
        }
    }
}

using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitySimulator.Desire {
    class CityActor {

        // TODO: This should be moved into zone, I believe. Also, there should be a list of buildings.

        Random _rnd = new Random();
        private string _name;
        ZoneType _zone;

        public CityActor(string name, ZoneType zone) {
            _name = name;
            _zone = zone;
        }

        public void Update(CityMap city, long deltaMs) {
            // TODO: Growth factor should depend on extra variables.
            var growthFactor = 6;

            var count = Math.Floor(_rnd.NextDouble() + (deltaMs / 1000.0) * growthFactor);

            PlaceBuildings(city, (int)count);

            // TODO: Building decay
        }

        private void PlaceBuildings(CityMap city, int count) {
            if (count == 0)
                return;

            var roadNetwork = FindRoadNetwork(city);
            var openPositions = FindOpenPositions(roadNetwork, city);

            for (var i = 0; i < count; i++) {
                if (!openPositions.Any()) {
                    return;
                }

                // TODO: Maybe multiple positions should be evaluated and the best be chosen. Experiment with it.
                var id = _rnd.Next(openPositions.Count);
                var pos = openPositions[id];

                // TODO: This should be buildings chosen by the actor, not the zone.
                city.PlaceBuilding(pos, _zone.GetRandom(_rnd));

                openPositions.RemoveAt(id);
            }
        }

        private IList<Vector2i> FindRoadNetwork(CityMap city) {

            // TODO: This function has an O(n^2) computation power which is likely too slow. Some kind of caching might be useful.
            var list = new List<Vector2i>();

            for (var x = 0; x < city.Width; x++) {
                for (var y = 0; y < city.Width; y++) {
                    if (city.Terrain[x, y].IsRoad()) {
                        list.Add(new Vector2i(x, y));
                    }
                }
            }
            return list;
        }

        private IList<Vector2i> FindOpenPositions(IList<Vector2i> roads, CityMap city) {

            // TODO: This function has an O(n^2) computation power which is likely too slow. Some kind of caching might be useful.
            var list = new List<Vector2i>();

            foreach (var roadPos in roads) {
                var X = roadPos.X;
                var Y = roadPos.Y;

                var xMin = Math.Max(city.Terrain[X - 1, Y].IsRoad() ? X : X - 3, 0);
                var xMax = Math.Min(city.Terrain[X + 1, Y].IsRoad() ? X : X + 3, city.Width - 1);
                var yMin = Math.Max(city.Terrain[X, Y - 1].IsRoad() ? Y : Y - 3, 0);
                var yMax = Math.Min(city.Terrain[X, Y + 1].IsRoad() ? Y : Y + 3, city.Height - 1);
                
                for (var x = xMin; x <= xMax; x++) {
                    for (var y = yMin; y <= yMax; y++) {
                        if (city.Terrain[x, y].ZoneId == _zone.Id && city.Terrain[x, y].Building == null) {
                            list.Add(new Vector2i(x, y));
                        }
                    }
                }
            }
            
            return list.Distinct().ToList();
        }
    }
}

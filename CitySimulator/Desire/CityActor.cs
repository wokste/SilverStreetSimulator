using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CitySimulator.Desire {
    internal class CityActor {

        // TODO: This should be moved into zone, I believe. Also, there should be a list of buildings.

        readonly Random _rnd = new Random();
        readonly ZoneType _zone;

        public CityActor( ZoneType zone) {
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

        private IList<Point> FindRoadNetwork(CityMap city) {

            // TODO: This function has an O(n^2) computation power which is likely too slow. Some kind of caching might be useful.
            var list = new List<Point>();

            for (var x = 0; x < city.SizeX; x++) {
                for (var y = 0; y < city.SizeX; y++) {
                    if (city.IsRoad(x, y)) {
                        list.Add(new Point(x, y));
                    }
                }
            }
            return list;
        }

        private IList<Point> FindOpenPositions(IList<Point> roads, CityMap city) {

            // TODO: This function has an O(n^2) computation power which is likely too slow. Some kind of caching might be useful.
            var list = new List<Point>();

            foreach (var roadPos in roads) {
                var rX = roadPos.X;
                var rY = roadPos.Y;

                var xMin = Math.Max(city.IsRoad(rX - 1, rY) ? rX : rX - 3, 0);
                var xMax = Math.Min(city.IsRoad(rX + 1, rY) ? rX : rX + 3, city.SizeX - 1);
                var yMin = Math.Max(city.IsRoad(rX, rY - 1) ? rY : rY - 3, 0);
                var yMax = Math.Min(city.IsRoad(rX, rY + 1) ? rY : rY + 3, city.SizeY - 1);
                
                for (var x = xMin; x <= xMax; x++) {
                    for (var y = yMin; y <= yMax; y++) {
                        if (city.Terrain[x, y].ZoneId == _zone.Id && city.Terrain[x, y].Building == null) {
                            list.Add(new Point(x, y));
                        }
                    }
                }
            }
            
            return list.Distinct().ToList();
        }
    }
}

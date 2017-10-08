using System;
using System.Collections.Generic;
using OpenTK;

namespace CitySimulator.MeshGeneration {
    class BuildingPlanGenerator
    {
        private Random _rnd;

        internal BuildingPlanGenerator(Random rnd)
        {
            _rnd = rnd;
        }

        internal BuildingPlan Generate(Vector2 center, Vector2 size)
        {
            var floorPlan = new BuildingPlan();
            var polygons = new List<Polygon2D>();

            floorPlan.Polygon = Rect(center, size);
            return floorPlan;
        }

        private Polygon2D Rect(Vector2 center, Vector2 size)
        {
            var halfSize = size / 2;

            var polygon = new Polygon2D
            {
                Corners = new[] { center - halfSize, new Vector2(center.X - halfSize.X, center.Y + halfSize.Y), center + halfSize, new Vector2(center.X + halfSize.X, center.Y - halfSize.Y)}
            };

            return polygon;
        }
    }
}

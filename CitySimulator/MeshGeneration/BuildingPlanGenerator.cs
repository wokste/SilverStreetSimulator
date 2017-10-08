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

            var prob = _rnd.NextDouble();
            if (prob < 0.6)
                floorPlan.Polygon = Rect(center, size);
            else
                floorPlan.Polygon = Poly(center, size, _rnd.Next(3,6));
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

        private Polygon2D Poly(Vector2 center, Vector2 size, int count)
        {
            var halfSize = size / 2;
            var corners = new Vector2[count];
            var offset = _rnd.NextDouble();

            for (var i = 0; i < count; i++)
            {
                var angle = (i + offset) * (2 * Math.PI) / count;

                corners[i] = new Vector2(center.X + halfSize.X * (float)Math.Sin(angle), center.Y + halfSize.Y * (float)Math.Cos(angle));
            }

            var polygon = new Polygon2D
            {
                Corners = corners
            };
            
            return polygon;
        }
    }
}

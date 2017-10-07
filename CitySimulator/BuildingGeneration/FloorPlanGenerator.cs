using OpenTK;

namespace CitySimulator.BuildingGeneration {
    class FloorPlanGenerator {
        internal FloorPlan Generate(Vector2 size)
        {
            var halfSize = size / 2;

            var floorPlan = new FloorPlan();

            var polygon = new Polygon
            {
                Corners = new[] {-halfSize, new Vector2(-halfSize.X, halfSize.Y), halfSize, new Vector2(halfSize.X, -halfSize.Y)}
            };

            floorPlan.Polygons = new[] { polygon };

            return floorPlan;
        }
    }
}

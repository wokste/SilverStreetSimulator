using OpenTK;

namespace CitySimulator.BuildingGeneration {
    class FloorPlanGenerator {
        internal FloorPlan Generate()
        {
            var floorPlan = new FloorPlan();

            var polygon = new Polygon();

            polygon.Corners = new[]
                {new Vector2(0.1f, 0.1f), new Vector2(0.1f, 0.9f), new Vector2(0.9f, 0.9f), new Vector2(0.9f, 0.1f)};

            return floorPlan;
        }
    }
}

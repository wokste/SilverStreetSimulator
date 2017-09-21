using OpenTK;

namespace CitySimulator.BuildingGeneration {
    internal class FloorPlan
    {
        internal Polygon[] Polygons;
    }

    internal struct Polygon {
        internal Vector2[] Corners;
    }
}

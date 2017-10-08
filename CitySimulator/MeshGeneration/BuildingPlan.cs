using OpenTK;

namespace CitySimulator.MeshGeneration {
    internal class BuildingPlan
    {
        internal Polygon2D Polygon;
        internal int NumFloors = 1;
        internal float FloorHeight = 0.3f;
        internal float GroundLevel;
        internal float LowGroundLevel;
        internal float RoofLevel => GroundLevel + NumFloors * FloorHeight;
    }

    internal struct Polygon2D {
        internal Vector2[] Corners;
    }
}

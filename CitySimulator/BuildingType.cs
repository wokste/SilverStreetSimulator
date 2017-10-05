using System.Drawing;
using CitySimulator.BuildingGeneration;
using OpenTK;

namespace CitySimulator {
    class BuildingType {
        internal int Jobs;
        internal int Population;
        internal Vector2 Size = new Vector2(2,2);
        internal float Height;

        internal Mesh GenerateMesh()
        {
            var factory = new Mesh.Factory();

            var floor = new FloorPlanGenerator().Generate(Size);
            var height = Height;

            var wallGenerator = new WallGenerator();
            wallGenerator.CreateWalls(floor, height, factory);

            var roofGenerator = new RoofGenerator();
            roofGenerator.CreateRoof(floor, height, factory);

            return factory.ToMesh();
        }
    }
}

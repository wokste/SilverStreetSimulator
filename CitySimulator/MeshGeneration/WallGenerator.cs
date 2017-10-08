using System;
using CitySimulator.Render;
using OpenTK;

namespace CitySimulator.MeshGeneration
{
    class WallGenerator
    {
        private Random _rnd;

        public WallGenerator(Random rnd)
        {
            _rnd = rnd;
        }

        internal void CreateWalls(Mesh.Factory factory, BuildingPlan plan)
        {
            for (var i = 0; i < plan.Polygon.Corners.Length; i++)
            {
                var c0 = plan.Polygon.Corners[i];
                var c1 = plan.Polygon.Corners[(i + 1) % plan.Polygon.Corners.Length];
                var c0To1 = c1 - c0;

                var v0 = new Mesh.Vertex
                {
                    Pos = new Vector3(c0.X, plan.LowGroundLevel, c0.Y),
                    TexCoords = new Vector2()
                };
                var v0To1 = new Mesh.Vertex
                {
                    Pos = new Vector3(c0To1.X, 0, c0To1.Y),
                    TexCoords = new Vector2(c0To1.Length, 0)
                };
                var v0To2 = new Mesh.Vertex
                {
                    Pos = new Vector3(0, plan.RoofLevel - plan.LowGroundLevel, 0),
                    TexCoords = new Vector2(0, plan.RoofLevel - plan.LowGroundLevel)
                };
                factory.AddSurface(v0, v0To1, v0To2);
            }
        }
    }
}

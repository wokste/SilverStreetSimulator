using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace CitySimulator.BuildingGeneration
{
    class WallGenerator
    {
        internal void CreateWalls(FloorPlan plan, float height, Mesh.Factory factory)
        {
            foreach (var p in plan.Polygons)
            {
                for (var i = 0; i < p.Corners.Length; i++)
                {
                    var c0 = p.Corners[i];
                    var c1 = p.Corners[(i + 1) % p.Corners.Length];
                    var c0To1 = c1 - c0;

                    var v0 = new Mesh.Vertex
                    {
                        Pos = new Vector3(c0.X, 0, c0.Y),
                        TexCoords = new Vector2()
                    };
                    var v0To1 = new Mesh.Vertex
                    {
                        Pos = new Vector3(c0To1.X, 0, c0To1.Y),
                        TexCoords = new Vector2(c0To1.Length, 0)
                    };
                    var v0To2 = new Mesh.Vertex
                    {
                        Pos = new Vector3(0, height, 0),
                        TexCoords = new Vector2(0, height)
                    };
                    factory.AddSurface(v0, v0To1, v0To2);
                }
            }
        }
    }
}

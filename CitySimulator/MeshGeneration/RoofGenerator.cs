using System;
using System.Diagnostics;
using CitySimulator.Render;
using OpenTK;

namespace CitySimulator.MeshGeneration
{
    class RoofGenerator
    {
        private Random _rnd;

        public RoofGenerator(Random rnd)
        {
            _rnd = rnd;
        }

        internal void CreateRoof(Mesh.Factory factory, BuildingPlan plan)
        {
            CreateRoofConvex(factory, plan);
        }

        // Code works only for convex polygons for now.
        private void CreateRoofConvex(Mesh.Factory factory, BuildingPlan plan)
        {
            Debug.Assert(IsConvexPolygon(plan.Polygon));

            var offset = factory.Vertices.Count;

            foreach (var c in plan.Polygon.Corners)
            {
                factory.Vertices.Add(new Mesh.Vertex
                {
                    Pos = new Vector3(c.X, plan.RoofLevel, c.Y),
                    TexCoords = c, // TODO
                    Normal = Vector3.UnitY
                });
            }

            for (var i = 0; i < plan.Polygon.Corners.Length - 2; i++)
            {
                factory.Faces.Add(new Mesh.Face(offset, offset+i+1, offset+i+2));
            }
        }

        /// <summary>
        /// Tests whether the provided polygon is convex and clockwise
        /// </summary>
        /// <param name="polygon2D">A polygon to be tested</param>
        /// <returns>True whether the polygon is convex and clockwise</returns>
        private bool IsConvexPolygon(Polygon2D polygon2D)
        {
            Vector3 To3D(Vector2 v)
            {
                return new Vector3(v.X, 0, v.Y);
            }

            var len = polygon2D.Corners.Length;

            for (var i = 0; i < len; i++)
            {
                var c0 = To3D(polygon2D.Corners[i]);
                var cNext = To3D(polygon2D.Corners[(i + 1) % len]);
                var cPrev = To3D(polygon2D.Corners[(i + len - 1) % len]);
                
                if (Vector3.Cross(cNext - c0, c0 - cPrev).Y > 0)
                    return false;
            }

            return true;
        }
    }
}

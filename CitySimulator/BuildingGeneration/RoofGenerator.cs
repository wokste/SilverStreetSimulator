using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace CitySimulator.BuildingGeneration
{
    class RoofGenerator
    {
        internal void CreateRoof(FloorPlan plan, float height, Mesh.Factory factory)
        {
            foreach (var p in plan.Polygons)
            {
                CreateRoofConvex(p, height, factory);
            }
        }

        // Code works only for convex polygons for now.
        private void CreateRoofConvex(Polygon polygon, float height, Mesh.Factory factory)
        {
            Debug.Assert(IsConvexPolygon(polygon));

            var offset = factory.Vertices.Count;

            foreach (var c in polygon.Corners)
            {
                factory.Vertices.Add(new Mesh.Vertex
                {
                    Pos = new Vector3(c.X, height, c.Y),
                    TexCoords = c // TODO
                });
            }

            for (var i = 0; i < polygon.Corners.Length - 2; i++)
            {
                factory.Faces.Add(new Mesh.Face(offset, offset+i+1, offset+i+2));
            }
        }

        /// <summary>
        /// Tests whether the provided polygon is convex and clockwise
        /// </summary>
        /// <param name="polygon">A polygon to be tested</param>
        /// <returns>True whether the polygon is convex and clockwise</returns>
        private bool IsConvexPolygon(Polygon polygon)
        {
            Vector3 To3D(Vector2 v)
            {
                return new Vector3(v.X, 0, v.Y);
            }

            var len = polygon.Corners.Length;

            for (var i = 0; i < len; i++)
            {
                var c0 = To3D(polygon.Corners[i]);
                var cNext = To3D(polygon.Corners[(i + 1) % len]);
                var cPrev = To3D(polygon.Corners[(i + len - 1) % len]);
                
                if (Vector3.Cross(cNext - c0, c0 - cPrev).Y > 0)
                    return false;
            }

            return true;
        }
    }
}

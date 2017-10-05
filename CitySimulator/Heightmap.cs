using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace CitySimulator {
    class HeightMap
    {
        internal float[,] Height;

        private const float _texScale = 0.1f;

        internal HeightMap(int sizeX, int sizeY)
        {
            Height = new float[sizeX + 1, sizeY + 1];
        }

        internal Mesh ToMesh()
        {
            var factory = new Mesh.Factory();
            
            for (var x = 0; x < Height.GetLength(0); x++) {
                for (var y = 0; y < Height.GetLength(1); y++)
                {
                    factory.Vertices.Add(new Mesh.Vertex {
                        Pos = new Vector3(x, Height[x, y], y),
                        TexCoords = new Vector2(x * _texScale, y * _texScale),
                        Normal = GetNormal(x,y)
                    });
                }
            }

            for (var x = 0; x < Height.GetLength(0) - 1; x++) {
                for (var y = 0; y < Height.GetLength(1) - 1; y++)
                {
                    var v00 = x * Height.GetLength(1) + y;
                    var v01 = v00 + 1;
                    var v10 = v00 + Height.GetLength(1);
                    var v11 = v10 + 1;

                    var lenV00ToV11Sq = (factory.Vertices[v00].Pos - factory.Vertices[v11].Pos).LengthSquared;
                    var lenV01ToV10Sq = (factory.Vertices[v01].Pos - factory.Vertices[v10].Pos).LengthSquared;

                    if (lenV00ToV11Sq < lenV01ToV10Sq)
                    {
                        factory.Faces.Add(new Mesh.Face(v00, v01, v11));
                        factory.Faces.Add(new Mesh.Face(v00, v11, v10));
                    }
                    else
                    {
                        factory.Faces.Add(new Mesh.Face(v00, v01, v10));
                        factory.Faces.Add(new Mesh.Face(v01, v11, v10));
                    }
                }
            }
            
            return factory.ToMesh();
        }

        private Vector3 GetNormal(int x, int y)
        {
            Vector3 delta = new Vector3(0,2,0);

            if (x == 0)
                delta.X = 2 * (Height[x + 1, y] - Height[x, y]);
            else if (x == Height.GetLength(0) - 1)
                delta.X = 2 * (Height[x, y] - Height[x - 1, y]);
            else
                delta.X = Height[x+1, y] - Height[x-1, y];

            if (y == 0)
                delta.Z = 2 * (Height[x, y + 1] - Height[x, y]);
            else if(y == Height.GetLength(1) - 1)
                delta.Z = 2 * (Height[x, y] - Height[x, y-1]);
            else
                delta.Z = Height[x,y + 1] - Height[x,y - 1];

            delta.X *= -1;
            delta.Z *= -1;

            return delta.Normalized();
        }
    }
}

using System;
using System.Security.Cryptography.X509Certificates;
using CitySimulator.Render;
using OpenTK;

namespace CitySimulator {
    class HeightMap
    {
        internal float[,] Height;

        private const float TexScale = 0.1f;
        private readonly int _sizeX;
        private readonly int _sizeY;

        internal HeightMap(int sizeX, int sizeY)
        {
            _sizeX = sizeX;
            _sizeY = sizeY;
            Height = new float[sizeX + 1, sizeY + 1];
        }

        internal Mesh ToMesh()
        {
            var factory = new Mesh.Factory();
            
            for (var x = 0; x <= _sizeX; x++) {
                for (var y = 0; y <= _sizeY; y++)
                {
                    factory.Vertices.Add(new Mesh.Vertex {
                        Pos = new Vector3(x, Height[x, y], y),
                        TexCoords = new Vector2(x * TexScale, y * TexScale),
                        Normal = GetNormal(x,y)
                    });
                }
            }

            for (var x = 0; x < _sizeX; x++) {
                for (var y = 0; y < _sizeY; y++)
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
            var delta = new Vector3(0,2,0);

            if (x == 0)
                delta.X = 2 * (Height[x + 1, y] - Height[x, y]);
            else if (x == _sizeX)
                delta.X = 2 * (Height[x, y] - Height[x - 1, y]);
            else
                delta.X = Height[x+1, y] - Height[x-1, y];

            if (y == 0)
                delta.Z = 2 * (Height[x, y + 1] - Height[x, y]);
            else if(y == _sizeY)
                delta.Z = 2 * (Height[x, y] - Height[x, y-1]);
            else
                delta.Z = Height[x,y + 1] - Height[x,y - 1];

            delta.X *= -1;
            delta.Z *= -1;

            return delta.Normalized();
        }
        
        internal float GetHeight(Vector2 v)
        {
            var x0 = (int) v.X;
            var x1 = (x0 == _sizeX) ? x0 : x0 + 1;
            var y0 = (int) v.Y;
            var y1 = (y0 == _sizeY) ? y0 : y0 + 1;
            var dX = v.X - x0;
            var dY = v.Y - y0;
            
            var hX0 = Height[x0, y0] * (1-dY) + Height[x0, y1] * dY;
            var hX1 = Height[x1, y0] * (1 - dY) + Height[x1, y1] * dY;
            var h = hX0 * (1 - dX) + hX1 * dX;

            return h;
        }
    }
}

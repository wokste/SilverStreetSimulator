using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace CitySimulator {
    class Mesh : IDisposable {
        private readonly uint _indexBufferId;
        private readonly uint _vertexBufferId;

        private readonly int _numIndices;

        internal Mesh(ushort[] indices, float[] vertexData) {
            GL.GenBuffers(1, out _indexBufferId);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _indexBufferId);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indices.Length * sizeof(ushort)), indices, BufferUsageHint.StaticDraw);

            GL.GenBuffers(1, out _vertexBufferId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferId);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexData.Length * sizeof(float)), vertexData, BufferUsageHint.StaticDraw);

            _numIndices = indices.Length;


        }

        internal void Render() {
            // Bind index buffer:
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _indexBufferId);

            // Bind vertex buffer:
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferId);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.TextureCoordArray);
            GL.EnableClientState(ArrayCap.NormalArray);

            GL.VertexPointer(3, VertexPointerType.Float, 8 * sizeof(float), IntPtr.Zero);
            GL.NormalPointer(NormalPointerType.Float, 8 * sizeof(float), IntPtr.Zero + 3 * sizeof(float));
            GL.TexCoordPointer(2,TexCoordPointerType.Float, 8 * sizeof(float), IntPtr.Zero + 6 * sizeof(float));

            // Draw:            
            GL.DrawElements(PrimitiveType.Triangles, _numIndices, DrawElementsType.UnsignedShort, IntPtr.Zero);

            // Disable:
            GL.DisableClientState(ArrayCap.TextureCoordArray);
            GL.DisableClientState(ArrayCap.VertexArray);
            GL.DisableClientState(ArrayCap.NormalArray);
        }
        
        public void Dispose() {
            GL.DeleteBuffers(2, new []{ _indexBufferId , _vertexBufferId});
        }

        internal struct Face {
            internal int V0, V1, V2;

            public Face(int v0, int v1, int v2) : this() {
                V0 = v0;
                V1 = v1;
                V2 = v2;
            }
        }
        
        internal struct Vertex
        {
            // TODO, normal
            internal Vector3 Pos;
            internal Vector2 TexCoords;
            internal Vector3 Normal;

            /// <summary>
            /// The sum of the two vectors.
            /// Warning: the normal is added which is probably not what you want.
            /// </summary>
            public static Vertex operator+(Vertex l, Vertex r)
            {
                return new Vertex
                {
                    Pos = l.Pos + r.Pos,
                    TexCoords = l.TexCoords + r.TexCoords,
                    Normal = (l.Normal + r.Normal).Normalized()
                };
            }
        }

        internal class Factory {
            internal List<Vertex> Vertices = new List<Vertex>();
            internal List<Face> Faces = new List<Face>();

            /// <summary>
            /// This function validates whether the mesh is well-formed. This is mainly to test for errors in other functions.
            /// </summary>
            [System.Diagnostics.Conditional("DEBUG")]
            internal void Validate()
            {
                bool InRange(int id) => id >= 0 && id < Vertices.Count;

                foreach (var f in Faces)
                {
                    if (!InRange(f.V0) || !InRange(f.V1) || !InRange(f.V2)) {
                        throw new ArgumentOutOfRangeException();
                    }

                    if (f.V0 == f.V1 || f.V1 == f.V2 || f.V2 == f.V0)
                    {
                        throw new Exception("Mesh has two vertices that are the same");
                    }
                }
            }

            internal float[] VerticesPrimitives => Vertices.SelectMany(v => new[] { v.Pos.X, v.Pos.Y, v.Pos.Z, v.Normal.X, v.Normal.Y, v.Normal.Z, v.TexCoords.X, v.TexCoords.Y }).ToArray();
            internal ushort[] FacesPrimitives => Faces.SelectMany(f => new[] {(ushort)f.V0, (ushort)f.V1, (ushort)f.V2}).ToArray();

            internal Mesh ToMesh()
            {
                Validate();
                return new Mesh(FacesPrimitives, VerticesPrimitives); 
            }

            internal void AddSurface(Vertex v0, Vertex v0To1, Vertex v0To2)
            {
                v0.Normal = Vector3.Cross(v0To1.Pos, v0To2.Pos);
                v0To1.Normal = Vector3.Zero;
                v0To2.Normal = Vector3.Zero;
                var v = new[] {v0, v0 + v0To1, v0 + v0To1 + v0To2, v0 + v0To2};

                var pos = Vertices.Count;

                Vertices.AddRange(v);

                Faces.Add(new Face(pos, pos + 1, pos + 2));
                Faces.Add(new Face(pos + 2, pos + 3, pos));
            }
        }
    }
}

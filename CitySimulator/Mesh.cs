using System;
using System.Collections.Generic;
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
            //GL.EnableClientState(ArrayCap.TextureCoordArray);

            GL.VertexPointer(3, VertexPointerType.Float, 0, IntPtr.Zero);
            //GL.TexCoordPointer(5,TexCoordPointerType.Float, 2, IntPtr.Zero);

            // Draw:            
            GL.DrawElements(PrimitiveType.Triangles, _numIndices, DrawElementsType.UnsignedShort, IntPtr.Zero);

            // Disable:
            //GL.DisableClientState(ArrayCap.TextureCoordArray);
            GL.DisableClientState(ArrayCap.VertexArray);
        }
        
        public void Dispose() {
            throw new NotImplementedException();
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

            internal float[] VerticesPrimitives => Vertices.SelectMany(v => new[] { v.Pos.X, v.Pos.Y, v.Pos.Z/*, v.TexCoords.X, v.TexCoords.Y*/ }).ToArray();
            internal ushort[] FacesPrimitives => Faces.SelectMany(f => new[] {(ushort)f.V0, (ushort)f.V1, (ushort)f.V2}).ToArray();

            internal Mesh ToMesh()
            {
                Validate();
                return new Mesh(FacesPrimitives, VerticesPrimitives); 
            }
        }
    }
}

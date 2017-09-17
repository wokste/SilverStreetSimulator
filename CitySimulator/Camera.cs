using System;
using OpenTK;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace CitySimulator {
    class Camera {
        private Vector3 _focus = new Vector3(0, 0, 0);
        private readonly Vector3 _dir = new Vector3(-1, -1, -0.70710678118f);
        private float _rotZ = (float)(Math.PI / 4) * -1f;
        private readonly float _rotX = 0.7654f;//1.3f;

        internal float Zoom = 32;

        internal Size ScreenSize;

        private Matrix4 ModelView
        {
            get
            {
                var quatZ = Quaternion.FromAxisAngle(Vector3.UnitZ, _rotZ);
                var quatX = Quaternion.FromAxisAngle(Vector3.UnitX, _rotX);
                var dir = quatZ * quatX * Vector3.UnitY;

                Console.WriteLine($"Dir: {dir} should be {_dir}");

                var modelView = Matrix4.LookAt(_focus - dir, _focus, Vector3.UnitZ);
                return modelView;
            }
        }

        internal void SetMatrices()
        {
            var modelview = ModelView;
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);

            var matrix = Matrix4.CreateOrthographic(ScreenSize.Width / Zoom, ScreenSize.Height / Zoom, -500.0f, 500.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref matrix);
        }

        internal void MoveFocus(Vector3 mouseDrag3D)
        {
            _focus += mouseDrag3D;
        }

        internal void MoveRotate(Vector2 rotation)
        {
            //_rotX = MathHelper.Clamp(_rotX + rotation.X, 0.3f, 1.3f); 
            _rotZ = _rotZ + rotation.Y;
        }
        
        internal Vector3 ScreenSpaceToWorldSpace(Point screenSpace, object heightMap, bool translate)
        {
            screenSpace.X -= ScreenSize.Width / 2;
            screenSpace.Y -= ScreenSize.Height / 2;

            // Determine line l = a + lambda b which represents the screen coordinate in 3D
            var screenSpace4D = new Vector4(screenSpace.X / Zoom, -screenSpace.Y / Zoom, 0, (translate) ? 1 : 0 );
            var modelViewInverted = ModelView.Inverted();

            var a4 = screenSpace4D * modelViewInverted;//new Vector4(screenSpace.X / Zoom, -screenSpace.Y / Zoom, 0, 0) * ModelView.Inverted();
            var a = a4.Xyz;
            var b = _dir;

            // Find the lambda for which this line intersects the z plane.
            var height = 0;
            var lambda = (height - a.Z) / b.Z;

            // Determine the point for this line with the determined lambda.
            var point = a + lambda * b;

            return point;
        }
    }
}

using OpenTK;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace CitySimulator {
    class Camera {
        private Vector3 _focus = new Vector3(0, 0, 0);
        private Vector3 _dir = new Vector3(-1, -1, -0.70710678118f);
        private float pitch = 45;//(float)(Math.PI / 4);
        private float yaw = 45;//(float)(Math.PI / 4);

        internal float Zoom = 32;

        internal Size ScreenSize;

        private Matrix4 ModelView
        {
            get
            {
                var dirQuad = Quaternion.FromEulerAngles(pitch, yaw, 0);
                var dir = dirQuad * Vector3.UnitZ;
                var modelView = Matrix4.LookAt(_focus - _dir, _focus, Vector3.UnitZ);
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

        internal void MoveRotate(Vector2 rotation) {

        }

        internal Vector3 ScreenSpaceToWorldSpace(Point screenSpace)
        {
            screenSpace.X -= ScreenSize.Width / 2;
            screenSpace.Y -= ScreenSize.Height / 2;

            // Determine line l = a + lambda b which represents the screen coordinate in 3D
            var a4 = new Vector4(screenSpace.X / Zoom, -screenSpace.Y / Zoom, 0, 0) * ModelView.Inverted();
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

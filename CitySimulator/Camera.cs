using System;
using System.Diagnostics;
using OpenTK;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace CitySimulator {
    class Camera {
        private Vector3 _focus = new Vector3(64, 0, 64);
        private readonly Vector3 _dir = new Vector3(0, -1 , 0).Normalized();
        private float _rotZ = (float)(Math.PI / 4) * 1f;
        private readonly float _rotX = (float)(Math.PI / 4) * -3f;

        internal float Zoom = 4;

        internal Size ScreenSize;

        private Matrix4 View
        {
            get
            {
                var quatZ = Quaternion.FromAxisAngle(Vector3.UnitY, _rotZ);
                var quatX = Quaternion.FromAxisAngle(Vector3.UnitX, _rotX);
                var dir = quatZ * quatX * Vector3.UnitY;

                //var modelView = Matrix4.LookAt(_focus - _dir, _focus, Vector3.UnitX); // Should be unitY
                var modelView = Matrix4.LookAt(_focus - dir, _focus, Vector3.UnitY);
                return modelView;
            }
        }

        private Matrix4 Projection
        {
            get
            {
                return Matrix4.CreateOrthographic(ScreenSize.Width / Zoom, ScreenSize.Height / Zoom, -500.0f, 500.0f);
            }
        }

        internal void SetMatrices()
        {
            var projection = Projection;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
            
            var modelview = View;
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);
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
        
        internal Vector3 ViewportSpaceToWorldSpace(Point mouse, object heightMap, bool translate)
        {
            // Following the tutorial of http://antongerdelan.net/opengl/raycasting.html

            var aNds = new Vector2 ((2.0f * mouse.X) / ScreenSize.Width - 1.0f, 1.0f - (2.0f * mouse.Y) / ScreenSize.Height);

            var aClip = new Vector4(aNds.X, aNds.Y, -1f, 1f); // translate?

            Debug.Assert(Math.Abs(aClip.X) <= 1 && Math.Abs(aClip.Y) <= 1 && Math.Abs(aClip.Z) <= 1 && Math.Abs(aClip.W) <= 1);

            var aEye = Projection.Inverted() * aClip;


            Debug.Assert(Math.Abs(aEye.W - 1) <= 0.001);

            aEye.Z = 0;
            //aEye.W = 0;

            var aWorld = View.Inverted() * aEye;
            var viewInv = View.Inverted();

            //Debug.Assert(Math.Abs(aWorld.W - 1) <= 0.001);

            var a = aWorld;
            var b = new Vector4(_dir, 0);


            Console.WriteLine($"I = a ({a}) + labda b({b})");

            return a.Xyz;
/*
            var normDevSpace4D = new Vector3(mouse.X - ScreenSize.Width / 2, mouse.Y - ScreenSize.Height / 2, 0) / Zoom;

            // Determine line l = a + lambda b which represents the screen coordinate in 3D
            var screenSpace4D = new Vector4(mouse.X / Zoom, -mouse.Y / Zoom, 0, (translate) ? 1 : 0 );
            var modelViewInverted = View.Inverted();

            var a4 = screenSpace4D * modelViewInverted;//new Vector4(mouse.X / Zoom, -mouse.Y / Zoom, 0, 0) * View.Inverted();
            var a = a4.Xyz;*/
            

            // Find the lambda for which this line intersects the y plane.
            var height = 0;
            var lambda = (height - a.Y) / b.Y;

            // Determine the point for this line with the determined lambda.
            var point = a + lambda * b;

            return point.Xyz;
        }
    }
}

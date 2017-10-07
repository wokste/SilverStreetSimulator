using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType;

namespace CitySimulator.Render
{
    class Light
    {
        private readonly LightName _id;
        internal float[] Position = { 0f, 0f, 0f, 0f };
        internal float[] Ambient = { 0.25f, 0.25f, 0.25f, 1.0f };
        internal float[] Colour = {1f, 0.5f, 0.5f, 1f};

        public Light(LightName id)
        {
            _id = id;

            GL.Enable(EnableCap.Lighting);
            GL.Enable((EnableCap)_id);
        }

        public void Update() {
            GL.Light(_id, LightParameter.Position, Position);
            GL.Light(_id, LightParameter.Ambient, Ambient);
            GL.Light(_id, LightParameter.Diffuse , Colour);
            GL.Light(_id, LightParameter.Specular, Colour);

            //GL.Light(_id, LightParameter.Colour, mat_specular);

            GL.Disable(EnableCap.Lighting);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(Position[0] * 128, Position[1] * 128, Position[2] * 128);

            GL.End();
            GL.Enable(EnableCap.Lighting);
        }
    }

    class Sun : Light
    {
        /// <summary>
        /// The time it takes the sun/moon to make a full 1-day cycle.
        /// </summary>
        private readonly float _cycleTime = 900f;

        private float _time;

        public Sun(LightName id) : base(id)
        {
        }

        public void AddTime()
        {
            _time += 1.0f / _cycleTime / (float) Math.PI / 2;

            // Quick fix for skipping nights. Should be removed when the night sky looks interesting as well.
            if (_time > 1.7)
                _time = -1.7f;

            SetPosition();
            SetColour();
        }

        private void SetPosition() {
            var v1 = new Vector3(1,0,0);
            var v2 = new Vector3(0, 0.7071f, 0.7071f);

            var pos = v1 * (float) Math.Sin(_time) + v2 * (float) Math.Cos(_time);

            Position[0] = pos.X;
            Position[1] = pos.Y;
            Position[2] = pos.Z;

            Update();
        }

        private void SetColour()
        {
            var height = (float)Math.Cos(_time);

            var c0 = new Vector3(0,0,0);
            var c02 = new Vector3(0.8f, 0.5f, 0.3f);
            var c06 = new Vector3(1, 1f, 1f);
            
            Vector3 c;

            if (height <= 0)
                c = c0;
            else if (height < 0.2)
                c = Vector3.Lerp(c0, c02, height / 0.2f);
            else if (height < 0.6)
                c = Vector3.Lerp(c02, c06, (height - 0.2f) / 0.4f);
            else
                c = c06;

            Colour[0] = c.X;
            Colour[1] = c.Y;
            Colour[2] = c.Z;

            Update();
        }
    }

    class Material
    {
        private readonly float[] _specular = { 1.0f, 1.0f, 1.0f, 1.0f };
        private readonly float[] _shininess = { 50.0f };

        public Material()
        {
        }

        public void Update()
        {
            GL.Material(MaterialFace.Front, MaterialParameter.Specular, _specular);
            GL.Material(MaterialFace.Front, MaterialParameter.Shininess, _shininess);
        }
    }
}

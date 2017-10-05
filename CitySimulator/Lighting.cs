using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace CitySimulator
{
    class Light
    {
        private readonly LightName _id;
        private float[] _position = { 10f, 25f, 10.0f, 0.0f };
        private float[] _ambient = { 0.5f, 0.5f, 0.5f, 1.0f };

        public Light(LightName id)
        {
            _id = id;

            GL.Enable(EnableCap.Lighting);
            GL.Enable((EnableCap)_id);
        }

        public void Update() {
            GL.Light(_id, LightParameter.Position, _position);
            GL.Light(_id, LightParameter.Ambient, _ambient);

            //GL.Light(_id, LightParameter.Diffuse, mat_specular);
        }
    }

    class Material
    {
        private float[] _specular = { 1.0f, 1.0f, 1.0f, 1.0f };
        private float[] _shininess = { 50.0f };

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

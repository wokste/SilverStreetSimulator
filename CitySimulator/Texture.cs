using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace CitySimulator {
    class Texture
    {
        internal readonly int TexId;
        internal readonly float Width;
        internal readonly float Height;

        internal Texture(string filename) {
            var image = new Bitmap($"{Program.AssetsFolder}{filename}"); 

            TexId = GL.GenTexture();
            Width = image.Width;
            Height = image.Height;

            GL.BindTexture(TextureTarget.Texture2D, TexId);
            var bitmapData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);

            image.UnlockBits(bitmapData);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Nearest);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Nearest);
        }


        public void Render2D(Point vecIso, Rectangle textureRect, Vector2 scale)
        {
            var v0 = new Vector3(vecIso.X, vecIso.Y, 3.0f);
            var vHor = Vector3.UnitX * textureRect.Width * scale.X;
            var vVert = Vector3.UnitY * textureRect.Height * scale.Y;

            var t0 = new Vector2(textureRect.Left / Width, textureRect.Top / Height);
            var tHor = Vector2.UnitX * textureRect.Width / Width;
            var tVert = Vector2.UnitY * textureRect.Height / Height;

            Bind();

            GL.Begin(PrimitiveType.Quads);

            GL.TexCoord2(t0);
            GL.Vertex3(v0);
            GL.TexCoord2(t0 + tHor);
            GL.Vertex3(v0 + vHor);
            GL.TexCoord2(t0 + tHor + tVert);
            GL.Vertex3(v0 + vHor + vVert);
            GL.TexCoord2(t0 + tVert);
            GL.Vertex3(v0 + vVert);
            
            GL.End();
        }

        public void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2D, TexId);
        }
    }
}

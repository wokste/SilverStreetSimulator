using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;

namespace CitySimulator.Render
{
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
        }

        public void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2D, TexId);
        }
    }
}

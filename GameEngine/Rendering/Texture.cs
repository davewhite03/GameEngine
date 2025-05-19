using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;
using System.Drawing;
using System.Drawing.Imaging;

namespace GameEngine.Rendering
{
    public class Texture : IDisposable
    {
        public int Handle { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        // Load texture from file path
        public Texture(string path)
        {
            Handle = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, Handle);

            using (var image = new Bitmap(path))
            {
                
                Width = image.Width;
                Height = image.Height;

               
                var data = image.LockBits(
                    new Rectangle(0, 0, image.Width, image.Height),
                    ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                // Upload to OpenGL
                GL.TexImage2D(
                    TextureTarget.Texture2D,
                    0,                           
                    PixelInternalFormat.Rgba,    
                    image.Width,
                    image.Height,
                    0,                           
                    OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, 
                    PixelType.UnsignedByte,      
                    data.Scan0);                 

                image.UnlockBits(data);
            }

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void Use(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }

       
        public void Dispose()
        {
            GL.DeleteTexture(Handle);
        }


    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

using OpenTK.Graphics.OpenGL;

using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace engine.graphics
{
    public static class TextureLibrary
    {
        private static readonly Dictionary<string, int> Textures = new Dictionary<string, int>();

        private static bool HasNamedTexture(string textureName)
        {
            return Textures.ContainsKey(textureName);
        }

        public static bool HasTexture(string textureName)
        {
            int val;
            return int.TryParse(textureName, out val) || HasNamedTexture(textureName);
        }

        public static void RegisterTexture(string name, int id)
        {
            Textures[name] = id;
        }

        public static int GetTexture(string textureName)
        {
            int val;
            return int.TryParse(textureName, out val) ? val : HasNamedTexture(textureName) ? Textures[textureName] : -1;
        }

        public static void LoadTextureFromFile(string filename)
        {
            var path = @"..\..\Textures\" + filename;
            if (!File.Exists(path))
                throw new Exception("missing texture file : " + path);
            if (HasTexture(filename))
                return;

            var textureId = GL.GenTexture();
            RegisterTexture(filename, textureId);

            //make a bitmap out of the file on the disk
            var textureBitmap = new Bitmap(path);

            //get the data out of the bitmap
            var textureData =
                textureBitmap.LockBits(
                    new Rectangle(0, 0, textureBitmap.Width, textureBitmap.Height),
                    ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb
                    );

            //Code to get the data to the OpenGL Driver

            //tell OpenGL that this is a 2D texture
            GL.BindTexture(TextureTarget.Texture2D, textureId);

            //the following code sets certian parameters for the texture
            GL.TexEnv(
                TextureEnvTarget.TextureEnv,
                TextureEnvParameter.TextureEnvMode, (float) TextureEnvMode.Modulate);
            GL.TexParameter(
                TextureTarget.Texture2D,
                TextureParameterName.TextureMinFilter, (float) TextureMinFilter.LinearMipmapLinear);
            GL.TexParameter(
                TextureTarget.Texture2D,
                TextureParameterName.TextureMagFilter, (float) TextureMagFilter.Linear);

            // tell OpenGL to build mipmaps out of the bitmap data
            GL.TexParameter(
                TextureTarget.Texture2D,
                TextureParameterName.GenerateMipmap, 1.0f);

            // load the texture
            GL.TexImage2D(
                TextureTarget.Texture2D,
                0, // level
                PixelInternalFormat.Four,
                textureBitmap.Width, textureBitmap.Height,
                0, // border
                PixelFormat.Bgra,
                PixelType.UnsignedByte,
                textureData.Scan0
                );

            //free the bitmap data (we dont need it anymore because it has been passed to the OpenGL driver
            textureBitmap.UnlockBits(textureData);
        }
    }
}

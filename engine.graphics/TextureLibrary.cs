using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using OpenTK.Graphics.OpenGL;

using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace engine.graphics
{
    public static class TextureLibrary
    {
        public static readonly Dictionary<string, int> Textures = new Dictionary<string, int>();

        public static bool HasTexture(string textureName)
        {
            int val;
            return (int.TryParse(textureName, out val) && Textures.ContainsValue(val)) || Textures.ContainsKey(textureName);
        }

        public static void RegisterTexture(string name, int id)
        {
            Textures[name] = id;
        }

        public static int GetTexture(string textureName)
        {
            int val;
            return int.TryParse(textureName, out val) ? val : HasTexture(textureName) ? Textures[textureName] : -1;
        }

        public static void LoadTextureFromFile(string filename)
        {
            if (!File.Exists(@"..\..\Textures\" + filename))
            {
                throw new Exception("missing texture file : " + @"..\..\Textures\" + filename);
            }
            if (HasTexture(filename))
                throw new Exception("file already loaded");

            int textureId;
            //generate one texture and put its ID number into the "Texture" variable
            GL.GenTextures(1, out textureId);
            RegisterTexture(filename, textureId);

            //make a bitmap out of the file on the disk
            var textureBitmap = new Bitmap(@"..\..\Textures\" + filename);

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

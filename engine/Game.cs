using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using engine.entities;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace engine
{
    public class Game : GameWindow
    {
        public Game(GameWorld world)
        {
            World = world;
        }

        public GameWorld World { get; set; }
        public Matrix4 Projection { get; set; }

        public int VertexShader;
        public int FragmentShader;
        public int Program;
        public int VertexAttribProjection;
        public int VertexAttribModelView;
        public int VertexAttribInverseTranspose;
        public int VertexAttribMVP;
        public int TextureId;


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Projection = Matrix4.CreatePerspectiveFieldOfView((float)(Math.PI / 4.0f), Width / (float)Height, 0.01f, 100000f);
            Title = "Game Engine Mk 0.0.1";

            // general
            GL.Enable(EnableCap.AlphaTest);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            // vbo
            GL.Enable(EnableCap.VertexArray);
            GL.Enable(EnableCap.IndexArray);

            LoadShaders();
            LoadTextureFromFile("smiley.png");
            GL.ClearColor(Color4.Black);

            ClientSize = new Size(1600, 900);
            Location = new Point((1920 - ClientSize.Width) / 2, (1080 - ClientSize.Height) / 2);

            World.OnLoad();
        }

        public void LoadShaders()
        {
            Program = GL.CreateProgram();

            LoadShaderFromFile("vs.sh", ShaderType.VertexShader, Program, out VertexShader);
            LoadShaderFromFile("fs.sh", ShaderType.FragmentShader, Program, out FragmentShader);

            GL.LinkProgram(Program);
            //debug, can remove
            Console.WriteLine(GL.GetProgramInfoLog(Program));
            /////

            //VertexAttribProjection = GL.GetAttribLocation(Program, "projection");
            //VertexAttribModelView = GL.GetUniformLocation(Program, "modelview");
            //VertexAttribMVP = GL.GetUniformLocation(Program, "MVP");

            //if (VertexAttribPosition == -1 || VertexAttribProjection == -1 || VertexAttribModelView == -1)
            //{
            //    Console.WriteLine("Error binding attributes. VPosition:" + VertexAttribPosition + " VColor" + VertexAttribProjection + " MView:" + VertexAttribModelView);
            // }

            GL.UseProgram(Program);
        }

        
        public void LoadShaderFromFile(string filename, ShaderType type, int program, out int address)
        {
            address = GL.CreateShader(type);
            
            using (var sr = new StreamReader(Environment.CurrentDirectory + @"\..\..\Shaders\" + filename))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
            }
            GL.CompileShader(address);
            GL.AttachShader(program, address);
            //debug, can remove
            Console.WriteLine(GL.GetShaderInfoLog(address));
            ////////
        }

        public void LoadTextureFromFile(string filename)
        {
            if (!File.Exists(@"..\..\Textures\" + filename))
            {
                throw new Exception("missing texture file : " + @"..\..\Textures\" + filename);
            }

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

            //generate one texture and put its ID number into the "Texture" variable
            GL.GenTextures(1, out TextureId);
            //tell OpenGL that this is a 2D texture
            GL.BindTexture(TextureTarget.Texture2D, TextureId);

            //the following code sets certian parameters for the texture
            GL.TexEnv(TextureEnvTarget.TextureEnv,
                   TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Modulate);
            GL.TexParameter(TextureTarget.Texture2D,
                   TextureParameterName.TextureMinFilter, (float)TextureMinFilter.LinearMipmapLinear);
            GL.TexParameter(TextureTarget.Texture2D,
                   TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);

            // tell OpenGL to build mipmaps out of the bitmap data
            GL.TexParameter(TextureTarget.Texture2D,
                   TextureParameterName.GenerateMipmap, (float)1.0f);

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

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            var keys = OpenTK.Input.Keyboard.GetState();
            var mouse = OpenTK.Input.Mouse.GetState();

            if (keys[Key.Escape])
                Close();

            World.OnUpdate(keys, mouse);
            OpenTK.Input.Mouse.SetPosition(X + Width / 2f + 7, Y + Height / 2f + 31);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //var pvMatrix = World.Camera.LookAt()*Projection;
            int loc = GL.GetUniformLocation(Program, "Tex1");
            GL.Uniform1(loc, TextureId);
            //foreach (var entity in World.Entities)
            //{
            //    var MVP =  entity.GetModelMatrix()*pvMatrix;
            //    GL.UniformMatrix4(VertexAttribMVP, false, ref MVP);
            //}

            World.OnRender();

            SwapBuffers();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle);

            var projection = Matrix4.CreatePerspectiveFieldOfView((float) (Math.PI / 4.0f), Width / (float) Height, 0.01f, 100000f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
            Projection = projection;
        }
    }
}

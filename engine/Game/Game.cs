using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Serialization.Formatters;
using engine.entities;
using engine.Libraries;
using engine.Libraries;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace engine.Game
{
    public class Game : GameWindow
    {
        public Game(GameWorld world)
        {
            World = world;
        }

        public GameWorld World { get; set; }
        public Matrix4 Projection { get; set; }

        public Vector3 LightPos { get; set; }

        public int UniformMvp;
        public int UniformLightPos;
        public int UniformAmbientColor;
        public int UniformModel;
        public int UniformView;
        public int UniformModelRotation;


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Projection = Matrix4.CreatePerspectiveFieldOfView((float)(Math.PI / 4.0f), Width / (float)Height, 0.01f, 100000f);
            Title = "Game Engine Mk 0.0.2";

            // general
            GL.Enable(EnableCap.AlphaTest);
            GL.Enable(EnableCap.DepthTest);

            // vbo
            GL.Enable(EnableCap.VertexArray);
            GL.Enable(EnableCap.IndexArray);

            var v = new List<int>();
            ShaderLibrary.LoadShaders("prog1", new [] {"vertexshader1.sh", "fragmentshader.sh"} , out v );
            TextureLibrary.LoadTextureFromFile("smiley.png");

            UniformMvp = GL.GetUniformLocation(ProgramLibrary.GetProgram("prog1"), "MVP");
            UniformLightPos = GL.GetUniformLocation(ProgramLibrary.GetProgram("prog1"), "LightPos");
            UniformAmbientColor = GL.GetUniformLocation(ProgramLibrary.GetProgram("prog1"), "AmbientLightColor");
            UniformModel = GL.GetUniformLocation(ProgramLibrary.GetProgram("prog1"), "Model");
            UniformView = GL.GetUniformLocation(ProgramLibrary.GetProgram("prog1"), "View");
            UniformModelRotation = GL.GetUniformLocation(ProgramLibrary.GetProgram("prog1"), "ModelRotation");
            GL.ClearColor(Color4.Black);

            ClientSize = new Size(1600, 900);
            Location = new Point((1920 - ClientSize.Width) / 2, (1080 - ClientSize.Height) / 2);

            World.OnLoad();

            //((VboEntity)World.Entities[1]).TextureId = TextureLibrary.GetTexture("smiley.png");
            ((VboEntity)World.Entities[0]).TextureId = TextureLibrary.GetTexture("smiley.png");
            GL.UseProgram(ProgramLibrary.GetProgram("prog1"));

            LightPos = new Vector3(0.0f, 4.0f, 0.0f);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            var keys = OpenTK.Input.Keyboard.GetState();
            var mouse = OpenTK.Input.Mouse.GetState();

            if (keys[Key.Escape])
                Close();
            if (keys[Key.Y])
                LightPos += Vector3.UnitY;
            if (keys[Key.H])
                LightPos -= Vector3.UnitY;
            if (keys[Key.T])
                World.Entities[0].Position += Vector3.UnitX;
            if (keys[Key.U])
                World.Entities[0].Position -= Vector3.UnitX;

            World.OnUpdate(keys, mouse);
            OpenTK.Input.Mouse.SetPosition(X + Width / 2f + 7, Y + Height / 2f + 31);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            var view = World.Camera.LookAt();
            var vM = view * Projection;
            var light = LightPos;
            var ambientColor = new Vector3(1,1,1);
            GL.Uniform3(UniformLightPos, ref light);
            GL.Uniform3(UniformAmbientColor, ref ambientColor);
            GL.UniformMatrix4(UniformView, false, ref view);
            
            foreach (var entity in World.Entities)
            {
                ((VboEntity)entity).Vp = vM;
                ((VboEntity) entity).Mvp = UniformMvp;
                ((VboEntity)entity).Model = UniformModel;
                ((VboEntity)entity).ModelRotation = UniformModelRotation;
            }
            
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

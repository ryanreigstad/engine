using System;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace engine
{
    public class Game : GameWindow
    {
        public Game(GameWorld world)
        {
            World = world;
        }

        public GameWorld World { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Title = "Game Engine Mk 0.0.1";

            // general
            GL.Enable(EnableCap.AlphaTest);
            GL.Enable(EnableCap.DepthTest);

            // vbo
            GL.Enable(EnableCap.VertexArray);
            GL.Enable(EnableCap.IndexArray);

            // lighting
            GL.Light(LightName.Light0, LightParameter.Position, new float[] { 0.0f, 100.0f, 0.0f });
            GL.Light(LightName.Light0, LightParameter.Ambient, new float[] { 0.1f, 0.1f, 0.1f, 1.0f });
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.Enable(EnableCap.ColorMaterial);

            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            GL.ClearColor(Color4.Black);

            ClientSize = new Size(1600, 900);
            Location = new Point((1920 - ClientSize.Width) / 2, (1080 - ClientSize.Height) / 2);

            World.OnLoad();
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

            GL.MatrixMode(MatrixMode.Modelview);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.LoadIdentity();
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
        }
    }
}

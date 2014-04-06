using System;
using System.Drawing;

using engine.graphics;
using engine.world;

using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace engine.testificate
{
    public class Game : GameWindow
    {
        public Game(World world, Renderer renderer)
        {
            World = world;
            Renderer = renderer;
        }

        public World World { get; set; }
        public Renderer Renderer { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Title = "Game Engine Mk 0.0.0.2";

            // TODO: make this configurable
            ClientSize = new Size(1600, 900);
            Location = new Point((1920 - ClientSize.Width) / 2, (1080 - ClientSize.Height) / 2);

            Renderer.OnLoad();
            World.OnLoad();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(ClientRectangle);
            Renderer.OnResize(Width, Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            KeyboardState keys = OpenTK.Input.Keyboard.GetState();
            MouseState mouse = OpenTK.Input.Mouse.GetState();

            if (keys[Key.Escape])
                Close();

            World.OnUpdate(keys, mouse);
            //OpenTK.Input.Mouse.SetPosition(X + Width / 2f + 7, Y + Height / 2f + 31);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            Renderer.OnRender(World);
            SwapBuffers();
        }
    }
}

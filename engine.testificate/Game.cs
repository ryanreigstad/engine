using System;
using System.Drawing;
using engine.core;
using engine.core.rendering;
using engine.data;
using engine.data.config;
using engine.io.config;

using OpenTK;
using OpenTK.Graphics.OpenGL4;

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
            SingletonConfigManager.LoadAll();

            Title = WindowConfig.WindowTitle;

            ClientSize = new Size(WindowConfig.ScreenSize.X, WindowConfig.ScreenSize.Y);
            var resolution = new Vector2i(DisplayDevice.Default.Width, DisplayDevice.Default.Height);
            Location = new Point((resolution.X - ClientSize.Width) / 2, (resolution.Y - ClientSize.Height) / 2);

            Renderer.OnLoad();
            World.OnLoad();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(ClientRectangle);
            Renderer.OnResize(Width, Height);
            WindowConfig.ScreenSize = new Vector2i(Width, Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            var keys = OpenTK.Input.Keyboard.GetState();
            var mouse = OpenTK.Input.Mouse.GetState();

            if (!keys[InputConfig.Quit])
                World.OnUpdate(keys, mouse);
            else
            {
                SingletonConfigManager.SaveAll();
                Close();
            }
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

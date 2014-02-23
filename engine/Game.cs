using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using engine.entities;
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
        public Matrix4 Projection { get; set; }

        public int VertexShader;
        public int FragmentShader;
        public int Program;
        public int VertexAttribProjection;
        public int VertexAttribModelView;
        public int VertexAttribInverseTranspose;
        public int VertexAttribMVP;


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Projection = Matrix4.CreatePerspectiveFieldOfView((float)(Math.PI / 4.0f), Width / (float)Height, 0.01f, 100000f);
            Title = "Game Engine Mk 0.0.1";

            // general
            GL.Enable(EnableCap.AlphaTest);
            GL.Enable(EnableCap.DepthTest);

            // vbo
            GL.Enable(EnableCap.VertexArray);
            GL.Enable(EnableCap.IndexArray);

            LoadShaders();

            GL.ClearColor(Color4.Black);

            ClientSize = new Size(1600, 900);
            Location = new Point((1920 - ClientSize.Width) / 2, (1080 - ClientSize.Height) / 2);

            World.OnLoad();
        }

        public void LoadShaders()
        {
            Program = GL.CreateProgram();

            LoadShaderFromFile("vertexshader1.sh", ShaderType.VertexShader, Program, out VertexShader);
            LoadShaderFromFile("fragmentshader.sh", ShaderType.FragmentShader, Program, out FragmentShader);

            GL.LinkProgram(Program);
            //debug, can remove
            Console.WriteLine(GL.GetProgramInfoLog(Program));
            /////

            VertexAttribProjection = GL.GetAttribLocation(Program, "projection");
            VertexAttribModelView = GL.GetUniformLocation(Program, "modelview");
            VertexAttribMVP = GL.GetUniformLocation(Program, "MVP");

            //if (VertexAttribPosition == -1 || VertexAttribProjection == -1 || VertexAttribModelView == -1)
            //{
            //    Console.WriteLine("Error binding attributes. VPosition:" + VertexAttribPosition + " VColor" + VertexAttribProjection + " MView:" + VertexAttribModelView);
            // }
        }

        
        public void LoadShaderFromFile(string filename, ShaderType type, int program, out int address)
        {
            address = GL.CreateShader(type);
            using (var sr = new StreamReader(@"C:\Users\Tree\Documents\GitHub\engine\engine\Shaders\" + filename))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
            }
            GL.CompileShader(address);
            GL.AttachShader(program, address);
            //debug, can remove
            Console.WriteLine(GL.GetShaderInfoLog(address));
            ////////
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

            GL.UseProgram(Program);
            
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            var pvMatrix = World.Camera.LookAt()*Projection;
            foreach (var entity in World.Entities)
            {
                var MVP =  entity.GetModelMatrix()*pvMatrix;
                GL.UniformMatrix4(VertexAttribMVP, false, ref MVP);
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

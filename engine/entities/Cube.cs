using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace engine.entities
{
    public class Cube : Entity
    {
        public Cube(Vector3 position, Quaternion rotation, Vector3 scale, Color4 color)
            : base(position, rotation, scale)
        {
            Color = color;
        }

        public Color4 Color { get; set; }

        public override void Render()
        {
            const float r = 0.5f;

            GL.PushMatrix();
            ApplyTransform();

            GL.Color4(Color);
            GL.Begin(PrimitiveType.Triangles);
            {
                // FRONT
                GL.Vertex3( r,  r,  r);
                GL.Vertex3( r, -r,  r);
                GL.Vertex3(-r, -r,  r);
                
                GL.Vertex3( r,  r,  r);
                GL.Vertex3(-r, -r,  r);
                GL.Vertex3(-r,  r,  r);
                
                // LEFT
                GL.Vertex3(-r,  r,  r);
                GL.Vertex3(-r, -r,  r);
                GL.Vertex3(-r, -r, -r);
                
                GL.Vertex3(-r,  r,  r);
                GL.Vertex3(-r, -r, -r);
                GL.Vertex3(-r,  r, -r);
                
                // TOP
                GL.Vertex3( r,  r,  r);
                GL.Vertex3(-r,  r,  r);
                GL.Vertex3(-r,  r, -r);
                
                GL.Vertex3( r,  r,  r);
                GL.Vertex3(-r,  r, -r);
                GL.Vertex3( r,  r, -r);
                
                // RIGHT
                GL.Vertex3( r,  r,  r);
                GL.Vertex3( r, -r,  r);
                GL.Vertex3( r, -r, -r);
                
                GL.Vertex3( r,  r,  r);
                GL.Vertex3( r, -r, -r);
                GL.Vertex3( r,  r, -r);
                
                // BOTTOM
                GL.Vertex3( r, -r,  r);
                GL.Vertex3( r, -r, -r);
                GL.Vertex3(-r, -r, -r);

                GL.Vertex3( r, -r,  r);
                GL.Vertex3(-r, -r, -r);
                GL.Vertex3(-r, -r,  r);
                
                // BACK
                GL.Vertex3( r,  r, -r);
                GL.Vertex3(-r,  r, -r);
                GL.Vertex3(-r, -r, -r);

                GL.Vertex3( r,  r, -r);
                GL.Vertex3(-r, -r, -r);
                GL.Vertex3( r, -r, -r);
            }
            GL.End();
            GL.PopMatrix();
        }

        public override void Update(KeyboardState keyboard, MouseState mouse)
        {
        }
    }
}

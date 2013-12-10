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
            const float u = 0.5f;

            GL.PushMatrix();
            ApplyTransform();

            GL.Color4(Color);
            GL.Begin(PrimitiveType.Triangles);
            {
                // FRONT
                GL.Vertex3( u,  u,  u);
                GL.Vertex3( u, -u,  u);
                GL.Vertex3(-u, -u,  u);
                
                GL.Vertex3( u,  u,  u);
                GL.Vertex3(-u, -u,  u);
                GL.Vertex3(-u,  u,  u);
                
                // LEFT
                GL.Vertex3(-u,  u,  u);
                GL.Vertex3(-u, -u,  u);
                GL.Vertex3(-u, -u, -u);
                
                GL.Vertex3(-u,  u,  u);
                GL.Vertex3(-u, -u, -u);
                GL.Vertex3(-u,  u, -u);
                
                // TOP
                GL.Vertex3( u,  u,  u);
                GL.Vertex3(-u,  u,  u);
                GL.Vertex3(-u,  u, -u);
                
                GL.Vertex3( u,  u,  u);
                GL.Vertex3(-u,  u, -u);
                GL.Vertex3( u,  u, -u);
                
                // RIGHT
                GL.Vertex3( u,  u,  u);
                GL.Vertex3( u, -u,  u);
                GL.Vertex3( u, -u, -u);
                
                GL.Vertex3( u,  u,  u);
                GL.Vertex3( u, -u, -u);
                GL.Vertex3( u,  u, -u);
                
                // BOTTOM
                GL.Vertex3( u, -u,  u);
                GL.Vertex3( u, -u, -u);
                GL.Vertex3(-u, -u, -u);

                GL.Vertex3( u, -u,  u);
                GL.Vertex3(-u, -u, -u);
                GL.Vertex3(-u, -u,  u);
                
                // BACK
                GL.Vertex3( u,  u, -u);
                GL.Vertex3(-u,  u, -u);
                GL.Vertex3(-u, -u, -u);

                GL.Vertex3( u,  u, -u);
                GL.Vertex3(-u, -u, -u);
                GL.Vertex3( u, -u, -u);
            }
            GL.End();

            const float u2 = u + 0.001f;
            GL.Color4(Color4.Black);
            GL.Begin(PrimitiveType.Lines);
            {
                // FRONT
                GL.Vertex3( u2,  u2,  u2);
                GL.Vertex3(-u2,  u2,  u2);
                
                GL.Vertex3(-u2,  u2,  u2);
                GL.Vertex3(-u2, -u2,  u2);

                GL.Vertex3(-u2, -u2,  u2);
                GL.Vertex3( u2, -u2,  u2);
                
                GL.Vertex3( u2, -u2,  u2);
                GL.Vertex3( u2,  u2,  u2);

                // BACK
                GL.Vertex3( u2,  u2, -u2);
                GL.Vertex3(-u2,  u2, -u2);

                GL.Vertex3(-u2,  u2, -u2);
                GL.Vertex3(-u2, -u2, -u2);

                GL.Vertex3(-u2, -u2, -u2);
                GL.Vertex3( u2, -u2, -u2);

                GL.Vertex3( u2, -u2, -u2);
                GL.Vertex3( u2,  u2, -u2);

                // LEFT
                GL.Vertex3(-u2,  u2,  u2);
                GL.Vertex3(-u2,  u2, -u2);

                GL.Vertex3(-u2, -u2,  u2);
                GL.Vertex3(-u2, -u2, -u2);

                // RIGHT
                GL.Vertex3( u2,  u2,  u2);
                GL.Vertex3( u2,  u2, -u2);
                
                GL.Vertex3( u2, -u2,  u2);
                GL.Vertex3( u2, -u2, -u2);

            }
            GL.End();

            GL.PopMatrix();
        }
    }
}

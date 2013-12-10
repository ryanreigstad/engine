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
    public class Plane : Entity
    {
        public Plane(Vector3 position, Quaternion rotation, Vector3 scale, Color4 color, int divisions = 10)
            : base(position, rotation, scale)
        {
            Color = color;
            Divisions = divisions;
        }

        public Color4 Color { get; set; }
        public int Divisions { get; set; }

        public override void Render()
        {
            GL.PushMatrix();
            ApplyTransform();
            GL.Color4(Color);

            var inc = 1 / (float) Divisions;
            var d2i = Divisions / 2f * inc;
            for (var i = -d2i; i < d2i; i += inc)
            {
                GL.Begin(PrimitiveType.LineStrip);
                {
                    for (var j = -d2i; j < d2i; j += inc)
                    {
                        GL.Vertex3(i, 0, j);
                    }
                }
                GL.End();

                GL.Begin(PrimitiveType.LineStrip);
                {
                    for (var j = -d2i; j < d2i; j += inc)
                    {
                        GL.Vertex3(j, 0, i);
                    }
                }
                GL.End();
            }
            GL.PopMatrix();
        }
    }
}

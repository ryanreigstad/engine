using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace engine.entities
{
    public class PointEntity : Entity
    {
        public PointEntity(Vector3 position, Color4 color)
            : base(position, Quaternion.Identity, Vector3.One)
        {
            Color = color;
        }

        public Color4 Color { get; set; }

        public override void Render()
        {
            GL.PushMatrix();
            GL.Translate(Position);

            GL.Color4(Color);
            GL.PointSize(3);
            GL.Begin(PrimitiveType.Points);
            {
                GL.Vertex3(0, 0, 0);
            }
            GL.End();
            GL.PointSize(1);
            GL.PopMatrix();
        }

        public override void Update(KeyboardState keyboard, MouseState mouse)
        {
        }
    }
}

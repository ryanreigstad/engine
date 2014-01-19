using System.Drawing;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace engine.entities
{
    public class CameraEntity : Entity
    {
        private float _velocity = 0.1f;
        private Vector2 _mouseLast;

        public CameraEntity(Vector3 position, Quaternion rotation)
            : base(position, rotation, Vector3.One)
        {
        }

        public override void Render()
        {
            // don't ApplyTransform() because we want inverse transform
            var matrix = Matrix4.CreateFromQuaternion(Rotation).Inverted();
            DrawAxisMap(matrix);

            matrix = Matrix4.CreateTranslation(-Position) * matrix;
            GL.MultMatrix(ref matrix);
        }

        private void DrawAxisMap(Matrix4 rotation)
        {
            // this could probably be made better
            GL.PushMatrix();
            var ortho = Matrix4.CreateOrthographic(1, 1, 0.01f, 10f);
            GL.LoadMatrix(ref ortho);
            GL.Translate(new Vector3(-0.4f, -0.3f, 5f));
            GL.MultMatrix(ref rotation);
            GL.Begin(PrimitiveType.Points);
            {
                GL.Color3(0.9, 0.9, 0.9);
                GL.Vertex3(Vector3.Zero);
            }
            GL.End();
            GL.Begin(PrimitiveType.Lines);
            {
                var t = -Position * 0.01f;
                GL.Color3(Color.Red);
                GL.Vertex3(t + Vector3.UnitX * -0.05f);
                GL.Vertex3(t + Vector3.UnitX * 0.2f);

                GL.Color3(Color.Green);
                GL.Vertex3(t + Vector3.UnitY * -0.05f);
                GL.Vertex3(t + Vector3.UnitY * 0.2f);

                GL.Color3(Color.Blue);
                GL.Vertex3(t + Vector3.UnitZ * -0.05f);
                GL.Vertex3(t + Vector3.UnitZ * 0.2f);
            }
            GL.End();

            GL.PopMatrix();
        }

        public override void Update(KeyboardState keyboard, MouseState mouse)
        {
            // breakpoint ?
            if (keyboard[Key.Space])
            {
            }

            // movement
            var v = Vector3.Zero;
            if (keyboard[InputConfig.MoveForward])
            {
                v += -Vector3.UnitZ * _velocity;
            }
            if (keyboard[InputConfig.MoveBackward])
            {
                v += (Vector3.UnitZ * _velocity);
            }
            if (keyboard[InputConfig.MoveRight])
            {
                v += (Vector3.UnitX * _velocity);
            }
            if (keyboard[InputConfig.MoveLeft])
            {
                v += (-Vector3.UnitX * _velocity);
            }
            if (keyboard[InputConfig.MoveUp])
            {
                v += (Vector3.UnitY * _velocity);
            }
            if (keyboard[InputConfig.MoveDown])
            {
                v += (-Vector3.UnitY * _velocity);
            }
            MoveLocal(v);

            // rotation
            if (keyboard[InputConfig.RollLeft])
            {
                RotateZ(0.03f);
            }
            if (keyboard[InputConfig.RollRight])
            {
                RotateZ(-0.03f);
            }

            var mouseNow = new Vector2(mouse.X, mouse.Y);
            if (mouseNow != _mouseLast)
            {
                var dmouse = _mouseLast - mouseNow;
                _mouseLast = mouseNow;

                RotateY(dmouse.X * 0.001f);
                RotateX(dmouse.Y * 0.001f);
            }

            // move speed
            if (keyboard[InputConfig.MoveFaster])
            {
                _velocity += 0.01f;
            }
            if (keyboard[InputConfig.MoveSlower])
            {
                _velocity -= 0.01f;
                if (_velocity < 0)
                    _velocity = 0;
            }
        }
    }
}

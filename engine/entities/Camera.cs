using System.Drawing;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace engine.entities
{
    public class Camera
    {
        public Camera(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }

        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }

        public void Move(Vector3 v)
        {
            Position += v;
        }

        public void MoveLocal(Vector3 v)
        {
            Position += Vector3.TransformVector(v, Matrix4.CreateFromQuaternion(Rotation));
        }

        public void RotateX(float radians)
        {
            var q = Quaternion.FromAxisAngle(Vector3.UnitX, radians);
            Rotation = Rotation * q;
            Rotation.Normalize();
        }

        public void RotateY(float radians)
        {
            var q = Quaternion.FromAxisAngle(Vector3.UnitY, radians);
            Rotation = q * Rotation;
            Rotation.Normalize();
        }
    }

    public class CameraEntity : Entity
    {
        public const float Velocity = 0.3f;
        private Vector2 _mouseLast;

        public CameraEntity(Vector3 position, Quaternion rotation)
            : base(position, rotation, Vector3.One)
        {
            Camera = new Camera(position, rotation);
        }

        public Camera Camera { get; set; }

        public new Vector3 Position
        {
            get { return Camera.Position; }
            set { Camera.Position = value; }
        }

        public new Quaternion Rotation
        {
            get { return Camera.Rotation; }
            set { Camera.Rotation = value; }
        }

        public new Vector3 Scale
        {
            get { return Vector3.One; }
        }

        public override void Render()
        {
            var matrix = Matrix4.CreateFromQuaternion(Rotation).Inverted();
            DrawAxisMap(matrix);

            matrix = Matrix4.CreateTranslation(-Position) * matrix;
            GL.MultMatrix(ref matrix);
        }

        private void DrawAxisMap(Matrix4 rotation)
        {
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
            if (keyboard[Key.Space])
            {
            }

            if (keyboard[InputConfig.MoveForward])
            {
                Camera.MoveLocal(-Vector3.UnitZ * Velocity);
            }
            if (keyboard[InputConfig.MoveBackward])
            {
                Camera.MoveLocal(Vector3.UnitZ * Velocity);
            }
            if (keyboard[InputConfig.MoveRight])
            {
                Camera.MoveLocal(Vector3.UnitX * Velocity);
            }
            if (keyboard[InputConfig.MoveLeft])
            {
                Camera.MoveLocal(-Vector3.UnitX * Velocity);
            }
            if (keyboard[InputConfig.MoveUp])
            {
                Camera.Move(Vector3.UnitY * Velocity);
            }
            if (keyboard[InputConfig.MoveDown])
            {
                Camera.Move(-Vector3.UnitY * Velocity);
            }

            var mouseNow = new Vector2(mouse.X, mouse.Y);
            var dmouse = _mouseLast - mouseNow;
            _mouseLast = mouseNow;

            Camera.RotateY(dmouse.X * 0.001f);
            Camera.RotateX(dmouse.Y * 0.001f);
        }
    }
}

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace engine.entities
{
    public class CameraEntity : Entity
    {
        private float _velocity = 0.5f;
        private Vector2 _mouseLast;

        public CameraEntity(Vector3 position, Quaternion rotation)
            : base(position, rotation, Vector3.One)
        {
            PitchRotation = Quaternion.FromAxisAngle(Vector3.UnitX, 0.0f);
            YawRotation = Quaternion.FromAxisAngle(Vector3.UnitY, 0.0f);
            RollRotation = Quaternion.FromAxisAngle(Vector3.UnitZ, 0.0f);
            PitchAngle = 0.0f;
            YawAngle = 0.0f;
            RollAngle = 0.0f;
        }

        public override void Render()
        {
            // don't ApplyTransform() because we want inverse transform
            var matrix = Matrix4.CreateFromQuaternion(Rotation);
            DrawAxisMap(matrix);

            matrix = Matrix4.CreateTranslation(-Position) * matrix;
            GL.MultMatrix(ref matrix);
        }

        public Matrix4 LookAt()
        {
            var m = Matrix3.CreateFromQuaternion(Rotation);
            var r = m.Row2;
            var v1 = new Vector3(r.X, r.Y, r.Z);

            //var pureZ = new Quaternion(Vector3.UnitZ, 0.0f);
            //var pureY = new Quaternion(Vector3.UnitY, 0.0f);
            
            //var qw = Rotation;
            //var conj = qw;
            //conj.Conjugate();
            //var temp = conj * pureZ * qw ;

            //var temp1 = conj * pureY * qw;

            return Matrix4.LookAt(Position, Position + v1, Vector3.UnitY);
        }

        private void DrawAxisMap(Matrix4 rotation)
        {
            // this could probably be made better
            GL.PushMatrix();
            var ortho = Matrix4.CreateOrthographic(1, 1, 0.05f, 10f);
            GL.LoadMatrix(ref ortho);
            GL.Translate(new Vector3(0.0f, 0.0f, 1f));
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
                v += -(GetForward() * _velocity);
            }
            if (keyboard[InputConfig.MoveBackward])
            {
                v += (GetForward() * _velocity);
            }
            if (keyboard[InputConfig.MoveRight])
            {
                v += (GetRight() * _velocity);
            }
            if (keyboard[InputConfig.MoveLeft])
            {
                v += -(GetRight() * _velocity);
            }
            if (keyboard[InputConfig.MoveUp])
            {
                v += (GetUp() * _velocity);
            }
            if (keyboard[InputConfig.MoveDown])
            {
                v += -(GetUp() * _velocity);
            }
            //MoveLocal(v);

            // rotation
            if (keyboard[InputConfig.RollLeft])
            {
                RollAngle += 0.03f;
                Roll();
            }
            if (keyboard[InputConfig.RollRight])
            {
                RollAngle -= 0.03f;
                Roll();
            }

            var mouseNow = new Vector2(mouse.X, mouse.Y);
            if (mouseNow != _mouseLast)
            {
                var dmouse = _mouseLast - mouseNow;
                _mouseLast = mouseNow;
                PitchAngle += dmouse.Y*-0.001f;
                YawAngle += dmouse.X*-0.001f;
                Yaw();
                Pitch();
            }
            TargetOrientation = PitchRotation*YawRotation  * RollRotation;
            Rotation = Quaternion.Slerp(Rotation, TargetOrientation, 0.1f);

            Position = Vector3.Lerp(Position, Position + v, 0.5f);
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

﻿using engine.Config;
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

        public new Matrix4 Transform
        {
            get
            {
                return Matrix4.CreateTranslation(-Position) * Matrix4.CreateFromQuaternion(Rotation).Inverted();
            }
        }

        public Matrix4 LookAt()
        {
            var v = Position + GetForward();
            return Matrix4.LookAt(Position, v, GetUp());
        }

        public override void Render()
        {
            // don't ApplyTransform() because we want inverse transform
            var matrix = Transform;
            GL.MultMatrix(ref matrix);
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
                v += Vector3.UnitZ * _velocity;
            }
            if (keyboard[InputConfig.MoveBackward])
            {
                v += (-Vector3.UnitZ * _velocity);
            }
            if (keyboard[InputConfig.MoveRight])
            {
                v += (-Vector3.UnitX * _velocity);
            }
            if (keyboard[InputConfig.MoveLeft])
            {
                v += (Vector3.UnitX * _velocity);
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
                RotateX(dmouse.Y * -0.001f);
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
using engine.data.config;

using OpenTK;
using OpenTK.Input;

namespace engine.core.entities
{
    public class Camera : Entity
    {
        private Vector2 _mouseLast;
        private float _velocity = 0.1f;

        public Camera()
            : this(Vector3.Zero, Quaternion.Identity)
        {
        }

        public Camera(Vector3 position, Quaternion rotation)
            : base(null, null, position, rotation, Vector3.One)
        {
        }

        public new Matrix4 Transform
        {
            get { return Matrix4.CreateTranslation(-Position) * Matrix4.CreateFromQuaternion(Rotation).Inverted(); }
        }

        public override void OnUpdate(KeyboardState keys, MouseState mouse)
        {
            // movement
            var v = Vector3.Zero;
            if (keys[InputConfig.MoveForward])
            {
                v += -Vector3.UnitZ * _velocity;
            }
            if (keys[InputConfig.MoveBackward])
            {
                v += Vector3.UnitZ * _velocity;
            }
            if (keys[InputConfig.MoveRight])
            {
                v += Vector3.UnitX * _velocity;
            }
            if (keys[InputConfig.MoveLeft])
            {
                v += -Vector3.UnitX * _velocity;
            }
            if (keys[InputConfig.MoveUp])
            {
                v += Vector3.UnitY * _velocity;
            }
            if (keys[InputConfig.MoveDown])
            {
                v += -Vector3.UnitY * _velocity;
            }
            MoveLocal(v);

            // rotation
            if (keys[InputConfig.RollLeft])
            {
                RotateZ(InputConfig.MouseSensitivity * 25);
            }
            if (keys[InputConfig.RollRight])
            {
                RotateZ(-InputConfig.MouseSensitivity * 25);
            }

            // mouse movement
            var mouseNow = new Vector2(mouse.X, mouse.Y);
            if (mouseNow != _mouseLast)
            {
                Vector2 dmouse = _mouseLast - mouseNow;
                _mouseLast = mouseNow;

                RotateY(dmouse.X * InputConfig.MouseSensitivity);
                RotateX(dmouse.Y * InputConfig.MouseSensitivity);
            }

            // move speed
            const float dv = 0.01f;
            if (keys[InputConfig.MoveFaster])
            {
                _velocity *= (1 + dv);
            }
            if (keys[InputConfig.MoveSlower])
            {
                _velocity *= (1 - dv);
                if (_velocity < dv)
                    _velocity = dv;
            }
        }
    }
}

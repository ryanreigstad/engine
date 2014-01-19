using System;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace engine.entities
{
    public abstract class Entity
    {
        protected Entity(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }

        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Scale { get; set; }

        public virtual void Load() {}

        public abstract void Render();

        public Action<Entity> OnUpdate = t => { };
        public virtual void Update(KeyboardState keyboard, MouseState mouse)
        {
            OnUpdate(this);
        }

        protected void ApplyTransform()
        {
            var trans = Matrix4.CreateFromQuaternion(Rotation)
                        * Matrix4.CreateScale(Scale)
                        * Matrix4.CreateTranslation(Position);
            GL.MultMatrix(ref trans);
        }

        public void MoveLocal(Vector3 v)
        {
            Position += Vector3.TransformVector(v, Matrix4.CreateFromQuaternion(Rotation));
        }

        public void RotateX(float radians)
        {
            Rotation *= Quaternion.FromAxisAngle(Vector3.UnitX, radians);
            Rotation.Normalize();
        }

        public void RotateY(float radians)
        {
            Rotation *= Quaternion.FromAxisAngle(Vector3.UnitY, radians);
            Rotation.Normalize();
        }

        public void RotateZ(float radians)
        {
            Rotation *= Quaternion.FromAxisAngle(Vector3.UnitZ, radians);
            Rotation.Normalize();
        }
    }
}

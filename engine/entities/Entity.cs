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

<<<<<<< HEAD
        public virtual void Load() { }
=======
        public virtual void Load() {}
>>>>>>> 2adcb1c3c9d879bd8d3e92ae273b452a761e885f

        public abstract void Render();

        public Action<Entity> OnUpdate = t => { };
        public virtual void Update(KeyboardState keyboard, MouseState mouse)
        {
            OnUpdate(this);
        }

        public Matrix4 GetModelMatrix()
        {
            return Matrix4.CreateFromQuaternion(Rotation) * Matrix4.CreateScale(Scale) * Matrix4.CreateTranslation(Position);
        }

        public Vector3 GetForward()
        {
            return GetTransform(Vector3.UnitZ);
        }
        public Vector3 GetUp()
        {
            return GetTransform(Vector3.UnitY);
        }
        public Vector3 GetRight()
        {
            return GetTransform(Vector3.UnitX);
        }

        protected void ApplyTransform()
        {
            var trans = Matrix4.CreateFromQuaternion(Rotation)
                        * Matrix4.CreateScale(Scale)
                        * Matrix4.CreateTranslation(Position);
            GL.MultMatrix(ref trans);
        }

        public Vector3 GetTransform(Vector3 v)
        {
<<<<<<< HEAD
            return Vector3.TransformVector(v, Matrix4.CreateFromQuaternion(Rotation));
        }

        public void MoveLocal(Vector3 v)
        {
=======
>>>>>>> 2adcb1c3c9d879bd8d3e92ae273b452a761e885f
            Position += Vector3.TransformVector(v, Matrix4.CreateFromQuaternion(Rotation));
        }

        /// <summary>
        /// Pitch in local frame
        /// </summary>
        /// <param name="radians"></param>
        public void RotateX(float radians)
        {
            Rotation *= Quaternion.FromAxisAngle(Vector3.UnitX, radians);
            Rotation.Normalize();
        }

        /// <summary>
        /// Yaw in local frame
        /// </summary>
        /// <param name="radians"></param>
        public void RotateY(float radians)
        {
            Rotation *= Quaternion.FromAxisAngle(Vector3.UnitY, radians);
            Rotation.Normalize();
        }

        /// <summary>
        /// Roll in local frame
        /// </summary>
        /// <param name="radians"></param>
        public void RotateZ(float radians)
        {
            Rotation *= Quaternion.FromAxisAngle(Vector3.UnitZ, radians);
            Rotation.Normalize();
        }
    }
}

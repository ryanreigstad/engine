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
            //TargetOrientation = YawRotation * PitchRotation * RollRotation;
            //Rotation = Quaternion.Slerp(Rotation, TargetOrientation, 0.1f);
            OnUpdate(this);
        }

        public Matrix4 GetModelMatrix()
        {
            return Matrix4.CreateFromQuaternion(Rotation) * Matrix4.CreateScale(Scale)  * Matrix4.CreateTranslation(Position);
        }

        public Vector3 GetForward()
        {
            var pureZ = new Quaternion(Vector3.UnitZ, 0.0f);
            var qw = Rotation;
            var conj = qw;
            conj.Conjugate();
            return (conj * pureZ * qw).Xyz;
        }
        public Vector3 GetUp()
        {
            var pureY = new Quaternion(Vector3.UnitY, 0.0f);
            var qw = Rotation;
            var conj = qw;
            conj.Conjugate();
            return (conj * pureY * qw).Xyz;
        }
        public Vector3 GetRight()
        {
            var pureX = new Quaternion(Vector3.UnitX, 0.0f);
            var qw = Rotation;
            var conj = qw;
            conj.Conjugate();
            return (conj * pureX * qw).Xyz;
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

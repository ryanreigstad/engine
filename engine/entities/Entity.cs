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
            TargetOrientation = rotation;
            Scale = scale;
        }

        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Scale { get; set; }

        public Quaternion PitchRotation { get; set; }
        public Quaternion YawRotation { get; set; }
        public Quaternion RollRotation { get; set; }

        public Quaternion TargetOrientation { get; set; }

        public float PitchAngle { get; set; }
        public float YawAngle { get; set; }
        public float RollAngle { get; set; }

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
            //Position = Vector3.Lerp(Position, v, .25f);
            Position += v;// Vector3.TransformVector(v, Matrix4.CreateFromQuaternion(Rotation));

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

        public void Pitch()
        {
            while (PitchAngle > Math.PI * 2)
                PitchAngle = PitchAngle - ((float)Math.PI * 2);
            while (PitchAngle < -Math.PI * 2)
                PitchAngle = PitchAngle + ((float)Math.PI * 2);

            PitchRotation = Quaternion.FromAxisAngle(Vector3.UnitX, PitchAngle);
        }

        public void Yaw()
        {
            while (YawAngle > Math.PI * 2)
                YawAngle = YawAngle - ((float)Math.PI * 2);
            while (YawAngle < -Math.PI * 2)
                YawAngle = YawAngle + ((float)Math.PI * 2);

            YawRotation = Quaternion.FromAxisAngle(Vector3.UnitY, YawAngle);
        }

        public void Roll()
        {
            while (RollAngle > Math.PI * 2)
                RollAngle = RollAngle - ((float)Math.PI * 2);
            while (RollAngle < -Math.PI * 2)
                RollAngle = RollAngle + ((float)Math.PI * 2);

            RollRotation = Quaternion.FromAxisAngle(Vector3.UnitZ, RollAngle);
        }
    }
}

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
            var trans = Matrix4.Identity;
            trans = trans * Matrix4.CreateFromQuaternion(Rotation);
            trans = trans * Matrix4.CreateScale(Scale);
            trans = trans * Matrix4.CreateTranslation(Position);
            GL.MultMatrix(ref trans);
        }
    }
}

using OpenTK;
using OpenTK.Input;

namespace engine.entities
{
    public abstract class Entity
    {
        protected Entity(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }

        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }

        public abstract void Render();

        public abstract void Update(KeyboardState keyboard, MouseState mouse);
    }
}

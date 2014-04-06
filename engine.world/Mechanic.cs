using OpenTK.Input;

namespace engine.world
{
    public abstract class Mechanic
    {
        public virtual void OnLoad()
        {
        }

        public abstract void OnUpdate(KeyboardState keys, MouseState mouse);
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;

using OpenTK.Input;

namespace engine.world
{
    public class World
    {
        public World()
            : this(new Camera(), new List<Entity>(), new List<Light>(), new List<Mechanic>())
        {
        }

        public World(Camera camera, List<Entity> entities, List<Light> lights, List<Mechanic> mechanics)
        {
            Camera = camera;
            Entities = entities;
            Lights = lights;
            Mechanics = mechanics;
        }

        public Camera Camera { get; set; }
        public List<Entity> Entities { get; set; }
        public List<Light> Lights { get; set; }
        public List<Mechanic> Mechanics { get; set; }

        public Action OnLoading = () => { };
        public void OnLoad()
        {
            OnLoading();

            foreach (Mechanic mechanic in Mechanics)
            {
                mechanic.OnLoad();
            }
            foreach (Entity entity in Entities)
            {
                entity.OnLoad();
            }
            Camera.OnLoad();
        }

        public void OnUpdate(KeyboardState keys, MouseState mouse)
        {
            if (keys[Key.Space])
                Debugger.Break();

            // TODO: make a input manager that can be polled

            Camera.OnUpdate(keys, mouse);

            foreach (Entity entity in Entities)
            {
                entity.OnUpdate(keys, mouse);
            }

            foreach (Mechanic mechanic in Mechanics)
            {
                mechanic.OnUpdate(keys, mouse);
            }
        }
    }
}

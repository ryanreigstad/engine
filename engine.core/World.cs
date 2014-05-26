using System;
using System.Collections.Generic;
using System.Diagnostics;

using engine.core.entities;
using engine.data.config;
using OpenTK.Input;

namespace engine.core
{
    public class World
    {
        public World()
            : this(new Camera(), new List<Entity>(), new List<Light>())
        {
        }

        public World(Camera camera, List<Entity> entities, List<Light> lights)
        {
            Camera = camera;
            Entities = entities;
            Lights = lights;
        }

        public Camera Camera { get; set; }
        public List<Entity> Entities { get; set; }
        public List<Light> Lights { get; set; }

        public Action OnLoading = () => { };
        public void OnLoad()
        {
            OnLoading();

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
        }
    }
}

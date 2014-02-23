using System;
using System.Collections.Generic;

using engine.entities;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace engine
{
    public class GameWorld
    {
        /// <summary>
        /// HACK!
        /// </summary>
        public static GameWorld Instance;
        public CameraEntity Camera { get; set; }
        public List<Entity> Entities { get; set; }

        public Action OnLoading = () => { };

        public GameWorld()
        {
            Camera = new CameraEntity(new Vector3(0, 0, -10.0f), Quaternion.FromAxisAngle(Vector3.UnitZ, 0));
            Entities = new List<Entity>();
            Instance = this;
        }


        public void OnLoad()
        {
            OnLoading();
            Camera.Load();
            Entities.ForEach(e => e.Load());
        }

        public void OnUpdate(KeyboardState keyboard, MouseState mouse)
        {
            Camera.Update(keyboard, mouse);
            Entities.ForEach(e => e.Update(keyboard, mouse));
        }

        public void OnRender()
        {
            GL.PushMatrix();
            Camera.Render();

            Entities.ForEach(e => e.Render());

            GL.PopMatrix();
        }
    }
}

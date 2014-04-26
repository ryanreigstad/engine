using System;

using engine.graphics;
using engine.world;

using OpenTK;

namespace engine.testificate
{
    public static class GameWorldFactory
    {
        public static World World()
        {
            var world = new World();

            world.OnLoading += () =>
            {
                TextureLibrary.LoadTextureFromFile("alduin.jpg");
                MeshLibrary.LoadObjFile("alduin.obj");

                world.Entities.Add(new Entity("alduin.obj", "alduin.jpg"));

                world.Lights.Add(new AmbientLight(Vector3.One, Vector3.One));
            };

            return world;
        }
    }
}

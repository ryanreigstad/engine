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
                MeshLibrary.LoadObjFile("alduin.obj");
                TextureLibrary.LoadTextureFromFile("alduin.jpg");

                world.Entities.Add(new Entity("alduin.obj", "alduin.jpg"));
                //world.Entities.Add(new FullScreenQuad("alduin.jpg") {Scale = Vector3.One / 1f});

                world.Lights.Add(new PointLight(Vector3.One, Vector3.One) {Scale = Vector3.One * 50});
            };

            return world;
        }
    }
}

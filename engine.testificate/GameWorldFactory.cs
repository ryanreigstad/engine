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
                TextureLibrary.LoadTextureFromFile("smiley.png");

                MeshLibrary.LoadObjFile("serpentine city.obj");

                world.Entities.Add(new Entity("serpentine city.obj", "smiley.png"));

                world.Lights.Add(new AmbientLight(Vector3.One, Vector3.One));
            };

            return world;
        }
    }
}

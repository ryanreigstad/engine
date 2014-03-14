using System.Collections.Generic;
using System.Drawing;
using engine.entities;
using OpenTK;

namespace engine.Libraries
{
    public static class GameWorldFactory
    {
        public static GameWorld BuildManagedTerrain()
        {
            return new GameWorld
            {
                Entities = new List<Entity>
                {
                    new TerrainManager(Vector3.Zero, Quaternion.Identity, Vector3.One)
                }
            };
        }

        public static GameWorld BuildNewVertexBuffer()
        {
            var world = new GameWorld();

            world.OnLoading += () =>
            {
                world.Entities.Add(PrimitiveFactory.BuildPlane(new Vector3(0, 0, 0), Quaternion.Identity, Vector3.One * 30, Color.Green, 20, 20));
                var c = PrimitiveFactory.BuildCube(new Vector3(0, 0, 0), Quaternion.Identity, Vector3.One, Color.Green);
                var d = PrimitiveFactory.BuildCube(new Vector3(2, 0, 0), Quaternion.Identity, Vector3.One, Color.Green);
                world.Entities.Add(c);
                world.Entities.Add(d);
            };

            return world;
        }
    }
}
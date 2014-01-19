using System;
using System.Collections.Generic;
using System.Drawing;

using engine.entities;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using SimplexNoise;

namespace engine
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
                world.Entities.Add(PrimitiveFactory.BuildPlane(new Vector3(0, -3, 0), Quaternion.Identity, Vector3.One * 5, Color.Green, 2, 2));
                world.Entities.Add(PrimitiveFactory.BuildPlane(new Vector3(0, -1, 0), Quaternion.Identity, Vector3.One * 3, Color.LightGreen, 2, 2));

                var c = PrimitiveFactory.BuildCube(new Vector3(0, 0, 0), Quaternion.Identity, Vector3.One, Color.Red);
                c.OnUpdate += _this => _this.RotateY(0.01f);
                world.Entities.Add(c);

                c = PrimitiveFactory.BuildCube(new Vector3(-3, 0, 0), Quaternion.Identity, Vector3.One, Color.Blue);
                c.OnUpdate += _this => _this.RotateX(0.01f);
                world.Entities.Add(c);

                c = PrimitiveFactory.BuildCube(new Vector3(3, 0, 0), Quaternion.Identity, Vector3.One, Color.Blue);
                c.OnUpdate += _this => _this.RotateZ(0.01f);
                world.Entities.Add(c);
            };

            return world;
        }
    }
}
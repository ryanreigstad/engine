using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using engine.entities;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

using SimplexNoise;

namespace engine
{
    public class GameWorld
    {
        public GameWorld()
        {
            Camera = new CameraEntity(Vector3.Zero, Quaternion.Identity);
            Entities = new List<Entity>();
        }

        public CameraEntity Camera { get; set; }
        public List<Entity> Entities { get; set; }

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

    public static class GameWorldFactory
    {
        public static GameWorld BuildVboTerrain()
        {
            var d = Enumerable.Range(0, 1000).ToList();
            var rand = new Random();
            var nx = (float)rand.NextDouble() * 100000;
            var ny = (float)rand.NextDouble() * 100000;
            var ns = 0.01f;

            var verticeData = new float[d.Count][];
            var colorData = new Color[d.Count][];

            Func<float, Color> getColor = v =>
            {
                if (v < -0.5f)
                    return Color.DarkBlue;
                if (v < -0.3f)
                    return Color.Blue;
                if (v < -0.1f)
                    return Color.SandyBrown;
                if (v < 0.5f)
                    return Color.Green;
                if (v < 0.7f)
                    return Color.LightGreen;
                if (v < 0.9f)
                    return Color.Gray;
                return Color.White;
            };

            d.ForEach(
                i =>
                {
                    verticeData[i] = new float[d.Count];
                    colorData[i] = new Color[d.Count];
                    d.ForEach(
                        j =>
                        {
                            verticeData[i][j] = Enumerable.Range(0, 4)
                                .Sum(iter =>
                                    Noise.Generate(i * ns * (1 << iter) + ny, j * ns * (1 << iter) + nx) / (float)(1 << iter)
                                );
                            colorData[i][j] = getColor(verticeData[i][j]);
                        });
                });

            var t = -0.5f;
            var xi = 1f / verticeData.Length;
            var zi = 1f / verticeData[0].Length;

            var vertices = new Vector3[verticeData.Length * verticeData[0].Length];
            var colors = new int[vertices.Length];
            var indices = new List<uint>();

            for (var x = 0; x < verticeData.Length; x++)
            {
                for (var z = 0; z < verticeData[x].Length; z++)
                {
                    var i = x * verticeData.Length + z;
                    vertices[i] = new Vector3(t + x * xi, verticeData[x][z], t + z * zi);
                    colors[i] = VboFactory.ColorToRgba32(colorData[x][z]);
                }
            }
            for (var x = 0; x < verticeData.Length - 1; x++)
            {
                for (var y = 0; y < verticeData[x].Length - 1; y++)
                {
                    var i = x * verticeData.Length + y;
                    var i0 = i;
                    var i1 = i + 1;
                    var i2 = i + verticeData[x].Length;
                    var i3 = i + verticeData[x].Length + 1;

                    //indices[i * 6 + 0] = (uint) i0;
                    //indices[i * 6 + 1] = (uint) i1;
                    //indices[i * 6 + 2] = (uint) i2;
                    //indices[i * 6 + 3] = (uint) i2;
                    //indices[i * 6 + 4] = (uint) i3;
                    //indices[i * 6 + 5] = (uint) i0;
                    indices.Add((uint) i0);
                    indices.Add((uint) i1);
                    indices.Add((uint) i2);
                    indices.Add((uint) i2);
                    indices.Add((uint) i1);
                    indices.Add((uint) i3);
                }
            }

            return new GameWorld
            {
                Entities = new List<Entity>
                {
                    new VboEntity(Vector3.Zero, Quaternion.Identity, new Vector3(400, 5, 400), vertices, null, colors, indices.ToArray())
                }
            };
        }

        public static GameWorld BuildTerrain()
        {
            var rand = new Random();
            var nx = (float) rand.NextDouble() * 100000;
            var ny = (float) rand.NextDouble() * 100000;
            var ns = 0.01f;

            var d = Enumerable.Range(0, 350).ToList();
            var data = new float[d.Count][];
            d.ForEach(
                i =>
                {
                    data[i] = new float[d.Count];
                    d.ForEach(
                        j =>
                        {
                            data[i][j] = Enumerable.Range(0, 5)
                                .Sum(iter =>
                                    Noise.Generate(i * ns * (1 << iter) + ny, j * ns * (1 << iter) + nx) / (float) (1 << iter)
                                );
                        });
                });

            var world = new GameWorld
            {
                Entities = new List<Entity>
                {
                    new HeightMap(Vector3.Zero, Quaternion.Identity, new Vector3(40, 1, 40), data)
                }
            };
            return world;
        }

        public static GameWorld BuildCube()
        {
            var d = new List<float> {-0.501f, 0.501f};
            return new GameWorld
            {
                Entities = d.SelectMany(
                    x => d.SelectMany(
                    y => d.Select(
                    z => (Entity)new Cube(new Vector3(x, y, z), Quaternion.Identity, Vector3.One,
                        new Color4(
                            x > 0f ? 1f : 0.1f,
                            y > 0f ? 1f : 0.1f,
                            z > 0f ? 1f : 0.1f,
                            1f))
                ))).Concat(
                    new List<Entity>
                    {
                        new Cube(new Vector3(0, 5, 0), Quaternion.FromAxisAngle(Vector3.UnitX, MathHelper.PiOver4) * Quaternion.FromAxisAngle(Vector3.UnitZ, MathHelper.PiOver4), new Vector3(3, 3, 3), Color4.White)
                        {
                            OnUpdate = cube =>
                            {
                            }
                        },
                        new Plane(-Vector3.UnitY, Quaternion.Identity, new Vector3(100, 1, 100), Color4.DarkBlue, 100),
                        new VboEntity(Vector3.UnitY * 10, Quaternion.Identity, Vector3.One)
                    }
                ).ToList()
            };
        }

        public static GameWorld BuildDottedSphere(float size, float variance, float density, int columns = 16, int rows = 16)
        {
            var rand = new Random();
            var sx = (float)rand.NextDouble() * 1000f;
            var sy = (float)rand.NextDouble() * 1000f;
            var sz = (float)rand.NextDouble() * 1000f;

            var world = new GameWorld();

            {
                var v = Noise.Generate(sx, sy + size * density, sz) * variance;
                //v = v * v * (v > 0 ? 1 : -1);
                world.Entities.Add(new PointEntity(new Vector3(0, size + size * v, 0), Color4.White));
            }
            for (var r = 1; r < rows; r++)
            {
                var rr = r / (float) rows;
                for (var c = 0; c < columns; c++)
                {
                    var cr = c / (float) columns;

                    var x = (float) (Math.Sin(Math.PI * rr) * Math.Cos(2 * Math.PI * cr) * size);
                    var y = (float) (Math.Cos(Math.PI * rr) * size);
                    var z = (float) (Math.Sin(Math.PI * rr) * Math.Sin(2 * Math.PI * cr) * size);

                    var v = Noise.Generate(sx + x * density, sy + y * density, sz + z * density) * variance;
                    //v = v * v * (v > 0 ? 1 : -1);

                    var nx = x + x * v;
                    var ny = y + y * v;
                    var nz = z + z * v;

                    world.Entities.Add(
                        new PointEntity(
                            new Vector3(nx, ny, nz),
                            new Color4(
                                nx > 0f ? 1f : 0.1f,
                                ny > 0f ? 1f : 0.1f,
                                nz > 0f ? 1f : 0.1f,
                                1f)));
                    // dark blue sphere (sea)
                    //world.Entities.Add(
                    //    new PointEntity(
                    //        new Vector3(x, y, z), Quaternion.Identity, new Color4(0, 0, 150, 25)));
                }
            }
            {
                var v = Noise.Generate(sx, sy - size * density, sz) * variance;
                //v = v * v * (v > 0 ? 1 : -1);
                world.Entities.Add(new PointEntity(new Vector3(0, -(size + size * v), 0), Color4.White));
            }

            return world;
        }
    }
}

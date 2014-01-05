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

        public Action OnLoading = () => { };
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

    public static class GameWorldFactory
    {
        public static GameWorld BuildVoxelTerrain()
        {
            var size = Enumerable.Range(0, 256).ToList();
            var rand = new Random();

            var nx = (float) rand.NextDouble() * 100000;
            var ny = (float) rand.NextDouble() * 100000;
            var nz = (float) rand.NextDouble() * 100000;
            var ns = 0.06f;

            var voxels = new Voxel[size.Count][][];
            size.ForEach(i => { voxels[i] = new Voxel[size.Count][]; size.ForEach(j => voxels[i][j] = new Voxel[size.Count]); });

            Console.WriteLine("Generating Voxels");
            size.ForEach(
                x => size.ForEach(
                y => size.ForEach(
                z =>
                {
                    float wx = (x - size.Count / 2f) / (size.Count / 2f),
                          wy = (y - size.Count / 2f) / (size.Count / 2f),
                          wz = (z - size.Count / 2f) / (size.Count / 2f);

                    //var data = wy; // flat(ish) surface
                    var data = 0.8f - (float)Math.Sqrt(wx * wx + wy * wy + wz * wz); // sphere(ish) surface
                    data += Noise.Generate(x * ns * 1 + nx, y * ns * 1 + ny, z * ns * 1 + nz) / 32f;
                    data += Noise.Generate(x * ns * 4 + nx, y * ns * 4 + ny, z * ns * 4 + nz) / 64f;

                    if (data < 0)
                        voxels[x][y][z] = Voxel.Empty;
                    else
                    {
                        voxels[x][y][z] = new Voxel(Color.Gray.MutateColor(rand).RgbToColor());
                        //if (y < 0.2 * size.Count)
                        //    voxels[x][y][z] = new Voxel(Color.SandyBrown.MutateColor(rand).RgbToColor());
                        //else if (y < 0.7 * size.Count)
                        //    voxels[x][y][z] = new Voxel(Color.Green.MutateColor(rand).RgbToColor());
                        //else if (y < 0.85 * size.Count)
                        //    voxels[x][y][z] = new Voxel(Color.LightGreen.MutateColor(rand).RgbToColor());
                        //else if (y < 0.95 * size.Count)
                        //    voxels[x][y][z] = new Voxel(Color.Gray.MutateColor(rand).RgbToColor());
                        //else
                        //    voxels[x][y][z] = new Voxel(Color.White.MutateColor(rand).RgbToColor());
                    }
                })));

            var world = new GameWorld();

            world.OnLoading += () =>
            {
                Console.WriteLine("Marching Voxels");
                var indices = 0;
                var ids = MarchingCubes.March(voxels, 100, "terrain", out indices);
                //var ids = MarchingCubes.March(
                //    new[]
                //    {
                //        new[]
                //        {
                //            new[] {Voxel.Empty, Voxel.Empty, Voxel.Empty, Voxel.Empty, Voxel.Empty},
                //            new[] {Voxel.Empty, Voxel.Empty, Voxel.Empty, Voxel.Empty, Voxel.Empty},
                //            new[] {Voxel.Empty, Voxel.Empty, Voxel.Empty, Voxel.Empty, Voxel.Empty},
                //            new[] {Voxel.Empty, Voxel.Empty, Voxel.Empty, Voxel.Empty, Voxel.Empty},
                //            new[] {Voxel.Empty, Voxel.Empty, Voxel.Empty, Voxel.Empty, Voxel.Empty}
                //        },
                //        new[]
                //        {
                //            new[] {Voxel.Empty, Voxel.Empty, Voxel.Empty, Voxel.Empty, Voxel.Empty},
                //            new[] {Voxel.Empty, new Voxel(Color.Red), Voxel.Empty, new Voxel(Color.Blue), Voxel.Empty},
                //            new[] {Voxel.Empty, Voxel.Empty, Voxel.Empty, Voxel.Empty, Voxel.Empty},
                //            new[] {Voxel.Empty, new Voxel(Color.Green), Voxel.Empty, new Voxel(Color.Yellow), Voxel.Empty},
                //            new[] {Voxel.Empty, Voxel.Empty, Voxel.Empty, Voxel.Empty, Voxel.Empty}
                //        },
                //        new[]
                //        {
                //            new[] {Voxel.Empty, Voxel.Empty, Voxel.Empty, Voxel.Empty, Voxel.Empty},
                //            new[] {Voxel.Empty, Voxel.Empty, Voxel.Empty, Voxel.Empty, Voxel.Empty},
                //            new[] {Voxel.Empty, Voxel.Empty, new Voxel(Color.White), Voxel.Empty, Voxel.Empty},
                //            new[] {Voxel.Empty, Voxel.Empty, Voxel.Empty, Voxel.Empty, Voxel.Empty},
                //            new[] {Voxel.Empty, Voxel.Empty, Voxel.Empty, Voxel.Empty, Voxel.Empty}
                //        },
                //        new[]
                //        {
                //            new[] {Voxel.Empty, Voxel.Empty, Voxel.Empty, Voxel.Empty, Voxel.Empty},
                //            new[] {Voxel.Empty, new Voxel(Color.Yellow), Voxel.Empty, new Voxel(Color.Green), Voxel.Empty},
                //            new[] {Voxel.Empty, Voxel.Empty, Voxel.Empty, Voxel.Empty, Voxel.Empty},
                //            new[] {Voxel.Empty, new Voxel(Color.Blue), Voxel.Empty, new Voxel(Color.Red), Voxel.Empty},
                //            new[] {Voxel.Empty, Voxel.Empty, Voxel.Empty, Voxel.Empty, Voxel.Empty}
                //        },
                //        new[]
                //        {
                //            new[] {Voxel.Empty, Voxel.Empty, Voxel.Empty, Voxel.Empty, Voxel.Empty},
                //            new[] {Voxel.Empty, Voxel.Empty, Voxel.Empty, Voxel.Empty, Voxel.Empty},
                //            new[] {Voxel.Empty, Voxel.Empty, Voxel.Empty, Voxel.Empty, Voxel.Empty},
                //            new[] {Voxel.Empty, Voxel.Empty, Voxel.Empty, new Voxel(Color.Maroon), Voxel.Empty},
                //            new[] {Voxel.Empty, Voxel.Empty, Voxel.Empty, Voxel.Empty, Voxel.Empty}
                //        }
                //    }, 1, "terrain", out indices);
                int v = ids[0],
                    i = ids[1],
                    c = ids[2];
                world.Entities.Add(new VboEntity(Vector3.Zero, Quaternion.Identity, Vector3.One, v, i, c, indices));
            };

            return world;
        }

        private static Color RgbToColor(this int c)
        {
            return Color.FromArgb(c & 255, (c & 255 << 8) >> 8, (c & 255 << 16) >> 16);
        }

        private static int MutateColor(this Color c, Random rand)
        {
            int temp;
            return ((temp = c.R + (rand.Next(100) - 50)) < 0 ? 0 : temp > 255 ? 255 : temp)
                 | ((temp = c.G + (rand.Next(100) - 50)) < 0 ? 0 : temp > 255 ? 255 : temp) << 8
                 | ((temp = c.B + (rand.Next(100) - 50)) < 0 ? 0 : temp > 255 ? 255 : temp) << 16
                 | c.A << 24;
        }

        public static GameWorld BuildVboTerrain()
        {
            var d = Enumerable.Range(0, 1000).ToList();
            var rand = new Random();
            var nx = (float)rand.NextDouble() * 100000;
            var ny = (float)rand.NextDouble() * 100000;
            var ns = 0.006f;

            var verticeData = new float[d.Count()][];
            var colorData = new Color[d.Count()][];

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
                    verticeData[i] = new float[d.Count()];
                    colorData[i] = new Color[d.Count()];
                    d.ForEach(
                        j =>
                        {
                            verticeData[i][j] = Enumerable.Range(0, 6)
                                .Sum(iter =>
                                    {
                                        var y = Noise.Generate(i * ns * (1 << iter) + ny, j * ns * (1 << iter) + nx);

                                        //y = y * y * (y < 0 ? -1 : 1);
                                        y = y / (1 << iter);

                                        return y;
                                    }
                                );
                            colorData[i][j] = getColor(verticeData[i][j]);
                        });
                });

            var t = -5f;
            var xi = 10f / verticeData.Length;
            var zi = 10f / verticeData[0].Length;

            var vertices = new Vector3[verticeData.Length * verticeData[0].Length];
            var colors = new int[vertices.Length];
            var indices = new List<uint>();

            for (var x = 0; x < verticeData.Length; x++)
            {
                for (var z = 0; z < verticeData[x].Length; z++)
                {
                    var i = x * verticeData.Length + z;

                    vertices[i] = new Vector3(t + x * xi, verticeData[x][z], t + z * zi);

                    var c = colorData[x][z];
                    int temp;
                    colors[i]
                        = ((temp = c.R + (rand.Next(20) - 10)) < 0 ? 0 : temp > 255 ? 255 : temp)
                        | ((temp = c.G + (rand.Next(20) - 10)) < 0 ? 0 : temp > 255 ? 255 : temp) << 8
                        | ((temp = c.B + (rand.Next(20) - 10)) < 0 ? 0 : temp > 255 ? 255 : temp) << 16
                        | c.A << 24;

                    if (x < verticeData.Length - 1 && z < verticeData[x].Length - 1)
                    {
                        var i0 = i;
                        var i1 = i + 1;
                        var i2 = i + verticeData[x].Length;
                        var i3 = i + verticeData[x].Length + 1;
                        indices.Add((uint)i0);
                        indices.Add((uint)i1);
                        indices.Add((uint)i2);
                        indices.Add((uint)i2);
                        indices.Add((uint)i1);
                        indices.Add((uint)i3);
                    }
                }
            }

            return new GameWorld
            {
                Entities = new List<Entity>
                {
                    new VboEntity(Vector3.Zero, Quaternion.Identity, new Vector3(40, 4, 40) * 2, vertices, null, colors, indices.ToArray())
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
                        Color.FromArgb(
                            x > 0f ? 255 : 15,
                            y > 0f ? 255 : 15,
                            z > 0f ? 255 : 15))
                ))).Concat(
                    new List<Entity>
                    {
                        new Cube(new Vector3(0, 5, 0), Quaternion.FromAxisAngle(Vector3.UnitX, MathHelper.PiOver4) * Quaternion.FromAxisAngle(Vector3.UnitZ, MathHelper.PiOver4), new Vector3(3, 3, 3), Color.White),
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

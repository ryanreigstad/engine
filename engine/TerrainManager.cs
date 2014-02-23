using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using engine.entities;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

using SimplexNoise;

namespace engine
{
    public static class FlatTerrainFactory
    {
        private static readonly Random Random = new Random();
        public static readonly Vector2i ChunkSize = new Vector2i {X = 32, Y = 32};
        public const int ChunkHeight = 64;
        public const double ChunkScale = 25d;

        private static readonly Vector3d NoiseLocation;
        private const double NoiseStep = 0.01d;

        static FlatTerrainFactory()
        {
            NoiseLocation = new Vector3d(Random.NextDouble() * 10000, Random.NextDouble() * 10000, Random.NextDouble() * 10000);
        }

        public static Voxel[,,] GenerateChunk(Vector2i i)
        {
            var data = new float[ChunkSize.X + 1, ChunkHeight, ChunkSize.Y + 1];
            var voxels = new Voxel[ChunkSize.X + 1, ChunkHeight, ChunkSize.Y + 1];
            var ns = new Vector3d(
                    NoiseStep * ChunkSize.X * i.X,
                    0,
                    NoiseStep * ChunkSize.Y * i.Y
                );

            // gen data
            for (var x = 0; x < data.GetLength(0); x++)
            {
                for (var y = 0; y < data.GetLength(1); y++)
                {
                    for (var z = 0; z < data.GetLength(2); z++)
                    {
                        var density = -y / (float) ChunkHeight / 2f + 0.1f;

                        density += Noise.Generate(
                            (float) (NoiseLocation.X + ns.X + NoiseStep * x * 1),
                            (float)(NoiseLocation.X + ns.Y + NoiseStep * y * 1),
                            (float)(NoiseLocation.X + ns.Z + NoiseStep * z * 1)) / 2f;

                        density += Noise.Generate(
                            (float)(NoiseLocation.X + ns.X * 2 + NoiseStep * x * 2),
                            (float)(NoiseLocation.X + ns.Y * 2 + NoiseStep * y * 2),
                            (float)(NoiseLocation.X + ns.Z * 2 + NoiseStep * z * 2)) / 8f;

                        density += Noise.Generate(
                            (float)(NoiseLocation.X + ns.X * 4 + NoiseStep * x * 4),
                            (float)(NoiseLocation.X + ns.Y * 4 + NoiseStep * y * 4),
                            (float)(NoiseLocation.X + ns.Z * 4 + NoiseStep * z * 4)) / 16f;

                        density += Noise.Generate(
                            (float)(NoiseLocation.X + ns.X * 8 + NoiseStep * x * 8),
                            (float)(NoiseLocation.X + ns.Y * 8 + NoiseStep * y * 8),
                            (float)(NoiseLocation.X + ns.Z * 8 + NoiseStep * z * 8)) / 32f;

                        data[x, y, z] = density;
                    }
                }
            }
            // make voxels
            for (var x = 0; x < data.GetLength(0); x++)
            {
                for (var y = 0; y < data.GetLength(1); y++)
                {
                    for (var z = 0; z < data.GetLength(2); z++)
                    {
                        var density = data[x, y, z];
                        var normal = new Vector3(
                                ((x < data.GetLength(0) - 1 ? data[x + 1, y, z] : data[x, y, z]) + (x > 0 ? data[x - 1, y, z] : data[x, y, z])) / 2f,
                                ((y < data.GetLength(0) - 1 ? data[x, y + 1, z] : data[x, y, z]) + (y > 0 ? data[x, y - 1, z] : data[x, y, z])) / 2f,
                                ((z < data.GetLength(0) - 1 ? data[x, y, z + 1] : data[x, y, z]) + (z > 0 ? data[x, y, z - 1] : data[x, y, z])) / 2f
                            ).Normalized();

                        if (y / (float)ChunkHeight < 0.2f)
                            voxels[x, y, z] = new Voxel(1d, VoxelType.Blue, normal);
                        else
                            if (density < 0f)
                                voxels[x, y, z] = new Voxel(density, VoxelType.None, normal);
                            else
                                voxels[x, y, z] = new Voxel(density, VoxelType.DarkGray, normal);
                    }
                }
            }

            return voxels;
        }

        public static Vector3d ChunkToWorldSpace(Vector2i v)
        {
            return new Vector3d(
                v.X * ChunkScale,
                0,
                v.Y * ChunkScale
                );
        }
    }

    public class TerrainManager : Entity
    {
        public TerrainManager(Vector3 position, Quaternion rotation, Vector3 scale)
            : base(position, rotation, scale)
        {
        }

        private readonly Dictionary<Vector2i, VboEntity> _chunks = new Dictionary<Vector2i, VboEntity>();
        private Vector2i _cameraChunk;

        public override void Update(KeyboardState keyboard, MouseState mouse)
        {
            base.Update(keyboard, mouse);

            _cameraChunk = new Vector2i
            {
                X = (int) (GameWorld.Instance.Camera.Position.X / FlatTerrainFactory.ChunkScale),
                Y = (int) (GameWorld.Instance.Camera.Position.Z / FlatTerrainFactory.ChunkScale)
            };

            var c = new[] {0, 1, -1, 2, -2};
            for (var i = 0; i < c.Length; i++)
            {
                for (var j = 0; j < c.Length; j++)
                {
                    var v = new Vector2i { X = _cameraChunk.X + c[i], Y = _cameraChunk.Y + c[j] };
                    if (!HasChunk(v))
                    {
                        var voxels = FlatTerrainFactory.GenerateChunk(v);
                        int ic;
                        var ids = MarchingCubes.March(voxels, (float) (FlatTerrainFactory.ChunkScale * (FlatTerrainFactory.ChunkSize.X + 1) / FlatTerrainFactory.ChunkSize.X), "chunk(" + v.X + ", " + v.Y + ")", out ic);
                        _chunks[v] = new VboEntity((Vector3) FlatTerrainFactory.ChunkToWorldSpace(v), Quaternion.Identity, new Vector3(1, 2, 1), ids[0], ids[1], ic, PrimitiveType.Triangles);
                        
                        // goto lol ( i wanted to "break outer;" but that's not in c# )
                        goto createdChunk;
                    }
                }
            }
            createdChunk:;
            
            _chunks.Keys.ToList().ForEach(k => _chunks[k].Update(keyboard, mouse));
        }

        public override void Render()
        {
            GL.PushMatrix();
            ApplyTransform();

            _chunks.Keys.ToList().ForEach(k => _chunks[k].Render());

            GL.PopMatrix();
        }

        public bool HasChunk(Vector2i i)
        {
            return _chunks.ContainsKey(i);
        }
    }

    public struct Vector2i
    {
        public int X;
        public int Y;
    }
}

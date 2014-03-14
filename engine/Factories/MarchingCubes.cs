using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using engine.Libraries;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace engine
{
    public static class MarchingCubes
    {
        // TODO: voxel density
        static MarchingCubes()
        {
            StateTriangles = new int[256][][];

            #region Empty / Full
            // not actually used
            // 0000 0000
            StateTriangles[0] = new[] {new[] {-1, -1, -1}};
            // 1111 1111
            StateTriangles[255] = new[] {new[] {-1, -1, -1}};
            #endregion

            #region One Corner
            // 0000 0001
            StateTriangles[1] = new[] {new[] {6, 10, 11}};
            StateTriangles[255 - 1] = new[] {new[] {6, 10, 11}};
            // 0000 0010
            StateTriangles[2] = new[] {new[] {2, 3, 6}};
            StateTriangles[255 - 2] = new[] { new[] { 2, 3, 6 } };
            // 0000 0100
            StateTriangles[4] = new[] {new[] {5, 8, 11}};
            StateTriangles[255 - 4] = new[] { new[] { 5, 8, 11 } };
            // 0000 1000
            StateTriangles[8] = new[] {new[] {1, 2, 5}};
            StateTriangles[255 - 8] = new[] { new[] { 1, 2, 5 } };
            // 0001 0000
            StateTriangles[16] = new[] {new[] {7, 9, 10}};
            StateTriangles[255 - 16] = new[] { new[] { 7, 9, 10 } };
            // 0010 0000
            StateTriangles[32] = new[] {new[] {0, 3, 7}};
            StateTriangles[255 - 32] = new[] { new[] { 0, 3, 7 } };
            // 0100 0000
            StateTriangles[64] = new[] {new[] {4, 8, 9}};
            StateTriangles[255 - 64] = new[] { new[] { 4, 8, 9 } };
            // 1000 0000
            StateTriangles[128] = new[] {new[] {0, 1, 4}};
            StateTriangles[255 - 128] = new[] { new[] { 0, 1, 4 } };
            #endregion

            #region 2 Adjacent Corners
            // 0000 0011
            StateTriangles[3] = new[] {new[] {2, 3, 11}, new[] {3, 11, 10}};
            StateTriangles[255 - 3] = new[] { new[] { 2, 3, 11 }, new[] { 3, 11, 10 } };
            // 0000 0101
            StateTriangles[5] = new[] {new[] {5, 6, 8}, new[] {6, 8, 10}};
            StateTriangles[255 - 5] = new[] { new[] { 5, 6, 8 }, new[] { 6, 8, 10 } };
            // 0001 0001
            StateTriangles[17] = new[] {new[] {6, 7, 11}, new[] {7, 11, 9}};
            StateTriangles[255 - 17] = new[] { new[] { 6, 7, 11 }, new[] { 7, 11, 9 } };
            // 0000 1010
            StateTriangles[10] = new[] {new[] {1, 3, 5}, new[] {3, 5, 6}};
            StateTriangles[255 - 10] = new[] { new[] { 1, 3, 5 }, new[] { 3, 5, 6 } };
            // 0010 0010
            StateTriangles[34] = new[] {new[] {2, 0, 6}, new[] {0, 6, 7}};
            StateTriangles[255 - 34] = new[] { new[] { 2, 0, 6 }, new[] { 0, 6, 7 } };
            // 0000 1100
            StateTriangles[12] = new[] {new[] {1, 2, 8}, new[] {2, 8, 11}};
            StateTriangles[255 - 12] = new[] { new[] { 1, 2, 8 }, new[] { 2, 8, 11 } };
            // 0100 0100
            StateTriangles[68] = new[] {new[] {5, 4, 11}, new[] {4, 11, 9}};
            StateTriangles[255 - 68] = new[] {new[] {5, 4, 11}, new[] {4, 11, 9}};
            // 1000 1000
            StateTriangles[136] = new[] {new[] {0, 2, 4}, new[] {2, 4, 5}};
            StateTriangles[255 - 136] = new[] { new[] { 0, 2, 4 }, new[] { 2, 4, 5 } };
            // 0011 0000
            StateTriangles[48] = new[] {new[] {3, 0, 10}, new[] {0, 10, 9}};
            StateTriangles[255 - 48] = new[] { new[] { 3, 0, 10 }, new[] { 0, 10, 9 } };
            // 0101 0000
            StateTriangles[80] = new[] {new[] {4, 7, 8}, new[] {7, 8, 10}};
            StateTriangles[255 - 80] = new[] { new[] { 4, 7, 8 }, new[] { 7, 8, 10 } };
            // 1010 0000
            StateTriangles[160] = new[] {new[] {1, 3, 4}, new[] {3, 4, 7}};
            StateTriangles[255 - 160] = new[] { new[] { 1, 3, 4 }, new[] { 3, 4, 7 } };
            // 1100 0000
            StateTriangles[192] = new[] {new[] {1, 0, 8}, new[] {0, 8, 9}};
            StateTriangles[255 - 192] = new[] { new[] { 1, 0, 8 }, new[] { 0, 8, 9 } };
            #endregion

            #region 2 Planer Corners
            // 0000 1001
            StateTriangles[9] = new[] {new []{6, 10, 11}, new []{1, 2, 5}};
            StateTriangles[255 - 9] = new[] {new []{6, 10, 11}, new []{1, 2, 5}};
            // 0010 0001
            StateTriangles[33] = new[] {new[] {6, 10, 11}, new[] {0, 3, 7}};
            StateTriangles[255 - 33] = new[] {new[] {6, 10, 11}, new[] {0, 3, 7}};
            // 0100 0001
            StateTriangles[65] = new[] {new[] {6, 10, 11}, new[] {4, 8, 9}};
            StateTriangles[255 - 65] = new[] {new[] {6, 10, 11}, new[] {4, 8, 9}};
            // 0000 0110
            StateTriangles[6] = new[] {new[] {2, 3, 6}, new[] {11, 5, 8}};
            StateTriangles[255 - 6] = new[] {new[] {2, 3, 6}, new[] {11, 5, 8}};
            // 0001 0010
            StateTriangles[18] = new[] {new[] {2, 3, 6}, new[] {7, 9, 10}};
            StateTriangles[255 - 18] = new[] {new[] {2, 3, 6}, new[] {7, 9, 10}};
            // 1000 0010
            StateTriangles[130] = new[] {new[] {2, 3, 6}, new[] {0, 1, 4}};
            StateTriangles[255 - 130] = new[] {new[] {2, 3, 6}, new[] {0, 1, 4}};
            // 0001 0100
            StateTriangles[20] = new[] {new[] {11, 5, 8}, new[] {7, 9, 10}};
            StateTriangles[255 - 20] = new[] {new[] {11, 5, 8}, new[] {7, 9, 10}};
            // 1000 0100
            StateTriangles[132] = new[] {new[] {11, 5, 8}, new[] {0, 1, 4}};
            StateTriangles[255 - 132] = new[] {new[] {11, 5, 8}, new[] {0, 1, 4}};
            // 0010 1000
            StateTriangles[40] = new[] {new[] {1, 2, 5}, new[] {0, 3, 7}};
            StateTriangles[255 - 40] = new[] {new[] {1, 2, 5}, new[] {0, 3, 7}};
            // 0100 1000
            StateTriangles[72] = new[] {new[] {1, 2, 5}, new[] {4, 8, 9}};
            StateTriangles[255 - 72] = new[] {new[] {1, 2, 5}, new[] {4, 8, 9}};
            // 1001 0000
            StateTriangles[144] = new[] {new[] {7, 9, 10}, new[] {0, 1, 4}};
            StateTriangles[255 - 144] = new[] {new[] {7, 9, 10}, new[] {0, 1, 4}};
            // 0110 0000
            StateTriangles[96] = new[] {new[] {0, 3, 7}, new[] {4, 8, 9}};
            StateTriangles[255 - 96] = new[] {new[] {0, 3, 7}, new[] {4, 8, 9}};
            #endregion

            #region 2 Opposite Corners
            // 1000 0001
            StateTriangles[129] = new[] {new[] {6, 10, 11}, new[] {0, 1, 4}};
            StateTriangles[255 - 129] = new[] {new[] {6, 10, 11}, new[] {0, 1, 4}};
            // 0100 0010
            StateTriangles[66] = new[] {new[] {2, 3, 6}, new[] {4, 8, 9}};
            StateTriangles[255 - 66] = new[] {new[] {2, 3, 6}, new[] {4, 8, 9}};
            // 0010 0100
            StateTriangles[36] = new[] {new[] {5, 8, 11}, new[] {0, 3, 7}};
            StateTriangles[255 - 36] = new[] {new[] {5, 8, 11}, new[] {0, 3, 7}};
            // 0001 1000
            StateTriangles[24] = new[] {new[] {1, 2, 5}, new[] {7, 9, 10}};
            StateTriangles[255 - 24] = new[] {new[] {1, 2, 5}, new[] {7, 9, 10}};
            #endregion

            #region 3 Planer Corners
            // 0000 0111
            StateTriangles[7] = new[] {new[] {10, 3, 8}, new[] {3, 8, 2}, new[] {8, 2, 5}};
            StateTriangles[255 - 7] = new[] {new[] {10, 3, 8}, new[] {3, 8, 2}, new[] {8, 2, 5}};
            // 0001 0011
            StateTriangles[19] = new[] {new[] {11, 2, 9}, new[] {2, 9, 3}, new[] {9, 3, 7}};
            StateTriangles[255 - 19] = new[] {new[] {11, 2, 9}, new[] {2, 9, 3}, new[] {9, 3, 7}};
            // 0001 0101
            StateTriangles[21] = new[] {new[] {6, 5, 7}, new[] {5, 7, 8}, new[] {7, 8, 9}};
            StateTriangles[255 - 21] = new[] {new[] {6, 5, 7}, new[] {5, 7, 8}, new[] {7, 8, 9}};
            // 0000 1011
            StateTriangles[11] = new[] {new[] {3, 10, 1}, new[] {10, 1, 11}, new[] {1, 11, 5}};
            StateTriangles[255 - 11] = new[] {new[] {3, 10, 1}, new[] {10, 1, 11}, new[] {1, 11, 5}};
            // 0010 0011
            StateTriangles[35] = new[] {new[] {2, 0, 11}, new[] {0, 11, 7}, new[] {11, 7, 10}};
            StateTriangles[255 - 35] = new[] {new[] {2, 0, 11}, new[] {0, 11, 7}, new[] {11, 7, 10}};
            // 0010 1010
            StateTriangles[42] = new[] {new[] {6, 5, 7}, new[] {5, 7, 1}, new[] {7, 1, 0}};
            StateTriangles[255 - 42] = new[] {new[] {6, 5, 7}, new[] {5, 7, 1}, new[] {7, 1, 0}};
            // 0000 1101
            StateTriangles[13] = new[] {new[] {8, 1, 10}, new[] {1, 10, 2}, new[] {10, 2, 6}};
            StateTriangles[255 - 13] = new[] {new[] {8, 1, 10}, new[] {1, 10, 2}, new[] {10, 2, 6}};
            // 0100 0101
            StateTriangles[69] = new[] { new[] { 5, 6, 4 }, new[] { 6, 4, 10 }, new[] { 4, 10, 9 } };
            StateTriangles[255 - 69] = new[] { new[] { 5, 6, 4 }, new[] { 6, 4, 10 }, new[] { 4, 10, 9 } };
            // 0100 1100
            StateTriangles[76] = new[] {new[] {11, 2, 9}, new[] {2, 9, 1}, new[] {9, 1, 4}};
            StateTriangles[255 - 76] = new[] {new[] {11, 2, 9}, new[] {2, 9, 1}, new[] {9, 1, 4}};
            // 0000 1110
            StateTriangles[14] = new[] {new[] {1, 3, 8}, new[] {3, 8, 6}, new[] {8, 6, 11}};
            StateTriangles[255 - 14] = new[] {new[] {1, 3, 8}, new[] {3, 8, 6}, new[] {8, 6, 11}};
            // 1000 1010
            StateTriangles[138] = new[] {new[] {5, 6, 4}, new[] {6, 4, 3}, new[] {4, 3, 0}};
            StateTriangles[255 - 138] = new[] {new[] {5, 6, 4}, new[] {6, 4, 3}, new[] {4, 3, 0}};
            // 1000 1100
            StateTriangles[140] = new[] {new[] {2, 11, 0}, new[] {11, 0, 8}, new[] {0, 8, 4}};
            StateTriangles[255 - 140] = new[] {new[] {2, 11, 0}, new[] {11, 0, 8}, new[] {0, 8, 4}};
            // 0011 0001
            StateTriangles[49] = new[] {new[] {9, 11, 0}, new[] {11, 0, 6}, new[] {0, 6, 3}};
            StateTriangles[255 - 49] = new[] {new[] {9, 11, 0}, new[] {11, 0, 6}, new[] {0, 6, 3}};
            // 0101 0001
            StateTriangles[81] = new[] {new[] {7, 6, 4}, new[] {6, 4, 11}, new[] {4, 11, 8}};
            StateTriangles[255 - 81] = new[] {new[] {7, 6, 4}, new[] {6, 4, 11}, new[] {4, 11, 8}};
            // 0111 0000
            StateTriangles[112] = new[] {new[] {10, 3, 8}, new[] {3, 8, 0}, new[] {8, 0, 4}};
            StateTriangles[255 - 112] = new[] {new[] {10, 3, 8}, new[] {3, 8, 0}, new[] {8, 0, 4}};
            // 0011 0010
            StateTriangles[50] = new[] {new[] {0, 2, 9}, new[] {2, 9, 6}, new[] {9, 6, 10}};
            StateTriangles[255 - 50] = new[] {new[] {0, 2, 9}, new[] {2, 9, 6}, new[] {9, 6, 10}};
            // 1010 0010
            StateTriangles[162] = new[] {new[] {7, 6, 4}, new[] {6, 4, 2}, new[] {4, 2, 1}};
            StateTriangles[255 - 162] = new[] {new[] {7, 6, 4}, new[] {6, 4, 2}, new[] {4, 2, 1}};
            // 1011 0000
            StateTriangles[176] = new[] {new[] {3, 10, 1}, new[] {10, 1, 9}, new[] {1, 9, 4}};
            StateTriangles[255 - 176] = new[] {new[] {3, 10, 1}, new[] {10, 1, 9}, new[] {1, 9, 4}};
            // 0101 0100
            StateTriangles[84] = new[] {new[] {4, 5, 7}, new[] {5, 7, 11}, new[] {7, 11, 10}};
            StateTriangles[255 - 84] = new[] {new[] {4, 5, 7}, new[] {5, 7, 11}, new[] {7, 11, 10}};
            // 1100 0100
            StateTriangles[196] = new[] {new[] {9, 11, 0}, new[] {11, 0, 5}, new[] {0, 5, 1}};
            StateTriangles[255 - 196] = new[] {new[] {9, 11, 0}, new[] {11, 0, 5}, new[] {0, 5, 1}};
            // 1101 0000
            StateTriangles[208] = new[] {new[] {8, 10, 1}, new[] {10, 1, 7}, new[] {1, 7, 0}};
            StateTriangles[255 - 208] = new[] {new[] {8, 10, 1}, new[] {10, 1, 7}, new[] {1, 7, 0}};
            // 1010 1000
            StateTriangles[168] = new[] {new[] {4, 5, 7,}, new[] {5, 7, 2}, new[] {7, 2, 3}};
            StateTriangles[255 - 168] = new[] {new[] {4, 5, 7,}, new[] {5, 7, 2}, new[] {7, 2, 3}};
            // 1100 1000
            StateTriangles[200] = new[] {new[] {0, 2, 9}, new[] {2, 9, 5}, new[] {9, 5, 8}};
            StateTriangles[255 - 200] = new[] {new[] {0, 2, 9}, new[] {2, 9, 5}, new[] {9, 5, 8}};
            // 1110 0000
            StateTriangles[224] = new[] {new[] {1, 3, 8}, new[] {3, 8, 7}, new[] {8, 7, 9}};
            StateTriangles[255 - 224] = new[] {new[] {1, 3, 8}, new[] {3, 8, 7}, new[] {8, 7, 9}};
            #endregion

            #region Full Plane
            // 0011 0011
            StateTriangles[51] = new[] {new[] {2, 0, 11}, new[] {0, 11, 9}};
            StateTriangles[255 - 51] = new[] {new[] {2, 0, 11}, new[] {0, 11, 9}};
            // 0000 1111
            StateTriangles[15] = new[] {new[] {3, 10, 1}, new[] {10, 1, 8}};
            StateTriangles[255 - 15] = new[] {new[] {3, 10, 1}, new[] {10, 1, 8}};
            // 0101 0101
            StateTriangles[85] = new[] { new[] { 5, 4, 6 }, new[] { 4, 6, 7 } };
            StateTriangles[255 - 85] = new[] { new[] { 5, 4, 6 }, new[] { 4, 6, 7 } };
            #endregion

            #region Full Diagonal
            // 0001 0111
            StateTriangles[23] = new[] {new[] {2, 5, 3}, new[] {5, 3, 8}, new[] {3, 8, 7}, new[] {8, 7, 9}};
            StateTriangles[255 - 23] = new[] {new[] {2, 5, 3}, new[] {5, 3, 8}, new[] {3, 8, 7}, new[] {8, 7, 9}};
            // 0010 1011
            StateTriangles[43] = new[] {new[] {11, 5, 10}, new[] {5, 10, 1}, new[] {10, 1, 7}, new[] {1, 7, 0}};
            StateTriangles[255 - 43] = new[] {new[] {11, 5, 10}, new[] {5, 10, 1}, new[] {10, 1, 7}, new[] {1, 7, 0}};
            // 0100 1101
            StateTriangles[77] = new[] {new[] {2, 1, 6}, new[] {1, 6, 4}, new[] {6, 4, 10}, new[] {4, 10, 9}};
            StateTriangles[255 - 77] = new[] {new[] {2, 1, 6}, new[] {1, 6, 4}, new[] {6, 4, 10}, new[] {4, 10, 9}};
            // 1000 1110
            StateTriangles[142] = new[] {new[] {0, 4, 3}, new[] {4, 3, 8}, new[] {3, 8, 6}, new[] {8, 6, 11}};
            StateTriangles[255 - 142] = new[] {new[] {0, 4, 3}, new[] {4, 3, 8}, new[] {3, 8, 6}, new[] {8, 6, 11}};
            #endregion
        }

        private static readonly Vector3[] StateVertices =
        {
            new Vector3( 0.5f,  0.0f,  0.5f), // front right        0
            new Vector3( 0.0f,  0.5f,  0.5f), // front top          1
            new Vector3(-0.5f,  0.0f,  0.5f), // front left         2
            new Vector3( 0.0f, -0.5f,  0.5f), // front bottom       3

            new Vector3( 0.5f,  0.5f,  0.0f), // mid upper right    4
            new Vector3(-0.5f,  0.5f,  0.0f), // mid upper left     5
            new Vector3(-0.5f, -0.5f,  0.0f), // mid lower left     6
            new Vector3( 0.5f, -0.5f,  0.0f), // mid lower right    7

            new Vector3( 0.0f,  0.5f, -0.5f), // back top           8
            new Vector3( 0.5f,  0.0f, -0.5f), // back right         9
            new Vector3( 0.0f, -0.5f, -0.5f), // back bottom        10
            new Vector3(-0.5f,  0.0f, -0.5f)  // back left          11
        };
        /*
        data[x]     [y]     [z]         != Voxel.Empty ? 1 : 0)
        data[x]     [y]     [z + 1]     != Voxel.Empty ? 1 : 0) << 1
        data[x]     [y + 1] [z]         != Voxel.Empty ? 1 : 0) << 2
        data[x]     [y + 1] [z + 1]     != Voxel.Empty ? 1 : 0) << 3
        data[x + 1] [y]     [z]         != Voxel.Empty ? 1 : 0) << 4
        data[x + 1] [y]     [z + 1]     != Voxel.Empty ? 1 : 0) << 5
        data[x + 1] [y + 1] [z]         != Voxel.Empty ? 1 : 0) << 6
        data[x + 1] [y + 1] [z + 1]     != Voxel.Empty ? 1 : 0) << 7;
        */
        private static readonly int[][][] StateTriangles;

        /// <summary>
        /// Turns Voxel data into a VBO surface
        /// </summary>
        /// <param name="data">the voxels</param>
        /// <param name="size">how big it is in local frame</param>
        /// <returns>new int[] {vb, ib, cb}</returns>
        public static int[] March(Voxel[,,] data, float size, string name, out int indexCount)
        {
            var vertices = new List<Vertex>();
            var indices = new List<uint>();
            var colors = new List<Color>();

            var t = -size / 2f;
            var xi = size / data.GetLength(0);
            var yi = size / data.GetLength(1);
            var zi = size / data.GetLength(2);

            Action<Vertex, Vertex, Vertex> makeTri
                = (v0, v1, v2) =>
                {
                    //var i = -1;
                    //if ((i = vertices.FindIndex(v => Math.Abs(v.X - v0.X) < xi / 4f && Math.Abs(v.Y - v0.Y) < yi / 4f && Math.Abs(v.Z - v0.Z) < zi / 4f)) != -1)
                    //{
                    //    indices.Add((uint)i);
                    //    colors[i] = colors[i].Blend(c0);
                    //}
                    //else
                    //{
                        indices.Add((uint)vertices.Count);
                        vertices.Add(v0);
                    //}
                    //if ((i = vertices.FindIndex(v => Math.Abs(v.X - v1.X) < xi / 4f && Math.Abs(v.Y - v1.Y) < yi / 4f && Math.Abs(v.Z - v1.Z) < zi / 4f)) != -1)
                    //{
                    //    indices.Add((uint)i);
                    //    colors[i] = colors[i].Blend(c1);
                    //}
                    //else
                    //{
                        indices.Add((uint)vertices.Count);
                        vertices.Add(v1);
                    //}
                    //if ((i = vertices.FindIndex(v => Math.Abs(v.X - v2.X) < xi / 4f && Math.Abs(v.Y - v2.Y) < yi / 4f && Math.Abs(v.Z - v2.Z) < zi / 4f)) != -1)
                    //{
                    //    indices.Add((uint) i);
                    //    colors[i] = colors[i].Blend(c2);
                    //}
                    //else
                    //{
                        indices.Add((uint) vertices.Count);
                        vertices.Add(v2);
                    //}
                };

            var rand = new Random();
            for (var x = 0; x < data.GetLength(0) - 1; x++)
            {
                for (var y = 0; y < data.GetLength(1) - 1; y++)
                {
                    for (var z = 0; z < data.GetLength(2) - 1; z++)
                    {
                        var state =
                            (data[x, y, z].IsSolid() ? 1 : 0)
                            | (data[x, y, z + 1].IsSolid() ? 1 : 0) << 1
                            | (data[x, y + 1, z].IsSolid() ? 1 : 0) << 2
                            | (data[x, y + 1, z + 1].IsSolid() ? 1 : 0) << 3
                            | (data[x + 1, y, z].IsSolid() ? 1 : 0) << 4
                            | (data[x + 1, y, z + 1].IsSolid() ? 1 : 0) << 5
                            | (data[x + 1, y + 1, z].IsSolid() ? 1 : 0) << 6
                            | (data[x + 1, y + 1, z + 1].IsSolid() ? 1 : 0) << 7;
                        var tris = StateTriangles[state];
                        if (state == 0 || state == 255 || tris == null)
                            continue;

                        var cpos = new Vector3(t + x * xi + xi / 2f, t + y * yi + yi / 2f, t + z * zi + zi / 2f);
                        var c = AvgNotTransparent(
                            data[x, y, z].GetColor().MutateColor(50, rand),
                            data[x, y, z + 1].GetColor().MutateColor(50, rand),
                            data[x, y + 1, z].GetColor().MutateColor(50, rand),
                            data[x, y + 1, z + 1].GetColor().MutateColor(50, rand),
                            data[x + 1, y, z].GetColor().MutateColor(50, rand),
                            data[x + 1, y, z + 1].GetColor().MutateColor(50, rand),
                            data[x + 1, y + 1, z].GetColor().MutateColor(50, rand),
                            data[x + 1, y + 1, z + 1].GetColor().MutateColor(50, rand));
                        var n = data[x, y, z].Normal;
                        for (var i = 0; i < tris.Length; i++)
                        {
                            var tri = tris[i];
                            var v0 = new Vertex(new Vector3(StateVertices[tri[0]].X * xi, StateVertices[tri[0]].Y * yi, StateVertices[tri[0]].Z * zi) + cpos, c.AsVector4(), n, new Vector2(0, 1));
                            var v1 = new Vertex(new Vector3(StateVertices[tri[1]].X * xi, StateVertices[tri[1]].Y * yi, StateVertices[tri[1]].Z * zi) + cpos, c.AsVector4(), n, new Vector2(0, 0));
                            var v2 = new Vertex(new Vector3(StateVertices[tri[2]].X * xi, StateVertices[tri[2]].Y * yi, StateVertices[tri[2]].Z * zi) + cpos, c.AsVector4(), n, new Vector2(1, 1));
                            makeTri(v0, v1, v2);
                        }
                    }
                }
            }

            var ids = new[] {0, 0, 0};
            ids[0] = BufferLibrary.CreateVertexBuffer(name + ":v", vertices.ToArray());
            ids[1] = BufferLibrary.CreateIndexBuffer(name + ":i", indices.ToArray(), PrimitiveType.Triangles);

            indexCount = indices.Count;
            return ids;
        }

        private static Color MutateColor(this Color c, int amount, Random r)
        {
            var m = new[] { r.Next(amount * 2) - amount, r.Next(amount * 2) - amount, r.Next(amount * 2) - amount };
            return Color.FromArgb(
                c.R + m[0] < 0 ? 0 : c.R + m[0] > 255 ? 255 : c.R + m[0],
                c.G + m[1] < 0 ? 0 : c.G + m[1] > 255 ? 255 : c.G + m[1],
                c.B + m[2] < 0 ? 0 : c.B + m[2] > 255 ? 255 : c.B + m[2]);
        }

        private static Vector4 AsVector4(this Color c, float alpha = 1.0f)
        {
            return new Vector4(c.R/255f, c.G/255f, c.B/255f, alpha);
        }

        public static Color Blend(this Color t, params Color[] others)
        {
            return Color.FromArgb(
                (t.A + others.Sum(c => c.A)) / (1 + others.Length),
                (t.R + others.Sum(c => c.R)) / (1 + others.Length),
                (t.G + others.Sum(c => c.G)) / (1 + others.Length),
                (t.B + others.Sum(c => c.B)) / (1 + others.Length));
        }

        public static Color AvgNotTransparent(params Color[] colors)
        {
            var cs = colors.Where(c => c != Color.Transparent).ToList();
            return Color.FromArgb(
                    cs.Sum(c => c.R) / cs.Count,
                    cs.Sum(c => c.G) / cs.Count,
                    cs.Sum(c => c.B) / cs.Count
                );
        }
    }
}

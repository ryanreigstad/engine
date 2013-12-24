using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace engine.entities
{
    public class HeightMap : Entity
    {
        public HeightMap(Vector3 position, Quaternion rotation, Vector3 scale, float[][] data)
            : base(position, rotation, scale)
        {
            Data = data;
        }

        public float[][] Data { get; set; }

        public override void Render()
        {
            GL.PushMatrix();
            ApplyTransform();

            var t = -0.5f;
            var xi = 1f / Data.Length;
            var zi = 1f / Data[0].Length;

            for (var x = 0; x < Data.Length; x++)
            {
                GL.Begin(PrimitiveType.LineStrip);
                {
                    for (var z = 0; z < Data[x].Length; z++)
                    {
                        var y = Data[x][z];
                        GL.Color3(Colors[GetColor(y)]);
                        GL.Vertex3(t + x * xi, y, t + z * zi);
                    }
                }
                GL.End();

                GL.Begin(PrimitiveType.LineStrip);
                {
                    for (var z = 0; z < Data[x].Length; z++)
                    {
                        var y = Data[z][x];
                        GL.Color3(Colors[GetColor(y)]);
                        GL.Vertex3(t + z * xi, y, t + x * zi);
                    }
                }
                GL.End();
            }
            GL.PopMatrix();
        }

        private static readonly Color[] Colors = { Color.DarkBlue, Color.Blue, Color.SandyBrown, Color.Green, Color.LightGreen, Color.Gray, Color.White };
        private int GetColor(float f)
        {
            if (f < -0.5f)
                return 0;
            if (f < -0.3f)
                return 1;
            if (f < -0.1f)
                return 2;
            if (f < 0.5f)
                return 3;
            if (f < 0.7f)
                return 4;
            if (f < 0.9f)
                return 5;
            return 6;
        }
    }
}

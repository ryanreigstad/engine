using System.Collections.Generic;
using engine.entities;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace engine.Libraries
{
    public static class PrimitiveFactory
    {
        public static VboEntity BuildCube(Vector3 position, Quaternion rotation, Vector3 scale, Color4 color)
        {
            var name = "cube";
            if (!BufferLibrary.HasBuffer(name + "_" + color.ToArgb() + ":v"))
            {
                BufferLibrary.CreateVertexBuffer(
                    name + "_" + color.ToArgb() + ":v", new[]
                    {
                        new Vertex(new Vector3(1f, 1f, 1f)  , new Vector4(1, 0, 0, 1f), new Vector3(0.5f, 0.5f, 0.5f), new Vector2(0, 1)),
                        new Vertex(new Vector3(-1f, 1f, 1f) , new Vector4(0, 1, 0, 1f), new Vector3(-0.5f, 0.5f, 0.5f), new Vector2(1, 1)),
                        new Vertex(new Vector3(-1f, 1f, -1f), new Vector4(0, 0, 1, 1f), new Vector3(-0.5f, 0.5f, -0.5f), new Vector2(1, 0)),
                        new Vertex(new Vector3(1f, 1f, -1f) , new Vector4(1, 0, 1, 1f), new Vector3(0.5f, 0.5f, -0.5f), new Vector2(0, 0)),
                        new Vertex(new Vector3(1f, -1f, 1f), new Vector4(1, 0, 0, 1f), new Vector3(0.5f, -0.5f, 0.5f), new Vector2(0, 1)),
                        new Vertex(new Vector3(-1f, -1f, 1f), new Vector4(0, 1, 0, 1f), new Vector3(-0.5f, -0.5f, 0.5f), new Vector2(1, 1)),
                        new Vertex(new Vector3(-1f, -1f, -1f), new Vector4(0, 0, 1, 1f), new Vector3(-0.5f, -0.5f, -0.5f), new Vector2(1, 0)),
                        new Vertex(new Vector3(1f, -1f, -1f), new Vector4(1, 0, 1, 1f), new Vector3(0.5f, -0.5f, -0.5f), new Vector2(0, 0))
                    });
            }
            if (!BufferLibrary.HasBuffer(name + ":i"))
            {
                BufferLibrary.CreateIndexBuffer(name + ":i", new uint[]
                    {
                        0, 1, 2, 0, 2, 3, 4, 5, 6, 4, 6, 7, //top 2 squares
                        1, 0, 4, 1, 5, 4, 2, 1, 5, 2, 5, 6 // 2 sides adjacent from eachother
                    }, PrimitiveType.Triangles);
            }
            return new VboEntity(position, rotation, scale, BufferLibrary.GetBuffer(name + "_" + color.ToArgb() + ":v"), BufferLibrary.GetBuffer(name + ":i"), BufferLibrary.GetBuffer(name + ":i#count"), (PrimitiveType)BufferLibrary.GetBuffer(name + ":i#mode"));
        }

        public static VboEntity BuildPlane(Vector3 position, Quaternion rotation, Vector3 scale, Color4 color, int xd, int yd)
        {
            var name = "plane_" + xd + "," + yd;
            if (!BufferLibrary.HasBuffer(name + "_" + color.ToArgb() + ":v"))
            {
                var verts = new List<Vertex>();

                for (var x = 0; x <= xd; x++)
                {
                    for (var y = 0; y <= yd; y++)
                    {
                        verts.Add(new Vertex(new Vector3((x - xd / 2f) / (xd / 2f), 0, (y - yd / 2f) / (yd / 2f)), new Vector4(0, 1, 0, 1), Vector3.UnitY, new Vector2(x/xd, y/yd)));
                    }
                }

                BufferLibrary.CreateVertexBuffer(name + "_" + color.ToArgb() + ":v", verts.ToArray());
            }
            if (!BufferLibrary.HasBuffer(name + ":i"))
            {
                var indices = new List<uint>();

                for (var x = 0; x < xd; x++)
                {
                    for (var y = 0; y < yd; y++)
                    {
                        indices.Add((uint) (x * (yd + 1) + y));
                        indices.Add((uint) (x * (yd + 1) + y + 1));
                        indices.Add((uint) ((x + 1) * (yd + 1) + y + 1));
                        indices.Add((uint) ((x + 1) * (yd + 1) + y));
                    }
                }

                BufferLibrary.CreateIndexBuffer(name + ":i", indices.ToArray(), PrimitiveType.Quads);
            }
            return new VboEntity(position, rotation, scale, BufferLibrary.GetBuffer(name + "_" + color.ToArgb() + ":v"), BufferLibrary.GetBuffer(name + ":i"), BufferLibrary.GetBuffer(name + ":i#count"), (PrimitiveType)BufferLibrary.GetBuffer(name + ":i#mode"));
        }
    }
}

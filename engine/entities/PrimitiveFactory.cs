using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace engine.entities
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
                        new Vertex(new Vector3(-0.5f, -0.5f, 0.5f), color, new Vector3(-0.5f, -0.5f, 0.5f)),
                        new Vertex(new Vector3(0.5f, -0.5f, 0.5f), color, new Vector3(0.5f, -0.5f, 0.5f)),
                        new Vertex(new Vector3(0.5f, 0.5f, 0.5f), color, new Vector3(0.5f, 0.5f, 0.5f)),
                        new Vertex(new Vector3(-0.5f, 0.5f, 0.5f), color, new Vector3(-0.5f, 0.5f, 0.5f)),
                        new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), color, new Vector3(-0.5f, -0.5f, -0.5f)),
                        new Vertex(new Vector3(0.5f, -0.5f, -0.5f), color, new Vector3(0.5f, -0.5f, -0.5f)),
                        new Vertex(new Vector3(0.5f, 0.5f, -0.5f), color, new Vector3(0.5f, 0.5f, -0.5f)),
                        new Vertex(new Vector3(-0.5f, 0.5f, -0.5f), color, new Vector3(-0.5f, 0.5f, -0.5f))
                    });
            }
            if (!BufferLibrary.HasBuffer(name + ":i"))
            {
                BufferLibrary.CreateIndexBuffer(name + ":i", new uint[]
                    {
                        0, 1, 2, 2, 3, 0, 
			            3, 2, 6, 6, 7, 3, 
			            7, 6, 5, 5, 4, 7, 
			            4, 0, 3, 3, 7, 4, 
			            0, 1, 5, 5, 4, 0,
			            1, 5, 6, 6, 2, 1
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
                        verts.Add(new Vertex(new Vector3((x - xd / 2f) / (xd / 2f), 0, (y - yd / 2f) / (yd / 2f)), color, Vector3.UnitY));
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

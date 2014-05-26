using System;
using System.Collections.Generic;

using engine.data;

using OpenTK.Graphics.OpenGL4;

namespace engine.core.resources
{
    public static class BufferLibrary
    {
        private static readonly Dictionary<string, int> Items = new Dictionary<string, int>();

        private static bool HasBuffer(string name)
        {
            return Items.ContainsKey(name);
        }

        public static int GetBufferVerts(string name)
        {
            return HasBuffer(name) ? Items[name] : 0;
        }

        public static int GetBufferIndices(string name)
        {
            return HasBuffer(name + "#i") ? Items[name + "#i"] : 0;
        }

        public static int GetBufferCount(string name)
        {
            return HasBuffer(name + "#i_count") ? Items[name + "#i_count"] : 0;
        }

        public static int GetBufferMode(string name)
        {
            return HasBuffer(name + "#i_mode") ? Items[name + "#i_mode"] : 0;
        }

        private static void RegisterBuffer(string name, int id)
        {
            Items[name] = id;
        }

        public static int CreateVertexBuffer(string name, Vertex[] vertices)
        {
            var id = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, id);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr) (Vertex.SizeInBytes * vertices.Length), vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            RegisterBuffer(name, id);

            return id;
        }

        public static int CreateIndexBuffer(string name, uint[] indices, PrimitiveType mode)
        {
            var id = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, id);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr) (sizeof(uint) * indices.Length), indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            RegisterBuffer(name + "#i", id);
            RegisterBuffer(name + "#i_count", indices.Length);
            RegisterBuffer(name + "#i_mode", (int) mode);

            return id;
        }
    }
}

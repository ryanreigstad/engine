using System;
using System.Collections.Generic;

using OpenTK.Graphics.OpenGL;

namespace engine.Libraries
{
    public static class BufferLibrary
    {
        private static readonly Dictionary<string, int> Items = new Dictionary<string, int>();

        public static bool HasBuffer(string name)
        {
            return Items.ContainsKey(name);
        }

        public static void RegisterBuffer(string name, int id)
        {
            Items[name] = id;
        }

        public static int CreateVertexBuffer(string name, Vertex[] vertices)
        {
            var isNormalized = true;

            var id = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, id);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(Vertex.SizeInBytes * vertices.Length), vertices, BufferUsageHint.StaticDraw);
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            RegisterBuffer(name, id);

            return id;
        }

        public static int CreateIndexBuffer(string name, uint[] indices, PrimitiveType mode)
        {
            var id = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, id);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(sizeof(uint) * indices.Length), indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            RegisterBuffer(name, id);
            RegisterBuffer(name + "#count", indices.Length);
            RegisterBuffer(name + "#mode", (int) mode);

            return id;
        }

        public static int GetBuffer(string name)
        {
            return HasBuffer(name) ? Items[name] : 0;
        }
    }
}

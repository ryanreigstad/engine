using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace engine
{
    public static class BufferLibrary
    {
        private static Dictionary<string, int> _items = new Dictionary<string, int>();

        public static bool HasBuffer(string name)
        {
            return _items.ContainsKey(name);
        }

        public static bool HasBuffer(int id)
        {
            return _items.ContainsValue(id);
        }

        public static void RegisterBuffer(string name, int id)
        {
            _items[name] = id;
        }

        public static int CreateVertexBuffer(string name, Vector3[] vertices)
        {
            int id;

            GL.GenBuffers(1, out id);
            GL.BindBuffer(BufferTarget.ArrayBuffer, id);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * Vector3.SizeInBytes), vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            RegisterBuffer(name, id);

            return id;
        }

        public static int CreateIndexBuffer(string name, uint[] indices)
        {
            int id;
            GL.GenBuffers(1, out id);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, id);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indices.Length * sizeof(uint)), indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            RegisterBuffer(name, id);

            return id;
        }

        public static int CreateNormalBuffer(string name, Vector3[] normals)
        {
            int id;
            GL.GenBuffers(1, out id);
            GL.BindBuffer(BufferTarget.ArrayBuffer, id);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(normals.Length * Vector3.SizeInBytes), normals, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            RegisterBuffer(name, id);

            return id;
        }

        public static int CreateColorBuffer(string name, int[] colors)
        {
            int id;
            GL.GenBuffers(1, out id);
            GL.BindBuffer(BufferTarget.ArrayBuffer, id);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(colors.Length * sizeof(int)), colors, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            RegisterBuffer(name, id);

            return id;
        }

        public static int GetBuffer(string name)
        {
            return HasBuffer(name) ? _items[name] : 0;
        }
    }

    public static class BufferLibraryHelper
    {
        public static int ToRgba(this Color c)
        {
            return (c.A << 24) | (c.B << 16) | (c.G << 8) | c.R;
        }
    }
}

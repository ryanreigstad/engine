using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using engine.data;

using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace engine.graphics
{
    /// <summary>
    ///     TODO: this should really probably take in a list of vertices and have a method to generate the buffers
    ///     also, with the advent of loading models from files, we also need mesh subgroups
    /// </summary>
    public class Mesh
    {
        public Mesh(int vbo, int ibo, int count, PrimitiveType mode)
        {
            VboId = vbo;
            IboId = ibo;
            Count = count;
            Mode = mode;
        }

        public int VboId { get; set; }
        public int IboId { get; set; }
        public int Count { get; set; }
        public PrimitiveType Mode { get; set; }
    }
}

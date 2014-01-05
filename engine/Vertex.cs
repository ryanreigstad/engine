using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace engine
{
    public struct Vertex
    {
        public Vertex(Vector3 position, Color color)
        {
            Position = position;
            Color = color;
        }

        public Vector3 Position;
        public Color Color;
    }
}

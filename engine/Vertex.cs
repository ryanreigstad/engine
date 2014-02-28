using System.Runtime.InteropServices;

using OpenTK;
using OpenTK.Graphics;

namespace engine
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex
    {
        public Vertex(Vector3 position, Vector4 color, Vector3 normal, Vector2 textureCoords)
        {
            Position = position;
            Color = color;
            Normal = normal;
            TextureCoords = textureCoords;
        }

        public Vector3 Position;
        public Vector4 Color;
        public Vector3 Normal;
        public Vector2 TextureCoords;

        public const int SizeInBytes = sizeof(float) * 12;
    }
}

using System.Runtime.InteropServices;

using OpenTK;

namespace engine.data
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex
    {
        public const int SizeInBytes = sizeof(float) * 8;

        public Vertex(Vector3 position, Vector3 normal, Vector2 textureCoords)
        {
            Position = position;
            Normal = normal;
            TextureCoords = textureCoords;
        }

        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 TextureCoords;
    }
}

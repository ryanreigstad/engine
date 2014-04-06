using System.Drawing;

using OpenTK;

namespace engine.data
{
    public struct Voxel
    {
        public static readonly Voxel Empty = new Voxel(-1.0, VoxelType.None, Vector3.Zero);

        public readonly double Density;
        public readonly Vector3 Normal;
        public readonly VoxelType Type;

        public Voxel(double density, VoxelType type, Vector3 normal)
        {
            Density = density;
            Type = type;
            Normal = normal;
        }

        public bool IsSolid()
        {
            return Density > 0;
        }
    }

    public enum VoxelType
    {
        None = 0,
        DarkGray,
        Blue,
    }
}

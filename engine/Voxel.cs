using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;

namespace engine
{
    public struct Voxel
    {
        public static readonly Voxel Empty = new Voxel(-1.0, VoxelType.None, Vector3.UnitY);

        public Voxel(double density, VoxelType type, Vector3 normal)
        {
            Density = density;
            Type = type;
            Normal = normal;
        }

        public readonly double Density;
        public readonly VoxelType Type;
        public readonly Vector3 Normal;

        public bool IsSolid()
        {
            return Density <= 0;
        }

        public Color GetColor()
        {
            return IsSolid() ? Color.Transparent : Color.FromName(Type.ToString());
        }
    }

    public enum VoxelType
    {
        None = 0,
        DarkGray,
        Blue,
    }
}

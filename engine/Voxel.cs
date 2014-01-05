using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace engine
{
    public struct Voxel
        : IEquatable<Voxel>
    {
        public static readonly Voxel Empty = new Voxel(Color.Transparent);

        public Voxel(Color color)
        {
            Color = color;
        }

        public Color Color;

        public bool Equals(Voxel other)
        {
            return Color.Equals(other.Color);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is Voxel && Equals((Voxel)obj);
        }

        public override int GetHashCode()
        {
            return Color.GetHashCode();
        }

        public static bool operator ==(Voxel left, Voxel right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Voxel left, Voxel right)
        {
            return !(left == right);
        }
    }
}

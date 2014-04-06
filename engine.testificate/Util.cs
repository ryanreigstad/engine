using OpenTK;
using OpenTK.Graphics;

namespace engine
{
    public static class Util
    {
        public static Vector4 AsVector4(this Color4 color)
        {
            return new Vector4(color.R, color.G, color.B, color.A);
        }
    }
}

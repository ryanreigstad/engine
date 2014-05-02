using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace engine.graphics
{
    public class FullScreenQuad : Entity
    {
        public const string Mesh = "plane.obj";

        public FullScreenQuad(string textureName)
            : base(Mesh, textureName)
        {
            MoveLocal(Vector3.UnitZ * -0.1f);
            RotateX((float) (Math.PI / 2.0f));
        }
    }
}

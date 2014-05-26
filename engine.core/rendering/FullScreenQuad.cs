using System;

using engine.core.entities;

using OpenTK;

namespace engine.core.rendering
{
    public class FullScreenQuad : Entity
    {
        private const string Mesh = "plane.obj";

        public static readonly Matrix4 StaticTransform = Matrix4.CreateRotationX((float)Math.PI / 2.0f)
                                                     * Matrix4.CreateTranslation(Vector3.UnitZ * -0.1f);

        public FullScreenQuad(string textureName)
            : base(Mesh, textureName)
        {
        }

        public new Matrix4 Transform
        {
            get { return StaticTransform; }
        }
    }
}

﻿using engine.core.rendering;

using OpenTK;

namespace engine.core.entities
{
    public abstract class Light : Entity
    {
        protected Light(string meshName, Vector3 specularity, Vector3 diffuse)
            : this(meshName, specularity, diffuse, Vector3.Zero, Quaternion.Identity, Vector3.One)
        {
        }

        protected Light(string meshName, Vector3 specularity, Vector3 diffuse, Vector3 position, Quaternion rotation, Vector3 scale)
            : base(meshName, null, position, rotation, scale)
        {
            Specularity = specularity;
            Diffuse = diffuse;
        }

        public Vector3 Specularity { get; set; }
        public Vector3 Diffuse { get; set; }
    }

    public class PointLight : Light
    {
        private const string Mesh = "sphere3.obj";

        public PointLight(Vector3 specularity, Vector3 diffuse)
            : base(Mesh, specularity, diffuse)
        {
        }

        public PointLight(Vector3 specularity, Vector3 diffuse, Vector3 position, Quaternion rotation, Vector3 scale)
            : base(Mesh, specularity, diffuse, position, rotation, scale)
        {
        }
    }

    public class AmbientLight : Light
    {
        private const string Mesh = "plane.obj";

        public AmbientLight(Vector3 specularity, Vector3 diffuse)
            : base(Mesh, specularity, diffuse)
        {
        }

        public AmbientLight(Vector3 specularity, Vector3 diffuse, Vector3 position, Quaternion rotation, Vector3 scale)
            : base(Mesh, specularity, diffuse, position, rotation, scale)
        {
        }

        public new Matrix4 Transform
        {
            get { return FullScreenQuad.StaticTransform; }
        }
    }
}

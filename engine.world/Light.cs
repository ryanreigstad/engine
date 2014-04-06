using OpenTK;

namespace engine.world
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

    public class SphereLight : Light
    {
        private const string SphereMesh = "sphere2.obj";

        public SphereLight(Vector3 specularity, Vector3 diffuse)
            : base(SphereMesh, specularity, diffuse)
        {
        }

        public SphereLight(Vector3 specularity, Vector3 diffuse, Vector3 position, Quaternion rotation, Vector3 scale)
            : base(SphereMesh, specularity, diffuse, position, rotation, scale)
        {
        }
    }
}

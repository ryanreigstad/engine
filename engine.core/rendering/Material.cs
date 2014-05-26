using OpenTK;

namespace engine.core.rendering
{
    public class Material
    {
        public Vector3 AmbientColor; // Ka in .mtl
        public Vector3 DiffuseColor; // Kd in .mtl
        public Vector3 SpecularColor; // Ks in .mtl

        public float Specularity; // Ns in .mtl (between 0 and 1000)
    }
}

using System.Collections.Generic;

using engine.data;
using engine.graphics;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace engine.testificate
{
    public static class PrimitiveFactory
    {
        public static Entity Cube()
        {
            return new Entity("cube.obj", null);
        }
    }
}

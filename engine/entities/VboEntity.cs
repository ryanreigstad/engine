using System;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace engine.entities
{
    public class VboEntity : Entity
    {
        public VboEntity(Vector3 position, Quaternion rotation, Vector3 scale, int vbo, int ibo, int count, PrimitiveType mode)
            : base(position, rotation, scale)
        {
            _vbo = vbo;
            _ibo = ibo;
            _count = count;
            _mode = mode;
        }

        private readonly int _vbo;
        private readonly int _ibo;
        private readonly int _count;
        private readonly PrimitiveType _mode;

        public override void Render()
        {
            if (_vbo == 0 || _ibo == 0)
                return;

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.VertexPointer(3, VertexPointerType.Float, Vertex.SizeInBytes, (IntPtr)0);
            GL.ColorPointer(4, ColorPointerType.Float, Vertex.SizeInBytes, (IntPtr)(sizeof(float) * 3));

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ibo);

            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.ColorArray);
            GL.EnableClientState(ArrayCap.IndexArray);

            GL.PushMatrix();
            ApplyTransform();

            GL.DrawRangeElements(_mode, 0, _count, _count, DrawElementsType.UnsignedInt, IntPtr.Zero);

            GL.PopMatrix();

            GL.DisableClientState(ArrayCap.VertexArray);
            GL.DisableClientState(ArrayCap.ColorArray);
            GL.DisableClientState(ArrayCap.IndexArray);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }
    }
}

using System;
using System.Net.Configuration;
using engine.Libraries;
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

        public readonly int _vbo;
        public readonly int _ibo;
        public readonly int _count;
        public readonly PrimitiveType _mode;
        public int TextureId { get; set; }

        public int Mvp { get; set; }
        public int Model { get; set; }
        public Matrix4 Vp { get; set; }
        public int ModelRotation { get; set; }

        public override void Render()
        {
            if (_vbo == 0 || _ibo == 0)
                return;

            //bind to the buffers
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ibo);


            var isNormalized = true;

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, !isNormalized, Vertex.SizeInBytes, (IntPtr)0);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, !isNormalized, Vertex.SizeInBytes, (IntPtr)(sizeof(float) * 3));
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, isNormalized, Vertex.SizeInBytes, (IntPtr)(sizeof(float) * 7));
            GL.EnableVertexAttribArray(3);
            GL.VertexAttribPointer(3, 2, VertexAttribPointerType.Float, !isNormalized, Vertex.SizeInBytes, (IntPtr)(sizeof(float) * 10)); 



            GL.TexCoordPointer(2, TexCoordPointerType.Float, Vertex.SizeInBytes, (IntPtr)(sizeof(float) * 10));

            //enable shit
            GL.EnableClientState(ArrayCap.TextureCoordArray);

            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            GL.BindTexture(TextureTarget.Texture2D, TextureId);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);

            var m = GetModelMatrix();
            var rot = Matrix4.CreateFromQuaternion(Rotation);
            var mM = m*Vp;
            GL.UniformMatrix4(Mvp, false, ref mM);

            GL.UniformMatrix4(Model, false, ref m);

            GL.UniformMatrix4(ModelRotation, false, ref rot);

            //begin the draw by pushing the transform matrix
            GL.PushMatrix();
            ApplyTransform();
            //draw stuff
            GL.DrawRangeElements(_mode, 0, _count, _count, DrawElementsType.UnsignedInt, IntPtr.Zero);
            //pop the matrix
            GL.PopMatrix();

            //disable shit
            GL.DisableClientState(ArrayCap.TextureCoordArray);

            //dont forget to flush your shit
            GL.Flush();
            //bind to empty positions to reset the buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }
    }
}

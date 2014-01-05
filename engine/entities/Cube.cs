using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace engine.entities
{
    public class Cube : Entity
    {
        public Cube(Vector3 position, Quaternion rotation, Vector3 scale, Color color)
            : base(position, rotation, scale)
        {
            Color = color;
        }

        public Color Color { get; set; }

        public override void Load()
        {
            if (!BufferLibrary.HasBuffer("vCube"))
            {
                const float u = 0.49f, uf = 0.5f;
                BufferLibrary.CreateVertexBuffer(
                    "vCube",
                    new[]
                    {
                        new Vector3(-u, -u,  u),
                        new Vector3( u, -u,  u),
                        new Vector3( u,  u,  u),
                        new Vector3(-u,  u,  u),
                        new Vector3(-u, -u, -u),
                        new Vector3( u, -u, -u),
                        new Vector3( u,  u, -u),
                        new Vector3(-u,  u, -u),
                        
                        //new Vector3(-uf, -uf,  uf),
                        //new Vector3( uf, -uf,  uf),
                        //new Vector3( uf,  uf,  uf),
                        //new Vector3(-uf,  uf,  uf),
                        //new Vector3(-uf, -uf, -uf),
                        //new Vector3( uf, -uf, -uf),
                        //new Vector3( uf,  uf, -uf),
                        //new Vector3(-uf,  uf, -uf)
                    });
            }
            if (!BufferLibrary.HasBuffer("iCube"))
            {
                BufferLibrary.CreateIndexBuffer("iCube", 
                    new uint[]
                    {
				        0, 1, 2, 2, 3, 0, 
				        3, 2, 6, 6, 7, 3, 
				        7, 6, 5, 5, 4, 7, 
				        4, 0, 3, 3, 7, 4, 
				        0, 1, 5, 5, 4, 0,
				        1, 5, 6, 6, 2, 1,

                        //8, 9, 9, 10, 10, 11, 11, 8,
                        //12, 13, 13, 14, 14, 15, 15, 12,
                        //8, 12, 9, 13, 10, 14, 11, 15
                    });
            }
            if (!BufferLibrary.HasBuffer("cCube" + Color.ToRgba()))
            {
                BufferLibrary.CreateColorBuffer(
                    "cCube" + Color.ToRgba(),
                    Enumerable.Repeat(Color.ToRgba(), 8)/*.Concat(Enumerable.Repeat(Color.Black.ToRgba(), 8))*/.ToArray());
            }
        }

        public override void Render()
        {
            int vc = BufferLibrary.GetBuffer("vCube"),
                ic = BufferLibrary.GetBuffer("iCube"),
                cc = BufferLibrary.GetBuffer("cCube" + Color.ToRgba());

            if (vc == 0 || ic == 0)
                return;

            // Color Buffer
            if (cc != 0)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, cc);
                GL.ColorPointer(4, ColorPointerType.UnsignedByte, sizeof(int), IntPtr.Zero);
                GL.EnableClientState(ArrayCap.ColorArray);
            }

            // Vertex Buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, vc);
            GL.VertexPointer(3, VertexPointerType.Float, Vector3.SizeInBytes, IntPtr.Zero);
            GL.EnableClientState(ArrayCap.VertexArray);

            // Index Buffer
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ic);

            GL.PushMatrix();
            ApplyTransform();

            GL.DrawRangeElements(PrimitiveType.Triangles, 0, 36, 36, DrawElementsType.UnsignedInt, IntPtr.Zero);
            //GL.DrawRangeElements(PrimitiveType.Lines, 36, 60, 24, DrawElementsType.UnsignedInt, IntPtr.Zero);

            GL.PopMatrix();
        }

        //public override void Render()
        //{
        //    const float u = 0.5f;

        //    GL.PushMatrix();
        //    ApplyTransform();

        //    GL.Color4(Color);
        //    GL.Begin(PrimitiveType.Triangles);
        //    {
        //        // FRONT
        //        GL.Vertex3( u,  u,  u);
        //        GL.Vertex3( u, -u,  u);
        //        GL.Vertex3(-u, -u,  u);
                
        //        GL.Vertex3( u,  u,  u);
        //        GL.Vertex3(-u, -u,  u);
        //        GL.Vertex3(-u,  u,  u);
                
        //        // LEFT
        //        GL.Vertex3(-u,  u,  u);
        //        GL.Vertex3(-u, -u,  u);
        //        GL.Vertex3(-u, -u, -u);
                
        //        GL.Vertex3(-u,  u,  u);
        //        GL.Vertex3(-u, -u, -u);
        //        GL.Vertex3(-u,  u, -u);
                
        //        // TOP
        //        GL.Vertex3( u,  u,  u);
        //        GL.Vertex3(-u,  u,  u);
        //        GL.Vertex3(-u,  u, -u);
                
        //        GL.Vertex3( u,  u,  u);
        //        GL.Vertex3(-u,  u, -u);
        //        GL.Vertex3( u,  u, -u);
                
        //        // RIGHT
        //        GL.Vertex3( u,  u,  u);
        //        GL.Vertex3( u, -u,  u);
        //        GL.Vertex3( u, -u, -u);
                
        //        GL.Vertex3( u,  u,  u);
        //        GL.Vertex3( u, -u, -u);
        //        GL.Vertex3( u,  u, -u);
                
        //        // BOTTOM
        //        GL.Vertex3( u, -u,  u);
        //        GL.Vertex3( u, -u, -u);
        //        GL.Vertex3(-u, -u, -u);

        //        GL.Vertex3( u, -u,  u);
        //        GL.Vertex3(-u, -u, -u);
        //        GL.Vertex3(-u, -u,  u);
                
        //        // BACK
        //        GL.Vertex3( u,  u, -u);
        //        GL.Vertex3(-u,  u, -u);
        //        GL.Vertex3(-u, -u, -u);

        //        GL.Vertex3( u,  u, -u);
        //        GL.Vertex3(-u, -u, -u);
        //        GL.Vertex3( u, -u, -u);
        //    }
        //    GL.End();

        //    const float u2 = u + 0.001f;
        //    GL.Color4(Color4.Black);
        //    GL.Begin(PrimitiveType.Lines);
        //    {
        //        // FRONT
        //        GL.Vertex3( u2,  u2,  u2);
        //        GL.Vertex3(-u2,  u2,  u2);
                
        //        GL.Vertex3(-u2,  u2,  u2);
        //        GL.Vertex3(-u2, -u2,  u2);

        //        GL.Vertex3(-u2, -u2,  u2);
        //        GL.Vertex3( u2, -u2,  u2);
                
        //        GL.Vertex3( u2, -u2,  u2);
        //        GL.Vertex3( u2,  u2,  u2);

        //        // BACK
        //        GL.Vertex3( u2,  u2, -u2);
        //        GL.Vertex3(-u2,  u2, -u2);

        //        GL.Vertex3(-u2,  u2, -u2);
        //        GL.Vertex3(-u2, -u2, -u2);

        //        GL.Vertex3(-u2, -u2, -u2);
        //        GL.Vertex3( u2, -u2, -u2);

        //        GL.Vertex3( u2, -u2, -u2);
        //        GL.Vertex3( u2,  u2, -u2);

        //        // LEFT
        //        GL.Vertex3(-u2,  u2,  u2);
        //        GL.Vertex3(-u2,  u2, -u2);

        //        GL.Vertex3(-u2, -u2,  u2);
        //        GL.Vertex3(-u2, -u2, -u2);

        //        // RIGHT
        //        GL.Vertex3( u2,  u2,  u2);
        //        GL.Vertex3( u2,  u2, -u2);
                
        //        GL.Vertex3( u2, -u2,  u2);
        //        GL.Vertex3( u2, -u2, -u2);

        //    }
        //    GL.End();

        //    GL.PopMatrix();
        //}
    }
}

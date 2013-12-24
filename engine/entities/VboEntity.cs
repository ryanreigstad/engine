using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace engine.entities
{
    public class VboEntity : Entity
    {
        public VboEntity(Vector3 position, Quaternion rotation, Vector3 scale)
            : base(position, rotation, scale)
        {
            _vertices = new[] { 
				new Vector3(-1.0f, -1.0f, 1.0f), 
				new Vector3(1.0f, -1.0f, 1.0f), 
				new Vector3(1.0f, 1.0f, 1.0f), 
				new Vector3(-1.0f, 1.0f, 1.0f), 
				new Vector3(-1.0f, -1.0f, -1.0f), 
				new Vector3(1.0f, -1.0f, -1.0f), 
				new Vector3(1.0f, 1.0f, -1.0f), 
				new Vector3(-1.0f, 1.0f, -1.0f) 
			};
            _normals = new[] { 
				new Vector3(-1.0f, -1.0f, 1.0f), 
				new Vector3(1.0f, -1.0f, 1.0f), 
				new Vector3(1.0f, 1.0f, 1.0f), 
				new Vector3(-1.0f, 1.0f, 1.0f), 
				new Vector3(-1.0f, -1.0f, -1.0f), 
				new Vector3(1.0f, -1.0f, -1.0f), 
				new Vector3(1.0f, 1.0f, -1.0f), 
				new Vector3(-1.0f, 1.0f, -1.0f) 
			};
            _colors = new[] { 
				VboFactory.ColorToRgba32(Color.Cyan), 
				VboFactory.ColorToRgba32(Color.Cyan), 
				VboFactory.ColorToRgba32(Color.DarkCyan), 
				VboFactory.ColorToRgba32(Color.DarkCyan), 
				VboFactory.ColorToRgba32(Color.Cyan), 
				VboFactory.ColorToRgba32(Color.Cyan), 
				VboFactory.ColorToRgba32(Color.DarkCyan), 
				VboFactory.ColorToRgba32(Color.DarkCyan) 
			};
            _indices = new uint[] { 
				0, 1, 2, 2, 3, 0, 
				3, 2, 6, 6, 7, 3, 
				7, 6, 5, 5, 4, 7, 
				4, 0, 3, 3, 7, 4, 
				0, 1, 5, 5, 4, 0,
				1, 5, 6, 6, 2, 1 
			};
        }


        public VboEntity(Vector3 position, Quaternion rotation, Vector3 scale, Vector3[] vertices, Vector3[] normals, int[] colors, uint[] indices)
            : base(position, rotation, scale)
        {
            _vertices = vertices;
            _normals = normals;
            _colors = colors;
            _indices = indices;
        }

        private readonly Vector3[] _vertices;
        private readonly Vector3[] _normals;
        private readonly int[] _colors;
        private readonly uint[] _indices;

        protected int[] ids = new int[0];

        public override void Render()
        {
            if (ids.Length == 0)
                ids = VboFactory.CreateVbo(_vertices, _normals, _colors, _indices);

            if (ids[(int)VboFactory.VboId.Vertex] == 0)
                return;
            if (ids[(int)VboFactory.VboId.Index] == 0)
                return;

            GL.PushMatrix();
            ApplyTransform();

            // Color Array Buffer (Colors not used when lighting is enabled)
            if (ids[(int)VboFactory.VboId.Color] != 0)
            {
                // Bind to the Array Buffer ID
                GL.BindBuffer(BufferTarget.ArrayBuffer, ids[(int)VboFactory.VboId.Color]);

                // Set the Pointer to the current bound array describing how the data ia stored
                GL.ColorPointer(4, ColorPointerType.UnsignedByte, sizeof(int), IntPtr.Zero);

                // Enable the client state so it will use this array buffer pointer
                GL.EnableClientState(ArrayCap.ColorArray);
            }

            // Vertex Array Buffer
            {
                // Bind to the Array Buffer ID
                GL.BindBuffer(BufferTarget.ArrayBuffer, ids[(int)VboFactory.VboId.Vertex]);

                // Set the Pointer to the current bound array describing how the data ia stored
                GL.VertexPointer(3, VertexPointerType.Float, Vector3.SizeInBytes, IntPtr.Zero);

                // Enable the client state so it will use this array buffer pointer
                GL.EnableClientState(ArrayCap.VertexArray);
            }

            // Element Array Buffer
            {
                // Bind to the Array Buffer ID
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, ids[(int)VboFactory.VboId.Index]);

                // Draw the elements in the element array buffer
                // Draws up items in the Color, Vertex, TexCoordinate, and Normal Buffers using indices in the ElementArrayBuffer
                GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);
            }

            GL.PopMatrix();
        }
    }

    public static class VboFactory
    {
        public static int[] CreateVbo(Vector3[] vertices, Vector3[] normals, int[] colors, uint[] indices)
        {
            var ids = new[] {0, 0, 0, 0};

            int bufferSize;
            // Color Array Buffer
            if (colors != null)
            {
                // Generate Array Buffer Id
                GL.GenBuffers(1, out ids[(int) VboId.Color]);

                // Bind current context to Array Buffer ID
                GL.BindBuffer(BufferTarget.ArrayBuffer, ids[(int)VboId.Color]);

                // Send data to buffer
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(colors.Length * sizeof(int)), colors, BufferUsageHint.StaticDraw);

                // Validate that the buffer is the correct size
                GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out bufferSize);

                if (colors.Length * sizeof(int) != bufferSize)
                    throw new ApplicationException("Vertex array not uploaded correctly");

                // Clear the buffer Binding
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            }

            // Normal Array Buffer
            if (normals != null)
            {
                // Generate Array Buffer Id
                GL.GenBuffers(1, out ids[(int)VboId.Normal]);

                // Bind current context to Array Buffer ID
                GL.BindBuffer(BufferTarget.ArrayBuffer, ids[(int)VboId.Normal]);

                // Send data to buffer
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(normals.Length * Vector3.SizeInBytes), normals, BufferUsageHint.StaticDraw);

                // Validate that the buffer is the correct size
                GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
                if (normals.Length * Vector3.SizeInBytes != bufferSize)
                    throw new ApplicationException("Normal array not uploaded correctly");

                // Clear the buffer Binding
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            }

            // Vertex Array Buffer
            {
                // Generate Array Buffer Id
                GL.GenBuffers(1, out ids[(int)VboId.Vertex]);

                // Bind current context to Array Buffer ID
                GL.BindBuffer(BufferTarget.ArrayBuffer, ids[(int)VboId.Vertex]);

                // Send data to buffer
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * Vector3.SizeInBytes), vertices, BufferUsageHint.DynamicDraw);

                // Validate that the buffer is the correct size
                GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
                if (vertices.Length * Vector3.SizeInBytes != bufferSize)
                    throw new ApplicationException("Vertex array not uploaded correctly");

                // Clear the buffer Binding
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            }

            // Element Array Buffer
            {
                // Generate Array Buffer Id
                GL.GenBuffers(1, out ids[(int)VboId.Index]);

                // Bind current context to Array Buffer ID
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, ids[(int)VboId.Index]);

                // Send data to buffer
                GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indices.Length * sizeof(int)), indices, BufferUsageHint.StaticDraw);

                // Validate that the buffer is the correct size
                GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
                if (indices.Length * sizeof(int) != bufferSize)
                    throw new ApplicationException("Element array not uploaded correctly");

                // Clear the buffer Binding
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            }
            return ids;
        }

        /// <summary>
        /// Converts a Color instance into an int representation
        /// </summary>
        /// <param name="c">
        /// A <see cref="Color"/> instance to be converted
        /// </param>
        /// <returns>
        /// A <see cref="System.Int32"/>
        /// </returns>
        public static int ColorToRgba32(Color c)
        {
            return (int)((c.A << 24) | (c.B << 16) | (c.G << 8) | c.R);
        }

        public enum VboId
        {
            Color = 0,
            Normal,
            Vertex,
            Index
        }
    }
}

using System;
using System.Collections.Generic;

using engine.data;
using engine.world;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

using CullFaceMode = OpenTK.Graphics.OpenGL.CullFaceMode;
using GL1 = OpenTK.Graphics.OpenGL.GL;
using EnableCap1 = OpenTK.Graphics.OpenGL.EnableCap;

namespace engine.graphics
{
    public class Renderer
    {
        private static readonly FullScreenQuad FullScreenQuad = new FullScreenQuad("");

        private const int PositionBuffer = 0;
        private const int NormalBuffer = 1;
        private const int TextureBuffer = 2;
        private const int DiffuseBuffer = -1; // TODO: this is for ambient occlusion data (i think)
        // todo: edge detection buffer, etc

        public Matrix4 Projection { get; set; }

        private int _frameBuffer;
        private int _depthBuffer;
        private int[] _frameBufferTextures;
        private int _width = 1600;
        private int _height = 900;

        public void OnLoad()
        {
            // vbo
            GL1.Enable(EnableCap1.VertexArray);
            GL1.Enable(EnableCap1.IndexArray);

            // depth
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);

            // culling
            GL.Enable(EnableCap.CullFace);
            GL.FrontFace(FrontFaceDirection.Ccw);

            GL.ClearColor(Color4.Black);

            InitShaders();
        }

        private void InitShaders()
        {
            // renders the position, normal, and texture data to 3 textures for further processing (1st pass)
            ShaderLibrary.LoadShader<ObjectShader>("deferred_pass1", "dp1_vert.vert", "dp1_frag.frag");
            // lighting (2nd pass)
            ShaderLibrary.LoadShader<LightingShader>("deferred_pass2", "passthru.vert", "dp2_frag.frag");
            // ui, is really simple for now, it inverts the color (2nd pass)
            ShaderLibrary.LoadShader<OverlayShader>("deferred_ui", "passthru.vert", "dui.frag");

            ShaderLibrary.LoadShader<ObjectShader>("fallback", "vs.vert", "fs.frag");

            InitDeferredRendering();
        }

        private void InitDeferredRendering()
        {
            _frameBuffer = GL.GenFramebuffer();
            Console.WriteLine("FrameBuffer = " + _frameBuffer);
            _frameBufferTextures = new int[3];

            _frameBufferTextures[PositionBuffer] = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, _frameBufferTextures[PositionBuffer]);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba32f, _width, _height, 0, PixelFormat.Bgra, PixelType.Float, (IntPtr)0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            Console.WriteLine("PositionBuffer = " + _frameBufferTextures[PositionBuffer]);

            _frameBufferTextures[NormalBuffer] = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, _frameBufferTextures[NormalBuffer]);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba32f, _width, _height, 0, PixelFormat.Bgra, PixelType.Float, (IntPtr)0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            Console.WriteLine("NormalBuffer = " + _frameBufferTextures[NormalBuffer]);

            _frameBufferTextures[TextureBuffer] = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, _frameBufferTextures[TextureBuffer]);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba32f, _width, _height, 0, PixelFormat.Bgra, PixelType.Float, (IntPtr)0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            Console.WriteLine("TextureBuffer = " + _frameBufferTextures[TextureBuffer]);

            _depthBuffer = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, _depthBuffer);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.R16f, _width, _height, 0, PixelFormat.Red, PixelType.Float, (IntPtr)0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            Console.WriteLine("Depth Buffer = " + _depthBuffer);

            GL.BindTexture(TextureTarget.Texture2D, 0);

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _frameBuffer);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, _frameBufferTextures[PositionBuffer], 0);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment1, TextureTarget.Texture2D, _frameBufferTextures[NormalBuffer], 0);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment2, TextureTarget.Texture2D, _frameBufferTextures[TextureBuffer], 0);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.Depth, TextureTarget.Texture2D, _depthBuffer, 0);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public void OnResize(int width, int height)
        {
            Projection = Matrix4.CreatePerspectiveFieldOfView(
                (float) (Math.PI / 4.0f), (_width = width) / (float) (_height = height), 0.1f, 80000f);
        }

        public void OnRender(World world)
        {
            // TODO: support multiple shaders (one per object)

            var view = world.Camera.Transform * Projection;

            // TEST DEFAULT RENDER (no lights)
            //var s = ShaderLibrary.GetShader<ObjectShader>("fallback");
            //s.Bind();
            //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            //foreach (var entity in Cull(world.Entities))
            //{
            //    s.UpdateUniforms(view, entity);
            //    RenderMesh(MeshLibrary.GetMesh(entity.MeshName));
            //}
            //s.Unbind();
            //return;
            // END TEST

            // ENTITY PASS (PASS 1)
            var os = ShaderLibrary.GetShader<ObjectShader>("deferred_pass1");
            os.Bind();

            var entities = world.Entities;
            var deferrable = GetDeferredRenderableEntities(entities);

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _frameBuffer);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.DrawBuffers(3, new[] { DrawBuffersEnum.ColorAttachment0, DrawBuffersEnum.ColorAttachment1, DrawBuffersEnum.ColorAttachment2, });
            foreach (var entity in Cull(deferrable))
            {
                os.UpdateUniforms(view, entity);
                RenderMesh(MeshLibrary.GetMesh(entity.MeshName));
            }
            os.Unbind();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // TEST DEFERRED BUFFERS (no lights)
            var s = ShaderLibrary.GetShader<ObjectShader>("fallback");
            s.Bind();
            RenderQuad(s, view, "" + _frameBufferTextures[PositionBuffer]);          // change the buffer to see a different one (pos = 0, normal = 1, texture)
            s.Unbind();
            return;
            // END TEST

            // BEGIN PASS 2
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, _frameBufferTextures[PositionBuffer]);
            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, _frameBufferTextures[NormalBuffer]);
            GL.ActiveTexture(TextureUnit.Texture2);
            GL.BindTexture(TextureTarget.Texture2D, _frameBufferTextures[TextureBuffer]);

            // TODO: maybe make these pass their own textures and have 3 stage deferred lighting.
            //          benefit: we can use the data later in the pipeline and do some potentially cool stuff
            //          con: memory, maybe fps (doubt it, depends on geometry)
            // LIGHT PASS
            var ls = ShaderLibrary.GetShader<LightingShader>("deferred_pass2");
            ls.Bind();

            var lights = world.Lights;
            foreach (var light in Cull(lights))
            {
                ls.UpdateUniforms(view, light);
                RenderMesh(MeshLibrary.GetMesh(light.MeshName));
            }
            ls.Unbind();

            // UI PASS
            var uis = ShaderLibrary.GetShader<OverlayShader>("deferred_ui");
            uis.Bind();
            RenderQuad(uis, view);
            uis.Unbind();
        }

        private void RenderMesh(Mesh mesh)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, mesh.VboId);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mesh.IboId);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, (IntPtr)0);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, (IntPtr)(sizeof(float) * 3));

            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, (IntPtr)(sizeof(float) * 6));

            GL.DrawRangeElements(mesh.Mode, 0, mesh.Count, mesh.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        /// <summary>
        /// Use for shaders that just need a fullscreen quad and do everything in the shaders
        /// </summary>
        private void RenderQuad(Shader s, Matrix4 view, string texture = "0")
        {
            FullScreenQuad.TextureName = texture;
            s.UpdateUniforms(view, FullScreenQuad);
            RenderMesh(MeshLibrary.GetMesh(FullScreenQuad.MeshName));
        }

        private List<Entity> GetDeferredRenderableEntities(List<Entity> source)
        {
            // TODO:
            return source;
        }

        private List<Entity> Cull(List<Entity> source)
        {
            // TODO:
            return source;
        }

        private List<Light> Cull(List<Light> source)
        {
            // TODO:
            return source;
        }
    }
}

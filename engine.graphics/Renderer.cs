using System;
using System.Collections.Generic;

using engine.data;
using engine.world;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

using GL1 = OpenTK.Graphics.OpenGL.GL;
using EnableCap1 = OpenTK.Graphics.OpenGL.EnableCap;

namespace engine.graphics
{
    public class Renderer
    {
        private const int PositionBuffer = 0;
        private const int NormalBuffer = 1;
        private const int TextureBuffer = 2;
        private const int DiffuseBuffer = -1; // TODO: this is for ambient occlusion data (i think)
        // todo: edge detection buffer, etc

        public Matrix4 Projection { get; set; }

        private int _frameBuffer;
        private int[] _frameBufferTextures;
        private int _width = 1600;
        private int _height = 900;

        public void OnLoad()
        {
            // vbo
            GL1.Enable(EnableCap1.VertexArray);
            GL1.Enable(EnableCap1.IndexArray);

            // other misc
            GL.Enable(EnableCap.DepthTest);

            GL.ClearColor(Color4.Black);

            InitShaders();
        }

        private void InitShaders()
        {
            ShaderLibrary.LoadShader<ObjectShader>("deferred_pass1", "dp1_vert.vert", "dp1_frag.frag");
            ShaderLibrary.LoadShader<LightingShader>("deferred_pass2", "dp2_vert.vert", "dp2_frag.frag");

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

            GL.BindTexture(TextureTarget.Texture2D, 0);

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _frameBuffer);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, _frameBufferTextures[PositionBuffer], 0);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment1, TextureTarget.Texture2D, _frameBufferTextures[NormalBuffer], 0);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment2, TextureTarget.Texture2D, _frameBufferTextures[TextureBuffer], 0);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            var renderbuffer = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, renderbuffer);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent16, _width, _height);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, renderbuffer);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
            Console.WriteLine("RenderBuffer = " + renderbuffer);
        }

        public void OnResize(int width, int height)
        {
            Projection = Matrix4.CreatePerspectiveFieldOfView(
                (float) (Math.PI / 4.0f), (_width = width) / (float) (_height = height), 0.1f, 10000f);
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
            //return;
            // END TEST

            // ENTITY PASS
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
            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, _frameBuffer);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            // TODO: blitting is slow (er than a fullscreen quad)
            GL.BlitFramebuffer(0, 0, _width, _height, 0, 0, _width, _height, ClearBufferMask.DepthBufferBit, BlitFramebufferFilter.Nearest);

            // TEST DEFERRED BUFFERS (no lights)
            //var s = ShaderLibrary.GetShader<ObjectShader>("fallback");
            //s.Bind();
            //var q = new FullScreenQuad("" + _frameBufferTextures[NormalBuffer]);          // change the buffer to see a different one
            //s.UpdateUniforms(view, q);
            //RenderMesh(MeshLibrary.GetMesh(q.MeshName));
            //return;
            // END TEST

            // LIGHT PASS
            var ls = ShaderLibrary.GetShader<LightingShader>("deferred_pass2");
            ls.Bind();

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, _frameBufferTextures[PositionBuffer]);
            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, _frameBufferTextures[NormalBuffer]);
            GL.ActiveTexture(TextureUnit.Texture2);
            GL.BindTexture(TextureTarget.Texture2D, _frameBufferTextures[TextureBuffer]);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.One);
            GL.Disable(EnableCap.DepthTest);
            GL.DepthMask(false);

            var lights = world.Lights;
            foreach (var light in Cull(lights))
            {
                ls.UpdateUniforms(view, light);
                RenderMesh(MeshLibrary.GetMesh(light.MeshName));
            }

            GL.Disable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);
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

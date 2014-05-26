using engine.core.entities;
using engine.core.resources;
using engine.data.exceptions;
using engine.util.extensions;

using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace engine.core.rendering
{
    public abstract class Shader
    {
        protected readonly int ProgramId;

        protected Shader()
        {
            ProgramId = GL.CreateProgram();

            if (ProgramId == 0)
                throw new ShaderException("couldn't create program");
        }

        public void AddProgram(string src, ShaderType type)
        {
            var shader = GL.CreateShader(type);

            if (shader == 0)
                throw new ShaderException("couldn't create shader");

            GL.ShaderSource(shader, src);
            GL.CompileShader(shader);

            int compilestatus;
            GL.GetShader(shader, ShaderParameter.CompileStatus, out compilestatus);
            if (compilestatus != 1)
                throw new ShaderException("shader didn't compile: {0}".Formatted(GL.GetShaderInfoLog(shader)));

            GL.AttachShader(ProgramId, shader);
        }

        public void Compile()
        {
            GL.LinkProgram(ProgramId);

            int linkstatus;
            GL.GetProgram(ProgramId, GetProgramParameterName.LinkStatus, out linkstatus);
            if (linkstatus != 1)
                throw new ShaderException("couldn't link program: {0}".Formatted(GL.GetProgramInfoLog(ProgramId)));

            GL.ValidateProgram(ProgramId);

            int validatestatus;
            GL.GetProgram(ProgramId, GetProgramParameterName.ValidateStatus, out validatestatus);
            if (validatestatus != 1)
                throw new ShaderException("couldn't validate program: {0}".Formatted(GL.GetProgramInfoLog(ProgramId)));

            OnCompile();
        }

        protected virtual void OnCompile()
        {
        }

        public virtual void Bind()
        {
            GL.UseProgram(ProgramId);
        }

        public virtual void UpdateUniforms(Matrix4 view, Entity entity)
        {
        }

        public virtual void Unbind()
        {
            // GL.UseProgram(0);        <-- no.
        }
    }

    public class ObjectShader : Shader
    {
        private const string ViewMatrix = "ViewMatrix";
        private const string ModelMatrix = "ModelMatrix";
        private const string ModelRotationMatrix = "ModelRotationMatrix";
        private const string ModelViewMatrix = "ModelViewMatrix";
        private const string Texture = "ObjectTexture";

        private int _modelMatrix;
        private int _modelRotationMatrix;
        private int _modelViewMatrix;
        private int _viewMatrix;
        private int _texture;

        protected override void OnCompile()
        {
            _viewMatrix = GL.GetUniformLocation(ProgramId, ViewMatrix);
            _modelMatrix = GL.GetUniformLocation(ProgramId, ModelMatrix);
            _modelRotationMatrix = GL.GetUniformLocation(ProgramId, ModelRotationMatrix);
            _modelViewMatrix = GL.GetUniformLocation(ProgramId, ModelViewMatrix);
            _texture = GL.GetUniformLocation(ProgramId, Texture);
        }

        public override void UpdateUniforms(Matrix4 view, Entity entity)
        {
            if (_viewMatrix >= 0)
                GL.UniformMatrix4(_viewMatrix, false, ref view);
            if (_modelMatrix >= 0)
            {
                var t = entity.Transform;
                GL.UniformMatrix4(_modelMatrix, false, ref t);
            }
            if (_modelRotationMatrix >= 0)
            {
                var t = entity.RotationMatrix;
                GL.UniformMatrix4(_modelRotationMatrix, false, ref t);
            }
            if (_modelViewMatrix >= 0)
            {
                Matrix4 modelView;
                if (entity is FullScreenQuad)
                    modelView = (entity as FullScreenQuad).Transform;
                else
                    modelView = entity.Transform * view;
                GL.UniformMatrix4(_modelViewMatrix, false, ref modelView);
            }
            if (_texture >= 0)
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, TextureLibrary.GetTexture(entity.TextureName));
                GL.Uniform1(_texture, 0);
            }
        }
    }

    public class LightingShader : Shader
    {
        private const string ViewMatrix = "ViewMatrix";
        private const string ModelMatrix = "ModelMatrix";
        private const string ModelRotationMatrix = "ModelRotationMatrix";
        private const string ModelViewMatrix = "ModelViewMatrix";
        private const string LightSpecularity = "LightSpecularity";
        private const string LightDiffuse = "LightDiffuse";
        private const string LightPosition = "LightPosition";

        private const string PositionTexture = "PositionTexture";
        private const string NormalTexture = "NormalTexture";
        private const string ColorTexture = "ColorTexture";

        private int _viewMatrix;
        private int _modelMatrix;
        private int _modelRotationMatrix;
        private int _modelViewMatrix;
        private int _lightSpecularity;
        private int _lightDiffuse;
        private int _lightPosition;

        private int _positionTexture;
        private int _normalTexture;
        private int _colorTexture;

        protected override void OnCompile()
        {
            _viewMatrix = GL.GetUniformLocation(ProgramId, ViewMatrix);
            _modelMatrix = GL.GetUniformLocation(ProgramId, ModelMatrix);
            _modelRotationMatrix = GL.GetUniformLocation(ProgramId, ModelRotationMatrix);
            _modelViewMatrix = GL.GetUniformLocation(ProgramId, ModelViewMatrix);
            _lightSpecularity = GL.GetUniformLocation(ProgramId, LightSpecularity);
            _lightDiffuse = GL.GetUniformLocation(ProgramId, LightDiffuse);
            _lightPosition = GL.GetUniformLocation(ProgramId, LightPosition);

            _positionTexture = GL.GetUniformLocation(ProgramId, PositionTexture);
            _normalTexture = GL.GetUniformLocation(ProgramId, NormalTexture);
            _colorTexture = GL.GetUniformLocation(ProgramId, ColorTexture);
        }

        public override void Bind()
        {
            base.Bind();

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.One);
            GL.Disable(EnableCap.DepthTest);
            GL.DepthMask(false);
        }

        public override void UpdateUniforms(Matrix4 view, Entity entity)
        {
            if (entity is Light)
                UpdateUniforms(view, (Light) entity);
        }

        public void UpdateUniforms(Matrix4 view, Light light)
        {
            if (_viewMatrix >= 0)
                GL.UniformMatrix4(_viewMatrix, false, ref view);
            if (_modelMatrix >= 0)
            {
                var t = light.Transform;
                GL.UniformMatrix4(_modelMatrix, false, ref t);
            }
            if (_modelRotationMatrix >= 0)
            {
                var t = light.RotationMatrix;
                GL.UniformMatrix4(_modelRotationMatrix, false, ref t);
            }
            if (_modelViewMatrix >= 0)
            {
                Matrix4 modelView;
                if (light is AmbientLight)
                    modelView = (light as AmbientLight).Transform;
                else
                    modelView = light.Transform * view;
                GL.UniformMatrix4(_modelViewMatrix, false, ref modelView);
            }
            if (_lightPosition >= 0)
                GL.Uniform3(_lightPosition, light.Position);
            if (_lightSpecularity >= 0)
                GL.Uniform3(_lightSpecularity, light.Specularity);
            if (_lightDiffuse >= 0)
                GL.Uniform3(_lightDiffuse, light.Diffuse);

            if (_positionTexture >= 0)
                GL.Uniform1(_positionTexture, 0);
            if (_normalTexture >= 0)
                GL.Uniform1(_normalTexture, 1);
            if (_colorTexture >= 0)
                GL.Uniform1(_colorTexture, 2);
        }

        public override void Unbind()
        {
            base.Unbind();

            GL.Disable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);
        }
    }

    public class OverlayShader : Shader
    {
        private const string ViewMatrix = "ViewMatrix";
        private const string ModelMatrix = "ModelMatrix";
        private const string ModelRotationMatrix = "ModelRotationMatrix";
        private const string ModelViewMatrix = "ModelViewMatrix";
        private const string ColorTexture = "ColorTexture";

        private int _viewMatrix;
        private int _modelMatrix;
        private int _modelRotationMatrix;
        private int _modelViewMatrix;
        private int _colorTexture;

        protected override void OnCompile()
        {
            _viewMatrix = GL.GetUniformLocation(ProgramId, ViewMatrix);
            _modelMatrix = GL.GetUniformLocation(ProgramId, ModelMatrix);
            _modelRotationMatrix = GL.GetUniformLocation(ProgramId, ModelRotationMatrix);
            _modelViewMatrix = GL.GetUniformLocation(ProgramId, ModelViewMatrix);
            _colorTexture = GL.GetUniformLocation(ProgramId, ColorTexture);
        }

        public override void Bind()
        {
            base.Bind();

            GL.Disable(EnableCap.DepthTest);
            GL.DepthMask(false);
        }

        public override void UpdateUniforms(Matrix4 view, Entity entity)
        {
            if (_viewMatrix >= 0)
                GL.UniformMatrix4(_viewMatrix, false, ref view);
            if (_modelMatrix >= 0)
            {
                var t = entity.Transform;
                GL.UniformMatrix4(_modelMatrix, false, ref t);
            }
            if (_modelRotationMatrix >= 0)
            {
                var t = entity.RotationMatrix;
                GL.UniformMatrix4(_modelRotationMatrix, false, ref t);
            }
            if (_modelViewMatrix >= 0)
            {
                var modelView = entity.Transform;
                if (!(entity is FullScreenQuad))
                    modelView *= view;
                GL.UniformMatrix4(_modelViewMatrix, false, ref modelView);
            }
            if (_colorTexture >= 0)
                GL.Uniform1(_colorTexture, 2);
        }

        public override void Unbind()
        {
            base.Unbind();

            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);
        }
    }
}

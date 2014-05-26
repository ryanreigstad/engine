using System;

namespace engine.data.exceptions
{
    public class RenderingException : EngineException
    {
        public RenderingException()
            : base()
        {
        }
        public RenderingException(string msg)
            : base(msg)
        {
        }
        public RenderingException(string msg, Exception cause)
            : base(msg, cause)
        {
        }
        public RenderingException(Exception cause)
            : base(null, cause)
        {
        }
    }

    public class ModelException : RenderingException
    {
        public ModelException()
            : base()
        {
        }
        public ModelException(string msg)
            : base(msg)
        {
        }
        public ModelException(string msg, Exception cause)
            : base(msg, cause)
        {
        }
        public ModelException(Exception cause)
            : base(null, cause)
        {
        }
    }

    public class ShaderException : RenderingException
    {
        public ShaderException()
            : base()
        {
        }
        public ShaderException(string msg)
            : base(msg)
        {
        }
        public ShaderException(string msg, Exception cause)
            : base(msg, cause)
        {
        }
        public ShaderException(Exception cause)
            : base(null, cause)
        {
        }
    }

    public class TextureException : RenderingException
    {
        public TextureException()
            : base()
        {
        }
        public TextureException(string msg)
            : base(msg)
        {
        }
        public TextureException(string msg, Exception cause)
            : base(msg, cause)
        {
        }
        public TextureException(Exception cause)
            : base(null, cause)
        {
        }
    }
}

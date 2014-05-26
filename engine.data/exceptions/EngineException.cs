using System;

namespace engine.data.exceptions
{
    public class EngineException : Exception
    {
        public EngineException()
            : base()
        {
            throw new EngineException("fuck you, give a message");
        }
        public EngineException(string msg)
            : base(msg)
        {
        }
        public EngineException(string msg, Exception cause)
            : base(msg, cause)
        {
        }
        public EngineException(Exception cause)
            : base(null, cause)
        {
        }
    }
    public class ImplementationException : EngineException
    {
        public ImplementationException()
            : base()
        {
        }
        public ImplementationException(string msg)
            : base(msg)
        {
        }
        public ImplementationException(string msg, Exception cause)
            : base(msg, cause)
        {
        }
        public ImplementationException(Exception cause)
            : base(null, cause)
        {
        }
    }
}

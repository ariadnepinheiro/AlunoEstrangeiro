namespace Seeduc.Infra.Data
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class ContextQueryException : Exception
    {
        public ContextQueryException()
        {
        }

        public ContextQueryException(string message)
            : base(message)
        {
        }

        public ContextQueryException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected ContextQueryException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
namespace Seeduc.Infra.Data
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class DataContextException : Exception
    {
        public DataContextException()
        {
        }

        public DataContextException(string message)
            : base(message)
        {
        }

        public DataContextException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected DataContextException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

namespace Seeduc.Infra
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class SeeducException : Exception
    {
        public SeeducException()
        {
        }

        public SeeducException(string message)
            : base(message)
        {
        }

        public SeeducException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected SeeducException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
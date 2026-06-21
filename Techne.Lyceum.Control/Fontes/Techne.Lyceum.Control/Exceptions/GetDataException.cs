using System;
using System.Runtime.Serialization;

namespace Techne.Exceptions
{
    [Serializable]
    public class GetDataException : ApplicationException
    {
        public GetDataException()
        {
        }

        public GetDataException(string message)
            : base(message)
        {
        }

        public GetDataException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected GetDataException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
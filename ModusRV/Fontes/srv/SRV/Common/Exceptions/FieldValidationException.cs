using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SRV.Common.Exceptions
{

    [global::System.Serializable]
    public class FieldValidationException : Exception
    {
        public FieldValidationException() { }

        public FieldValidationException(string message) : base(message) { }
        
        public FieldValidationException(string message, Exception inner) : base(message, inner) { }
        
        protected FieldValidationException(System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SRV.Common.Exceptions
{
    public class BusinessException : Exception
    {

	    //default constructor
		public BusinessException()
        {
        }

        //constructor with exception message 
        public BusinessException(string message)
            : base(message)
        {
        }

        //constructor with message key, message and inner exception
        public BusinessException(string message, Exception inner)
            : base(message, inner)
        {
        }


    }
}
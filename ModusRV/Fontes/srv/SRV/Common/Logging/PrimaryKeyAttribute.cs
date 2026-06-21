using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SRV.Common.Logging
{

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PrimaryKeyAttribute : Attribute
    {
    }
}
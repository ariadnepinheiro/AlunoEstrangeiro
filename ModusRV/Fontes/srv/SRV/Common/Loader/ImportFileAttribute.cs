using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SRV.Common.Loader
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ImportFileAttribute : Attribute
    {
        /// <summary>
        /// The number of columns in file
        /// </summary>
        public int Columns { get; set; }

        /// <summary>
        /// The field delimiter
        /// </summary>
        public string FieldDelimiter { get; set; }

        public ImportFileAttribute()
        {
            //Default delimiter
            FieldDelimiter = ";";
        }

    }
}
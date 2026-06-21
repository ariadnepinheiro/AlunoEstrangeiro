using System;
using System.Collections.Generic;
using System.Text;

namespace SRV.Common.Loader
{
    public enum DataType
    {
        String,
        Integer,
        DateTime
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ImportFieldAttribute : Attribute
    {
        /// <summary>
        /// The position of the field
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Set to true if validation is required
        /// </summary>
        public bool EnableValidation { get; set; }

        /// <summary>
        /// The regexp validation pattern for the field
        /// Validation happens only if EnableValidation is set to true
        /// </summary>
        public string ValidationPattern { get; set; }

        /// <summary>
        /// Set to true if the field will accept the character point '.'
        /// </summary>
        public bool AllowsPoint { get; set; }

        /// <summary>
        /// Set to number of decimal places of the field
        /// </summary>
        public int NumDecimalPlaces { get; set; }

        /// <summary>
        /// Set to true if is required
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// The exact length of the field
        /// </summary>
        public int ValidationLength { get; set; }

        /// <summary>
        /// The max length of the field
        /// </summary>
        public int ValidationMaxLength { get; set; }

        /// <summary>
        /// Determines whether input should be trimmed
        /// </summary>
        public bool EnableTrimming { get; set; }

        /// <summary>
        /// The type of data for the field
        /// </summary>
        public DataType DataType { get; set; }

        public ImportFieldAttribute(int position)
        {
            this.Position = position;
            this.DataType = DataType.String;
            this.AllowsPoint = true;
        }

    }

}
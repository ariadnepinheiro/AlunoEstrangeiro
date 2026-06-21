using System;
using System.ComponentModel;
using System.Globalization;

namespace Techne.Library.Sql.Structure
{
    public class SqlSelectConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, 
                                            Type sourceType)
        {
            if (sourceType == typeof (string))
            {
                return true;
            }

            return false;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, 
                                          Type destinationType)
        {
            if (destinationType == typeof (string))
            {
                return true;
            }

            return false;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, 
                                           CultureInfo culture, 
                                           object value)
        {
            if (value is string)
            {
                return SqlSelect.Parse((string)value);
            }

            throw new NotSupportedException();
        }

        public override object ConvertTo(ITypeDescriptorContext context, 
                                         CultureInfo culture, 
                                         object value, 
                                         Type destinationType)
        {
            if (destinationType == typeof (string))
            {
                var sql = (SqlSelect)value;
                return sql.ToString();
            }

            throw new NotSupportedException();
        }
    }
}
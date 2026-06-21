using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace Techne.Controls
{
    public class StringArrayConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, 
                                            Type sourceType)
        {
            if (sourceType == typeof (string))
            {
                return true;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, 
                                          Type destinationType)
        {
            if (destinationType == typeof (string))
            {
                return true;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public override object ConvertFrom(ITypeDescriptorContext context, 
                                           CultureInfo culture, 
                                           object value)
        {
            if (value is string)
            {
                var array = new ArrayList();
                foreach (var item in ((string)value).Split(','))
                {
                    array.Add(item.Trim());
                }

                return array.ToArray(typeof (string));
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public override object ConvertTo(ITypeDescriptorContext context, 
                                         CultureInfo culture, 
                                         object value, 
                                         Type destinationType)
        {
            if (destinationType == typeof (string))
            {
                return StrLib.EnumerableToStr((string[])value, ", ");
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}
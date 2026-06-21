using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Techne.Controls
{
    /// <summary>
    ///   Conversor de Type para string. Utilizado para persistência de propriedades do tipo Type persistidas
    ///   em páginas aspx. Tipos suportados: VarChar, Number e Date.
    /// </summary>
    internal class DataTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof (string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, 
                                          Type destinationType)
        {
            if (destinationType == typeof (InstanceDescriptor))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, 
                                           CultureInfo culture, 
                                           object value)
        {
            if (value is string)
            {
                return Type.GetType((string)value);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, 
                                         CultureInfo culture, 
                                         object value, 
                                         Type destinationType)
        {
            if (destinationType == typeof (InstanceDescriptor))
            {
                var method = typeof (Type).GetMethod("GetType", new[] { typeof (string) });
                return new InstanceDescriptor(method, new[] { ((Type)value).FullName });
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new[]
                                                              {
                                                                  typeof (VarChar), 
                                                                  typeof (Number), 
                                                                  typeof (Date)
                                                              });
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}
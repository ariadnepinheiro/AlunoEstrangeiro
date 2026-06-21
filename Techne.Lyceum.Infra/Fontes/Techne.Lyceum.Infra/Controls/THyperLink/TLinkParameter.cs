using System;
using System.Collections;
using System.ComponentModel;

namespace Techne.Controls
{
    [
        TypeConverter(typeof (TLinkParameterConverter)), 
    ]
    internal class TLinkParameter
    {
        private string name;

        private string value;

        public TLinkParameter() : this(string.Empty, typeof (string), string.Empty)
        {
        }

        public TLinkParameter(string name, Type dataType, string value)
        {
            this.name = name;
            this.DataType = dataType;
            this.value = value;
        }

        [DefaultValue(typeof (string)), TypeConverter(typeof (DataTypeConverter)),]
        public Type DataType { get; set; }

        [
            DefaultValue(""), 
        ]
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value == null ? string.Empty : value;
            }
        }

        [
            DefaultValue(""), 
        ]
        public string Value
        {
            get
            {
                return this.value;
            }

            set
            {
                this.value = value == null ? string.Empty : value;
            }
        }

        public override string ToString()
        {
            return this.name.Length == 0 ? "[unnamed]" : this.name;
        }

        /// <summary>
        ///   Retorna um objeto contendo a propriedade Value convertida para
        ///   o tipo DataType
        /// </summary>
        /// <returns></returns>
        public object getTypedValue()
        {
            object ret = null;
            ret = Techne.Web.Navigation.ConvertToType(this.Value, this.DataType);
            return ret;
        }
    }

    internal class TLinkParameterCollection : CollectionBase
    {
        private readonly ArrayList names;

        public TLinkParameterCollection()
        {
            this.names = new ArrayList();
        }

        public TLinkParameter this[int index]
        {
            get
            {
                return (TLinkParameter)this.List[index];
            }
        }

        public int Add(TLinkParameter parameter)
        {
            this.names.Add(parameter.Name);
            return this.List.Add(parameter);
        }

        public int Add(string name, Type dataType, string value)
        {
            return this.Add(new TLinkParameter(name, dataType, value));
        }

        public void AddRange(TLinkParameter[] parameters)
        {
            foreach (var parameter in parameters)
            {
                this.Add(parameter);
            }
        }

        public int IndexOf(string name)
        {
            return this.names.IndexOf(name);
        }

        // IMPORTANTE: Não pode haver overload de indexador (this) por string
        // porque causa erro de parsing (tanto em design quanto em runtime).
    }
}

namespace Techne.Controls
{
    using System.ComponentModel.Design.Serialization;
    using System.Globalization;
    using System.Reflection;

    internal class TLinkParameterConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof (InstanceDescriptor))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, 
                                         CultureInfo culture, 
                                         object value, 
                                         Type destinationType)
        {
            if (destinationType == typeof (InstanceDescriptor))
            {
                return this.GetConstructor((TLinkParameter)value);
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        private InstanceDescriptor GetConstructor(TLinkParameter parameter)
        {
            MemberInfo memberInfo = typeof (TLinkParameter).GetConstructor(new[] { typeof (string), typeof (Type), typeof (string) });
            return new InstanceDescriptor(memberInfo, new object[] { parameter.Name, parameter.DataType, parameter.Value });
        }
    }
}
using System;
using System.CodeDom;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Techne
{
    internal class DesignerLib
    {
        public static object Deserialize(IDesignerSerializationManager manager, object codeObject, Type type)
        {
            var baseSerializer = (CodeDomSerializer)manager.GetSerializer(type.BaseType, typeof (CodeDomSerializer));
            var statements = codeObject as CodeStatementCollection;
            CodeAssignStatement assignment = null;
            CodeTypeOfExpression typeOf = null;

            if (statements != null)
            {
                foreach (CodeStatement statement in statements)
                {
                    assignment = statement as CodeAssignStatement;
                    var invoke = assignment == null ? null : assignment.Right as CodeMethodInvokeExpression;
                    typeOf = invoke == null ? null : invoke.Method.TargetObject as CodeTypeOfExpression;
                    if (typeOf != null)
                    {
                        break;
                    }
                }
            }

            if (typeOf != null)
            {
                // Achou a inicialização no formato "<var> = <classe>.Create()".
                // Substitui a chamada do factory method ("<classe>.Create()") pelo construtor (new <classe>()) em design-time.
                statements.Remove(assignment);

                // Desserializa o instanciamento utilizando o construtor sem parâmetros (que existe só para isto).
                var obj = baseSerializer.Deserialize(manager, new CodeStatementCollection(new CodeStatement[]
                                                                                          {
                                                                                              new CodeAssignStatement(
                                                                                                  assignment.Left, 
                                                                                                  new CodeObjectCreateExpression(typeOf.Type.BaseType)
                                                                                                  )
                                                                                          }));

                // Desserializa o resto (inicializações).
                baseSerializer.Deserialize(manager, codeObject);

                return obj;
            }
            else
            {
                // Não encontrou instanciamento ou ele não é do tipo "<var> = <classe>.Create()".
                // Chama o Deserialize() default.
                return baseSerializer.Deserialize(manager, codeObject);
            }
        }

        public static object Serialize(IDesignerSerializationManager manager, object value, Type type)
        {
            if (value == null)
            {
                return null;
            }

            var baseSerializer = (CodeDomSerializer)manager.GetSerializer(type.BaseType, typeof (CodeDomSerializer));
            return baseSerializer.Serialize(manager, value);
        }
    }
}

namespace Techne.Data
{
    internal class TDataSetSerializer : CodeDomSerializer
    {
        public override object Deserialize(IDesignerSerializationManager manager, object codeObject)
        {
            return DesignerLib.Deserialize(manager, codeObject, typeof (TDataSet));
        }

        public override object Serialize(IDesignerSerializationManager manager, object value)
        {
            return DesignerLib.Serialize(manager, value, typeof (TDataSet));
        }
    }

    internal class TDataSetConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof (InstanceDescriptor))
            {
                return true;
            }
            else
            {
                return base.CanConvertTo(context, destinationType);
            }
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if ((destinationType != typeof (InstanceDescriptor)) || !(value is TDataSet))
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }

            var method = value.GetType().GetMethod("Create", BindingFlags.Public | BindingFlags.Static);
            if (method == null)
            {
                throw new ApplicationException("GetMethod() falhou.");
            }

            return new InstanceDescriptor(method, new object[0], true);
        }
    }

    internal class TDataTableSerializer : CodeDomSerializer
    {
        public override object Deserialize(IDesignerSerializationManager manager, object codeObject)
        {
            return DesignerLib.Deserialize(manager, codeObject, typeof (TDataTable));
        }

        public override object Serialize(IDesignerSerializationManager manager, object value)
        {
            return DesignerLib.Serialize(manager, value, typeof (TDataTable));
        }
    }

    internal class TDataTableConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof (InstanceDescriptor))
            {
                return true;
            }
            else
            {
                return base.CanConvertTo(context, destinationType);
            }
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if ((destinationType != typeof (InstanceDescriptor)) || !(value is TDataTable))
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }

            var method = value.GetType().GetMethod("Create", BindingFlags.Public | BindingFlags.Static);
            if (method == null)
            {
                throw new ApplicationException("GetMethod() falhou.");
            }

            return new InstanceDescriptor(method, new object[0], true);
        }
    }
}
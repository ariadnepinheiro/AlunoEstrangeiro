namespace Seeduc.Infra.Mapper
{
    using System;

    internal class ConvertObject
    {
        internal static object To(Type type, object value)
        {
            if (value == null
                || value.GetType() == type)
            {
                return value;
            }

            // Try to convert the value
            var toType = type;

            if (type.IsGenericType)
            {
                var genericTypeDefinition = type.GetGenericTypeDefinition();

                if (genericTypeDefinition != null
                    && genericTypeDefinition.Equals(typeof(Nullable<>)))
                {
                    toType = Nullable.GetUnderlyingType(type);
                }
            }

            return Convert.ChangeType(value, toType);
        }
    }
}
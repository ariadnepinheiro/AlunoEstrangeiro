namespace Seeduc.Infra.Mapper
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Seeduc.Infra.Entities;

    public abstract class BaseMapper
    {
        protected static void MapDictionaryTo<T>(IDictionary<PropertyInfo, object> properties, T t)
            where T : class, IEntity, new()
        {
            foreach (var property in properties)
            {
                BindAttribute(property.Value, property.Key, t);
            }
        }

        private static void BindAttribute(object value, PropertyInfo property, object entity)
        {
            try
            {
                var convertedValue = ConvertObject.To(property.PropertyType, value);

                property.SetValue(entity, convertedValue, null);
            }
            catch (Exception)
            {
            }
        }
    }
}
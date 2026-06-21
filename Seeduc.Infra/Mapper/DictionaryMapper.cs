namespace Seeduc.Infra.Mapper
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Seeduc.Infra.Cache;
    using Seeduc.Infra.Entities;

    public class DictionaryMapper : BaseMapper
    {
        public static T CreateAndMapTo<T>(IDictionary dictionary)
            where T : class, IEntity, new()
        {
            var entity = new T();

            return MapTo(dictionary, entity);
        }

        public static T MapTo<T>(IDictionary dictionary, T entity)
            where T : class, IEntity, new()
        {
            if (dictionary == null)
            {
                return entity;
            }

            var properties = ReflectionCache.Instance.GetProperties<T>();
            var keys = dictionary
                .Keys
                .Cast<string>();

            return MapTo(dictionary, keys, entity, properties);
        }

        private static T MapTo<T>(IDictionary dictionary, IEnumerable<string> keys, T entity, IDictionary<string, PropertyInfo> properties)
            where T : class, IEntity, new()
        {
            var data = new Dictionary<PropertyInfo, object>();

            foreach (var key in keys)
            {
                var propertyName = key.ToUpper();

                if (!properties.ContainsKey(propertyName))
                {
                    continue;
                }

                var property = properties[propertyName];
                var value = dictionary[key] == DBNull.Value ? null : dictionary[key];

                data[property] = value;
            }

            MapDictionaryTo(data, entity);

            return entity;
        }
    }
}
namespace Seeduc.Infra.Mapper
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Reflection;
    using Seeduc.Infra.Cache;
    using Seeduc.Infra.Entities;

    internal class SqlDataReaderMapper : BaseMapper
    {
        public static T CreateAndMapTo<T>(SqlDataReader sqlDataReader)
            where T : class, IEntity, new()
        {
            var entity = new T();

            if (sqlDataReader == null)
            {
                return entity;
            }

            var properties = ReflectionCache.Instance.GetProperties<T>();
            var data = new Dictionary<PropertyInfo, object>();

            for (var i = 0; i < sqlDataReader.FieldCount; i++)
            {
                var propertyName = sqlDataReader.GetName(i).ToUpper();

                if (!properties.ContainsKey(propertyName))
                {
                    continue;
                }

                var property = properties[propertyName];
                var value = sqlDataReader[i] == DBNull.Value ? null : sqlDataReader[i];

                data[property] = value;
            }

            MapDictionaryTo(data, entity);

            return entity;
        }
    }
}
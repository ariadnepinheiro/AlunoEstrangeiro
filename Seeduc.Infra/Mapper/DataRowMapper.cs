namespace Seeduc.Infra.Mapper
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using Seeduc.Infra.Cache;
    using Seeduc.Infra.Entities;

    public class DataRowMapper : BaseMapper
    {
        public static T CreateAndMapTo<T>(DataRow dataRow)
            where T : class, IEntity, new()
        {
            var entity = new T();

            if (dataRow == null)
            {
                return entity;
            }

            var properties = ReflectionCache.Instance.GetProperties<T>();
            var columnNames = dataRow
                .Table
                .Columns
                .Cast<DataColumn>()
                .Select(column => column.ColumnName);

            return MapTo(dataRow, columnNames, entity, properties);
        }

        internal static T MapTo<T>(DataRow dataRow, IEnumerable<string> columnNames, T entity, IDictionary<string, PropertyInfo> properties)
            where T : class, IEntity, new()
        {
            var data = new Dictionary<PropertyInfo, object>();

            foreach (var columnName in columnNames)
            {
                var propertyName = columnName.ToUpper();

                if (!properties.ContainsKey(propertyName))
                {
                    continue;
                }

                var property = properties[propertyName];
                var value = dataRow[columnName] == DBNull.Value ? null : dataRow[columnName];

                data[property] = value;
            }

            MapDictionaryTo(data, entity);

            return entity;
        }
    }
}
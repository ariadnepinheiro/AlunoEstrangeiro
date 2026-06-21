namespace Seeduc.Infra.Mapper
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Seeduc.Infra.Cache;
    using Seeduc.Infra.Entities;

    public class DataTableMapper : BaseMapper
    {
        public static ICollection<T> CreateAndMapTo<T>(DataTable dataTable)
            where T : class, IEntity, new()
        {
            var entities = new List<T>();

            if (dataTable == null)
            {
                return entities;
            }

            var properties = ReflectionCache.Instance.GetProperties<T>();
            var columnsNames = dataTable
                .Columns
                .Cast<DataColumn>()
                .Select(column => column.ColumnName);

            foreach (DataRow dataRow in dataTable.Rows)
            {
                entities.Add(DataRowMapper.MapTo(dataRow, columnsNames, new T(), properties));
            }

            return entities;
        }
    }
}
namespace Seeduc.Infra.Mapper
{
    using System.Collections.Generic;
    using Seeduc.Infra.Entities;
    using Techne.Data;

    public class QueryTableMapper : BaseMapper
    {
        public static ICollection<T> CreateAndMapTo<T>(QueryTable queryTable)
            where T : class, IEntity, new()
        {
            return DataTableMapper.CreateAndMapTo<T>(queryTable);
        }
    }
}
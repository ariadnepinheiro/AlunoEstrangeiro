namespace Seeduc.Infra.Mapper
{
    using Seeduc.Infra.Entities;
    using Techne.Data;

    public class SimpleRowMapper : BaseMapper
    {
        public static T CreateAndMapTo<T>(SimpleRow simpleRow)
            where T : class, IEntity, new()
        {
            return DataRowMapper.CreateAndMapTo<T>(simpleRow);
        }
    }
}
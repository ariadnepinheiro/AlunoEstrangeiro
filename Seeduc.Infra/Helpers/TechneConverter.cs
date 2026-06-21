namespace Seeduc.Infra.Helpers
{
    using System.Data;
    using Techne.Data;

    public static class TechneConverter
    {
        public static QueryTable ToQueryTable(string sql, DataTable dataTable)
        {
            var queryTable = new QueryTable(sql);

            queryTable.Merge(dataTable);

            return queryTable;
        }
    }
}
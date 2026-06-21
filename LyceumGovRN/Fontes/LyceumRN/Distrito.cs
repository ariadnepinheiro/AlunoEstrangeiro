using System.Data;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN
{
    public class Distrito : RNBase
    {
        public static DataTable Listar(string idMunicipio)
        {
            using (var ctx = DataContextBuilder.FromHades.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"SELECT  ID_DISTRITO, DISTRITO
                        FROM    dbo.TCE_DISTRITO
                        WHERE   ID_MUNICIPIO = @ID_MUNICIPIO 
                        ORDER BY DISTRITO"
                };
                contextQuery.Parameters.Add("@ID_MUNICIPIO", idMunicipio);

                return ctx.GetDataTable(contextQuery);
            }
        }
    }
}

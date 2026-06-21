using System.Data;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN
{
    public class TipoTransporte : RNBase
    {
        public static DataTable Listar()
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"SELECT  DESCRICAO,TIPO
                                FROM    LY_TIPO_TRANSPORTE
                                ORDER BY DESCRICAO"
                };

                return ctx.GetDataTable(contextQuery);
            }
        }
    }
}


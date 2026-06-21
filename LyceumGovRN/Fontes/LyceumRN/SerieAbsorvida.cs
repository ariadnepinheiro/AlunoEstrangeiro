using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN
{
    public class SerieAbsorvida
    {
        public static DataTable ListaUnidadesDestino(string censo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @" SELECT SA.UNIDADEENSINODESTINOID AS UNIDADE_ENS, UNIDADE_ENS + ' - ' + UE.NOME_COMP as NOME_COMP FROM SERIEABSORVIDA SA
	                    INNER JOIN LY_UNIDADE_ENSINO UE
	                    ON SA.UNIDADEENSINODESTINOID = UE.UNIDADE_ENS
	                    WHERE UNIDADEENSINOORIGEMID = @UNIDADEENSINOORIGEMID ");

                contextQuery.Parameters.Add("@UNIDADEENSINOORIGEMID", censo);

                return ctx.GetDataTable(contextQuery);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.RecursosHumanos
{
    public class HistoricoAlteracaoAlunoCampos
    {
        public void Insere(DataContext contexto, int historicoAlteracaoAlunoId, string campo, string valorAnterior, string valorAtual)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO RecursosHumanos.HISTORICOALTERACAOALUNO_CAMPOS
                                               (HISTORICOALTERACAOALUNOID
                                               ,CAMPO
                                               ,VALORANTERIOR
                                               ,VALORATUAL)
                                         VALUES
                                               (@HISTORICOALTERACAOALUNOID, 
                                               @CAMPO, 
                                               @VALORANTERIOR, 
                                               @VALORATUAL) ";

            contextQuery.Parameters.Add("@HISTORICOALTERACAOALUNOID", SqlDbType.Decimal, historicoAlteracaoAlunoId);
            contextQuery.Parameters.Add("@CAMPO", SqlDbType.VarChar, campo);
            contextQuery.Parameters.Add("@VALORANTERIOR", SqlDbType.VarChar, valorAnterior);
            contextQuery.Parameters.Add("@VALORATUAL", SqlDbType.VarChar, valorAtual);

            contexto.ApplyModifications(contextQuery);
        }

        public void Remove(DataContext contexto, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE HC
                                    FROM  RecursosHumanos.HISTORICOALTERACAOALUNO RH 
                                        INNER JOIN RecursosHumanos.HISTORICOALTERACAOALUNO_CAMPOS HC on HC.HISTORICOALTERACAOALUNOID= RH.HISTORICOALTERACAOALUNOID
                                    WHERE RH.PESSOA = @PESSOA ";

            contextQuery.Parameters.Add("@PESSOA", SqlDbType.Decimal, pessoa);

            contexto.ApplyModifications(contextQuery);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.GestaoRede
{
    public class RegionalHistoricoUnidade
    {
        public void Insere(DataContext contexto, GestaoRede.Entidades.RegionalHistoricoUnidade regionalHistoricoUnidade)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO GestaoRede.REGIONALHISTORICOUNIDADE
                                           (REGIONALID
                                           ,CENSO
                                           ,DATAINICIO
                                           ,DATAFIM
                                           ,USUARIOID
                                           ,DATACADASTRO
                                           ,DATAALTERACAO)
                                     VALUES
                                           (@REGIONALID, 
                                            @CENSO, 
                                            @DATAINICIO,
                                            @DATAFIM, 
                                            @USUARIOID, 
                                            @DATACADASTRO, 
                                            @DATAALTERACAO) ";

            contextQuery.Parameters.Add("@REGIONALID", SqlDbType.Int, regionalHistoricoUnidade.RegionalId);
            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, regionalHistoricoUnidade.Censo);
            contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, regionalHistoricoUnidade.DataInicio.Date);

            if (regionalHistoricoUnidade.DataFim == null || regionalHistoricoUnidade.DataFim == DateTime.MinValue)
            {
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, null);
            }
            else
            {
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, Convert.ToDateTime(regionalHistoricoUnidade.DataFim).Date);
            }
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, regionalHistoricoUnidade.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public GestaoRede.Entidades.RegionalHistoricoUnidade ObtemRegionalAtivaPor(DataContext contexto, string censo)
        {
            GestaoRede.Entidades.RegionalHistoricoUnidade regionalHistorico = new GestaoRede.Entidades.RegionalHistoricoUnidade();
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT TOP 1 RU.* 
                                        FROM  GestaoRede.REGIONALHISTORICOUNIDADE RU (NOLOCK)
                                        WHERE CENSO = @CENSO
		                                        AND CONVERT(DATE, DATAINICIO) <= CONVERT(DATE, GETDATE())
		                                        AND (DATAFIM IS NULL
			                                        OR CONVERT(DATE, DATAFIM) > CONVERT(DATE, GETDATE()))
                                        ORDER BY DATAINICIO DESC ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);

            regionalHistorico = contexto.TryToBindEntity<GestaoRede.Entidades.RegionalHistoricoUnidade>(contextQuery);

            return regionalHistorico;
        }

        public void Finaliza(DataContext contexto, int regionalHistoricoUnidadeId, string usuarioResponsavel, DateTime dataFim)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"UPDATE GestaoRede.REGIONALHISTORICOUNIDADE
                                        SET DATAFIM = @DATAFIM,
	                                        USUARIOID = @USUARIOID,
	                                        DATAALTERACAO = @DATAALTERACAO
                                        WHERE REGIONALHISTORICOUNIDADEID = @REGIONALHISTORICOUNIDADEID ";

            contextQuery.Parameters.Add("@REGIONALHISTORICOUNIDADEID", SqlDbType.Int, regionalHistoricoUnidadeId);
            contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, dataFim);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioResponsavel);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }
    }
}

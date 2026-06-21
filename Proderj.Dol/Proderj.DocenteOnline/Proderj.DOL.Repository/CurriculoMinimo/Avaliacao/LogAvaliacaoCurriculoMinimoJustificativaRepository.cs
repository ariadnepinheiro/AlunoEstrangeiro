using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository
{
    public class LogAvaliacaoCurriculoMinimoJustificativaRepository : NHRepositoryBase<LogAvaliacaoCurriculoMinimoJustificativa>, ILogAvaliacaoCurriculoMinimoJustificativaRepository
    {
        public int InserePor(short ano, short periodo, short subperiodo, string matricula)
        {         
            try
            {
                var query = SessaoAuditada.CreateSQLQuery(
                                        @"  INSERT  INTO dbo.TCE_LOG_AVALIACAO_CM_JUSTIFICATIVA
										(
										  ID_AVALIACAO_CM_JUSTIFICATIVA,
										  ANO,
										  PERIODO,
										  SUBPERIODO,
										  JUSTIFICATIVA,
										  MATRICULA,
										  DT_CADASTRO
                                                
										)
										SELECT  ID_AVALIACAO_CM_JUSTIFICATIVA,
												ANO,
												PERIODO,
												SUBPERIODO,
												JUSTIFICATIVA,
												MATRICULA,
												DT_CADASTRO
										FROM    dbo.TCE_AVALIACAO_CM_JUSTIFICATIVA
										WHERE   MATRICULA = :MATRICULA
												AND ANO = :ANO
												AND PERIODO = :PERIODO
												AND SUBPERIODO = :SUBPERIODO"
                                    );

                query.SetString("MATRICULA", matricula);
                query.SetInt16("ANO", ano);
                query.SetInt16("PERIODO", periodo);
                query.SetInt16("SUBPERIODO", subperiodo);

                return query.ExecuteUpdate();              
            }
            catch (Exception)
            {
                
                throw new Exception("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.");
            }          
        }
    }
}

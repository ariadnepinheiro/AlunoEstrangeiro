using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository
{
    public class AvaliacaoCurriculoMinimoDocenteRepository : NHRepositoryBase<AvaliacaoCurriculoMinimoDocente>, IAvaliacaoCurriculoMinimoDocenteRepository
    {
        public int RemoveCompetenciasAntigas(string matricula, short ano, short periodo, short subperiodo)
        {
            try
            {
                var query = SessaoAuditada.CreateSQLQuery(
                                    @"  DELETE  FROM TCE_AVALIACAO_CM_DOCENTE
									WHERE   EXISTS ( SELECT 1
														FROM   TCE_AVALIACAO_CM
														WHERE  ANO = :ANO
															AND PERIODO = :PERIODO
															AND SUBPERIODO = :SUBPERIODO
															AND TCE_AVALIACAO_CM_DOCENTE.ID_AVALIACAO_CM = ID_AVALIACAO_CM 
															)
											AND MATRICULA = :MATRICULA");

                query.SetInt16("ANO", ano);
                query.SetInt16("PERIODO", periodo);
                query.SetInt16("SUBPERIODO", subperiodo);
                query.SetString("MATRICULA", matricula);

                return query.ExecuteUpdate();
            }
            catch (Exception)
            {
                throw new Exception("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.");
            }
        }

        public int InserePor(AvaliacaoCurriculoMinimoDocente avaliacaoCurriculoMinimoDocente)
        {
            try
            {


                var query = SessaoAuditada.CreateSQLQuery(
                                        @"INSERT  INTO TCE_AVALIACAO_CM_DOCENTE
									(
										ID_AVALIACAO_CM,
										RESPOSTA,
										MATRICULA
									)
									VALUES  
									(
										:ID_AVALIACAO_CM,
										:RESPOSTA,
										:MATRICULA
									)"
                                    );

                query.SetInt32("ID_AVALIACAO_CM", avaliacaoCurriculoMinimoDocente.AvaliacaoCurriculoMinimo.Id);
                query.SetBoolean("RESPOSTA", avaliacaoCurriculoMinimoDocente.Resposta);
                query.SetString("MATRICULA", avaliacaoCurriculoMinimoDocente.Matricula);

                return query.ExecuteUpdate();              
            }
            catch (Exception)
            {                
                throw new Exception("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.");
            }           
        }

        public bool InserePor(IList<AvaliacaoCurriculoMinimoDocente> avaliacoesCurriculoMinimoDocente, bool abrirTransacaoParaLista)
        {
            if (this.ConfereTransacaoEmCurso() && abrirTransacaoParaLista)
                abrirTransacaoParaLista = false;

            if (abrirTransacaoParaLista)
                this.InicializaTransacao();

            foreach (AvaliacaoCurriculoMinimoDocente avaliacaoCurriculoMinimoDocente in avaliacoesCurriculoMinimoDocente)
            {
                int retorno = InserePor(avaliacaoCurriculoMinimoDocente);
                if (retorno == 0)
                {
                    if (abrirTransacaoParaLista)
                        this.TransacaoRollback();
                    return false;
                }
            }

            if (abrirTransacaoParaLista)
                this.FinalizaTransacao();

            return true;
        }
    }
}

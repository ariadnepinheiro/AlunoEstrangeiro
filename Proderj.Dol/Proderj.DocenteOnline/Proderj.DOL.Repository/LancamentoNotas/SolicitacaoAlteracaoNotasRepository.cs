using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;
using Proderj.DOL.Domain;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace Proderj.DOL.Repository
{
	public class SolicitacaoAlteracaoNotasRepository : NHRepositoryBase<SolicitacaoAlteracaoNotas>, ISolicitacaoAlteracaoNotasRepository
	{
		#region ISolicitacaoAlteracaoNotasRepository Members

		public bool ExisteSolicitacaoAlteracaoNotasValido(SolicitacaoAlteracaoNotas solicitacao)
		{
			DateTime dataAtual = DateTime.Now;
		
			SolicitacaoAlteracaoNotas solic = Sessao.CreateCriteria<SolicitacaoAlteracaoNotas>()
			 .Add(Restrictions.Eq("NumeroFuncionario", solicitacao.NumeroFuncionario))
			 .Add(Restrictions.Eq("Status", solicitacao.Status))
			 .Add(Restrictions.Eq("Turma", solicitacao.Turma))
			 .Add(Restrictions.Eq("Disciplina", solicitacao.Disciplina))
			 .Add(Restrictions.Eq("Ano", solicitacao.Ano))
			 .Add(Restrictions.Eq("SubPeriodo", solicitacao.SubPeriodo))
			 .Add(Restrictions.Eq("UnidadeEnsino", solicitacao.UnidadeEnsino))
			 .Add(Restrictions.Le("DataStatus", dataAtual))
			 .Add(Restrictions.Ge("DataLimite", dataAtual))
			 .SetProjection
			 (
				Projections.ProjectionList()
				   .Add(Projections.Property("Id"), "Id")
			 )
			 .SetMaxResults(1)
			 .SetResultTransformer(Transformers.AliasToBean(typeof(SolicitacaoAlteracaoNotas)))
			 .UniqueResult<SolicitacaoAlteracaoNotas>();

			return solic != null;
		}

		public bool ExisteSolicitacaoAlteracaoNotas(SolicitacaoAlteracaoNotas solicitacao)
		{
			SolicitacaoAlteracaoNotas solic = Sessao.CreateCriteria<SolicitacaoAlteracaoNotas>()
			 .Add(Restrictions.Eq("NumeroFuncionario", solicitacao.NumeroFuncionario))
			 .Add(Restrictions.Eq("Status", solicitacao.Status))
			 .Add(Restrictions.Eq("Turma", solicitacao.Turma))
			 .Add(Restrictions.Eq("Disciplina", solicitacao.Disciplina))
			 .Add(Restrictions.Eq("Ano", solicitacao.Ano))
			 .Add(Restrictions.Eq("SubPeriodo", solicitacao.SubPeriodo))
			 .Add(Restrictions.Eq("UnidadeEnsino", solicitacao.UnidadeEnsino))
			 .SetProjection
			 (
				Projections.ProjectionList()
				   .Add(Projections.Property("Id"), "Id")
			 )
			 .SetMaxResults(1)
			 .SetResultTransformer(Transformers.AliasToBean(typeof(SolicitacaoAlteracaoNotas)))
			 .UniqueResult<SolicitacaoAlteracaoNotas>();

			return solic != null;
		}

		public DateTime? ObtemDataPor(SolicitacaoAlteracaoNotas solicitacao)
		{
			SolicitacaoAlteracaoNotas solic = Sessao.CreateCriteria<SolicitacaoAlteracaoNotas>()
			 .Add(Restrictions.Eq("NumeroFuncionario", solicitacao.NumeroFuncionario))
			 .Add(Restrictions.Eq("Status", solicitacao.Status))
			 .Add(Restrictions.Eq("Turma", solicitacao.Turma))
			 .Add(Restrictions.Eq("Disciplina", solicitacao.Disciplina))
			 .Add(Restrictions.Eq("Ano", solicitacao.Ano))
			 .Add(Restrictions.Eq("SubPeriodo", solicitacao.SubPeriodo))
			 .Add(Restrictions.Eq("UnidadeEnsino", solicitacao.UnidadeEnsino))
			 .SetProjection
			 (
				Projections.ProjectionList()
				   .Add(Projections.Property("DataSolicitacao"), "DataSolicitacao")
			 )
			 .SetMaxResults(1)
			 .SetResultTransformer(Transformers.AliasToBean(typeof(SolicitacaoAlteracaoNotas)))
			 .UniqueResult<SolicitacaoAlteracaoNotas>();

			if (solic != null)
				return solic.DataSolicitacao;
			else
				return null;
		}

		public void InsereSolicitacaoReabertura(SolicitacaoAlteracaoNotas solicitacao)
		{
            try
            {
                var query = SessaoAuditada.CreateSQLQuery(
                                    @"INSERT  INTO [dbo].[TCE_SOLICITACAO_ALTERACAO_NOTA]
                                (
                                  NUM_FUNC,
                                  UNIDADE_ENS,
                                  TURMA,
                                  DISCIPLINA,
                                  ANO,
                                  SEMESTRE,
                                  SUBPERIODO,
                                  STATUS,
                                  DT_STATUS,
                                  DT_SOLICITACAO,
                                  JUSTIFICATIVA
                                )
                        VALUES  (
                                  :NUM_FUNC,
                                  :UNIDADE_ENS,
                                  :TURMA,
                                  :DISCIPLINA,
                                  :ANO,
                                  :SEMESTRE,
                                  :SUBPERIODO,
                                  :STATUS,
                                  :DT_STATUS,
                                  :DT_SOLICITACAO,
                                  :JUSTIFICATIVA
                                )");

                query.SetInt64("NUM_FUNC", solicitacao.NumeroFuncionario);
                query.SetString("UNIDADE_ENS", solicitacao.UnidadeEnsino);
                query.SetString("TURMA", solicitacao.Turma);
                query.SetString("DISCIPLINA", solicitacao.Disciplina);
                query.SetInt16("ANO", solicitacao.Ano);
                query.SetInt16("SEMESTRE", solicitacao.Periodo);
                query.SetInt16("SUBPERIODO", solicitacao.SubPeriodo);
                query.SetString("STATUS", solicitacao.Status);
                query.SetDateTime("DT_STATUS", solicitacao.DataStatus);
                query.SetDateTime("DT_SOLICITACAO", solicitacao.DataSolicitacao);
                query.SetString("JUSTIFICATIVA", solicitacao.Justificativa);

                query.ExecuteUpdate();
            }
            catch (Exception)
            {
                throw new Exception("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.");
            }       
		}

		#endregion
	}
}

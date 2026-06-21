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
	public class CompetenciaHabilidadeDocenteRepository : NHRepositoryBase<CompetenciaHabilidadeDocente>, ICompetenciaHabilidadeDocenteRepository
	{
		public IEnumerable<CompetenciaHabilidadeDocente> EnumeraCompetencias(CompetenciaHabilidadeDocente competencia)
		{
			IEnumerable<CompetenciaHabilidadeDocente> competencias = Sessao.CreateCriteria<CompetenciaHabilidadeDocente>()
			 .CreateAlias("CompetenciaHabilidadeItem", "item")
			 .CreateAlias("item.CompetenciaHabilidadeGrupo", "grupo")
			 .Add(Restrictions.Eq("Matricula", competencia.Matricula))
			 .Add(Restrictions.Eq("Turma", competencia.Turma))
			 .Add(Restrictions.Eq("SubPeriodo", competencia.SubPeriodo))
			 .Add(Restrictions.Eq("grupo.Disciplina", competencia.Disciplina))
			 .Add(Restrictions.Eq("grupo.Ano", competencia.Ano))
			 .Add(Restrictions.Eq("grupo.Periodo", competencia.Periodo))
				.SetProjection
				(
				   Projections.ProjectionList()
					  .Add(Projections.Property("DataCadastro"), "DataCadastro")
				)
			 .SetResultTransformer(Transformers.AliasToBean(typeof(CompetenciaHabilidadeDocente)))
			 .List<CompetenciaHabilidadeDocente>();
			return competencias;
		}
		
		public int Insere(TOCompetenciaHabilidade_Insere competenciaHabilidadeDocente)
		{
            try
            {
                var query = SessaoAuditada.CreateSQLQuery(
                                        @"INSERT  INTO TCE_COMPETENCIA_HABILIDADE_DOCENTE
                                            (
                                            ID_COMPETENCIA_HABILIDADE_ITEM,
                                            DISCIPLINA,
                                            TURMA,
                                            ANO,
                                            PERIODO,
                                            SUBPERIODO,
                                            MATRICULA
                                            )
                                    VALUES  (
                                            :ID_COMPETENCIA_HABILIDADE_ITEM,
                                            :DISCIPLINA,
                                            :TURMA,
                                            :ANO,
                                            :PERIODO,
                                            :SUBPERIODO,
                                            :MATRICULA)"
                                            );

                query.SetInt32("ID_COMPETENCIA_HABILIDADE_ITEM", competenciaHabilidadeDocente.IdCompetenciaHabilidadeItem);
                query.SetString("DISCIPLINA", competenciaHabilidadeDocente.CodigoDisciplina);
                query.SetString("TURMA", competenciaHabilidadeDocente.CodigoTurma);
                query.SetInt16("ANO", competenciaHabilidadeDocente.Ano);
                query.SetInt16("PERIODO", competenciaHabilidadeDocente.Periodo);
                query.SetInt16("SUBPERIODO", competenciaHabilidadeDocente.SubPeriodo);
                query.SetString("MATRICULA", competenciaHabilidadeDocente.Matricula);


                return query.ExecuteUpdate();
            }
            catch (Exception)
            {
                throw new Exception("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.");
            }       
		}

		public bool Insere(IList<TOCompetenciaHabilidade_Insere> competenciasHabilidadeDocente, bool abrirTransacaoParaLista)
		{
			if (abrirTransacaoParaLista)
				this.InicializaTransacao();

			foreach (TOCompetenciaHabilidade_Insere competenciaItem in competenciasHabilidadeDocente)
			{
				int retorno = Insere(competenciaItem);
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


		public int RemoveCompetenciasAntigas(TOCompetenciaHabilidadeDocente_RemoveCompetenciasAntigas voRemoveCompetenciasAntigas)
		{
			StringBuilder stb = new StringBuilder();
            try
            {
                stb.Append("delete from Proderj.DOL.Domain.CompetenciaHabilidadeDocente ");
                stb.Append("where Matricula = :matricula ");
                stb.Append("and Turma = :turma ");
                stb.Append("and Disciplina = :disciplina ");
                stb.Append("and Ano = :ano ");
                stb.Append("and Periodo = :periodo ");
                stb.Append("and SubPeriodo = :subPeriodo ");

                var query = SessaoAuditada.CreateQuery(stb.ToString());
                query.SetParameter("matricula", voRemoveCompetenciasAntigas.Matricula);
                query.SetParameter("disciplina", voRemoveCompetenciasAntigas.CodigoDisciplina);
                query.SetParameter("turma", voRemoveCompetenciasAntigas.CodigoTurma);
                query.SetParameter("ano", voRemoveCompetenciasAntigas.Ano);
                query.SetParameter("periodo", voRemoveCompetenciasAntigas.Periodo);
                query.SetParameter("subPeriodo", voRemoveCompetenciasAntigas.SubPeriodo);

                return query.ExecuteUpdate();
            }
            catch (Exception ex)
            {

                throw new Exception("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde." + ex.Message);
            }       
		}
	}
}

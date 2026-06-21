using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;
using Proderj.Foundation.Common;
using Proderj.DOL.Domain;
using NHibernate.Criterion;
using NHibernate.Transform;
using System.Collections;

namespace Proderj.DOL.Repository
{
	public class CompetenciaHabilidadeGrupoRepository : NHRepositoryBase<CompetenciaHabilidadeGrupo>, ICompetenciaHabilidadeGrupoRepository
	{
		public int QuantidadeItensRespostaPor(CompetenciaHabilidadeGrupo competencia)
		{
			return Sessao.CreateCriteria<CompetenciaHabilidadeGrupo>()
			 .Add(Restrictions.Eq("Serie", competencia.Serie))
			 .Add(Restrictions.Eq("TipoCurso", competencia.TipoCurso))
			 .Add(Restrictions.Eq("Modalidade", competencia.Modalidade))
			 .Add(Restrictions.Eq("Curso", competencia.Curso))
			 .Add(Restrictions.Eq("SubPeriodo", competencia.SubPeriodo))
			 .Add(Restrictions.Eq("Disciplina", competencia.Disciplina))
			 .Add(Restrictions.Eq("Ano", competencia.Ano))
			 .Add(Restrictions.Eq("Periodo", competencia.Periodo))
			 .CreateCriteria("CompetenciaHabilidadeItens", "item")
			 .List()
				.Count;
		}

		public IEnumerable<CompetenciaHabilidadeGrupo> EnumeraPor(CompetenciaHabilidadeGrupo competencia)
		{
			IList competencias = Sessao.CreateCriteria<CompetenciaHabilidadeGrupo>()
			 .Add(Restrictions.Eq("Serie", competencia.Serie))
			 .Add(Restrictions.Eq("TipoCurso", competencia.TipoCurso))
			 .Add(Restrictions.Eq("Modalidade", competencia.Modalidade))
			 .Add(Restrictions.Eq("Curso", competencia.Curso))
			 .Add(Restrictions.Eq("SubPeriodo", competencia.SubPeriodo))
			 .Add(Restrictions.Eq("Disciplina", competencia.Disciplina))
			 .Add(Restrictions.Eq("Ano", competencia.Ano))
			 .Add(Restrictions.Eq("Periodo", competencia.Periodo))
			 .CreateCriteria("CompetenciaHabilidadeItens", "item")
				.SetProjection
				(
				   Projections.ProjectionList()
					  .Add(Projections.GroupProperty("Grupo"), "Grupo")
					  .Add(Projections.GroupProperty("Id"), "Id")
					  .Add(Projections.GroupProperty("item.Id"), "item.Id")
					  .Add(Projections.GroupProperty("item.CompetenciaHabilidade"), "item.Id")
					  .Add(Projections.GroupProperty("item.DataCadastro"), "item.DataCadastro")
				).List();

			return MontaCompetencias(competencias);
		}

		public IEnumerable<VOCompetenciaHabilidadeGrupoComResposta> EnumeraComRespostaPor(TOCompetenciaHabilidadeGrupo_EnumeraPor competencia)
		{
			var query = Sessao.CreateSQLQuery(
								@"SELECT G.GRUPO, I.ID_COMPETENCIA_HABILIDADE_GRUPO, I.ID_COMPETENCIA_HABILIDADE_ITEM, I.COMPETENCIA_HABILIDADE
                                , ISNULL((SELECT DISTINCT 1 FROM DBO.TCE_COMPETENCIA_HABILIDADE_DOCENTE D 
                                WHERE D.ID_COMPETENCIA_HABILIDADE_ITEM = I.ID_COMPETENCIA_HABILIDADE_ITEM 
                                AND D.MATRICULA = :MATRICULA
                                AND D.DISCIPLINA = :DISCIPLINA
                                AND D.TURMA = :TURMA
                                AND D.ANO = :ANO
                                AND D.PERIODO = :PERIODO
                                AND D.SUBPERIODO = :SUBPERIODO),0) AS RESPOSTA, I.ORDEM as ORDEM_ITEM, G.ORDEM AS ORDEM_GRUPO

                        FROM TCE_COMPETENCIA_HABILIDADE_ITEM I
                        INNER JOIN TCE_COMPETENCIA_HABILIDADE_GRUPO G 
                        ON I.ID_COMPETENCIA_HABILIDADE_GRUPO = G.ID_COMPETENCIA_HABILIDADE_GRUPO 
                        WHERE G.DISCIPLINA = :DISCIPLINA
                        AND G.CURSO = :CURSO
                        AND G.MODALIDADE = :MODALIDADE
                        AND G.TIPO_CURSO = :TIPOCURSO
                        AND G.ANO = :ANO
                        AND G.SERIE = :SERIE
                        AND G.PERIODO = :PERIODO
                        AND G.SUBPERIODO = :SUBPERIODO 
                        ORDER BY G.ORDEM , I.ORDEM");

			query.SetString("DISCIPLINA", competencia.Disciplina);
			query.SetString("CURSO", competencia.Curso);
			query.SetString("MODALIDADE", competencia.Modalidade);
			query.SetString("TIPOCURSO", competencia.TipoCurso);
			query.SetInt16("ANO", competencia.Ano);
			query.SetInt16("SERIE", competencia.Serie);
			query.SetInt16("PERIODO", competencia.Periodo);
			query.SetInt16("SUBPERIODO", competencia.SubPeriodo);
			query.SetString("MATRICULA", competencia.Matricula);
			query.SetString("TURMA", competencia.CodigoTurma);

			var resultado = query.List();

			return MontaCompetenciasComResposta(resultado);
		}

		private IEnumerable<CompetenciaHabilidadeGrupo> MontaCompetencias(IList competencias)
		{
			var dicionario = new Dictionary<int, CompetenciaHabilidadeGrupo>();

			foreach (object[] item in competencias)
			{
				var grupoId = item[1].To<int>();

				var habilidade = new CompetenciaHabilidadeItem
				{
					Id = item[2].To<int>(),
					CompetenciaHabilidade = item[3].To<string>(),
					DataCadastro = item[4].To<DateTime>()
				};

				if (!dicionario.ContainsKey(grupoId))
				{
					dicionario[grupoId] = new CompetenciaHabilidadeGrupo
					{
						Grupo = item[0].To<string>(),
						Id = grupoId
					};
				}

				dicionario[grupoId].CompetenciaHabilidadeItens.Add(habilidade);

			}
			return dicionario.Values;
		}

		private IEnumerable<VOCompetenciaHabilidadeGrupoComResposta> MontaCompetenciasComResposta(IList competencias)
		{
			var listaVO = new List<VOCompetenciaHabilidadeGrupoComResposta>();

			foreach (object[] item in competencias)
			{
				var habilidade = new VOCompetenciaHabilidadeGrupoComResposta
				{
					DescricaoGrupo = item[0].To<string>(),
					IdCompetenciaHabilidadeGrupo = item[1].To<int>(),
					IdCompetenciaHabilidadeItem = item[2].To<int>(),
					DescricaoCompetenciaHabilidade = item[3].To<string>(),
					Resposta = item[4].To<bool>(),
					OrdemResposta = item[5].To<short>(),
					OrdemGrupo = item[6].To<short>()
				};

				listaVO.Add(habilidade);
			}

			return listaVO;
		}
	}
}

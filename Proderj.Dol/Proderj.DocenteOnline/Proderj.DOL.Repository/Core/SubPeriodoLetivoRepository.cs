using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Proderj.DOL.Domain;
using Proderj.Foundation.Framework;
using NHibernate.Criterion;
using System.Linq.Expressions;
using System.Reflection;
using NHibernate.Transform;
using Proderj.Foundation.Framework.NHibernateExtension;

namespace Proderj.DOL.Repository
{
	public class SubPeriodoLetivoRepository : NHRepositoryBase<SubPeriodoLetivo>, ISubPeriodoLetivoRepository
	{
		#region ISubPeriodoLetivoRepository Members

		public Int16? ObtemAtualParaLancamentoDeNotasPor(short ano, short periodo)
		{
			return
				ObtemAtualPor(ano, periodo, x => new { x.SubPeriodo, x.Descricao });
		}

		public short? ObtemAtualPor<T>(short ano, short periodo, Expression<Func<SubPeriodoLetivo, T>> expressaoDeProjecao)
		{
			DateTime dataAtual = DateTime.Now;

			SubPeriodoLetivo subPeriodo = Sessao.CreateCriteria<SubPeriodoLetivo>()
			 .Add(Restrictions.Eq("Ano", ano))
			 .Add(Restrictions.Eq("Periodo", periodo))
			 .Add(Restrictions.Le("DataInicio", dataAtual.Date))
			 .Add(Restrictions.Ge("DataLancamento", dataAtual.Date))
			 .AddOrder(Order.Asc("SubPeriodo"))
			 .SetProjection(expressaoDeProjecao.GeraListaProjecaoLambda())
			 .SetMaxResults(1)
			 .SetResultTransformer(Transformers.AliasToBean(typeof(SubPeriodoLetivo)))
			 .UniqueResult<SubPeriodoLetivo>();

			if (subPeriodo != null)
			{
				return subPeriodo.SubPeriodo;
			}
			return null;
		}

		public short? ObtemAtualParaCurriculoMinimoPor(short ano, short periodo)
		{
			DateTime dataAtual = DateTime.Now;

			SubPeriodoLetivo subPeriodo = Sessao.CreateCriteria<SubPeriodoLetivo>()
			 .Add(Restrictions.Eq("Ano", ano))
			 .Add(Restrictions.Eq("Periodo", periodo))
			 .Add(Restrictions.Le("DataInicio", dataAtual.Date))
			 .Add(Restrictions.Ge("DataCurriculoMinimo", dataAtual.Date))
			 .AddOrder(Order.Asc("SubPeriodo"))
			 .SetProjection(
			 Projections.Distinct(
					Projections.ProjectionList()
					   .Add(Projections.Property("SubPeriodo"), "SubPeriodo")
					)
			 )
			 .SetMaxResults(1)
			 .SetResultTransformer(Transformers.AliasToBean(typeof(SubPeriodoLetivo)))
			 .UniqueResult<SubPeriodoLetivo>();

			if (subPeriodo != null)
			{
				return subPeriodo.SubPeriodo;
			}
			return null;
		}

		public bool EhAtivoParaLancamentoDeAvaliacaoDeCurriculoMinimoPor(short ano, short periodo, short subPeriodo)
		{
			ISQLQuery consulta = Sessao.CreateSQLQuery(@"SELECT 1
														FROM 
															LY_SUBPERIODO_LETIVO S
															INNER JOIN TCE_AVALIACAO_CM c
																ON c.SUBPERIODO = S.SUBPERIODO 
																AND c.ANO = s.ANO
																AND c.PERIODO = s.PERIODO
														WHERE   
															C.ANO = :Ano
															AND C.PERIODO = :Periodo
															AND C.SUBPERIODO = :Subperiodo
															AND C.HABILITADO = 1
														GROUP BY
															s.SubPeriodo");

			consulta.SetInt16("Ano", ano);
			consulta.SetInt16("Periodo", periodo);
			consulta.SetInt16("Subperiodo", subPeriodo);

			//consulta.SetResultTransformer(Transformers.AliasToBean<SubPeriodoLetivo>());

			return consulta.List().Count > 0;
		}

		public IEnumerable<SubPeriodoLetivo> EnumeraPor(short ano, short periodo)
		{
			IEnumerable<SubPeriodoLetivo> subPeriodos = Sessao.CreateCriteria<SubPeriodoLetivo>()
			 .Add(Restrictions.Eq("Ano", ano))
			 .Add(Restrictions.Eq("Periodo", periodo))
			 .SetProjection
			 (
				Projections.Distinct(
					Projections.ProjectionList()
					   .Add(Projections.Property("SubPeriodo"), "SubPeriodo")
					   .Add(Projections.Property("Descricao"), "Descricao")
				)
			 )
			 .SetResultTransformer(Transformers.AliasToBean(typeof(SubPeriodoLetivo)))
			 .List<SubPeriodoLetivo>();

			return subPeriodos;

		}

		public IEnumerable<SubPeriodoLetivo> EnumeraAtivosParaLancamentoDeCurriculoMinimoPor(short ano, short periodo)
		{
			DateTime dataAtual = DateTime.Now.Date; //Horas, minutos e segundos zerados

			IEnumerable<SubPeriodoLetivo> subPeriodos = Sessao.CreateCriteria<SubPeriodoLetivo>()
			 .Add(Restrictions.Eq("Ano", ano))
			 .Add(Restrictions.Eq("Periodo", periodo))
			 .Add(Restrictions.Ge("DataCurriculoMinimo", dataAtual.Date))
			 .Add(Restrictions.Le("DataInicio", dataAtual.Date))
			 .SetProjection
			 (
				Projections.Distinct(
					Projections.ProjectionList()
					   .Add(Projections.Property("SubPeriodo"), "SubPeriodo")
					   .Add(Projections.Property("Descricao"), "Descricao")
				)
			 )
			 .SetResultTransformer(Transformers.AliasToBean(typeof(SubPeriodoLetivo)))
			 .List<SubPeriodoLetivo>();

			return subPeriodos;

		}

		public IEnumerable<SubPeriodoLetivo> EnumeraParaAvaliacaoPor(short ano, short periodo)
		{
			return EnumeraParaAvaliacaoPor(ano, periodo, false);
		}

		public IEnumerable<SubPeriodoLetivo> EnumeraParaAvaliacaoPor(short ano, short periodo, bool acrescentaPeriodoFinal)
		{
			IList<SubPeriodoLetivo> subPeriodos = Sessao.CreateCriteria<SubPeriodoLetivo>()
			 .Add(Restrictions.Eq("Ano", ano))
			 .Add(Restrictions.Eq("Periodo", periodo))
			 .SetProjection
			 (
				Projections.Distinct(
					Projections.ProjectionList()
					   .Add(Projections.Property("SubPeriodo"), "SubPeriodo")
					   .Add(Projections.Property("Descricao"), "Descricao")
				)
			 )
			 .SetResultTransformer(Transformers.AliasToBean(typeof(SubPeriodoLetivo)))
			 .List<SubPeriodoLetivo>();

			if (acrescentaPeriodoFinal)
			{
				subPeriodos.Add(new SubPeriodoLetivo { SubPeriodo = 5, Descricao = "FINAL" });
			}

			return subPeriodos;
		}

		public bool EhAtivoParaLancamentoDeNotasPor(short ano, short periodo, short subPeriodo)
		{
			DateTime dataAtual = DateTime.Now;

			SubPeriodoLetivo subPeriodoLetivo = Sessao.CreateCriteria<SubPeriodoLetivo>()
			 .Add(Restrictions.Eq("Ano", ano))
			 .Add(Restrictions.Eq("Periodo", periodo))
			 .Add(Restrictions.Eq("SubPeriodo", subPeriodo))
			 .Add(Restrictions.Ge("DataLancamento", dataAtual.Date))
			 .Add(Restrictions.Le("DataInicio", dataAtual.Date))
			 .SetProjection
			 (
				Projections.Distinct(
					Projections.ProjectionList()
					   .Add(Projections.Property("SubPeriodo"), "SubPeriodo")
				)
			 )
			 .SetMaxResults(1)
			 .SetResultTransformer(Transformers.AliasToBean(typeof(SubPeriodoLetivo)))
			 .UniqueResult<SubPeriodoLetivo>();

			return subPeriodoLetivo != null;
		}

		public bool EhAtivoParaLancamentoDeRespostaDeCurriculoMinimoPor(short ano, short periodo, short subPeriodo)
		{
			DateTime dataAtual = DateTime.Now;

			SubPeriodoLetivo subPeriodoLetivo = Sessao.CreateCriteria<SubPeriodoLetivo>()
			 .Add(Restrictions.Eq("Ano", ano))
			 .Add(Restrictions.Eq("Periodo", periodo))
			 .Add(Restrictions.Eq("SubPeriodo", subPeriodo))
			 .Add(Restrictions.Ge("DataCurriculoMinimo", dataAtual.Date))
			 .Add(Restrictions.Le("DataInicio", dataAtual.Date))
			 .SetProjection
			 (
				Projections.Distinct(
					Projections.ProjectionList()
					   .Add(Projections.Property("SubPeriodo"), "SubPeriodo")
				)
			 )
			 .SetMaxResults(1)
			 .SetResultTransformer(Transformers.AliasToBean(typeof(SubPeriodoLetivo)))
			 .UniqueResult<SubPeriodoLetivo>();

			return subPeriodoLetivo != null;
		}

		public bool EhAntigoParaLancamentoDeNotasPor(short ano, short periodo, short subPeriodo)
		{
			DateTime dataAtual = DateTime.Now;

			SubPeriodoLetivo subPeriodoLetivo = Sessao.CreateCriteria<SubPeriodoLetivo>()
			 .Add(Restrictions.Eq("Ano", ano))
			 .Add(Restrictions.Eq("Periodo", periodo))
			 .Add(Restrictions.Eq("SubPeriodo", subPeriodo))
			 .Add(Restrictions.Lt("DataLancamento", dataAtual.Date))
			 .SetProjection
			 (
				Projections.Distinct(
					Projections.ProjectionList()
					   .Add(Projections.Property("SubPeriodo"), "SubPeriodo")
				)
			 )
			 .SetMaxResults(1)
			 .SetResultTransformer(Transformers.AliasToBean(typeof(SubPeriodoLetivo)))
			 .UniqueResult<SubPeriodoLetivo>();

			return subPeriodoLetivo != null;
		}

		public IEnumerable<SubPeriodoLetivo> EnumeraAtivosParaLancamentoDeRespostaDeCurriculoMinimoPor(TOSubPeriodoLetivo_EnumeraAtivosParaLancamentoDeRespostaDeCurriculoMinimoPor toSolicitacao)
		{

			ISQLQuery consulta =
				Sessao.CreateSQLQuery(
					@"SELECT 
						Cast(s.SubPeriodo as tinyint) As SubPeriodo
					FROM 
						TCE_COMPETENCIA_HABILIDADE_ITEM I
						INNER JOIN TCE_COMPETENCIA_HABILIDADE_GRUPO G 
							ON I.ID_COMPETENCIA_HABILIDADE_GRUPO = G.ID_COMPETENCIA_HABILIDADE_GRUPO 
						INNER JOIN LY_SUBPERIODO_LETIVO S
							ON S.SUBPERIODO = G.SUBPERIODO 
							AND S.PERIODO = G.PERIODO 
							AND S.ANO = G.ANO
					WHERE 
						G.DISCIPLINA = :disciplina
						AND G.CURSO = :curso
						AND G.MODALIDADE = :modalidade
						AND G.TIPO_CURSO = :tipoCurso
						AND G.ANO = :ano
						AND G.SERIE = :serie
						AND G.PERIODO = :periodo
					GROUP BY
						S.SubPeriodo");

			consulta.SetString("disciplina", toSolicitacao.Disciplina);
			consulta.SetString("curso", toSolicitacao.Curso);
			consulta.SetString("modalidade", toSolicitacao.Modalidade);
			consulta.SetString("tipoCurso", toSolicitacao.TipoCurso);
			consulta.SetInt16("ano", toSolicitacao.Ano);
			consulta.SetInt16("periodo", toSolicitacao.Periodo);
			consulta.SetInt16("serie", toSolicitacao.Serie);

			consulta.SetResultTransformer(Transformers.AliasToBean<SubPeriodoLetivo>());
			return consulta.List<SubPeriodoLetivo>().AsEnumerable();

		}

		public IEnumerable<SubPeriodoLetivo> EnumeraAtivosParaLancamentoDeAvaliacaoDeCurriculoMinimoPor(short ano, short periodo)
		{
			ISQLQuery consulta = Sessao.CreateSQLQuery(@"SELECT 
															Cast(s.SubPeriodo as tinyint) As SubPeriodo
														FROM 
															LY_SUBPERIODO_LETIVO S
															INNER JOIN TCE_AVALIACAO_CM c
																ON c.SUBPERIODO = S.SUBPERIODO 
																AND c.ANO = s.ANO
																AND c.PERIODO = s.PERIODO
														WHERE   
															C.ANO = :Ano
															AND C.PERIODO = :Periodo
															AND C.HABILITADO = 1
														GROUP BY
															s.SubPeriodo");

			consulta.SetInt16("Ano", ano);
			consulta.SetInt16("Periodo", periodo);

			consulta.SetResultTransformer(Transformers.AliasToBean<SubPeriodoLetivo>());
			return consulta.List<SubPeriodoLetivo>().AsEnumerable();
			
		}

		#endregion
	}
}

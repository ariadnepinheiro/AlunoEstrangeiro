using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
	public class LogAvaliacaoCurriculoMinimoDocenteMap : ClassMap<LogAvaliacaoCurriculoMinimoDocente>
	{
		public LogAvaliacaoCurriculoMinimoDocenteMap()
		{
			Table("VW_TCE_LOG_AVALIACAO_CM_DOCENTE");

			LazyLoad();

			Id(x => x.Id)
				.Column("ID_LOG_AVALIACAO_CM_DOCENTE");

			Map(x => x.IdAvaliacaoCurriculoMinimoDocente)
				.Column("ID_AVALIACAO_CM_DOCENTE").Not.Nullable();

			Map(x => x.IdAvaliacaoCurriculoMinimo)
				.Column("ID_AVALIACAO_CM")
					.Not.Nullable();

			Map(x => x.Resposta)
				.Column("RESPOSTA")
					.Not.Nullable();

			Map(x => x.Matricula)
				.Column("MATRICULA")
					.Not.Nullable()
						.Length(8);

			Map(x => x.DataCadastro)
				.Column("DT_CADASTRO")
					.Not.Nullable();

		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
	public class LogAvaliacaoCurriculoMinimoJustificativaMap : ClassMap<LogAvaliacaoCurriculoMinimoJustificativa>
	{
		public LogAvaliacaoCurriculoMinimoJustificativaMap()
		{
			Table("VW_TCE_LOG_AVALIACAO_CM_JUSTIFICATIVA");
			
			LazyLoad();
			
			Id(x => x.Id)
				.Column("ID_LOG_AVALIACAO_CM_JUSTIFICATIVA");
			
			Map(x => x.IdAvaliacaoCurriculoMinimoJustificativa)
				.Column("ID_AVALIACAO_CM_JUSTIFICATIVA");
			
			Map(x => x.Ano)
				.Column("ANO")
					.Not.Nullable();
			
			Map(x => x.Periodo)
				.Column("PERIODO")
					.Not.Nullable();
			
			Map(x => x.SubPeriodo)
				.Column("SUBPERIODO")
					.Not.Nullable();
			
			Map(x => x.Justificativa)
				.Column("JUSTIFICATIVA")
					.Not.Nullable()
						.Length(500);
			
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

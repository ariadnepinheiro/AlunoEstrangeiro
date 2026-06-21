using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
	public class AvaliacaoCurriculoMinimoDocenteMap : ClassMap<AvaliacaoCurriculoMinimoDocente>
	{
		public AvaliacaoCurriculoMinimoDocenteMap()
		{
			Table("VW_TCE_AVALIACAO_CM_DOCENTE");
			
			LazyLoad();
			
			Id(x => x.Id)
				.Column("ID_AVALIACAO_CM_DOCENTE");
			
			References(x => x.AvaliacaoCurriculoMinimo)
				.Column("ID_AVALIACAO_CM")
					.Cascade.All();

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

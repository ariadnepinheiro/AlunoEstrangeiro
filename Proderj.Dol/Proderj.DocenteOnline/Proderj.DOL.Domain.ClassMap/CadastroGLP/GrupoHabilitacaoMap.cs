using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
	public class GrupoHabilitacaoMap : ClassMap<GrupoHabilitacao>
	{
		public GrupoHabilitacaoMap()
		{
			Table("VW_LY_GRUPO_HABILITACAO");

			LazyLoad();
			
			Id(x => x.Agrupamento)
				.Column("AGRUPAMENTO");
			
			Map(x => x.Descricao)
				.Column("DESCRICAO")
					.Not.Nullable()
						.Length(200);
			
			Map(x => x.Tipo)
				.Column("TIPO")
					.Length(20);

			Map(x => x.DisponibilidadeGLPDocente)
				.Column("DISP_GLP_DOL")
					.Length(1);
			
		}
	}
}

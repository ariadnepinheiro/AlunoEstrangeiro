using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
	public class DocenteDisponivelGLPMap : ClassMap<DocenteDisponivelGLP>
	{
		public DocenteDisponivelGLPMap()
		{
            Table("VW_LY_DOCENTE_DISPONIVEL_GLP");
			
			LazyLoad();

			Id(x => x.Id)
				.Column("ID_DOCENTE_DISPONIVEL_GLP");
			
			References(x => x.Docente)
				.Column("NUM_FUNC");
			
			References(x => x.Regional)
				.Column("REGIONALID");
			
			References(x => x.Municipio)
				.Column("MUNICIPIO");

			References(x => x.GrupoHabilitacao)
				.Column("GRUPO_HABILITACAO");

			References(x => x.UnidadeEnsino)
				.Column("UNIDADE_ENS");
			
			Map(x => x.DiaSemana)
				.Column("DIA_SEMANA");
			
			Map(x => x.HoraInicio)
				.Column("HORA_INI");
			
			Map(x => x.HoraFinal)
				.Column("HORA_FIM");
			
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
	public class AlunoMap : ClassMap<Aluno>
	{
		public AlunoMap()
		{
			Table("VW_LY_ALUNO");

			LazyLoad();

			Id(x => x.MatriculaAluno)
					.Column("ALUNO")
						.Not.Nullable()
							.Length(20);

			Map(x => x.NomeCompleto)
					.Column("NOME_COMPL")
						.Not.Nullable()
							.Length(100);
		}
	}
}

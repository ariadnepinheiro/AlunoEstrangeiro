using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Data;
using Techne.Lyceum.CR;

namespace Techne.Lyceum.RN.Custom
{
	class Ly_concurso_titulacaoCustom : Ly_concurso_titulacao.CustomBase
	{
		public override string PreDelete(Ly_concurso_titulacao.Row row, TConnectionWritable cn)
		{
			string sql;
			QueryTable qt;
			//Valida duplicação de registro
			sql = @"select 1 from Ly_concurso_titulacao ct
					inner join LY_CONCURSO_DOC_TITULACOES cdt on ct.TITULACAO = cdt.TITULACAO
					where ct.TITULACAO = ?";
			qt = new QueryTable(sql);
			qt.Query(cn, row.Titulacao);
			if (qt.Rows.Count > 0)
				return string.Format("Existe Titulação do processo seletivo cadastrada para este Item.");
			return base.PreDelete(row, cn);
		}
	}
}

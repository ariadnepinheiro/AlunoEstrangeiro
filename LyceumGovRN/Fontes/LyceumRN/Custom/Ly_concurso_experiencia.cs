using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Data;
using Techne.Lyceum.CR;

namespace Techne.Lyceum.RN.Custom
{
	class Ly_concurso_experienciaCustom : Ly_concurso_experiencia.CustomBase
	{
		public override string PreDelete(Ly_concurso_experiencia.Row row, TConnectionWritable cn)
		{
			string sql;
			QueryTable qt;
			//Valida duplicação de registro
			sql = @"select 1 from Ly_concurso_experiencia ce
					inner join LY_CONCURSO_DOC_EXPERIENCIA cde on ce.experiencia = cde.EXPERIENCIA
					where ce.EXPERIENCIA = ?";
			qt = new QueryTable(sql);
			qt.Query(cn, row.Experiencia);
			if (qt.Rows.Count > 0)
				return string.Format("Existe Experiência do processo seletivo cadastrada para este Item.");
			return base.PreDelete(row, cn);
		}
	}
}
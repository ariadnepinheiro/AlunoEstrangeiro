using System;
using System.Globalization;

using Techne.Data;

using Techne.Lyceum.CR;

namespace Techne.Lyceum.RN.Custom
{
    public class Ly_concurso_docenteCustom : Ly_concurso_docente.CustomBase
    {
        public override string PreInsert(Ly_concurso_docente.Row row, TConnectionWritable cn)
        {
            return string.Empty;
        }

        public override string PreUpdate(Ly_concurso_docente.Row row, TConnectionWritable cn)
        {
            return string.Empty;
        }

        public override string PreDelete(Ly_concurso_docente.Row row, TConnectionWritable cn)
        {
			string sql;
			QueryTable qt;
			//Verifica se existe habilitação cadastrada neste processo seletivo
			sql = "select 1 from LY_CONCURSO_DOC_HABILITACAO where CONCURSO = ?";
			qt = new QueryTable(sql);
			qt.Query(cn, row.Concurso);
			if (qt.Rows.Count == 1)
				return string.Format("Existe habilitação cadastrada para este processo seletivo.");
			if (qt.Rows.Count > 1)
				return string.Format("Existem habilitações cadastradas para este processo seletivo.");
			//Verifica se existe experiência cadastrada neste processo seletivo
			sql = "select 1 from LY_CONCURSO_DOC_EXPERIENCIA where CONCURSO = ?";
			qt = new QueryTable(sql);
			qt.Query(cn, row.Concurso);
			if (qt.Rows.Count == 1)
				return string.Format("Existe experiência cadastrada para este processo seletivo.");
			if (qt.Rows.Count > 1)
				return string.Format("Existem experiências cadastradas para este processo seletivo.");
			//Verifica se existe titulação cadastrada neste processo seletivo
			sql = "select 1 from LY_CONCURSO_DOC_TITULACOES where CONCURSO = ?";
			qt = new QueryTable(sql);
			qt.Query(cn, row.Concurso);
			if (qt.Rows.Count == 1)
				return string.Format("Existe titulação cadastrada para este processo seletivo.");
			if (qt.Rows.Count > 1)
				return string.Format("Existem titulações cadastradas para este processo seletivo.");
			return base.PreDelete(row, cn);
        }
    }
}

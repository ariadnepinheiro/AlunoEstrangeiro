using System;
using System.Collections.Generic;
using System.Text;
using Techne.Lyceum.CR;
using Techne.Data;

namespace Techne.Lyceum.RN.Custom
{
    public class Ly_gradeCustom : Ly_grade.CustomBase
    {
        public override string PreDelete(Ly_grade.Row row, TConnectionWritable cn)
        {
            QueryTable qt = new QueryTable("select m.disciplina " +
                                           "from LY_ALUNO a join LY_MATRICULA m " +
                                           "on a.ALUNO = m.ALUNO " +
                                           "where a.CURSO = ? " +
                                           "and a.TURNO = ? " +
                                           "and a.CURRICULO = ? " +
                                           "and m.DISCIPLINA = ? ");

            qt.Query(cn, row.Curso, row.Turno, row.Curriculo, row.Disciplina);

            if (qt.Rows.Count > 0)
                return "Existem alunos matriculados nesta disciplina.";
            else
                return string.Empty;
        }
    }
}

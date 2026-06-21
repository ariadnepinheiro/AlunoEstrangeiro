using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Data;

namespace Techne.Lyceum.RN
{
    public class Diario
    {
        public static QueryTable ObterTurmaAluno(String aluno)
        {
            TConnection tconn = Config.CreateConnection();
            QueryTable qt = null;

            try
            {
                tconn.Open();

                qt = new QueryTable(
                    " SELECT DISTINCT m.TURMA, " +
                        " m.ANO, " +
                        " m.SEMESTRE " +
                    " FROM LY_DISCIPLINA d " +
                    " INNER JOIN LY_MATRICULA m ON d.DISCIPLINA = m.DISCIPLINA " +
                    " WHERE m.ALUNO = ? " +
                        " AND m.SIT_MATRICULA = 'Matriculado' ");
                qt.Query(tconn, aluno);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                tconn.Close();
            }
            return qt;
        }

        public static QueryTable ObterDisciplina(String aluno)
        {
            TConnection tconn = Config.CreateConnection();
            QueryTable qt = null;

            try
            {
                tconn.Open();

                qt = new QueryTable(
                    " SELECT DISTINCT d.DISCIPLINA, " +
                        " d.NOME_COMPL AS DESCRICAO " +
                    " FROM LY_DISCIPLINA d " +
                    " INNER JOIN LY_MATRICULA m ON d.DISCIPLINA = m.DISCIPLINA " +
                    " WHERE m.ALUNO = ? " +
                        " AND m.SIT_MATRICULA = 'Matriculado' " +
                    " UNION ALL" +
                    " SELECT NULL AS DISCIPLINA, " +
                        " ' <Selecione a Disciplina>' AS DESCRICAO " +
                    " UNION ALL" +
                    " SELECT '-1' AS DISCIPLINA, " +
                        " ' <Todas>' AS DESCRICAO " +
                    " ORDER BY d.NOME_COMPL ");
                qt.Query(tconn, aluno);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                tconn.Close();
            }
            return qt;
        }
    }
}

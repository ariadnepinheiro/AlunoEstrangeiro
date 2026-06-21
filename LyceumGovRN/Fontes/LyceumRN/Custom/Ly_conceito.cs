using System;
using System.Globalization;

using Techne.Data;

using Techne.Lyceum.CR;
using System.Data;

namespace Techne.Lyceum.RN.Custom
{
    public class Ly_conceitoCustom : Ly_conceito.CustomBase
    {
        public override string PreInsert(Ly_conceito.Row row, TConnectionWritable cn)
        {
            if (!string.IsNullOrEmpty(row.Conceito))
                row.Conceito = row.Conceito.Trim();
            if (!string.IsNullOrEmpty(row.Descricao))
                row.Descricao = row.Descricao.Trim();
            if (!string.IsNullOrEmpty(row.Grupo))
                row.Grupo = row.Grupo.Trim();

            return VerificarAreaValores(row, cn);
        }

        public override string PreUpdate(Ly_conceito.Row row, TConnectionWritable cn)
        {
            if (!string.IsNullOrEmpty(row.Conceito))
                row.Conceito = row.Conceito.Trim();
            if (!string.IsNullOrEmpty(row.Descricao))
                row.Descricao = row.Descricao.Trim();
            if (!string.IsNullOrEmpty(row.Grupo))
                row.Grupo = row.Grupo.Trim();

            return VerificarAreaValores(row, cn);
        }

        public override string PreDelete(Ly_conceito.Row row, TConnectionWritable cn)
        {
            if (RN.Disciplina.ExisteGrupoNotaDisciplina(row.Grupo, row.Conceito))
            {
                return "Existem disciplinas cadastradas para esse grupo.";
            }

            return string.Empty;
        }

        private string VerificarAreaValores(DataRow row, TConnectionWritable connection)
        {
            if (row != null)
            {
                string sql = " SELECT COUNT(C.CONCEITO) FROM LY_CONCEITO C INNER JOIN LY_GRUPO_CONCEITO GC ON GC.GRUPO = C.GRUPO " +
                             " AND GC.GRUPO = ? AND C.CONCEITO <> ? " +
                             " WHERE VALOR_MINIMO >= ? AND VALOR_MINIMO <= ? " +
                             " OR VALOR_MAXIMO >= ? AND VALOR_MAXIMO <= ? ";

                TCommand cmd = connection.CreateCommand();

                cmd.CommandText = sql;
                cmd.Parameters.Add(new TParameter("?", row["grupo"].ToString()));
                cmd.Parameters.Add(new TParameter("?", row["conceito"].ToString()));
                cmd.Parameters.Add(new TParameter("?", row["valor_minimo"].ToString()));
                cmd.Parameters.Add(new TParameter("?", row["valor_maximo"].ToString()));
                cmd.Parameters.Add(new TParameter("?", row["valor_minimo"].ToString()));
                cmd.Parameters.Add(new TParameter("?", row["valor_maximo"].ToString()));

                DbObject obj = cmd.ExecuteScalar();

                if (!obj.IsNull)
                {
                    if (obj.ToString().Equals("0"))
                        return string.Empty;
                }
                else
                    return string.Empty;

                return "Este intervalo de valores mínimo e máximo já está contido em outro conceito. Favor inserir um novo intervalo. \nObs.: Se o valor máximo de um intervalo é 5, o intervalo seguinte poderá iniciar em 5,1.";
                ;
            }

            return string.Empty;
        }


    }
}

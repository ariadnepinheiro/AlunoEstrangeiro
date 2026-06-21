using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Data;
using Techne.Lyceum.CR;
using Techne.Library;

namespace Techne.Lyceum.RN
{
    public class ProvaAluno : RNBase
    {

        public static void DeletaNota(Techne.Lyceum.CR.Ly_nota.Row dtNotas)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                string sql = "Delete ly_nota where aluno = ? and disciplina = ? and turma = ? and ano = ? and semestre = ? and prova = ?";
                IAE(sql, dtNotas.Aluno, dtNotas.Disciplina, dtNotas.Turma, dtNotas.Ano, dtNotas.Semestre, dtNotas.Prova);
            }
            finally
            {
                connection.Close();
            }

        }

        public static RetValue AtualizarNotas(Techne.Lyceum.CR.Ly_nota.Row dtNotas, bool useFormula, string formula)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            RetValue retorno = null;
            connection.Open(true);

            try
            {

                if (useFormula)
                {
                    string conceito = string.Empty;
                    RN.Formula.CalculaConceitoProvaAluno(formula.ToString(), dtNotas.Disciplina, dtNotas.Turma, Convert.ToInt32(dtNotas.Ano), Convert.ToInt32(dtNotas.Semestre), dtNotas.Aluno, out conceito);
                    if (!string.IsNullOrEmpty(conceito))
                    {
                        dtNotas.Conceito = conceito;
                    }
                }

                ColunasTable colunas = MontarParametros(dtNotas.Table.Columns, dtNotas);
                Ly_nota.Row.Insert(connection, dtNotas.Aluno, dtNotas.Disciplina, dtNotas.Turma, dtNotas.Ano, dtNotas.Semestre, dtNotas.Prova, dtNotas.Recuperacao_paralela, dtNotas.Sem_avaliacao, dtNotas.Justificativa, null, null, null, colunas.Colunas, colunas.ValorColuna);
                retorno = VerificarErro(connection.GetErrors());
                if (retorno != null)
                {
                    connection.Rollback();
                    return retorno;
                }

                if (retorno == null)
                {
                    return new RetValue(true, null, new ErrorList("Notas gravadas com sucesso."));
                }

            }
            finally
            {
                connection.Close();
            }

            return retorno;
        }
    }
}

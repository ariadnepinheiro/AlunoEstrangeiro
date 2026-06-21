using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Data;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN
{
    public class Historicos : RNBase
    {


        //gera ordem de lotação a partir da última de uma matrícula
        public static decimal GeraOrdem(string aluno)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            decimal ordem;
            string ordem_0;

            try
            {
                ordem_0 = Convert.ToString(TCommand.ExecuteScalar(connection, "Select max(ordem) From LY_HIST_FACULDADE where aluno = ?", aluno));
                if (!string.IsNullOrEmpty(ordem_0))
                    ordem = Convert.ToDecimal(ordem_0);
                else
                    ordem = 0;
            }
            finally
            {
                connection.Close();
            }

            return ordem + 1;
        }

        public static string ConsultaCreditos(string disciplina)
        {
            string sql = "select CREDITOS from ly_disciplina where DISCIPLINA = ? ";
            return ConsultarCampo(sql, disciplina);
        }

        public static bool VerificaExisteInstituicao(string instituicao, string aluno)
        {

            string sql = "select 1 from LY_HIST_FACULDADE where aluno = ? and OUTRA_FACULDADE = ? ";

            int retorno = ExecutarFuncao(sql, aluno, instituicao);
            if (retorno == 1)
                return true;
            else
                return false;
        }       
    }
}

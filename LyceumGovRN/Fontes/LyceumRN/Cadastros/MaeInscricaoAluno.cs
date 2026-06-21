using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN.Cadastros
{
    public class MaeInscricaoAluno
    {
        public DataTable ListaAlunosPor(string cpf)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT I.MAE_INSCRICAOID,
		                        IA.MAE_INSCRICAOALUNOID,
		                        IA.Aluno,
		                        P.NOME_COMPL,
		                        IA.MAE_VINCULOID ,
		                        V.DESCRICAO AS VINCULODESCRICAO,
		                        IA.OUTROVINCULO
                        FROM Cadastros.MAE_INSCRICAO i
		                        INNER JOIN CADASTROS.MAE_INSCRICAOALUNO IA ON I.MAE_INSCRICAOID = IA.MAE_INSCRICAOID
		                        INNER JOIN CADASTROS.MAE_VINCULO V ON IA.MAE_VINCULOID = V.MAE_VINCULOID
		                        INNER JOIN LY_ALUNO A ON IA.ALUNO = A.ALUNO
		                        INNER JOIN LY_PESSOA P ON A.PESSOA = P.PESSOA
                        WHERE I.CPF = @CPF
                        ORDER BY IA.MAE_INSCRICAOALUNOID ";

                contextQuery.Parameters.Add("@CPF", SqlDbType.VarChar, cpf); 

                dt = contexto.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                contexto.Abandon();
                string mensagem = string.Empty;
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                contexto.Dispose();
            }

            return dt;
        }
    }
}

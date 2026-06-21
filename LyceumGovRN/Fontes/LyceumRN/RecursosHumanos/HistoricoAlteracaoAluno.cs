using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.RecursosHumanos
{
    public class HistoricoAlteracaoAluno
    {
        public void Insere(DataContext contexto, Entidades.HistoricoAlteracaoAluno historicoAlteracaoAluno)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO RecursosHumanos.HISTORICOALTERACAOALUNO
                                           (PESSOA
                                           ,USUARIOID
                                           ,DATAALTERACAO)
                                     VALUES
                                           (@PESSOA, 
                                           @USUARIOID, 
                                           @DATAALTERACAO )

		                                SELECT IDENT_CURRENT('RecursosHumanos.HISTORICOALTERACAOALUNO') ";

            contextQuery.Parameters.Add("@PESSOA", SqlDbType.Decimal, historicoAlteracaoAluno.Pessoa);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, historicoAlteracaoAluno.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            historicoAlteracaoAluno.HistoricoAlteracaoAlunoId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        public DataTable ListaPor(string aluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" select HISTORICOALTERACAOALUNO_CAMPOSID, CAMPO, VALORANTERIOR, VALORATUAL, RH.DATAALTERACAO, RH.USUARIOID +' - ' + PU.NOME_COMPL as USUARIO
                                            from  LY_ALUNO A
                                                Inner Join LY_PESSOA P on A.PESSOA=P.PESSOA
                                                Inner Join RecursosHumanos.HISTORICOALTERACAOALUNO RH on A.PESSOA=RH.PESSOA 
                                                Inner Join RecursosHumanos.HISTORICOALTERACAOALUNO_CAMPOS HC on HC.HISTORICOALTERACAOALUNOID= RH.HISTORICOALTERACAOALUNOID
                                                LEFT JOIN USUARIO U ON U.USUARIO = RH.USUARIOID
                                                LEFT JOIN LY_PESSOA PU ON Pu.PESSOA = U.PESSOA_USUARIO
                                                where Aluno= @ALUNO 
                                                order by RH.DATAALTERACAO DESC ";

                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);

                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
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
                ctx.Dispose();
            }

            return dt;
        }

        public void Remove(DataContext contexto, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE RecursosHumanos.HISTORICOALTERACAOALUNO
                                    WHERE PESSOA = @PESSOA ";

            contextQuery.Parameters.Add("@PESSOA", SqlDbType.Decimal, pessoa);

            contexto.ApplyModifications(contextQuery);
        }
    }
}

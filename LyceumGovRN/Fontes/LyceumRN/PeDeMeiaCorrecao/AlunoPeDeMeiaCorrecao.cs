using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using System.Configuration;

namespace Techne.Lyceum.RN.PeDeMeiaCorrecao
{
    public class AlunoPeDeMeiaCorrecao
    {
        public DataTable ListaTurmaPor(int ano, int periodo, string unidade)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable turmas = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT C.TURMA
                                        FROM [PeDeMeiaCorrecao].[ALUNOPEDEMEIA] A
	                                        INNER JOIN [PeDeMeiaCorrecao].[ALUNOPEDEMEIACORRECAO] C ON A.ALUNO = C.ALUNO AND A.ANO = C.ANO AND A.MESREFERENCIA = C.MES
                                        WHERE A.ANO = @ANO 
	                                        AND C.PERIODO = @PERIODO 
	                                        AND C.CENSO = @UNIDADE 
                                        ORDER  BY C.TURMA ";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@UNIDADE", unidade);

                turmas = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
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
                if (ctx != null)
                    ctx.Dispose();
            }

            return turmas;
        }

        public DataTable ListaMatriculasElegiveisCorrecaoPor(string turmaReferencia, int ano, int periodo, int mes)
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            DataTable lista = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                contextQuery.Command = @"PeDeMeiaCorrecao.SP_FREQUENCIAALUNO";
                contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turmaReferencia);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
                contextQuery.Parameters.Add("@MES", SqlDbType.Int, mes);

                lista = contexto.GetDataTable(contextQuery);
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

            return lista;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class ProgramaTrabalho
    {
        public DataTable Lista()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT PROGRAMATRABALHOID,
                                               PT.WSPROGRAMASEFAZID,
                                               PT.ATIVO,
                                               PT.USUARIOID,
                                               PTS.DESCRICAO,
                                               PTS.PT,
                                               PTS.PTRES,
                                               PTS.UO
                                        FROM   PRESTACAOCONTAS.PROGRAMATRABALHO PT (NOLOCK)
                                               INNER JOIN PRESTACAOCONTAS.WSPROGRAMASEFAZ PTS (NOLOCK)
                                                       ON PT.WSPROGRAMASEFAZID = PTS.WSPROGRAMASEFAZID
                                        ORDER  BY PROGRAMATRABALHOID  ";

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

        public string ObtemPtresPor(DataContext contexto, int programaTrabalhoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT PTRES
		                                FROM PRESTACAOCONTAS.PROGRAMATRABALHO PT
		                                INNER JOIN PRESTACAOCONTAS.WSPROGRAMASEFAZ PTS (NOLOCK) ON PT.WSPROGRAMASEFAZID = PTS.WSPROGRAMASEFAZID
		                                WHERE PROGRAMATRABALHOID = @PROGRAMATRABALHOID ";

            contextQuery.Parameters.Add("@PROGRAMATRABALHOID", SqlDbType.Int, programaTrabalhoId); 

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        public ValidacaoDados ValidaAtualizaAtivo(int programaTrabalhoId, bool ativo, string usuarioId)
        {
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (programaTrabalhoId <= 0)
            {
                mensagens.Add("Campo CÓDIGO é obrigatório.");
            }

            if (usuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public void AtualizaAtivo(int programaTrabalhoId, bool ativo, string usuarioId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE PrestacaoContas.PROGRAMATRABALHO
                                             SET ATIVO = @ATIVO,
                                                 USUARIOID = @USUARIOID, 
                                                 DATAALTERACAO = @DATAALTERACAO
                                          WHERE  PROGRAMATRABALHOID = @PROGRAMATRABALHOID  ";

                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@PROGRAMATRABALHOID", SqlDbType.Int, programaTrabalhoId);

                ctx.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
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
        }
    }
}

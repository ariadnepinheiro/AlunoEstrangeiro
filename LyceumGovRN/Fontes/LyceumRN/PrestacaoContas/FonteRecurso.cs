using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Util;
using System.Data;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN.PrestacaoContas
{
   public class FonteRecurso
    {
       public DataTable Lista()
       {
           DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
           ContextQuery contextQuery = new ContextQuery();
           DataTable dt = null;

           try
           {
               contextQuery.Command = @" SELECT FONTERECURSOID,
                                               FR.[CODIGOSEFAZ],
                                               FR.DATAINICIO,
											   FR.DATAFIM,
                                               FR.USUARIOID,
                                               FRS.DESCRICAO
                                        FROM   PRESTACAOCONTAS.FONTERECURSO FR (NOLOCK)
                                               INNER JOIN [PrestacaoContas].[WSFONTERECURSOSEFAZ] FRS (NOLOCK)
                                                       ON FR.[CODIGOSEFAZ] = FRS.[CODIGOSEFAZ]
                                        ORDER  BY FONTERECURSOID  ";

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


       public DataTable ListaAtivo()
       {
           DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
           ContextQuery contextQuery = new ContextQuery();
           DataTable dt = null;

           try
           {
               contextQuery.Command = @" SELECT FONTERECURSOID,
                                               FR.[CODIGOSEFAZ],
                                               FR.DATAINICIO,
											   FR.DATAFIM,
                                               FR.USUARIOID,
                                               FRS.DESCRICAO,
                                               (FR.[CODIGOSEFAZ] + ' - ' + FRS.DESCRICAO) as DESCRICAOCOMPLETA
                                        FROM   PRESTACAOCONTAS.FONTERECURSO FR (NOLOCK)
                                               INNER JOIN [PrestacaoContas].[WSFONTERECURSOSEFAZ] FRS (NOLOCK)
                                                       ON FR.[CODIGOSEFAZ] = FRS.[CODIGOSEFAZ]
                                        WHERE GETDATE() BETWEEN DATAINICIO AND ISNULL(DATAFIM, GETDATE())
                                        ORDER  BY FR.[CODIGOSEFAZ]  ";

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

       public ValidacaoDados ValidaAtualizaDatas(int fonteRecursoId, DateTime inicio, DateTime? fim, string usuarioId)
       {
           List<string> mensagens = new List<string>();
           ValidacaoDados validacaoDados = new ValidacaoDados
           {
               Valido = false
           };

           if (fonteRecursoId <= 0)
           {
               mensagens.Add("Campo CÓDIGO é obrigatório.");
           }


           if (inicio == DateTime.MinValue)
           {
               mensagens.Add("Campo DATA INÍCIO é obrigatório.");
           }
           else if (fim != null && fim != DateTime.MinValue)
           {
               if (inicio > fim)
               {
                   mensagens.Add("A DATA INÍCIO não pode ser superior a DATA FIM.");
               }
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

       public void AtualizaDatas(int fonteRecursoId, DateTime inicio, DateTime? fim, string usuarioId)
       {
           DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
           ContextQuery contextQuery = new ContextQuery();

           try
           {
               contextQuery.Command = @" UPDATE PrestacaoContas.FONTERECURSO
                                             SET DATAINICIO = @DATAINICIO,
                                                 DATAFIM = @DATAFIM,
                                                 USUARIOID = @USUARIOID, 
                                                 DATAALTERACAO = @DATAALTERACAO
                                          WHERE  FONTERECURSOID = @FONTERECURSOID  ";

               contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, inicio);

               if (fim == null || fim == DateTime.MinValue)
               {
                   contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, DBNull.Value);
               }
               else
               {
                   contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, fim);
               }

               contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
               contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
               contextQuery.Parameters.Add("@FONTERECURSOID", SqlDbType.Int, fonteRecursoId);

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

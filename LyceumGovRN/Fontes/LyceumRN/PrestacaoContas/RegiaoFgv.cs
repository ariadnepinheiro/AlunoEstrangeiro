using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.PrestacaoContas
{
   public class RegiaoFgv
   {
       public DataTable ListaAtivo()
       {
           DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
           ContextQuery contextQuery = new ContextQuery();
           DataTable dt = null;

           try
           {
               contextQuery.Command = @"  SELECT  REGIAOFGVID, 
                                                   DESCRICAO
                                            FROM PrestacaoContas.REGIAOFGV (NOLOCK)
                                                 WHERE GETDATE() BETWEEN DATAINICIO AND ISNULL(DATAFIM, GETDATE())
                                            ORDER BY REGIAOFGVID ";

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

       public DataTable Lista()
       {
           DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
           ContextQuery contextQuery = new ContextQuery();
           DataTable dt = null;

           try
           {
               contextQuery.Command = @" SELECT  REGIAOFGVID, 
		                                        DESCRICAO, 	                                 
		                                        DATAINICIO, 
		                                        DATAFIM, 
		                                        USUARIOID, 
		                                        DATACADASTRO, 
		                                        DATAALTERACAO
                                        FROM PrestacaoContas.REGIAOFGV (NOLOCK)
                                        ORDER BY DESCRICAO ";

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

       public ValidacaoDados Valida(Entidades.RegiaoFgv regiaoFgv, bool cadastro)
       {
           List<string> mensagens = new List<string>();
           DataContext contexto = null;
           ValidacaoDados validacaoDados = new ValidacaoDados
           {
               Valido = false
           };

           if (regiaoFgv == null)
           {
               return validacaoDados;
           }

           //Verifica se é alteração
           if (!cadastro)
           {
               if (regiaoFgv.RegiaoFgvId <= 0)
               {
                   mensagens.Add("Campo CÓDIGO é obrigatório.");
               }
           }

           if (regiaoFgv.Descricao.IsNullOrEmptyOrWhiteSpace())
           {
               mensagens.Add("Campo DESCRIÇÃO é obrigatório.");
           }

           if (regiaoFgv.DataInicio == DateTime.MinValue)
           {
               mensagens.Add("Campo DATA INÍCIO é obrigatório.");
           }
           else if (regiaoFgv.DataFim != null && regiaoFgv.DataFim != DateTime.MinValue)
           {
               if (regiaoFgv.DataInicio > regiaoFgv.DataFim)
               {
                   mensagens.Add("A DATA INÍCIO não pode ser inferior a DATA FIM.");
               }
           }

           if (regiaoFgv.UsuarioId.IsNullOrEmptyOrWhiteSpace())
           {
               mensagens.Add("Campo USUARIOID é obrigatório.");
           }

           if (mensagens.Count == 0)
           {
               try
               {
                   contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                   //Verifica se já existe a descrição cadastrada
                   if (this.PossuiOutraDescricaoCadastradaPor(contexto, regiaoFgv.Descricao, regiaoFgv.RegiaoFgvId))
                   {
                       mensagens.Add("Esta DESCRIÇÃO já foi utilizada.");
                   }
               }
               catch (Exception ex)
               {
                   if (contexto != null)
                   {
                       contexto.Abandon();
                   }
                   throw new Exception(ex.Message);
               }
               finally
               {
                   if (contexto != null)
                   {
                       contexto.Dispose();
                   }
               }
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

       private bool PossuiOutraDescricaoCadastradaPor(DataContext ctx, string descricao, int regiaoFgvId)
       {
           ContextQuery contextQuery = new ContextQuery();
           bool existe = false;

           contextQuery.Command = @" SELECT COUNT(*) 
                                FROM PrestacaoContas.REGIAOFGV (NOLOCK)
                                WHERE DESCRICAO = @DESCRICAO
	                                AND REGIAOFGVID <> @REGIAOFGVID ";

           contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, descricao);
           contextQuery.Parameters.Add("@REGIAOFGVID", SqlDbType.Int, regiaoFgvId);

           if (ctx.GetReturnValue<int>(contextQuery) > 0)
           {
               existe = true;
           }

           return existe;
       }

       public void Insere(Entidades.RegiaoFgv regiaoFgv)
       {
           DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
           ContextQuery contextQuery = new ContextQuery();

           try
           {
               contextQuery.Command = @" INSERT INTO PrestacaoContas.REGIAOFGV
                                                       (DESCRICAO, 
		                                                 DATAINICIO, 
		                                                 DATAFIM, 
                                                         USUARIOID, 
                                                         DATACADASTRO, 
                                                         DATAALTERACAO) 
                                            VALUES      (@DESCRICAO, 
		                                                 @DATAINICIO, 
		                                                 @DATAFIM, 
                                                         @USUARIOID, 
                                                         @DATACADASTRO, 
                                                         @DATAALTERACAO)";

               contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, regiaoFgv.Descricao);
               contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, regiaoFgv.DataInicio);

               if (regiaoFgv.DataFim == null || regiaoFgv.DataFim == DateTime.MinValue)
               {
                   contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, DBNull.Value);
               }
               else
               {
                   contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, regiaoFgv.DataFim);
               }
               contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, regiaoFgv.UsuarioId);
               contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
               contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

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

       public void Atualiza(Entidades.RegiaoFgv regiaoFgv)
       {
           DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
           ContextQuery contextQuery = new ContextQuery();

           try
           {
               contextQuery.Command = @" UPDATE PrestacaoContas.REGIAOFGV
                                        SET    DESCRICAO = @DESCRICAO, 
		                                       DATAINICIO = @DATAINICIO, 
		                                       DATAFIM = @DATAFIM,
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  REGIAOFGVID = @REGIAOFGVID ";

               contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, regiaoFgv.Descricao);
               contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, regiaoFgv.DataInicio);

               if (regiaoFgv.DataFim == null || regiaoFgv.DataFim == DateTime.MinValue)
               {
                   contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, DBNull.Value);
               }
               else
               {
                   contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, regiaoFgv.DataFim);
               }
               contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, regiaoFgv.UsuarioId);
               contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
               contextQuery.Parameters.Add("@REGIAOFGVID", SqlDbType.Int, regiaoFgv.RegiaoFgvId);

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

       public ValidacaoDados ValidaRemocao(int regiaoFgvId)
       {
           List<string> mensagens = new List<string>();
           DataContext contexto = null;
           ValidacaoDados validacaoDados = new ValidacaoDados
           {
               Valido = false
           };

           if (regiaoFgvId <= 0)
           {
               mensagens.Add("Campo ID é obrigatório.");
           }

           if (mensagens.Count == 0)
           {
               try
               {
                   contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                   //Verifica se motivo ja foi utilizado
                   if (this.PossuiRegiaoFgvMunicipioPor(contexto, regiaoFgvId))
                   {
                       mensagens.Add("Esta região não pode ser excluída, pois já foi associada a um município.");
                   }
               }
               catch
               {
                   if (contexto != null)
                   {
                       contexto.Abandon();
                   }
                   throw;
               }
               finally
               {
                   if (contexto != null)
                   {
                       contexto.Dispose();
                   }
               }
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

       public void Remove(int regiaoFgvId)
       {
           DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
           ContextQuery contextQuery = new ContextQuery();

           try
           {
               contextQuery.Command = @" DELETE PrestacaoContas.REGIAOFGV
                            WHERE  REGIAOFGVID = @REGIAOFGVID  ";

               contextQuery.Parameters.Add("@REGIAOFGVID", SqlDbType.Int, regiaoFgvId);

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

       private bool PossuiRegiaoFgvMunicipioPor(DataContext contexto, int regiaoFgvId)
       {
           ContextQuery contextQuery = new ContextQuery();
           bool existe = false;

           contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM PrestacaoContas.REGIAOFGV__MUNICIPIO (NOLOCK)
                                    WHERE REGIAOFGVID = @REGIAOFGVID ";

           contextQuery.Parameters.Add("@REGIAOFGVID", SqlDbType.Int, regiaoFgvId);

           if (contexto.GetReturnValue<int>(contextQuery) > 0)
           {
               existe = true;
           }

           return existe;
       }

       public DataTable ListaMunicipioPor(int regiaoFgvId)
       {
           DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
           ContextQuery contextQuery = new ContextQuery();
           DataTable dt = null;

           try
           {
               contextQuery.Command = @" sELECT RM.REGIAOFGV__MUNICIPIOID,
                                               RM.REGIAOFGVID,
                                               RM.MUNICIPIOID,
                                               M.NOME
                                        FROM   PRESTACAOCONTAS.REGIAOFGV__MUNICIPIO RM (NOLOCK)
                                               INNER JOIN HADES.DBO.HD_MUNICIPIO M (NOLOCK)
                                                       ON RM.MUNICIPIOID = M.MUNICIPIO
                                        WHERE  RM.REGIAOFGVID = @REGIAOFGVID 
                                        ORDER  BY M.NOME ";

               contextQuery.Parameters.Add("@REGIAOFGVID", SqlDbType.Int, regiaoFgvId);

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

       public ValidacaoDados ValidaMunicipio(int regiaoFgvId, string municipio, string usuarioId)
       {
           List<string> mensagens = new List<string>();
           DataContext contexto = null;
           ValidacaoDados validacaoDados = new ValidacaoDados
           {
               Valido = false
           };


           if (regiaoFgvId <= 0)
           {
               mensagens.Add("Campo CÓDIGO é obrigatório.");
           }

           if (municipio.IsNullOrEmptyOrWhiteSpace())
           {
               mensagens.Add("Campo MUNICIPIO é obrigatório.");
           }

           if (usuarioId.IsNullOrEmptyOrWhiteSpace())
           {
               mensagens.Add("Campo USUARIO é obrigatório.");
           }

           if (mensagens.Count == 0)
           {
               try
               {
                   contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                   // Verifica se já existe cadastrada
                   if (this.PossuiRegiaoFgvMunicipioPor(contexto, regiaoFgvId, municipio))
                   {
                       mensagens.Add("Este município já foi cadastrado para esta região.");
                   }

                   if (this.PossuiMunicipioAssociadoPor(contexto, regiaoFgvId, municipio))
                   {
                       mensagens.Add("Este município já se encontra associado a uma região.");
                   }
               }
               catch (Exception ex)
               {
                   if (contexto != null)
                   {
                       contexto.Abandon();
                   }
                   throw new Exception(ex.Message);
               }
               finally
               {
                   if (contexto != null)
                   {
                       contexto.Dispose();
                   }
               }
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

       private bool PossuiRegiaoFgvMunicipioPor(DataContext contexto, int regiaoFgvId, string municipio)
       {
           ContextQuery contextQuery = new ContextQuery();
           bool existe = false;

           contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM PrestacaoContas.REGIAOFGV__MUNICIPIO (NOLOCK)
                                    WHERE REGIAOFGVID = @REGIAOFGVID
                                          AND MUNICIPIOID = @MUNICIPIOID ";

           contextQuery.Parameters.Add("@REGIAOFGVID", SqlDbType.Int, regiaoFgvId);
           contextQuery.Parameters.Add("@MUNICIPIOID", SqlDbType.VarChar, municipio);

           if (contexto.GetReturnValue<int>(contextQuery) > 0)
           {
               existe = true;
           }

           return existe;
       }

       private bool PossuiMunicipioAssociadoPor(DataContext contexto, int regiaoFgvId, string municipio)
       {
           ContextQuery contextQuery = new ContextQuery();
           bool existe = false;

           contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM PrestacaoContas.REGIAOFGV__MUNICIPIO (NOLOCK)
                                    WHERE REGIAOFGVID <> @REGIAOFGVID
                                          AND MUNICIPIOID = @MUNICIPIOID ";

           contextQuery.Parameters.Add("@REGIAOFGVID", SqlDbType.Int, regiaoFgvId);
           contextQuery.Parameters.Add("@MUNICIPIOID", SqlDbType.VarChar, municipio);

           if (contexto.GetReturnValue<int>(contextQuery) > 0)
           {
               existe = true;
           }

           return existe;
       }

       public void InsereMunicipio(int regiaoFgvId, string municipio, string usuarioId)
       {
           DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
           ContextQuery contextQuery = new ContextQuery();

           try
           {
               contextQuery.Command = @" INSERT INTO PrestacaoContas.REGIAOFGV__MUNICIPIO
                                                       (REGIAOFGVID
                                                       ,MUNICIPIOID
                                                       ,USUARIOID
                                                       ,DATACADASTRO
                                                       ,DATAALTERACAO)
                                                 VALUES
                                                       (@REGIAOFGVID, 
                                                       @MUNICIPIOID, 
                                                       @USUARIOID, 
                                                       @DATACADASTRO, 
                                                       @DATAALTERACAO)";

               contextQuery.Parameters.Add("@REGIAOFGVID", SqlDbType.Int, regiaoFgvId);
               contextQuery.Parameters.Add("@MUNICIPIOID", SqlDbType.VarChar, municipio);
               contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
               contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
               contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

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

       public void RemoveMunicipio(int regiaoMunicipioid)
       {
           DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
           ContextQuery contextQuery = new ContextQuery();

           try
           {
               contextQuery.Command = @" DELETE from PrestacaoContas.REGIAOFGV__MUNICIPIO 
                                         WHERE REGIAOFGV__MUNICIPIOID = @REGIAOFGV__MUNICIPIOID  ";

               contextQuery.Parameters.Add("@REGIAOFGV__MUNICIPIOID", SqlDbType.Int, regiaoMunicipioid);
 

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
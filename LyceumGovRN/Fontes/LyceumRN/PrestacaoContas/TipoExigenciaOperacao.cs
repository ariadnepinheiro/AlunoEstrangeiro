using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class TipoExigenciaOperacao
    {
        public DataTable ListaAtivo()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"  SELECT  TIPOEXIGENCIAOPERACAOID, 
                                                   DESCRICAO
                                            FROM PrestacaoContas.TipoExigenciaOperacao (NOLOCK)
                                               WHERE ATIVO =1
                                            ORDER BY TIPOEXIGENCIAOPERACAOID ";

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
                contextQuery.Command = @" SELECT  TIPOEXIGENCIAEXTRATOID, 
		                                        DESCRICAO, 	                                 
		                                        DATAINICIO, 
		                                        DATAFIM, 
		                                        USUARIOID, 
		                                        DATACADASTRO, 
		                                        DATAALTERACAO
                                        FROM PrestacaoContas.TIPOEXIGENCIAEXTRATO (NOLOCK)
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

        public ValidacaoDados Valida(Entidades.TipoExigenciaExtrato tipoExigenciaExtrato, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (tipoExigenciaExtrato == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (tipoExigenciaExtrato.TipoExigenciaExtratoId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (tipoExigenciaExtrato.Descricao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo DESCRIÇÃO é obrigatório.");
            }

            if (tipoExigenciaExtrato.DataInicio == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA INÍCIO é obrigatório.");
            }
            else if (tipoExigenciaExtrato.DataFim != null && tipoExigenciaExtrato.DataFim != DateTime.MinValue)
            {
                if (tipoExigenciaExtrato.DataInicio > tipoExigenciaExtrato.DataFim)
                {
                    mensagens.Add("A DATA INÍCIO não pode ser superior a DATA FIM.");
                }
            }

            if (tipoExigenciaExtrato.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se já existe a descrição cadastrada
                    if (this.PossuiOutraDescricaoCadastradaPor(contexto, tipoExigenciaExtrato.Descricao, tipoExigenciaExtrato.TipoExigenciaExtratoId))
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

        private bool PossuiOutraDescricaoCadastradaPor(DataContext ctx, string descricao, int tipoExigenciaExtratoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM PrestacaoContas.TIPOEXIGENCIAEXTRATO (NOLOCK)
                                WHERE DESCRICAO = @DESCRICAO
	                                AND TIPOEXIGENCIAEXTRATOID <> @TIPOEXIGENCIAEXTRATOID ";

            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, descricao);
            contextQuery.Parameters.Add("@TIPOEXIGENCIAEXTRATOID", SqlDbType.Int, tipoExigenciaExtratoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.TipoExigenciaExtrato tipoExigenciaExtrato)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO PrestacaoContas.TIPOEXIGENCIAEXTRATO
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

                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, tipoExigenciaExtrato.Descricao);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, tipoExigenciaExtrato.DataInicio);

                if (tipoExigenciaExtrato.DataFim == null || tipoExigenciaExtrato.DataFim == DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, DBNull.Value);
                }
                else
                {
                    contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, tipoExigenciaExtrato.DataFim);
                }
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, tipoExigenciaExtrato.UsuarioId);
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

        public void Atualiza(Entidades.TipoExigenciaExtrato tipoExigenciaExtrato)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE PrestacaoContas.TIPOEXIGENCIAEXTRATO
                                        SET    DESCRICAO = @DESCRICAO, 
		                                       DATAINICIO = @DATAINICIO, 
		                                       DATAFIM = @DATAFIM,
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  TIPOEXIGENCIAEXTRATOID = @TIPOEXIGENCIAEXTRATOID ";

                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, tipoExigenciaExtrato.Descricao);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, tipoExigenciaExtrato.DataInicio);

                if (tipoExigenciaExtrato.DataFim == null || tipoExigenciaExtrato.DataFim == DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, DBNull.Value);
                }
                else
                {
                    contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, tipoExigenciaExtrato.DataFim);
                }
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, tipoExigenciaExtrato.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@TIPOEXIGENCIAEXTRATOID", SqlDbType.Int, tipoExigenciaExtrato.TipoExigenciaExtratoId);

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

        public ValidacaoDados ValidaRemocao(int tipoExigenciaExtratoId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ExigenciaExtrato rnExigenciaExtrato = new ExigenciaExtrato();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (tipoExigenciaExtratoId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se motivo ja foi utilizado
                    if (rnExigenciaExtrato.PossuiTipoExigenciaExtratoPor(contexto, tipoExigenciaExtratoId))
                    {
                        mensagens.Add("Este tipo de exigência não pode ser excluído, pois já foi utilizado em um extrato.");
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

        public void Remove(int tipoExigenciaExtratoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE PrestacaoContas.TIPOEXIGENCIAEXTRATO
                            WHERE  TIPOEXIGENCIAEXTRATOID = @TIPOEXIGENCIAEXTRATOID  ";

                contextQuery.Parameters.Add("@TIPOEXIGENCIAEXTRATOID", SqlDbType.Int, tipoExigenciaExtratoId);

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

        public bool Existe(DataContext ctx, int tipoExigenciaExtratoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            
            contextQuery.Command = @" SELECT COUNT(*) 
                                      FROM   PrestacaoContas.TIPOEXIGENCIAEXTRATO (NOLOCK) 
                                      WHERE  TIPOEXIGENCIAEXTRATOID = @TIPOEXIGENCIAEXTRATOID ";

            contextQuery.Parameters.Add("@TIPOEXIGENCIAEXTRATOID", SqlDbType.Int, tipoExigenciaExtratoId);

            return ctx.GetReturnValue<int>(contextQuery) > 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class TipoContratacao
    {
        public DataTable ListaAtivo()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"  SELECT  TIPOCONTRATACAOID, 
                                                   DESCRICAO
                                            FROM PrestacaoContas.TIPOCONTRATACAO (NOLOCK)
                                                 WHERE GETDATE() BETWEEN DATAINICIO AND ISNULL(DATAFIM, GETDATE())
                                            ORDER BY TIPOCONTRATACAOID ";

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
                contextQuery.Command = @" SELECT  TIPOCONTRATACAOID, 
		                                        DESCRICAO, 	                                 
		                                        DATAINICIO, 
		                                        DATAFIM, 
		                                        USUARIOID, 
		                                        DATACADASTRO, 
		                                        DATAALTERACAO
                                        FROM PrestacaoContas.TIPOCONTRATACAO (NOLOCK)
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

        public ValidacaoDados Valida(Entidades.TipoContratacao tipoContratacao, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (tipoContratacao == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (tipoContratacao.TipoContratacaoId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (tipoContratacao.Descricao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo DESCRIÇÃO é obrigatório.");
            }

            if (tipoContratacao.DataInicio == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA INÍCIO é obrigatório.");
            }
            else if (tipoContratacao.DataFim != null && tipoContratacao.DataFim != DateTime.MinValue)
            {
                if (tipoContratacao.DataInicio > tipoContratacao.DataFim)
                {
                    mensagens.Add("A DATA INÍCIO não pode ser superior a DATA FIM.");
                }
            }

            if (tipoContratacao.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se já existe a descrição cadastrada
                    if (this.PossuiOutraDescricaoCadastradaPor(contexto, tipoContratacao.Descricao, tipoContratacao.TipoContratacaoId))
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

        private bool PossuiOutraDescricaoCadastradaPor(DataContext ctx, string descricao, int tipoContratacaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM PrestacaoContas.TIPOCONTRATACAO (NOLOCK)
                                WHERE DESCRICAO = @DESCRICAO
	                                AND TIPOCONTRATACAOID <> @TIPOCONTRATACAOID ";

            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, descricao);
            contextQuery.Parameters.Add("@TIPOCONTRATACAOID", SqlDbType.Int, tipoContratacaoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.TipoContratacao tipoContratacao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO PrestacaoContas.TIPOCONTRATACAO
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

                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, tipoContratacao.Descricao);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, tipoContratacao.DataInicio);

                if (tipoContratacao.DataFim == null || tipoContratacao.DataFim == DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, DBNull.Value);
                }
                else
                {
                    contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, tipoContratacao.DataFim);
                }
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, tipoContratacao.UsuarioId);
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

        public void Atualiza(Entidades.TipoContratacao tipoContratacao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE PrestacaoContas.TIPOCONTRATACAO
                                        SET    DESCRICAO = @DESCRICAO, 
		                                       DATAINICIO = @DATAINICIO, 
		                                       DATAFIM = @DATAFIM,
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  TIPOCONTRATACAOID = @TIPOCONTRATACAOID ";

                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, tipoContratacao.Descricao);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, tipoContratacao.DataInicio);

                if (tipoContratacao.DataFim == null || tipoContratacao.DataFim == DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, DBNull.Value);
                }
                else
                {
                    contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, tipoContratacao.DataFim);
                }
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, tipoContratacao.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@TIPOCONTRATACAOID", SqlDbType.Int, tipoContratacao.TipoContratacaoId);

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

        public ValidacaoDados ValidaRemocao(int tipoContratacaoId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            PlanoTrabalho rnPlanoTrabalho = new PlanoTrabalho();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (tipoContratacaoId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se motivo ja foi utilizado
                    if (rnPlanoTrabalho.PossuiTipoContratacaoPor(contexto, tipoContratacaoId))
                    {
                        mensagens.Add("Este tipo de contratação não pode ser excluído, pois já foi utilizado em um Projeto / Programa.");
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

        public void Remove(int tipoContratacaoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE PrestacaoContas.TIPOCONTRATACAO
                            WHERE  TIPOCONTRATACAOID = @TIPOCONTRATACAOID  ";

                contextQuery.Parameters.Add("@TIPOCONTRATACAOID", SqlDbType.Int, tipoContratacaoId);

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

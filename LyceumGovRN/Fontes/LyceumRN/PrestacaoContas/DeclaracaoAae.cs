using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class DeclaracaoAae : RNBase
    {
        public enum Periodicidade
        {
            [StringValue("Sem Periodicidade")]
            SemPeriodicidade = 0,
            [StringValue("Mensal")]
            Mensal = 1,
            [StringValue("Anual")]
            Anual = 12,
        }

        public DataTable ListaAtivo()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"  SELECT  DECLARACAOAAEID, 
                                                   DESCRICAO
                                            FROM PrestacaoContas.DECLARACAOAAE (NOLOCK)
                                                 WHERE GETDATE() BETWEEN DATAINICIO AND ISNULL(DATAFIM, GETDATE())
                                            ORDER BY DECLARACAOAAEID ";

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
                contextQuery.Command = @" SELECT  DECLARACAOAAEID, 
		                                        DESCRICAO, 	
		                                        PERIODICIDADE,
		                                        OBRIGATORIO ,                                
		                                        DATAINICIO, 
		                                        DATAFIM, 
		                                        USUARIOID, 
		                                        DATACADASTRO, 
		                                        DATAALTERACAO
                                        FROM PrestacaoContas.DECLARACAOAAE (NOLOCK)
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

        public ValidacaoDados Valida(Entidades.DeclaracaoAae declaracaoAae, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (declaracaoAae == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (declaracaoAae.DeclaracaoAaeId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (declaracaoAae.Descricao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo DESCRIÇÃO é obrigatório.");
            }

            if (declaracaoAae.Periodicidade < 0)
            {
                mensagens.Add("Campo PERIODICIDADE é obrigatório.");
            }
            else if (declaracaoAae.Periodicidade != (int)Periodicidade.SemPeriodicidade
                && declaracaoAae.Periodicidade != (int)Periodicidade.Mensal
                && declaracaoAae.Periodicidade != (int)Periodicidade.Anual)
            {
                mensagens.Add("Campo PERIODICIDADE deve ser igual a Anual, Mensal ou Sem Periodicidade.");
            }

            if (declaracaoAae.DataInicio == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA INÍCIO é obrigatório.");
            }
            else if (declaracaoAae.DataFim != null && declaracaoAae.DataFim != DateTime.MinValue)
            {
                if (declaracaoAae.DataInicio > declaracaoAae.DataFim)
                {
                    mensagens.Add("A DATA INÍCIO não pode ser superior a DATA FIM.");
                }
            }

            if (declaracaoAae.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se já existe a descrição cadastrada
                    if (this.PossuiOutraDescricaoCadastradaPor(contexto, declaracaoAae.Descricao, declaracaoAae.DeclaracaoAaeId))
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

        private bool PossuiOutraDescricaoCadastradaPor(DataContext ctx, string descricao, int declaracaoAaeId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM PrestacaoContas.DECLARACAOAAE (NOLOCK)
                                WHERE DESCRICAO = @DESCRICAO
	                                AND DECLARACAOAAEID <> @DECLARACAOAAEID ";

            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, descricao);
            contextQuery.Parameters.Add("@DECLARACAOAAEID", SqlDbType.Int, declaracaoAaeId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.DeclaracaoAae declaracaoAae)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO PrestacaoContas.DECLARACAOAAE
                                                       (DESCRICAO, 
		                                                 PERIODICIDADE,
		                                                 OBRIGATORIO, 
		                                                 DATAINICIO, 
		                                                 DATAFIM, 
                                                         USUARIOID, 
                                                         DATACADASTRO, 
                                                         DATAALTERACAO) 
                                            VALUES      (@DESCRICAO, 
		                                                 @PERIODICIDADE,
		                                                 @OBRIGATORIO, 
		                                                 @DATAINICIO, 
		                                                 @DATAFIM, 
                                                         @USUARIOID, 
                                                         @DATACADASTRO, 
                                                         @DATAALTERACAO)";

                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, declaracaoAae.Descricao);
                contextQuery.Parameters.Add("@PERIODICIDADE", SqlDbType.Int, declaracaoAae.Periodicidade);
                contextQuery.Parameters.Add("@OBRIGATORIO", SqlDbType.Bit, declaracaoAae.Obrigatorio);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, declaracaoAae.DataInicio);

                if (declaracaoAae.DataFim == null || declaracaoAae.DataFim == DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, DBNull.Value);
                }
                else
                {
                    contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, declaracaoAae.DataFim);
                }
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, declaracaoAae.UsuarioId);
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

        public void Atualiza(Entidades.DeclaracaoAae declaracaoAae)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE PrestacaoContas.DECLARACAOAAE
                                        SET    DESCRICAO = @DESCRICAO, 
		                                       PERIODICIDADE = @PERIODICIDADE, 
		                                       OBRIGATORIO = @OBRIGATORIO, 
		                                       DATAINICIO = @DATAINICIO, 
		                                       DATAFIM = @DATAFIM,
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  DECLARACAOAAEID = @DECLARACAOAAEID ";

                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, declaracaoAae.Descricao);
                contextQuery.Parameters.Add("@PERIODICIDADE", SqlDbType.Int, declaracaoAae.Periodicidade);
                contextQuery.Parameters.Add("@OBRIGATORIO", SqlDbType.Bit, declaracaoAae.Obrigatorio);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, declaracaoAae.DataInicio);

                if (declaracaoAae.DataFim == null || declaracaoAae.DataFim == DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, DBNull.Value);
                }
                else
                {
                    contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, declaracaoAae.DataFim);
                }
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, declaracaoAae.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DECLARACAOAAEID", SqlDbType.Int, declaracaoAae.DeclaracaoAaeId);

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

        public ValidacaoDados ValidaRemocao(int declaracaoAaeId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ObrigacaoFiscalAae rnObrigacaoFiscalAae = new ObrigacaoFiscalAae();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (declaracaoAaeId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se motivo ja foi utilizado
                    if (rnObrigacaoFiscalAae.PossuiDeclaracaoAaePor(contexto, declaracaoAaeId))
                    {
                        mensagens.Add("Esta declaração não pode ser excluída, pois já foi utilizada.");
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

        public void Remove(int declaracaoAaeId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE PrestacaoContas.DECLARACAOAAE
                            WHERE  DECLARACAOAAEID = @DECLARACAOAAEID  ";

                contextQuery.Parameters.Add("@DECLARACAOAAEID", SqlDbType.Int, declaracaoAaeId);

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;
namespace Techne.Lyceum.RN.Patrimonio
{
    public class PeriodoLancamento
    {
        public DataTable Lista()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT PERIODOLANCAMENTOID, 
                            ANO,
                           ANO AS ANOPERIODO,
                           DATAINICIO,
                           DATAFIM,
                           USUARIOID, 
                           DATACADASTRO, 
                           DATAALTERACAO 
                    FROM   [PATRIMONIO].[PeriodoLancamento] (NOLOCK)  ";

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

        public void Insere(Entidades.PeriodoLancamento periodoLancamento)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"  INSERT INTO Patrimonio.PERIODOLANCAMENTO
                                                (ANO,
                                                 DATAINICIO,
                                                 DATAFIM,
                                                 USUARIOID, 
                                                 DATACADASTRO, 
                                                 DATAALTERACAO) 
                                    VALUES      (@ANO,
                                                 @DATAINICIO, 
                                                 @DATAFIM,
                                                 @USUARIOID, 
                                                 @DATACADASTRO, 
                                                 @DATAALTERACAO)  ";


                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, periodoLancamento.Ano);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, periodoLancamento.DataInicio);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, periodoLancamento.DataFim);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, periodoLancamento.UsuarioId);
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

        public void Atualiza(Entidades.PeriodoLancamento periodoLancamento)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE Patrimonio.PERIODOLANCAMENTO
                                        SET    DATAINICIO = @DATAINICIO, 
                                               DATAFIM = @DATAFIM,                                               
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  PERIODOLANCAMENTOID = @PERIODOLANCAMENTOID ";

                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, periodoLancamento.DataInicio);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, periodoLancamento.DataFim);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, periodoLancamento.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@PERIODOLANCAMENTOID", SqlDbType.Int, periodoLancamento.PeriodoLancamentoId);

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

        public void Remove(int periodoLancamentoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE Patrimonio.PERIODOLANCAMENTO
                            WHERE  PERIODOLANCAMENTOID = @PERIODOLANCAMENTOID  ";

                contextQuery.Parameters.Add("@PERIODOLANCAMENTOID", SqlDbType.Int, periodoLancamentoId);

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


        public ValidacaoDados Valida(Entidades.PeriodoLancamento periodoLancamento, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (periodoLancamento == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (periodoLancamento.PeriodoLancamentoId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (periodoLancamento.Ano <= 0)
            {
                mensagens.Add("Campo ANO é obrigatório.");
            }

            if (periodoLancamento.DataInicio <= DateTime.MinValue)
            {
                mensagens.Add("Campo DATA INÍCIO é obrigatório.");
            }

            if (periodoLancamento.DataFim <= DateTime.MinValue)
            {
                mensagens.Add("Campo DATA FIM é obrigatório.");
            }

            if (periodoLancamento.DataInicio > periodoLancamento.DataFim)
            {
                mensagens.Add("O campo DATA INÍCIO deve ser menor que a DATA FIM.");
            }

            if (periodoLancamento.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    // Verifica se já existe o conceito cadstrado
                    if (this.PossuiOutroAnoCadastradoPor(contexto, periodoLancamento.Ano, periodoLancamento.PeriodoLancamentoId))
                    {
                        mensagens.Add("Este ANO já foi cadastrado.");
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

        private bool PossuiOutroAnoCadastradoPor(DataContext ctx, int ano, int periodoLancamentoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [PATRIMONIO].[PERIODOLANCAMENTO] (NOLOCK)
                                WHERE ANO = @ANO
	                                AND PERIODOLANCAMENTOID <> @PERIODOLANCAMENTOID ";

            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@PERIODOLANCAMENTOID", SqlDbType.Int, periodoLancamentoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiPeriodoLancamentoAbertoPor(int ano, DateTime data)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.PossuiPeriodoLancamentoAbertoPor(ctx, ano, data);
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

        public bool PossuiPeriodoLancamentoAbertoPor(DataContext ctx, int ano, DateTime data)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @" SELECT COUNT(*)                                        
                                            FROM [PATRIMONIO].[PERIODOLANCAMENTO] (NOLOCK)
                                            WHERE ANO = @ANO
                                            AND CAST(@DATA AS DATE) BETWEEN CAST(DATAINICIO AS DATE)
                                                                   AND     CAST(DATAFIM AS DATE)";

            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@DATA", data);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;
namespace Techne.Lyceum.RN.Reposicao
{
    public class PeriodoLancamento : RNBase
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
                           DATAINICIOGREVE,
                           DATAFIMGREVE,
                           USUARIOID, 
                           DATACADASTRO, 
                           DATAALTERACAO 
                    FROM   [Reposicao].[PeriodoLancamento] (NOLOCK)  ";

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

        public DataTable ListaPeriodoAberto()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT PERIODOLANCAMENTOID, 
                           CONVERT(varchar, DATAINICIOGREVE, 103) + ' - '+  CONVERT(varchar, DATAFIMGREVE, 103) AS DESCRICAO,
                           DATAINICIO,
                           DATAFIM,
                            DATAINICIOGREVE,
                            DATAFIMGREVE
                    FROM   [Reposicao].[PeriodoLancamento] (NOLOCK) 
                    WHERE CONVERT(DATE, GETDATE()) BETWEEN DATAINICIO AND DATAFIM  ";

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
                contextQuery.Command = @"  INSERT INTO Reposicao.PERIODOLANCAMENTO
                                                (ANO,
                                                 DATAINICIO,
                                                 DATAFIM,
                                                 DATAINICIOGREVE,
                                                 DATAFIMGREVE,
                                                 USUARIOID, 
                                                 DATACADASTRO, 
                                                 DATAALTERACAO) 
                                    VALUES      (@ANO,
                                                 @DATAINICIO, 
                                                 @DATAFIM,
                                                 @DATAINICIOGREVE, 
                                                 @DATAFIMGREVE,
                                                 @USUARIOID, 
                                                 @DATACADASTRO, 
                                                 @DATAALTERACAO)  ";


                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, periodoLancamento.Ano);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, periodoLancamento.DataInicio);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, periodoLancamento.DataFim);
                contextQuery.Parameters.Add("@DATAINICIOGREVE", SqlDbType.DateTime, periodoLancamento.DataInicioGreve);
                contextQuery.Parameters.Add("@DATAFIMGREVE", SqlDbType.DateTime, periodoLancamento.DataFimGreve);
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
                contextQuery.Command = @" UPDATE Reposicao.PERIODOLANCAMENTO
                                        SET    DATAINICIO = @DATAINICIO, 
                                               DATAFIM = @DATAFIM,
                                               DATAINICIOGREVE = @DATAINICIOGREVE, 
                                               DATAFIMGREVE = @DATAFIMGREVE,                                               
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  PERIODOLANCAMENTOID = @PERIODOLANCAMENTOID ";

                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, periodoLancamento.DataInicio);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, periodoLancamento.DataFim);
                contextQuery.Parameters.Add("@DATAINICIOGREVE", SqlDbType.DateTime, periodoLancamento.DataInicioGreve);
                contextQuery.Parameters.Add("@DATAFIMGREVE", SqlDbType.DateTime, periodoLancamento.DataFimGreve);
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
                contextQuery.Command = @" DELETE Reposicao.PERIODOLANCAMENTO
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

        public bool PossuiPeriodoAbertoPor(int periodoLancamentoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                ContextQuery contextQuery = new ContextQuery();
                bool retorno = false;

                contextQuery.Command = @" SELECT  COUNT(1)
                                        FROM Reposicao.PERIODOLANCAMENTO (NOLOCK)
                                        WHERE CONVERT(DATE, GETDATE()) BETWEEN DATAINICIO AND DATAFIM
                                              AND PERIODOLANCAMENTOID = @PERIODOLANCAMENTOID ";

                contextQuery.Parameters.Add("@PERIODOLANCAMENTOID", SqlDbType.Int, periodoLancamentoId);

                if (contexto.GetReturnValue<int>(contextQuery) > 0)
                {
                    retorno = true;
                }

                return retorno;
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
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
        }

        public Entidades.PeriodoLancamento ObtemPor(int periodoLancamentoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.ObtemPor(contexto, periodoLancamentoId);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
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
        }

        public Entidades.PeriodoLancamento ObtemPor(DataContext contexto, int periodoLancamentoId)
        {
            Entidades.PeriodoLancamento entidade = new Entidades.PeriodoLancamento();
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT *
                                        FROM Reposicao.PERIODOLANCAMENTO (NOLOCK)
                                        WHERE PERIODOLANCAMENTOID = @PERIODOLANCAMENTOID ";

            contextQuery.Parameters.Add("@PERIODOLANCAMENTOID", SqlDbType.Int, periodoLancamentoId);

            entidade = contexto.TryToBindEntity<Entidades.PeriodoLancamento>(contextQuery);

            return entidade;
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
            else if (periodoLancamento.DataInicioGreve > DateTime.MinValue && periodoLancamento.DataInicio < periodoLancamento.DataInicioGreve)
            {
                mensagens.Add("A DATA INÍCIO não deve ser menor que DATA INÍCIO DA GREVE.");
            }

            if (periodoLancamento.DataFim <= DateTime.MinValue)
            {
                mensagens.Add("Campo DATA FIM é obrigatório.");
            }

            if (periodoLancamento.DataInicio > periodoLancamento.DataFim)
            {
                mensagens.Add("O campo DATA INÍCIO deve ser menor que a DATA FIM.");
            }

            if (periodoLancamento.DataInicioGreve <= DateTime.MinValue)
            {
                mensagens.Add("Campo DATA INÍCIO GREVE é obrigatório.");
            }
            else if (periodoLancamento.Ano > 0 && periodoLancamento.DataInicioGreve.Year != periodoLancamento.Ano)
            {
                mensagens.Add("O ano da DATA INÍCIO GREVE deve ser igual ao ano do periodo de lançamento.");
            }

            if (periodoLancamento.DataFimGreve <= DateTime.MinValue)
            {
                mensagens.Add("Campo DATA FIM GREVE é obrigatório.");
            }

            if (periodoLancamento.DataInicioGreve > periodoLancamento.DataFimGreve)
            {
                mensagens.Add("O campo DATA INÍCIO GREVE deve ser menor que a DATA FIM GREVE.");
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

                    if (PossuiPeriodoGrevePor(contexto, periodoLancamento.PeriodoLancamentoId, periodoLancamento.Ano, periodoLancamento.DataInicioGreve) ||
                        PossuiPeriodoGrevePor(contexto, periodoLancamento.PeriodoLancamentoId, periodoLancamento.Ano, periodoLancamento.DataFimGreve.Value))
                    {
                        mensagens.Add("Não é possível cadastrar PERÍODOS DE GREVE INTERCALADOS.");
                    }

                    /*
                    RN4.	Crítica de Período de Lançamento
                    O sistema NÃO deve permitir que um mesmo período de lançamento seja cadastrado em um mesmo ano [M3]. 
                    
                    RN5.	Crítica de Período de Lançamento Intercalado
                    O sistema NÃO deve permitir que um mesmo período de lançamento seja cadastrado INTERCALADO a outro período pré-existente [M4].
                    */
                    //if (PossuiPeriodoLancamentoAbertoPor(contexto, periodoLancamento.PeriodoLancamentoId, periodoLancamento.Ano, periodoLancamento.DataInicio) ||
                    //    PossuiPeriodoLancamentoAbertoPor(contexto, periodoLancamento.PeriodoLancamentoId, periodoLancamento.Ano, periodoLancamento.DataFim.Value))
                    //{
                    //    mensagens.Add("Não é possível cadastrar PERÍODOS DE LANÇAMENTO INTERCALADOS.");
                    //}

                    /*
                    RN3.	Crítica de Ano
                    O sistema deve permitir que um mesmo ano seja cadastrado em um ou mais cadastros de períodos de lançamento diferentes.
                    */
                    //// Verifica se já existe o conceito cadstrado
                    //if (this.PossuiOutroAnoCadastradoPor(contexto, periodoLancamento.Ano, periodoLancamento.PeriodoLancamentoId))
                    //{
                    //    mensagens.Add("Este ANO já foi cadastrado.");
                    //}
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
                                FROM [Reposicao].[PERIODOLANCAMENTO] (NOLOCK)
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

        public bool PossuiPeriodoLancamentoAbertoPor(int? periodoLancamentoId, int ano, DateTime data)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.PossuiPeriodoLancamentoAbertoPor(ctx, periodoLancamentoId, ano, data);
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

        public bool PossuiPeriodoGrevePor(DataContext ctx, int? periodoLancamentoId, int ano, DateTime data)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @" SELECT COUNT(*)                                        
                                            FROM [Reposicao].[PERIODOLANCAMENTO] (NOLOCK)
                                            WHERE PERIODOLANCAMENTOID <> @PERIODOLANCAMENTOID 
                                            AND ANO = @ANO
                                            AND CAST(@DATA AS DATE) BETWEEN CAST(DATAINICIOGREVE AS DATE)
                                                                   AND     CAST(DATAFIMGREVE AS DATE)";

            contextQuery.Parameters.Add("@PERIODOLANCAMENTOID", (object)periodoLancamentoId ?? DBNull.Value);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@DATA", data);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        public bool PossuiPeriodoLancamentoAbertoPor(DataContext ctx, int? periodoLancamentoId, int ano, DateTime data)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @" SELECT COUNT(*)                                        
                                            FROM [Reposicao].[PERIODOLANCAMENTO] (NOLOCK)
                                            WHERE PERIODOLANCAMENTOID <> @PERIODOLANCAMENTOID 
                                            AND ANO = @ANO
                                            AND CAST(@DATA AS DATE) BETWEEN CAST(DATAINICIO AS DATE)
                                                                   AND     CAST(DATAFIM AS DATE)";

            contextQuery.Parameters.Add("@PERIODOLANCAMENTOID", (object)periodoLancamentoId ?? DBNull.Value);
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

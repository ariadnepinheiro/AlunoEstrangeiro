using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;
namespace Techne.Lyceum.RN.PeDeMeiaCorrecao
{
    public class PeriodoFrequenciaAluno
    {
        public DataTable Lista()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT PERIODOFREQUENCIAALUNOID, 
                           ANO,
                           MES,
                           ANO AS ANOPERIODO,
                           DATAINICIO,
                           DATAFIM,
                           USUARIOID, 
                           DATACADASTRO, 
                           DATAALTERACAO 
                    FROM   PeDeMeiaCorrecao.PERIODOFREQUENCIAALUNO (NOLOCK)  ";

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

        public DataTable ListaPor(int ano, int mes)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT PERIODOFREQUENCIAALUNOID, 
	                                        CONVERT(VARCHAR(20), DATAINICIO, 103) + ' - ' + CONVERT(VARCHAR(20), DATAFIM, 103) AS PERIODO
                                        FROM   PeDeMeiaCorrecao.PERIODOFREQUENCIAALUNO (NOLOCK)
                                        WHERE ANO = @ANO
	                                        AND MES = @MES ";

                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@MES", SqlDbType.Int, mes);

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

        public Entidades.PeriodoFrequenciaAluno ObtemPor(int ano, int mes)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            Entidades.PeriodoFrequenciaAluno periodo = new Techne.Lyceum.RN.PeDeMeiaCorrecao.Entidades.PeriodoFrequenciaAluno();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT *
                                        FROM PeDeMeiaCorrecao.PERIODOFREQUENCIAALUNO
                                        WHERE ANO = @ANO
	                                        AND MES = @MES ";

                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@MES", SqlDbType.Int, mes);

                periodo = contexto.TryToBindEntity<Entidades.PeriodoFrequenciaAluno>(contextQuery);

                return periodo;
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

        public void Insere(Entidades.PeriodoFrequenciaAluno periodoLancamento)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"  INSERT INTO PeDeMeiaCorrecao.PERIODOFREQUENCIAALUNO
                                                (ANO,
                                                 MES,
                                                 DATAINICIO,
                                                 DATAFIM,
                                                 USUARIOID, 
                                                 DATACADASTRO, 
                                                 DATAALTERACAO) 
                                    VALUES      (@ANO,
                                                 @MES,
                                                 @DATAINICIO, 
                                                 @DATAFIM,
                                                 @USUARIOID, 
                                                 @DATACADASTRO, 
                                                 @DATAALTERACAO)  ";


                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, periodoLancamento.Ano);
                contextQuery.Parameters.Add("@MES", SqlDbType.Int, periodoLancamento.Mes);
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

        public void Atualiza(Entidades.PeriodoFrequenciaAluno periodoLancamento)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE PeDeMeiaCorrecao.PERIODOFREQUENCIAALUNO
                                        SET    DATAINICIO = @DATAINICIO, 
                                               DATAFIM = @DATAFIM,                                               
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  PERIODOFREQUENCIAALUNOID = @PERIODOFREQUENCIAALUNOID ";

                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, periodoLancamento.DataInicio);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, periodoLancamento.DataFim);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, periodoLancamento.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@PERIODOFREQUENCIAALUNOID", SqlDbType.Int, periodoLancamento.PeriodoFrequenciaAlunoId);

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

        public void Remove(int periodoFrequenciaAlunoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE PeDeMeiaCorrecao.PERIODOFREQUENCIAALUNO
                            WHERE  PERIODOFREQUENCIAALUNOID = @PERIODOFREQUENCIAALUNOID  ";

                contextQuery.Parameters.Add("@PERIODOFREQUENCIAALUNOID", SqlDbType.Int, periodoFrequenciaAlunoId);

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

        public ValidacaoDados Valida(Entidades.PeriodoFrequenciaAluno periodoLancamento, bool cadastro)
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
                if (periodoLancamento.PeriodoFrequenciaAlunoId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (periodoLancamento.Ano <= 0)
            {
                mensagens.Add("Campo ANO é obrigatório.");
            }

            if (periodoLancamento.Mes <= 0)
            {
                mensagens.Add("Campo MÊS é obrigatório.");
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

                    // Verifica se já existe ano/mes/inicio cadstrado
                    if (this.PossuiOutroPeriodoCadastradoPor(contexto, periodoLancamento.Ano, periodoLancamento.Mes, periodoLancamento.DataInicio, periodoLancamento.PeriodoFrequenciaAlunoId))
                    {
                        mensagens.Add("Este ANO/MÊS/INÍCIO já foi cadastrado.");
                    }

                    //Verifica se a data de inicio está intercalada com outro
                    if (this.PossuiDataEmOutroIntervaloPor(contexto, periodoLancamento.Ano, periodoLancamento.Mes, periodoLancamento.DataInicio, periodoLancamento.PeriodoFrequenciaAlunoId))
                    {
                        mensagens.Add("DATA INÍCIO não pode estar dentro do intervalo de outro periodo desse ANO / MÊS.");
                    }

                    //Verifica se a data de inicio está intercalada com outro
                    if (this.PossuiDataEmOutroIntervaloPor(contexto, periodoLancamento.Ano, periodoLancamento.Mes, periodoLancamento.DataFim, periodoLancamento.PeriodoFrequenciaAlunoId))
                    {
                        mensagens.Add("DATA FIM não pode estar dentro do intervalo de outro periodo desse ANO / MÊS.");
                    }

                    //Verifica se as datas de inicio e de fim estão intercalada com outro
                    if (this.PossuiOutraIntercaladaPor(contexto, periodoLancamento.Ano, periodoLancamento.Mes, periodoLancamento.DataInicio, periodoLancamento.DataFim, periodoLancamento.PeriodoFrequenciaAlunoId))
                    {
                        mensagens.Add("DATA INÍCIO E FIM não podem intercalar com outro periodo desse ANO / MÊS.");
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

        private bool PossuiDataEmOutroIntervaloPor(DataContext ctx, int ano, int mes, DateTime data, int PERIODOFREQUENCIAALUNOId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM  PeDeMeiaCorrecao.PERIODOFREQUENCIAALUNO  (NOLOCK)
                                    WHERE ANO = @ANO
	                                    AND MES = @MES
                                        AND PERIODOFREQUENCIAALUNOID <> @PERIODOFREQUENCIAALUNOID
	                                    AND @DATA BETWEEN DATAINICIO AND 
			                                    CONVERT(DATE, CONVERT(DATETIME, ISNULL(DATAFIM, GETDATE())) ) ";

            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@MES", SqlDbType.Int, mes);
            contextQuery.Parameters.Add("@PERIODOFREQUENCIAALUNOID", SqlDbType.Int, PERIODOFREQUENCIAALUNOId);
            contextQuery.Parameters.Add("@DATA", SqlDbType.Date, data.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        private bool PossuiOutraIntercaladaPor(DataContext ctx, int ano, int mes, DateTime dataInicio, DateTime dataFim, int periodoFrequenciaAlunoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM   PeDeMeiaCorrecao.PERIODOFREQUENCIAALUNO  (NOLOCK)
                                    WHERE ANO = @ANO
	                                    AND MES = @MES
	                                    AND PERIODOFREQUENCIAALUNOID <> @PERIODOFREQUENCIAALUNOID
	                                    AND @DATAINICIO <= CONVERT(DATE, DATAINICIO) 
	                                    AND @DATAFIM >= CONVERT(DATE, DATAFIM) ";

            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@MES", SqlDbType.Int, mes);
            contextQuery.Parameters.Add("@PERIODOFREQUENCIAALUNOID", SqlDbType.Int, periodoFrequenciaAlunoId);
            contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.Date, dataInicio.Date);
            contextQuery.Parameters.Add("@DATAFIM", SqlDbType.Date, dataFim.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        private bool PossuiOutroPeriodoCadastradoPor(DataContext ctx, int ano, int mes, DateTime inicio, int periodoFrequenciaAlunoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM PeDeMeiaCorrecao.PERIODOFREQUENCIAALUNO (NOLOCK)
                                WHERE ANO = @ANO
                                    AND MES = @MES
                                    AND DATAINICIO = @DATAINICIO
	                                AND PERIODOFREQUENCIAALUNOID <> @PERIODOFREQUENCIAALUNOID ";

            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@MES", SqlDbType.Int, mes);
            contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.Date, inicio);
            contextQuery.Parameters.Add("@PERIODOFREQUENCIAALUNOID", SqlDbType.Int, periodoFrequenciaAlunoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiPeriodoLancamentoAbertoPor(int ano, int mes, DateTime data)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.PossuiPeriodoLancamentoAbertoPor(ctx, ano, mes, data);
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

        public bool PossuiPeriodoLancamentoAbertoPor(DataContext ctx, int ano, int mes, DateTime data)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @" SELECT COUNT(*)                                        
                                            FROM PeDeMeiaCorrecao.PERIODOFREQUENCIAALUNO (NOLOCK)
                                            WHERE ANO = @ANO
                                            AND MES = @MES
                                            AND CAST(@DATA AS DATE) BETWEEN CAST(DATAINICIO AS DATE)
                                                                   AND     CAST(DATAFIM AS DATE)";

            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@MES", mes);
            contextQuery.Parameters.Add("@DATA", data);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
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
                                            FROM PeDeMeiaCorrecao.PERIODOFREQUENCIAALUNO (NOLOCK)
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

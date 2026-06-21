using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;
namespace Techne.Lyceum.RN.RecursosHumanos
{
    public class PeriodoAlteracaoAluno
    {
        public DataTable Lista()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT PERIODOALTERACAOALUNOID, 
                                           E.ANO,
                                           E.ANO AS ANOPERIODO,
                                           E.DATAINICIO,
                                           E.DATAFIM,
                                           E.USUARIOID + ' - ' + P.NOME_COMPL AS USUARIOID,
                                           E.DATACADASTRO, 
                                           E.DATAALTERACAO 
                                          FROM [RECURSOSHUMANOS].PERIODOALTERACAOALUNO E (NOLOCK)
                                               LEFT JOIN USUARIO U ON U.USUARIO = E.USUARIOID
                                               LEFT JOIN LY_PESSOA P ON P.PESSOA = U.PESSOA_USUARIO
                                        ";

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

        public DataTable ListaPor(int ano)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT PERIODOALTERACAOALUNOID, 
	                                        CONVERT(VARCHAR(20), DATAINICIO, 103) + ' - ' + CONVERT(VARCHAR(20), DATAFIM, 103) AS PERIODO
                                          FROM  [RECURSOSHUMANOS].PERIODOALTERACAOALUNO (NOLOCK)
                                          WHERE ANO = @ANO ";

                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);

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

        public Entidades.PeriodoAlteracaoAluno ObtemPor(int ano)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            Entidades.PeriodoAlteracaoAluno periodo = new Techne.Lyceum.RN.RecursosHumanos.Entidades.PeriodoAlteracaoAluno();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT *
                                          FROM RECURSOSHUMANOS.PERIODOALTERACAOALUNO
                                          WHERE ANO = @ANO ";

                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);

                periodo = contexto.TryToBindEntity<Entidades.PeriodoAlteracaoAluno>(contextQuery);

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

        public void Insere(Entidades.PeriodoAlteracaoAluno periodoAlteracao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"  INSERT INTO RECURSOSHUMANOS.PERIODOALTERACAOALUNO
                                                    (ANO,
                                                     DATAINICIO,
                                                     DATAFIM,
                                                     USUARIOID, 
                                                     DATACADASTRO, 
                                                     DATAALTERACAO) 
                                             VALUES (@ANO,
                                                     @DATAINICIO, 
                                                     @DATAFIM,
                                                     @USUARIOID, 
                                                     @DATACADASTRO, 
                                                     @DATAALTERACAO)  ";

                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, periodoAlteracao.Ano);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, periodoAlteracao.DataInicio);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, periodoAlteracao.DataFim);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, periodoAlteracao.UsuarioId);
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

        public void Atualiza(Entidades.PeriodoAlteracaoAluno periodoAlteracao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE RECURSOSHUMANOS.PERIODOALTERACAOALUNO
                                          SET    DATAINICIO = @DATAINICIO, 
                                                 DATAFIM = @DATAFIM,                                               
                                                 USUARIOID = @USUARIOID, 
                                                 DATAALTERACAO = @DATAALTERACAO 
                                          WHERE  PERIODOALTERACAOALUNOID = @PERIODOALTERACAOALUNOID ";

                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, periodoAlteracao.DataInicio);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, periodoAlteracao.DataFim);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, periodoAlteracao.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@PERIODOALTERACAOALUNOID", SqlDbType.Int, periodoAlteracao.PeriodoAlteracaoAlunoId);

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

        public void Remove(int periodoAlteracao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE RECURSOSHUMANOS.PERIODOALTERACAOALUNO
                                          WHERE  PERIODOALTERACAOALUNOID = @PERIODOALTERACAOALUNOID ";

                contextQuery.Parameters.Add("@PERIODOALTERACAOALUNOID", SqlDbType.Int, periodoAlteracao);

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

        public ValidacaoDados Valida(Entidades.PeriodoAlteracaoAluno periodoAlteracaoAluno, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (periodoAlteracaoAluno == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (periodoAlteracaoAluno.PeriodoAlteracaoAlunoId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (periodoAlteracaoAluno.Ano <= 0)
            {
                mensagens.Add("Campo ANO é obrigatório.");
            }

            if (periodoAlteracaoAluno.DataInicio <= DateTime.MinValue)
            {
                mensagens.Add("Campo DATA INÍCIO é obrigatório.");
            }
            else
            {
                if (periodoAlteracaoAluno.DataInicio.Date < DateTime.Now.Date && cadastro)
                {
                    mensagens.Add("DATA INÍCIO não pode ser anterior a data corrente.");
                }

                if (periodoAlteracaoAluno.DataInicio.Year != periodoAlteracaoAluno.Ano)
                {
                    mensagens.Add("DATA INÍCIO tem que estar dentro do ANO escolhido.");
                }
            }

            if (periodoAlteracaoAluno.DataFim <= DateTime.MinValue)
            {
                mensagens.Add("Campo DATA FIM é obrigatório.");
            }
            else
            {
                if (periodoAlteracaoAluno.DataFim.Year != periodoAlteracaoAluno.Ano)
                {
                    mensagens.Add("DATA FIM tem que estar dentro do ANO escolhido.");
                }
            }

            if (periodoAlteracaoAluno.DataInicio > periodoAlteracaoAluno.DataFim)
            {
                mensagens.Add("O campo DATA INÍCIO deve ser menor que a DATA FIM.");
            }

            if (periodoAlteracaoAluno.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    // Verifica se já existe ano/inicio cadstrado
                    if (this.PossuiOutroPeriodoCadastradoPor(contexto, periodoAlteracaoAluno.Ano, periodoAlteracaoAluno.DataInicio, periodoAlteracaoAluno.PeriodoAlteracaoAlunoId))
                    {
                        mensagens.Add("Este ANO/INÍCIO já foi cadastrado.");
                    }

                    //Verifica se a data de inicio está intercalada com outro
                    if (this.PossuiDataEmOutroIntervaloPor(contexto, periodoAlteracaoAluno.Ano, periodoAlteracaoAluno.DataInicio, periodoAlteracaoAluno.PeriodoAlteracaoAlunoId))
                    {
                        mensagens.Add("DATA INÍCIO não pode estar dentro do intervalo de outro periodo desse ANO.");
                    }

                    //Verifica se a data de inicio está intercalada com outro
                    if (this.PossuiDataEmOutroIntervaloPor(contexto, periodoAlteracaoAluno.Ano, periodoAlteracaoAluno.DataFim, periodoAlteracaoAluno.PeriodoAlteracaoAlunoId))
                    {
                        mensagens.Add("DATA FIM não pode estar dentro do intervalo de outro periodo desse ANO.");
                    }

                    //Verifica se as datas de inicio e de fim estão intercalada com outro
                    if (this.PossuiOutraIntercaladaPor(contexto, periodoAlteracaoAluno.Ano, periodoAlteracaoAluno.DataInicio, periodoAlteracaoAluno.DataFim, periodoAlteracaoAluno.PeriodoAlteracaoAlunoId))
                    {
                        mensagens.Add("DATA INÍCIO E FIM não podem intercalar com outro periodo desse ANO.");
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

        private bool PossuiDataEmOutroIntervaloPor(DataContext ctx, int ano, DateTime data, int PeriodoAlteracaoAluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                      FROM  [RECURSOSHUMANOS].PERIODOALTERACAOALUNO  (NOLOCK)
                                      WHERE ANO = @ANO
                                        AND PERIODOALTERACAOALUNOID <> @PERIODOALTERACAOALUNOID
	                                    AND @DATA BETWEEN DATAINICIO AND 
			                                CONVERT(DATE, CONVERT(DATETIME, ISNULL(DATAFIM, GETDATE())) ) ";

            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@PERIODOALTERACAOALUNOID", SqlDbType.Int, PeriodoAlteracaoAluno);
            contextQuery.Parameters.Add("@DATA", SqlDbType.Date, data.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        private bool PossuiOutraIntercaladaPor(DataContext ctx, int ano, DateTime dataInicio, DateTime dataFim, int PeriodoAlteracaoAluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                      FROM  [RECURSOSHUMANOS].PERIODOALTERACAOALUNO (NOLOCK)
                                      WHERE ANO = @ANO
	                                    AND PERIODOALTERACAOALUNOID <> @PERIODOALTERACAOALUNOID
	                                    AND @DATAINICIO <= CONVERT(DATE, DATAINICIO) 
	                                    AND @DATAFIM >= CONVERT(DATE, DATAFIM) ";

            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@PERIODOALTERACAOALUNOID", SqlDbType.Int, PeriodoAlteracaoAluno);
            contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.Date, dataInicio.Date);
            contextQuery.Parameters.Add("@DATAFIM", SqlDbType.Date, dataFim.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        private bool PossuiOutroPeriodoCadastradoPor(DataContext ctx, int ano, DateTime inicio, int PeriodoAlteracaoAluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                      FROM [RECURSOSHUMANOS].PERIODOALTERACAOALUNO (NOLOCK)
                                      WHERE ANO = @ANO
                                        AND DATAINICIO = @DATAINICIO
	                                    AND PERIODOALTERACAOALUNOID <> @PERIODOALTERACAOALUNOID";

            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.Date, inicio);
            contextQuery.Parameters.Add("@PERIODOALTERACAOALUNOID", SqlDbType.Int, PeriodoAlteracaoAluno);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiPeriodoAlteracaoAlunoAbertoPor(int ano, DateTime data)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.PossuiPeriodoAlteracaoAlunoAbertoPor(ctx, ano, data);
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

        public bool PossuiPeriodoAlteracaoAlunoAbertoPor(DataContext ctx, int ano, DateTime data)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @" SELECT COUNT(*)                                        
                                      FROM [RECURSOSHUMANOS].PERIODOALTERACAOALUNO (NOLOCK)
                                      WHERE ANO = @ANO
                                        AND CAST(@DATA AS DATE) BETWEEN CAST(DATAINICIO AS DATE)
                                        AND CAST(DATAFIM AS DATE)";

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
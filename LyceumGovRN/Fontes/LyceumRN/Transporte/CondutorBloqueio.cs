using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Transporte
{
    public class CondutorBloqueio
    {
        public DataTable Lista()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT CONDUTORBLOQUEIOID, 
                                           P.CONDUTORID, 
                                           P.NOME, 
                                           P.CPF, 
                                           PB.DATABLOQUEIO, 
                                           M.DESCRICAO           AS MOTIVOBLOQUEIO, 
                                           M.MOTIVOBLOQUEIOID, 
                                           PB.OBSERVACAO, 
                                           UB.NOME               AS USUARIOBLOQUEIO, 
	                                       PB.DATADESBLOQUEIO, 
                                           UD.NOME               AS USUARIODESBLOQUEIO, 
                                           M.TIPO 
                                    FROM   [TRANSPORTE].[CONDUTORBLOQUEIO] PB (NOLOCK) 
                                           INNER JOIN [TRANSPORTE].[CONDUTOR] P (NOLOCK) 
                                                   ON PB.CONDUTORID = P.CONDUTORID 
                                           INNER JOIN [TRANSPORTE].[MOTIVOBLOQUEIO] M (NOLOCK) 
                                                   ON PB.MOTIVOBLOQUEIOID = M.MOTIVOBLOQUEIOID 
                                           LEFT JOIN HADES.DBO.HD_USUARIO UB (NOLOCK) 
                                                  ON UB.USUARIO = PB.USUARIOBLOQUEIOID 
                                           LEFT JOIN HADES.DBO.HD_USUARIO UD (NOLOCK) 
                                                  ON UD.USUARIO = PB.USUARIODESBLOQUEIOID 
                                    ORDER BY PB.DATABLOQUEIO DESC ";

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

        public ValidacaoDados ValidaBloqueio(Entidades.CondutorBloqueio condutorBloqueio, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (condutorBloqueio == null)
            {
                return validacaoDados;
            }

            if (!cadastro)
            {
                if (condutorBloqueio.CondutorBloqueioId <= 0)
                {
                    mensagens.Add("Campo CODIGO é obrigatório.");
                }
            }

            if (condutorBloqueio.CondutorId <= 0)
            {
                mensagens.Add("Campo CONDUTOR é obrigatório.");
            }

            if (condutorBloqueio.MotivoBloqueioId <= 0)
            {
                mensagens.Add("Campo MOTIVO DO BLOQUEIO é obrigatório.");
            }

            if (!condutorBloqueio.Observacao.IsNullOrEmptyOrWhiteSpace() && condutorBloqueio.Observacao.Length > 500)
            {
                mensagens.Add("Campo OBSERVAÇÃO deve conter no máximo 500 caracteres.");
            }

            if (condutorBloqueio.DataBloqueio == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA DO BLOQUEIO é obrigatório.");
            }
            else if (condutorBloqueio.DataBloqueio.Date > DateTime.Now.Date)
            {
                mensagens.Add("Campo DATA DO BLOQUEIO não pode deve ser maior que a data atual.");
            }

            if (condutorBloqueio.UsuarioBloqueioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO RESPONSÁVEL PELO BLOQUEIO é obrigatório.");
            }            

            if (mensagens.Count == 0)
            {
                try
                {
                    condutorBloqueio.DataDesbloqueio = null;
                    condutorBloqueio.UsuarioDesbloqueioId = null;

                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se já existe um bloqueio sem desbloqueio
                    if (this.PossuiOutroBloqueioAbertoPor(contexto, condutorBloqueio.CondutorBloqueioId, condutorBloqueio.CondutorId))
                    {
                        mensagens.Add("Este condutor já possui um bloqueio em aberto.");
                    }
                    else
                    {
                        //Verifica se já existe um bloqueio no mesmo intervalo de tempo
                        if (this.PossuiOutroBloqueioPor(contexto, condutorBloqueio.CondutorBloqueioId, condutorBloqueio.CondutorId, condutorBloqueio.DataBloqueio)
                            || this.PossuiOutroPosteriorPor(contexto, condutorBloqueio.CondutorBloqueioId, condutorBloqueio.CondutorId, condutorBloqueio.DataBloqueio))
                        {
                            mensagens.Add("Este condutor já está bloqueado neste periodo.");
                        }
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

        public List<DateTime> RetornaDiasBloqueiosPor(DataContext contexto, int condutorId, DateTime dataInicio, DateTime dataFim)
        {
            List<DateTime> diasBloqueados = new List<DateTime>();

            for (DateTime i = dataInicio; i.Date <= dataFim.Date; i = i.AddDays(1))
            {
                DateTime data = i;

                //Verificar se possui bloqueio
                if (this.PossuiBloqueioAbertoPor(contexto, condutorId, data))
                {
                    diasBloqueados.Add(data);
                }
            }

            return diasBloqueados;
        }       

        public bool PossuiBloqueioAbertoPor(DataContext ctx, int condutorId, DateTime data)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [TRANSPORTE].[CONDUTORBLOQUEIO] (NOLOCK)
                                WHERE CONDUTORID = @CONDUTORID
									AND CONVERT(DATE, DATABLOQUEIO) <= CONVERT(DATE, @DATA)
									AND (DATADESBLOQUEIO IS NULL OR CONVERT(DATE, DATADESBLOQUEIO) >= CONVERT(DATE, @DATA)) ";

            contextQuery.Parameters.Add("@CONDUTORID", SqlDbType.Int, condutorId);
            contextQuery.Parameters.Add("@DATA", SqlDbType.DateTime, data.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiOutroBloqueioAbertoPor(DataContext ctx, int condutorBloqueioId, int condutorId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [TRANSPORTE].[CONDUTORBLOQUEIO] (NOLOCK)
                                WHERE CONDUTORBLOQUEIOID <> @CONDUTORBLOQUEIOID
									AND CONDUTORID = @CONDUTORID
									AND (DATADESBLOQUEIO IS NULL OR CONVERT(DATE, DATADESBLOQUEIO) >= CONVERT(DATE, GETDATE())) ";

            contextQuery.Parameters.Add("@CONDUTORBLOQUEIOID", SqlDbType.Int, condutorBloqueioId);
            contextQuery.Parameters.Add("@CONDUTORID", SqlDbType.Int, condutorId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiOutroPosteriorPor(DataContext ctx, int condutorBloqueioId, int condutorId, DateTime dataBloqueio)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [TRANSPORTE].[CONDUTORBLOQUEIO] (NOLOCK)
                                WHERE CONDUTORBLOQUEIOID <> @CONDUTORBLOQUEIOID
									AND CONDUTORID = @CONDUTORID
									AND (DATABLOQUEIO >  @DATABLOQUEIO OR DATADESBLOQUEIO > @DATABLOQUEIO) ";

            contextQuery.Parameters.Add("@CONDUTORBLOQUEIOID", SqlDbType.Int, condutorBloqueioId);
            contextQuery.Parameters.Add("@CONDUTORID", SqlDbType.Int, condutorId);
            contextQuery.Parameters.Add("@DATABLOQUEIO", SqlDbType.DateTime, dataBloqueio.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiOutroBloqueioPor(DataContext ctx, int condutorBloqueioId, int condutorId, DateTime dataBloqueio)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [TRANSPORTE].[CONDUTORBLOQUEIO] (NOLOCK)
                                WHERE CONDUTORBLOQUEIOID <> @CONDUTORBLOQUEIOID
									AND CONDUTORID = @CONDUTORID
									AND DATADESBLOQUEIO IS NOT NULL									
									AND @DATABLOQUEIO BETWEEN DATABLOQUEIO
                                                           AND DATADESBLOQUEIO ";

            contextQuery.Parameters.Add("@CONDUTORBLOQUEIOID", SqlDbType.Int, condutorBloqueioId);
            contextQuery.Parameters.Add("@CONDUTORID", SqlDbType.Int, condutorId);
            contextQuery.Parameters.Add("@DATABLOQUEIO", SqlDbType.DateTime, dataBloqueio.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Bloqueia(Entidades.CondutorBloqueio condutorBloqueio)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO Transporte.CONDUTORBLOQUEIO
                                                   (CONDUTORID
                                                   ,MOTIVOBLOQUEIOID
                                                   ,OBSERVACAO
                                                   ,USUARIOBLOQUEIOID
                                                   ,DATABLOQUEIO)
                                             VALUES
                                                   (@CONDUTORID,
                                                   @MOTIVOBLOQUEIOID,
                                                   @OBSERVACAO, 
                                                   @USUARIOBLOQUEIOID, 
                                                   @DATABLOQUEIO) ";

                contextQuery.Parameters.Add("@CONDUTORID", SqlDbType.Int, condutorBloqueio.CondutorId);
                contextQuery.Parameters.Add("@MOTIVOBLOQUEIOID", SqlDbType.Int, condutorBloqueio.MotivoBloqueioId);
                contextQuery.Parameters.Add("@OBSERVACAO", SqlDbType.VarChar, condutorBloqueio.Observacao);
                contextQuery.Parameters.Add("@USUARIOBLOQUEIOID", SqlDbType.VarChar, condutorBloqueio.UsuarioBloqueioId);
                contextQuery.Parameters.Add("@DATABLOQUEIO", SqlDbType.DateTime, condutorBloqueio.DataBloqueio.Date);

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

        public ValidacaoDados ValidaDesbloqueio(int condutorBloqueioId, DateTime dataDesbloqueio, string usuarioDesbloqueioId, DateTime dataBloqueio)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (condutorBloqueioId <= 0)
            {
                mensagens.Add("Campo CODIGO é obrigatório.");
            }

            if (dataBloqueio == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA DO BLOQUEIO é obrigatório.");
            }

            if (dataDesbloqueio == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA DO DESBLOQUEIO é obrigatório.");
            }
            else
            {
                if (dataDesbloqueio.Date > DateTime.Now.Date)
                {
                    mensagens.Add("Campo DATA DO DESBLOQUEIO deve ser menor ou igual a data atual.");
                }

                if (dataBloqueio != DateTime.MinValue && dataBloqueio.Date > dataDesbloqueio.Date)
                {
                    mensagens.Add("A DATA DO BLOQUEIO deve ser menor ou igual a DATA DO DESBLOQUEIO.");
                }
            }

            if (usuarioDesbloqueioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO RESPONSÁVEL PELO DESBLOQUEIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se ja existe desbloqueio
                    if (!this.PossuiDataDesbloqueioPor(contexto, condutorBloqueioId))
                    {
                        mensagens.Add("Este bloqueio já possui uma DATA DO DESBLOQUEIO.");
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

        private bool EhBloqueioAbertoPor(DataContext ctx, int condutorBloqueioId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [TRANSPORTE].[CONDUTORBLOQUEIO] (NOLOCK)
                                WHERE CONDUTORBLOQUEIOID = @CONDUTORBLOQUEIOID
									AND (DATADESBLOQUEIO IS NULL OR CONVERT(DATE, DATADESBLOQUEIO) >= CONVERT(DATE, GETDATE())) ";

            contextQuery.Parameters.Add("@CONDUTORBLOQUEIOID", SqlDbType.Int, condutorBloqueioId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Desbloqueia(int condutorBloqueioId, DateTime dataDesbloqueio, string usuarioDesbloqueioId, string observacao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE Transporte.CONDUTORBLOQUEIO
                                            SET USUARIODESBLOQUEIOID = @USUARIODESBLOQUEIOID,
	                                            DATADESBLOQUEIO = @DATADESBLOQUEIO,
                                                OBSERVACAO = @OBSERVACAO
                                            WHERE CONDUTORBLOQUEIOID = @CONDUTORBLOQUEIOID ";

                contextQuery.Parameters.Add("@CONDUTORBLOQUEIOID", SqlDbType.Int, condutorBloqueioId);
                contextQuery.Parameters.Add("@USUARIODESBLOQUEIOID", SqlDbType.VarChar, usuarioDesbloqueioId);
                contextQuery.Parameters.Add("@DATADESBLOQUEIO", SqlDbType.DateTime, dataDesbloqueio.Date);
                contextQuery.Parameters.Add("@OBSERVACAO", SqlDbType.VarChar, observacao);

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

        public ValidacaoDados ValidaRemocao(int condutorBloqueioId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (condutorBloqueioId <= 0)
            {
                mensagens.Add("Campo CODIGO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Um tipo com data de bloqueio e desbloqueio preenchido não poderá ser excluído.
                    if (this.PossuiDataDesbloqueioPor(contexto, condutorBloqueioId))
                    {
                        mensagens.Add("Registro não pode ser excluído, pois já possui data de desbloqueio.");
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

        public void Remove(int condutorBloqueioId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE Transporte.CONDUTORBLOQUEIO
                                            WHERE CONDUTORBLOQUEIOID = @CONDUTORBLOQUEIOID ";

                contextQuery.Parameters.Add("@CONDUTORBLOQUEIOID", SqlDbType.Int, condutorBloqueioId);

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

        public bool PossuiBloqueioPor(DataContext contexto, int motivoBloqueioId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Transporte.CONDUTORBLOQUEIO (NOLOCK)
                                    WHERE MOTIVOBLOQUEIOID = @MOTIVOBLOQUEIOID ";

            contextQuery.Parameters.Add("@MOTIVOBLOQUEIOID", SqlDbType.Int, motivoBloqueioId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiDataDesbloqueioPor(DataContext contexto, int condutorBloqueioId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Transporte.CONDUTORBLOQUEIO (NOLOCK)
                                    WHERE CONDUTORBLOQUEIOID = @CONDUTORBLOQUEIOID
										AND DATADESBLOQUEIO IS NOT NULL ";

            contextQuery.Parameters.Add("@CONDUTORBLOQUEIOID", SqlDbType.Int, condutorBloqueioId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }
    }
}
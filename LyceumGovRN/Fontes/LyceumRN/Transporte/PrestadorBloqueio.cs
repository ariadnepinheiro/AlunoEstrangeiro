using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Transporte
{
    public class PrestadorBloqueio
    {
        public DataTable Lista()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT PRESTADORBLOQUEIOID, 
                                           P.PRESTADORID, 
                                           P.NOME, 
                                           ISNULL(P.CNPJ, P.CPF) AS CNPJCPF, 
                                           PB.DATABLOQUEIO,
                                           M.DESCRICAO           AS MOTIVOBLOQUEIO, 
                                           M.MOTIVOBLOQUEIOID, 
                                           PB.OBSERVACAO, 
                                           UB.NOME               AS USUARIOBLOQUEIO, 
	                                       PB.DATADESBLOQUEIO, 
                                           UD.NOME               AS USUARIODESBLOQUEIO, 
                                           M.TIPO 
                                    FROM   [TRANSPORTE].[PRESTADORBLOQUEIO] PB (NOLOCK) 
                                           INNER JOIN [TRANSPORTE].[PRESTADOR] P (NOLOCK) 
                                                   ON PB.PRESTADORID = P.PRESTADORID 
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

        public bool PossuiBloqueioPor(DataContext contexto, int motivoBloqueioId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Transporte.PRESTADORBLOQUEIO (NOLOCK)
                                    WHERE MOTIVOBLOQUEIOID = @MOTIVOBLOQUEIOID ";

            contextQuery.Parameters.Add("@MOTIVOBLOQUEIOID", SqlDbType.Int, motivoBloqueioId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiDataDesbloqueioPor(DataContext contexto, int prestadorBloqueioId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Transporte.PRESTADORBLOQUEIO (NOLOCK)
                                    WHERE PRESTADORBLOQUEIOID = @PRESTADORBLOQUEIOID
										AND DATADESBLOQUEIO IS NOT NULL ";

            contextQuery.Parameters.Add("@PRESTADORBLOQUEIOID", SqlDbType.Int, prestadorBloqueioId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public ValidacaoDados ValidaBloqueio(Entidades.PrestadorBloqueio prestadorBloqueio, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (prestadorBloqueio == null)
            {
                return validacaoDados;
            }

            if (!cadastro)
            {
                if (prestadorBloqueio.PrestadorBloqueioId <= 0)
                {
                    mensagens.Add("Campo CODIGO é obrigatório.");
                }
            }

            if (prestadorBloqueio.PrestadorId <= 0)
            {
                mensagens.Add("Campo PRESTADOR é obrigatório.");
            }

            if (prestadorBloqueio.MotivoBloqueioId <= 0)
            {
                mensagens.Add("Campo MOTIVO DO BLOQUEIO é obrigatório.");
            }

            if (!prestadorBloqueio.Observacao.IsNullOrEmptyOrWhiteSpace() && prestadorBloqueio.Observacao.Length > 500)
            {
                mensagens.Add("Campo OBSERVAÇÃO deve conter no máximo 500 caracteres.");
            }

            if (prestadorBloqueio.DataBloqueio == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA DO BLOQUEIO é obrigatório.");
            }
            else if (prestadorBloqueio.DataBloqueio.Date > DateTime.Now.Date)
            {
                mensagens.Add("Campo DATA DO BLOQUEIO deve ser menor ou igual a data atual.");
            }

            if (prestadorBloqueio.UsuarioBloqueioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO RESPONSÁVEL PELO BLOQUEIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    prestadorBloqueio.DataDesbloqueio = null;
                    prestadorBloqueio.UsuarioDesbloqueioId = null;

                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se já existe um bloqueio sem desbloqueio
                    if (this.PossuiOutroBloqueioAbertoPor(contexto, prestadorBloqueio.PrestadorBloqueioId, prestadorBloqueio.PrestadorId))
                    {
                        mensagens.Add("Este prestador já possui um bloqueio em aberto.");
                    }
                    else
                    {
                        //Verifica se já existe um bloqueio no mesmo intervalo de tempo
                        if (this.PossuiOutroBloqueioPor(contexto, prestadorBloqueio.PrestadorBloqueioId, prestadorBloqueio.PrestadorId, prestadorBloqueio.DataBloqueio)
                            || this.PossuiOutroPosteriorPor(contexto, prestadorBloqueio.PrestadorBloqueioId, prestadorBloqueio.PrestadorId, prestadorBloqueio.DataBloqueio))
                        {
                            mensagens.Add("Este prestador já está bloqueado neste periodo.");
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

        private bool PossuiOutroBloqueioAbertoPor(DataContext ctx, int prestadorBloqueioId, int prestadorId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [TRANSPORTE].[PRESTADORBLOQUEIO] (NOLOCK)
                                WHERE PRESTADORBLOQUEIOID <> @PRESTADORBLOQUEIOID
									AND PRESTADORID = @PRESTADORID
									AND (DATADESBLOQUEIO IS NULL OR CONVERT(DATE, DATADESBLOQUEIO) >= CONVERT(DATE, GETDATE())) ";

            contextQuery.Parameters.Add("@PRESTADORBLOQUEIOID", SqlDbType.Int, prestadorBloqueioId);
            contextQuery.Parameters.Add("@PRESTADORID", SqlDbType.Int, prestadorId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public List<DateTime> RetornaDiasBloqueiosPor(DataContext contexto, int prestadorId, DateTime dataInicio, DateTime dataFim)
        {
            List<DateTime> diasBloqueados = new List<DateTime>();

            for (DateTime i = dataInicio; i.Date <= dataFim.Date; i = i.AddDays(1))
            {
                DateTime data = i;

                //Verificar se possui bloqueio
                if (this.PossuiBloqueioAbertoPor(contexto, prestadorId, data))
                {
                    diasBloqueados.Add(data);
                }
            }

            return diasBloqueados;
        }

        public bool PossuiBloqueioAbertoPor(DataContext ctx, int prestadorId, DateTime data)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [TRANSPORTE].[PRESTADORBLOQUEIO] (NOLOCK)
                                WHERE PRESTADORID = @PRESTADORID
									AND CONVERT(DATE, DATABLOQUEIO) <= CONVERT(DATE, @DATA)
									AND (DATADESBLOQUEIO IS NULL OR CONVERT(DATE, DATADESBLOQUEIO) >= CONVERT(DATE, @DATA)) ";

            contextQuery.Parameters.Add("@PRESTADORID", SqlDbType.Int, prestadorId);
            contextQuery.Parameters.Add("@DATA", SqlDbType.DateTime, data.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiOutroPosteriorPor(DataContext ctx, int prestadorBloqueioId, int prestadorId, DateTime dataBloqueio)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [TRANSPORTE].[PRESTADORBLOQUEIO] (NOLOCK)
                                WHERE PRESTADORBLOQUEIOID <> @PRESTADORBLOQUEIOID
									AND PRESTADORID = @PRESTADORID
									AND (DATABLOQUEIO >  @DATABLOQUEIO OR DATADESBLOQUEIO > @DATABLOQUEIO) ";

            contextQuery.Parameters.Add("@PRESTADORBLOQUEIOID", SqlDbType.Int, prestadorBloqueioId);
            contextQuery.Parameters.Add("@PRESTADORID", SqlDbType.Int, prestadorId);
            contextQuery.Parameters.Add("@DATABLOQUEIO", SqlDbType.DateTime, dataBloqueio.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiOutroBloqueioPor(DataContext ctx, int prestadorBloqueioId, int prestadorId, DateTime dataBloqueio)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [TRANSPORTE].[PRESTADORBLOQUEIO] (NOLOCK)
                                WHERE PRESTADORBLOQUEIOID <> @PRESTADORBLOQUEIOID
									AND PRESTADORID = @PRESTADORID
									AND DATADESBLOQUEIO IS NOT NULL									
									AND @DATABLOQUEIO BETWEEN DATABLOQUEIO
                                                           AND DATADESBLOQUEIO ";

            contextQuery.Parameters.Add("@PRESTADORBLOQUEIOID", SqlDbType.Int, prestadorBloqueioId);
            contextQuery.Parameters.Add("@PRESTADORID", SqlDbType.Int, prestadorId);
            contextQuery.Parameters.Add("@DATABLOQUEIO", SqlDbType.DateTime, dataBloqueio.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Bloqueia(Entidades.PrestadorBloqueio prestadorBloqueio)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO Transporte.PRESTADORBLOQUEIO
                                                   (PRESTADORID
                                                   ,MOTIVOBLOQUEIOID
                                                   ,OBSERVACAO
                                                   ,USUARIOBLOQUEIOID
                                                   ,DATABLOQUEIO)
                                             VALUES
                                                   (@PRESTADORID,
                                                   @MOTIVOBLOQUEIOID,
                                                   @OBSERVACAO, 
                                                   @USUARIOBLOQUEIOID, 
                                                   @DATABLOQUEIO) ";

                contextQuery.Parameters.Add("@PRESTADORID", SqlDbType.Int, prestadorBloqueio.PrestadorId);
                contextQuery.Parameters.Add("@MOTIVOBLOQUEIOID", SqlDbType.Int, prestadorBloqueio.MotivoBloqueioId);
                contextQuery.Parameters.Add("@OBSERVACAO", SqlDbType.VarChar, prestadorBloqueio.Observacao);
                contextQuery.Parameters.Add("@USUARIOBLOQUEIOID", SqlDbType.VarChar, prestadorBloqueio.UsuarioBloqueioId);
                contextQuery.Parameters.Add("@DATABLOQUEIO", SqlDbType.DateTime, prestadorBloqueio.DataBloqueio.Date);

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

        public ValidacaoDados ValidaDesbloqueio(int prestadorBloqueioId, DateTime dataDesbloqueio, string usuarioDesbloqueioId, DateTime dataBloqueio)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (prestadorBloqueioId <= 0)
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

                    //Verifica se ja´existe desbloqueio
                    if (!this.PossuiDataDesbloqueioPor(contexto, prestadorBloqueioId))
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

        private bool EhBloqueioAbertoPor(DataContext ctx, int prestadorBloqueioId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [TRANSPORTE].[PRESTADORBLOQUEIO] (NOLOCK)
                                WHERE PRESTADORBLOQUEIOID = @PRESTADORBLOQUEIOID
									AND (DATADESBLOQUEIO IS NULL OR CONVERT(DATE, DATADESBLOQUEIO) >= CONVERT(DATE, GETDATE())) ";

            contextQuery.Parameters.Add("@PRESTADORBLOQUEIOID", SqlDbType.Int, prestadorBloqueioId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Desbloqueia(int prestadorBloqueioId, DateTime dataDesbloqueio, string usuarioDesbloqueioId, string observacao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE Transporte.PRESTADORBLOQUEIO
                                            SET USUARIODESBLOQUEIOID = @USUARIODESBLOQUEIOID,
	                                            DATADESBLOQUEIO = @DATADESBLOQUEIO,
                                                OBSERVACAO = @OBSERVACAO
                                            WHERE PRESTADORBLOQUEIOID = @PRESTADORBLOQUEIOID ";

                contextQuery.Parameters.Add("@PRESTADORBLOQUEIOID", SqlDbType.Int, prestadorBloqueioId);
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

        public ValidacaoDados ValidaRemocao(int prestadorBloqueioId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (prestadorBloqueioId <= 0)
            {
                mensagens.Add("Campo CODIGO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Um tipo com data de bloqueio e desbloqueio preenchido não poderá ser excluído.
                    if (this.PossuiDataDesbloqueioPor(contexto, prestadorBloqueioId))
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

        public void Remove(int prestadorBloqueioId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE Transporte.PRESTADORBLOQUEIO
                                            WHERE PRESTADORBLOQUEIOID = @PRESTADORBLOQUEIOID ";

                contextQuery.Parameters.Add("@PRESTADORBLOQUEIOID", SqlDbType.Int, prestadorBloqueioId);

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

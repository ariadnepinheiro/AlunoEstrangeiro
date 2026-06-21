using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Transporte
{
    public class VeiculoBloqueio
    {
        public DataTable Lista()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT VEICULOBLOQUEIOID, 
                                           P.VEICULOID, 
                                           P.PLACA, 
                                           P.NOME, 
                                           PB.DATABLOQUEIO,
                                           M.DESCRICAO           AS MOTIVOBLOQUEIO, 
                                           M.MOTIVOBLOQUEIOID, 
                                           PB.OBSERVACAO, 
                                           UB.NOME               AS USUARIOBLOQUEIO, 
	                                       PB.DATADESBLOQUEIO,
                                           UD.NOME               AS USUARIODESBLOQUEIO, 
                                           M.TIPO 
                                    FROM   [TRANSPORTE].[VEICULOBLOQUEIO] PB (NOLOCK) 
                                           INNER JOIN [TRANSPORTE].[VEICULO] P (NOLOCK) 
                                                   ON PB.VEICULOID = P.VEICULOID 
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
                                    FROM Transporte.VEICULOBLOQUEIO (NOLOCK)
                                    WHERE MOTIVOBLOQUEIOID = @MOTIVOBLOQUEIOID ";

            contextQuery.Parameters.Add("@MOTIVOBLOQUEIOID", SqlDbType.Int, motivoBloqueioId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiDataDesbloqueioPor(DataContext contexto, int veiculoBloqueioId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Transporte.VEICULOBLOQUEIO (NOLOCK)
                                    WHERE VEICULOBLOQUEIOID = @VEICULOBLOQUEIOID
										AND DATADESBLOQUEIO IS NOT NULL ";

            contextQuery.Parameters.Add("@VEICULOBLOQUEIOID", SqlDbType.Int, veiculoBloqueioId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public ValidacaoDados ValidaBloqueio(Entidades.VeiculoBloqueio veiculoBloqueio, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (veiculoBloqueio == null)
            {
                return validacaoDados;
            }

            if (!cadastro)
            {
                if (veiculoBloqueio.VeiculoBloqueioId <= 0)
                {
                    mensagens.Add("Campo CODIGO é obrigatório.");
                }
            }

            if (veiculoBloqueio.VeiculoId <= 0)
            {
                mensagens.Add("Campo VEICULO é obrigatório.");
            }

            if (veiculoBloqueio.MotivoBloqueioId <= 0)
            {
                mensagens.Add("Campo MOTIVO DO BLOQUEIO é obrigatório.");
            }

            if (!veiculoBloqueio.Observacao.IsNullOrEmptyOrWhiteSpace() && veiculoBloqueio.Observacao.Length > 500)
            {
                mensagens.Add("Campo OBSERVAÇÃO deve conter no máximo 500 caracteres.");
            }

            if (veiculoBloqueio.DataBloqueio == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA DO BLOQUEIO é obrigatório.");
            }
            else if (veiculoBloqueio.DataBloqueio.Date > DateTime.Now.Date)
            {
                mensagens.Add("Campo DATA DO BLOQUEIO deve ser maior que a data atual.");
            }

            if (veiculoBloqueio.UsuarioBloqueioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO RESPONSÁVEL PELO BLOQUEIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    veiculoBloqueio.DataDesbloqueio = null;
                    veiculoBloqueio.UsuarioDesbloqueioId = null;

                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se já existe um bloqueio sem desbloqueio
                    if (this.PossuiOutroBloqueioAbertoPor(contexto, veiculoBloqueio.VeiculoBloqueioId, veiculoBloqueio.VeiculoId))
                    {
                        mensagens.Add("Este veiculo já possui um bloqueio em aberto.");
                    }
                    else
                    {
                        //Verifica se já existe um bloqueio no mesmo intervalo de tempo
                        if (this.PossuiOutroBloqueioPor(contexto, veiculoBloqueio.VeiculoBloqueioId, veiculoBloqueio.VeiculoId, veiculoBloqueio.DataBloqueio)
                             || this.PossuiOutroPosteriorPor(contexto, veiculoBloqueio.VeiculoBloqueioId, veiculoBloqueio.VeiculoId, veiculoBloqueio.DataBloqueio))
                        {
                            mensagens.Add("Este veiculo já está bloqueado neste periodo.");
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

        public List<DateTime> RetornaDiasBloqueiosPor(DataContext contexto, int veiculoId, DateTime dataInicio, DateTime dataFim)
        {
            List<DateTime> diasBloqueados = new List<DateTime>();

            for (DateTime i = dataInicio; i.Date <= dataFim.Date; i = i.AddDays(1))
            {
                DateTime data = i;

                //Verificar se possui bloqueio
                if (this.PossuiBloqueioAbertoPor(contexto, veiculoId, data))
                {
                    diasBloqueados.Add(data);
                }
            }

            return diasBloqueados;
        }       

        public bool PossuiBloqueioAbertoPor(DataContext ctx, int veiculoId, DateTime data)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [TRANSPORTE].[VEICULOBLOQUEIO] (NOLOCK)
                                WHERE VEICULOID = @VEICULOID
                                    AND CONVERT(DATE, DATABLOQUEIO) <= CONVERT(DATE, @DATA)
									AND (DATADESBLOQUEIO IS NULL OR CONVERT(DATE, DATADESBLOQUEIO) >= CONVERT(DATE, @DATA)) ";

            contextQuery.Parameters.Add("@VEICULOID", SqlDbType.Int, veiculoId);
            contextQuery.Parameters.Add("@DATA", SqlDbType.DateTime, data.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiOutroBloqueioAbertoPor(DataContext ctx, int veiculoBloqueioId, int veiculoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [TRANSPORTE].[VEICULOBLOQUEIO] (NOLOCK)
                                WHERE VEICULOBLOQUEIOID <> @VEICULOBLOQUEIOID
									AND VEICULOID = @VEICULOID
									AND (DATADESBLOQUEIO IS NULL OR CONVERT(DATE, DATADESBLOQUEIO) >= CONVERT(DATE, GETDATE())) ";

            contextQuery.Parameters.Add("@VEICULOBLOQUEIOID", SqlDbType.Int, veiculoBloqueioId);
            contextQuery.Parameters.Add("@VEICULOID", SqlDbType.Int, veiculoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiOutroPosteriorPor(DataContext ctx, int veiculoBloqueioId, int veiculoId, DateTime dataBloqueio)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [TRANSPORTE].[VEICULOBLOQUEIO] (NOLOCK)
                                WHERE VEICULOBLOQUEIOID <> @VEICULOBLOQUEIOID
									AND VEICULOID = @VEICULOID
									AND (DATABLOQUEIO >  @DATABLOQUEIO OR DATADESBLOQUEIO > @DATABLOQUEIO) ";

            contextQuery.Parameters.Add("@VEICULOBLOQUEIOID", SqlDbType.Int, veiculoBloqueioId);
            contextQuery.Parameters.Add("@VEICULOID", SqlDbType.Int, veiculoId);
            contextQuery.Parameters.Add("@DATABLOQUEIO", SqlDbType.DateTime, dataBloqueio.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }
        
        private bool PossuiOutroBloqueioPor(DataContext ctx, int veiculoBloqueioId, int veiculoId, DateTime dataBloqueio)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [TRANSPORTE].[VEICULOBLOQUEIO] (NOLOCK)
                                WHERE VEICULOBLOQUEIOID <> @VEICULOBLOQUEIOID
									AND VEICULOID = @VEICULOID
									AND DATADESBLOQUEIO IS NOT NULL									
									AND @DATABLOQUEIO BETWEEN DATABLOQUEIO
                                                           AND DATADESBLOQUEIO ";

            contextQuery.Parameters.Add("@VEICULOBLOQUEIOID", SqlDbType.Int, veiculoBloqueioId);
            contextQuery.Parameters.Add("@VEICULOID", SqlDbType.Int, veiculoId);
            contextQuery.Parameters.Add("@DATABLOQUEIO", SqlDbType.DateTime, dataBloqueio.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Bloqueia(Entidades.VeiculoBloqueio veiculoBloqueio)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO Transporte.VEICULOBLOQUEIO
                                                   (VEICULOID
                                                   ,MOTIVOBLOQUEIOID
                                                   ,OBSERVACAO
                                                   ,USUARIOBLOQUEIOID
                                                   ,DATABLOQUEIO)
                                             VALUES
                                                   (@VEICULOID,
                                                   @MOTIVOBLOQUEIOID,
                                                   @OBSERVACAO, 
                                                   @USUARIOBLOQUEIOID, 
                                                   @DATABLOQUEIO) ";

                contextQuery.Parameters.Add("@VEICULOID", SqlDbType.Int, veiculoBloqueio.VeiculoId);
                contextQuery.Parameters.Add("@MOTIVOBLOQUEIOID", SqlDbType.Int, veiculoBloqueio.MotivoBloqueioId);
                contextQuery.Parameters.Add("@OBSERVACAO", SqlDbType.VarChar, veiculoBloqueio.Observacao);
                contextQuery.Parameters.Add("@USUARIOBLOQUEIOID", SqlDbType.VarChar, veiculoBloqueio.UsuarioBloqueioId);
                contextQuery.Parameters.Add("@DATABLOQUEIO", SqlDbType.DateTime, veiculoBloqueio.DataBloqueio.Date);

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

        public ValidacaoDados ValidaDesbloqueio(int veiculoBloqueioId, DateTime dataDesbloqueio, string usuarioDesbloqueioId, DateTime dataBloqueio)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (veiculoBloqueioId <= 0)
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
                    if (!this.PossuiDataDesbloqueioPor(contexto, veiculoBloqueioId))
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

        private bool EhBloqueioAbertoPor(DataContext ctx, int veiculoBloqueioId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [TRANSPORTE].[VEICULOBLOQUEIO] (NOLOCK)
                                WHERE VEICULOBLOQUEIOID = @VEICULOBLOQUEIOID
									AND (DATADESBLOQUEIO IS NULL OR CONVERT(DATE, DATADESBLOQUEIO) >= CONVERT(DATE, GETDATE())) ";

            contextQuery.Parameters.Add("@VEICULOBLOQUEIOID", SqlDbType.Int, veiculoBloqueioId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Desbloqueia(int veiculoBloqueioId, DateTime dataDesbloqueio, string usuarioDesbloqueioId,string observacao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE Transporte.VEICULOBLOQUEIO
                                            SET USUARIODESBLOQUEIOID = @USUARIODESBLOQUEIOID,
	                                            DATADESBLOQUEIO = @DATADESBLOQUEIO,
                                                OBSERVACAO = @OBSERVACAO
                                            WHERE VEICULOBLOQUEIOID = @VEICULOBLOQUEIOID ";

                contextQuery.Parameters.Add("@VEICULOBLOQUEIOID", SqlDbType.Int, veiculoBloqueioId);
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

        public ValidacaoDados ValidaRemocao(int veiculoBloqueioId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (veiculoBloqueioId <= 0)
            {
                mensagens.Add("Campo CODIGO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Um tipo com data de bloqueio e desbloqueio preenchido não poderá ser excluído.
                    if (this.PossuiDataDesbloqueioPor(contexto, veiculoBloqueioId))
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

        public void Remove(int veiculoBloqueioId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE Transporte.VEICULOBLOQUEIO
                                            WHERE VEICULOBLOQUEIOID = @VEICULOBLOQUEIOID ";

                contextQuery.Parameters.Add("@VEICULOBLOQUEIOID", SqlDbType.Int, veiculoBloqueioId);

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
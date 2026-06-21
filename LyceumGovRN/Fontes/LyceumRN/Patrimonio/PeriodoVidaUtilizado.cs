using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.Patrimonio
{
    public class PeriodoVidaUtilizado
    {
        public DataTable Lista()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT PERIODOVIDAUTILIZADOID, 
                           CONCEITO, 
                           QUANTIDADEANOS,
                           PONTUACAO, 
                           ATIVO, 
                           USUARIOID, 
                           DATACADASTRO, 
                           DATAALTERACAO 
                    FROM   [PATRIMONIO].[PERIODOVIDAUTILIZADO] (NOLOCK) ";

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

        public DataTable ListaPeriodoVidaUtilizadoAtivo()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT PERIODOVIDAUTILIZADOID as CODIGO, 
                           CONCEITO, 
                           QUANTIDADEANOS,
                           PONTUACAO, 
                           ATIVO, 
                           USUARIOID, 
                           DATACADASTRO, 
                           DATAALTERACAO 
                    FROM   [PATRIMONIO].[PERIODOVIDAUTILIZADO] (NOLOCK)
                    WHERE  ATIVO = 1  ";

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

        public ValidacaoDados Valida(Entidades.PeriodoVidaUtilizado periodoVidaUtilizado, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (periodoVidaUtilizado == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (periodoVidaUtilizado.PeriodoVidaUtilizadoId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (periodoVidaUtilizado.Conceito.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CONCEITO é obrigatório.");
            }

            if (periodoVidaUtilizado.QuantidadeAnos <= 0)
            {
                mensagens.Add("Campo QUANTIDADE DE ANOS é obrigatório.");
            }

            if (periodoVidaUtilizado.Pontuacao <= 0)
            {
                mensagens.Add("Campo PONTUAÇÃO é obrigatório.");
            }

            if (periodoVidaUtilizado.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    // Verifica se já existe o periodocadstrado
                    if (this.PossuiOutroConceitoCadastradoPor(contexto, periodoVidaUtilizado.Conceito, periodoVidaUtilizado.PeriodoVidaUtilizadoId))
                    {
                        mensagens.Add("Este CONCEITO já foi utilizado.");
                    }

                    // Verifica se já existe a quantidade de anos
                    if (this.PossuiOutraQuantidadeAnosCadastradaPor(contexto, periodoVidaUtilizado.QuantidadeAnos, periodoVidaUtilizado.PeriodoVidaUtilizadoId))
                    {
                        mensagens.Add("Esta QUANTIDADE DE ANOS já foi utilizada.");
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

        private bool PossuiOutroConceitoCadastradoPor(DataContext ctx, string conceito, int periodoVidaUtilizadoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [PATRIMONIO].[PERIODOVIDAUTILIZADO] (NOLOCK)
                                WHERE CONCEITO = @CONCEITO
	                                AND PERIODOVIDAUTILIZADOID <> @PERIODOVIDAUTILIZADOID ";

            contextQuery.Parameters.Add("@CONCEITO", SqlDbType.VarChar, conceito);
            contextQuery.Parameters.Add("@PERIODOVIDAUTILIZADOID", SqlDbType.Int, periodoVidaUtilizadoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiOutraQuantidadeAnosCadastradaPor(DataContext ctx, int quantidadeAnos, int periodoVidaUtilizadoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [PATRIMONIO].[PERIODOVIDAUTILIZADO] (NOLOCK)
                                WHERE QUANTIDADEANOS = @QUANTIDADEANOS
	                                AND PERIODOVIDAUTILIZADOID <> @PERIODOVIDAUTILIZADOID ";

            contextQuery.Parameters.Add("@QUANTIDADEANOS", SqlDbType.VarChar, quantidadeAnos);
            contextQuery.Parameters.Add("@PERIODOVIDAUTILIZADOID", SqlDbType.Int, periodoVidaUtilizadoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public int ObtemPontuacaoPor(DataContext contexto, int quantidadeAnos)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;
            try
            {
                contextQuery.Command = @"  SELECT PONTUACAO
                            FROM PATRIMONIO.PERIODOVIDAUTILIZADO (NOLOCK)
                            WHERE QUANTIDADEANOS = @QUANTIDADEANOS ";

                contextQuery.Parameters.Add("@QUANTIDADEANOS", SqlDbType.Int, quantidadeAnos);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["PONTUACAO"]);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return retorno;
        }

        public void Insere(Entidades.PeriodoVidaUtilizado periodoVidaUtilizado)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"  INSERT INTO Patrimonio.PERIODOVIDAUTILIZADO
                                                (CONCEITO, 
                                                 QUANTIDADEANOS,
                                                 PONTUACAO, 
                                                 ATIVO, 
                                                 USUARIOID, 
                                                 DATACADASTRO, 
                                                 DATAALTERACAO) 
                                    VALUES      (@CONCEITO, 
                                                 @QUANTIDADEANOS,
                                                 @PONTUACAO, 
                                                 @ATIVO, 
                                                 @USUARIOID, 
                                                 @DATACADASTRO, 
                                                 @DATAALTERACAO)  ";

                contextQuery.Parameters.Add("@CONCEITO", SqlDbType.VarChar, periodoVidaUtilizado.Conceito);
                contextQuery.Parameters.Add("@QUANTIDADEANOS", SqlDbType.Int, periodoVidaUtilizado.QuantidadeAnos);
                contextQuery.Parameters.Add("@PONTUACAO", SqlDbType.Int, periodoVidaUtilizado.Pontuacao);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, periodoVidaUtilizado.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, periodoVidaUtilizado.UsuarioId);
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

        public void Atualiza(Entidades.PeriodoVidaUtilizado periodoVidaUtilizado)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE Patrimonio.PERIODOVIDAUTILIZADO
                                        SET    CONCEITO = @CONCEITO, 
                                               QUANTIDADEANOS = @QUANTIDADEANOS,
                                               PONTUACAO = @PONTUACAO,
                                               ATIVO = @ATIVO, 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  PERIODOVIDAUTILIZADOID = @PERIODOVIDAUTILIZADOID ";

                contextQuery.Parameters.Add("@CONCEITO", SqlDbType.VarChar, periodoVidaUtilizado.Conceito);
                contextQuery.Parameters.Add("@QUANTIDADEANOS", SqlDbType.Int, periodoVidaUtilizado.QuantidadeAnos);
                contextQuery.Parameters.Add("@PONTUACAO", SqlDbType.Int, periodoVidaUtilizado.Pontuacao);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, periodoVidaUtilizado.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, periodoVidaUtilizado.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@PERIODOVIDAUTILIZADOID", SqlDbType.Int, periodoVidaUtilizado.PeriodoVidaUtilizadoId);
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

        public void Remove(int periodoVidaUtilizadoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE Patrimonio.periodoVidaUtilizado
                            WHERE  PERIODOVIDAUTILIZADOID = @PERIODOVIDAUTILIZADOID  ";

                contextQuery.Parameters.Add("@PERIODOVIDAUTILIZADOID", SqlDbType.Int, periodoVidaUtilizadoId);

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

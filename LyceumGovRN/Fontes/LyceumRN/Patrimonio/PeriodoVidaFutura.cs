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
    public class PeriodoVidaFutura
    {
        public DataTable Lista()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT PERIODOVIDAFUTURAID, 
                           CONCEITO, 
                           QUANTIDADEANOS,
                           PONTUACAO, 
                           ATIVO, 
                           USUARIOID, 
                           DATACADASTRO, 
                           DATAALTERACAO 
                    FROM   [PATRIMONIO].[PeriodoVidaFutura] (NOLOCK)  ";

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

        public DataTable ListaPeriodoVidaFuturaAtivo()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT PERIODOVIDAFUTURAID as CODIGO, 
                           CONCEITO, 
                           QUANTIDADEANOS,
                           PONTUACAO, 
                           ATIVO, 
                           USUARIOID, 
                           DATACADASTRO, 
                           DATAALTERACAO 
                    FROM   [PATRIMONIO].[PeriodoVidaFutura] (NOLOCK)
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

        public ValidacaoDados Valida(Entidades.PeriodoVidaFutura periodoVidaFutura, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (periodoVidaFutura == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (periodoVidaFutura.PeriodoVidaFuturaId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (periodoVidaFutura.Conceito.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CONCEITO é obrigatório.");
            }

            if (periodoVidaFutura.Pontuacao <= 0)
            {
                mensagens.Add("Campo PONTUAÇÃO é obrigatório.");
            }
            
            if (periodoVidaFutura.QuantidadeAnos <= 0)
            {
                mensagens.Add("Campo QUANTIDADE DE ANOS é obrigatório.");
            }

            if (periodoVidaFutura.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    // Verifica se já existe o periodocadstrado
                    if (this.PossuiOutroConceitoCadastradoPor(contexto, periodoVidaFutura.Conceito, periodoVidaFutura.PeriodoVidaFuturaId))
                    {
                        mensagens.Add("Este CONCEITO já foi utilizado.");
                    }

                    // Verifica se já existe a quantidade de anos
                    if (this.PossuiOutraQuantidadeAnosCadastradaPor(contexto, periodoVidaFutura.QuantidadeAnos, periodoVidaFutura.PeriodoVidaFuturaId))
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

        private bool PossuiOutroConceitoCadastradoPor(DataContext ctx, string conceito, int periodoVidaFuturaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [PATRIMONIO].[PERIODOVIDAFUTURA] (NOLOCK)
                                WHERE CONCEITO = @CONCEITO
	                                AND PERIODOVIDAFUTURAID <> @PERIODOVIDAFUTURAID ";

            contextQuery.Parameters.Add("@CONCEITO", SqlDbType.VarChar, conceito);
            contextQuery.Parameters.Add("@PERIODOVIDAFUTURAID", SqlDbType.Int, periodoVidaFuturaId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiOutraQuantidadeAnosCadastradaPor(DataContext ctx, int quantidadeAnos, int periodoVidaFuturaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [PATRIMONIO].[PERIODOVIDAFUTURA] (NOLOCK)
                                WHERE QUANTIDADEANOS = @QUANTIDADEANOS
	                                AND PERIODOVIDAFUTURAID <> @PERIODOVIDAFUTURAID ";

            contextQuery.Parameters.Add("@QUANTIDADEANOS", SqlDbType.VarChar, quantidadeAnos);
            contextQuery.Parameters.Add("@PERIODOVIDAFUTURAID", SqlDbType.Int, periodoVidaFuturaId);

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
                            FROM PATRIMONIO.PERIODOVIDAFUTURA (NOLOCK)
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

        public void Insere(Entidades.PeriodoVidaFutura periodoVidaFutura)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"  INSERT INTO Patrimonio.PERIODOVIDAFUTURA
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

                contextQuery.Parameters.Add("@CONCEITO", SqlDbType.VarChar, periodoVidaFutura.Conceito);
                contextQuery.Parameters.Add("@QUANTIDADEANOS", SqlDbType.Int, periodoVidaFutura.QuantidadeAnos);
                contextQuery.Parameters.Add("@PONTUACAO", SqlDbType.Int, periodoVidaFutura.Pontuacao);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, periodoVidaFutura.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, periodoVidaFutura.UsuarioId);
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

        public void Atualiza(Entidades.PeriodoVidaFutura periodoVidaFutura)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE Patrimonio.PERIODOVIDAFUTURA
                                        SET    CONCEITO = @CONCEITO, 
                                               QUANTIDADEANOS = @QUANTIDADEANOS,
                                               PONTUACAO = @PONTUACAO,
                                               ATIVO = @ATIVO, 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  PERIODOVIDAFUTURAID = @PERIODOVIDAFUTURAID ";

                contextQuery.Parameters.Add("@CONCEITO", SqlDbType.VarChar, periodoVidaFutura.Conceito);
                contextQuery.Parameters.Add("@QUANTIDADEANOS", SqlDbType.Int, periodoVidaFutura.QuantidadeAnos);
                contextQuery.Parameters.Add("@PONTUACAO", SqlDbType.Int, periodoVidaFutura.Pontuacao);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, periodoVidaFutura.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, periodoVidaFutura.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@PERIODOVIDAFUTURAID", SqlDbType.Int, periodoVidaFutura.PeriodoVidaFuturaId);

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

        public void Remove(int periodoVidaFuturaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE Patrimonio.PERIODOVIDAFUTURA
                            WHERE  PERIODOVIDAFUTURAID = @PERIODOVIDAFUTURAID  ";

                contextQuery.Parameters.Add("@PERIODOVIDAFUTURAID", SqlDbType.Int, periodoVidaFuturaId);

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

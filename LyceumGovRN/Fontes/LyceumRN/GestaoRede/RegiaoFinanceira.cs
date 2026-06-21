using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Techne.Lyceum.RN.Util;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN.GestaoRede
{
    public class RegiaoFinanceira
    {
        public Entidades.RegiaoFinanceira ObtemRegiaoFinanceiraAtivaPor(string municipioId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            Entidades.RegiaoFinanceira regiaoFinanceira = new Entidades.RegiaoFinanceira();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT TOP 1 RF.* 
                                            FROM  GESTAOREDE.REGIAOFINANCEIRA RF											
											INNER JOIN GESTAOREDE.REGIAOFINANCEIRAMUNICIPIO RFM (NOLOCK) 
													ON RF.REGIAOFINANCEIRAID = RFM.REGIAOFINANCEIRAID
                                            WHERE MUNICIPIOID = @MUNICIPIOID
												  AND CONVERT(DATE, DATAINICIO) <= CONVERT(DATE, GETDATE())
												  AND (DATAFIM IS NULL
														OR CONVERT(DATE, DATAFIM) > CONVERT(DATE, GETDATE()))
											ORDER BY DATAINICIO DESC ";

                contextQuery.Parameters.Add("@MUNICIPIOID", SqlDbType.VarChar, municipioId);

                regiaoFinanceira = contexto.TryToBindEntity<Entidades.RegiaoFinanceira>(contextQuery);

                return regiaoFinanceira;
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

        public DataTable Lista()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT REGIAOFINANCEIRAID, 
                           DESCRICAO, 
                           CODIGOCG,
                           USUARIOID, 
                           DATACADASTRO, 
                           DATAALTERACAO 
                    FROM   GestaoRede.REGIAOFINANCEIRA (NOLOCK) ";

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

        public ValidacaoDados Valida(Entidades.RegiaoFinanceira regiaoFinanceira, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (regiaoFinanceira == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (regiaoFinanceira.RegiaoFinanceiraId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (regiaoFinanceira.Descricao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo DESCRIÇÃO é obrigatório.");
            }

            if (regiaoFinanceira.CodigoCg.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CÓDIGO CG é obrigatório.");
            }

            if (regiaoFinanceira.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    // Verifica se já existe a descricao cadastrada
                    if (this.PossuiOutraDescricaoCadastradaPor(contexto, regiaoFinanceira.Descricao, regiaoFinanceira.RegiaoFinanceiraId))
                    {
                        mensagens.Add("Esta DESCRIÇÃO já foi utilizada.");
                    }

                    if (this.PossuiOutroCodigoCgCadastradoPor(contexto, regiaoFinanceira.CodigoCg, regiaoFinanceira.RegiaoFinanceiraId))
                    {
                        mensagens.Add("EstE CÓDIGO CG já foi utilizadO.");
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

        private bool PossuiOutraDescricaoCadastradaPor(DataContext ctx, string descricao, int regiaoFinanceiraId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM GestaoRede.REGIAOFINANCEIRA (NOLOCK)
                                WHERE DESCRICAO = @DESCRICAO
	                                AND REGIAOFINANCEIRAID <> @REGIAOFINANCEIRAID ";

            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, descricao);
            contextQuery.Parameters.Add("@REGIAOFINANCEIRAID", SqlDbType.Int, regiaoFinanceiraId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiOutroCodigoCgCadastradoPor(DataContext ctx, string codigoCg, int regiaoFinanceiraId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM GestaoRede.REGIAOFINANCEIRA (NOLOCK)
                                WHERE CODIGOCG = @CODIGOCG
	                                AND REGIAOFINANCEIRAID <> @REGIAOFINANCEIRAID ";

            contextQuery.Parameters.Add("@CODIGOCG", SqlDbType.VarChar, codigoCg);
            contextQuery.Parameters.Add("@REGIAOFINANCEIRAID", SqlDbType.Int, regiaoFinanceiraId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.RegiaoFinanceira regiaoFinanceira)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"  INSERT INTO GestaoRede.REGIAOFINANCEIRA
			                                    (DESCRICAO, 
                                                 CODIGOCG,
                                                 USUARIOID, 
                                                 DATACADASTRO, 
                                                 DATAALTERACAO) 
                                    VALUES      (@DESCRICAO, 
                                                 @CODIGOCG,
                                                 @USUARIOID, 
                                                 @DATACADASTRO, 
                                                 @DATAALTERACAO)   ";

                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, regiaoFinanceira.Descricao);
                contextQuery.Parameters.Add("@CODIGOCG", SqlDbType.VarChar, regiaoFinanceira.CodigoCg);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, regiaoFinanceira.UsuarioId);
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

        public void Atualiza(Entidades.RegiaoFinanceira regiaoFinanceira)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE GestaoRede.REGIAOFINANCEIRA
                                        SET    DESCRICAO = @DESCRICAO, 
                                               CODIGOCG = @CODIGOCG, 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  REGIAOFINANCEIRAID = @REGIAOFINANCEIRAID ";

                contextQuery.Parameters.Add("@REGIAOFINANCEIRAID", SqlDbType.Int, regiaoFinanceira.RegiaoFinanceiraId);
                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, regiaoFinanceira.Descricao);
                contextQuery.Parameters.Add("@CODIGOCG", SqlDbType.VarChar, regiaoFinanceira.CodigoCg);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, regiaoFinanceira.UsuarioId);
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

        public ValidacaoDados ValidaRemocao(int regiaoFinanceiraId)
        {
            List<string> mensagens = new List<string>();
            RegiaoFinanceiraMunicipio rnRegiaoFinanceiraMunicipio = new RegiaoFinanceiraMunicipio();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (regiaoFinanceiraId <= 0)
            {
                mensagens.Add("Campo Código é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se ja foi utilizado
                    if (rnRegiaoFinanceiraMunicipio.PossuiRegiaoFinanceiraCadastradaPor(contexto, regiaoFinanceiraId))
                    {
                        mensagens.Add("Esta região financeira não pode ser excluida pois já foi associada a um municipio.");
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
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public void Remove(int regiaoFinanceiraId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE GestaoRede.REGIAOFINANCEIRA
                            WHERE  REGIAOFINANCEIRAID = @REGIAOFINANCEIRAID  ";

                contextQuery.Parameters.Add("@REGIAOFINANCEIRAID", SqlDbType.Int, regiaoFinanceiraId);

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

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
    public class Moeda
    {
        public DataTable Lista()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT MOEDAID, 
                           DESCRICAO, 
                           DATAINICIO,
                           DATAFIM,
                           SIGLA, 
                           FATOR,
                           USUARIOID, 
                           DATACADASTRO, 
                           DATAALTERACAO 
                    FROM   [PATRIMONIO].[Moeda] (NOLOCK) ";

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

        public Entidades.Moeda ObtemMoedaVigentePor(DateTime dataPesquisa)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            Entidades.Moeda moeda = new Entidades.Moeda();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT * 
                                    FROM PATRIMONIO.MOEDA (NOLOCK)
                                    WHERE DATAINICIO  <= @DATA
	                                    AND (DATAFIM IS NULL OR DATAFIM >= @DATA) ";

                contextQuery.Parameters.Add("@DATA", SqlDbType.Date, dataPesquisa.Date);

                moeda = contexto.TryToBindEntity<Entidades.Moeda>(contextQuery);

                return moeda;
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

        public ValidacaoDados Valida(Entidades.Moeda moeda, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (moeda == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (moeda.MoedaId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (moeda.Descricao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo DESCRIÇÃO é obrigatório.");
            }

            if (moeda.Sigla.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo SIGLA é obrigatório.");
            }

            if (moeda.Fator <= 0)
            {
                mensagens.Add("Campo FATOR é obrigatório.");
            }

            if (moeda.DataInicio <= DateTime.MinValue)
            {
                mensagens.Add("Campo DATA DE INÍCIO é obrigatório.");
            }
            else
            {
                if (moeda.DataFim != null && moeda.DataFim > DateTime.MinValue)
                {
                    if (Convert.ToDateTime(moeda.DataFim).Date < moeda.DataInicio.Date)
                    {
                        mensagens.Add("A DATA DE FIM não pode ser menor que a DATA DE INÍCIO.");
                    }
                }
            }

            if (moeda.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    // Verifica se já existe a descricao cadastrada
                    if (this.PossuiOutraDescricaoCadastradaPor(contexto, moeda.Descricao, moeda.MoedaId))
                    {
                        mensagens.Add("Este DESCRIÇÃO já foi utilizado.");
                    }

                    // Verifica se já existe a sigla cadastrada
                    if (this.PossuiOutraSiglaCadastradaPor(contexto, moeda.Sigla, moeda.MoedaId))
                    {
                        mensagens.Add("Este SIGLA já foi utilizado.");
                    }

                    if (moeda.DataFim == null || moeda.DataFim  <= DateTime.MinValue || Convert.ToDateTime(moeda.DataFim).Date >= DateTime.Now.Date)
                    {
                        //Verifica se já existe outra moeda vigente
                        if (this.PossuiOutraMoedaVigentePor(contexto, moeda.MoedaId))
                        {
                            mensagens.Add("Já existe outra moeda vigente.");
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

        private bool PossuiOutraMoedaVigentePor(DataContext ctx, int moedaId)
        {  
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM [Patrimonio].[MOEDA] (NOLOCK)
                                    WHERE  MOEDAID <> @MOEDAID
                                           AND DATAINICIO <= GETDATE() 
                                           AND ( DATAFIM IS NULL 
                                                  OR DATAFIM >= GETDATE() )  ";

            contextQuery.Parameters.Add("@MOEDAID", SqlDbType.Int, moedaId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiOutraDescricaoCadastradaPor(DataContext ctx, string descricao, int moedaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [PATRIMONIO].[MOEDA] (NOLOCK)
                                WHERE DESCRICAO = @DESCRICAO
	                                AND MOEDAID <> @MOEDAID ";

            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, descricao);
            contextQuery.Parameters.Add("@MOEDAID", SqlDbType.Int, moedaId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiOutraSiglaCadastradaPor(DataContext ctx, string sigla, int moedaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [PATRIMONIO].[MOEDA] (NOLOCK)
                                WHERE SIGLA = @SIGLA
	                                AND MOEDAID <> @MOEDAID ";

            contextQuery.Parameters.Add("@SIGLA", SqlDbType.VarChar, sigla);
            contextQuery.Parameters.Add("@MOEDAID", SqlDbType.Int, moedaId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.Moeda moeda)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"  INSERT INTO Patrimonio.MOEDA
			                                    (DESCRICAO, 
                                                 SIGLA, 
                                                 FATOR, 
                                                 DATAINICIO, 
                                                 DATAFIM, 
                                                 USUARIOID, 
                                                 DATACADASTRO, 
                                                 DATAALTERACAO) 
                                    VALUES      (@DESCRICAO, 
                                                 @SIGLA, 
                                                 @FATOR, 
                                                 @DATAINICIO, 
                                                 @DATAFIM, 
                                                 @USUARIOID, 
                                                 @DATACADASTRO, 
                                                 @DATAALTERACAO)   ";

                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, moeda.Descricao);
                contextQuery.Parameters.Add("@SIGLA", SqlDbType.VarChar, moeda.Sigla);
                contextQuery.Parameters.Add("@FATOR", SqlDbType.Int, moeda.Fator);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.Date, moeda.DataInicio.Date);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, moeda.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                if (moeda.DataFim != null && moeda.DataFim > DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@DATAFIM", SqlDbType.Date, Convert.ToDateTime(moeda.DataFim).Date);
                }
                else
                {
                    contextQuery.Parameters.Add("@DATAFIM", SqlDbType.Date, null);
                }

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

        public void Atualiza(Entidades.Moeda moeda)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE Patrimonio.Moeda
                                        SET    DESCRICAO = @DESCRICAO, 
                                               SIGLA = @SIGLA,
                                               FATOR = @FATOR,
                                               DATAINICIO = @DATAINICIO,
                                               DATAFIM = @DATAFIM, 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  MOEDAID = @MOEDAID ";

                contextQuery.Parameters.Add("@MOEDAID", SqlDbType.Int, moeda.MoedaId);
                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, moeda.Descricao);
                contextQuery.Parameters.Add("@SIGLA", SqlDbType.VarChar, moeda.Sigla);
                contextQuery.Parameters.Add("@FATOR", SqlDbType.Int, moeda.Fator);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.Date, moeda.DataInicio.Date);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, moeda.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                if (moeda.DataFim != null && moeda.DataFim > DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@DATAFIM", SqlDbType.Date, Convert.ToDateTime(moeda.DataFim).Date);
                }
                else
                {
                    contextQuery.Parameters.Add("@DATAFIM", SqlDbType.Date, null);
                }

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

        public ValidacaoDados ValidaRemocao(int moedaId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.Patrimonio.Bem rnBem = new Bem();
            RN.Patrimonio.BemValor rnBemValor = new BemValor();
            RN.Patrimonio.Reavaliacao rnReavaliacao = new Reavaliacao();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (moedaId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se a moeda ja foi utilizado em algum valor inicial
                    if (rnBemValor.PossuiMoedaPor(contexto, moedaId))
                    {
                        mensagens.Add("Registro não pode ser excluído pois já foi utilizado.");
                    }

                    //Verifica se a moeda ja foi utilizado em alguma reavaliacao
                    if (rnReavaliacao.PossuiMoedaPor(contexto, moedaId))
                    {
                        mensagens.Add("Registro não pode ser excluído pois já foi utilizado.");
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

        public void Remove(int moedaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE Patrimonio.MOEDA
                            WHERE  MOEDAID = @MOEDAID  ";

                contextQuery.Parameters.Add("@MOEDAID", SqlDbType.Int, moedaId);

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

        public Entidades.Moeda ObtemDadosMoedaPor(int moedaId)
        {
            Entidades.Moeda moeda = new Entidades.Moeda();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT * 
                                    FROM PATRIMONIO.MOEDA (NOLOCK)
                                    WHERE MOEDAID  = @MOEDAID
	                                     ";

                contextQuery.Parameters.Add("@MOEDAID", SqlDbType.Int, moedaId);

                moeda = contexto.TryToBindEntity<Entidades.Moeda>(contextQuery);

                return moeda;
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
                if (reader != null)
                {
                    reader.Close();
                }
                contexto.Dispose();
            }
        }
    }
}

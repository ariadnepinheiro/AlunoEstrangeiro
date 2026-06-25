using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class ProdutoServicoValorMaximo
    {
        public DataTable ListaPor(int produtoServicoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT VM.PRODUTOSERVICOVALORMAXIMOID, 
                                              VM.PRODUTOSERVICOID,
                                              VM.REGIAOFGVID, 
											  R.DESCRICAO AS REGIAOFGV,
                                              VM.VALORMAXIMO, 
                                              VM.DATAINICIO, 
                                              VM.DATAFIM, 
                                              VM.USUARIOID, 
                                              VM.DATACADASTRO, 
                                              VM.DATAALTERACAO
                                        FROM PrestacaoContas.PRODUTOSERVICOVALORMAXIMO VM (NOLOCK)
											 inner join PrestacaoContas.REGIAOFGV R (NOLOCK)
														ON VM.REGIAOFGVID = R.REGIAOFGVID
                                        WHERE PRODUTOSERVICOID = @PRODUTOSERVICOID ";

                contextQuery.Parameters.Add("@PRODUTOSERVICOID", SqlDbType.Int, produtoServicoId);

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

        public ValidacaoDados Valida(Entidades.ProdutoServicoValorMaximo produtoServicoValorMaximo, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ProdutoServico rnProdutoServico = new ProdutoServico();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (produtoServicoValorMaximo == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (produtoServicoValorMaximo.ProdutoServicoValorMaximoId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }
            else if (produtoServicoValorMaximo.RegiaoFgvId <= 0)
            {
                mensagens.Add("Campo ÁREAS GEOGRÁFICAS é obrigatório.");
            }

            if (produtoServicoValorMaximo.ProdutoServicoId <= 0)
            {
                mensagens.Add("Campo PRODUTO OU SERVIÇO é obrigatório.");
            }

            if (produtoServicoValorMaximo.ValorMaximo <= 0)
            {
                mensagens.Add("Campo VALOR MÁXIMO é obrigatório e não pode ser inferior a R$0,01.");
            }

            if (produtoServicoValorMaximo.DataInicio <= DateTime.MinValue)
            {
                mensagens.Add("Campo DATA INÍCIO é obrigatório.");
            }
            else
            {
                if (produtoServicoValorMaximo.DataFim != null && produtoServicoValorMaximo.DataFim != DateTime.MinValue)
                {
                    if (produtoServicoValorMaximo.DataInicio > produtoServicoValorMaximo.DataFim)
                    {
                        mensagens.Add("A DATA INÍCIO não pode ser superior a DATA FIM.");
                    }
                }
            }
            

            if (produtoServicoValorMaximo.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUÁRIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (this.PossuiOutroValorMaximoPor(contexto, produtoServicoValorMaximo.ProdutoServicoId, produtoServicoValorMaximo.RegiaoFgvId, produtoServicoValorMaximo.ProdutoServicoValorMaximoId))
                    {
                        mensagens.Add("Já consta um valor máximo cadastrado para esta região.");
                    }

                    //Verifica se a data de inicio está intercalada com outro 
                    if (this.PossuiDataInicioEmOutroIntervaloPor(contexto, produtoServicoValorMaximo.ProdutoServicoId, produtoServicoValorMaximo.RegiaoFgvId, produtoServicoValorMaximo.DataInicio, produtoServicoValorMaximo.ProdutoServicoValorMaximoId))
                    {
                        mensagens.Add("DATA INÍCIO não pode estar dentro do intervalo de outro valor máximo.");
                    }

                    //Verifica se as datas de inicio e de fim estão intercalada com outro 
                    if (this.PossuiOutraIntercaladaPor(contexto, produtoServicoValorMaximo.ProdutoServicoId, produtoServicoValorMaximo.RegiaoFgvId, produtoServicoValorMaximo.DataInicio, Convert.ToDateTime(produtoServicoValorMaximo.DataFim), produtoServicoValorMaximo.ProdutoServicoValorMaximoId))
                    {
                        mensagens.Add("DATA INÍCIO E FIM não podem intercalar com outro valor máximo.");
                    }

                    //Verifica se não possui data de fim
                    if (produtoServicoValorMaximo.DataFim != null && produtoServicoValorMaximo.DataFim > DateTime.MinValue)
                    {
                        //Verifica se a data de inicio está intercalada com outro 
                        if (this.PossuiDataFimEmOutroIntervaloPor(contexto, produtoServicoValorMaximo.ProdutoServicoId, produtoServicoValorMaximo.RegiaoFgvId, Convert.ToDateTime(produtoServicoValorMaximo.DataFim), produtoServicoValorMaximo.ProdutoServicoValorMaximoId))
                        {
                            mensagens.Add("DATA FIM não pode estar dentro do intervalo de outro valor máximo.");
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

        private bool PossuiDataInicioEmOutroIntervaloPor(DataContext ctx, int produtoServicoId, int regiaoFgvId, DateTime data, int produtoServicoValorMaximoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM  [PrestacaoContas].[PRODUTOSERVICOVALORMAXIMO]  (NOLOCK)
                                WHERE  PRODUTOSERVICOID = @PRODUTOSERVICOID 
                                    AND REGIAOFGVID = @REGIAOFGVID
	                                AND PRODUTOSERVICOVALORMAXIMOID <> @PRODUTOSERVICOVALORMAXIMOID
	                                AND @DATA BETWEEN CONVERT(DATE,DATAINICIO) AND 
			                                CONVERT(DATE, CONVERT(DATETIME, ISNULL(DATAFIM, DATAINICIO)) ) ";

            contextQuery.Parameters.Add("@PRODUTOSERVICOID", SqlDbType.Int, produtoServicoId);
            contextQuery.Parameters.Add("@REGIAOFGVID", SqlDbType.Int, regiaoFgvId);
            contextQuery.Parameters.Add("@PRODUTOSERVICOVALORMAXIMOID", SqlDbType.Int, produtoServicoValorMaximoId);
            contextQuery.Parameters.Add("@DATA", SqlDbType.Date, data.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        private bool PossuiDataFimEmOutroIntervaloPor(DataContext ctx, int produtoServicoId, int regiaoFgvId, DateTime data, int produtoServicoValorMaximoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                        FROM   [PrestacaoContas].[PRODUTOSERVICOVALORMAXIMO] 
                        WHERE  PRODUTOSERVICOID = @PRODUTOSERVICOID 
                                AND REGIAOFGVID = @REGIAOFGVID
                                AND PRODUTOSERVICOVALORMAXIMOID <> @PRODUTOSERVICOVALORMAXIMOID
                                AND @DATA BETWEEN 
                                    CONVERT(DATE, CONVERT(DATETIME, DATAINICIO) + 1) AND CONVERT( 
                                    DATE, CONVERT(DATETIME, ISNULL(DATAFIM, GETDATE())))  ";

            contextQuery.Parameters.Add("@PRODUTOSERVICOID", SqlDbType.Int, produtoServicoId);
            contextQuery.Parameters.Add("@REGIAOFGVID", SqlDbType.Int, regiaoFgvId);
            contextQuery.Parameters.Add("@PRODUTOSERVICOVALORMAXIMOID", SqlDbType.Int, produtoServicoValorMaximoId);
            contextQuery.Parameters.Add("@DATA", SqlDbType.Date, data.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        private bool PossuiOutraIntercaladaPor(DataContext ctx, int produtoServicoId, int regiaoFgvId, DateTime dataInicio, DateTime dataFim, int produtoServicoValorMaximoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                            FROM   [PrestacaoContas].[PRODUTOSERVICOVALORMAXIMO]  (NOLOCK)
                            WHERE PRODUTOSERVICOID = @PRODUTOSERVICOID
                                    AND REGIAOFGVID = @REGIAOFGVID
                                    AND PRODUTOSERVICOVALORMAXIMOID <> @PRODUTOSERVICOVALORMAXIMOID
                                    AND @DATAINICIO <= CONVERT(DATE, DATAINICIO) 
                                    AND @DATAFIM >= CONVERT(DATE, CONVERT(DATETIME, ISNULL(DATAFIM, GETDATE()))) ";

            contextQuery.Parameters.Add("@PRODUTOSERVICOID", SqlDbType.Int, produtoServicoId);
            contextQuery.Parameters.Add("@REGIAOFGVID", SqlDbType.Int, regiaoFgvId);
            contextQuery.Parameters.Add("@PRODUTOSERVICOVALORMAXIMOID", SqlDbType.Int, produtoServicoValorMaximoId);
            contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.Date, dataInicio.Date);
            contextQuery.Parameters.Add("@DATAFIM", SqlDbType.Date, dataFim.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public void Insere(Entidades.ProdutoServicoValorMaximo produtoServicoValorMaximo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            RN.PrestacaoContas.ProdutoServico rnProdutoServico = new ProdutoServico();

            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @"  INSERT INTO PrestacaoContas.PRODUTOSERVICOVALORMAXIMO
                                               (PRODUTOSERVICOID
                                               ,REGIAOFGVID
                                               ,VALORMAXIMO
                                               ,DATAINICIO
                                               ,DATAFIM
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO)
                                         VALUES
                                               (@PRODUTOSERVICOID
                                               ,@REGIAOFGVID
                                               ,@VALORMAXIMO
                                               ,@DATAINICIO
                                               ,@DATAFIM
                                               ,@USUARIOID
                                               ,@DATACADASTRO
                                               ,@DATAALTERACAO) ";

                contextQuery.Parameters.Add("@PRODUTOSERVICOID", SqlDbType.Int, produtoServicoValorMaximo.ProdutoServicoId);
                contextQuery.Parameters.Add("@REGIAOFGVID", SqlDbType.Int, produtoServicoValorMaximo.RegiaoFgvId);
                contextQuery.Parameters.Add("@VALORMAXIMO", SqlDbType.Decimal, produtoServicoValorMaximo.ValorMaximo);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, produtoServicoValorMaximo.DataInicio.Date);

                if (produtoServicoValorMaximo.DataFim == null || produtoServicoValorMaximo.DataFim == DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@DATAFIM", null);
                }
                else
                {
                    contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, Convert.ToDateTime(produtoServicoValorMaximo.DataFim).Date);
                }

                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, produtoServicoValorMaximo.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                contexto.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
                if (contexto != null)
                {
                    contexto.Abandon();
                }
                string mensagem = string.Empty;
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    if (Convert.ToString(ex.Message).Contains("ERRO:"))
                    {
                        mensagem = ex.Message.Replace("ERRO: ", string.Empty);
                    }
                    else
                    {
                        mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine,
                            Convert.ToString(ex.Message));
                    }
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

        public void Atualiza(Entidades.ProdutoServicoValorMaximo produtoServicoValorMaximo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"  UPDATE PRESTACAOCONTAS.PRODUTOSERVICOVALORMAXIMO 
                                            SET    VALORMAXIMO = @VALORMAXIMO, 
                                                   DATAINICIO = @DATAINICIO, 
                                                   DATAFIM = @DATAFIM, 
                                                   USUARIOID = @USUARIOID, 
                                                   DATAALTERACAO = @DATAALTERACAO 
                                            WHERE  PRODUTOSERVICOVALORMAXIMOID = @PRODUTOSERVICOVALORMAXIMOID ";

                contextQuery.Parameters.Add("@PRODUTOSERVICOVALORMAXIMOID", SqlDbType.Int, produtoServicoValorMaximo.ProdutoServicoValorMaximoId);
                contextQuery.Parameters.Add("@VALORMAXIMO", SqlDbType.Decimal, produtoServicoValorMaximo.ValorMaximo);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, produtoServicoValorMaximo.DataInicio.Date);

                if (produtoServicoValorMaximo.DataFim == null || produtoServicoValorMaximo.DataFim == DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@DATAFIM", null);
                }
                else
                {
                    contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, Convert.ToDateTime(produtoServicoValorMaximo.DataFim).Date);
                }

                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, produtoServicoValorMaximo.UsuarioId);
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

        public ValidacaoDados ValidaRemocao(int produtoServicoValorMaximoId, int produtoServicoId, string usuarioResponsavel)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (produtoServicoValorMaximoId <= 0)
            {
                mensagens.Add("Campo CÓDIGO é obrigatório.");
            }

            if (produtoServicoId <= 0)
            {
                mensagens.Add("Campo PRODUTO OU SERVICO é obrigatório.");
            }

            if (usuarioResponsavel.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUÁRIO RESPONSÁVEL é obrigatório.");
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

        public void Remove(int produtoServicoValorMaximoId, int produtoServicoId, string usuarioResponsavel)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @"  DELETE PRESTACAOCONTAS.PRODUTOSERVICOVALORMAXIMO 
                                            WHERE  PRODUTOSERVICOVALORMAXIMOID = @PRODUTOSERVICOVALORMAXIMOID ";

                contextQuery.Parameters.Add("@PRODUTOSERVICOVALORMAXIMOID", SqlDbType.Int, produtoServicoValorMaximoId);

                contexto.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
                if (contexto != null)
                {
                    contexto.Abandon();
                }
                string mensagem = string.Empty;
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    if (Convert.ToString(ex.Message).Contains("ERRO:"))
                    {
                        mensagem = ex.Message.Replace("ERRO: ", string.Empty);
                    }
                    else
                    {
                        mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine,
                            Convert.ToString(ex.Message));
                    }
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

        private bool PossuiOutroValorMaximoPor(DataContext ctx, int produtoServicoId, int regiaoFgvId, int produtoServicoValorMaximoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM  [PrestacaoContas].[PRODUTOSERVICOVALORMAXIMO]  (NOLOCK)
                                WHERE  PRODUTOSERVICOID = @PRODUTOSERVICOID 
                                    AND REGIAOFGVID = @REGIAOFGVID
                                    AND DATAFIM IS NULL
	                                AND PRODUTOSERVICOVALORMAXIMOID <> @PRODUTOSERVICOVALORMAXIMOID
 ";

            contextQuery.Parameters.Add("@PRODUTOSERVICOID", SqlDbType.Int, produtoServicoId);
            contextQuery.Parameters.Add("@REGIAOFGVID", SqlDbType.Int, regiaoFgvId);
            contextQuery.Parameters.Add("@PRODUTOSERVICOVALORMAXIMOID", SqlDbType.Int, produtoServicoValorMaximoId);


            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }		
    }
}

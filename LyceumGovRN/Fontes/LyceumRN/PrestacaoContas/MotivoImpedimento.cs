using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class MotivoImpedimento : RNBase
    {
    
        public System.Data.DataTable ListaAtivoPor()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"  SELECT  MOTIVOIMPEDIMENTOID, 
		                                            DESCRICAO
                                            FROM PrestacaoContas.MOTIVOIMPEDIMENTO (NOLOCK)
                                                -- WHERE ATIVO = 1
                                            ORDER BY MOTIVOIMPEDIMENTOID ";

                dt = contexto.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                contexto.Abandon();
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
                contexto.Dispose();
            }

            return dt;
        }

        public DataTable Lista()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT  MOTIVOIMPEDIMENTOID, 
		                                        DESCRICAO,
                                                DATAINICIO,
                                                DATAFIM,      
		                                        USUARIOID, 
		                                        DATACADASTRO, 
		                                        DATAALTERACAO
                                        FROM PrestacaoContas.MOTIVOIMPEDIMENTO (NOLOCK)
                                        ORDER BY DESCRICAO ";

                dt = contexto.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                contexto.Abandon();
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
                contexto.Dispose();
            }

            return dt;
        }

        public ValidacaoDados Valida(Entidades.MotivoImpedimento motivoImpedimento, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (motivoImpedimento == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (motivoImpedimento.MotivoImpedimentoId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (motivoImpedimento.Descricao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo DESCRIÇÃO é obrigatório.");
            }

            if (motivoImpedimento.DataInicio == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA INÍCIO é obrigatório.");
            }
            else if (motivoImpedimento.DataFim != null && motivoImpedimento.DataFim != DateTime.MinValue)
            {
                if (motivoImpedimento.DataInicio > motivoImpedimento.DataFim)
                {
                    mensagens.Add("A DATA INÍCIO não pode ser superior a DATA FIM.");
                }
            }

            if (motivoImpedimento.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se já existe a descrição cadastrada
                    if (this.PossuiOutraDescricaoCadastradaPor(contexto, motivoImpedimento.Descricao, motivoImpedimento.MotivoImpedimentoId))
                    {
                        mensagens.Add("Esta DESCRIÇÃO já foi utilizada.");
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

        private bool PossuiOutraDescricaoCadastradaPor(DataContext ctx, string descricao, int motivoImpedimentoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM PrestacaoContas.MOTIVOIMPEDIMENTO (NOLOCK)
                                WHERE DESCRICAO = @DESCRICAO
	                                AND MOTIVOIMPEDIMENTOID <> @MOTIVOIMPEDIMENTOID ";

            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, descricao);
            contextQuery.Parameters.Add("@MOTIVOIMPEDIMENTOID", SqlDbType.Int, motivoImpedimentoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.MotivoImpedimento motivoImpedimento)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();            
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO PrestacaoContas.MOTIVOIMPEDIMENTO
                                                        (DESCRICAO,                                                         
                                                         DATAINICIO,
                                                         DATAFIM,	
                                                         USUARIOID, 
                                                         DATACADASTRO, 
                                                         DATAALTERACAO) 
                                            VALUES      (@DESCRICAO,                                                         
                                                         @DATAINICIO,
                                                         @DATAFIM,	
                                                         @USUARIOID, 
                                                         @DATACADASTRO, 
                                                         @DATAALTERACAO) ";

                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, motivoImpedimento.Descricao);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, motivoImpedimento.DataInicio);

                if (motivoImpedimento.DataFim == null || motivoImpedimento.DataFim == DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, DBNull.Value);
                }
                else
                {
                    contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, motivoImpedimento.DataFim);
                }

                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, motivoImpedimento.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, motivoImpedimento.UsuarioId);
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

        public void Atualiza(Entidades.MotivoImpedimento motivoImpedimentoUnidade)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE PrestacaoContas.MOTIVOIMPEDIMENTO
                                        SET    DESCRICAO = @DESCRICAO,                                               
                                               DATAINICIO = @DATAINICIO,
                                               DATAFIM = @DATAFIM,	
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  MOTIVOIMPEDIMENTOID = @MOTIVOIMPEDIMENTOID ";

                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, motivoImpedimentoUnidade.Descricao);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, motivoImpedimentoUnidade.DataInicio);

                if (motivoImpedimentoUnidade.DataFim == null || motivoImpedimentoUnidade.DataFim == DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, DBNull.Value);
                }
                else
                {
                    contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, motivoImpedimentoUnidade.DataFim);
                }
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, motivoImpedimentoUnidade.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@MOTIVOIMPEDIMENTOID", SqlDbType.Int, motivoImpedimentoUnidade.MotivoImpedimentoId);

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

        public ValidacaoDados ValidaRemocao(int motivoImpedimentoId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.PrestacaoContas.UnidadeEnsinoImpedida rnUnidadeEnsinoImpedida = new UnidadeEnsinoImpedida();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (motivoImpedimentoId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se motivo ja foi utilizado
                    if (rnUnidadeEnsinoImpedida.PossuiUnidadeEnsinoImpedidaPor(contexto, motivoImpedimentoId))
                    {
                        mensagens.Add("Este motivo não pode ser excluído, pois já foi utilizado para uma unidade impedida.");
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

        public void Remove(int motivoImpedimentoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE PrestacaoContas.MOTIVOIMPEDIMENTO
                            WHERE  MOTIVOIMPEDIMENTOID = @MOTIVOIMPEDIMENTOID  ";

                contextQuery.Parameters.Add("@MOTIVOIMPEDIMENTOID", SqlDbType.Int, motivoImpedimentoId);

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

        public string ObtemDescricaoPor(DataContext contexto, int motivoImpedimentoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT DESCRICAO
                            FROM PrestacaoContas.MOTIVOIMPEDIMENTO
                            WHERE  MOTIVOIMPEDIMENTOID = @MOTIVOIMPEDIMENTOID ";

            contextQuery.Parameters.Add("@MOTIVOIMPEDIMENTOID", SqlDbType.Int, motivoImpedimentoId); 

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }
    }
}

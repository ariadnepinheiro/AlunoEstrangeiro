using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Util;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.FiscalizacaoLink
{
    public class Velocidade
    {
        public DataTable Lista()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT VELOCIDADEID, 
												VALOR, 
												USUARIOID, 
												DATACADASTRO, 
												DATAALTERACAO, 
												ATIVO, 
												UNIDADEVELOCIDADEID
                                        FROM   FISCALIZACAOLINK.VELOCIDADE (NOLOCK) ";

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

        public DataTable ListaVelocidadeAtiva()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT V.VELOCIDADEID,
						CONVERT(VARCHAR, V.VALOR) + ' ' + U.DESCRICAO AS DESCRICAO
                        FROM FISCALIZACAOLINK.VELOCIDADE V (NOLOCK)
							 INNER JOIN FISCALIZACAOLINK.UNIDADEVELOCIDADE U (NOLOCK)
												   ON V.UNIDADEVELOCIDADEID = U.UNIDADEVELOCIDADEID
                        WHERE v.ATIVO = 1
                        ORDER BY VALOR ";

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

        public ValidacaoDados Valida(Entidades.Velocidade velocidade, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (velocidade == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (velocidade.VelocidadeId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (velocidade.UnidadeVelocidadeId == 0)
            {
                mensagens.Add("Campo UNIDADE é obrigatório.");
            } 

            if (velocidade.Valor == 0)
            {
                mensagens.Add("Campo VALOR é obrigatório.");
            }          

            if (velocidade.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    // Verifica se já existe a Descricao cadastrada 
                    if (this.PossuiOutraDescricaoCadastradaPor(contexto, velocidade.Valor, velocidade.UnidadeVelocidadeId, velocidade.VelocidadeId))
                    {
                        mensagens.Add("Esta VALOR já foi utilizada!");
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

        public bool PossuiUnidadeVelocidadePor(DataContext contexto, int unidadeVelocidadeId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM FISCALIZACAOLINK.VELOCIDADE (NOLOCK)
                                        WHERE UNIDADEVELOCIDADEID = @UNIDADEVELOCIDADEID";

            contextQuery.Parameters.Add("@UNIDADEVELOCIDADEID", SqlDbType.VarChar, unidadeVelocidadeId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiOutraDescricaoCadastradaPor(DataContext ctx, decimal valor, int unidadeVelocidadeId, int velocidadeId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM FISCALIZACAOLINK.VELOCIDADE
                                WHERE VALOR = @VALOR
                                    AND UNIDADEVELOCIDADEID = @UNIDADEVELOCIDADEID
	                                AND VELOCIDADEID <> @VELOCIDADEID ";

            contextQuery.Parameters.Add("@VALOR", valor);
            contextQuery.Parameters.Add("@UNIDADEVELOCIDADEID", unidadeVelocidadeId);
            contextQuery.Parameters.Add("@VELOCIDADEID", velocidadeId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.Velocidade velocidade)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO FISCALIZACAOLINK.VELOCIDADE 
                                                        (VALOR,ATIVO, USUARIOID, DATACADASTRO, DATAALTERACAO, UNIDADEVELOCIDADEID) 
                                            VALUES      (@VALOR,@ATIVO, @USUARIOID, @DATACADASTRO, @DATAALTERACAO, @UNIDADEVELOCIDADEID) ";

                contextQuery.Parameters.Add("@VALOR", velocidade.Valor);
                contextQuery.Parameters.Add("@ATIVO", velocidade.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", velocidade.UsuarioId);
                contextQuery.Parameters.Add("@UNIDADEVELOCIDADEID", velocidade.UnidadeVelocidadeId);
                contextQuery.Parameters.Add("@DATACADASTRO", DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);

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

        public void Atualiza(Entidades.Velocidade velocidade)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE FISCALIZACAOLINK.VELOCIDADE 
                                    SET    VALOR = @VALOR,
                                           ATIVO = @ATIVO,
                                           UNIDADEVELOCIDADEID = @UNIDADEVELOCIDADEID,
                                           USUARIOID = @USUARIOID,
                                           DATAALTERACAO = @DATAALTERACAO
                                    WHERE  VelocidadeID = @VelocidadeID ";

                contextQuery.Parameters.Add("@VALOR", velocidade.Valor);
                contextQuery.Parameters.Add("@ATIVO", velocidade.Ativo);
                contextQuery.Parameters.Add("@UNIDADEVELOCIDADEID", velocidade.UnidadeVelocidadeId);
                contextQuery.Parameters.Add("@VELOCIDADEID", velocidade.VelocidadeId);
                contextQuery.Parameters.Add("@USUARIOID", velocidade.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);

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

        public ValidacaoDados ValidaRemocao(int velocidadeId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.FiscalizacaoLink.CircuitoSetor rnCircuitoSetor = new CircuitoSetor();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (velocidadeId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (rnCircuitoSetor.PossuiContratoSetorVelocidadePor(contexto, velocidadeId))
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
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public void Remove(int velocidadeId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE FISCALIZACAOLINK.VELOCIDADE
                            WHERE  VELOCIDADEID = @VELOCIDADEID  ";

                contextQuery.Parameters.Add("@VELOCIDADEID", velocidadeId);

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class UnidadeMedida
    {
        public DataTable Lista()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT UNIDADEMEDIDAID, 
                           DESCRICAO, 
                           SIGLA,
                           ATIVO,
                           USUARIOID, 
                           DATACADASTRO, 
                           DATAALTERACAO 
                    FROM   PrestacaoContas.UNIDADEMEDIDA (NOLOCK) ";

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

        public DataTable ListaAtivo()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT UNIDADEMEDIDAID, 
                                               DESCRICAO + ' - ' + SIGLA as DESCRICAO
                                          FROM   PrestacaoContas.UNIDADEMEDIDA (NOLOCK)
                                          WHERE ATIVO = 1 ";

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

        public ValidacaoDados Valida(Entidades.UnidadeMedida unidadeMedida, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (unidadeMedida == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (unidadeMedida.UnidadeMedidaId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (unidadeMedida.Descricao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo DESCRIÇÃO é obrigatório.");
            }
            else
            {
                if (unidadeMedida.Descricao.Length > 1000)
                {
                    mensagens.Add("Campo DESCRIÇÃO deve possuir no máximo 1000 caracteres.");
                }
            }

            if (unidadeMedida.Sigla.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo SIGLA é obrigatório.");
            }
            else
            {
                if (unidadeMedida.Sigla.Length > 20)
                {
                    mensagens.Add("Campo SIGLA deve possuir no máximo 20 caracteres.");
                }
            }

            if (unidadeMedida.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUÁRIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    // Verifica se já existe a descricao cadastrada
                    if (this.PossuiOutraDescricaoCadastradaPor(contexto, unidadeMedida.Descricao, unidadeMedida.UnidadeMedidaId))
                    {
                        mensagens.Add("Esta DESCRIÇÃO já foi utilizada.");
                    }

                    // Verifica se já existe a descricao cadastrada
                    if (this.PossuiOutraSiglaCadastradaPor(contexto, unidadeMedida.Sigla, unidadeMedida.UnidadeMedidaId))
                    {
                        mensagens.Add("Esta SIGLA já foi utilizada.");
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

        private bool PossuiOutraDescricaoCadastradaPor(DataContext ctx, string descricao, int unidadeMedidaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM PrestacaoContas.UNIDADEMEDIDA (NOLOCK)
                                WHERE DESCRICAO = @DESCRICAO
	                                AND UNIDADEMEDIDAID <> @UNIDADEMEDIDAID ";

            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, descricao);
            contextQuery.Parameters.Add("@UNIDADEMEDIDAID", SqlDbType.Int, unidadeMedidaId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiOutraSiglaCadastradaPor(DataContext ctx, string sigla, int unidadeMedidaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM PrestacaoContas.UNIDADEMEDIDA (NOLOCK)
                                WHERE SIGLA = @SIGLA
	                                AND UNIDADEMEDIDAID <> @UNIDADEMEDIDAID ";

            contextQuery.Parameters.Add("@SIGLA", SqlDbType.VarChar, sigla);
            contextQuery.Parameters.Add("@UNIDADEMEDIDAID", SqlDbType.Int, unidadeMedidaId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.UnidadeMedida unidadeMedida)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"  INSERT INTO PrestacaoContas.UNIDADEMEDIDA
			                                    (DESCRICAO,
                                                 SIGLA, 
                                                 ATIVO, 
                                                 USUARIOID, 
                                                 DATACADASTRO, 
                                                 DATAALTERACAO) 
                                    VALUES      (@DESCRICAO, 
                                                 @SIGLA, 
                                                 @ATIVO, 
                                                 @USUARIOID, 
                                                 @DATACADASTRO, 
                                                 @DATAALTERACAO)   
                            SELECT IDENT_CURRENT('PrestacaoContas.UNIDADEMEDIDA')";

                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, unidadeMedida.Descricao);
                contextQuery.Parameters.Add("@SIGLA", SqlDbType.VarChar, unidadeMedida.Sigla);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, unidadeMedida.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, unidadeMedida.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                unidadeMedida.UnidadeMedidaId = Convert.ToInt32(ctx.GetReturnValue(contextQuery));
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

        public void Atualiza(Entidades.UnidadeMedida unidadeMedida)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE PrestacaoContas.UNIDADEMEDIDA
                                        SET    DESCRICAO = @DESCRICAO, 
                                               SIGLA = @SIGLA,
                                               ATIVO = @ATIVO, 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  UNIDADEMEDIDAID = @UNIDADEMEDIDAID ";

                contextQuery.Parameters.Add("@UNIDADEMEDIDAID", SqlDbType.Int, unidadeMedida.UnidadeMedidaId);
                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, unidadeMedida.Descricao);
                contextQuery.Parameters.Add("@SIGLA", SqlDbType.VarChar, unidadeMedida.Sigla);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, unidadeMedida.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, unidadeMedida.UsuarioId);
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

        public ValidacaoDados ValidaRemocao(int unidadeMedidaId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ProdutoServico rnProdutoServico = new ProdutoServico();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (unidadeMedidaId <= 0)
            {
                mensagens.Add("Campo CÓDIGO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se já foi associado
                    if (rnProdutoServico.PossuiUnidadeMedidaPor(contexto, unidadeMedidaId))
                    {
                        mensagens.Add("Esta unidade de medida não pode ser removida pois foi vinculada a um produto.");
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
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public void Remove(int unidadeMedidaId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE PrestacaoContas.unidadeMedida
                                         WHERE UNIDADEMEDIDAID = @UNIDADEMEDIDAID ";

                contextQuery.Parameters.Add("@UNIDADEMEDIDAID", SqlDbType.Int, unidadeMedidaId);

                contexto.ApplyModifications(contextQuery);
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
    }
}

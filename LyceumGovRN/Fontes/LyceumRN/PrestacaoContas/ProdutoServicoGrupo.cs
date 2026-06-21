using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class ProdutoServicoGrupo 
    {
        public string ObtemCodigoCnaePor(int produtoServicoGrupoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            try
            {
                contextQuery.Command = @" SELECT CODIGOCNAE 
                                        FROM PRESTACAOCONTAS.PRODUTOSERVICOGRUPO (NOLOCK)
                                        WHERE PRODUTOSERVICOGRUPOID = @PRODUTOSERVICOGRUPOID ";

                contextQuery.Parameters.Add("@PRODUTOSERVICOGRUPOID", SqlDbType.Int, produtoServicoGrupoId);

                resultado = contexto.GetReturnValue<string>(contextQuery);

                return resultado;
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

        public DataTable ListaAtivoPor()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"  SELECT  PRODUTOSERVICOGRUPOID, 
                                                   DESCRICAO,
                                                   CODIGOCNAE
                                            FROM PrestacaoContas.PRODUTOSERVICOGRUPO (NOLOCK)
                                                 WHERE ATIVO = 1
                                            ORDER BY PRODUTOSERVICOGRUPOID ";

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
                contextQuery.Command = @" SELECT  PRODUTOSERVICOGRUPOID, 
		                                        DESCRICAO, 
		                                        CODIGOCNAE,	                                 
		                                        ATIVO, 
		                                        USUARIOID, 
		                                        DATACADASTRO, 
		                                        DATAALTERACAO
                                        FROM PrestacaoContas.PRODUTOSERVICOGRUPO (NOLOCK)
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

        public ValidacaoDados Valida(Entidades.ProdutoServicoGrupo produtoServicoGrupo, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (produtoServicoGrupo == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (produtoServicoGrupo.ProdutoServicoGrupoId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (produtoServicoGrupo.Descricao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo DESCRIÇÃO é obrigatório.");
            }

            if (produtoServicoGrupo.CodigoCnae.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CÓDIGO CNAE é obrigatório.");
            }

            if (produtoServicoGrupo.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se já existe a descrição cadastrada
                    if (this.PossuiOutraDescricaoCadastradaPor(contexto, produtoServicoGrupo.Descricao, produtoServicoGrupo.ProdutoServicoGrupoId))
                    {
                        mensagens.Add("Esta DESCRIÇÃO já foi utilizada.");
                    }

                    //Verifica se já existe o codigo cadastrado
                    if (this.PossuiOutroCodigoCnaeCadastradoPor(contexto, produtoServicoGrupo.Descricao, produtoServicoGrupo.ProdutoServicoGrupoId))
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

        private bool PossuiOutraDescricaoCadastradaPor(DataContext ctx, string descricao, int produtoServicoGrupoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM PrestacaoContas.PRODUTOSERVICOGRUPO (NOLOCK)
                                WHERE DESCRICAO = @DESCRICAO
	                                AND PRODUTOSERVICOGRUPOID <> @PRODUTOSERVICOGRUPOID ";

            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, descricao);
            contextQuery.Parameters.Add("@PRODUTOSERVICOGRUPOID", SqlDbType.Int, produtoServicoGrupoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiOutroCodigoCnaeCadastradoPor(DataContext ctx, string codigoCnae, int produtoServicoGrupoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM PrestacaoContas.PRODUTOSERVICOGRUPO (NOLOCK)
                                WHERE CODIGOCNAE = @CODIGOCNAE
	                                AND PRODUTOSERVICOGRUPOID <> @PRODUTOSERVICOGRUPOID ";

            contextQuery.Parameters.Add("@CODIGOCNAE", SqlDbType.VarChar, codigoCnae);
            contextQuery.Parameters.Add("@PRODUTOSERVICOGRUPOID", SqlDbType.Int, produtoServicoGrupoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.ProdutoServicoGrupo produtoServicoGrupo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO PrestacaoContas.PRODUTOSERVICOGRUPO
                                                        (DESCRICAO, 
		                                                 CODIGOCNAE,	
                                                         ATIVO, 
                                                         USUARIOID, 
                                                         DATACADASTRO, 
                                                         DATAALTERACAO) 
                                            VALUES      (@DESCRICAO, 
		                                                 @CODIGOCNAE,
                                                         @ATIVO, 
                                                         @USUARIOID, 
                                                         @DATACADASTRO, 
                                                         @DATAALTERACAO) ";

                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, produtoServicoGrupo.Descricao);
                contextQuery.Parameters.Add("@CODIGOCNAE", SqlDbType.VarChar, produtoServicoGrupo.CodigoCnae);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, produtoServicoGrupo.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, produtoServicoGrupo.UsuarioId);
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

        public void Atualiza(Entidades.ProdutoServicoGrupo produtoServicoGrupo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE PrestacaoContas.PRODUTOSERVICOGRUPO
                                        SET    DESCRICAO = @DESCRICAO, 
                                               CODIGOCNAE = @CODIGOCNAE, 
                                               ATIVO = @ATIVO, 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  PRODUTOSERVICOGRUPOID = @PRODUTOSERVICOGRUPOID ";

                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, produtoServicoGrupo.Descricao);
                contextQuery.Parameters.Add("@CODIGOCNAE", SqlDbType.VarChar, produtoServicoGrupo.CodigoCnae);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, produtoServicoGrupo.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, produtoServicoGrupo.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@PRODUTOSERVICOGRUPOID", SqlDbType.Int, produtoServicoGrupo.ProdutoServicoGrupoId);

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

        public ValidacaoDados ValidaRemocao(int produtoServicoGrupoId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ProdutoServico rnProdutoServico = new ProdutoServico();
            Fornecedor rnFornecedor = new Fornecedor();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (produtoServicoGrupoId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se motivo ja foi utilizado
                    if (rnProdutoServico.PossuiProdutoServicoGrupoPor(contexto, produtoServicoGrupoId))
                    {
                        mensagens.Add("Este grupo não pode ser excluído, pois já foi utilizado para um Produto / Serviço.");
                    }

                    //Verifica se motivo ja foi utilizado
                    if (rnFornecedor.PossuiProdutoServicoGrupoPor(contexto, produtoServicoGrupoId))
                    {
                        mensagens.Add("Este grupo não pode ser excluído, pois já foi utilizado para um Fornecedor.");
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

        public void Remove(int produtoServicoGrupoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE PrestacaoContas.PRODUTOSERVICOGRUPO
                            WHERE  PRODUTOSERVICOGRUPOID = @PRODUTOSERVICOGRUPOID  ";

                contextQuery.Parameters.Add("@PRODUTOSERVICOGRUPOID", SqlDbType.Int, produtoServicoGrupoId);

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class FornecedorProdutoServicoGrupo
    {
        public DataTable ListaPor(int fornecedorId)
        {
            DataContext contexto = null;
            
            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                var contextQuery = new ContextQuery();

                contextQuery.Command = @"
                select * from PrestacaoContas.PRODUTOSERVICOGRUPO psg (nolock)
                left join PrestacaoContas.FORNECEDOR__PRODUTOSERVICOGRUPO fpsg (nolock) on psg.PRODUTOSERVICOGRUPOID = fpsg.PRODUTOSERVICOGRUPOID
                where fpsg.FORNECEDORID = @FORNECEDORID
                ";

                contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, fornecedorId);

                return contexto.GetDataTable(contextQuery);
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
        }

        public ValidacaoDados Valida(Entidades.FornecedorProdutoServicoGrupo f)
        {
            var validacaoDados = new ValidacaoDados();
            var mensagens = new List<string>();

            //escreva sua validação aqui!

            if (mensagens.Count == 0)
            {
                DataContext contexto = null;

                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //escreva a sua validação aqui!
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

        public ValidacaoDados ValidaRemocao(int fornecedorRazaoSocialId)
        {
            var validacaoDados = new ValidacaoDados();
            var mensagens = new List<string>();

            //escreva a validação aqui!

            if (mensagens.Count == 0)
            {
                DataContext contexto = null;

                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //escreva a validação de banco de dados aqui!
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

        public void Insere(Entidades.FornecedorProdutoServicoGrupo f) 
        {
            DataContext contexto = null;
            

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingLock();

                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @"
                    insert into PrestacaoContas.FORNECEDOR__PRODUTOSERVICOGRUPO (
	                    FORNECEDORID
	                    ,PRODUTOSERVICOGRUPOID
	                    ,USUARIOID
	                    ,DATACADASTRO
	                    ,DATAALTERACAO
                    ) values (
	                    @FORNECEDORID
	                    ,@PRODUTOSERVICOGRUPOID
	                    ,@USUARIOID
	                    ,@DATACADASTRO
	                    ,@DATACADASTRO
                    )

                    update PrestacaoContas.FORNECEDOR set FINALIZADO = 0 where FORNECEDORID = @FORNECEDORID
                ";

                contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, f.FornecedorId);
                contextQuery.Parameters.Add("@PRODUTOSERVICOGRUPOID", SqlDbType.Int, f.ProdutoServicoGrupoId);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, f.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, f.DataCadastro);

                contexto.ApplyModifications(contextQuery);
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
        }

        public void Atualiza(Entidades.FornecedorProdutoServicoGrupo f)
        {
            DataContext contexto = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingLock();
                
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @"
                    update PrestacaoContas.FORNECEDOR__PRODUTOSERVICOGRUPO set
                    PRODUTOSERVICOGRUPOID = @PRODUTOSERVICOGRUPOID
                    ,USUARIOID = @USUARIOID
                    ,DATAALTERACAO = @DATAALTERACAO
                    where FORNECEDOR__PRODUTOSERVICOGRUPOID = @FORNECEDORPRODUTOSERVICOGRUPOID

                declare @FORNECEDORID int
                select @FORNECEDORID = FORNECEDORID from PrestacaoContas.FORNECEDOR__PRODUTOSERVICOGRUPO 
                where FORNECEDOR__PRODUTOSERVICOGRUPOID = @FORNECEDORPRODUTOSERVICOGRUPOID

                update PrestacaoContas.FORNECEDOR set FINALIZADO = 0 where FORNECEDORID = @FORNECEDORID
                ";

                contextQuery.Parameters.Add("@FORNECEDORPRODUTOSERVICOGRUPOID", SqlDbType.Int, f.FornecedorProdutoServicoGrupoId);
                contextQuery.Parameters.Add("@PRODUTOSERVICOGRUPOID", SqlDbType.Int, f.ProdutoServicoGrupoId);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, f.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, f.DataAlteracao);

                contexto.ApplyModifications(contextQuery);
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
        }

        public void Remove(int fornecedorProdutoServicoGrupoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"
                    declare @FORNECEDORID int
                    select @FORNECEDORID = FORNECEDORID from PrestacaoContas.FORNECEDOR__PRODUTOSERVICOGRUPO 
                    where FORNECEDOR__PRODUTOSERVICOGRUPOID = @FORNECEDORPRODUTOSERVICOGRUPOID

                    update PrestacaoContas.FORNECEDOR set FINALIZADO = 0 where FORNECEDORID = @FORNECEDORID

                    delete from PrestacaoContas.FORNECEDOR__PRODUTOSERVICOGRUPO where FORNECEDOR__PRODUTOSERVICOGRUPOID = @FORNECEDORPRODUTOSERVICOGRUPOID
                ";

                contextQuery.Parameters.Add("@FORNECEDORPRODUTOSERVICOGRUPOID", SqlDbType.Int, fornecedorProdutoServicoGrupoId);

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
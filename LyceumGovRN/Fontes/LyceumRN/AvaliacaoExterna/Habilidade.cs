using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.AvaliacaoExterna
{
    public class Habilidade
    {
        public int ObtemIdPeloCodigo(string codigo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT HABILIDADEID 
                                        FROM AVALIACAOEXTERNA.HABILIDADE 
                                        WHERE CODIGO = @CODIGO ";

            contextQuery.Parameters.Add("@CODIGO", SqlDbType.VarChar, codigo);

            return contexto.GetReturnValue<int>(contextQuery);
        }

        public DataTable ListaAtivo()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT H.HABILIDADEID, 
		                                        H.COMPONENTEID, 
		                                        H.CODIGO, 
		                                        H.DESCRICAO, 
		                                        H.CODIGO + ' - ' + H.DESCRICAO AS CODIGODESCRICAO, 
		                                        H.ATIVO, 
		                                        H.USUARIOID, 
		                                        H.DATACADASTRO, 
		                                        H.DATAALTERACAO 
                                        FROM AVALIACAOEXTERNA.HABILIDADE H 
										WHERE H.ATIVO = 1
                                        ORDER BY H.CODIGO ";

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

        public DataTable ListaPorComponente(int componenteId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT H.HABILIDADEID, 
		                                        H.COMPONENTEID, 
		                                        H.CODIGO, 
		                                        H.DESCRICAO, 
		                                        H.CODIGO + ' - ' + H.DESCRICAO AS CODIGODESCRICAO, 
		                                        H.ATIVO, 
		                                        H.USUARIOID, 
		                                        H.DATACADASTRO, 
		                                        H.DATAALTERACAO 
                                        FROM AVALIACAOEXTERNA.HABILIDADE H 
	                                        INNER JOIN AVALIACAOEXTERNA.COMPONENTE C ON C.COMPONENTEID = H.COMPONENTEID
                                        WHERE H.COMPONENTEID = @COMPONENTEID
                                              AND H.ATIVO = 1
                                        ORDER BY H.CODIGO ";

                contextQuery.Parameters.Add("@COMPONENTEID", SqlDbType.Int, componenteId);

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

        public void Insere(Entidades.Habilidade habilidade)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"INSERT INTO AvaliacaoExterna.HABILIDADE
                                                        (COMPONENTEID,
                                                         CODIGO,
                                                         DESCRICAO,
                                                         ATIVO, 
                                                         USUARIOID, 
                                                         DATACADASTRO, 
                                                         DATAALTERACAO)
                                            VALUES      (@COMPONENTEID,
                                                         @CODIGO,
                                                         @DESCRICAO,
                                                         @ATIVO, 
                                                         @USUARIOID, 
                                                         @DATACADASTRO, 
                                                         @DATAALTERACAO)

                                            SELECT IDENT_CURRENT('AvaliacaoExterna.HABILIDADE') ";

                contextQuery.Parameters.Add("@COMPONENTEID", SqlDbType.Int, habilidade.ComponenteId);
                contextQuery.Parameters.Add("@CODIGO", SqlDbType.VarChar, habilidade.Codigo);
                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, habilidade.Descricao);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, habilidade.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, habilidade.UsuarioID);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                habilidade.HabilidadeId = Convert.ToInt32(ctx.GetReturnValue(contextQuery));
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

        public void Atualiza(Entidades.Habilidade habilidade)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE AvaliacaoExterna.HABILIDADE SET
                                                         COMPONENTEID = @COMPONENTEID,
                                                         CODIGO = @CODIGO,
                                                         DESCRICAO = @DESCRICAO,
                                                         ATIVO = @ATIVO, 
                                                         USUARIOID = @USUARIOID, 
                                                         DATAALTERACAO = @DATAALTERACAO
                                          WHERE HABILIDADEID = @HABILIDADEID ";

                contextQuery.Parameters.Add("@HABILIDADEID", SqlDbType.Int, habilidade.HabilidadeId);
                contextQuery.Parameters.Add("@COMPONENTEID", SqlDbType.Int, habilidade.ComponenteId);
                contextQuery.Parameters.Add("@CODIGO", SqlDbType.VarChar, habilidade.Codigo);
                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, habilidade.Descricao);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, habilidade.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, habilidade.UsuarioID);
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

        public ValidacaoDados ValidaRemocao(int habilidadeId)
        {
            List<string> mensagens = new List<string>();
            RN.AvaliacaoExterna.Questao rnQuestao = new Questao();
            ValidacaoDados validacaoDados = new ValidacaoDados();

            //Verifica se o ID foi fornecido
            if (habilidadeId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            //Verifica se tem detalhes vinculados ao objeto a ser excluído
            if (habilidadeId > 0 && rnQuestao.PossuiHabilidadePor(habilidadeId))
            {
                mensagens.Add("Esta habilidade não pode ser excluída porque possui questões vinculadas à ela. Exclua as questões antes.");
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

        public void Remove(int habilidadeId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE FROM AvaliacaoExterna.HABILIDADE WHERE HABILIDADEID = @HABILIDADEID ";

                contextQuery.Parameters.Add("@HABILIDADEID", SqlDbType.Int, habilidadeId);

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

        public ValidacaoDados Valida(Entidades.Habilidade habilidade, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados();

            if (habilidade == null)
            {
                return validacaoDados;
            }
            if (!cadastro)
            {
                if (habilidade.HabilidadeId <= 0)
                {
                    mensagens.Add("Campo ID é obrigatório.");
                }
            }

            if (habilidade.ComponenteId <= 0)
            {
                mensagens.Add("Campo COMPONENTE é obrigatório.");
            }

            if (habilidade.Codigo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CÓDIGO  é obrigatório.");
            }

            if (habilidade.Descricao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo DESCRIÇÃO é obrigatório.");
            }

            if (habilidade.UsuarioID.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUÁRIO é obrigatório.");
            }

            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [AvaliacaoExterna].[HABILIDADE] (NOLOCK)
                                WHERE CODIGO = @CODIGO
                                AND HABILIDADEID <> @HABILIDADEID ";

                contextQuery.Parameters.Add("@CODIGO", SqlDbType.VarChar, habilidade.Codigo);
                contextQuery.Parameters.Add("@HABILIDADEID", SqlDbType.Int, habilidade.HabilidadeId);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    mensagens.Add("Este CÓDIGO já foi utilizado.");
                }
            }
            catch (Exception ex)
            {
                if (ctx != null)
                {
                    ctx.Abandon();
                }
                throw new Exception(ex.Message);
            }
            finally
            {
                if (ctx != null)
                {
                    ctx.Dispose();
                }
            }

            if (habilidade.UsuarioID.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
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

        public bool PossuiComponentePor(DataContext ctx, int componenteId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT COUNT(0) 
                                FROM [AvaliacaoExterna].[HABILIDADE] (NOLOCK)
                                WHERE COMPONENTEID = @COMPONENTEID ";

            contextQuery.Parameters.Add("@COMPONENTEID", SqlDbType.Int, componenteId);

            return ctx.GetReturnValue<int>(contextQuery) > 0;
        }        
    }
}
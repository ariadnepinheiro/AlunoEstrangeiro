using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.AvaliacaoExterna
{
    public class Componente
    {
        public DataTable ListaAtivo()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT  COMPONENTEID, 
		                                        DESCRICAO, 
		                                        ATIVO, 
		                                        USUARIOID, 
		                                        DATACADASTRO, 
		                                        DATAALTERACAO
                                        FROM AvaliacaoExterna.COMPONENTE (NOLOCK)
                                        WHERE ATIVO = 1
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

        public DataTable Lista()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT  COMPONENTEID, 
		                                        DESCRICAO, 
		                                        ATIVO, 
		                                        USUARIOID, 
		                                        DATACADASTRO, 
		                                        DATAALTERACAO
                                        FROM AvaliacaoExterna.COMPONENTE (NOLOCK)
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

        public string RetornaDescricaoPor(int componenteId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            try
            {
                contextQuery.Command = @" select DESCRICAO
                                    from AvaliacaoExterna.COMPONENTE
                                    where COMPONENTEID = @COMPONENTEID ";

                contextQuery.Parameters.Add("@COMPONENTEID", SqlDbType.Int, componenteId);

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

        private bool PossuiOutraDescricaoCadastradaPor(DataContext ctx, string descricao, int componenteId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [AvaliacaoExterna].[COMPONENTE] (NOLOCK)
                                WHERE DESCRICAO = @DESCRICAO
	                                AND COMPONENTEID <> @COMPONENTEID ";

            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, descricao);
            contextQuery.Parameters.Add("@COMPONENTEID", SqlDbType.Int, componenteId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.Componente componente)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO AvaliacaoExterna.COMPONENTE
                                                        (DESCRICAO, 
                                                         ATIVO, 
                                                         USUARIOID, 
                                                         DATACADASTRO, 
                                                         DATAALTERACAO) 
                                            VALUES      (@DESCRICAO, 
                                                         @ATIVO, 
                                                         @USUARIOID, 
                                                         @DATACADASTRO, 
                                                         @DATAALTERACAO) ";

                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, componente.Descricao);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, componente.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, componente.UsuarioId);
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

        public void Atualiza(Entidades.Componente componente)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE AvaliacaoExterna.COMPONENTE
                                        SET    DESCRICAO = @DESCRICAO, 
                                               ATIVO = @ATIVO, 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  COMPONENTEID = @COMPONENTEID ";

                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, componente.Descricao);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, componente.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, componente.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@COMPONENTEID", SqlDbType.Int, componente.ComponenteId);

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

        public void Remove(int componenteId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE AvaliacaoExterna.COMPONENTE
                            WHERE  COMPONENTEID = @COMPONENTEID  ";

                contextQuery.Parameters.Add("@COMPONENTEID", SqlDbType.Int, componenteId);

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

        public ValidacaoDados Valida(Entidades.Componente componente, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (componente == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (componente.ComponenteId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (componente.Descricao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo DESCRIÇÃO é obrigatório.");
            }

            if (componente.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    // Verifica se já existe a descrição cadastrada
                    if (this.PossuiOutraDescricaoCadastradaPor(contexto, componente.Descricao, componente.ComponenteId))
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

        public ValidacaoDados ValidaRemocao(int componente)
        {
            List<string> mensagens = new List<string>();
            RN.AvaliacaoExterna.Habilidade rnHabilidade = new Habilidade();
            RN.AvaliacaoExterna.AlunoParticipante rnAlunoParticipante = new AlunoParticipante();
            ValidacaoDados validacaoDados = new ValidacaoDados();
            DataContext contexto = null;

            //Verifica se o ID foi fornecido
            if (componente <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }            

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se tem habilidades vinculados ao objeto a ser excluído
                    if (rnHabilidade.PossuiComponentePor(contexto, componente))
                    {
                        mensagens.Add("Este componente não pode ser excluído porque possui habilidades vinculados à ele. Exclua as habilidades antes.");
                    }

                    //Verifica se tem aluno participantes
                    if (rnAlunoParticipante.PossuiComponentePor(contexto, componente))
                    {
                        mensagens.Add("Este componente não pode ser excluído porque possui habilidades vinculados à ele. Exclua as habilidades antes.");
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

        public DataTable ListaPorProva(int provaId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT C.COMPONENTEID, 
		                                        C.DESCRICAO, 
		                                        C.ATIVO, 
		                                        C.USUARIOID, 
		                                        C.DATACADASTRO, 
		                                        C.DATAALTERACAO
                                        FROM AVALIACAOEXTERNA.COMPONENTE C (NOLOCK)
										INNER JOIN AVALIACAOEXTERNA.HABILIDADE H (NOLOCK)
												ON C.COMPONENTEID = H.COMPONENTEID
										INNER JOIN AVALIACAOEXTERNA.QUESTAO Q (NOLOCK)
												ON H.HABILIDADEID = Q.HABILIDADEID
										WHERE Q.PROVAID = @PROVAID
                                        ORDER BY DESCRICAO ";

                contextQuery.Parameters.Add("@PROVAID", SqlDbType.Int, provaId);

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

        public IList<Entidades.Componente> CarregaPorProva(DataContext ctx, int provaId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"SELECT DISTINCT C.*
                                        FROM AVALIACAOEXTERNA.COMPONENTE C (NOLOCK)
										INNER JOIN AVALIACAOEXTERNA.HABILIDADE H (NOLOCK)
												ON C.COMPONENTEID = H.COMPONENTEID
										INNER JOIN AVALIACAOEXTERNA.QUESTAO Q (NOLOCK)
												ON H.HABILIDADEID = Q.HABILIDADEID
										WHERE Q.PROVAID = @PROVAID ";

            contextQuery.Parameters.Add("@PROVAID", SqlDbType.Int, provaId);

            return ctx.GetDataTable(contextQuery).ToList<Entidades.Componente>();
        }
    }
}

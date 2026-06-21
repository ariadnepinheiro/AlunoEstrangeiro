using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.AvaliacaoExterna
{
    public class Prova
    {
        public DataTable ListaProvaAvaliacaoPor(int avaliacaoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT P.PROVAID,
	                                     P.AVALIACAOID, 
	                                     A.DESCRICAO AS AVALIACAO,
	                                     P.DESCRICAO, 
	                                     P.QUANTIDADEQUESTOES, 
	                                     P.ATIVO,
	                                     P.USUARIOID, 
	                                     P.DATACADASTRO, 
	                                     P.DATAALTERACAO
                                    FROM AVALIACAOEXTERNA.PROVA P
	                                    INNER JOIN AVALIACAOEXTERNA.AVALIACAO A ON P.AVALIACAOID = A.AVALIACAOID
                                    WHERE P.AVALIACAOID = @AVALIACAOID ";

                contextQuery.Parameters.Add("@AVALIACAOID", SqlDbType.Int, avaliacaoId);
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

        public DataTable ListaAtivoPor(int avaliacaoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT PROVAID,
	                                     AVALIACAOID, 
	                                     DESCRICAO, 
	                                     QUANTIDADEQUESTOES, 
	                                     ATIVO,
	                                     USUARIOID, 
	                                     DATACADASTRO, 
	                                     DATAALTERACAO
                                    FROM AVALIACAOEXTERNA.PROVA (NOLOCK)
                                    WHERE AVALIACAOID = @AVALIACAOID
                                         AND ATIVO = 1 ";

                contextQuery.Parameters.Add("@AVALIACAOID", SqlDbType.Int, avaliacaoId);
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

        public DataTable ListaAtivoPor(int avaliacaoId, string censo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT P.PROVAID,
											   P.AVALIACAOID, 
											   P.DESCRICAO, 
											   P.QUANTIDADEQUESTOES, 
											   P.ATIVO,
											   P.USUARIOID, 
											   P.DATACADASTRO, 
											   P.DATAALTERACAO
                                        FROM  AVALIACAOEXTERNA.PROVA P (NOLOCK)				                                       
											  INNER JOIN AVALIACAOEXTERNA.ETAPA E (NOLOCK) ON E.PROVAID = P.PROVAID
	                                          INNER JOIN LY_UNIDADE_ENSINO_CURSOS LYC (NOLOCK)
                                                        ON LYC.CURSO = E.CURSO
                                        WHERE P.AVALIACAOID = @AVALIACAOID
											AND LYC.UNIDADE_ENS = @CENSO
											AND P.ATIVO = 1
											AND E.ATIVO = 1
                                        ORDER BY P.DESCRICAO ";

                contextQuery.Parameters.Add("@AVALIACAOID", SqlDbType.Int, avaliacaoId);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);

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

        public ValidacaoDados Valida(Entidades.Prova prova, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (prova == null)
            {
                return validacaoDados;
            }

            if (!cadastro)
            {
                if (prova.ProvaId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (prova.AvaliacaoId <= 0)
            {
                mensagens.Add("Campo AVALIAÇÃO é obrigatório.");
            }

            if (prova.Descricao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo DESCRICAO é obrigatório.");
            }
            else if (prova.Descricao.Length > 500)
            {
                mensagens.Add("Campo DESCRICAO deve ser composto no máximo por 500 caracteres.");
            }

            if (prova.QuantidadeQuestoes <= 0)
            {
                mensagens.Add("Campo QUANTIDADE DE QUESTÕES é obrigatório.");
            }

            if (prova.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUÁRIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se já existe a descrição cadastrada
                    if (this.PossuiOutraDescricaoCadastradaPor(contexto, prova.Descricao, prova.ProvaId))
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
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public bool PossuiAvaliacaoPor(DataContext contexto, int avaliacaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(0) 
                                FROM [AvaliacaoExterna].[PROVA] (NOLOCK)
                                WHERE AVALIACAOID = @AVALIACAOID ";

            contextQuery.Parameters.Add("@AVALIACAOID", SqlDbType.Int, avaliacaoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public int ObtemQtdQuestoesPorEtapaId(int provaId)
        {
            var dataTable = new DataTable();

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery();

                contextQuery.Command = @" SELECT count(0) as QTDQUESTOES 
                                            FROM AvaliacaoExterna.QUESTAO
                                            WHERE PROVAID = @PROVAID ";

                contextQuery.Parameters.Add("@PROVAID", SqlDbType.Int, provaId);

                dataTable = ctx.GetDataTable(contextQuery);
            }

            return dataTable.Rows.Count > 0 ? Convert.ToInt32(dataTable.Rows[0]["QTDQUESTOES"]) : 0;
        }

        private bool PossuiOutraDescricaoCadastradaPor(DataContext ctx, string descricao, int provaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM AVALIACAOEXTERNA.PROVA (NOLOCK)
                                WHERE DESCRICAO = @DESCRICAO
	                                AND PROVAID <> @PROVAID ";

            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, descricao);
            contextQuery.Parameters.Add("@PROVAID", SqlDbType.Int, provaId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.Prova prova)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO AvaliacaoExterna.PROVA
                                               (AVALIACAOID
                                               ,DESCRICAO
                                               ,QUANTIDADEQUESTOES
                                               ,ATIVO
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO)
                                         VALUES
                                               (@AVALIACAOID,
                                               @DESCRICAO, 
                                               @QUANTIDADEQUESTOES,
                                               @ATIVO,
                                               @USUARIOID, 
                                               @DATACADASTRO, 
                                               @DATAALTERACAO) ";

                contextQuery.Parameters.Add("@AVALIACAOID", SqlDbType.Int, prova.AvaliacaoId);
                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, prova.Descricao);
                contextQuery.Parameters.Add("@QUANTIDADEQUESTOES", SqlDbType.Int, prova.QuantidadeQuestoes);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, prova.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, prova.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now); 

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

        public void Atualiza(Entidades.Prova prova)
        {	
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE AvaliacaoExterna.PROVA
                                           SET DESCRICAO = @DESCRICAO, 
                                               QUANTIDADEQUESTOES = @QUANTIDADEQUESTOES, 
                                               ATIVO = @ATIVO,
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO
                                         WHERE PROVAID = @PROVAID ";

                contextQuery.Parameters.Add("@PROVAID", SqlDbType.Int, prova.ProvaId);
                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, prova.Descricao);
                contextQuery.Parameters.Add("@QUANTIDADEQUESTOES", SqlDbType.Int, prova.QuantidadeQuestoes);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, prova.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, prova.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now); 

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

        public ValidacaoDados ValidaRemocao(int provaId)
        {
            List<string> mensagens = new List<string>();
            RN.AvaliacaoExterna.Etapa rnEtapa = new Etapa();
            RN.AvaliacaoExterna.Questao rnQuestao = new Questao();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };


            if (provaId <= 0)
            {
                mensagens.Add("Campo CODIGO é obrigatório.");
            }                      
          
            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se já existe questoes cadastradas
                    if (rnQuestao.PossuiProvaPor(contexto, provaId))
                    { 
                        mensagens.Add("Esta prova não pode ser excluída pois já possui questões cadastradas.");
                    }

                    //Verifica se já existe etapas cadastradas
                    if (rnEtapa.PossuiProvaPor(contexto, provaId))
                    {
                        mensagens.Add("Esta prova não pode ser excluída pois já possui etapas cadastradas.");
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

        public void Remove(int provaId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE AvaliacaoExterna.PROVA
                                          WHERE PROVAID = @PROVAID ";

                contextQuery.Parameters.Add("@PROVAID", SqlDbType.Int, provaId);

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Util;
using System.Data;
using Seeduc.Infra.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.Ocorrencias
{
    public class SubClasse
    {
        public bool PossuiClassePor(DataContext contexto, int classeId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Ocorrencias.SUBCLASSE (NOLOCK)
                                    WHERE CLASSEID = @CLASSEID ";

            contextQuery.Parameters.Add("@CLASSEID", SqlDbType.Int, classeId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public DataTable ListaAtivoPor(int classeId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"  SELECT  SUBCLASSEID, 
                                                    S.DESCRICAO,
                                                    C.CLASSEID,
                                                    C.DESCRICAO AS CLASSE,
                                                    S.ATIVO
                                            FROM OCORRENCIAS.SUBCLASSE S(NOLOCK)
                                            INNER JOIN OCORRENCIAS.CLASSE C ON S.CLASSEID=C.CLASSEID
                                                 WHERE S.ATIVO = 1
                                                    AND C.CLASSEID = @CLASSEID
                                            ORDER BY ORDEM ";

                contextQuery.Parameters.Add("@CLASSEID", SqlDbType.Int, classeId);

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

        public DataTable ListaPor(int classeId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT  SUBCLASSEID, 
                                                    S.DESCRICAO,
                                                    C.CLASSEID,
                                                    C.DESCRICAO AS CLASSE,
                                                    S.ATIVO,
                                                    S.ORDEM
                                            FROM OCORRENCIAS.SUBCLASSE S(NOLOCK)
                                            INNER JOIN OCORRENCIAS.CLASSE C ON S.CLASSEID=C.CLASSEID
                                                 WHERE C.CLASSEID = @CLASSEID
                                            ORDER BY S.ORDEM";

                contextQuery.Parameters.Add("@CLASSEID", SqlDbType.Int, classeId);


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

        public ValidacaoDados Valida(Entidades.SubClasse subClasse, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (subClasse == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (subClasse.SubClasseId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }
            if (subClasse.ClasseId <= 0)
            {
                mensagens.Add("Campo CLASSE é obrigatório.");
            }

            if (subClasse.Descricao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo DESCRIÇÃO é obrigatório.");
            }

            if (subClasse.Ordem <= 0)
            {
                mensagens.Add("Campo ORDEM é obrigatório.");
            }

            if (subClasse.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se já existe a descrição cadastrada
                    if (this.PossuiOutraDescricaoCadastradaPor(contexto, subClasse.Descricao, subClasse.SubClasseId))
                    {
                        mensagens.Add("Esta DESCRIÇÃO já foi utilizada.");
                    }

                    //Verifica se já existe a ordem cadastrada
                    if (this.PossuiOutraOrdemCadastradaPor(contexto, subClasse.Ordem, subClasse.SubClasseId, subClasse.ClasseId))
                    {
                        mensagens.Add("Esta ORDEM já foi utilizada para esta classe.");
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

        private bool PossuiOutraOrdemCadastradaPor(DataContext ctx, int ordem, int subClasseId, int classeId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM Ocorrencias.SUBCLASSE (NOLOCK)
                                WHERE ORDEM = @ORDEM
	                                AND SUBCLASSEID <> @SUBCLASSEID 
                                    AND CLASSEID = @CLASSEID ";

            contextQuery.Parameters.Add("@ORDEM", SqlDbType.Int, ordem);
            contextQuery.Parameters.Add("@SUBCLASSEID", SqlDbType.Int, subClasseId);
            contextQuery.Parameters.Add("@CLASSEID", SqlDbType.Int, classeId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiOutraDescricaoCadastradaPor(DataContext ctx, string descricao, int subClasseId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM Ocorrencias.SUBCLASSE (NOLOCK)
                                WHERE DESCRICAO = @DESCRICAO
	                                AND SUBCLASSEID <> @SUBCLASSEID ";

            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, descricao);
            contextQuery.Parameters.Add("@SUBCLASSEID", SqlDbType.Int, subClasseId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.SubClasse subClasse)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO Ocorrencias.SubClasse
                                                        (CLASSEID,
                                                         DESCRICAO, 
                                                         ORDEM, 
                                                         ATIVO, 
                                                         USUARIOID, 
                                                         DATACADASTRO, 
                                                         DATAALTERACAO) 
                                            VALUES      (@CLASSEID,
                                                         @DESCRICAO, 
                                                         @ORDEM, 
                                                         @ATIVO, 
                                                         @USUARIOID, 
                                                         @DATACADASTRO, 
                                                         @DATAALTERACAO) ";

                contextQuery.Parameters.Add("@CLASSEID", SqlDbType.Int, subClasse.ClasseId);
                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, subClasse.Descricao);
                contextQuery.Parameters.Add("@ORDEM", SqlDbType.Int, subClasse.Ordem);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, subClasse.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, subClasse.UsuarioId);
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

        public void Atualiza(Entidades.SubClasse subClasse)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE Ocorrencias.SUBCLASSE
                                        SET    CLASSEID = @CLASSEID,
                                               DESCRICAO = @DESCRICAO,
                                               ORDEM = @ORDEM, 
                                               ATIVO = @ATIVO, 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  SUBCLASSEID = @SUBCLASSEID ";

                contextQuery.Parameters.Add("@CLASSEID", SqlDbType.Int, subClasse.ClasseId);
                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, subClasse.Descricao);
                contextQuery.Parameters.Add("@ORDEM", SqlDbType.Int, subClasse.Ordem);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, subClasse.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, subClasse.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@SUBCLASSEID", SqlDbType.Int, subClasse.SubClasseId);

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

        public ValidacaoDados ValidaRemocao(int subClasseId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            Ocorrencia rnOcorrencia = new Ocorrencia();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (subClasseId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se ja foi utilizado
                    if (rnOcorrencia.PossuiSubClassePor(contexto, subClasseId))
                    {
                        mensagens.Add("Esta subclasse não pode ser excluída, pois já foi utilizada para um Registro de Violência Escolar.");
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

        public void Remove(int subClasseId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE Ocorrencias.SubClasse
                            WHERE  SUBCLASSEID = @SUBCLASSEID  ";

                contextQuery.Parameters.Add("@SUBCLASSEID", SqlDbType.Int, subClasseId);

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


        public int ObtemIdSubClassePor(string descricao)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int id = 0;

            try
            {
                contextQuery.Command = @" SELECT SUBCLASSEID
                                            FROM   Ocorrencias.SUBCLASSE (NOLOCK) 
											WHERE DESCRICAO = @DESCRICAO ";

                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, descricao);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    id = (int)reader["SUBCLASSEID"];
                }

                return id;
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


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.CartaoEstudante
{
    public class ExecucaoIntegrador
    {
        public DataTable Lista()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT EXECUCAOINTEGRADORID,
			                            TIPO,
										DATAEXECUCAO,
										PARAMETROSCONSULTA,
			                            QUANTIDADEREGISTROS,
			                            CADASTRADOS,
			                            ALTERADOS,
			                            CASE
											WHEN SUCESSO IS NULL THEN 'Não finalizado'
											WHEN SUCESSO = 1 THEN 'Sucesso'											 
											ELSE 'Falha'
										END SUCESSO,
			                            ERRO
                            FROM CartaoEstudante.EXECUCAOINTEGRADOR (NOLOCK) 
                            ORDER BY DATACADASTRO DESC ";

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

        public void Insere(Entity.ExecucaoIntegrador execucaoIntegrador)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" INSERT INTO CartaoEstudante.EXECUCAOINTEGRADOR
                                               (TIPO
                                               ,DATAEXECUCAO
                                               ,PARAMETROSCONSULTA
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO)
                                         VALUES
                                               (@TIPO, 
                                               @DATAEXECUCAO, 
                                               @PARAMETROSCONSULTA, 
                                               @USUARIOID, 
                                               @DATACADASTRO, 
                                               @DATAALTERACAO)

                                       SELECT IDENT_CURRENT('CartaoEstudante.EXECUCAOINTEGRADOR') ";

                contextQuery.Parameters.Add("@TIPO", SqlDbType.VarChar, execucaoIntegrador.Tipo);
                contextQuery.Parameters.Add("@DATAEXECUCAO", SqlDbType.DateTime, execucaoIntegrador.DataExecucao);
                contextQuery.Parameters.Add("@PARAMETROSCONSULTA", SqlDbType.VarChar, execucaoIntegrador.ParametrosConsulta);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, execucaoIntegrador.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                execucaoIntegrador.ExecucaoIntegradorId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
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

        private int ObtemIdPor(DataContext contexto, Entity.ExecucaoIntegrador execucaoIntegrador)
        {
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int retorno = 0;

            try
            {
                contextQuery.Command = @" SELECT EXECUCAOINTEGRADORID
                                        FROM CartaoEstudante.EXECUCAOINTEGRADOR 
                                        WHERE TIPO = @TIPO
	                                        AND PARAMETROSCONSULTA = @PARAMETROSCONSULTA
	                                        AND DATAEXECUCAO = @DATAEXECUCAO ";

                contextQuery.Parameters.Add("@TIPO", SqlDbType.VarChar, execucaoIntegrador.Tipo);
                contextQuery.Parameters.Add("@DATAEXECUCAO", SqlDbType.DateTime, execucaoIntegrador.DataExecucao);
                contextQuery.Parameters.Add("@PARAMETROSCONSULTA", SqlDbType.VarChar, execucaoIntegrador.ParametrosConsulta);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["EXECUCAOINTEGRADORID"]);
                }

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public void AtualizaResultado(Entity.ExecucaoIntegrador execucaoIntegrador)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                this.AtualizaResultado(contexto, execucaoIntegrador);
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

        private void AtualizaResultado(DataContext contexto, Entity.ExecucaoIntegrador execucaoIntegrador)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE CartaoEstudante.EXECUCAOINTEGRADOR
                                       SET QUANTIDADEREGISTROS = @QUANTIDADEREGISTROS, 
	                                      CADASTRADOS = @CADASTRADOS,
                                          ALTERADOS = @ALTERADOS, 
                                          SUCESSO = @SUCESSO, 
                                          ERRO = @ERRO, 
                                          USUARIOID = @USUARIOID, 
                                          DATAALTERACAO = @DATAALTERACAO
                                     WHERE EXECUCAOINTEGRADORID = @EXECUCAOINTEGRADORID ";

            contextQuery.Parameters.Add("@EXECUCAOINTEGRADORID", SqlDbType.Int, execucaoIntegrador.ExecucaoIntegradorId);
            contextQuery.Parameters.Add("@QUANTIDADEREGISTROS", SqlDbType.Int, execucaoIntegrador.QuantidadeRegistros == null ? DBNull.Value : (object)execucaoIntegrador.QuantidadeRegistros);
            contextQuery.Parameters.Add("@CADASTRADOS", SqlDbType.Int, execucaoIntegrador.Cadastrados == null ? DBNull.Value : (object)execucaoIntegrador.Cadastrados);
            contextQuery.Parameters.Add("@ALTERADOS", SqlDbType.Int, execucaoIntegrador.Alterados == null ? DBNull.Value : (object)execucaoIntegrador.Alterados);
            contextQuery.Parameters.Add("@SUCESSO", SqlDbType.Bit, execucaoIntegrador.Sucesso == null ? DBNull.Value : (object)execucaoIntegrador.Sucesso);
            contextQuery.Parameters.Add("@ERRO", SqlDbType.VarChar, execucaoIntegrador.Erro);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, execucaoIntegrador.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void SalvaComErro(Entity.ExecucaoIntegrador execucaoIntegrador)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Verifica se já foi criado o registro
                int id = this.ObtemIdPor(contexto, execucaoIntegrador);

                if (id > 0)
                {
                    execucaoIntegrador.ExecucaoIntegradorId = id;
                    this.AtualizaResultado(contexto, execucaoIntegrador);
                }
                else
                {
                    this.InsereComErro(contexto, execucaoIntegrador);
                }
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

        private void InsereComErro(DataContext contexto, Entity.ExecucaoIntegrador execucaoIntegrador)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO CartaoEstudante.EXECUCAOINTEGRADOR
                                               (TIPO
                                               ,DATAEXECUCAO
                                               ,PARAMETROSCONSULTA
                                               ,SUCESSO
                                               ,ERRO
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO)
                                         VALUES
                                               (@TIPO, 
                                               @DATAEXECUCAO, 
                                               @PARAMETROSCONSULTA,
                                               @SUCESSO,
                                               @ERRO, 
                                               @USUARIOID, 
                                               @DATACADASTRO, 
                                               @DATAALTERACAO)";

            contextQuery.Parameters.Add("@TIPO", SqlDbType.VarChar, execucaoIntegrador.Tipo);
            contextQuery.Parameters.Add("@DATAEXECUCAO", SqlDbType.DateTime, execucaoIntegrador.DataExecucao);
            contextQuery.Parameters.Add("@PARAMETROSCONSULTA", SqlDbType.VarChar, execucaoIntegrador.ParametrosConsulta);
            contextQuery.Parameters.Add("@SUCESSO", SqlDbType.Bit, false);
            contextQuery.Parameters.Add("@ERRO", SqlDbType.VarChar, execucaoIntegrador.Erro);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, execucaoIntegrador.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }
    }
}

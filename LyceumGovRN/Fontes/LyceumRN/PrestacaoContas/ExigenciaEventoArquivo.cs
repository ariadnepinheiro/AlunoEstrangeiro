using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Seeduc.Infra.Data;
using System.Data;
using System.Web;

namespace Techne.Lyceum.RN.PrestacaoContas
{    
    public class ExigenciaEventoArquivo
    {
        public bool PossuiExigenciaEventoPor(DataContext contexto, int exigenciaEventoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM PrestacaoContas.EXIGENCIAEVENTOARQUIVO (NOLOCK)
                                    WHERE EXIGENCIAEVENTOID = @EXIGENCIAEVENTOID ";

            contextQuery.Parameters.Add("@EXIGENCIAEVENTOID", SqlDbType.Int, exigenciaEventoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public byte[] ObtemArquivoPorId(int exigenciaEventoArquivoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            byte[] arquivo = null;

            try
            {
                contextQuery.Command = @" SELECT ARQUIVO, 
	                                               TIPOARQUIVO, 
                                                   NOMEARQUIVO 
                                            FROM   PrestacaoContas.EXIGENCIAEVENTOARQUIVO (NOLOCK) 
											WHERE EXIGENCIAEVENTOARQUIVOID = @EXIGENCIAEVENTOARQUIVOID ";

                contextQuery.Parameters.Add("@EXIGENCIAEVENTOARQUIVOID", SqlDbType.Int, exigenciaEventoArquivoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    arquivo = (byte[])reader["ARQUIVO"];
                }

                return arquivo;
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

        public byte[] ObtemArquivoPor(int exigenciaEventoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            byte[] arquivo = null;

            try
            {
                contextQuery.Command = @" SELECT ARQUIVO, 
	                                               TIPOARQUIVO, 
                                                   NOMEARQUIVO 
                                            FROM   PrestacaoContas.EXIGENCIAEVENTOARQUIVO (NOLOCK) 
											WHERE EXIGENCIAEVENTOID = @EXIGENCIAEVENTOID ";

                contextQuery.Parameters.Add("@EXIGENCIAEVENTOID", SqlDbType.Int, exigenciaEventoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    arquivo = (byte[])reader["ARQUIVO"];
                }

                return arquivo;
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

        public void Insere(DataContext contexto, Entidades.ExigenciaEventoArquivo exigenciaEventoArquivo)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO PrestacaoContas.EXIGENCIAEVENTOARQUIVO
                                           (EXIGENCIAEVENTOID
                                           ,CHAVEARQUIVO
                                           ,ARQUIVO
                                           ,TIPOARQUIVO
                                           , NOMEARQUIVO                                          
                                           ,USUARIOID
                                           ,DATACADASTRO )
                                     VALUES
	                                       (@EXIGENCIAEVENTOID
                                           ,NEWID()
                                           ,@ARQUIVO
                                           ,@TIPOARQUIVO
                                           ,@NOMEARQUIVO
                                           ,@USUARIOID
                                           ,@DATACADASTRO
										   ) 

                         SELECT IDENT_CURRENT('PrestacaoContas.EXIGENCIAEVENTOARQUIVO') ";

            contextQuery.Parameters.Add("@EXIGENCIAEVENTOID", SqlDbType.Int, exigenciaEventoArquivo.ExigenciaEventoId);
            contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, exigenciaEventoArquivo.Arquivo);
            contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, exigenciaEventoArquivo.TipoArquivo);
            contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, exigenciaEventoArquivo.NomeArquivo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, exigenciaEventoArquivo.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);

            exigenciaEventoArquivo.ExigenciaEventoArquivoId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        public void Atualiza(DataContext contexto, Entidades.ExigenciaEventoArquivo exigenciaEventoArquivo)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE PrestacaoContas.EXIGENCIAEVENTOARQUIVO
                                       SET									    
										ARQUIVO = @ARQUIVO
										,TIPOARQUIVO = @TIPOARQUIVO
										,NOMEARQUIVO = @NOMEARQUIVO 
                                        ,USUARIOID = @USUARIOID
                                        ,DATAALTERACAO = @DATAALTERACAO
                                     WHERE EXIGENCIAEVENTOID = @EXIGENCIAEVENTOID ";

            contextQuery.Parameters.Add("@EXIGENCIAEVENTOID", SqlDbType.Int, exigenciaEventoArquivo.ExigenciaEventoId);
            contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, exigenciaEventoArquivo.Arquivo);
            contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, exigenciaEventoArquivo.TipoArquivo);
            contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, exigenciaEventoArquivo.NomeArquivo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, exigenciaEventoArquivo.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }
        
        public void InsereAuditoria(DataContext contexto, Entidades.ExigenciaEventoArquivo exigenciaEventoArquivo, string operacao, string estacao)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  INSERT INTO Poseidon.PrestacaoContas.EXIGENCIAEVENTOARQUIVO
                                               (EXIGENCIAEVENTOARQUIVOID
                                               ,EXIGENCIAEVENTOID
                                               ,CHAVEARQUIVO
                                               ,ARQUIVO
                                               ,TIPOARQUIVO
                                               ,NOMEARQUIVO
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO
                                               ,DATAAUDITORIA
                                               ,OPERACAO
                                               ,ESTACAO )
                                         VALUES
                                               (@EXIGENCIAEVENTOARQUIVOID, 
                                               @EXIGENCIAEVENTOID,
                                               NEWID(), 
                                               @ARQUIVO,
                                               @TIPOARQUIVO, 
                                               @NOMEARQUIVO, 
                                               @USUARIOID, 
                                               @DATACADASTRO, 
                                               @DATAALTERACAO,
                                               @DATAAUDITORIA,
                                               @OPERACAO,
                                               @ESTACAO) 

                                        SELECT IDENT_CURRENT('PRESTACAOCONTAS.FORNECEDORDOCUMENTO') ";

            contextQuery.Parameters.Add("@EXIGENCIAEVENTOARQUIVOID", SqlDbType.Int, exigenciaEventoArquivo.ExigenciaEventoArquivoId);
            contextQuery.Parameters.Add("@EXIGENCIAEVENTOID", SqlDbType.Int, exigenciaEventoArquivo.ExigenciaEventoId);
            contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, exigenciaEventoArquivo.Arquivo);
            contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, exigenciaEventoArquivo.TipoArquivo);
            contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, exigenciaEventoArquivo.NomeArquivo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, exigenciaEventoArquivo.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAAUDITORIA", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@OPERACAO", SqlDbType.VarChar, operacao);
            contextQuery.Parameters.Add("@ESTACAO", SqlDbType.VarChar, estacao);

            contexto.ApplyModifications(contextQuery);
        }

        public void InsereAuditoriaPorEvento(DataContext contexto, int eventoId, string operacao, string estacao)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO POSEIDON.PRESTACAOCONTAS.EXIGENCIAEVENTOARQUIVO 
                                                    (EXIGENCIAEVENTOARQUIVOID, 
                                                     EXIGENCIAEVENTOID, 
                                                     CHAVEARQUIVO, 
                                                     ARQUIVO, 
                                                     TIPOARQUIVO, 
                                                     NOMEARQUIVO, 
                                                     USUARIOID, 
                                                     DATACADASTRO, 
                                                     DATAALTERACAO, 
                                                     DATAAUDITORIA,
                                                     OPERACAO, 
                                                     ESTACAO) 
                                        SELECT EXIGENCIAEVENTOARQUIVOID, 
                                               EEA.EXIGENCIAEVENTOID, 
                                               NEWID(), 
                                               ARQUIVO, 
                                               TIPOARQUIVO, 
                                               NOMEARQUIVO, 
                                               @USUARIOID, 
                                               EEA.DATACADASTRO, 
                                               EEA.DATAALTERACAO, 
                                               @DATAAUDITORIA,
                                               @OPERACAO, 
                                               @ESTACAO 
                                        FROM   LYCEUM.PRESTACAOCONTAS.EXIGENCIAEVENTOARQUIVO EEA
	                                           INNER JOIN LYCEUM.PRESTACAOCONTAS.EXIGENCIAEVENTO E ON EEA.EXIGENCIAEVENTOID = E.EXIGENCIAEVENTOID
                                        WHERE  E.EVENTOID = @EVENTOID ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);
            contextQuery.Parameters.Add("@DATAAUDITORIA", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@OPERACAO", SqlDbType.VarChar, operacao);
            contextQuery.Parameters.Add("@ESTACAO", SqlDbType.VarChar, estacao);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, HttpContext.Current.User.Identity.Name);

            contexto.ApplyModifications(contextQuery);
        }

        public void RemovePorEvento(DataContext contexto, int eventoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"   DELETE  EEA
                                        FROM   LYCEUM.PRESTACAOCONTAS.EXIGENCIAEVENTOARQUIVO EEA
	                                           INNER JOIN LYCEUM.PRESTACAOCONTAS.EXIGENCIAEVENTO E ON EEA.EXIGENCIAEVENTOID = E.EXIGENCIAEVENTOID
                                        WHERE  E.EVENTOID = @EVENTOID ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);

            contexto.ApplyModifications(contextQuery);
        }
    }
}

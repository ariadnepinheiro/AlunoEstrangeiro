using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using System.Data.SqlClient;
using System.Web;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class EvidenciaArquivo
    {
        public byte[] ObtemArquivoPor(int evidenciaArquivoId)
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
                                            FROM   PrestacaoContas.EVIDENCIAARQUIVO (NOLOCK) 
											WHERE EVIDENCIAARQUIVOID = @EVIDENCIAARQUIVOID ";

                contextQuery.Parameters.Add("@EVIDENCIAARQUIVOID", SqlDbType.Int, evidenciaArquivoId);

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

        public void Insere(DataContext contexto, Entidades.EvidenciaArquivo evidenciaArquivo)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO PrestacaoContas.EVIDENCIAARQUIVO
                                           (EVENTOID
                                           ,CHAVEARQUIVO
                                           ,ARQUIVO
                                           ,TIPOARQUIVO
                                           , NOMEARQUIVO                                          
                                           ,USUARIOID
                                           ,DATACADASTRO )
                                     VALUES
	                                       (@EVENTOID
                                           ,NEWID()
                                           ,@ARQUIVO
                                           ,@TIPOARQUIVO
                                           ,@NOMEARQUIVO
                                           ,@USUARIOID
                                           ,@DATACADASTRO
										   ) 

                         SELECT IDENT_CURRENT('PrestacaoContas.EVIDENCIAARQUIVO') ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, evidenciaArquivo.EventoId);
            contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, evidenciaArquivo.Arquivo);
            contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, evidenciaArquivo.TipoArquivo);
            contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, evidenciaArquivo.NomeArquivo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, evidenciaArquivo.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);

            evidenciaArquivo.EvidenciaArquivoId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        public void InsereAuditoria(DataContext contexto, Entidades.EvidenciaArquivo evidenciaArquivo, string operacao, string estacao)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  INSERT INTO Poseidon.PrestacaoContas.EVIDENCIAARQUIVO
                                               (EVIDENCIAARQUIVOID
                                               ,EVENTOID
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
                                               (@EVIDENCIAARQUIVOID, 
                                               @EVENTOID,
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

                                        SELECT IDENT_CURRENT('PRESTACAOCONTAS.EVIDENCIAARQUIVO') ";

            contextQuery.Parameters.Add("@EVIDENCIAARQUIVOID", SqlDbType.Int, evidenciaArquivo.EvidenciaArquivoId);
            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, evidenciaArquivo.EventoId);
            contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, evidenciaArquivo.Arquivo);
            contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, evidenciaArquivo.TipoArquivo);
            contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, evidenciaArquivo.NomeArquivo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, evidenciaArquivo.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAAUDITORIA", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@OPERACAO", SqlDbType.VarChar, operacao);
            contextQuery.Parameters.Add("@ESTACAO", SqlDbType.VarChar, estacao);

            contexto.ApplyModifications(contextQuery);
        }

        public void InsereAuditoria(DataContext contexto, int eventoId, string operacao, string estacao)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO POSEIDON.PRESTACAOCONTAS.EVIDENCIAARQUIVO 
                                                    (EVIDENCIAARQUIVOID, 
                                                     EVENTOID, 
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
                                        SELECT EVIDENCIAARQUIVOID, 
                                               EVENTOID, 
                                               NEWID(), 
                                               ARQUIVO, 
                                               TIPOARQUIVO, 
                                               NOMEARQUIVO, 
                                               @USUARIOID, 
                                               DATACADASTRO, 
                                               DATAALTERACAO, 
                                               @DATAAUDITORIA,
                                               @OPERACAO, 
                                               @ESTACAO 
                                        FROM   LYCEUM.PRESTACAOCONTAS.EVIDENCIAARQUIVO 
                                        WHERE  EVENTOID = @EVENTOID  ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);
            contextQuery.Parameters.Add("@DATAAUDITORIA", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@OPERACAO", SqlDbType.VarChar, operacao);
            contextQuery.Parameters.Add("@ESTACAO", SqlDbType.VarChar, estacao);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, HttpContext.Current.User.Identity.Name);

            contexto.ApplyModifications(contextQuery);
        }

        public void Remove(DataContext contexto, int eventoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE PRESTACAOCONTAS.EVIDENCIAARQUIVO                                     
                                        WHERE  EVENTOID = @EVENTOID  ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);

            contexto.ApplyModifications(contextQuery);
        }
    }
}

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
    public class OrcamentoArquivo
    {
        public DataTable ListaOrcamentoPor(DataContext contexto, int eventoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            contextQuery.Command = @" SELECT  ORCAMENTOARQUIVOID, 
		                                        EVENTOID, 
		                                        CHAVEARQUIVO, 
		                                        ARQUIVO, 
		                                        TIPOARQUIVO, 
		                                        NOMEARQUIVO
                                        from PrestacaoContas.ORCAMENTOARQUIVO
                                        WHERE EVENTOID = @CENSO ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.Int, eventoId);

            dt = contexto.GetDataTable(contextQuery);

            return dt;
        }

        public byte[] ObtemArquivoPor(int orcamentoArquivoId)
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
                                            FROM   PrestacaoContas.ORCAMENTOARQUIVO (NOLOCK) 
											WHERE ORCAMENTOARQUIVOID = @ORCAMENTOARQUIVOID ";

                contextQuery.Parameters.Add("@ORCAMENTOARQUIVOID", SqlDbType.Int, orcamentoArquivoId);

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

        public void Insere(DataContext contexto, Entidades.OrcamentoArquivo orcamentoArquivo)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO PrestacaoContas.ORCAMENTOARQUIVO
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

                         SELECT IDENT_CURRENT('PrestacaoContas.ORCAMENTOARQUIVO') ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, orcamentoArquivo.EventoId);
            contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, orcamentoArquivo.Arquivo);
            contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, orcamentoArquivo.TipoArquivo);
            contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, orcamentoArquivo.NomeArquivo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, orcamentoArquivo.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);

            orcamentoArquivo.OrcamentoArquivoId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        public void InsereAuditoria(DataContext contexto, Entidades.OrcamentoArquivo orcamentoArquivo, string operacao, string estacao)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  INSERT INTO Poseidon.PrestacaoContas.ORCAMENTOARQUIVO
                                               (ORCAMENTOARQUIVOID
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
                                               (@ORCAMENTOARQUIVOID, 
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

                                        SELECT IDENT_CURRENT('PRESTACAOCONTAS.ORCAMENTOARQUIVO') ";

            contextQuery.Parameters.Add("@ORCAMENTOARQUIVOID", SqlDbType.Int, orcamentoArquivo.OrcamentoArquivoId);
            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, orcamentoArquivo.EventoId);
            contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, orcamentoArquivo.Arquivo);
            contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, orcamentoArquivo.TipoArquivo);
            contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, orcamentoArquivo.NomeArquivo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, orcamentoArquivo.UsuarioId);
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

            contextQuery.Command = @" INSERT INTO POSEIDON.PRESTACAOCONTAS.ORCAMENTOARQUIVO 
                                                    (ORCAMENTOARQUIVOID, 
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
                                        SELECT ORCAMENTOARQUIVOID, 
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
                                        FROM   LYCEUM.PRESTACAOCONTAS.ORCAMENTOARQUIVO 
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

            contextQuery.Command = @" DELETE PRESTACAOCONTAS.ORCAMENTOARQUIVO                                     
                                        WHERE  EVENTOID = @EVENTOID  ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);

            contexto.ApplyModifications(contextQuery);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class AplicacaoFinanceiraComprovanteArquivo
    {
        public void Atualiza(DataContext contexto, Entidades.AplicacaoFinanceiraComprovanteArquivo aplicacaoFinanceiraComprovanteArquivo)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE [PrestacaoContas].[APLICACAOFINANCEIRACOMPROVANTEARQUIVO]
                                       SET									    
										   ARQUIVO = @ARQUIVO,
                                           TIPOARQUIVO = @TIPOARQUIVO,
                                           NOMEARQUIVO = @NOMEARQUIVO,
                                           USUARIOID = @USUARIOID,
                                           DATAALTERACAO = @DATAALTERACAO 
                                     WHERE APLICACAOFINANCEIRACOMPROVANTEARQUIVOID = @APLICACAOFINANCEIRACOMPROVANTEARQUIVOID ";

            contextQuery.Parameters.Add("@APLICACAOFINANCEIRACOMPROVANTEARQUIVOID", SqlDbType.Int, aplicacaoFinanceiraComprovanteArquivo.AplicacaoFinanceiraComprovanteArquivoId);
            contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, aplicacaoFinanceiraComprovanteArquivo.Arquivo);
            contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, aplicacaoFinanceiraComprovanteArquivo.TipoArquivo);
            contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, aplicacaoFinanceiraComprovanteArquivo.NomeArquivo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, aplicacaoFinanceiraComprovanteArquivo.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void Insere(DataContext contexto, Entidades.AplicacaoFinanceiraComprovanteArquivo aplicacaoFinanceiraComprovanteArquivo)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO [PrestacaoContas].[APLICACAOFINANCEIRACOMPROVANTEARQUIVO]
                                           ([APLICACAOFINANCEIRAID]
                                           ,[CHAVEARQUIVO]
                                           ,[ARQUIVO]
                                           ,[TIPOARQUIVO]
                                           ,[NOMEARQUIVO]
                                           ,[USUARIOID]
                                           ,[DATACADASTRO]
                                           ,[DATAALTERACAO])
                                     VALUES
                                           (@APLICACAOFINANCEIRAID 
                                           ,@CHAVEARQUIVO
                                           ,@ARQUIVO
                                           ,@TIPOARQUIVO
                                           ,@NOMEARQUIVO
                                           ,@USUARIOID
                                           ,@DATACADASTRO
                                           ,@DATAALTERACAO    )

                         SELECT IDENT_CURRENT('PrestacaoContas.APLICACAOFINANCEIRACOMPROVANTEARQUIVO') ";

            contextQuery.Parameters.Add("@APLICACAOFINANCEIRAID", SqlDbType.Int, aplicacaoFinanceiraComprovanteArquivo.AplicacaoFinanceiraId);
            contextQuery.Parameters.Add("@CHAVEARQUIVO", SqlDbType.VarChar, aplicacaoFinanceiraComprovanteArquivo.ChaveArquivo);
            contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, aplicacaoFinanceiraComprovanteArquivo.Arquivo);
            contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, aplicacaoFinanceiraComprovanteArquivo.TipoArquivo);
            contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, aplicacaoFinanceiraComprovanteArquivo.NomeArquivo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, aplicacaoFinanceiraComprovanteArquivo.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            aplicacaoFinanceiraComprovanteArquivo.AplicacaoFinanceiraComprovanteArquivoId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        public void InsereAuditoria(DataContext contexto, Entidades.AplicacaoFinanceiraComprovanteArquivo aplicacaoFinanceiraComprovanteArquivo, string operacao, string estacao)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  INSERT INTO Poseidon.PrestacaoContas.APLICACAOFINANCEIRACOMPROVANTEARQUIVO
                                               (APLICACAOFINANCEIRACOMPROVANTEARQUIVOID
                                               ,APLICACAOFINANCEIRAID
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
                                               (@APLICACAOFINANCEIRACOMPROVANTEARQUIVOID, 
                                               @APLICACAOFINANCEIRAID,
                                               NEWID(), 
                                               @ARQUIVO,
                                               @TIPOARQUIVO, 
                                               @NOMEARQUIVO, 
                                               @USUARIOID, 
                                               @DATACADASTRO, 
                                               @DATAALTERACAO,
                                               @DATAAUDITORIA,
                                               @OPERACAO,
                                               @ESTACAO) ";

            contextQuery.Parameters.Add("@APLICACAOFINANCEIRACOMPROVANTEARQUIVOID", SqlDbType.Int, aplicacaoFinanceiraComprovanteArquivo.AplicacaoFinanceiraComprovanteArquivoId);
            contextQuery.Parameters.Add("@APLICACAOFINANCEIRAID", SqlDbType.Int, aplicacaoFinanceiraComprovanteArquivo.AplicacaoFinanceiraId);
            contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, aplicacaoFinanceiraComprovanteArquivo.Arquivo);
            contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, aplicacaoFinanceiraComprovanteArquivo.TipoArquivo);
            contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, aplicacaoFinanceiraComprovanteArquivo.NomeArquivo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, aplicacaoFinanceiraComprovanteArquivo.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAAUDITORIA", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@OPERACAO", SqlDbType.VarChar, operacao);
            contextQuery.Parameters.Add("@ESTACAO", SqlDbType.VarChar, estacao);

            contexto.ApplyModifications(contextQuery);
        }

        public void InsereAuditoria(DataContext contexto, int aplicacaoFinanceiraId, string operacao, string estacao, string usuario)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO POSEIDON.PRESTACAOCONTAS.APLICACAOFINANCEIRACOMPROVANTEARQUIVO 
                                                    (APLICACAOFINANCEIRACOMPROVANTEARQUIVOID, 
                                                     APLICACAOFINANCEIRAID, 
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
                                        SELECT APLICACAOFINANCEIRACOMPROVANTEARQUIVOID, 
                                               APLICACAOFINANCEIRAID, 
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
                                        FROM   LYCEUM.PRESTACAOCONTAS.APLICACAOFINANCEIRACOMPROVANTEARQUIVO 
                                        WHERE  APLICACAOFINANCEIRAID = @APLICACAOFINANCEIRAID  ";

            contextQuery.Parameters.Add("@APLICACAOFINANCEIRAID", SqlDbType.Int, aplicacaoFinanceiraId);
            contextQuery.Parameters.Add("@DATAAUDITORIA", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuario);
            contextQuery.Parameters.Add("@OPERACAO", SqlDbType.VarChar, operacao);
            contextQuery.Parameters.Add("@ESTACAO", SqlDbType.VarChar, estacao);

            contexto.ApplyModifications(contextQuery);
        }

        public void Remove(DataContext contexto, int aplicacaoFinanceiraId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE PRESTACAOCONTAS.APLICACAOFINANCEIRACOMPROVANTEARQUIVO                                     
                                        WHERE  APLICACAOFINANCEIRAID = @APLICACAOFINANCEIRAID  ";

            contextQuery.Parameters.Add("@APLICACAOFINANCEIRAID", SqlDbType.Int, aplicacaoFinanceiraId);

            contexto.ApplyModifications(contextQuery);
        }

        public byte[] ObtemArquivoPor(int aplicacaoFinanceiraId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            byte[] arquivo = null;

            try
            {
                contextQuery.Command = @" 	 SELECT ARQUIVO, 
	                                               TIPOARQUIVO, 
                                                   NOMEARQUIVO 
                                            FROM   PrestacaoContas.APLICACAOFINANCEIRACOMPROVANTEARQUIVO (NOLOCK) 
											where APLICACAOFINANCEIRAID = @APLICACAOFINANCEIRAID ";

                contextQuery.Parameters.Add("@APLICACAOFINANCEIRAID", SqlDbType.Int, aplicacaoFinanceiraId);

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
    }
}

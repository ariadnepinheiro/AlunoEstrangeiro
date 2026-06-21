using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.Ocorrencias
{
    public class OcorrenciaEncaminhamentoArquivo
    {
        public byte[] ObtemArquivoPor(int ocorrenciaEncaminhamentoArquivoId)
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
                                            FROM   Ocorrencias.OCORRENCIAENCAMINHAMENTOARQUIVO (NOLOCK) 
											WHERE OCORRENCIAENCAMINHAMENTOARQUIVOID = @OCORRENCIAENCAMINHAMENTOARQUIVOID ";

                contextQuery.Parameters.Add("@OCORRENCIAENCAMINHAMENTOARQUIVOID", SqlDbType.Int, ocorrenciaEncaminhamentoArquivoId);

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

        public void Insere(DataContext contexto, Entidades.OcorrenciaEncaminhamentoAquivo ocorrenciaEncaminhamentoArquivo)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO [Ocorrencias].[OCORRENCIAENCAMINHAMENTOARQUIVO]
                                           ([OCORRENCIAENCAMINHAMENTOID]
                                           ,[CHAVEARQUIVO]
                                           ,[ARQUIVO]
                                           ,[TIPOARQUIVO]
                                           ,[NOMEARQUIVO]
                                           ,[USUARIOID]
                                           ,[DATACADASTRO]
                                           ,[DATAALTERACAO])
                                     VALUES
                                           (@OCORRENCIAENCAMINHAMENTOID 
                                           ,NEWID()
                                           ,@ARQUIVO
                                           ,@TIPOARQUIVO
                                           ,@NOMEARQUIVO
                                           ,@USUARIOID
                                           ,@DATACADASTRO
                                           ,@DATAALTERACAO    )

                         SELECT IDENT_CURRENT('Ocorrencias.OCORRENCIAENCAMINHAMENTOARQUIVO') ";

            contextQuery.Parameters.Add("@OCORRENCIAENCAMINHAMENTOID", SqlDbType.Int, ocorrenciaEncaminhamentoArquivo.OcorrenciaEncaminhamentoId);
            contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, ocorrenciaEncaminhamentoArquivo.Arquivo);
            contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, ocorrenciaEncaminhamentoArquivo.TipoArquivo);
            contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, ocorrenciaEncaminhamentoArquivo.NomeArquivo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, ocorrenciaEncaminhamentoArquivo.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            ocorrenciaEncaminhamentoArquivo.OcorrenciaEncaminhamentoAquivoId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        public void InsereAuditoria(DataContext contexto, Entidades.OcorrenciaEncaminhamentoAquivo ocorrenciaEncaminhamentoArquivo, string operacao, string estacao)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  INSERT INTO Poseidon.Ocorrencias.OCORRENCIAENCAMINHAMENTOARQUIVO
                                               (OCORRENCIAENCAMINHAMENTOARQUIVOID
                                               ,OCORRENCIAENCAMINHAMENTOID
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
                                               (@OCORRENCIAENCAMINHAMENTOARQUIVOID, 
                                               @OCORRENCIAENCAMINHAMENTOID,
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

            contextQuery.Parameters.Add("@OCORRENCIAENCAMINHAMENTOARQUIVOID", SqlDbType.Int, ocorrenciaEncaminhamentoArquivo.OcorrenciaEncaminhamentoAquivoId);
            contextQuery.Parameters.Add("@OCORRENCIAENCAMINHAMENTOID", SqlDbType.Int, ocorrenciaEncaminhamentoArquivo.OcorrenciaEncaminhamentoId);
            contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, ocorrenciaEncaminhamentoArquivo.Arquivo);
            contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, ocorrenciaEncaminhamentoArquivo.TipoArquivo);
            contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, ocorrenciaEncaminhamentoArquivo.NomeArquivo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, ocorrenciaEncaminhamentoArquivo.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAAUDITORIA", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@OPERACAO", SqlDbType.VarChar, operacao);
            contextQuery.Parameters.Add("@ESTACAO", SqlDbType.VarChar, estacao);

            contexto.ApplyModifications(contextQuery);
        }

        public void InsereAuditoria(DataContext contexto, int ocorrenciaEncaminhamentoId, string operacao, string estacao, string usuario)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO POSEIDON.OCORRENCIAS.OCORRENCIAENCAMINHAMENTOARQUIVO 
                                                    (OCORRENCIAENCAMINHAMENTOARQUIVOID, 
                                                     OCORRENCIAENCAMINHAMENTOID, 
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
                                        SELECT OCORRENCIAENCAMINHAMENTOARQUIVOID, 
                                               OCORRENCIAENCAMINHAMENTOID, 
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
                                        FROM   LYCEUM.OCORRENCIAS.OCORRENCIAENCAMINHAMENTOARQUIVO 
                                        WHERE  OCORRENCIAENCAMINHAMENTOID = @OCORRENCIAENCAMINHAMENTOID  ";

            contextQuery.Parameters.Add("@OCORRENCIAENCAMINHAMENTOID", SqlDbType.Int, ocorrenciaEncaminhamentoId);
            contextQuery.Parameters.Add("@DATAAUDITORIA", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuario);
            contextQuery.Parameters.Add("@OPERACAO", SqlDbType.VarChar, operacao);
            contextQuery.Parameters.Add("@ESTACAO", SqlDbType.VarChar, estacao);

            contexto.ApplyModifications(contextQuery);
        }

        public void Remove(DataContext contexto, int ocorrenciaEncaminhamentoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE OCORRENCIAS.OCORRENCIAENCAMINHAMENTOARQUIVO                                     
                                        WHERE  OCORRENCIAENCAMINHAMENTOID = @OCORRENCIAENCAMINHAMENTOID  ";

            contextQuery.Parameters.Add("@OCORRENCIAENCAMINHAMENTOID", SqlDbType.Int, ocorrenciaEncaminhamentoId);

            contexto.ApplyModifications(contextQuery);
        }
    }
}

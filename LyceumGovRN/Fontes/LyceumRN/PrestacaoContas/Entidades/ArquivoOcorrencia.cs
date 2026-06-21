using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using System.Data.SqlClient;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Ocorrencias
{
    public class ArquivoOcorrencia
    {
        public DataTable ListaPor(int ocorrenciaId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT ARQUIVOOCORRENCIAID,
	                                            OCORRENCIAID,
	                                            NOMEARQUIVO,
	                                            TIPOARQUIVO,
	                                            CASE
		                                            WHEN TIPOARQUIVO = 'application/pdf' then 'PDF'
		                                            WHEN TIPOARQUIVO = 'image/jpeg' then 'IMAGEM'		
		                                            ELSE  ''
	                                            END TIPO,
	                                            DATACADASTRO
                                            FROM Ocorrencias.ARQUIVOOCORRENCIA
                                            WHERE OCORRENCIAID = @OCORRENCIAID ";

                contextQuery.Parameters.Add("@OCORRENCIAID", SqlDbType.Int, ocorrenciaId);

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

        public ValidacaoDados Valida(Entidades.ArquivoOcorrencia arquivoOcorrencia)
        {
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (arquivoOcorrencia == null)
            {
                return validacaoDados;
            }

            if (arquivoOcorrencia.OcorrenciaId <= 0)
            {
                mensagens.Add("Campo CÓDIGO é obrigatório.");
            }

            if (arquivoOcorrencia.Arquivo == null || arquivoOcorrencia.Arquivo.Count() <= 0)
            {
                mensagens.Add("Campo ARQUIVO é obrigatório.");
            }
            else
            {
                if (arquivoOcorrencia.TipoArquivo.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo TIPO ARQUIVO é obrigatório.");
                }
                else
                {
                    //Apenas aceitar pdf e imagem 
                    if (arquivoOcorrencia.TipoArquivo.ToUpper() != "IMAGE/JPEG"
                        && arquivoOcorrencia.TipoArquivo.ToUpper() != "APPLICATION/PDF")
                    {
                        mensagens.Add("Apenas serão aceitos arquivos dos tipos .jpeg e .pdf .");
                    }
                }

                //Verifica tamanho do arquivo - documentos com até 1 MB
                int tamanhoByte = Buffer.ByteLength(arquivoOcorrencia.Arquivo);
                if (tamanhoByte > 1048576) //1MB
                {
                    mensagens.Add("Os arquivos devem ter tamanho com até 1 MB.");
                }

                if (arquivoOcorrencia.NomeArquivo.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo NOME ARQUIVO é obrigatório.");
                }
                else if (arquivoOcorrencia.NomeArquivo.Length > 500)
                {
                    mensagens.Add("Campo NOME ARQUIVO deve conter no máximo por 500 caracteres.");
                }
            }

            if (arquivoOcorrencia.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatório.");
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

        public void Insere(Entidades.ArquivoOcorrencia arquivoOcorrencia)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Insere arquivo
                this.Insere(ctx, arquivoOcorrencia);

                //Insere auditoria arquivo
                this.InsereAuditoria(ctx, arquivoOcorrencia, "CADASTRADO", System.Web.HttpContext.Current.Request.UserHostName);
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

        private void Insere(DataContext contexto, Entidades.ArquivoOcorrencia arquivoOcorrencia)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO [Ocorrencias].[ARQUIVOOCORRENCIA]
                                           ([OCORRENCIAID]
                                           ,[CHAVEARQUIVO]
                                           ,[ARQUIVO]
                                           ,[TIPOARQUIVO]
                                           ,[NOMEARQUIVO]
                                           ,[USUARIOID]
                                           ,[DATACADASTRO]
                                           ,[DATAALTERACAO])
                                     VALUES
                                           (@OCORRENCIAID 
                                           ,NEWID()
                                           ,@ARQUIVO
                                           ,@TIPOARQUIVO
                                           ,@NOMEARQUIVO
                                           ,@USUARIOID
                                           ,@DATACADASTRO
                                           ,@DATAALTERACAO    )

                         SELECT IDENT_CURRENT('Ocorrencias.ARQUIVOOCORRENCIA') ";

            contextQuery.Parameters.Add("@OCORRENCIAID", SqlDbType.Int, arquivoOcorrencia.OcorrenciaId);
            contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, arquivoOcorrencia.Arquivo);
            contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, arquivoOcorrencia.TipoArquivo);
            contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, arquivoOcorrencia.NomeArquivo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, arquivoOcorrencia.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            arquivoOcorrencia.ArquivoOcorrenciaId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        private void InsereAuditoria(DataContext contexto, Entidades.ArquivoOcorrencia arquivoOcorrencia, string operacao, string estacao)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  INSERT INTO Poseidon.Ocorrencias.ARQUIVOOCORRENCIA
                                               (ARQUIVOOCORRENCIAID
                                               ,OCORRENCIAID
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
                                               (@ARQUIVOOCORRENCIAID, 
                                               @OCORRENCIAID,
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

            contextQuery.Parameters.Add("@ARQUIVOOCORRENCIAID", SqlDbType.Int, arquivoOcorrencia.ArquivoOcorrenciaId);
            contextQuery.Parameters.Add("@OCORRENCIAID", SqlDbType.Int, arquivoOcorrencia.OcorrenciaId);
            contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, arquivoOcorrencia.Arquivo);
            contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, arquivoOcorrencia.TipoArquivo);
            contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, arquivoOcorrencia.NomeArquivo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, arquivoOcorrencia.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAAUDITORIA", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@OPERACAO", SqlDbType.VarChar, operacao);
            contextQuery.Parameters.Add("@ESTACAO", SqlDbType.VarChar, estacao);

            contexto.ApplyModifications(contextQuery);
        }

        private void InsereAuditoria(DataContext contexto, int arquivoOcorrenciaId, string operacao, string estacao, string usuario)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO POSEIDON.OCORRENCIAS.ARQUIVOOCORRENCIA 
                                                    (ARQUIVOOCORRENCIAID, 
                                                     OCORRENCIAID, 
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
                                        SELECT ARQUIVOOCORRENCIAID, 
                                               OCORRENCIAID, 
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
                                        FROM   LYCEUM.OCORRENCIAS.ARQUIVOOCORRENCIA 
                                        WHERE  ARQUIVOOCORRENCIAID = @ARQUIVOOCORRENCIAID  ";

            contextQuery.Parameters.Add("@ARQUIVOOCORRENCIAID", SqlDbType.Int, arquivoOcorrenciaId);
            contextQuery.Parameters.Add("@DATAAUDITORIA", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuario);
            contextQuery.Parameters.Add("@OPERACAO", SqlDbType.VarChar, operacao);
            contextQuery.Parameters.Add("@ESTACAO", SqlDbType.VarChar, estacao);

            contexto.ApplyModifications(contextQuery);
        }

        public ValidacaoDados ValidaRemocao(int arquivoId, string usuarioId)
        {
            List<string> mensagens = new List<string>();
            OcorrenciaEncaminhamento rnOcorrenciaEncaminhamento = new OcorrenciaEncaminhamento();
            Perfil rnPerfil = new Perfil();
            RN.Usuarios rnUsuarios = new Usuarios();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (arquivoId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if(usuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    int ocorrenciaId = this.ObtemOcorrenciaIdPor(contexto, arquivoId);

                    //Verificar se a ocorrencia possui encaminhamento, caso seja alteracao
                    if (rnOcorrenciaEncaminhamento.PossuiEncaminhamentoPor(contexto, ocorrenciaId))
                    {
                        //Caso exita encaminhamento verifica se o usuario tem perfil de adm                        
                        if (!rnUsuarios.EhPrivilegiado(contexto, usuarioId) && !rnPerfil.PossuiPerfilAdministradorRVEPor(contexto, usuarioId))
                        {
                            //verifica se o usuario tem perfil de adm
                            mensagens.Add("Este registro não pode ser alterado pois já existem encaminhamentos cadastrados, com isso apenas usuários com Perfil ADMINISTRADOR RVE podem alterar.");
                        }
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

        public void Remove(int arquivoId, string usuarioId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Insere auditoria arquivo
                this.InsereAuditoria(ctx, arquivoId, "REMOVIDO", System.Web.HttpContext.Current.Request.UserHostName, usuarioId);

                //Remove arquivo
                this.Remove(ctx, arquivoId);
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

        private void Remove(DataContext contexto, int arquivoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE OCORRENCIAS.ARQUIVOOCORRENCIA                                     
                                        WHERE  ARQUIVOOCORRENCIAID = @ARQUIVOOCORRENCIAID  ";

            contextQuery.Parameters.Add("@ARQUIVOOCORRENCIAID", SqlDbType.Int, arquivoId);

            contexto.ApplyModifications(contextQuery);
        }

        public byte[] ObtemArquivoPor(int arquivoId)
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
                                            FROM   Ocorrencias.ARQUIVOOCORRENCIA (NOLOCK) 
											where ARQUIVOOCORRENCIAID = @ARQUIVOOCORRENCIAID ";

                contextQuery.Parameters.Add("@ARQUIVOOCORRENCIAID", SqlDbType.Int, arquivoId);

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

        public int ObtemOcorrenciaIdPor(DataContext contexto, int arquivoId)
        {
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int id = 0;

            try
            {
                contextQuery.Command = @" 	 SELECT OCORRENCIAID 
                                            FROM   Ocorrencias.ARQUIVOOCORRENCIA (NOLOCK) 
											where ARQUIVOOCORRENCIAID = @ARQUIVOOCORRENCIAID ";

                contextQuery.Parameters.Add("@ARQUIVOOCORRENCIAID", SqlDbType.Int, arquivoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    id = Convert.ToInt32(reader["OCORRENCIAID"]);
                }

                return id;
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
    }
}

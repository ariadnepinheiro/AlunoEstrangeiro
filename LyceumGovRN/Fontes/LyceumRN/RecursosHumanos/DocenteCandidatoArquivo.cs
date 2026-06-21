using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Data;
using System.Data.SqlClient;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.RecursosHumanos
{
    public class DocenteCandidatoArquivo : RNBase
    {
        public ICollection<Entidades.DocenteCandidatoArquivo> ObtemListaPor(int docenteCandidatoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ICollection<Entidades.DocenteCandidatoArquivo> matGrades = new List<Entidades.DocenteCandidatoArquivo>();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT  DOCENTECANDIDATOID, 
		                                    NUM_FUNC, 
		                                    CONCURSO, 
		                                    QTDEANOSGLP, 
		                                    ACUMULACAO, 
		                                    UTILIZARUBRICA, 
		                                    ID_REGIONAL, 
		                                    MUNICIPIO, 
		                                    SEDE, 
		                                    DISCIPLINAINGRESSO, 
		                                    USUARIOID, 
		                                    DATACADASTRO, 
		                                    DATAALTERACAO
                                    from RecursosHumanos.DOCENTECANDIDATOARQUIVO
                                    WHERE DOCENTECANDIDATOID = @DOCENTECANDIDATOID ";

                contextQuery.Parameters.Add("@DOCENTECANDIDATOID", SqlDbType.Int, docenteCandidatoId);

                matGrades = contexto.TryToBindEntities<Entidades.DocenteCandidatoArquivo>(contextQuery);

                return matGrades;
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

        public bool PossuiTipoDocumentoPor(DataContext contexto, int tipoDocumentoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                    FROM RecursosHumanos.DOCENTECANDIDATOARQUIVO
                                    WHERE TIPODOCUMENTOID = @TIPODOCUMENTOID ";

            contextQuery.Parameters.Add("@TIPODOCUMENTOID", SqlDbType.Int, tipoDocumentoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public static QueryTable ListaDocumento(DbObject concurso, DbObject candidato)
        {
            string sql = @" SELECT  D.[DOCENTECANDIDATOARQUIVOID], D.NOMEARQUIVO,
	                                         CASE WHEN d.TIPOARQUIVO = 'application/pdf' then 'PDF'
		                                          WHEN d.TIPOARQUIVO = 'image/jpeg' then 'IMAGEM'		
		                                     ELSE  ''
	                                         END TIPO,
                                             T.DESCRICAO,
	                                         d.DATACADASTRO,
	                                         d.TIPOARQUIVO
                                          FROM [LYCEUM].[RecursosHumanos].[DOCENTECANDIDATOARQUIVO] D
                                          inner join [RecursosHumanos].[TIPODOCUMENTOCONCURSO] TC on D.TIPODOCUMENTOID = tc.TIPODOCUMENTOID
                                          INNER JOIN RecursosHumanos.TIPODOCUMENTO T ON TC.TIPODOCUMENTOID = T.TIPODOCUMENTOID
                                          where DOCENTECANDIDATOID = ? and
                                           tc.CONCURSO = ? AND 
                                            T.ATIVO = 1 AND 
	                                        TC.ANEXO = 1 ";

            return Consultar(sql, candidato, concurso);
        }

        public DataTable ListaDocumentoPor(string concurso, string idVinculo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" DECLARE @DOCENTECANDIDATOID INT

                                        SELECT DISTINCT @DOCENTECANDIDATOID = D.DOCENTECANDIDATOID
                                        FROM VW_FUNCIONARIOS F
	                                        INNER JOIN RecursosHumanos.DOCENTECANDIDATO D ON F.NUM_FUNC = D.NUM_FUNC
                                        WHERE IDVINCULO = @IDVINCULO
											and CONCURSO = @CONCURSO

                                        SELECT  T.TIPODOCUMENTOID, 
											 T.DESCRICAO,										
											 D.[DOCENTECANDIDATOARQUIVOID], 
											 D.NOMEARQUIVO,
	                                         CASE WHEN d.TIPOARQUIVO = 'application/pdf' then 'PDF'
		                                          WHEN d.TIPOARQUIVO = 'image/jpeg' then 'IMAGEM'		
		                                     ELSE  ''
	                                         END TIPO,                                             
	                                         d.DATACADASTRO,
	                                         d.TIPOARQUIVO,
											 case
												when [DOCENTECANDIDATOARQUIVOID] is not null then 1
												else 0
										     end POSSUIARQUIVO
                                          FROM  RecursosHumanos.TIPODOCUMENTO T
											  INNER JOIN [RecursosHumanos].[TIPODOCUMENTOCONCURSO] TC ON TC.TIPODOCUMENTOID = T.TIPODOCUMENTOID
											  LEFT JOIN  [LYCEUM].[RecursosHumanos].[DOCENTECANDIDATOARQUIVO] D on D.TIPODOCUMENTOID = tc.TIPODOCUMENTOID
																and DOCENTECANDIDATOID = @DOCENTECANDIDATOID
                                          where tc.CONCURSO = @CONCURSO 
											  AND T.ATIVO = 1 
											  AND TC.ANEXO = 1 ";

                contextQuery.Parameters.Add("@IDVINCULO", SqlDbType.VarChar, idVinculo);
                contextQuery.Parameters.Add("@CONCURSO", SqlDbType.VarChar, concurso);

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

        public List<int> ObtemArquivoPorDocente(string candidato, string concurso)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            var arquivo = new List<int>();

            try
            {
                contextQuery.Command = @" SELECT DOCENTECANDIDATOARQUIVOID 
                                            FROM   RecursosHumanos.DOCENTECANDIDATOARQUIVO D(NOLOCK) 
                                            inner join [RecursosHumanos].[TIPODOCUMENTOCONCURSO] TC on D.TIPODOCUMENTOID = tc.TIPODOCUMENTOID
                                            INNER JOIN RecursosHumanos.TIPODOCUMENTO T ON TC.TIPODOCUMENTOID = T.TIPODOCUMENTOID
											WHERE T.ATIVO = 1 AND 
	                                                TC.ANEXO = 1 AND 
                                                    D.DOCENTECANDIDATOID = @DOCENTECANDIDATOID and 
                                                    tc.CONCURSO = @CONCURSO";

                contextQuery.Parameters.Add("@DOCENTECANDIDATOID", SqlDbType.Int, candidato);
                contextQuery.Parameters.Add("@CONCURSO", SqlDbType.VarChar, concurso);


                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    arquivo.Add(Convert.ToInt32(reader["DOCENTECANDIDATOARQUIVOID"]));
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

        public RN.RecursosHumanos.Entidades.DocenteCandidatoArquivo ObtemArquivoPor(int ArquivoId)
        {
            var ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                return ObtemArquivoPor(ctx, ArquivoId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public RN.RecursosHumanos.Entidades.DocenteCandidatoArquivo ObtemArquivoPor(DataContext ctx, int ArquivoId)
        {
            RN.RecursosHumanos.Entidades.DocenteCandidatoArquivo dados = new RN.RecursosHumanos.Entidades.DocenteCandidatoArquivo();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            DataTable arquivo = new DataTable();

            try
            {
                contextQuery.Command = @" SELECT ARQUIVO, 
	                                               TIPOARQUIVO, 
                                                   NOMEARQUIVO 
                                            FROM   RecursosHumanos.DOCENTECANDIDATOARQUIVO (NOLOCK) 
											WHERE DOCENTECANDIDATOARQUIVOID = @DOCENTECANDIDATOARQUIVOID ";

                contextQuery.Parameters.Add("@DOCENTECANDIDATOARQUIVOID", SqlDbType.Int, ArquivoId);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    RN.RecursosHumanos.Entidades.DocenteCandidatoArquivo item = new RN.RecursosHumanos.Entidades.DocenteCandidatoArquivo();
                    item.Arquivo = (byte[])reader["ARQUIVO"];
                    item.TipoArquivo = Convert.ToString(reader["TIPOARQUIVO"]);
                    item.NomeArquivo = Convert.ToString(reader["NOMEARQUIVO"]);
                    return item;
                }
                return null;

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
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();
            }
        }

        public byte[] ObtemDocumentoPor(int docenteCandidatoArquivoId)
        {
            DataContext contexto = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" SELECT ARQUIVO, 
	                                               TIPOARQUIVO, 
                                                   NOMEARQUIVO 
                                            FROM   RecursosHumanos.DOCENTECANDIDATOARQUIVO (NOLOCK) 
                                            WHERE DOCENTECANDIDATOARQUIVOID = @DOCENTECANDIDATOARQUIVOID ";

                contextQuery.Parameters.Add("@DOCENTECANDIDATOARQUIVOID", SqlDbType.Int, docenteCandidatoArquivoId);

                using (var reader = contexto.GetDataReader(contextQuery))
                    while (reader.Read())
                        return (byte[])reader["ARQUIVO"];

                return null;
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
        }

        public ValidacaoDados Valida(Entidades.DocenteCandidatoArquivo docenteCandidatoArquivo, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            DocenteCandidato rnDocenteCandidato = new DocenteCandidato();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (!cadastro)
            {
                if (docenteCandidatoArquivo.DocenteCandidatoArquivoId <= 0)
                {
                    mensagens.Add("Campo DOCENTE é obrigatório.");
                }
            }

            if (docenteCandidatoArquivo.DocenteCandidatoId <= 0)
            {
                mensagens.Add("Campo DOCENTE é obrigatório.");
            }

            if (docenteCandidatoArquivo.TipoDocumentoId <= 0)
            {
                mensagens.Add("Campo TIPO DOCUMENTO é obrigatório.");
            }

            if (docenteCandidatoArquivo.Arquivo == null || docenteCandidatoArquivo.Arquivo.Count() <= 0)
            {
                mensagens.Add("Campo ARQUIVO é obrigatório.");
            }

            //Verifica tamanho do arquivo - documentos com até 5 MB
            int tamanhoByte = Buffer.ByteLength(docenteCandidatoArquivo.Arquivo);
            if (tamanhoByte > 5242880) //5MB
            {
                mensagens.Add("Os arquivos devem ter tamanho com até 5MB.");
            }

            if (docenteCandidatoArquivo.NomeArquivo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NOME ARQUIVO é obrigatório.");
            }

            if (docenteCandidatoArquivo.TipoArquivo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TIPO ARQUIVO é obrigatório.");
            }
            else
            {
                //Apenas aceitar pdf 
                if (docenteCandidatoArquivo.TipoArquivo.ToUpper() != "APPLICATION/PDF"
                    && docenteCandidatoArquivo.TipoArquivo.ToUpper() != "IMAGE/JPEG"
                    && docenteCandidatoArquivo.TipoArquivo.ToUpper() != "IMAGE/PNG")
                {
                    mensagens.Add("Apenas serão aceitos arquivos do tipo .PDF, .JPG e .PNG .");
                }
            }

            if (docenteCandidatoArquivo.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Busca situação do docente
                    int situacao = rnDocenteCandidato.ObtemSituacaoPor(contexto, Convert.ToInt32(docenteCandidatoArquivo.DocenteCandidatoId));

                    // Verifica se o candidato esta com situação convocado
                    if (situacao != (int)RN.RecursosHumanos.DocenteCandidato.EnumSituacao.Inscrito)
                    {
                        mensagens.Add("Este CANDIDATO já foi convocado e/ou Analisado.");
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

        public void Insere(Entidades.DocenteCandidatoArquivo docenteCandidatoArquivo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Insere documentos
                this.Insere(contexto, docenteCandidatoArquivo);

                //Insere Auditoria
                this.InsereAuditoria(contexto, docenteCandidatoArquivo, "CADASTRADO", System.Web.HttpContext.Current.Request.UserHostAddress);
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

        private void Insere(DataContext contexto, Entidades.DocenteCandidatoArquivo docenteCandidatoArquivo)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO RecursosHumanos.DOCENTECANDIDATOARQUIVO
                                           (DOCENTECANDIDATOID
                                           ,TIPODOCUMENTOID
                                           ,CHAVEARQUIVO
                                           ,ARQUIVO
                                           ,TIPOARQUIVO
                                           ,NOMEARQUIVO
                                           ,USUARIOID
                                           ,DATACADASTRO
                                           ,DATAALTERACAO)
                                     VALUES
	                                       (@DOCENTECANDIDATOID
                                           ,@TIPODOCUMENTOID
                                           ,NEWID()
                                           ,@ARQUIVO
                                           ,@TIPOARQUIVO
                                           ,@NOMEARQUIVO
                                           ,@USUARIOID
                                           ,@DATACADASTRO
                                           ,@DATAALTERACAO
										   ) 

                         SELECT IDENT_CURRENT('RecursosHumanos.DOCENTECANDIDATOARQUIVO') ";

            contextQuery.Parameters.Add("@DOCENTECANDIDATOID", SqlDbType.Int, docenteCandidatoArquivo.DocenteCandidatoId);
            contextQuery.Parameters.Add("@TIPODOCUMENTOID", SqlDbType.Int, docenteCandidatoArquivo.TipoDocumentoId);
            contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, docenteCandidatoArquivo.Arquivo);
            contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, docenteCandidatoArquivo.TipoArquivo);
            contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, docenteCandidatoArquivo.NomeArquivo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, docenteCandidatoArquivo.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            docenteCandidatoArquivo.DocenteCandidatoArquivoId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        public void Atualiza(Entidades.DocenteCandidatoArquivo docenteCandidatoArquivo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Insere documentos
                this.Atualiza(contexto, docenteCandidatoArquivo);

                //Insere Auditoria
                this.InsereAuditoria(contexto, docenteCandidatoArquivo, "ATUALIZA", System.Web.HttpContext.Current.Request.UserHostAddress);
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

        private void Atualiza(DataContext contexto, Entidades.DocenteCandidatoArquivo docenteCandidatoArquivo)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE RecursosHumanos.DOCENTECANDIDATOARQUIVO
                                       SET ARQUIVO = @ARQUIVO										
										,TIPOARQUIVO = @TIPOARQUIVO
										,NOMEARQUIVO = @NOMEARQUIVO 
                                        ,USUARIOID = @USUARIOID
                                        ,DATAALTERACAO = @DATAALTERACAO
                                     WHERE DOCENTECANDIDATOID = @DOCENTECANDIDATOID 
                                        AND TIPODOCUMENTOID = @TIPODOCUMENTOID";

            contextQuery.Parameters.Add("@DOCENTECANDIDATOID", SqlDbType.Int, docenteCandidatoArquivo.DocenteCandidatoId);
            contextQuery.Parameters.Add("@TIPODOCUMENTOID", SqlDbType.Int, docenteCandidatoArquivo.TipoDocumentoId);
            contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, docenteCandidatoArquivo.Arquivo);
            contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, docenteCandidatoArquivo.TipoArquivo);
            contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, docenteCandidatoArquivo.NomeArquivo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, docenteCandidatoArquivo.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public ValidacaoDados ValidaRemocao(int docenteCandidatoArquivoId, string usuarioId)
        {
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (docenteCandidatoArquivoId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (usuarioId.IsNullOrEmptyOrWhiteSpace())
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

        public void Remove(int docenteCandidatoArquivoId, string usuarioId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Insere auditoria arquivo
                this.InsereAuditoria(ctx, docenteCandidatoArquivoId, "REMOVIDO", System.Web.HttpContext.Current.Request.UserHostName, usuarioId);

                //Remove arquivo
                this.Remove(ctx, docenteCandidatoArquivoId);
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

        private void Remove(DataContext contexto, int docenteCandidatoArquivoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE RecursosHumanos.DOCENTECANDIDATOARQUIVO                                     
                                        WHERE  DOCENTECANDIDATOARQUIVOID = @DOCENTECANDIDATOARQUIVOID  ";

            contextQuery.Parameters.Add("@DOCENTECANDIDATOARQUIVOID", SqlDbType.Int, docenteCandidatoArquivoId);

            contexto.ApplyModifications(contextQuery);
        }

        public void RemovePorCandidatoDocente(DataContext contexto, int candidatoDocenteId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"   DELETE LYCEUM.RecursosHumanos.DOCENTECANDIDATOARQUIVO
                                        WHERE  DOCENTECANDIDATOID = @DOCENTECANDIDATOID ";

            contextQuery.Parameters.Add("@DOCENTECANDIDATOID", SqlDbType.Int, candidatoDocenteId);

            contexto.ApplyModifications(contextQuery);
        }

        private void InsereAuditoria(DataContext contexto, Entidades.DocenteCandidatoArquivo docenteCandidatoArquivo, string operacao, string estacao)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  INSERT INTO Poseidon.RecursosHumanos.DOCENTECANDIDATOARQUIVO
                                               (DOCENTECANDIDATOARQUIVOID
                                               ,DOCENTECANDIDATOID
                                                ,TIPODOCUMENTOID
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
                                               (@DOCENTECANDIDATOARQUIVOID, 
                                               @DOCENTECANDIDATOID,
                                               @TIPODOCUMENTOID,
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

                                        SELECT IDENT_CURRENT('RecursosHumanos.FORNECEDORDOCUMENTO') ";

            contextQuery.Parameters.Add("@DOCENTECANDIDATOARQUIVOID", SqlDbType.Int, docenteCandidatoArquivo.DocenteCandidatoArquivoId);
            contextQuery.Parameters.Add("@DOCENTECANDIDATOID", SqlDbType.Int, docenteCandidatoArquivo.DocenteCandidatoId);
            contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, docenteCandidatoArquivo.Arquivo);
            contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, docenteCandidatoArquivo.TipoArquivo);
            contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, docenteCandidatoArquivo.NomeArquivo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, docenteCandidatoArquivo.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAAUDITORIA", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@OPERACAO", SqlDbType.VarChar, operacao);
            contextQuery.Parameters.Add("@ESTACAO", SqlDbType.VarChar, estacao);
            contextQuery.Parameters.Add("@TIPODOCUMENTOID", SqlDbType.VarChar, docenteCandidatoArquivo.TipoDocumentoId);

            contexto.ApplyModifications(contextQuery);
        }

        public void InsereAuditoriaPorCandidatoDocente(DataContext contexto, int candidatoDocenteId, string operacao, string estacao, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO POSEIDON.RecursosHumanos.DOCENTECANDIDATOARQUIVO 
                                                    (DOCENTECANDIDATOARQUIVOID, 
                                                     DOCENTECANDIDATOID, 
                                                     TIPODOCUMENTOID,
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
                                        SELECT DOCENTECANDIDATOARQUIVOID, 
                                               DOCENTECANDIDATOID,
                                               TIPODOCUMENTOID ,
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
                                        FROM   LYCEUM.RecursosHumanos.DOCENTECANDIDATOARQUIVO
                                        WHERE  DOCENTECANDIDATOID = @DOCENTECANDIDATOID ";

            contextQuery.Parameters.Add("@DOCENTECANDIDATOID", SqlDbType.Int, candidatoDocenteId);
            contextQuery.Parameters.Add("@DATAAUDITORIA", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@OPERACAO", SqlDbType.VarChar, operacao);
            contextQuery.Parameters.Add("@ESTACAO", SqlDbType.VarChar, estacao);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);

            contexto.ApplyModifications(contextQuery);
        }

        private void InsereAuditoria(DataContext contexto, int docenteCandidatoArquivoId, string operacao, string estacao, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO POSEIDON.RecursosHumanos.DOCENTECANDIDATOARQUIVO 
                                                    (DOCENTECANDIDATOARQUIVOID, 
                                                     DOCENTECANDIDATOID,
                                                     TIPODOCUMENTOID,
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
                                        SELECT DOCENTECANDIDATOARQUIVOID, 
                                               DOCENTECANDIDATOID, 
                                               TIPODOCUMENTOID,
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
                                        FROM   LYCEUM.RecursosHumanos.DOCENTECANDIDATOARQUIVO
                                        WHERE  DOCENTECANDIDATOARQUIVOID = @DOCENTECANDIDATOARQUIVOID ";

            contextQuery.Parameters.Add("@DOCENTECANDIDATOARQUIVOID", SqlDbType.Int, docenteCandidatoArquivoId);
            contextQuery.Parameters.Add("@DATAAUDITORIA", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@OPERACAO", SqlDbType.VarChar, operacao);
            contextQuery.Parameters.Add("@ESTACAO", SqlDbType.VarChar, estacao);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);

            contexto.ApplyModifications(contextQuery);
        }
    }
}
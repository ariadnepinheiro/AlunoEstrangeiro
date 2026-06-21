using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using System.Data.SqlClient;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Certificacao
{
    public class DocumentoGerado
    {

        public DataTable Listar(int documentoCertID)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" 
                                        SELECT 
                                               DOCUMENTOGERADOID, DOCUMENTOCERTID, 
                                               NUMEROGERADO,NOMEARQUIVO,CHAVEARQUIVO,USUARIOID,DATACADASTRO
                                              
                                          FROM [CertificacaoEscolar].[DOCUMENTOGERADO]
                                         WHERE DOCUMENTOCERTID=@DOCUMENTOCERTID  ";
                contextQuery.Parameters.Add("@DOCUMENTOCERTID", SqlDbType.Int, documentoCertID);

                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
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
                ctx.Dispose();
            }

            return dt;
        }

        public RN.Certificacao.Entidades.DocumentoGerado ListarEntidade(int documentoCertID)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            Entidades.DocumentoGerado dadosDocumentoGerado = new Entidades.DocumentoGerado();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" 
                                        SELECT 
                                               DOCUMENTOGERADOID, DOCUMENTOCERTID, 
                                               NUMEROGERADO,NOMEARQUIVO,USUARIOID,
                                                DATACADASTRO,CHAVEARQUIVO,
                                                TIPOARQUIVO,DATACADASTRO,
                                                DATAALTERACAO
                                              
                                          FROM [CertificacaoEscolar].[DOCUMENTOGERADO]
                                         WHERE                                                                                                                                 DOCUMENTOCERTID=@DOCUMENTOCERTID
                                            ";
                contextQuery.Parameters.Add("@DOCUMENTOCERTID", SqlDbType.Int, documentoCertID);

                dt = ctx.GetDataTable(contextQuery);

                if (dt.Rows.Count > 0)
                {

                    dadosDocumentoGerado.DocumentoGeradoID = Convert.ToInt32(dt.Rows[0]["DOCUMENTOGERADOID"]);
                    dadosDocumentoGerado.DOCUMENTOCERTID = Convert.ToInt32(dt.Rows[0]["DOCUMENTOCERTID"]);
                    dadosDocumentoGerado.NUMEROGERADO = dt.Rows[0]["NUMEROGERADO"].ToString();
                    dadosDocumentoGerado.ChaveArquivo = dt.Rows[0]["CHAVEARQUIVO"].ToString();
                    dadosDocumentoGerado.NomeArquivo = dt.Rows[0]["NOMEARQUIVO"].ToString();
                    dadosDocumentoGerado.TipoArquivo = dt.Rows[0]["TIPOARQUIVO"].ToString();
                    dadosDocumentoGerado.DataAlteracao = Convert.ToDateTime(dt.Rows[0]["DATAALTERACAO"].ToString());
                    dadosDocumentoGerado.DataAlteracao = Convert.ToDateTime(dt.Rows[0]["DATACADASTRO"].ToString());


                }

            }
            catch (Exception ex)
            {
                ctx.Abandon();
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
                //throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }

            return dadosDocumentoGerado;
        }

        public bool Insere(Entidades.DocumentoGerado dadoscertificado)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            bool retorno = false;
            try
            {
                // retorno = this.
                Insere(contexto, dadoscertificado);
                this.Auditoria(contexto, dadoscertificado, "CADASTRO");
                retorno = true;
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
                //throw new Exception(mensagem);
            }
            finally
            {
                contexto.Dispose();
            }
            return retorno;
        }

        private void Insere(DataContext contexto, Entidades.DocumentoGerado DocumentoGerado)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO [CertificacaoEscolar].[DOCUMENTOGERADO]
                                                   ([DOCUMENTOCERTID]
                                                   ,[NUMEROGERADO]
                                                   ,[USUARIOID]
                                                   ,[DATACADASTRO]
                                                   ,[DATAALTERACAO])
                                             VALUES
			                                       (@DOCUMENTOCERTID
                                                  ,@NUMEROGERADO
                                                   ,@USUARIOID
                                                   ,@DATACADASTRO
                                                   ,@DATAALTERACAO)  

               SELECT IDENT_CURRENT('CertificacaoEscolar.DOCUMENTOGERADO') ";

            contextQuery.Parameters.Add("@DOCUMENTOCERTID", SqlDbType.Int, DocumentoGerado.DOCUMENTOCERTID);
            contextQuery.Parameters.Add("@NUMEROGERADO", SqlDbType.VarChar, DocumentoGerado.NUMEROGERADO);

            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, DocumentoGerado.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            DocumentoGerado.DocumentoGeradoID = Convert.ToInt32(contexto.GetReturnValue(contextQuery));

            // int x = contexto.ApplyModifications(contextQuery);

        }

        public bool Atualizar(Entidades.DocumentoGerado dadoscertificado)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            bool retorno = false;
            try
            {
                if (!string.IsNullOrEmpty(dadoscertificado.NomeArquivo))
                {
                    this.AtualizaArquivo(contexto, dadoscertificado);
                    this.Auditoria(contexto, dadoscertificado, "ALTERACAO");
                    retorno = true;
                }
                else
                {

                    this.Atualizar(contexto, dadoscertificado);
                    this.Auditoria(contexto, dadoscertificado, "CADASTRO");
                    retorno = true;
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
                //throw new Exception(mensagem);
            }
            finally
            {
                contexto.Dispose();
            }
            return retorno;
        }

        private void Atualizar(DataContext contexto, Entidades.DocumentoGerado DocumentoGerado)
        {
            ContextQuery contextQuery = new ContextQuery();
            //bool retorno = false;


            contextQuery.Command = @"UPDATE [CertificacaoEscolar].[DOCUMENTOGERADO]
                                               SET
                                                  [NUMEROGERADO] = @NUMEROGERADO
                                                  ,[USUARIOID] = @USUARIOID
                                                  ,[DATAALTERACAO] = @DATAALTERACAO
                                             WHERE 
                                             DOCUMENTOCERTID= @DOCUMENTOCERTID ";


            contextQuery.Parameters.Add("@DOCUMENTOCERTID", SqlDbType.Int, DocumentoGerado.DOCUMENTOCERTID);

            contextQuery.Parameters.Add("@NUMEROGERADO", SqlDbType.VarChar, DocumentoGerado.NUMEROGERADO);

            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, DocumentoGerado.UsuarioId);

            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);


            contexto.ApplyModifications(contextQuery);


        }

        private void Excluir(DataContext contexto, int documentoCertID)
        {
            ContextQuery contextQuery = new ContextQuery();
            //bool retorno = false;


            contextQuery.Command = @"delete from [CertificacaoEscolar].[DOCUMENTOGERADO]
                                               
                                             WHERE 
                                             DOCUMENTOCERTID= @DOCUMENTOCERTID ";


            contextQuery.Parameters.Add("@DOCUMENTOCERTID", SqlDbType.Int, documentoCertID);


            contexto.ApplyModifications(contextQuery);

        }

        private void AtualizaArquivo(DataContext contexto, Entidades.DocumentoGerado DocumentoGerado)
        {
            ContextQuery contextQuery = new ContextQuery();
            //bool retorno = false;


            contextQuery.Command = @"UPDATE [CertificacaoEscolar].[DOCUMENTOGERADO]
                                               SET 
                                              [ARQUIVO]=@ARQUIVO
                                              ,[TIPOARQUIVO]=@TIPOARQUIVO
                                              ,[NOMEARQUIVO]=@NOMEARQUIVO
                                              ,[NUMEROGERADO] = @NUMEROGERADO
                                              ,[USUARIOID] = @USUARIOID
                                              ,[DATAALTERACAO] = @DATAALTERACAO
                                             WHERE DOCUMENTOCERTID= @DOCUMENTOCERTID ";


            // SELECT IDENT_CURRENT('CertificacaoEscolar.DOCUMENTOGERADO') ";

            contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, DocumentoGerado.Arquivo);

            contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, DocumentoGerado.TipoArquivo);

            contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, DocumentoGerado.NomeArquivo);

            contextQuery.Parameters.Add("@DOCUMENTOCERTID", SqlDbType.Int, DocumentoGerado.DOCUMENTOCERTID);

            contextQuery.Parameters.Add("@NUMEROGERADO", SqlDbType.VarChar, DocumentoGerado.NUMEROGERADO);

            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, DocumentoGerado.UsuarioId);

            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);


            contexto.ApplyModifications(contextQuery);


        }

        public bool GeraSegundaVia(RN.Certificacao.infAluno aluno,string usuario)
        {
            RN.Certificacao.Entidades.DocumentoGerado dadosDocumentoGerado = new Techne.Lyceum.RN.Certificacao.Entidades.DocumentoGerado();
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            bool retorno = false;
            try
            {
                // retorno = this.
                dadosDocumentoGerado = this.ListarEntidade(aluno.documentoCertID);
                dadosDocumentoGerado.UsuarioId = usuario;

                byte[] arquivo = this.ObtemArquivoPor(aluno.documentoCertID);

                if(arquivo.Length>0)
                dadosDocumentoGerado.Arquivo = arquivo;

                this.Auditoria(contexto, dadosDocumentoGerado, "EXCLUSÃO");
                Excluir(contexto, aluno.documentoCertID);
                retorno = true;
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
                //throw new Exception(mensagem);
            }
            finally
            {
                contexto.Dispose();
            }
            return retorno;
        }

        public void Manter(Entidades.DocumentoGerado DocumentoGerado, string censo, int sequencial, string codigoValidador)
        {
            RN.Certificacao.DocumentoCertificacao rnDocumentoCertificacao = new DocumentoCertificacao();
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            DataTable dtDados = null;

            try
            {
                //Verifica se já existe cadastro
                dtDados = Listar(DocumentoGerado.DOCUMENTOCERTID);

                if (dtDados.Rows.Count > 0)
                {
                    DocumentoGerado.DocumentoGeradoID = Convert.ToInt32(dtDados.Rows[0]["DocumentoGeradoID"].ToString());
                    Atualizar(contexto, DocumentoGerado);   
                }
                else
                {
                    Insere(contexto, DocumentoGerado); 
                }

                //Atualizar dados do codigo gerado
                rnDocumentoCertificacao.AtualizarCodigoValidador(contexto, DocumentoGerado.DOCUMENTOCERTID, censo, sequencial, codigoValidador, DocumentoGerado.UsuarioId);
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

        private void Auditoria(DataContext contexto, Entidades.DocumentoGerado DocumentoGerado, string operacao)
        {

            ContextQuery contextQuery = new ContextQuery();


            switch (operacao)
            {
                case "CADASTRO":
                    {

                        contextQuery.Command = @"  INSERT INTO Poseidon.[CertificacaoEscolar].[DOCUMENTOGERADO]
                                                   ([DOCUMENTOGERADOID]
                                                   ,[DOCUMENTOCERTID]
                                                   ,[NUMEROGERADO]
                                                   ,[OPERACAO]
                                                   ,[USUARIOID]
                                                   ,[DATACADASTRO]
                                                   ,[DATAALTERACAO]
                                                   ,[CHAVEARQUIVO])
                                             VALUES
			                                       (@DOCUMENTOGERADOID
                                                   ,@DOCUMENTOCERTID
                                                   ,@NUMEROGERADO
                                                   ,@OPERACAO
                                                   ,@USUARIOID
                                                   ,@DATACADASTRO
                                                   ,@DATAALTERACAO
                                                   ,NEWID())";


                        contextQuery.Parameters.Add("@DOCUMENTOGERADOID", SqlDbType.Int, DocumentoGerado.DocumentoGeradoID);
                        contextQuery.Parameters.Add("@DOCUMENTOCERTID", SqlDbType.Int, DocumentoGerado.DOCUMENTOCERTID);
                        contextQuery.Parameters.Add("@NUMEROGERADO", SqlDbType.VarChar, DocumentoGerado.NUMEROGERADO);
                        contextQuery.Parameters.Add("@OPERACAO", SqlDbType.VarChar, operacao);
                        contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, DocumentoGerado.UsuarioId);
                        contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                        contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                    }
                    break;

                case "ALTERACAO":
                case "EXCLUSÃO":
                    {
                        contextQuery.Command = @"  INSERT INTO Poseidon.[CertificacaoEscolar].[DOCUMENTOGERADO]
                                                   ([DOCUMENTOGERADOID]
                                                   ,[DOCUMENTOCERTID]
                                                   ,[NUMEROGERADO]
                                                   ,[CHAVEARQUIVO]
                                                   ,[NOMEARQUIVO]
                                                   ,[TIPOARQUIVO]
                                                   ,[ARQUIVO]
                                                   ,[OPERACAO]
                                                   ,[USUARIOID]
                                                   ,[DATAALTERACAO])
                                             VALUES
			                                       (@DOCUMENTOGERADOID
                                                   ,@DOCUMENTOCERTID
                                                   ,@NUMEROGERADO
                                                   ,NEWID() 
                                                   ,@NOMEARQUIVO
                                                   ,@TIPOARQUIVO
                                                   ,@ARQUIVO
                                                   ,@OPERACAO
                                                   ,@USUARIOID
                                                   ,@DATAALTERACAO)";


                        contextQuery.Parameters.Add("@DOCUMENTOGERADOID", SqlDbType.Int, DocumentoGerado.DocumentoGeradoID);
                        contextQuery.Parameters.Add("@DOCUMENTOCERTID", SqlDbType.Int, DocumentoGerado.DOCUMENTOCERTID);
                        contextQuery.Parameters.Add("@NUMEROGERADO", SqlDbType.VarChar, DocumentoGerado.NUMEROGERADO);
                        //CHAVEARQUIVO
                        contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, DocumentoGerado.NomeArquivo);
                        contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, DocumentoGerado.TipoArquivo);
                        contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, DocumentoGerado.Arquivo);
                        contextQuery.Parameters.Add("@OPERACAO", SqlDbType.VarChar, operacao);
                        contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, DocumentoGerado.UsuarioId);
                        contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                    }
                    break;

            }


            contexto.ApplyModifications(contextQuery);


        }


        public byte[] ObtemArquivoPor(int documentocertid)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            byte[] arquivo = null;

            try
            {
                contextQuery.Command = @" SELECT ARQUIVO 
	                                                
                                         FROM [CertificacaoEscolar].[DOCUMENTOGERADO](NOLOCK) 
                                         WHERE                                                                                                                           DOCUMENTOCERTID=@DOCUMENTOCERTID ";

                contextQuery.Parameters.Add("@DOCUMENTOCERTID", SqlDbType.Int, documentocertid);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    arquivo = (byte[])reader["ARQUIVO"];
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
                //throw new Exception(mensagem);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                contexto.Dispose();

            }
            return arquivo;
        }



        public bool possuiArquivoGerado(DataContext ctx, int documentocertid)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @"SELECT COUNT(0) EXISTE 
                                         FROM [CertificacaoEscolar].[DOCUMENTOGERADO] 
                                         WHERE 
                                            DOCUMENTOCERTID=@DOCUMENTOCERTID 
                                            AND ARQUIVO IS NOT NULL  ";

            //verificar o aluno,modalidade,nivel
            contextQuery.Parameters.Add("@DOCUMENTOCERTID", SqlDbType.Int, documentocertid);


            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;

        }
    }
}

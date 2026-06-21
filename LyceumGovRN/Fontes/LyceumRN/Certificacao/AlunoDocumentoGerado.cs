using Techne.Lyceum.RN.DTOs;
using Techne.Lyceum.RN.Certificacao;
using System.ComponentModel;
using Techne.Data;
using Techne.Library;
using Techne.Lyceum.CR;
using Techne.Lyceum.RN.Entidades;
using Seeduc.Infra.Validation;
using Techne.Lyceum.RN.Servicos;
using Seeduc.Infra.Extensions;
using System.Text.RegularExpressions;
using Seeduc.Infra.Data;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.Certificacao
{
    public class AlunoDocumentoGerado
    {
        public DataTable Listar(int alunoDocumentoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" 
                                        SELECT 
                                              *                                              
                                          FROM [CertificacaoEscolar].[ALUNODOCUMENTOGERADO]
                                         WHERE 
                                                ALUNODOCUMENTOID = @ALUNODOCUMENTOID  ";

                contextQuery.Parameters.Add("@ALUNODOCUMENTOID", SqlDbType.Int, alunoDocumentoId);

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

        public bool Insere(Entidades.AlunoDocumentoGerado alunoDocumentoGerado)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            bool retorno = false;
            try
            {
                // retorno = this.
                Insere(contexto, alunoDocumentoGerado);
                this.Auditoria(contexto, alunoDocumentoGerado, "CADASTRO");
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

        private void Insere(DataContext contexto, Entidades.AlunoDocumentoGerado alunoDocumentoGerado)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO [CertificacaoEscolar].[ALUNODOCUMENTOGERADO]
                                                   ([ALUNODOCUMENTOID]                                                  
                                                   ,[NUMEROGERADO]
                                                   ,[ARQUIVO]
                                                   ,[TIPOARQUIVO]
                                                   ,[NOMEARQUIVO]
                                                   ,[CHAVEARQUIVO]
                                                   , USUARIOID
                                                   ,[DATACADASTRO]
                                                   ,[DATAALTERACAO])
                                             VALUES
			                                       (@ALUNODOCUMENTOID                                                 
                                                    ,@NUMEROGERADO
                                                    ,@ARQUIVO
                                                    ,@TIPOARQUIVO
                                                    ,@NOMEARQUIVO                                                   
                                                    ,@CHAVEARQUIVO
                                                   ,@USUARIOID
                                                   ,@DATACADASTRO
                                                   ,@DATAALTERACAO)  

               SELECT IDENT_CURRENT('CertificacaoEscolar.ALUNODOCUMENTOGERADO') ";

            contextQuery.Parameters.Add("@ALUNODOCUMENTOID", SqlDbType.Int, alunoDocumentoGerado.AlunoDocumentoId);
            contextQuery.Parameters.Add("@NUMEROGERADO", SqlDbType.VarChar, alunoDocumentoGerado.NumeroGerado);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, alunoDocumentoGerado.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, alunoDocumentoGerado.Arquivo);
            contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, alunoDocumentoGerado.TipoArquivo);
            contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, alunoDocumentoGerado.NomeArquivo);
            contextQuery.Parameters.Add("@CHAVEARQUIVO", SqlDbType.UniqueIdentifier, Guid.NewGuid());


            alunoDocumentoGerado.AlunoDocumentoGeradoId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));

        }

        public bool AtualizarArquivo(Entidades.AlunoDocumentoGerado alunoDocumentoGerado)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            bool retorno = false;
            try
            {
                if (!string.IsNullOrEmpty(alunoDocumentoGerado.NomeArquivo))
                {
                    this.AtualizaArquivo(contexto, alunoDocumentoGerado);
                    this.Auditoria(contexto, alunoDocumentoGerado, "ALTERACAO");
                    retorno = true;
                }
                else
                {

                    this.Atualizar(contexto, alunoDocumentoGerado);
                    this.Auditoria(contexto, alunoDocumentoGerado, "CADASTRO");
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

        private void Atualizar(DataContext contexto, Entidades.AlunoDocumentoGerado alunoDocumentoGerado)
        {
            ContextQuery contextQuery = new ContextQuery();
            //bool retorno = false;


            contextQuery.Command = @"UPDATE [CertificacaoEscolar].[ALUNODOCUMENTOGERADO]
                                               SET
                                                  [NUMEROGERADO] = @NUMEROGERADO
                                                  ,[USUARIOID] = @USUARIOID
                                                  ,[DATAALTERACAO] = @DATAALTERACAO
                                             WHERE 
                                             ALUNODOCUMENTOGERADOID= @ALUNODOCUMENTOGERADOID ";


            contextQuery.Parameters.Add("@ALUNODOCUMENTOGERADOID", SqlDbType.Int, alunoDocumentoGerado.AlunoDocumentoGeradoId);

            contextQuery.Parameters.Add("@NUMEROGERADO", SqlDbType.VarChar, alunoDocumentoGerado.NumeroGerado);

            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, alunoDocumentoGerado.UsuarioId);

            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);


            contexto.ApplyModifications(contextQuery);


        }

        private void Excluir(DataContext contexto, int alunoDocumentoGeradoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            //bool retorno = false;


            contextQuery.Command = @"delete from [CertificacaoEscolar].[ALUNODOCUMENTOGERADO]
                                               
                                             WHERE 
                                             ALUNODOCUMENTOGERADOID= @ALUNODOCUMENTOGERADOID ";


            contextQuery.Parameters.Add("@ALUNODOCUMENTOGERADOID", SqlDbType.Int, alunoDocumentoGeradoId);


            contexto.ApplyModifications(contextQuery);

        }

        public RN.Certificacao.Entidades.AlunoDocumentoGerado ListarEntidade(int alunoDocumentoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            Entidades.AlunoDocumentoGerado dadosAlunoDocumentoGerado = new Entidades.AlunoDocumentoGerado();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" 
                                        SELECT 
                                               ALUNODOCUMENTOGERADOID, ALUNOCERTIFICACAOID, 
                                               NUMEROGERADO,NOMEARQUIVO,USUARIOID,
                                                DATACADASTRO,CHAVEARQUIVO,
                                                TIPOARQUIVO,DATACADASTRO,
                                                DATAALTERACAO
                                              
                                         FROM [CertificacaoEscolar].[ALUNODOCUMENTOGERADOID]
                                         WHERE 
                                                ALUNODOCUMENTOID = @ALUNODOCUMENTOID                                                                                                                                 DOCUMENTOCERTID=@DOCUMENTOCERTID
                                            ";

                contextQuery.Parameters.Add("@ALUNODOCUMENTOID", SqlDbType.Int, alunoDocumentoId);

                dt = ctx.GetDataTable(contextQuery);

                if (dt.Rows.Count > 0)
                {

                    dadosAlunoDocumentoGerado.AlunoDocumentoGeradoId = Convert.ToInt32(dt.Rows[0]["ALUNODOCUMENTOGERADOID"]);
                    dadosAlunoDocumentoGerado.AlunoDocumentoId = Convert.ToInt32(dt.Rows[0]["ALUNODOCUMENTOID"]);
                    dadosAlunoDocumentoGerado.NumeroGerado = dt.Rows[0]["NUMEROGERADO"].ToString();
                    dadosAlunoDocumentoGerado.ChaveArquivo = dt.Rows[0]["CHAVEARQUIVO"].ToString();
                    dadosAlunoDocumentoGerado.NomeArquivo = dt.Rows[0]["NOMEARQUIVO"].ToString();
                    dadosAlunoDocumentoGerado.TipoArquivo = dt.Rows[0]["TIPOARQUIVO"].ToString();
                    dadosAlunoDocumentoGerado.DataAlteracao = Convert.ToDateTime(dt.Rows[0]["DATAALTERACAO"].ToString());
                    dadosAlunoDocumentoGerado.DataAlteracao = Convert.ToDateTime(dt.Rows[0]["DATACADASTRO"].ToString());


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

            return dadosAlunoDocumentoGerado;
        }

        public bool GeraSegundaVia(RN.Certificacao.infAluno aluno, string usuario)
        {
            RN.Certificacao.Entidades.AlunoDocumentoGerado dadosAlunoDocumentoGerado = new Techne.Lyceum.RN.Certificacao.Entidades.AlunoDocumentoGerado();
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            bool retorno = false;
            int alunoId = 0;
            try
            {
                // retorno = this.
                dadosAlunoDocumentoGerado = this.ListarEntidade(dadosAlunoDocumentoGerado.AlunoDocumentoId);
                dadosAlunoDocumentoGerado.UsuarioId = usuario;

                byte[] arquivo = this.ObtemArquivoPor(alunoId);

                if (arquivo.Length > 0)
                    dadosAlunoDocumentoGerado.Arquivo = arquivo;

                this.Auditoria(contexto, dadosAlunoDocumentoGerado, "EXCLUSÃO");
                Excluir(contexto, alunoId);
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

        public void Manter(Entidades.AlunoDocumentoGerado dadosAlunoDocumentoGerado, string unidade_Ens, int sequencial, string numerocertificado)
        {
            RN.Certificacao.DocumentoCertificacao rnDocumentoCertificacao = new DocumentoCertificacao();
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            DataTable dtDados = null;

            try
            {
                //Verifica se já existe cadastro
                dtDados = Listar(dadosAlunoDocumentoGerado.AlunoDocumentoId);

                if (dtDados.Rows.Count > 0)
                {
                    dadosAlunoDocumentoGerado.AlunoDocumentoGeradoId = Convert.ToInt32(dtDados.Rows[0]["ALUNODOCUMENTOGERADOID"].ToString());
                    Atualizar(contexto, dadosAlunoDocumentoGerado);
                }
                else
                {
                    Insere(contexto, dadosAlunoDocumentoGerado);
                }

                Atualiza(contexto, dadosAlunoDocumentoGerado);

                AtualizarCodigoValidador(contexto, dadosAlunoDocumentoGerado.AlunoDocumentoId, unidade_Ens, sequencial, numerocertificado, dadosAlunoDocumentoGerado.UsuarioId);

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

        private void Auditoria(DataContext contexto, Entidades.AlunoDocumentoGerado alunoDocumentoGerado, string operacao)
        {

            ContextQuery contextQuery = new ContextQuery();


            switch (operacao)
            {
                case "CADASTRO":
                    {

                        contextQuery.Command = @"  INSERT INTO Poseidon.[CertificacaoEscolar].[ALUNODOCUMENTOGERADO]
                                                   (ALUNODOCUMENTOGERADOID
                                                   ,[ALUNODOCUMENTOID]
                                                   ,[NUMEROGERADO]
                                                   ,[OPERACAO]
                                                   ,[USUARIOID]
                                                   ,[DATACADASTRO]
                                                   ,[DATAALTERACAO]
                                                   ,[CHAVEARQUIVO])
                                             VALUES
			                                       (@ALUNODOCUMENTOGERADOID
                                                   ,@ALUNODOCUMENTOID
                                                   ,@NUMEROGERADO
                                                   ,@OPERACAO
                                                   ,@USUARIOID
                                                   ,@DATACADASTRO
                                                   ,@DATAALTERACAO
                                                   ,NEWID())";


                        contextQuery.Parameters.Add("@ALUNODOCUMENTOID", SqlDbType.Int, alunoDocumentoGerado.AlunoDocumentoId);
                        contextQuery.Parameters.Add("@ALUNODOCUMENTOGERADOID", SqlDbType.Int, alunoDocumentoGerado.AlunoDocumentoGeradoId);
                        contextQuery.Parameters.Add("@NUMEROGERADO", SqlDbType.VarChar, alunoDocumentoGerado.NumeroGerado);
                        contextQuery.Parameters.Add("@OPERACAO", SqlDbType.VarChar, operacao);
                        contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, alunoDocumentoGerado.UsuarioId);
                        contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                        contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                    }
                    break;

                case "ALTERACAO":
                case "EXCLUSÃO":
                    {
                        contextQuery.Command = @"  INSERT INTO Poseidon.[CertificacaoEscolar].[ALUNODOCUMENTOGERADO]
                                                   (
                                                   ALUNODOCUMENTOGERADOID
                                                   ,[ALUNODOCUMENTOID]
                                                   ,[NUMEROGERADO]
                                                   ,[CHAVEARQUIVO]
                                                   ,[NOMEARQUIVO]
                                                   ,[TIPOARQUIVO]
                                                   ,[ARQUIVO]
                                                   ,[OPERACAO]
                                                   ,[USUARIOID]
                                                   ,[DATAALTERACAO])
                                             VALUES
			                                       (@ALUNODOCUMENTOGERADOID
                                                   ,@ALUNODOCUMENTOID
                                                   ,@NUMEROGERADO
                                                   ,NEWID() 
                                                   ,@NOMEARQUIVO
                                                   ,@TIPOARQUIVO
                                                   ,@ARQUIVO
                                                   ,@OPERACAO
                                                   ,@USUARIOID
                                                   ,@DATAALTERACAO)";


                        contextQuery.Parameters.Add("@ALUNODOCUMENTOID", SqlDbType.Int, alunoDocumentoGerado.AlunoDocumentoId);
                        contextQuery.Parameters.Add("@ALUNODOCUMENTOGERADOID", SqlDbType.Int, alunoDocumentoGerado.AlunoDocumentoGeradoId);
                        contextQuery.Parameters.Add("@NUMEROGERADO", SqlDbType.VarChar, alunoDocumentoGerado.NumeroGerado);
                        //CHAVEARQUIVO
                        contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, alunoDocumentoGerado.NomeArquivo);
                        contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, alunoDocumentoGerado.TipoArquivo);
                        contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, alunoDocumentoGerado.Arquivo);
                        contextQuery.Parameters.Add("@OPERACAO", SqlDbType.VarChar, operacao);
                        contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, alunoDocumentoGerado.UsuarioId);
                        contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                    }
                    break;

            }


            contexto.ApplyModifications(contextQuery);


        }


        public byte[] ObtemArquivoPor(int alunoCertificacaoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            byte[] arquivo = null;

            try
            {
                contextQuery.Command = @" SELECT ARQUIVO 
	                                                
                                         FROM [CertificacaoEscolar].[ALUNODOCUMENTOGERADO](NOLOCK) 
                                         WHERE    ALUNOCERTIFICACAOID=@ALUNOCERTIFICACAOID ";

                contextQuery.Parameters.Add("@ALUNOCERTIFICACAOID", SqlDbType.Int, alunoCertificacaoId);

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



        public bool possuiArquivoGerado(DataContext ctx, int alunoCertificacaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @"SELECT COUNT(0) EXISTE 
                                         FROM [CertificacaoEscolar].[ALUNODOCUMENTOGERADO] 
                                         WHERE 
                                            ALUNOCERTIFICACAOID=@ALUNOCERTIFICACAOID 
                                            AND ARQUIVO IS NOT NULL  ";

            //verificar o aluno,modalidade,nivel
            contextQuery.Parameters.Add("@ALUNOCERTIFICACAOID", SqlDbType.Int, alunoCertificacaoId);


            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;

        }

        public bool AtualizarCodigoValidador(int alunoCertificacaoId, string censo, int sequencial, string codigoValidador, string usuarioId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            bool retorno = false;
            try
            {
                if (alunoCertificacaoId != 0)
                {
                    this.AtualizarCodigoValidador(contexto, alunoCertificacaoId, censo, sequencial, codigoValidador, usuarioId);
                    retorno = true;
                }
                else
                    throw new Exception("Não foi possível encontrar o ID");
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

            }
            finally
            {
                contexto.Dispose();
            }
            return retorno;
        }

        public void AtualizarCodigoValidador(DataContext ctx, int alunoDocumentoId, string censo, int sequencial, string codigoValidador, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"Update [CertificacaoEscolar].[ALUNODOCUMENTO]                                       
                                        set
                                          [UNIDADEENSINO] = @UNIDADEENSINO 
                                         ,[SEQUENCIAL] = @SEQUENCIAL  
                                         ,[CODIGOVALIDOR] = @CODIGOVALIDOR                                                                                                               
                                         ,[USUARIOID] = @USUARIOID                                         
                                         ,[DATAALTERACAO] = @DATAALTERACAO
                                   where   
                                          [ALUNODOCUMENTOID] = @ALUNODOCUMENTOID";

            contextQuery.Parameters.Add("@UNIDADEENSINO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@SEQUENCIAL", SqlDbType.Int, sequencial);
            contextQuery.Parameters.Add("@CODIGOVALIDOR", SqlDbType.VarChar, codigoValidador);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@ALUNODOCUMENTOID", SqlDbType.Int, alunoDocumentoId);

            ctx.ApplyModifications(contextQuery);
        }



        public int ObtemSequencialPor(DataContext ctx, string unidade, int tipoDocumento, int documentoCertId)
        {
            int quantidade = 0;
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" select isnull(SEQUENCIAL, 0) SEQUENCIAL
                                            from [CertificacaoEscolar].[DOCUMENTOCERTIFICACAO]
                                            where DOCUMENTOCERTID = @DOCUMENTOCERTID
                                                and CENSO = @CENSO
                                                and DOCUMENTOID = @TIPODOCUMENTO ";

                contextQuery.Parameters.Add("@DOCUMENTOCERTID", documentoCertId);
                contextQuery.Parameters.Add("@CENSO", unidade);
                contextQuery.Parameters.Add("@TIPODOCUMENTO", tipoDocumento);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    quantidade = Convert.ToInt32(reader["SEQUENCIAL"]);
                }

                return quantidade;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }


        public bool Atualiza(DataContext contexto, Entidades.AlunoDocumentoGerado documentoGerado)
        {

            bool retorno = false;
            if (!string.IsNullOrEmpty(documentoGerado.NomeArquivo))
            {
                this.AtualizaArquivo(contexto, documentoGerado);
                this.Auditoria(contexto, documentoGerado, "ALTERACAO");
                retorno = true;
            }
            else
            {

                this.Atualizar(contexto, documentoGerado);
                this.Auditoria(contexto, documentoGerado, "CADASTRO");
                retorno = true;
            }

            return retorno;
        }

        private void AtualizaArquivo(DataContext contexto, Entidades.AlunoDocumentoGerado documentoGerado)
        {
            ContextQuery contextQuery = new ContextQuery();
            contextQuery.Command = @"UPDATE [CertificacaoEscolar].[ALUNODOCUMENTOGERADO]
                                               SET 
                                              [ARQUIVO]=@ARQUIVO
                                              ,[TIPOARQUIVO]=@TIPOARQUIVO
                                              ,[NOMEARQUIVO]=@NOMEARQUIVO
                                              ,[NUMEROGERADO] = @NUMEROGERADO
                                              ,[CHAVEARQUIVO] = @CHAVEARQUIVO
                                              ,[USUARIOID] = @USUARIOID
                                              ,[DATAALTERACAO] = @DATAALTERACAO
                                             WHERE ALUNODOCUMENTOGERADOID= @ALUNODOCUMENTOGERADOID ";


            contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, documentoGerado.Arquivo);
            contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, documentoGerado.TipoArquivo);
            contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, documentoGerado.NomeArquivo);
            contextQuery.Parameters.Add("@ALUNODOCUMENTOGERADOID", SqlDbType.Int, documentoGerado.AlunoDocumentoGeradoId);
            contextQuery.Parameters.Add("@NUMEROGERADO", SqlDbType.VarChar, documentoGerado.NumeroGerado);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, documentoGerado.UsuarioId);
            contextQuery.Parameters.Add("@CHAVEARQUIVO", SqlDbType.UniqueIdentifier, Guid.NewGuid());
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);


            contexto.ApplyModifications(contextQuery);

        }
    }
}
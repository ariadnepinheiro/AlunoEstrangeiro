using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.Certificacao
{
    public class DocumentoCertificacao
    {

        /// <summary>
        /// Lista as informaçãoes do Documento de Certificação filtrando pelo Aluno.
        /// </summary>
        /// <param name="Aluno">Matricula do Aluno</param>
        /// <returns>DataTable com as informações da certificação </returns>
        public DataTable Listar(string Aluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" 
                                     SELECT    [DOCUMENTOCERTID]
                                              ,[TIPOCONCLUSAOID]
                                              ,[ALUNO]
                                              ,[DOCUMENTOID]
                                              ,[NUMERO]
                                              ,[FOLHAS]
                                              ,[LIVRO]
                                              ,[OBSERVACAO]
                                              ,[EIXO]
                                              ,[CENSO]
                                              ,[SEQUENCIAL]
                                              ,[CODIGOVALIDOR]
                                          FROM [CertificacaoEscolar].[DOCUMENTOCERTIFICACAO]
                                          where [ALUNO]=@aluno
                                          and DOCUMENTOID <> 1
                                       ";

                contextQuery.Parameters.Add("@aluno", SqlDbType.VarChar, Aluno);

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

        /// <summary>
        /// Lista as informaçãoes do Documento de Certificação filtrando pelo Aluno e tipo de Conclusão.
        /// </summary>
        /// <param name="Aluno">Matricula do Aluno</param>
        /// <param name="TipoConclusaoId">Tipo de conclusão
        ///1	FUNDAMENTAL
        ///2	MÉDIO
        ///3	PROFISSIONALIZANTE
        /// 
        /// </param>
        /// <returns>DataTable com as informações da certificação</returns>
        /// 
        public DataTable Listar(string Aluno, int TipoConclusaoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" 
                                     SELECT    [DOCUMENTOCERTID]
                                              ,[TIPOCONCLUSAOID]
                                              ,[ALUNO]
                                              ,[DOCUMENTOID]
                                              ,[NUMERO]
                                              ,[FOLHAS]
                                              ,[LIVRO]
                                              ,[OBSERVACAO]
                                              ,[EIXO]
                                              ,[CENSO]
                                              ,[SEQUENCIAL]
                                              ,[CODIGOVALIDOR]
                                          FROM [CertificacaoEscolar].[DOCUMENTOCERTIFICACAO]
                                         where [ALUNO]=@ALUNO AND
                                               [TIPOCONCLUSAOID]=@TIPOCONCLUSAOID
                                       ";

                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, Aluno);
                contextQuery.Parameters.Add("@TIPOCONCLUSAOID", SqlDbType.Int, TipoConclusaoId);

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


        /// <summary>
        /// Lista as informaçãoes do Documento de Certificação filtrando pelo Aluno,Tipo de Conclusão e Tipo de Documento.
        /// </summary>
        /// <param name="Aluno">Matricula do Aluno</param>
        /// <param name="TipoConclusaoId">Tipo de Conclusão
        ///1	FUNDAMENTAL
        ///2	MÉDIO
        ///3	PROFISSIONALIZANTE/// </param>
        /// 
        /// <param name="TipoDocumentoCertifica">Tipo de Documento
        ///
        ///1		Histórico Escolar
        ///2		Certidão
        ///3		Certificado Escolar
        ///4		Diploma
        /// 
        /// 
        /// </param>
        /// <returns>DataTable com as informações da certificação:[DOCUMENTOCERTID]                                             ,[TIPOCONCLUSAOID],[ALUNO],[DOCUMENTOID],[NUMERO],[FOLHAS],[LIVRO],[OBSERVACAO],[EIXO]</returns>
        /// 

        public DataTable Listar(string Aluno, int TipoConclusaoId, int TipoDocumentoCertifica)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" 
                                     SELECT    [DOCUMENTOCERTID]
                                              ,[TIPOCONCLUSAOID]
                                              ,[ALUNO]
                                              ,[DOCUMENTOID]
                                              ,[NUMERO]
                                              ,[FOLHAS]
                                              ,[LIVRO]
                                              ,[OBSERVACAO]
                                              ,[EIXO]                                              
                                              ,[CENSO]
                                              ,[SEQUENCIAL]
                                              ,[CODIGOVALIDOR]
                                          FROM [CertificacaoEscolar].[DOCUMENTOCERTIFICACAO]
                                         where [ALUNO]=@ALUNO AND
                                               [TIPOCONCLUSAOID]=@TIPOCONCLUSAOID AND
                                               [DOCUMENTOID]=@TIPODOCUMENTOCERTIFICA
                                       ";

                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, Aluno);
                contextQuery.Parameters.Add("@TIPOCONCLUSAOID", SqlDbType.Int, TipoConclusaoId);
                contextQuery.Parameters.Add("@TIPODOCUMENTOCERTIFICA", SqlDbType.Int, TipoDocumentoCertifica);


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


        public ValidacaoDados ValidaInsercao(Entidades.DocumentoCertificacao DocumentoCert)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };


            if (DocumentoCert.Aluno.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Aluno é obrigatório.");
            }


            if (DocumentoCert.TipoConclusaoId == 0)
            {
                mensagens.Add("TipoConclusao é obrigatório.");
            }

            if (DocumentoCert.DocumentoId == 0)
            {
                mensagens.Add("TipoDocumento é obrigatório.");
            }

            if (DocumentoCert.DocumentoId != 1) // apenas o histórico pode ser nulo, o restante precisa preencher os dados.
            {

                if (DocumentoCert.Livro.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Livro é obrigatório.");
                }
                else 
                {
                    if (DocumentoCert.Livro.Length > 10)
                    {
                        mensagens.Add("Livro deve conter no máximo 10 caracteres.");
                    }
                }

                if (DocumentoCert.Numero.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Numero é obrigatório.");
                }
                else
                {
                    if (DocumentoCert.Numero.Length > 10)
                    {
                        mensagens.Add("Numero deve conter no máximo 10 caracteres.");
                    }
                }

                if (DocumentoCert.Folhas.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Folha é obrigatória.");
                }
                else 
                {
                    if (DocumentoCert.Folhas.Length > 10)
                    {
                        mensagens.Add("Folha deve conter no máximo 10 caracteres.");
                    }
                }
            }
            else
            {
                DocumentoCert.Livro = null;
                DocumentoCert.Numero = null;
                DocumentoCert.Folhas = null;
                DocumentoCert.Eixo = string.Empty;
            }


            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se JÁ FOI UTILLIZADO 
                    if (this.possuiDocumentoCertificacao(contexto, DocumentoCert))
                    {
                        mensagens.Add("As informações já existem.");
                    }

                    if (PossuiDocumentoCertificacaoPessoaPor(contexto, DocumentoCert))
                    {
                        mensagens.Add("Já existe uma solicitação para esta pessoa do mesmo Tipo de Documento e Conclusão.");
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
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + "</BR>" + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public bool Insere(Entidades.DocumentoCertificacao dadosLivro)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            bool retorno = false;
            try
            {
                // retorno = this.
                Insere(contexto, dadosLivro);
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

        private void Insere(DataContext contexto, Entidades.DocumentoCertificacao DocumentoCert)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"INSERT INTO [CertificacaoEscolar].[DOCUMENTOCERTIFICACAO]
                                      (
                                      [TIPOCONCLUSAOID]
                                      ,[ALUNO]
                                      ,[DOCUMENTOID]
                                      ,[NUMERO]
                                      ,[FOLHAS]
                                      ,[LIVRO]
                                      ,[OBSERVACAO]
                                      ,[EIXO]
                                      ,[USUARIOID]
                                      ,[DATACADASTRO]
                                      ,[DATAALTERACAO]
                                      ,[AUTORIZADO]
                                      ,[PESSOA]
                                      )
                                VALUES (@TIPOCONCLUSAOID,
                                        @ALUNO,
                                        @DOCUMENTOID,
                                        @NUMERO,
                                        @FOLHAS,
                                        @LIVRO,
                                        @OBSERVACAO,
                                        @EIXO,
                                        @USUARIOID,
                                        @DATACADASTRO,
                                        @DATAALTERACAO,
                                        @AUTORIZADO,
                                        @PESSOA)
                                ";

            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, DocumentoCert.Aluno);
            contextQuery.Parameters.Add("@EIXO", SqlDbType.VarChar, DocumentoCert.Eixo);
            contextQuery.Parameters.Add("@TIPOCONCLUSAOID", SqlDbType.Int, DocumentoCert.TipoConclusaoId);
            contextQuery.Parameters.Add("@DOCUMENTOID", SqlDbType.Int, DocumentoCert.DocumentoId);
            contextQuery.Parameters.Add("@NUMERO", SqlDbType.VarChar, DocumentoCert.Numero);
            contextQuery.Parameters.Add("@FOLHAS", SqlDbType.VarChar, DocumentoCert.Folhas);
            contextQuery.Parameters.Add("@LIVRO", SqlDbType.VarChar, DocumentoCert.Livro);
            contextQuery.Parameters.Add("@OBSERVACAO", SqlDbType.VarChar, DocumentoCert.Observacao);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, DocumentoCert.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@PESSOA", SqlDbType.Decimal, DocumentoCert.Pessoa);

            if (DocumentoCert.DocumentoId == 1)
                contextQuery.Parameters.Add("@AUTORIZADO", SqlDbType.Bit, 1);
            else
                contextQuery.Parameters.Add("@AUTORIZADO", SqlDbType.Bit, 0);

            //dadosGrupo.GrupoId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));

            int x = contexto.ApplyModifications(contextQuery);

        }

        public void Remover(Entidades.DocumentoCertificacao DocumentoCert)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" delete [CertificacaoEscolar].[DOCUMENTOCERTIFICACAO]
                                                WHERE 
                                              DOCUMENTOCERTID = @DOCUMENTOCERTID";


                contextQuery.Parameters.Add("@DOCUMENTOCERTID", SqlDbType.Int, DocumentoCert.DocumentoCertId);

                contexto.ApplyModifications(contextQuery);
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

        public ValidacaoDados ValidaRemocao(int documentocertid)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };


            if (documentocertid == 0)
            {
                mensagens.Add("Campo ID é obrigatório para exclusão.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se JÁ FOI UTILLIZADO pelo ASSUNTO
                    if (this.possuiDocumentoGerado(contexto, documentocertid))
                    {
                        mensagens.Add("O Documento não pode ser excluido pois já possui documento gerado.");
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
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + "</BR>" + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public ValidacaoDados ValidaAtualizacao(Entidades.DocumentoCertificacao DocumentoCert)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };


            if (DocumentoCert.Aluno.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Aluno é obrigatório.");
            }


            if (DocumentoCert.DocumentoId == 0)
            {
                mensagens.Add("TipoDocumento é obrigatório.");
            }

            if (DocumentoCert.TipoConclusaoId == 0)
            {
                mensagens.Add("TipoConclusao é obrigatório.");
            }

            if (DocumentoCert.DocumentoId != 1) // apenas o histórico pode ser nulo, o restante precisa preencher os dados.
            {

                if (DocumentoCert.Livro.IsNullOrEmptyOrWhiteSpace())
                    mensagens.Add("Livro é obrigatório.");

                if (DocumentoCert.Numero.IsNullOrEmptyOrWhiteSpace())
                    mensagens.Add("Número é obrigatório.");

                if (DocumentoCert.Folhas.IsNullOrEmptyOrWhiteSpace())
                    mensagens.Add("Folha é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se JÁ FOI UTILLIZADO 
                    //if (this.possuiDocumentoCertificacao(contexto, DocumentoCert))
                    //{
                    //    mensagens.Add("As informações já existem.");
                    //}

                    if (DocumentoCert.DocumentoId == 2) //Certidão 
                    {
                        if (this.possuiDocumentoGerado(contexto, DocumentoCert.DocumentoCertId))
                        {
                            mensagens.Add("O documento já foi gerado, não é possivel atualizar as informações. Solicite 2ª via do documento.");
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
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + "</BR>" + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public bool AutorizarDocumentoCertificacao(string usuarioid, int documentocertid)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;
            try
            {

                contextQuery.Command = @"Update [CertificacaoEscolar].[DOCUMENTOCERTIFICACAO]                                       
                                        set
                                          [AUTORIZADO] = 1                                                                                                               
                                         ,[USUARIOID] = @USUARIOID                                         
                                         ,[DATAALTERACAO] = @DATAALTERACAO
                                   where   
                                          [DOCUMENTOCERTID] = @DOCUMENTOCERTID";

                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioid);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DOCUMENTOCERTID", SqlDbType.Int, documentocertid);

                ctx.ApplyModifications(contextQuery);

                retorno = true;
            }
            catch (Exception ex)
            {
                retorno = false;

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

            }
            finally
            {
                ctx.Dispose();
            }
            return retorno;
        }

        public void AtualizarCodigoValidador(DataContext ctx, int documentocertid, string censo, int sequencial, string codigoValidador, string usuarioId)
        {
            //Os dados serão atualizados ao criar o pdf
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"Update [CertificacaoEscolar].[DOCUMENTOCERTIFICACAO]                                       
                                        set
                                          [CENSO] = @CENSO 
                                         ,[SEQUENCIAL] = @SEQUENCIAL  
                                         ,[CODIGOVALIDOR] = @CODIGOVALIDOR                                                                                                               
                                         ,[USUARIOID] = @USUARIOID                                         
                                         ,[DATAALTERACAO] = @DATAALTERACAO
                                   where   
                                          [DOCUMENTOCERTID] = @DOCUMENTOCERTID";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@SEQUENCIAL", SqlDbType.Int, sequencial);
            contextQuery.Parameters.Add("@CODIGOVALIDOR", SqlDbType.VarChar, codigoValidador);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DOCUMENTOCERTID", SqlDbType.Int, documentocertid);

            ctx.ApplyModifications(contextQuery);
        }

        public bool Atualizar(Entidades.DocumentoCertificacao dadosLivro)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            bool retorno = false;
            try
            {
                if (dadosLivro.DocumentoCertId != 0)
                {
                    this.Atualizar(contexto, dadosLivro);
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

        private void Atualizar(DataContext contexto, Entidades.DocumentoCertificacao DocumentoCert)
        {
            ContextQuery contextQuery = new ContextQuery();
            //bool retorno = false;


            contextQuery.Command = @"Update [CertificacaoEscolar].[DOCUMENTOCERTIFICACAO]                                       
                                        set                                           
                                          [TIPOCONCLUSAOID] = @TIPOCONCLUSAOID
                                          ,[ALUNO] = @ALUNO
                                          ,[NUMERO] = @NUMERO
                                          ,[FOLHAS] = @FOLHAS
                                          ,[LIVRO] = @LIVRO
                                          ,[OBSERVACAO] = @OBSERVACAO
                                          ,[EIXO]=@EIXO
                                          ,[USUARIOID] = @USUARIOID                                         
                                          ,[DATAALTERACAO] = @DATAALTERACAO
                                          ,[PESSOA]=@PESSOA
                                   where   
                                          [DOCUMENTOCERTID] = @DOCUMENTOCERTID";

            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, DocumentoCert.Aluno);
            contextQuery.Parameters.Add("@DOCUMENTOCERTID", SqlDbType.Int, DocumentoCert.DocumentoCertId);
            contextQuery.Parameters.Add("@TIPOCONCLUSAOID", SqlDbType.Int, DocumentoCert.TipoConclusaoId);
            contextQuery.Parameters.Add("@EIXO", SqlDbType.VarChar, DocumentoCert.Eixo);
            contextQuery.Parameters.Add("@NUMERO", SqlDbType.VarChar, DocumentoCert.Numero);
            contextQuery.Parameters.Add("@FOLHAS", SqlDbType.VarChar, DocumentoCert.Folhas);
            contextQuery.Parameters.Add("@LIVRO", SqlDbType.VarChar, DocumentoCert.Livro);
            contextQuery.Parameters.Add("@OBSERVACAO", SqlDbType.VarChar, DocumentoCert.Observacao);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, DocumentoCert.UsuarioId);
            contextQuery.Parameters.Add("@PESSOA", SqlDbType.Decimal, DocumentoCert.Pessoa);

            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public bool possuiDocumentoCertificacao(DataContext ctx, Entidades.DocumentoCertificacao DocumentoCert)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @"SELECT COUNT(0) EXISTE 
                                         FROM [CertificacaoEscolar].[DOCUMENTOCERTIFICACAO] 
                                         WHERE 
                                             DOCUMENTOID=@DOCUMENTOID and 
                                             TIPOCONCLUSAOID=@TIPOCONCLUSAOID and
                                             ALUNO=@ALUNO ";

            //verificar o aluno,modalidade,nivel
            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, DocumentoCert.Aluno);
            contextQuery.Parameters.Add("@DOCUMENTOID", SqlDbType.Int, DocumentoCert.DocumentoId);
            contextQuery.Parameters.Add("@TIPOCONCLUSAOID", SqlDbType.Int, DocumentoCert.TipoConclusaoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;

        }

        public bool possuiDocumentoGerado(DataContext ctx, int documentocertid)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @"SELECT COUNT(0) EXISTE 
                                         FROM [CertificacaoEscolar].[DOCUMENTOGERADO] 
                                         WHERE 
                                              DOCUMENTOCERTID=@DOCUMENTOCERTID ";

            //verificar o aluno,modalidade,nivel
            contextQuery.Parameters.Add("@DOCUMENTOCERTID", SqlDbType.Int, documentocertid);


            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;

        }

        public bool possuiDocumentoGerado(int documentocertid)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @"SELECT COUNT(0) EXISTE 
                                         FROM [CertificacaoEscolar].[DOCUMENTOGERADO] 
                                         WHERE 
                                              DOCUMENTOCERTID=@DOCUMENTOCERTID ";

            //verificar o aluno,modalidade,nivel
            contextQuery.Parameters.Add("@DOCUMENTOCERTID", SqlDbType.Int, documentocertid);


            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;

        }

        public void AtualizaPessoa(DataContext contexto, decimal pessoaCorreta, decimal pessoaErrada, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE CertificacaoEscolar.DOCUMENTOCERTIFICACAO
	                                    SET PESSOA = @PESSOACORRETA,
	                                    DATAALTERACAO = @DATAALTERACAO,
	                                    USUARIOID = @USUARIO
                                    WHERE PESSOA = @PESSOAERRADA
										AND NOT EXISTS (SELECT TOP 1 1 
												FROM CERTIFICACAOESCOLAR.DOCUMENTOCERTIFICACAO D
												WHERE D.PESSOA = @PESSOACORRETA
													AND D.TIPOCONCLUSAOID = CERTIFICACAOESCOLAR.DOCUMENTOCERTIFICACAO.TIPOCONCLUSAOID
													AND D.DOCUMENTOID = CERTIFICACAOESCOLAR.DOCUMENTOCERTIFICACAO.DOCUMENTOID  )  ";

            contextQuery.Parameters.Add("@PESSOACORRETA", pessoaCorreta);
            contextQuery.Parameters.Add("@USUARIO", usuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);
            contextQuery.Parameters.Add("@PESSOAERRADA", pessoaErrada);

            contexto.ApplyModifications(contextQuery);
        }

        public void RemovePessoa(DataContext contexto, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE CertificacaoEscolar.DOCUMENTOGERADO
                                    WHERE DOCUMENTOCERTID = (SELECT DOCUMENTOCERTID
					                                    FROM CertificacaoEscolar.DOCUMENTOCERTIFICACAO
							                                    WHERE PESSOA = @PESSOA)

                                    DELETE CertificacaoEscolar.DOCUMENTOCERTIFICACAO
                                           WHERE PESSOA = @PESSOA  ";

            contextQuery.Parameters.Add("@PESSOA", pessoa);

            contexto.ApplyModifications(contextQuery);
        }

        public bool PossuiDocumentoCertificacaoPessoaPor(DataContext ctx, Entidades.DocumentoCertificacao DocumentoCert)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @"SELECT COUNT(0) EXISTE 
                                         FROM [CertificacaoEscolar].[DOCUMENTOCERTIFICACAO] 
                                         WHERE 
                                             DOCUMENTOID=@DOCUMENTOID and 
                                             TIPOCONCLUSAOID=@TIPOCONCLUSAOID and
                                             PESSOA=@PESSOA ";

            //verificar o aluno,modalidade,nivel
            contextQuery.Parameters.Add("@PESSOA", SqlDbType.VarChar, DocumentoCert.Pessoa);
            contextQuery.Parameters.Add("@DOCUMENTOID", SqlDbType.Int, DocumentoCert.DocumentoId);
            contextQuery.Parameters.Add("@TIPOCONCLUSAOID", SqlDbType.Int, DocumentoCert.TipoConclusaoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;

        }

        public int ObtemSequencialPor(string unidade, int tipoDocumento, int documentoCertId)
        {
            int sequencial = 0;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();

            try
            {
                //Verifica se ja tem sequencial criado
                sequencial = this.ObtemSequencialPor(ctx, unidade, tipoDocumento, documentoCertId);
                
                //Caso não tenha busca o proximo
                if (sequencial == 0)
                {
                    sequencial = this.ObtemSequencialPor(ctx, unidade, tipoDocumento);
                }

                return sequencial;
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
        }

        public int ObtemSequencialPor(DataContext ctx, string unidade, int tipoDocumento)
        {
            int quantidade = 0;
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"select isnull(MAX(SEQUENCIAL), 0) CONTADOR
                                            from [CertificacaoEscolar].[DOCUMENTOCERTIFICACAO]
                                            where CENSO = @CENSO
                                            and DOCUMENTOID = @TIPODOCUMENTO ";

                contextQuery.Parameters.Add("@CENSO", unidade);
                contextQuery.Parameters.Add("@TIPODOCUMENTO", tipoDocumento);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    quantidade = Convert.ToInt32(reader["CONTADOR"]) + 1;
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.RecursosHumanos
{
    public class TipoDocumentoConcurso
    {
        public DataTable ListaPor(string concurso)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"  SELECT  TIPODOCUMENTOCONCURSOID,
                                                   TDC.TIPODOCUMENTOID,
                                                   TD.DESCRICAO, 
                                                   ANEXO,
                                                   TDC.USUARIOID,
                                                   TDC.DATACADASTRO,
                                                   TDC.DATAALTERACAO 
                                            FROM RecursosHumanos.TIPODOCUMENTOCONCURSO (NOLOCK) TDC
                                            INNER JOIN  RecursosHumanos.TIPODOCUMENTO (NOLOCK) TD ON TD.TIPODOCUMENTOID = TDC.TIPODOCUMENTOID 
                                            WHERE CONCURSO = @CONCURSO                                               
                                            ORDER BY DESCRICAO ";

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

        public ValidacaoDados Valida(Entidades.TipoDocumentoConcurso tipoDocumentoConcurso, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (tipoDocumentoConcurso == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (tipoDocumentoConcurso.TipoDocumentoConcursoId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (tipoDocumentoConcurso.TipoDocumentoId <= 0)
            {
                mensagens.Add("Campo DOCUMENTO é obrigatório.");
            }

            if (tipoDocumentoConcurso.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se já existe a descrição cadastrada
                    //if (this.PossuiOutraDescricaoCadastradaPor(contexto, tipoDocumentoConcurso., tipoDocumentoConcurso.TipoDocumentoId))
                    //{
                    //    mensagens.Add("Esta DESCRIÇÃO já foi utilizada.");
                    //}
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

        private bool PossuiOutraDescricaoCadastradaPor(DataContext ctx, string descricao, int TIPODOCUMENTOId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM RecursosHumanos.TIPODOCUMENTO (NOLOCK)
                                WHERE DESCRICAO = @DESCRICAO
	                                AND TIPODOCUMENTOID <> @TIPODOCUMENTOID ";

            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, descricao);
            contextQuery.Parameters.Add("@TIPODOCUMENTOID", SqlDbType.Int, TIPODOCUMENTOId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiTipoDocumentoPor(DataContext contexto, int tipoDocumentoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                    FROM RecursosHumanos.TIPODOCUMENTOCONCURSO
                                    WHERE TIPODOCUMENTOID = @TIPODOCUMENTOID ";

            contextQuery.Parameters.Add("@TIPODOCUMENTOID", SqlDbType.Int, tipoDocumentoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.TipoDocumentoConcurso tipoDocumentoConcurso)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO RecursosHumanos.TIPODOCUMENTOCONCURSO
                                                        (TIPODOCUMENTOID, 
                                                         CONCURSO,
                                                         ANEXO, 
                                                         USUARIOID, 
                                                         DATACADASTRO, 
                                                         DATAALTERACAO) 
                                            VALUES      (@TIPODOCUMENTOID, 
                                                         @CONCURSO,
                                                         @ANEXO, 
                                                         @USUARIOID, 
                                                         @DATACADASTRO, 
                                                         @DATAALTERACAO) ";

                contextQuery.Parameters.Add("@TIPODOCUMENTOID", SqlDbType.Int, tipoDocumentoConcurso.TipoDocumentoId);
                contextQuery.Parameters.Add("@CONCURSO", SqlDbType.VarChar, tipoDocumentoConcurso.Concurso);
                contextQuery.Parameters.Add("@ANEXO", SqlDbType.Bit, tipoDocumentoConcurso.Anexo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, tipoDocumentoConcurso.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                ctx.ApplyModifications(contextQuery);
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

        public void Atualiza(Entidades.TipoDocumentoConcurso tipoDocumentoConcurso)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE RecursosHumanos.TIPODOCUMENTOCONCURSO                                        SET   
                                               ANEXO = @ANEXO, 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  TIPODOCUMENTOCONCURSOID = @TIPODOCUMENTOCONCURSOID ";

                contextQuery.Parameters.Add("@ANEXO", SqlDbType.Bit, tipoDocumentoConcurso.Anexo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, tipoDocumentoConcurso.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@TIPODOCUMENTOCONCURSOID", SqlDbType.Int, tipoDocumentoConcurso.TipoDocumentoConcursoId);

                ctx.ApplyModifications(contextQuery);
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

        public ValidacaoDados ValidaRemocao(int tipoDocumentoId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (tipoDocumentoId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se motivo ja foi utilizado
                    //if (rnFornecedorAnalise.PossuiTIPODOCUMENTOPor(contexto, tipoDocumentoId))
                    //{
                    //    mensagens.Add("Este motivo não pode ser excluído, pois já foi utilizado para uma análise de Fornecedor.");
                    //}
                }
                catch
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw;
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

        public void Remove(int tipoDocumentoConcursoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE RecursosHumanos.TIPODOCUMENTOCONCURSO
                            WHERE  TIPODOCUMENTOCONCURSOID = @TIPODOCUMENTOCONCURSOID  ";

                contextQuery.Parameters.Add("@TIPODOCUMENTOCONCURSOID", SqlDbType.Int, tipoDocumentoConcursoId);

                ctx.ApplyModifications(contextQuery);
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
    }
}

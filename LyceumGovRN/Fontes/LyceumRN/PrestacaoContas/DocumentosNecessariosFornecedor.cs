using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class DocumentosNecessariosFornecedor : RNBase
    {
        public enum Periodicidade
        {
            [StringValue("Sem Periodicidade")]
            SemPeriodicidade = 0,
            [StringValue("Mensal")]
            Mensal = 1,
            [StringValue("Bimestral")]
            Bimestral = 2,
            [StringValue("Trimestral")]
            Trimestral = 3,
            [StringValue("Semestal")]
            Semestal = 6,
            [StringValue("Anual")]
            Anual = 12,
        }

        public DataTable Lista()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT  DOCUMENTOSNECESSARIOSFORNECEDORID, 
		                                        DESCRICAO, 	
                                                PERIODICIDADE,
                                                DATAINICIO,
                                                DATAFIM,
                                                TIPO,	                                     
		                                        ATIVO, 
		                                        USUARIOID, 
		                                        DATACADASTRO, 
		                                        DATAALTERACAO
                                        FROM PrestacaoContas.DOCUMENTOSNECESSARIOSFORNECEDOR (NOLOCK)
                                        ORDER BY DESCRICAO ";

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

        public ValidacaoDados Valida(Entidades.DocumentosNecessariosFornecedor documentosNecessariosFornecedor, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (documentosNecessariosFornecedor == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (documentosNecessariosFornecedor.DocumentosNecessariosFornecedorId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (documentosNecessariosFornecedor.Descricao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo DESCRIÇÃO é obrigatório.");
            }

            if (documentosNecessariosFornecedor.Periodicidade < 0)
            {
                mensagens.Add("Campo PERIODICIDADE é obrigatório.");
            }
            else if (documentosNecessariosFornecedor.Periodicidade != (int)Periodicidade.SemPeriodicidade
                && documentosNecessariosFornecedor.Periodicidade != (int)Periodicidade.Mensal
                && documentosNecessariosFornecedor.Periodicidade != (int)Periodicidade.Bimestral
                && documentosNecessariosFornecedor.Periodicidade != (int)Periodicidade.Trimestral
                && documentosNecessariosFornecedor.Periodicidade != (int)Periodicidade.Semestal
                && documentosNecessariosFornecedor.Periodicidade != (int)Periodicidade.Anual)
            {
                mensagens.Add("Campo PERIODICIDADE deve ser igual a Anual, Mensal, Bimestral, Trimestral ou Semestral ou Sem Periodicidade.");
            }

            if (documentosNecessariosFornecedor.Tipo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TIPO FORNECEDOR é obrigatório.");
            }
            else if (documentosNecessariosFornecedor.Tipo != "Pessoa Física" && documentosNecessariosFornecedor.Tipo != "Pessoa Jurídica")
            {
                mensagens.Add("Campo TIPO FORNECEDOR deve ser 'Pessoa Física' ou 'Pessoa Jurídica'.");
            }

            if (documentosNecessariosFornecedor.DataInicio == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA INÍCIO é obrigatório.");
            }
            else if (documentosNecessariosFornecedor.DataFim != null && documentosNecessariosFornecedor.DataFim != DateTime.MinValue)
            {
                if (documentosNecessariosFornecedor.DataInicio > documentosNecessariosFornecedor.DataFim)
                {
                    mensagens.Add("A DATA INÍCIO não pode ser inferior a DATA FIM.");
                }
            }            

            if (documentosNecessariosFornecedor.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se já existe a descrição cadastrada
                    if (this.PossuiOutraDescricaoCadastradaPor(contexto, documentosNecessariosFornecedor.Descricao, documentosNecessariosFornecedor.DocumentosNecessariosFornecedorId, documentosNecessariosFornecedor.Tipo))
                    {
                        mensagens.Add("Esta DESCRIÇÃO já foi utilizada.");
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

        private bool PossuiOutraDescricaoCadastradaPor(DataContext ctx, string descricao, int documentosNecessariosFornecedorId, string tipo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM PrestacaoContas.DOCUMENTOSNECESSARIOSFORNECEDOR (NOLOCK)
                                WHERE DESCRICAO = @DESCRICAO
                                    AND TIPO = @TIPO
	                                AND DOCUMENTOSNECESSARIOSFORNECEDORID <> @DOCUMENTOSNECESSARIOSFORNECEDORID ";

            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, descricao);
            contextQuery.Parameters.Add("@TIPO", SqlDbType.VarChar, tipo);
            contextQuery.Parameters.Add("@DOCUMENTOSNECESSARIOSFORNECEDORID", SqlDbType.Int, documentosNecessariosFornecedorId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.DocumentosNecessariosFornecedor documentosNecessariosFornecedor)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();            
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO PrestacaoContas.DOCUMENTOSNECESSARIOSFORNECEDOR
                                                        (DESCRICAO, 
                                                         PERIODICIDADE,
                                                         DATAINICIO,
                                                         DATAFIM,
                                                         TIPO,	
                                                         ATIVO, 
                                                         USUARIOID, 
                                                         DATACADASTRO, 
                                                         DATAALTERACAO) 
                                            VALUES      (@DESCRICAO, 
                                                         @PERIODICIDADE,
                                                         @DATAINICIO,
                                                         @DATAFIM,
                                                         @TIPO,	
                                                         @ATIVO, 
                                                         @USUARIOID, 
                                                         @DATACADASTRO, 
                                                         @DATAALTERACAO) ";

                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, documentosNecessariosFornecedor.Descricao);
                contextQuery.Parameters.Add("@PERIODICIDADE", SqlDbType.VarChar, documentosNecessariosFornecedor.Periodicidade);
                contextQuery.Parameters.Add("@TIPO", SqlDbType.VarChar, documentosNecessariosFornecedor.Tipo);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, documentosNecessariosFornecedor.DataInicio);

                if (documentosNecessariosFornecedor.DataFim == null || documentosNecessariosFornecedor.DataFim == DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, DBNull.Value);
                }
                else
                {
                    contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, documentosNecessariosFornecedor.DataFim);
                }

                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, documentosNecessariosFornecedor.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, documentosNecessariosFornecedor.UsuarioId);
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

        public void Atualiza(Entidades.DocumentosNecessariosFornecedor documentosNecessariosFornecedor)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE PrestacaoContas.DOCUMENTOSNECESSARIOSFORNECEDOR
                                        SET    DESCRICAO = @DESCRICAO, 
                                               PERIODICIDADE = @PERIODICIDADE,
                                               DATAINICIO = @DATAINICIO,
                                               DATAFIM = @DATAFIM,
                                               TIPO = @TIPO,	
                                               ATIVO = @ATIVO, 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  DOCUMENTOSNECESSARIOSFORNECEDORID = @DOCUMENTOSNECESSARIOSFORNECEDORID ";

                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, documentosNecessariosFornecedor.Descricao);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, documentosNecessariosFornecedor.Ativo);
                contextQuery.Parameters.Add("@PERIODICIDADE", SqlDbType.VarChar, documentosNecessariosFornecedor.Periodicidade);
                contextQuery.Parameters.Add("@TIPO", SqlDbType.VarChar, documentosNecessariosFornecedor.Tipo);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, documentosNecessariosFornecedor.DataInicio);

                if (documentosNecessariosFornecedor.DataFim == null || documentosNecessariosFornecedor.DataFim == DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, DBNull.Value);
                }
                else
                {
                    contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, documentosNecessariosFornecedor.DataFim);
                }
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, documentosNecessariosFornecedor.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DOCUMENTOSNECESSARIOSFORNECEDORID", SqlDbType.Int, documentosNecessariosFornecedor.DocumentosNecessariosFornecedorId);

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

        public ValidacaoDados ValidaRemocao(int documentosNecessariosFornecedorId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            DocumentosFornecedor rnDocumentosFornecedor = new DocumentosFornecedor();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (documentosNecessariosFornecedorId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se motivo ja foi utilizado
                    if (rnDocumentosFornecedor.PossuiDocumentosNecessariosFornecedorPor(contexto, documentosNecessariosFornecedorId))
                    {
                        mensagens.Add("Este documento não pode ser excluído, pois já foi utilizado para um fornecedor.");
                    }                   
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

        public void Remove(int documentosNecessariosFornecedorId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE PrestacaoContas.DOCUMENTOSNECESSARIOSFORNECEDOR
                            WHERE  DOCUMENTOSNECESSARIOSFORNECEDORID = @DOCUMENTOSNECESSARIOSFORNECEDORID  ";

                contextQuery.Parameters.Add("@DOCUMENTOSNECESSARIOSFORNECEDORID", SqlDbType.Int, documentosNecessariosFornecedorId);

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

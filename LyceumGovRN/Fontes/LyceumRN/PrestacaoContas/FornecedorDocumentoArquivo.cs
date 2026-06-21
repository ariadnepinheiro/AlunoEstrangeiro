using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using System.Data.SqlClient;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class FornecedorDocumentoArquivo
    {
        public byte[] ObtemArquivoPor(int fornecedorDocumentoId)
        {
            DataContext contexto = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" SELECT ARQUIVO, 
	                                               TIPOARQUIVO, 
                                                   NOMEARQUIVO 
                                            FROM   PrestacaoContas.FORNECEDORDOCUMENTOARQUIVO (NOLOCK) 
                                            WHERE DOCUMENTOSFORNECEDORID = @DOCUMENTOSFORNECEDORID ";

                contextQuery.Parameters.Add("@DOCUMENTOSFORNECEDORID", SqlDbType.Int, fornecedorDocumentoId);

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

        public ValidacaoDados Valida(Entidades.FornecedorDocumentoArquivo fornecedorDocumentoArquivo)
        {
            var validacaoDados = new ValidacaoDados();
            var mensagens = new List<string>();

            if (fornecedorDocumentoArquivo.DocumentosFornecedorId <= 0)
            {
                mensagens.Add("Campo DOCUMENTOSFORNECEDORID é obrigatório.");
            }

            if (fornecedorDocumentoArquivo.Arquivo == null || fornecedorDocumentoArquivo.Arquivo.Count() <= 0)
            {
                mensagens.Add("Campo ARQUIVO é obrigatório.");
            }

            if (fornecedorDocumentoArquivo.TipoArquivo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TIPO ARQUIVO é obrigatório.");
            }
            else
            {
                //Apenas aceitar pdf 
                if (fornecedorDocumentoArquivo.TipoArquivo.ToUpper() != "APPLICATION/PDF")
                {
                    mensagens.Add("Apenas serão aceitos arquivos do tipo .pdf .");
                }
            }

            //Verifica tamanho do arquivo - documentos com até 2 MB
            int tamanhoByte = Buffer.ByteLength(fornecedorDocumentoArquivo.Arquivo);
            if (tamanhoByte > 2097152) //2MB
            {
                mensagens.Add("Os arquivos devem ter tamanho com até 2 MB.");
            }

            if (fornecedorDocumentoArquivo.NomeArquivo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NOME ARQUIVO é obrigatório.");
            }

            if (fornecedorDocumentoArquivo.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUÁRIO é obrigatório.");
            }                    


            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

//        public void Insere(DataContext contexto, Entidades.FornecedorDocumentoArquivo fornecedorDocumentoArquivo)
//        {
//            try
//            {
//                ContextQuery contextQuery = new ContextQuery();

//                contextQuery.Command = @"  INSERT INTO PrestacaoContas.FORNECEDORDOCUMENTOARQUIVO
//                                               (DOCUMENTOSFORNECEDORID 
//                                               ,CHAVEARQUIVO
//                                               ,ARQUIVO
//                                               ,TIPOARQUIVO
//                                               ,NOMEARQUIVO
//                                               ,USUARIOID
//                                               ,DATACADASTRO
//                                               ,DATAALTERACAO)
//                                         VALUES
//                                               (@DOCUMENTOSFORNECEDORID, 
//                                               NEWID(), 
//                                               @ARQUIVO,
//                                               @TIPOARQUIVO, 
//                                               @NOMEARQUIVO, 
//                                               @USUARIOID, 
//                                               @DATACADASTRO, 
//                                               @DATAALTERACAO) 
//
//                                        SELECT IDENT_CURRENT('PRESTACAOCONTAS.FORNECEDORDOCUMENTOARQUIVO') 
//
//                    declare @FORNECEDORID int
//                    select @FORNECEDORID = FORNECEDORID from PRESTACAOCONTAS.DOCUMENTOSFORNECEDOR 
//                    WHERE  DOCUMENTOSFORNECEDORID = @DOCUMENTOSFORNECEDORID
//
//                    update PrestacaoContas.FORNECEDOR set FINALIZADO = 0 where FORNECEDORID = @FORNECEDORID
//                ";

//                contextQuery.Parameters.Add("@DOCUMENTOSFORNECEDORID", SqlDbType.Int, fornecedorDocumentoArquivo.DocumentosFornecedorId);
//                contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, fornecedorDocumentoArquivo.Arquivo);
//                contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, fornecedorDocumentoArquivo.TipoArquivo);
//                contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, fornecedorDocumentoArquivo.NomeArquivo);
//                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, fornecedorDocumentoArquivo.UsuarioId);
//                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
//                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

//                fornecedorDocumentoArquivo.FornecedorDocumentoArquivoId = contexto.GetReturnValue<int>(contextQuery);
//            }
//            catch (Exception ex)
//            {
//                string mensagem = string.Empty;
//                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
//                {
//                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
//                        Environment.NewLine,
//                        Convert.ToString(ex.Message));
//                }
//                else
//                {
//                    mensagem = Convert.ToString(ex.Message);
//                }
//                throw new Exception(mensagem);
//            }
//        }

//        public void Atualiza(DataContext ctx, Entidades.FornecedorDocumentoArquivo fornecedorDocumentoArquivo)
//        {
//            try
//            {
//                ContextQuery contextQuery = new ContextQuery();

//                contextQuery.Command = @"  UPDATE prestacaocontas.FORNECEDORDOCUMENTOARQUIVO 
//                                            SET    ARQUIVO = @ARQUIVO, 
//                                                   TIPOARQUIVO = @TIPOARQUIVO, 
//                                                   NOMEARQUIVO = @NOMEARQUIVO, 
//                                                   USUARIOID = @USUARIOID, 
//                                                   DATAALTERACAO = @DATAALTERACAO 
//                                            WHERE  DOCUMENTOSFORNECEDORID = @DOCUMENTOSFORNECEDORID 
//
//                    declare @FORNECEDORID int
//                    select @FORNECEDORID = FORNECEDORID from PRESTACAOCONTAS.DOCUMENTOSFORNECEDOR 
//                    WHERE  DOCUMENTOSFORNECEDORID = @DOCUMENTOSFORNECEDORID
//
//                    update PrestacaoContas.FORNECEDOR set FINALIZADO = 0 where FORNECEDORID = @FORNECEDORID
//                ";

//                contextQuery.Parameters.Add("@DOCUMENTOSFORNECEDORID", SqlDbType.Int, fornecedorDocumentoArquivo.DocumentosFornecedorId);
//                contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, fornecedorDocumentoArquivo.Arquivo);
//                contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, fornecedorDocumentoArquivo.TipoArquivo);
//                contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, fornecedorDocumentoArquivo.NomeArquivo);
//                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, fornecedorDocumentoArquivo.UsuarioId);
//                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

//                ctx.ApplyModifications(contextQuery);
//            }
//            catch (Exception ex)
//            {
//                string mensagem = string.Empty;
//                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
//                {
//                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
//                        Environment.NewLine,
//                        Convert.ToString(ex.Message));
//                }
//                else
//                {
//                    mensagem = Convert.ToString(ex.Message);
//                }
//                throw new Exception(mensagem);
//            }
//        }

        public void Atualiza(Entidades.FornecedorDocumentoArquivo fornecedorDocumentoArquivo)
        {
            DataContext contexto = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingLock();

                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @"
                if not exists (select top 1 1 from PrestacaoContas.FORNECEDORDOCUMENTOARQUIVO where DOCUMENTOSFORNECEDORID = @DOCUMENTOSFORNECEDORID)
                begin
	                insert into PrestacaoContas.FORNECEDORDOCUMENTOARQUIVO (
		                DOCUMENTOSFORNECEDORID
		                ,CHAVEARQUIVO
		                ,ARQUIVO
		                ,TIPOARQUIVO
		                ,NOMEARQUIVO
		                ,USUARIOID
		                ,DATACADASTRO
		                ,DATAALTERACAO
	                ) values (
		                @DOCUMENTOSFORNECEDORID
		                ,@CHAVEARQUIVO
		                ,@ARQUIVO
		                ,@TIPOARQUIVO
		                ,@NOMEARQUIVO
		                ,@USUARIOID
		                ,@DATACADASTRO
		                ,@DATAALTERACAO
	                )
                end
                else
                begin
	                update PrestacaoContas.FORNECEDORDOCUMENTOARQUIVO set
	                ARQUIVO = @ARQUIVO
	                ,TIPOARQUIVO = @TIPOARQUIVO
	                ,NOMEARQUIVO = @NOMEARQUIVO
	                ,USUARIOID = @USUARIOID	              
	                ,DATAALTERACAO = @DATAALTERACAO
	                where DOCUMENTOSFORNECEDORID = @DOCUMENTOSFORNECEDORID
                end

                declare @FORNECEDORID int
                select @FORNECEDORID = FORNECEDORID from PRESTACAOCONTAS.DOCUMENTOSFORNECEDOR 
                WHERE  DOCUMENTOSFORNECEDORID = @DOCUMENTOSFORNECEDORID

                update PrestacaoContas.FORNECEDOR set FINALIZADO = 0 where FORNECEDORID = @FORNECEDORID

                select FORNECEDORDOCUMENTOARQUIVOID from PrestacaoContas.FORNECEDORDOCUMENTOARQUIVO where DOCUMENTOSFORNECEDORID = @DOCUMENTOSFORNECEDORID
                ";

                contextQuery.Parameters.Add("@DOCUMENTOSFORNECEDORID", SqlDbType.Int, fornecedorDocumentoArquivo.DocumentosFornecedorId);
                contextQuery.Parameters.Add("@CHAVEARQUIVO", SqlDbType.UniqueIdentifier, new Guid(fornecedorDocumentoArquivo.ChaveArquivo));
                contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.Binary, fornecedorDocumentoArquivo.Arquivo);
                contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, fornecedorDocumentoArquivo.TipoArquivo);
                contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, fornecedorDocumentoArquivo.NomeArquivo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, fornecedorDocumentoArquivo.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, fornecedorDocumentoArquivo.DataCadastro);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, fornecedorDocumentoArquivo.DataAlteracao);

                fornecedorDocumentoArquivo.FornecedorDocumentoArquivoId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));

                InsereAuditoria(contexto, fornecedorDocumentoArquivo, "ATUALIZA", System.Web.HttpContext.Current.Request.UserHostAddress);
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

        public void InsereAuditoria(DataContext contexto, Entidades.FornecedorDocumentoArquivo fornecedorDocumentoArquivo, string operacao, string estacao)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @"  INSERT INTO Poseidon.PrestacaoContas.FORNECEDORDOCUMENTOARQUIVO
                                               (FORNECEDORDOCUMENTOARQUIVOID
                                               ,DOCUMENTOSFORNECEDORID
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
                                               (@FORNECEDORDOCUMENTOARQUIVOID, 
                                               @DOCUMENTOSFORNECEDORID,
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

                                        SELECT IDENT_CURRENT('PRESTACAOCONTAS.FORNECEDORDOCUMENTO') ";

                contextQuery.Parameters.Add("@FORNECEDORDOCUMENTOARQUIVOID", SqlDbType.Int, fornecedorDocumentoArquivo.FornecedorDocumentoArquivoId);
                contextQuery.Parameters.Add("@DOCUMENTOSFORNECEDORID", SqlDbType.Int, fornecedorDocumentoArquivo.DocumentosFornecedorId);
                contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, fornecedorDocumentoArquivo.Arquivo);
                contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, fornecedorDocumentoArquivo.TipoArquivo);
                contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, fornecedorDocumentoArquivo.NomeArquivo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, fornecedorDocumentoArquivo.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAAUDITORIA", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@OPERACAO", SqlDbType.VarChar, operacao);
                contextQuery.Parameters.Add("@ESTACAO", SqlDbType.VarChar, estacao);

                contexto.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
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
        }       
    }
}

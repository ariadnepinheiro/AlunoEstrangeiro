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
    public class ExigenciaExtratoArquivo : IExtratoBancarioArquivo
    {
        public byte[] ObtemArquivoPor(int exigenciaExtratoId)
        {
            DataContext contexto = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" SELECT ARQUIVO, 
	                                               TIPOARQUIVO, 
                                                   NOMEARQUIVO 
                                            FROM   PrestacaoContas.EXIGENCIAEXTRATOARQUIVO (NOLOCK) 
                                            WHERE EXIGENCIAEXTRATOID = @EXIGENCIAEXTRATOID ";

                contextQuery.Parameters.Add("@EXIGENCIAEXTRATOID", SqlDbType.Int, exigenciaExtratoId);

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

        public DataTable ObtemExtratoBancario(int mesInicio, int mesFim, int ano, string unidadeEnsino)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                ContextQuery query = new ContextQuery();

                query.Command = @"
                    select * from PrestacaoContas.EXTRATOBANCARIO eb
                    left join PrestacaoContas.EXTRATOBANCARIOARQUIVO eba on eba.EXTRATOBANCARIOID = eb.EXTRATOBANCARIOID
                    where eb.MES BETWEEN @MESINICIO AND @MESFIM
                    and eb.ANO = @ANO
                    and eb.CENSO = @UNIDADEENSINO
                ";

                query.Parameters.Add("@MESINICIO", SqlDbType.Int, mesInicio);
                query.Parameters.Add("@MESFIM", SqlDbType.Int, mesFim);
                query.Parameters.Add("@ANO", SqlDbType.Int, ano);
                query.Parameters.Add("@UNIDADEENSINO", SqlDbType.VarChar, unidadeEnsino);

                return ctx.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ValidacaoDados Valida(Entidades.ExigenciaExtratoArquivo exigenciaExtratoArquivo)
        {
            var validacaoDados = new ValidacaoDados();
            var mensagens = new List<string>();

            if (exigenciaExtratoArquivo.Arquivo.Length <= 1)
                mensagens.Add("ARQUIVO: preenchimento obrigatório");

            if (!new string[] { "image/jpeg", "application/pdf" }.Contains(exigenciaExtratoArquivo.TipoArquivo))
                mensagens.Add("ARQUIVO: suporta somente arquivos JPG e PDF");

            if (exigenciaExtratoArquivo.Arquivo.Length > 15728640)
                mensagens.Add("ARQUIVO: suporta somente arquivos de 15 MB");

            if (exigenciaExtratoArquivo.UsuarioId.IsNullOrEmptyOrWhiteSpace())
                mensagens.Add("USUÁRIO ID: Preenchimento obrigatório");

            if (exigenciaExtratoArquivo.UsuarioId.Length > 15)
                mensagens.Add("USUÁRIO ID: Não pode ter mais do que 15 caracteres");

            //if (mensagens.Count == 0)
            //{
            //    DataContext contexto = null;

            //    try
            //    {
            //        contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            //        //escreva a sua validação aqui!
            //    }
            //    catch (Exception ex)
            //    {
            //        if (contexto != null)
            //        {
            //            contexto.Abandon();
            //        }
            //        throw new Exception(ex.Message);
            //    }
            //    finally
            //    {
            //        if (contexto != null)
            //        {
            //            contexto.Dispose();
            //        }
            //    }
            //}

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

        public void Atualiza(Entidades.ExigenciaExtratoArquivo exigenciaExtratoArquivo)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    ContextQuery contextQuery = new ContextQuery();

                    contextQuery.Command = @"
                    if not exists (select top 1 1 from PrestacaoContas.EXIGENCIAEXTRATOARQUIVO where EXIGENCIAEXTRATOID = @EXIGENCIAEXTRATOID)
                    begin
	                    insert into PrestacaoContas.EXIGENCIAEXTRATOARQUIVO (
		                    EXIGENCIAEXTRATOID
		                    ,CHAVEARQUIVO
		                    ,ARQUIVO
		                    ,TIPOARQUIVO
		                    ,NOMEARQUIVO
		                    ,USUARIOID
		                    ,DATACADASTRO
		                    ,DATAALTERACAO
	                    ) values (
		                    @EXIGENCIAEXTRATOID
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
	                    update PrestacaoContas.EXIGENCIAEXTRATOARQUIVO set
	                     ARQUIVO = @ARQUIVO
	                    ,TIPOARQUIVO = @TIPOARQUIVO
	                    ,NOMEARQUIVO = @NOMEARQUIVO
	                    ,USUARIOID = @USUARIOID
	                    ,DATAALTERACAO = @DATAALTERACAO
	                    where EXIGENCIAEXTRATOID = @EXIGENCIAEXTRATOID
                    end

                    select EXIGENCIAEXTRATOARQUIVOID from PrestacaoContas.EXIGENCIAEXTRATOARQUIVO where EXIGENCIAEXTRATOID = @EXIGENCIAEXTRATOID
                    ";

                    contextQuery.Parameters.Add("@EXIGENCIAEXTRATOID", SqlDbType.Int, exigenciaExtratoArquivo.ExigenciaExtratoId);
                    contextQuery.Parameters.Add("@CHAVEARQUIVO", SqlDbType.UniqueIdentifier, new Guid(exigenciaExtratoArquivo.ChaveArquivo));
                    contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.Binary, exigenciaExtratoArquivo.Arquivo);
                    contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, exigenciaExtratoArquivo.TipoArquivo);
                    contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, exigenciaExtratoArquivo.NomeArquivo);
                    contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, exigenciaExtratoArquivo.UsuarioId);
                    contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, exigenciaExtratoArquivo.DataCadastro);
                    contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, exigenciaExtratoArquivo.DataAlteracao);

                    exigenciaExtratoArquivo.ExigenciaExtratoArquivoId = Convert.ToInt32(ctx.GetReturnValue(contextQuery));

                    InsereAuditoria(ctx, exigenciaExtratoArquivo, "ATUALIZA", System.Web.HttpContext.Current.Request.UserHostAddress);
                }
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

        public void InsereAuditoria(DataContext contexto, Entidades.ExigenciaExtratoArquivo exigenciaExtratoArquivo, string operacao, string estacao)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @"  INSERT INTO Poseidon.PrestacaoContas.EXIGENCIAEXTRATOARQUIVO
                                               (EXIGENCIAEXTRATOARQUIVOID
                                               ,EXIGENCIAEXTRATOID
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
                                               (@EXIGENCIAEXTRATOARQUIVOID, 
                                               @EXIGENCIAEXTRATOID,
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

                                        SELECT IDENT_CURRENT('PRESTACAOCONTAS.EXIGENCIAEXTRATOARQUIVO') ";

                contextQuery.Parameters.Add("@EXIGENCIAEXTRATOARQUIVOID", SqlDbType.Int, exigenciaExtratoArquivo.ExigenciaExtratoArquivoId);
                contextQuery.Parameters.Add("@EXIGENCIAEXTRATOID", SqlDbType.Int, exigenciaExtratoArquivo.ExigenciaExtratoId);
                contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, exigenciaExtratoArquivo.Arquivo);
                contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, exigenciaExtratoArquivo.TipoArquivo);
                contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, exigenciaExtratoArquivo.NomeArquivo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, exigenciaExtratoArquivo.UsuarioId);
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
using System;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using System.Collections.Generic;
using System.Linq;

namespace Techne.Lyceum.RN.Cadastros
{
    public class MaeFormularioBancoArquivo
    {
        public byte[] ObtemArquivoPor(int maeInscricaoId)
        {
            DataContext contexto = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" SELECT ARQUIVO, 
	                                               TIPOARQUIVO, 
                                                   NOMEARQUIVO 
                                            FROM   Cadastros.MAE_FORMULARIOBANCOARQUIVO (NOLOCK) 
                                            WHERE MAE_INSCRICAOID = @MAE_INSCRICAOID ";

                contextQuery.Parameters.Add("@MAE_INSCRICAOID", SqlDbType.Int, maeInscricaoId);

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

        public ValidacaoDados Valida(Entidades.MaeFormularioBancoArquivo maeFormularioBancoArquivo)
        {
            var validacaoDados = new ValidacaoDados();
            var mensagens = new List<string>();

            if (maeFormularioBancoArquivo.Arquivo.Length <= 1)
                mensagens.Add("ARQUIVO: preenchimento obrigatório");

            if (!new string[] { "image/jpeg", "application/pdf" }.Contains(maeFormularioBancoArquivo.TipoArquivo))
                mensagens.Add("ARQUIVO: suporta somente arquivos JPG e PDF");

            if (maeFormularioBancoArquivo.Arquivo.Length > 15728640)
                mensagens.Add("ARQUIVO: suporta somente arquivos de 15 MB");

            if (maeFormularioBancoArquivo.UsuarioId.IsNullOrEmptyOrWhiteSpace())
                mensagens.Add("USUÁRIO ID: Preenchimento obrigatório");

            if (maeFormularioBancoArquivo.UsuarioId.Length > 15)
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

        public void Atualiza(Entidades.MaeFormularioBancoArquivo maeFormularioBancoArquivo)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    ContextQuery contextQuery = new ContextQuery();

                    contextQuery.Command = @"
                    if not exists (select top 1 1 from Cadastros.MAE_FORMULARIOBANCOARQUIVO where MAE_INSCRICAOID = @MAE_INSCRICAOID)
                    begin
	                    insert into Cadastros.MAE_FORMULARIOBANCOARQUIVO (
		                    MAE_INSCRICAOID
		                    ,CHAVEARQUIVO
		                    ,ARQUIVO
		                    ,TIPOARQUIVO
		                    ,NOMEARQUIVO
		                    ,USUARIOID
		                    ,DATACADASTRO
		                    ,DATAALTERACAO
	                    ) values (
		                    @MAE_INSCRICAOID
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
	                    update Cadastros.MAE_FORMULARIOBANCOARQUIVO set
	                    CHAVEARQUIVO = @CHAVEARQUIVO
	                    ,ARQUIVO = @ARQUIVO
	                    ,TIPOARQUIVO = @TIPOARQUIVO
	                    ,NOMEARQUIVO = @NOMEARQUIVO
	                    ,USUARIOID = @USUARIOID
	                    ,DATACADASTRO = @DATACADASTRO
	                    ,DATAALTERACAO = @DATAALTERACAO
	                    where MAE_INSCRICAOID = @MAE_INSCRICAOID
                    end

                    select MAE_FORMULARIOBANCOARQUIVOID from Cadastros.MAE_FORMULARIOBANCOARQUIVO where MAE_INSCRICAOID = @MAE_INSCRICAOID
                    ";

                    contextQuery.Parameters.Add("@MAE_INSCRICAOID", SqlDbType.Int, maeFormularioBancoArquivo.MaeInscricaoId);
                    contextQuery.Parameters.Add("@CHAVEARQUIVO", SqlDbType.UniqueIdentifier, new Guid(maeFormularioBancoArquivo.ChaveArquivo));
                    contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.Binary, maeFormularioBancoArquivo.Arquivo);
                    contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, maeFormularioBancoArquivo.TipoArquivo);
                    contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, maeFormularioBancoArquivo.NomeArquivo);
                    contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, maeFormularioBancoArquivo.UsuarioId);
                    contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, maeFormularioBancoArquivo.DataCadastro);
                    contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, maeFormularioBancoArquivo.DataAlteracao);

                    maeFormularioBancoArquivo.MaeFormularioBancoArquivoId = Convert.ToInt32(ctx.GetReturnValue(contextQuery));

                    InsereAuditoria(ctx, maeFormularioBancoArquivo, "ATUALIZA", System.Web.HttpContext.Current.Request.UserHostAddress);
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

        private void InsereAuditoria(DataContext contexto, Entidades.MaeFormularioBancoArquivo maeFormularioBancoArquivo, string operacao, string estacao)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @"  INSERT INTO Poseidon.Cadastros.MAE_FORMULARIOBANCOARQUIVO
                                               (MAE_FORMULARIOBANCOARQUIVOID
                                               ,MAE_INSCRICAOID
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
                                               (@MAE_FORMULARIOBANCOARQUIVOID, 
                                               @MAE_INSCRICAOID,
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

                                        SELECT IDENT_CURRENT('CADASTROS.MAE_FORMULARIOBANCOARQUIVO') ";

                contextQuery.Parameters.Add("@MAE_FORMULARIOBANCOARQUIVOID", SqlDbType.Int, maeFormularioBancoArquivo.MaeFormularioBancoArquivoId);
                contextQuery.Parameters.Add("@MAE_INSCRICAOID", SqlDbType.Int, maeFormularioBancoArquivo.MaeInscricaoId);
                contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, maeFormularioBancoArquivo.Arquivo);
                contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, maeFormularioBancoArquivo.TipoArquivo);
                contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, maeFormularioBancoArquivo.NomeArquivo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, maeFormularioBancoArquivo.UsuarioId);
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

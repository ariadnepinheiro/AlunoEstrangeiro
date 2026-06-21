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
    public class OperacaoExigenciaArquivo : IOperacaoExigenciaArquivo
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
                                            FROM   PrestacaoContas.OPERACAOEXIGENCIAARQUIVO (NOLOCK) 
                                            WHERE OPERACAOEXIGENCIAID = @EXIGENCIAEXTRATOID ";

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
        public bool PossuiChave(int chave)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                    return PossuiChave(ctx, chave);
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
        public bool PossuiChave(DataContext ctx, int chave)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT COUNT(*) 
                                      FROM   PrestacaoContas.OPERACAODOCUMENTOS (NOLOCK) 
                                      WHERE  OPERACAODOCUMENTOSID = @EXTRATOBANCARIOID 
                                     ";

            contextQuery.Parameters.Add("@EXTRATOBANCARIOID", SqlDbType.Int, chave);

            return ctx.GetReturnValue<int>(contextQuery) > 0;
        }
        public ValidacaoDados Valida(Entidades.OperacaoExigenciaArquivo exigenciaExtratoArquivo)
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

    }
    public interface IOperacaoExigenciaArquivo
    {
        byte[] ObtemArquivoPor(int id);
    }
}
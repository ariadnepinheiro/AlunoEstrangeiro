using System;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class OperacaoArquivo : IOperacaoArquivo
    {
        public byte[] ObtemArquivoPor(int extratoBancarioId)
        {
            DataContext contexto = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" SELECT ARQUIVO, 
	                                               TIPOARQUIVO, 
                                                   NOMEARQUIVO 
                                            FROM   PrestacaoContas.OPERACAODOCUMENTOS (NOLOCK) 
                                            WHERE OPERACAODOCUMENTOSID = @EXTRATOBANCARIOID ";

                contextQuery.Parameters.Add("@EXTRATOBANCARIOID", SqlDbType.Int, extratoBancarioId);

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
    }


    public interface IOperacaoArquivo
    {
        byte[] ObtemArquivoPor(int id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.Certificacao
{
    public class EnccejaDocumentoArquivo
    {
        public byte[] ObtemArquivoPor(int DocumentoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            byte[] arquivo = null;

            try
            {
                contextQuery.Command = @" SELECT ARQUIVO, 
	                                               TIPOARQUIVO, 
                                                   NOMEARQUIVO 
                                            FROM   certificacaoescolar.ENCCEJADOCUMENTOARQUIVO (NOLOCK) 
                                            WHERE ENCCEJADOCUMENTOARQUIVOID = @ENCCEJADOCUMENTOARQUIVOID ";

                contextQuery.Parameters.Add("@ENCCEJADOCUMENTOARQUIVOID", SqlDbType.Int, DocumentoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    arquivo = (byte[])reader["ARQUIVO"];
                }

                return arquivo;
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
                if (reader != null)
                {
                    reader.Close();
                }
                contexto.Dispose();
            }
        }

        public DataTable ObtemListaPor(int enccejaRequerimentoId, int tipoDocumentoEnccejaId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT  ENCCEJADOCUMENTOARQUIVOID,
		                                        TIPODOCUMENTOENCCEJAID,
		                                        TIPOARQUIVO,
		                                        NOMEARQUIVO			
                                        FROM [CERTIFICACAOESCOLAR].[ENCCEJADOCUMENTOARQUIVO]  
                                        WHERE ENCCEJAREQUERIMENTOID = @ENCCEJAREQUERIMENTOID
		                                        AND TIPODOCUMENTOENCCEJAID = @TIPODOCUMENTOENCCEJAID ";

                contextQuery.Parameters.Add("@ENCCEJAREQUERIMENTOID", SqlDbType.Int, enccejaRequerimentoId);
                contextQuery.Parameters.Add("@TIPODOCUMENTOENCCEJAID", SqlDbType.Int, tipoDocumentoEnccejaId);
                
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
    }
}

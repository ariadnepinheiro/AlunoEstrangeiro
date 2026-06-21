using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.Patrimonio
{
    public class Classificacao
    {
        public int RetornaVidaUtilVigentePor(int classificacaoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int retorno = 0;

            try
            {
                contextQuery.Command = @" SELECT VIDAUTIL
                            FROM   PATRIMONIO.CLASSIFICACAOVIGENCIA  (NOLOCK) 
                            WHERE CLASSIFICACAOID = @CLASSIFICACAOID
	                            AND DATAINICIO <= GETDATE()
                                    AND ( DATAFIM IS NULL 
                                            OR DATAFIM <= GETDATE() ) ";

                contextQuery.Parameters.Add("@CLASSIFICACAOID", SqlDbType.Int, classificacaoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["VIDAUTIL"]);
                }

                return retorno;
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

        public string RetornaContaPor(int classificacaoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            string retorno = string.Empty;

            try
            {
                contextQuery.Command = @" SELECT CONTA
                            FROM   PATRIMONIO.CLASSIFICACAO  (NOLOCK) 
                            WHERE CLASSIFICACAOID = @CLASSIFICACAOID
	                             ";

                contextQuery.Parameters.Add("@CLASSIFICACAOID", SqlDbType.Int, classificacaoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToString(reader["CONTA"]);
                }

                return retorno;
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

        public decimal RetornaValorResidualVigentePor(DataContext contexto, int classificacaoId)
        {
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            decimal retorno = 0;

            try
            {
                contextQuery.Command = @" SELECT TAXAVALORRESIDUAL
                            FROM   PATRIMONIO.CLASSIFICACAOVIGENCIA  (NOLOCK) 
                            WHERE CLASSIFICACAOID = @CLASSIFICACAOID
	                            AND DATAINICIO <= GETDATE()
                                    AND ( DATAFIM IS NULL 
                                            OR DATAFIM <= GETDATE() ) ";

                contextQuery.Parameters.Add("@CLASSIFICACAOID", SqlDbType.Int, classificacaoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToDecimal(reader["TAXAVALORRESIDUAL"]);
                }

                return retorno;
            }            
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public decimal RetornaValorResidualVigentePor(DataContext contexto, int classificacaoId, DateTime dataConsulta)
        {
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            decimal retorno = 0;

            try
            {
                contextQuery.Command = @" SELECT TAXAVALORRESIDUAL
                            FROM   PATRIMONIO.CLASSIFICACAOVIGENCIA  (NOLOCK) 
                            WHERE CLASSIFICACAOID = @CLASSIFICACAOID
	                            AND DATAINICIO <= @DATACONSULTA
                                    AND ( DATAFIM IS NULL 
                                            OR DATAFIM <= @DATACONSULTA ) ";

                contextQuery.Parameters.Add("@CLASSIFICACAOID", SqlDbType.Int, classificacaoId);
                contextQuery.Parameters.Add("@DATACONSULTA", SqlDbType.Date, dataConsulta.Date);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToDecimal(reader["TAXAVALORRESIDUAL"]);
                }

                return retorno;
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

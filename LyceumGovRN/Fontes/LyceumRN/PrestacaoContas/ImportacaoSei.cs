using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class ImportacaoSei
    {
        public bool PossuiPeriodoReferenciaPor(DataContext contexto, int periodoReferenciaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM PrestacaoContas.IMPORTACAOSEI (NOLOCK)
                                    WHERE PERIODOREFERENCIAID = @PERIODOREFERENCIAID ";

            contextQuery.Parameters.Add("@PERIODOREFERENCIAID", SqlDbType.Int, periodoReferenciaId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiImportacaoSeiPor(DataContext contexto, int ano, int mes, string censo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @"   SELECT COUNT(*)
                                        FROM PRESTACAOCONTAS.PERIODOREFERENCIA P
	                                        INNER JOIN PRESTACAOCONTAS.IMPORTACAOSEI I ON P.PERIODOREFERENCIAID = I.PERIODOREFERENCIAID
                                        WHERE P.ANO = @ANO
	                                        AND @MES BETWEEN P.MESINICIAL AND P.MESFINAL
                                            AND I.CENSO = @CENSO ";

            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@MES", SqlDbType.Int, mes);
            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(string censo, int periodoReferenciaId, string usuarioId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO PrestacaoContas.IMPORTACAOSEI
                                                   (PERIODOREFERENCIAID
                                                   ,CENSO
                                                   ,USUARIOID
                                                   ,DATAIMPORTACAO)
                                             VALUES
                                                   (@PERIODOREFERENCIAID, 
                                                   @CENSO, 
                                                   @USUARIOID,
                                                   @DATAIMPORTACAO) ";

                contextQuery.Parameters.Add("@PERIODOREFERENCIAID", SqlDbType.Int, periodoReferenciaId);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
                contextQuery.Parameters.Add("@DATAIMPORTACAO", SqlDbType.DateTime, DateTime.Now);

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

        public DateTime ObtemUltimaImportacaoPor(int periodoReferenciaId, string censo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;            
            DateTime data = DateTime.MinValue;

            try
            {
                contextQuery.Command = @" SELECT max(DATAIMPORTACAO) as DATAIMPORTACAO
                                        FROM PrestacaoContas.IMPORTACAOSEI
                                        WHERE CENSO = @CENSO
                                            AND PERIODOREFERENCIAID = @PERIODOREFERENCIAID ";

                contextQuery.Parameters.Add("@PERIODOREFERENCIAID", SqlDbType.Int, periodoReferenciaId);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    data = reader["DATAIMPORTACAO"] != DBNull.Value ? Convert.ToDateTime(reader["DATAIMPORTACAO"]) : DateTime.MinValue;

                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return data;
        }
    }
}

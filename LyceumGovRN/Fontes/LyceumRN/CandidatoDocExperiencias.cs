using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Techne.Lyceum.RN.Entidades;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.RecursosHumanos.DTO;
using System.Xml;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Globalization;
using System.Configuration;
namespace Techne.Lyceum.RN
{
    public class CandidatoDocExperiencias
    {
        public static void RemoverExperiencia(DataContext dataContext, string concurso, string candidato)
        {
            var contextQuery = new ContextQuery
            {
                Command =
                    @"DELETE dbo.LY_CANDIDATO_DOC_EXPERIENCIAS
                                        WHERE CONCURSO = @CONCURSO AND CANDIDATO=@CANDIDATO"
            };

            contextQuery.Parameters.Add("@CONCURSO", concurso);
            contextQuery.Parameters.Add("@CANDIDATO", candidato);

            dataContext.ApplyModifications(contextQuery);

        }
        public DataTable ListaCandidatoExperiencia(string concurso, string candidato)
        {
            var ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                return ListaCandidatoExperiencia(ctx, concurso, candidato);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ctx.Dispose();
            }
        }
        private DataTable ListaCandidatoExperiencia(DataContext ctx, string concurso, string candidato)
        {
            SqlDataReader reader = null;
            DataTable dt = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"   SELECT ct.CONCURSO, 
		                                            dt.DOCENTECANDIDATOID as CANDIDATO, 
		                                            T.DESCRICAO as EXPERIENCIA, 
		                                            CT.PONTUACAO as pontuacao
                                            FROM [RECURSOSHUMANOS].[DOCENTECANDIDATOEXPERIENCIA] DT
	                                            INNER JOIN [DBO].[LY_CONCURSO_DOC_EXPERIENCIA] CT ON DT.CONCURSO = CT.CONCURSO
													                                            AND DT.EXPERIENCIA = CT.EXPERIENCIA
	                                            INNER JOIN [LY_CONCURSO_EXPERIENCIA] T ON CT.EXPERIENCIA = T.EXPERIENCIA                 
                                            where DT.concurso = @CONCURSO and dt.DOCENTECANDIDATOID=@CANDIDATO
                ";
   
                contextQuery.Parameters.Add("@CONCURSO", SqlDbType.VarChar, concurso);
                contextQuery.Parameters.Add("@CANDIDATO", SqlDbType.VarChar, candidato);
                dt = ctx.GetDataTable(contextQuery);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public int Insere(LyCandidatoDocExperiencias dadosCandidato)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();
            int ret = int.MinValue;

            try
            {
                contextQuery.Command = @"INSERT INTO LYCEUM.dbo.LY_CANDIDATO_DOC_EXPERIENCIAS(CONCURSO, CANDIDATO, EXPERIENCIA) VALUES(@CONCURSO, @CANDIDATO, @EXPERIENCIA) ";

                contextQuery.Parameters.Add("@CONCURSO", dadosCandidato.Concurso);
                contextQuery.Parameters.Add("@CANDIDATO", dadosCandidato.Candidato);
                contextQuery.Parameters.Add("@EXPERIENCIA", dadosCandidato.Experiencia);

                ret = ctx.ApplyModifications(contextQuery);
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

            return ret;
        }       
    }
}

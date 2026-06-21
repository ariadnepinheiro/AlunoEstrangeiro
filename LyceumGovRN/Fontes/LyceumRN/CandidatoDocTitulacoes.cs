using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.RecursosHumanos.DTO;
using System.Xml;
using Seeduc.Infra.Data;
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
    public class CandidatoDocTitulacoes : RNBase
    {
        public int Insere(LyCandidatoDocTitulacoes dadosCandidato)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();
            int ret = int.MinValue;

            try
            {
                contextQuery.Command = @"INSERT INTO LYCEUM.dbo.LY_CANDIDATO_DOC_TITULACOES(CONCURSO, CANDIDATO, TITULACAO) VALUES(@CONCURSO, @CANDIDATO, @TITULACAO) ";

                contextQuery.Parameters.Add("@CONCURSO", dadosCandidato.Concurso);
                contextQuery.Parameters.Add("@CANDIDATO", dadosCandidato.Candidato);
                contextQuery.Parameters.Add("@TITULACAO", dadosCandidato.Titulacao);

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

        public static void RemoverTitulacao(DataContext dataContext, string concurso, string candidato)
        {
            var contextQuery = new ContextQuery(
                @"DELETE dbo.LY_CANDIDATO_DOC_TITULACOES
                                        WHERE CONCURSO = @CONCURSO AND CANDIDATO=@CANDIDATO");

            contextQuery.Parameters.Add("@CONCURSO", concurso);
            contextQuery.Parameters.Add("@CANDIDATO", candidato);

            dataContext.ApplyModifications(contextQuery);
        }

        public DataTable ListaCandidatoTitulacoes(string concurso, string candidato)
        {
            var ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                return ListaCandidatoTitulacoes(ctx, concurso, candidato);
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
    
        private DataTable ListaCandidatoTitulacoes(DataContext ctx, string concurso, string candidato)
        {
          //  List<DocenteCandidatoTitulacao> dados = new List<DocenteCandidatoTitulacao>();
            SqlDataReader reader = null;
            DataTable dt = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"   SELECT DT.CONCURSO, 
		                                            DT.DOCENTECANDIDATOID as CANDIDATO, 
		                                            T.DESCRICAO as TITULACAO, 
		                                            CT.PONTUACAO as pontuacao
                                            FROM [RECURSOSHUMANOS].[DOCENTECANDIDATOTITULACAO] DT
	                                            INNER JOIN [DBO].[LY_CONCURSO_DOC_TITULACOES] CT ON DT.CONCURSO = CT.CONCURSO
													                                            AND DT.TITULACAO = CT.TITULACAO
	                                            INNER JOIN [LY_CONCURSO_TITULACAO] T ON CT.TITULACAO = T.TITULACAO
                                            WHERE  DT.CONCURSO =  @CONCURSO
                                            AND DOCENTECANDIDATOID = @CANDIDATO
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

    }

}

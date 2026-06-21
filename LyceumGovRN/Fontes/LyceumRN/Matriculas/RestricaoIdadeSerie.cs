using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data.SqlClient;
using System.Data;

namespace Techne.Lyceum.RN.Matriculas
{
    public class RestricaoIdadeSerie
    {
        public Entidades.RestricaoIdadeSerie ObtemPor(DataContext contexto, int controleVagaId)
        {
            Entidades.RestricaoIdadeSerie eRestricaoIdadeSerie = new Entidades.RestricaoIdadeSerie();
            SqlDataReader dataReader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"    SELECT RS.ID_RESTRICAO_IDADE_SERIE, 
                                       RS.CURSO, 
                                       RS.SERIE, 
                                       RS.IDADE_MINIMA, 
                                       RS.IDADE_MAXIMA, 
                                       RS.DT_CADASTRO, 
                                       RS.DT_ALTERACAO 
                                FROM   TCE_RESTRICAO_IDADE_SERIE RS (NOLOCK) 
                                       INNER JOIN TCE_CONTROLE_VAGA CV (NOLOCK) 
                                               ON RS.CURSO = CV.CURSO 
                                                  AND RS.SERIE = CV.SERIE 
                                WHERE  CV.ID_CONTROLE_VAGA = @ID_CONTROLE_VAGA  ";

                contextQuery.Parameters.Add("@ID_CONTROLE_VAGA", SqlDbType.Int, controleVagaId);

                dataReader = contexto.GetDataReader(contextQuery);

                while (dataReader.Read())
                {
                    eRestricaoIdadeSerie.IdRestricaoIdadeSerie = Convert.ToInt32(dataReader["ID_RESTRICAO_IDADE_SERIE"]);
                    eRestricaoIdadeSerie.Curso = Convert.ToString(dataReader["CURSO"]);
                    eRestricaoIdadeSerie.Serie = Convert.ToInt32(dataReader["SERIE"]);
                    eRestricaoIdadeSerie.IdadeMinima = Convert.ToInt32(dataReader["IDADE_MINIMA"]);
                    eRestricaoIdadeSerie.IdadeMaxima = Convert.ToInt32(dataReader["IDADE_MAXIMA"]);
                    eRestricaoIdadeSerie.DtCadastro = Convert.ToDateTime(dataReader["DT_CADASTRO"]);
                    eRestricaoIdadeSerie.DtAlteracao = dataReader["DT_ALTERACAO"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["DT_ALTERACAO"]);
                }

                return eRestricaoIdadeSerie;
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                }
            }
        }
    }
}

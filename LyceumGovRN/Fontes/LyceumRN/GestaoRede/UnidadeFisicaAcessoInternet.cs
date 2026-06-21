using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.GestaoRede
{
    public class UnidadeFisicaAcessoInternet
    {
        public List<int> ListaAcessoInternetPor(DataContext contexto, string unidadeFisica)
        {
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            List<int> listaAcessoInternet = new List<int>();
            try
            {
                contextQuery.Command = @" SELECT DISTINCT ACESSOINTERNETID
                                            FROM GESTAOREDE.UNIDADEFISICA_ACESSOINTERNET (NOLOCK)
                                            WHERE UNIDADEFISICAID = @UNIDADEFISICAID ";

                contextQuery.Parameters.Add("@UNIDADEFISICAID", SqlDbType.VarChar, unidadeFisica);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    listaAcessoInternet.Add(Convert.ToInt32(reader["AcessoInternetID"]));
                }

                return listaAcessoInternet;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public void Insere(DataContext contexto, string unidadeFisica, int acessoInternetId, string usuarioResponsavel)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO GestaoRede.UNIDADEFISICA_ACESSOINTERNET
                                               (ACESSOINTERNETID
                                               ,UNIDADEFISICAID
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO)
                                         VALUES
                                              (@ACESSOINTERNETID, 
                                               @UNIDADEFISICAID,
                                               @USUARIOID,
                                               @DATACADASTRO,
                                               @DATAALTERACAO) ";

            contextQuery.Parameters.Add("@ACESSOINTERNETID", SqlDbType.Int, acessoInternetId);
            contextQuery.Parameters.Add("@UNIDADEFISICAID", SqlDbType.VarChar, unidadeFisica);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioResponsavel);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void RemovePorUnidade(DataContext contexto, string unidadeFisica)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE GestaoRede.UNIDADEFISICA_ACESSOINTERNET
                                      WHERE UNIDADEFISICAID = @UNIDADEFISICAID ";

            contextQuery.Parameters.Add("@UNIDADEFISICAID", SqlDbType.VarChar, unidadeFisica);

            contexto.ApplyModifications(contextQuery);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.GestaoRede
{
    public class UnidadeFisicaMaterialPedagogico
    {
        public List<int> ListaMaterialPedagogicoPor(DataContext contexto, string unidadeFisica)
        {
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            List<int> listaOrgaoColegiado = new List<int>();
            try
            {
                contextQuery.Command = @" SELECT DISTINCT MATERIALPEDAGOGICOID
                                            FROM GESTAOREDE.UNIDADEFISICA_MATERIALPEDAGOGICO (NOLOCK)
                                            WHERE UNIDADEFISICAID = @UNIDADEFISICAID ";

                contextQuery.Parameters.Add("@UNIDADEFISICAID", SqlDbType.VarChar, unidadeFisica);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    listaOrgaoColegiado.Add(Convert.ToInt32(reader["MATERIALPEDAGOGICOID"]));
                }

                return listaOrgaoColegiado;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public void Insere(DataContext contexto, string unidadeFisica, int materialPedagogicoId, string usuarioResponsavel)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO GestaoRede.UNIDADEFISICA_MATERIALPEDAGOGICO
                                               (MATERIALPEDAGOGICOID
                                               ,UNIDADEFISICAID
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO)
                                         VALUES
                                              (@MATERIALPEDAGOGICOID, 
                                               @UNIDADEFISICAID,
                                               @USUARIOID,
                                               @DATACADASTRO,
                                               @DATAALTERACAO) ";

            contextQuery.Parameters.Add("@MATERIALPEDAGOGICOID", SqlDbType.Int, materialPedagogicoId);
            contextQuery.Parameters.Add("@UNIDADEFISICAID", SqlDbType.VarChar, unidadeFisica);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioResponsavel);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void RemovePorUnidade(DataContext contexto, string unidadeFisica)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE GestaoRede.UNIDADEFISICA_MATERIALPEDAGOGICO
                                      WHERE UNIDADEFISICAID = @UNIDADEFISICAID ";

            contextQuery.Parameters.Add("@UNIDADEFISICAID", SqlDbType.VarChar, unidadeFisica);

            contexto.ApplyModifications(contextQuery);
        }
    }
}

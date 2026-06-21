using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.GestaoRede
{
    public class UnidadeFisicaFormasAcessibilidade
    {
        public List<int> ListaFormasAcessibilidadePor(DataContext contexto, string unidadeFisica)
        {
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            List<int> listaFormasAcesso = new List<int>();
            try
            {
                contextQuery.Command = @" SELECT DISTINCT FORMASACESSIBILIDADEID
                                            FROM GESTAOREDE.UNIDADEFISICA_FORMASACESSIBILIDADE (NOLOCK)
                                            WHERE UNIDADEFISICAID = @UNIDADEFISICAID ";

                contextQuery.Parameters.Add("@UNIDADEFISICAID", SqlDbType.VarChar, unidadeFisica);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    listaFormasAcesso.Add(Convert.ToInt32(reader["FORMASACESSIBILIDADEID"]));
                }

                return listaFormasAcesso;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public void Insere(DataContext contexto, string unidadeFisica, int formasAcessibilidadeId, string usuarioResponsavel)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO GestaoRede.UNIDADEFISICA_FORMASACESSIBILIDADE
                                               (FORMASACESSIBILIDADEID
                                               ,UNIDADEFISICAID
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO)
                                         VALUES
                                              (@FORMASACESSIBILIDADEID, 
                                               @UNIDADEFISICAID,
                                               @USUARIOID,
                                               @DATACADASTRO,
                                               @DATAALTERACAO) ";

            contextQuery.Parameters.Add("@FORMASACESSIBILIDADEID", SqlDbType.Int, formasAcessibilidadeId);
            contextQuery.Parameters.Add("@UNIDADEFISICAID", SqlDbType.VarChar, unidadeFisica);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioResponsavel);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void RemovePorUnidade(DataContext contexto, string unidadeFisica)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE GestaoRede.UNIDADEFISICA_FORMASACESSIBILIDADE
                                      WHERE UNIDADEFISICAID = @UNIDADEFISICAID ";

            contextQuery.Parameters.Add("@UNIDADEFISICAID", SqlDbType.VarChar, unidadeFisica);

            contexto.ApplyModifications(contextQuery);
        }
    }
}

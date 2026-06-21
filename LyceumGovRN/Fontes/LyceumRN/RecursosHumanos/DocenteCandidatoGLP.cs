using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.RecursosHumanos
{
    public class DocenteCandidatoGLP : RNBase
    {       

        public void Insere(DataContext contexto, Entidades.DocenteCandidatoGLP docenteCandidatoGLP)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  INSERT INTO RecursosHumanos.DOCENTECANDIDATOGLP
                                           (DOCENTECANDIDATOID
                                            ,ANO
                                           ,USUARIOID
                                           ,DATACADASTRO
                                           ,DATAALTERACAO)
                                     VALUES
                                           (@DOCENTECANDIDATOID                                         
                                           ,@ANO
                                           ,@USUARIOID
                                           ,@DATACADASTRO
                                           ,@DATAALTERACAO) ";

            contextQuery.Parameters.Add("@DOCENTECANDIDATOID", SqlDbType.Int, docenteCandidatoGLP.DocenteCandidatoId);
            contextQuery.Parameters.Add("@ANO", SqlDbType.VarChar, docenteCandidatoGLP.Ano);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, docenteCandidatoGLP.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void Remove(DataContext contexto, int docenteCandidatoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  DELETE RecursosHumanos.DOCENTECANDIDATOGLP                                            
                                            WHERE DOCENTECANDIDATOID = @DOCENTECANDIDATOID
                                                  ";

            contextQuery.Parameters.Add("@DOCENTECANDIDATOID", SqlDbType.Int, docenteCandidatoId);
            

            contexto.ApplyModifications(contextQuery);
        }


        public List<int> ListaAnosGLPPor(DataContext contexto, int docenteCandidatoId)
        {
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            List<int> listaAnos = new List<int>();
            try
            {
                contextQuery.Command = @" SELECT DISTINCT ANO
                                            FROM  RecursosHumanos.DOCENTECANDIDATOGLP  
                                            WHERE DOCENTECANDIDATOID = @DOCENTECANDIDATOID ";

                contextQuery.Parameters.Add("@DOCENTECANDIDATOID", SqlDbType.Int, docenteCandidatoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    listaAnos.Add(Convert.ToInt32(reader["ANO"]));
                }

                return listaAnos;
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

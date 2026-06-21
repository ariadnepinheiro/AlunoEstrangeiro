using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.Ocorrencias
{
    public class OcorrenciaArma: RNBase
    {
        public enum TipoArma 
        {
            [StringValue("Branca")]
            Branca = 1,
            [StringValue("Fogo")]
            Fogo = 2,
            [StringValue("Artefato")]
            Artefato = 3
        }

        public List<int> ObtemListaPor(DataContext contexto, int ocorrenciaId)
        {
            List<int> armas = new List<int>();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT TIPOARMA
                                        FROM Ocorrencias.OCORRENCIAARMA
                                        WHERE OCORRENCIAID = @OCORRENCIAID ";

                contextQuery.Parameters.Add("@OCORRENCIAID", SqlDbType.Int, ocorrenciaId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    int tipoArma = Convert.ToInt32(reader["TIPOARMA"]);
                    armas.Add(tipoArma);
                }

                return armas;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public void Insere(DataContext contexto, int ocorrenciaId, int tipoArmaId, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Ocorrencias.OCORRENCIAARMA
                                               (OCORRENCIAID
                                               ,TIPOARMA
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO)
                                         VALUES
                                               (@OCORRENCIAID, 
                                               @TIPOARMA, 
                                               @USUARIOID, 
                                               @DATACADASTRO, 
                                               @DATAALTERACAO) ";

            contextQuery.Parameters.Add("@OCORRENCIAID", SqlDbType.Int, ocorrenciaId);
            contextQuery.Parameters.Add("@TIPOARMA", SqlDbType.Int, tipoArmaId);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void RemoveTodosPor(DataContext contexto, int ocorrenciaId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE Ocorrencias.OCORRENCIAARMA
                                      WHERE OCORRENCIAID = @OCORRENCIAID ";

            contextQuery.Parameters.Add("@OCORRENCIAID", SqlDbType.Int, ocorrenciaId);

            contexto.ApplyModifications(contextQuery);
        }
    }
}

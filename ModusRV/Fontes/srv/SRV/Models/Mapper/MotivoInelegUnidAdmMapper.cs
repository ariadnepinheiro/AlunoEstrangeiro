using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;

namespace SRV.Models.Mapper
{
    public class MotivoInelegUnidAdmMapper : BaseMapper<MotivoInelegibilidade>
    {
        protected override string QueryFindObject()
        {
            throw new NotImplementedException();
        }

        protected override string QueryListObjects()
        {
            return @"SELECT -1 as id_motivo_inelegibilidade, b.des_motivo_inelegibilidade AS motivo
                       FROM rv_motivo_ineleg_unidadm a, rv_motivo_inelegibilidade b
                      WHERE a.id_unidade_administrativa = @idUnidade
                        AND a.id_ano_referencia = @idAnoReferencia
                        AND b.id_motivo_inelegibilidade = a.id_motivo_inelegibilidade";
        }

        public override MotivoInelegibilidade LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            MotivoInelegibilidade motivoInelegibilidade = new MotivoInelegibilidade();

            motivoInelegibilidade.IdMotivoInelegibilidade = Convert.ToInt32(reader["id_motivo_inelegibilidade"]);
            motivoInelegibilidade.DesMotivoInelegibilidade = reader["motivo"].ToString();

            return motivoInelegibilidade;
        }

        public IList<MotivoInelegibilidade> List(int idUnidadeAdministrativa, int idAnoReferencia)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("idUnidade", idUnidadeAdministrativa);
            param.Add("idAnoReferencia", idAnoReferencia);

            return ListObjects(param);
        }

    }
}
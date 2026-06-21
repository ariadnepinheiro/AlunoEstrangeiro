using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using System.Text;

namespace SRV.Models.Mapper
{
    public class MotivoInelegDocenteMapper : BaseMapper<MotivoInelegibilidade>
    {
        protected override string QueryFindObject()
        {
            throw new NotImplementedException();
        }

        protected override string QueryListObjects()
        {
            return @"SELECT mi.id_motivo_inelegibilidade, mi.des_motivo_inelegibilidade AS motivo
                       FROM rv_motivo_ineleg_docente mid, rv_motivo_inelegibilidade mi
                      WHERE mid.id_servidor = @idServidor
                        AND mid.id_ano_referencia = @idAnoReferencia
                        AND mid.id_motivo_inelegibilidade = mi.id_motivo_inelegibilidade
                        AND mid.id_unidade_administrativa = @idUnidade";
        }

        public override MotivoInelegibilidade LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            MotivoInelegibilidade motivoInelegibilidade = new MotivoInelegibilidade();

            motivoInelegibilidade.IdMotivoInelegibilidade = Convert.ToInt32(reader["id_motivo_inelegibilidade"]);
            motivoInelegibilidade.DesMotivoInelegibilidade = reader["motivo"].ToString();

            return motivoInelegibilidade;
        }

        public IList<MotivoInelegibilidade> List(int idServidor, int idAnoReferencia, int idUnidade)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("idServidor", idServidor);
            param.Add("idAnoReferencia", idAnoReferencia);
            param.Add("idUnidade", idUnidade);

            return ListObjects(param);
        }
    }
}
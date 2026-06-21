using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;

namespace SRV.Models.Mapper
{
    public class MotivoInelegibilidadeMapper : BaseMapper<MotivoInelegibilidade>
    {
        protected override string QueryFindObject()
        {
            return @"SELECT id_motivo_inelegibilidade, des_motivo_inelegibilidade
                       FROM rv_motivo_inelegibilidade 
                      WHERE b.id_motivo_inelegibilidade = @idMotivo";
        }

        protected override string QueryListObjects()
        {
            throw new NotImplementedException();
        }

        public override MotivoInelegibilidade LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            MotivoInelegibilidade motivoInelegibilidade = new MotivoInelegibilidade();

            motivoInelegibilidade.IdMotivoInelegibilidade = Convert.ToInt32(reader["id_motivo_inelegibilidade"]);
            motivoInelegibilidade.DesMotivoInelegibilidade = reader["des_motivo_inelegibilidade"].ToString();

            return motivoInelegibilidade;
        }

        public IList<MotivoInelegibilidade> List()
        {
            return ListObjects();
        }

        public MotivoInelegibilidade Find(int idMotivo)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idMotivo", idMotivo);

            return FindObject(QueryFindObject(), param, LoadObject);
        }
    }
}
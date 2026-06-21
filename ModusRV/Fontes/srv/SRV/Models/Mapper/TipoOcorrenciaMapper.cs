using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;

namespace SRV.Models.Mapper
{
    public class TipoOcorrenciaMapper : BaseMapper<TipoOcorrencia>
    {
        protected override string QueryFindObject()
        {
            return @"SELECT id_tipo_ocorrencia, des_tipo_ocorrencia
                       FROM rv_tipo_ocorrencia
                      WHERE id_tipo_ocorrencia = @idTipoOcorrencia";
        }

        protected override string QueryListObjects()
        {
            return @"SELECT * FROM rv_tipo_ocorrencia ORDER BY des_tipo_ocorrencia";
        }

        private string QueryInsert()
        {
            return @"INSERT INTO rv_tipo_ocorrencia
                               (des_tipo_ocorrencia)
                        VALUES (@desTipoOcorrencia)";
        }

        private string QueryUpdate()
        {
            return @"UPDATE rv_tipo_ocorrencia
                        SET des_tipo_ocorrencia = @desTipoOcorrencia
                      WHERE id_tipo_ocorrencia = @idTipoOcorrencia";
        }

        private string QueryDelete()
        {
            return @"DELETE FROM rv_tipo_ocorrencia
                           WHERE id_tipo_ocorrencia = @idTipoOcorrencia";
        }

        public override TipoOcorrencia LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            TipoOcorrencia tipoOcorrencia = new TipoOcorrencia();

            tipoOcorrencia.IdTipoOcorrencia = (int)reader["id_tipo_ocorrencia"];
            tipoOcorrencia.DesTipoOcorrencia = (string)reader["des_tipo_ocorrencia"];

            return tipoOcorrencia;
        }

        public IList<TipoOcorrencia> List()
        {
            return ListObjects();
        }

        public TipoOcorrencia Find(int idTipoOcorrencia)
        {
            return FindObject("idTipoOcorrencia", idTipoOcorrencia);
        }

        public TipoOcorrencia Insert(TipoOcorrencia tipoOcorrencia)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("desTipoOcorrencia", tipoOcorrencia.DesTipoOcorrencia.ToUpper());

            tipoOcorrencia.IdTipoOcorrencia = InsertObjectWithIdentity(QueryInsert(), param);

            return tipoOcorrencia;
        }

        public void Update(TipoOcorrencia tipoOcorrencia)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idTipoOcorrencia", tipoOcorrencia.IdTipoOcorrencia);
            param.Add("desTipoOcorrencia", tipoOcorrencia.DesTipoOcorrencia.ToUpper());

            UpdateObject(QueryUpdate(), param);
        }

        public void Delete(int idTipoOcorrencia)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idTipoOcorrencia", idTipoOcorrencia);

            DeleteObject(QueryDelete(), param);
        }
    }
}
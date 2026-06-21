using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using SRV.Models.DTO;

namespace SRV.Models.Mapper
{
    public class OcorrenciaMapper : BaseMapper<Ocorrencia>
    {
        protected override string QueryFindObject()
        {
            return @"SELECT o.id_ocorrencia, o.des_ocorrencia,
                            t.id_tipo_ocorrencia, t.des_tipo_ocorrencia
                       FROM rv_ocorrencia o, rv_tipo_ocorrencia t
                      WHERE o.id_tipo_ocorrencia = t.id_tipo_ocorrencia
                        AND o.id_ocorrencia = @idOcorrencia";
        }

        protected override string QueryListObjects()
        {
            return @"SELECT o.id_ocorrencia, o.des_ocorrencia,
                            t.id_tipo_ocorrencia, t.des_tipo_ocorrencia
                       FROM rv_ocorrencia o, rv_tipo_ocorrencia t
                      WHERE o.id_tipo_ocorrencia = t.id_tipo_ocorrencia
                   ORDER BY des_ocorrencia";
        }

        private string QueryInsert()
        {
            return @"INSERT INTO rv_ocorrencia 
                                (des_ocorrencia,
                                 id_tipo_ocorrencia)
                         VALUES (@desOcorrencia,
                                 @idTipoOcorrencia)";
        }

        private string QueryUpdate()
        {
            return @"UPDATE rv_ocorrencia 
                        SET des_ocorrencia = @desOcorrencia,
                            id_tipo_ocorrencia = @idTipoOcorrencia
                      WHERE id_ocorrencia = @idOcorrencia";
        }

        private string QueryDelete()
        {
            return @"DELETE 
                       FROM rv_ocorrencia 
                      WHERE id_ocorrencia = @idOcorrencia";
        }

        private string QueryExisteOcorrencia()
        {
            return @"SELECT COUNT(*)
                       FROM rv_ocorrencia
                      WHERE id_ocorrencia = @idOcorrencia";
        }

        public override Ocorrencia LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            Ocorrencia ocorrencia = new Ocorrencia();

            ocorrencia.IdOcorrencia = (int)reader["id_ocorrencia"];
            ocorrencia.DesOcorrencia = (string)reader["des_ocorrencia"];

            TipoOcorrencia tipoOcorrencia = new TipoOcorrencia();
            tipoOcorrencia.IdTipoOcorrencia = (int)reader["id_tipo_ocorrencia"];
            tipoOcorrencia.DesTipoOcorrencia = (string)reader["des_tipo_ocorrencia"];
            ocorrencia.TipoOcorrencia = tipoOcorrencia;

            return ocorrencia;
        }

        public Ocorrencia Find(int id)
        {
            return FindObject("idOcorrencia", id);
        }

        public IList<Ocorrencia> List()
        {
            return ListObjects();
        }

        public Paging<Ocorrencia> List(int currentPage, int pageSize)
        {
            return ListPagingObjects(currentPage, pageSize);
        }

        public Ocorrencia Insert(Ocorrencia ocorrencia)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("id_ocorrencia", ocorrencia.IdOcorrencia);
            param.Add("desOcorrencia", ocorrencia.DesOcorrencia.ToUpper());
            param.Add("idTipoOcorrencia", ocorrencia.TipoOcorrencia.IdTipoOcorrencia);

			ocorrencia.IdOcorrencia = InsertObjectWithIdentity(QueryInsert(), param).Value;

            return ocorrencia;
        }

        public void Update(Ocorrencia ocorrencia)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idOcorrencia", ocorrencia.IdOcorrencia);
            param.Add("desOcorrencia", ocorrencia.DesOcorrencia.ToUpper());
            param.Add("idTipoOcorrencia", ocorrencia.TipoOcorrencia.IdTipoOcorrencia);

            UpdateObject(QueryUpdate(), param);
        }

        public void Delete(int idOcorrencia)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idOcorrencia", idOcorrencia);

            DeleteObject(QueryDelete(), param);
        }

        public bool ExisteOcorrencia(int idOcorrencia)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idOcorrencia", idOcorrencia);

            short cont = Convert.ToInt16(ExecuteScalarQuery(QueryExisteOcorrencia(), param));

            return cont > 0 ? true : false;
        }
    }
}
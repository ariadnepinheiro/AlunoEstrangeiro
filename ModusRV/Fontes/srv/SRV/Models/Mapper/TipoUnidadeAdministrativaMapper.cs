using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using SRV.Models.DTO;

namespace SRV.Models.Mapper
{
    public class TipoUnidadeAdministrativaMapper : BaseMapper<TipoUnidadeAdministrativa>
    {
        protected override string QueryFindObject()
        {
            return @"SELECT id_tipo_unidadm, des_tipo_unidadm 
                       FROM rv_tipo_unidadm
                      WHERE id_tipo_unidadm = @idTipoUnidAdm";
        }

        protected override string QueryListObjects()
        {
            return @"SELECT id_tipo_unidadm, des_tipo_unidadm FROM rv_tipo_unidadm ORDER BY des_tipo_unidadm";
        }

        private string QueryInsert()
        {
            return @"INSERT INTO rv_tipo_unidadm 
                                (des_tipo_unidadm)
                         VALUES (@desTipoUnidadm)";
        }

        private string QueryUpdate()
        {
            return @"UPDATE rv_tipo_unidadm
                        SET des_tipo_unidadm = @desTipoUnidadm
                      WHERE id_tipo_unidadm = @idTipoUnidAdm";
        }

        private string QueryDelete()
        {
            return @"DELETE FROM rv_tipo_unidadm
                           WHERE id_tipo_unidadm = @idTipoUnidAdm";
        }

        protected string QueryExiste()
        {
            return @"SELECT count(*)
                       FROM rv_tipo_unidadm
                      WHERE id_tipo_unidadm = @idTipoUnidAdm";
        }

        public override TipoUnidadeAdministrativa LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            TipoUnidadeAdministrativa tipoUnidadeAdministrativa = new TipoUnidadeAdministrativa();

            tipoUnidadeAdministrativa.IdTipoUnidAdm = (int)reader["id_tipo_unidadm"];
            tipoUnidadeAdministrativa.DesTipoUnidAdm = (string)reader["des_tipo_unidadm"];

            return tipoUnidadeAdministrativa;
        }

        public TipoUnidadeAdministrativa Find(int id)
        {
            return FindObject("idTipoUnidAdm", id);
        }

        public IList<TipoUnidadeAdministrativa> List()
        {
            return ListObjects();
        }

        public TipoUnidadeAdministrativa Insert(TipoUnidadeAdministrativa tipoUnidadeAdministrativa)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("desTipoUnidadm", tipoUnidadeAdministrativa.DesTipoUnidAdm.ToUpper());

            tipoUnidadeAdministrativa.IdTipoUnidAdm = InsertObjectWithIdentity(QueryInsert(), param);

            return tipoUnidadeAdministrativa;
        }

        public void Update(TipoUnidadeAdministrativa tipoUnidadeAdministrativa)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idTipoUnidAdm", tipoUnidadeAdministrativa.IdTipoUnidAdm);
            param.Add("desTipoUnidadm", tipoUnidadeAdministrativa.DesTipoUnidAdm.ToUpper());

            UpdateObject(QueryUpdate(), param);
        }

        public void Delete(int idTipoUnidAdm)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idTipoUnidAdm", idTipoUnidAdm);

            DeleteObject(QueryDelete(), param);
        }


        public bool ExisteTipoUnidadeAdministrativa(int idTipoUnidAdm)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idTipoUnidAdm", idTipoUnidAdm);

            short cont = Convert.ToInt16(ExecuteScalarQuery(QueryExiste(), param));

            return cont > 0 ? true : false;
        }

    }
}
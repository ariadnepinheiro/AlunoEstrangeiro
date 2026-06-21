using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using SRV.Models.DTO;
using System.Text;
using SRV.Common.Extension;

namespace SRV.Models.Mapper
{
    public class MetaIGEUnidadeAdministrativaMapper : BaseMapper<MetaIGEUnidadeAdministrativa>
    {
        protected override string QueryFindObject()
        {
            return @"SELECT id_unidade_administrativa, id_ano_referencia, nm_meta_ige
                       FROM rv_meta_ige_unidadm
                      WHERE id_unidade_administrativa = @idUnidadeAdministrativa
                        AND id_ano_referencia = @idAnoReferencia";
        }

        protected override string QueryListObjects()
        {
            throw new NotImplementedException();
        }

        private string QueryList(FiltroMetaIGEUnidadeAdministrativa filtro)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"SELECT ua.id_unidade_administrativa, ua.des_unidade_administrativa,
                                m.id_ano_referencia, m.nm_meta_ige
                           FROM rv_meta_ige_unidadm m,
                                rv_unidade_administrativa ua
                          WHERE m.id_unidade_administrativa = ua.id_unidade_administrativa");

            if (filtro.IdUnidadeAdministrativa != null)
                sql.Append(" AND ua.id_unidade_administrativa = @idUnidadeAdministrativa");


            sql.Append(@"   AND m.id_ano_referencia = @idAnoReferencia
                          ORDER BY ua.des_unidade_administrativa");

            return sql.ToString();
        }

        private string QueryInsert()
        {
            return @"INSERT INTO rv_meta_ige_unidadm
                                   (id_unidade_administrativa,
                                    id_ano_referencia,
                                    nm_meta_ige)
                            VALUES (@idUnidadeAdministrativa,
                                    @idAnoReferencia,
                                    @metaIge)";
        }

        private string QueryUpdate()
        {
            return @"UPDATE rv_meta_ige_unidadm
                        SET nm_meta_ige = @metaIge
                      WHERE id_unidade_administrativa = @idUnidadeAdministrativa
                        AND id_ano_referencia = @idAnoReferencia";
        }

        private string QueryDelete()
        {
            return @"DELETE FROM rv_meta_ige_unidadm
                           WHERE id_unidade_administrativa = @idUnidadeAdministrativa
                             AND id_ano_referencia = @idAnoReferencia";
        }

        private string QueryExisteMetaIge()
        {
            return @"SELECT COUNT(*)
                       FROM rv_meta_ige_unidadm
                      WHERE id_unidade_administrativa = @idUnidadeAdministrativa
                        AND id_ano_referencia = @idAnoReferencia";
        }

        public override MetaIGEUnidadeAdministrativa LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            MetaIGEUnidadeAdministrativa metaIGEUnidadeAdministrativa = new MetaIGEUnidadeAdministrativa();

            metaIGEUnidadeAdministrativa.UnidadeAdministrativa = new UnidadeAdministrativa();
            metaIGEUnidadeAdministrativa.UnidadeAdministrativa.IdUnidadeAdministrativa = Convert.ToInt32(reader["id_unidade_administrativa"]);

            if(reader.HasColumn("des_unidade_administrativa"))
                metaIGEUnidadeAdministrativa.UnidadeAdministrativa.DesUnidadeAdministrativa = (string)reader["des_unidade_administrativa"];

            metaIGEUnidadeAdministrativa.AnoReferencia = new AnoReferencia() { IdAnoReferencia = (int)reader["id_ano_referencia"] };

            metaIGEUnidadeAdministrativa.MetaIge = (decimal)reader["nm_meta_ige"];

            return metaIGEUnidadeAdministrativa;
        }

        public MetaIGEUnidadeAdministrativa Find(int idUnidadeAdministrativa, int idAnoReferencia)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idUnidadeAdministrativa", idUnidadeAdministrativa);
            param.Add("idAnoReferencia", idAnoReferencia);

            return FindObject(QueryFindObject(), param);
        }

        public Paging<MetaIGEUnidadeAdministrativa> List(FiltroMetaIGEUnidadeAdministrativa filtro, int currentPage, int pageSize)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idUnidadeAdministrativa", filtro.IdUnidadeAdministrativa);
            param.Add("idAnoReferencia", filtro.IdAnoReferencia);

            return ListPagingObjects(QueryList(filtro), param, LoadObject, currentPage, pageSize);
        }

        public void Insert(MetaIGEUnidadeAdministrativa metaIGEUnidadeAdministrativa)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idUnidadeAdministrativa", metaIGEUnidadeAdministrativa.UnidadeAdministrativa.IdUnidadeAdministrativa);
            param.Add("idAnoReferencia", metaIGEUnidadeAdministrativa.AnoReferencia.IdAnoReferencia);
            param.Add("metaIge", metaIGEUnidadeAdministrativa.MetaIge);

            InsertObject(QueryInsert(), param);
        }

        public void Update(MetaIGEUnidadeAdministrativa metaIGEUnidadeAdministrativa)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idUnidadeAdministrativa", metaIGEUnidadeAdministrativa.UnidadeAdministrativa.IdUnidadeAdministrativa);
            param.Add("idAnoReferencia", metaIGEUnidadeAdministrativa.AnoReferencia.IdAnoReferencia);
            param.Add("metaIge", metaIGEUnidadeAdministrativa.MetaIge);

            UpdateObject(QueryUpdate(), param);
        }

        public void Delete(int idUnidadeAdministrativa, int idAnoReferencia)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idUnidadeAdministrativa", idUnidadeAdministrativa);
            param.Add("idAnoReferencia", idAnoReferencia);

            DeleteObject(QueryDelete(), param);
        }

        public bool ExisteMetaIge(int idUnidadeAdministrativa, int idAnoReferencia)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idUnidadeAdministrativa", idUnidadeAdministrativa);
            param.Add("idAnoReferencia", idAnoReferencia);

            short cont = Convert.ToInt16(ExecuteScalarQuery(QueryExisteMetaIge(), param));

            return cont > 0 ? true : false;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;

namespace SRV.Models.Mapper
{
    public class TipoCriterioElegibilidadeMapper : BaseMapper<TipoCriterioElegibilidade>
    {
        protected override string QueryFindObject()
        {
            return @"SELECT tce.id_criterio_elegibilidade ,tce.id_ano_referencia,
                            tua.id_tipo_unidadm, tua.des_tipo_unidadm,
                            tce.des_criterio_elegibilidade, tce.nm_valor_criterio
                       FROM rv_tipo_criterio_elegibilidade tce,
                            rv_tipo_unidadm tua
                      WHERE tce.id_tipo_unidadm = tua.id_tipo_unidadm
                        AND tce.id_criterio_elegibilidade = @idTipoCriterioElegibilidade
                        AND tce.id_ano_referencia = @idAnoReferencia
                        AND tce.id_tipo_unidadm = @idTipoUnidadeAdm";
        }

        protected override string QueryListObjects()
        {
            return @"SELECT tce.id_criterio_elegibilidade ,tce.id_ano_referencia,
                            tua.id_tipo_unidadm, tua.des_tipo_unidadm,
                            tce.des_criterio_elegibilidade, tce.nm_valor_criterio
                       FROM rv_tipo_criterio_elegibilidade tce,
                            rv_tipo_unidadm tua
                      WHERE tce.id_tipo_unidadm = tua.id_tipo_unidadm
                        AND tce.id_ano_referencia = @idAnoReferencia
                      ORDER BY tce.des_criterio_elegibilidade, tua.des_tipo_unidadm";
        }

        private string QueryInsert()
        {
            return @"INSERT INTO rv_tipo_criterio_elegibilidade
                                   (id_ano_referencia,
                                    id_tipo_unidadm,
                                    des_criterio_elegibilidade,
                                    nm_valor_criterio)
                            VALUES (@idAnoReferencia,
                                    @idTipoUnidadeAdm,
                                    @desCriterio,
                                    @valorCriterio)";
        }

        private string QueryUpdate()
        {
            return @"UPDATE rv_tipo_criterio_elegibilidade
                        SET des_criterio_elegibilidade = @desCriterio,
                            nm_valor_criterio = @valorCriterio
                      WHERE id_criterio_elegibilidade = @idTipoCriterioElegibilidade
                        AND id_ano_referencia = @idAnoReferencia
                        AND id_tipo_unidadm = @idTipoUnidadeAdm";
        }

        private string QueryDelete()
        {
            return @"DELETE FROM rv_tipo_criterio_elegibilidade
                           WHERE id_criterio_elegibilidade = @idTipoCriterioElegibilidade
                             AND id_ano_referencia = @idAnoReferencia
                             AND id_tipo_unidadm = @idTipoUnidadeAdm";
        }

        public override TipoCriterioElegibilidade LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            TipoCriterioElegibilidade tipoCriterioElegibilidade = new TipoCriterioElegibilidade();

            tipoCriterioElegibilidade.IdTipoCriterioElegibilidade = (int)reader["id_criterio_elegibilidade"];
            tipoCriterioElegibilidade.DesTipoCriterioElegibilidade = (string)reader["des_criterio_elegibilidade"];
            tipoCriterioElegibilidade.ValorCriterio = (decimal)reader["nm_valor_criterio"];

            tipoCriterioElegibilidade.AnoReferencia = new AnoReferencia();
            tipoCriterioElegibilidade.AnoReferencia.IdAnoReferencia = (int)reader["id_ano_referencia"];

            tipoCriterioElegibilidade.TipoUnidadeAdministrativa = new TipoUnidadeAdministrativa();
            tipoCriterioElegibilidade.TipoUnidadeAdministrativa.IdTipoUnidAdm = (int)reader["id_tipo_unidadm"];
            tipoCriterioElegibilidade.TipoUnidadeAdministrativa.DesTipoUnidAdm = (string)reader["des_tipo_unidadm"];

            return tipoCriterioElegibilidade;
        }

        public TipoCriterioElegibilidade Find(int idTipoCriterioElegibilidade, int idAnoReferencia, int idTipoUnidadeAdiministrativa)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idTipoCriterioElegibilidade", idTipoCriterioElegibilidade);
            param.Add("idAnoReferencia", idAnoReferencia);
            param.Add("idTipoUnidadeAdm", idTipoUnidadeAdiministrativa);

            return FindObject(QueryFindObject(), param);
        }

        public IList<TipoCriterioElegibilidade> List(int idAnoReferencia)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idAnoReferencia", idAnoReferencia);

            return ListObjects(QueryListObjects(), param);
        }

        public TipoCriterioElegibilidade Insert(TipoCriterioElegibilidade tipoCriterioElegibilidade)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idAnoReferencia", tipoCriterioElegibilidade.AnoReferencia.IdAnoReferencia);
            param.Add("idTipoUnidadeAdm", tipoCriterioElegibilidade.TipoUnidadeAdministrativa.IdTipoUnidAdm);
            param.Add("desCriterio", tipoCriterioElegibilidade.DesTipoCriterioElegibilidade.ToUpper());
            param.Add("valorCriterio", tipoCriterioElegibilidade.ValorCriterio);

            tipoCriterioElegibilidade.IdTipoCriterioElegibilidade = InsertObjectWithIdentity(QueryInsert(), param);

            return tipoCriterioElegibilidade;
        }

        public void Update(TipoCriterioElegibilidade tipoCriterioElegibilidade)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idTipoCriterioElegibilidade", tipoCriterioElegibilidade.IdTipoCriterioElegibilidade);
            param.Add("idAnoReferencia", tipoCriterioElegibilidade.AnoReferencia.IdAnoReferencia);
            param.Add("idTipoUnidadeAdm", tipoCriterioElegibilidade.TipoUnidadeAdministrativa.IdTipoUnidAdm);
            param.Add("desCriterio", tipoCriterioElegibilidade.DesTipoCriterioElegibilidade.ToUpper());
            param.Add("valorCriterio", tipoCriterioElegibilidade.ValorCriterio);

            UpdateObject(QueryUpdate(), param);
        }

        public void Delete(int idTipoCriterioElegibilidade, int idAnoReferencia, int idTipoUnidadeAdiministrativa)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idTipoCriterioElegibilidade", idTipoCriterioElegibilidade);
            param.Add("idAnoReferencia", idAnoReferencia);
            param.Add("idTipoUnidadeAdm", idTipoUnidadeAdiministrativa);

            DeleteObject(QueryDelete(), param);
        }

		internal decimal ObtemValorPor(int idTipoCriterioElegibilidade)
		{
			Dictionary<string, object> param = new Dictionary<string, object>();

			param.Add("idTipoCriterioElegibilidade", idTipoCriterioElegibilidade);

			return Convert.ToDecimal(ExecuteScalarQuery(QueryObtemValorCriterio(), param));
		}

		private string QueryObtemValorCriterio()
		{
			return @"--SELECT ROUND(CONVERT(DECIMAL(6,2), NM_VALOR_CRITERIO/100),2) 
					  SELECT NM_VALOR_CRITERIO
					   FROM RV_TIPO_CRITERIO_ELEGIBILIDADE
					  WHERE ID_CRITERIO_ELEGIBILIDADE = @idTipoCriterioElegibilidade";
		}
	}
}
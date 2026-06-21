using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRV.Models.Domain;
using System.Data.SqlClient;
using SRV.Models.Mapper;
using SRV.Models.DTO;

namespace SRV.Models.Service
{
    public class TipoCriterioElegibilidadeService : BaseService
    {
        private readonly ModelStateDictionary modelState;

		public TipoCriterioElegibilidadeService()
		{}

		public TipoCriterioElegibilidadeService(ModelStateDictionary modelState)
        {
            this.modelState = modelState;
        }

        public TipoCriterioElegibilidade Find(int idTipoCriterioElegibilidade, int idAnoReferencia, int idTipoUnidadeAdiministrativa)
        {
            TipoCriterioElegibilidade result;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                TipoCriterioElegibilidadeMapper mapper = new TipoCriterioElegibilidadeMapper();
                mapper.connection = conn;

                result = mapper.Find(idTipoCriterioElegibilidade, idAnoReferencia, idTipoUnidadeAdiministrativa);
            }

            return result;
        }

        public IList<TipoCriterioElegibilidade> List(int idAnoReferencia)
        {
            IList<TipoCriterioElegibilidade> result;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                TipoCriterioElegibilidadeMapper mapper = new TipoCriterioElegibilidadeMapper();
                mapper.connection = conn;

                result = mapper.List(idAnoReferencia);
            }

            return result;
        }

        public TipoCriterioElegibilidade Insert(TipoCriterioElegibilidade tipoCriterioElegibilidade, UserState usuario)
        {
            if (ValidaTipoCriterioElegibilidade(tipoCriterioElegibilidade))
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        TipoCriterioElegibilidadeMapper mapper = new TipoCriterioElegibilidadeMapper();
                        mapper.connection = conn;
                        mapper.transaction = trans;

                        tipoCriterioElegibilidade = mapper.Insert(tipoCriterioElegibilidade);

                        AuditInsert(tipoCriterioElegibilidade, usuario, trans);

                        trans.Commit();
                    }
                }
            }

            return tipoCriterioElegibilidade;
        }

        public void Update(TipoCriterioElegibilidade tipoCriterioElegibilidade, UserState usuario)
        {
            if (ValidaTipoCriterioElegibilidade(tipoCriterioElegibilidade))
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        TipoCriterioElegibilidadeMapper mapper = new TipoCriterioElegibilidadeMapper();
                        mapper.connection = conn;
                        mapper.transaction = trans;

                        TipoCriterioElegibilidade tipoCriterioElegibilidadeOld = mapper.Find(tipoCriterioElegibilidade.IdTipoCriterioElegibilidade.Value, tipoCriterioElegibilidade.AnoReferencia.IdAnoReferencia.Value, tipoCriterioElegibilidade.TipoUnidadeAdministrativa.IdTipoUnidAdm.Value);

                        mapper.Update(tipoCriterioElegibilidade);

                        AuditUpdate(tipoCriterioElegibilidade, tipoCriterioElegibilidadeOld, usuario, trans);

                        trans.Commit();
                    }
                }
            }
        }

        public void Delete(int idTipoCriterioElegibilidade, int idAnoReferencia, int idTipoUnidadeAdiministrativa, UserState usuario)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    TipoCriterioElegibilidadeMapper mapper = new TipoCriterioElegibilidadeMapper();
                    mapper.connection = conn;
                    mapper.transaction = trans;

                    TipoCriterioElegibilidade tipoCriterioElegibilidadeOld = mapper.Find(idTipoCriterioElegibilidade, idAnoReferencia, idTipoUnidadeAdiministrativa);

                    mapper.Delete(idTipoCriterioElegibilidade, idAnoReferencia, idTipoUnidadeAdiministrativa);

                    AuditDelete(tipoCriterioElegibilidadeOld, usuario, trans);

                    trans.Commit();
                }
            }
        }

        private bool ValidaTipoCriterioElegibilidade(TipoCriterioElegibilidade tipoCriterioElegibilidade)
        {
            if (tipoCriterioElegibilidade.AnoReferencia == null || tipoCriterioElegibilidade.AnoReferencia.IdAnoReferencia == null)
                modelState.AddModelError("TipoCriterioElegibilidade.AnoReferencia.IdAnoReferencia", "Campo é obrigatório");

            if (tipoCriterioElegibilidade.TipoUnidadeAdministrativa == null || tipoCriterioElegibilidade.TipoUnidadeAdministrativa.IdTipoUnidAdm == null)
                modelState.AddModelError("TipoCriterioElegibilidade.TipoUnidadeAdministrativa.IdTipoUnidAdm", "Campo é obrigatório");

            if (tipoCriterioElegibilidade.DesTipoCriterioElegibilidade == null)
                modelState.AddModelError("TipoCriterioElegibilidade.DesTipoCriterioElegibilidade", "Campo é obrigatório");

            if (tipoCriterioElegibilidade.ValorCriterio == null)
                modelState.AddModelError("TipoCriterioElegibilidade.ValorCriterio", "Campo é obrigatório");

            return modelState.IsValid;
        }

		public decimal ObtemValorPor(int idTipoCriterioElegibilidade)
		{
			decimal valorCriterio;

			using (SqlConnection conn = GetConnection())
			{
				conn.Open();

				TipoCriterioElegibilidadeMapper mapper = new TipoCriterioElegibilidadeMapper();
				mapper.connection = conn;

				valorCriterio = mapper.ObtemValorPor(idTipoCriterioElegibilidade);
			}

			return valorCriterio;
		}
    }
}
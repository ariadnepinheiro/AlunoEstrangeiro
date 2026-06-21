using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using System.Data.SqlClient;
using SRV.Models.Mapper;
using SRV.Models.DTO;
using System.Web.Mvc;

namespace SRV.Models.Service
{
    public class TipoUnidadeAdministrativaService : BaseService
    {
        private readonly ModelStateDictionary modelState;

        public TipoUnidadeAdministrativaService(ModelStateDictionary modelState)
        {
            this.modelState = modelState;
        }

        public IList<TipoUnidadeAdministrativa> List()
        {
            IList<TipoUnidadeAdministrativa> result;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                TipoUnidadeAdministrativaMapper mapper = new TipoUnidadeAdministrativaMapper();
                mapper.connection = conn;

                result = mapper.List();
            }

            return result;
        }

        public TipoUnidadeAdministrativa Find(int id)
        {
            TipoUnidadeAdministrativa result;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                TipoUnidadeAdministrativaMapper mapper = new TipoUnidadeAdministrativaMapper();
                mapper.connection = conn;

                result = mapper.Find(id);
            }

            return result;
        }

        public TipoUnidadeAdministrativa Insert(TipoUnidadeAdministrativa tipoUnidadeAdministrativa, UserState usuario)
        {
            if (ValidaTipoUnidadeAdministrativa(tipoUnidadeAdministrativa))
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        TipoUnidadeAdministrativaMapper mapper = new TipoUnidadeAdministrativaMapper();
                        mapper.connection = conn;
                        mapper.transaction = trans;

                        tipoUnidadeAdministrativa = mapper.Insert(tipoUnidadeAdministrativa);

                        AuditInsert(tipoUnidadeAdministrativa, usuario, trans);

                        trans.Commit();
                    }
                }
            }

            return tipoUnidadeAdministrativa;
        }


        public void Update(TipoUnidadeAdministrativa tipoUnidadeAdministrativa, UserState usuario)
        {
            if (ValidaTipoUnidadeAdministrativa(tipoUnidadeAdministrativa))
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        TipoUnidadeAdministrativaMapper mapper = new TipoUnidadeAdministrativaMapper();
                        mapper.connection = conn;
                        mapper.transaction = trans;

                        TipoUnidadeAdministrativa tipoUnidadeAdministrativaOld = Find(tipoUnidadeAdministrativa.IdTipoUnidAdm.Value);

                        mapper.Update(tipoUnidadeAdministrativa);

                        AuditUpdate(tipoUnidadeAdministrativa, tipoUnidadeAdministrativaOld, usuario, trans);

                        trans.Commit();
                    }
                }
            }
        }

        public void Delete(int idTipoUnidadeAdministrativa, UserState usuario)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    TipoUnidadeAdministrativaMapper mapper = new TipoUnidadeAdministrativaMapper();
                    mapper.connection = conn;
                    mapper.transaction = trans;

                    TipoUnidadeAdministrativa tipoUnidadeAdministrativaOld = Find(idTipoUnidadeAdministrativa);

                    mapper.Delete(idTipoUnidadeAdministrativa);

                    AuditDelete(tipoUnidadeAdministrativaOld, usuario, trans);

                    trans.Commit();
                }
            }
        }

        private bool ValidaTipoUnidadeAdministrativa(TipoUnidadeAdministrativa tipoUnidadeAdministrativa)
        {

            if (tipoUnidadeAdministrativa.DesTipoUnidAdm == null)
                modelState.AddModelError("DesTipoUnidAdm", "Campo é obrigatório");

            return modelState.IsValid;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using System.Data.SqlClient;
using SRV.Models.Mapper;
using System.Web.Mvc;
using SRV.Models.DTO;

namespace SRV.Models.Service
{
    public class TipoOcorrenciaService : BaseService
    {
        private readonly ModelStateDictionary modelState;

        public TipoOcorrenciaService(ModelStateDictionary modelState)
        {
            this.modelState = modelState;
        }

        public IList<TipoOcorrencia> List()
        {
            IList<TipoOcorrencia> result;

            using (SqlConnection conn = GetConnection())
            {                
                conn.Open();

                TipoOcorrenciaMapper mapper = new TipoOcorrenciaMapper();
                mapper.connection = conn;

                result = mapper.List();
            }

            return result;
        }

        public TipoOcorrencia Find(int idTipoOcorrencia)
        {
            TipoOcorrencia tipoOcorrencia;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                TipoOcorrenciaMapper mapper = new TipoOcorrenciaMapper();
                mapper.connection = conn;

                tipoOcorrencia = mapper.Find(idTipoOcorrencia);
            }

            return tipoOcorrencia;
        }

        public TipoOcorrencia Insert(TipoOcorrencia tipoOcorrencia, UserState usuario)
        {
            if (ValidaTipoOcorrencia(tipoOcorrencia))
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        TipoOcorrenciaMapper mapper = new TipoOcorrenciaMapper();
                        mapper.connection = conn;
                        mapper.transaction = trans;

                        tipoOcorrencia = mapper.Insert(tipoOcorrencia);

                        AuditInsert(tipoOcorrencia, usuario, trans);

                        trans.Commit();
                    }
                }
            }

            return tipoOcorrencia;
        }

        public void Update(TipoOcorrencia tipoOcorrencia, UserState usuario)
        {
            if (ValidaTipoOcorrencia(tipoOcorrencia))
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        TipoOcorrenciaMapper mapper = new TipoOcorrenciaMapper();
                        mapper.connection = conn;
                        mapper.transaction = trans;

                        TipoOcorrencia tipoOcorrenciaOld = mapper.Find(tipoOcorrencia.IdTipoOcorrencia.Value);

                        mapper.Update(tipoOcorrencia);

                        AuditUpdate(tipoOcorrencia, tipoOcorrenciaOld, usuario, trans);

                        trans.Commit();
                    }
                }
            }
        }

        public void Delete(int idTipoOcorrencia, UserState usuario)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    TipoOcorrenciaMapper mapper = new TipoOcorrenciaMapper();
                    mapper.connection = conn;
                    mapper.transaction = trans;

                    TipoOcorrencia tipoOcorrenciaOld = mapper.Find(idTipoOcorrencia);

                    mapper.Delete(idTipoOcorrencia);

                    AuditDelete(tipoOcorrenciaOld, usuario, trans); 

                    trans.Commit();
                }
            }
        }

        private bool ValidaTipoOcorrencia(TipoOcorrencia tipoOcorrencia)
        {
            if (tipoOcorrencia.DesTipoOcorrencia == null)
                modelState.AddModelError("DesTipoOcorrencia", "Campo é obrigatório");

            return modelState.IsValid;
        }
    }
}
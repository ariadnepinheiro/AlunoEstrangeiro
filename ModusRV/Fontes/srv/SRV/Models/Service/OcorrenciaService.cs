using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using System.Data.SqlClient;
using SRV.Models.Mapper;
using SRV.Models.DTO;
using System.Web.Mvc;
using SRV.Common.Exceptions;

namespace SRV.Models.Service
{
    public class OcorrenciaService : BaseService
    {
        private readonly ModelStateDictionary modelState;

        public OcorrenciaService(){}

		public OcorrenciaService(ModelStateDictionary modelState)
        {
            this.modelState = modelState;
        }

        public IList<Ocorrencia> List()
        {
            IList<Ocorrencia> result;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                OcorrenciaMapper mapper = new OcorrenciaMapper();
                mapper.connection = conn;

                result = mapper.List();
            }

            return result;
        }

        public Paging<Ocorrencia> List(int currentPage, int pageSize)
        {
            Paging<Ocorrencia> result;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                OcorrenciaMapper mapper = new OcorrenciaMapper();
                mapper.connection = conn;

                result = mapper.List(currentPage, pageSize);
            }

            return result;
        }

        public Ocorrencia Find(int id)
        {
            Ocorrencia result;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                OcorrenciaMapper mapper = new OcorrenciaMapper();
                mapper.connection = conn;

                result = mapper.Find(id);
            }

            return result;
        }

        public Ocorrencia Insert(Ocorrencia ocorrencia, UserState usuario)
        {
            if (ValidaOcorrencia(ocorrencia, true))
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        OcorrenciaMapper mapper = new OcorrenciaMapper();
                        mapper.connection = conn;
                        mapper.transaction = trans;

                        ocorrencia = mapper.Insert(ocorrencia);

                        AuditInsert(ocorrencia, usuario, trans);

                        trans.Commit();
                    }
                }
            }
            return ocorrencia;
        }

        public void Update(Ocorrencia ocorrencia, UserState usuario)
        {
            if (ValidaOcorrencia(ocorrencia, false))
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        OcorrenciaMapper mapper = new OcorrenciaMapper();
                        mapper.connection = conn;
                        mapper.transaction = trans;

                        Ocorrencia ocorrenciaOld = Find(ocorrencia.IdOcorrencia);

                        mapper.Update(ocorrencia);

                        AuditUpdate(ocorrencia, ocorrenciaOld, usuario, trans);

                        trans.Commit();
                    }
                }
            }

        }

        public void Delete(int idOcorrencia, UserState usuario)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    OcorrenciaMapper mapper = new OcorrenciaMapper();
                    mapper.connection = conn;
                    mapper.transaction = trans;

                    Ocorrencia ocorrenciaOld = Find(idOcorrencia);

                    mapper.Delete(idOcorrencia);

                    AuditDelete(ocorrenciaOld, usuario, trans);

                    trans.Commit();
                }
            }
        }

        private bool ValidaOcorrencia(Ocorrencia ocorrencia, bool OperacaoInsert)
        {
            if (ocorrencia.DesOcorrencia == null)
                modelState.AddModelError("Ocorrencia.DesOcorrencia", "Campo obrigatório");

            if (!OperacaoInsert && ocorrencia.IdOcorrencia == 0)
                modelState.AddModelError("Ocorrencia.IdOcorrencia", "Campo obrigatório");            

            if (ocorrencia.TipoOcorrencia == null || ocorrencia.TipoOcorrencia.IdTipoOcorrencia == null)
                modelState.AddModelError("Ocorrencia.TipoOcorrencia.IdTipoOcorrencia", "Campo obrigatório");

            if (modelState.IsValid && OperacaoInsert)
            {                
                var ocorrenciaOld = Find(ocorrencia.IdOcorrencia);
                if (ocorrenciaOld != null)
                {
                    modelState.AddModelError("Ocorrencia.IdOcorrencia", "Código já está cadastrado, favor informar um novo código."); 
                }
            }

            return modelState.IsValid;
        }


    }
}
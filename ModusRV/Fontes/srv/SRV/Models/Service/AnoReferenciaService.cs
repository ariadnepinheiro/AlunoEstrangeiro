using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using System.Data.SqlClient;
using SRV.Models.Mapper;
using System.Web.Mvc;
using SRV.Models.DTO;
using SRV.Common.Exceptions;

namespace SRV.Models.Service
{
    public class AnoReferenciaService : BaseService
    {
        private readonly ModelStateDictionary modelState;

        public AnoReferenciaService(ModelStateDictionary modelState)
        {
            this.modelState = modelState;
        }

        public AnoReferencia Find(int idAnoReferencia)
        {
            AnoReferencia result;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                AnoReferenciaMapper mapper = new AnoReferenciaMapper();
                mapper.connection = conn;

                result = mapper.Find(idAnoReferencia);
            }

            return result;
        }

        public IList<AnoReferencia> List()
        {
            IList<AnoReferencia> result;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                AnoReferenciaMapper mapper = new AnoReferenciaMapper();
                mapper.connection = conn;

                result = mapper.List();
            }

            return result;
        }

        public AnoReferencia Insert(AnoReferencia anoReferencia, UserState usuario)
        {
            if (ValidaAnoReferencia(anoReferencia))
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        AnoReferenciaMapper mapper = new AnoReferenciaMapper();
                        mapper.connection = conn;
                        mapper.transaction = trans;

                        if (!mapper.ValidaInsert(anoReferencia.IdAnoReferencia.Value))
                            throw new BusinessException("Ano de referência já cadastrado");

                        anoReferencia = mapper.Insert(anoReferencia);

                        AuditInsert(anoReferencia, usuario, trans);

                        trans.Commit();
                    }
                }
            }

            return anoReferencia;
        }

        public void Update(AnoReferencia anoReferencia, UserState usuario)
        {
            if (ValidaAnoReferencia(anoReferencia))
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        AnoReferenciaMapper mapper = new AnoReferenciaMapper();
                        mapper.connection = conn;
                        mapper.transaction = trans;

                        AnoReferencia anoReferenciaOld = mapper.Find(anoReferencia.IdAnoReferencia.Value); 

                        mapper.Update(anoReferencia);

                        AuditUpdate(anoReferencia, anoReferenciaOld, usuario, trans);

                        trans.Commit();
                    }
                }
            }
        }

        public void Delete(int idAnoReferencia, UserState usuario)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    AnoReferenciaMapper mapper = new AnoReferenciaMapper();
                    mapper.connection = conn;
                    mapper.transaction = trans;

                    AnoReferencia anoReferenciaOld = mapper.Find(idAnoReferencia);

                    mapper.Delete(idAnoReferencia);

                    AuditDelete(anoReferenciaOld, usuario, trans);

                    trans.Commit();
                }
            }
        }

        private bool ValidaAnoReferencia(AnoReferencia anoReferencia)
        {
            if (anoReferencia.IdAnoReferencia == null)
                modelState.AddModelError("IdAnoReferencia", "Campo é obrigatório");

            if (anoReferencia.DtInicioPeriodoLetivo == null)
            {
                modelState.AddModelError("DtInicioPeriodoLetivo", "Campo é obrigatório");
            }
            else
            {
                if (anoReferencia.DtInicioPeriodoLetivo.Value.Year != anoReferencia.IdAnoReferencia.Value)
                    modelState.AddModelError("", "Data inicial deve pertencer ao mesmo ano de referência");
            }

            if (anoReferencia.DtFimPeriodoLetivo == null)
            {
                modelState.AddModelError("DtFimPeriodoLetivo", "Campo é obrigatório");
            }
            else
            {
                if (anoReferencia.DtFimPeriodoLetivo.Value.Year != anoReferencia.IdAnoReferencia.Value)
                    modelState.AddModelError("", "Data final deve pertencer ao mesmo ano de referência");
            }

            if (anoReferencia.NomeProcCalculo == null)
                modelState.AddModelError("NomeProcCalculo", "Campo é obrigatório");

            if (anoReferencia.DtInicioPeriodoLetivo > anoReferencia.DtFimPeriodoLetivo)
                modelState.AddModelError("", "Data final deve ser maior que a data inicial");

            return modelState.IsValid;
        }
    }
}
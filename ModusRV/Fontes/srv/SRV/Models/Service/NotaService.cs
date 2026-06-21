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
    public class NotaService : BaseService
    {
        private readonly ModelStateDictionary modelState;

        public NotaService(ModelStateDictionary modelState)
        {
            this.modelState = modelState;
        }

        public Nota Find(int idNota)
        {
            Nota result;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                NotaMapper mapper = new NotaMapper();
                mapper.connection = conn;

                result = mapper.Find(idNota);
            }

            return result;
        }

        public IList<Nota> List(int ciclo)
        {
            IList<Nota> result;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                NotaMapper mapper = new NotaMapper();
                mapper.connection = conn;

                result = mapper.List(ciclo);
            }

            return result;
        }

        public Nota Insert(Nota nota, UserState usuario)
        {
            if (ValidaNota(nota))
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        NotaMapper mapper = new NotaMapper();
                        mapper.connection = conn;
                        mapper.transaction = trans;

                        // Carrega o ciclo
                        nota.AnoReferencia = new AnoReferencia() { IdAnoReferencia = usuario.Ciclo };

                        nota = mapper.Insert(nota);

                        AuditInsert(nota, usuario, trans);

                        trans.Commit();
                    }
                }
            }

            return nota;
        }

        public void Update(Nota nota, UserState usuario)
        {
            if (ValidaNota(nota))
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        NotaMapper mapper = new NotaMapper();
                        mapper.connection = conn;
                        mapper.transaction = trans;

                        // Carrega o ciclo
                        nota.AnoReferencia = new AnoReferencia() { IdAnoReferencia = usuario.Ciclo };

                        Nota notaOld = mapper.Find(nota.IdNota.Value);

                        mapper.Update(nota);

                        AuditUpdate(nota, notaOld, usuario, trans);

                        trans.Commit();
                    }
                }
            }
        }

        public void Delete(int idNota, UserState usuario)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    NotaMapper mapper = new NotaMapper();
                    mapper.connection = conn;
                    mapper.transaction = trans;

                    Nota notaOld = mapper.Find(idNota);

                    mapper.Delete(idNota);

                    AuditDelete(notaOld, usuario, trans);

                    trans.Commit();
                }
            }
        }

        private bool ValidaNota(Nota nota)
        {
            if (nota.DesNota == null)
                modelState.AddModelError("DesNota", "Campo é obrigatório");

            return modelState.IsValid;
        }
    }
}
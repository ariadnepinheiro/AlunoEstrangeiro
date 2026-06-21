using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using SRV.Models.Domain;
using SRV.Models.Mapper;
using SRV.Models.DTO;
using SRV.Common.Exceptions;

namespace SRV.Models.Service
{
    public class NivelEnsinoService : BaseService
    {
        private readonly ModelStateDictionary modelState;

        public NivelEnsinoService(ModelStateDictionary modelState)
        {
            this.modelState = modelState;
        }

        public NivelEnsino Find(int idNivelEnsino, int idModalidade)
        {
            NivelEnsino result;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                NivelEnsinoMapper mapper = new NivelEnsinoMapper();
                mapper.connection = conn;

                result = mapper.Find(idNivelEnsino, idModalidade);
            }

            return result;
        }

        public IList<NivelEnsino> List()
        {
            IList<NivelEnsino> result;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                NivelEnsinoMapper mapper = new NivelEnsinoMapper();
                mapper.connection = conn;

                result = mapper.List();
            }

            return result;
        }

        public IList<NivelEnsino> ListByModalidade(int idModalidade)
        {
            IList<NivelEnsino> result;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                NivelEnsinoMapper mapper = new NivelEnsinoMapper();
                mapper.connection = conn;

                result = mapper.ListByModalidade(idModalidade);
            }

            return result;
        }

        public NivelEnsino Insert(NivelEnsino nivelEnsino, UserState usuario)
        {
            if (ValidaNivelEnsino(nivelEnsino))
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        NivelEnsinoMapper mapper = new NivelEnsinoMapper();
                        mapper.connection = conn;
                        mapper.transaction = trans;

                        if (!mapper.ValidaNivelEnsino(nivelEnsino))
                            throw new BusinessException("Nível de ensino já cadastrado para esta modalidade");

                        nivelEnsino = mapper.Insert(nivelEnsino);

                        AuditInsert(nivelEnsino, usuario, trans);

                        trans.Commit();
                    }
                }
            }

            return nivelEnsino;
        }

        public void Update(NivelEnsino nivelEnsino, UserState usuario)
        {
            if (ValidaNivelEnsino(nivelEnsino))
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        NivelEnsinoMapper mapper = new NivelEnsinoMapper();
                        mapper.connection = conn;
                        mapper.transaction = trans;

                        if (!mapper.ValidaNivelEnsino(nivelEnsino))
                            throw new BusinessException("Nível de ensino já cadastrado para esta modalidade");

                        NivelEnsino nivelEnsinoOld = mapper.Find(nivelEnsino.IdNivelEnsino.Value, nivelEnsino.Modalidade.IdModalidade.Value);

                        mapper.Update(nivelEnsino);

                        AuditUpdate(nivelEnsino, nivelEnsinoOld, usuario, trans);

                        trans.Commit();
                    }
                }
            }
        }

        public void Delete(int idNivelEnsino, int idModalidade, UserState usuario)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    NivelEnsinoMapper mapper = new NivelEnsinoMapper();
                    mapper.connection = conn;
                    mapper.transaction = trans;

                    NivelEnsino nivelEnsinoOld = mapper.Find(idNivelEnsino, idModalidade);

                    mapper.Delete(idNivelEnsino, idModalidade);

                    AuditDelete(nivelEnsinoOld, usuario, trans);

                    trans.Commit();
                }
            }
        }

        private bool ValidaNivelEnsino(NivelEnsino nivelEnsino)
        {
            if (nivelEnsino.DesNivelEnsino == null)
                modelState.AddModelError("NivelEnsino.DesNivelEnsino", "Campo é obrigatório");

            if (nivelEnsino.Modalidade == null || nivelEnsino.Modalidade.IdModalidade == null)
                modelState.AddModelError("NivelEnsino.Modalidade.IdModalidade", "Campo é obrigatório");

            return modelState.IsValid;
        }
    }
}
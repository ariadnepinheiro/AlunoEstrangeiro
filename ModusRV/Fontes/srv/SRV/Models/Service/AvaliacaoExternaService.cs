using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRV.Models.Domain;
using System.Data.SqlClient;
using SRV.Models.Mapper;
using SRV.Models.DTO;
using SRV.Common.Exceptions;

namespace SRV.Models.Service
{
    public class AvaliacaoExternaService : BaseService
    {
        readonly private ModelStateDictionary modelState;

        public AvaliacaoExternaService(ModelStateDictionary modelState)
        {
            this.modelState = modelState;
        }

        public AvaliacaoExterna Find(int idAvaliacaoExterna)
        {
            AvaliacaoExterna result;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                AvaliacaoExternaMapper mapper = new AvaliacaoExternaMapper();
                mapper.connection = conn;

                result = mapper.Find(idAvaliacaoExterna);
            }

            return result;
        }

        public IList<AvaliacaoExterna> List()
        {
            IList<AvaliacaoExterna> result;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                AvaliacaoExternaMapper mapper = new AvaliacaoExternaMapper();
                mapper.connection = conn;

                result = mapper.List();
            }

            return result;
        }

        public AvaliacaoExterna Insert(AvaliacaoExterna avaliacaoExterna, UserState usuario)
        {
            if (ValidaAvaliacaoExterna(avaliacaoExterna))
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        AvaliacaoExternaMapper mapper = new AvaliacaoExternaMapper();
                        mapper.connection = conn;
                        mapper.transaction = trans;

                        if (mapper.Valida(avaliacaoExterna.IdAvaliacaoExterna.Value))
                            throw new BusinessException("Código já cadastrado");

                        avaliacaoExterna = mapper.Insert(avaliacaoExterna);

                        AuditInsert(avaliacaoExterna, usuario, trans);

                        trans.Commit();
                    }
                }
            }

            return avaliacaoExterna;
        }

        public void Update(AvaliacaoExterna avaliacaoExterna, UserState usuario)
        {
            if (ValidaAvaliacaoExterna(avaliacaoExterna))
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        AvaliacaoExternaMapper mapper = new AvaliacaoExternaMapper();
                        mapper.connection = conn;
                        mapper.transaction = trans;

                        AvaliacaoExterna avaliacaoExternaOld = mapper.Find(avaliacaoExterna.IdAvaliacaoExterna.Value);

                        mapper.Update(avaliacaoExterna);

                        AuditUpdate(avaliacaoExterna, avaliacaoExternaOld, usuario, trans);

                        trans.Commit();
                    }
                }
            }
        }

        public void Delete(int idAvaliacaoExterna, UserState usuario)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    AvaliacaoExternaMapper mapper = new AvaliacaoExternaMapper();
                    mapper.connection = conn;
                    mapper.transaction = trans;

                    AvaliacaoExterna avaliacaoExternaOld = mapper.Find(idAvaliacaoExterna);

                    mapper.Delete(idAvaliacaoExterna);

                    AuditDelete(avaliacaoExternaOld, usuario, trans);

                    trans.Commit();
                }
            }
        }

        private bool ValidaAvaliacaoExterna(AvaliacaoExterna avaliacaoExterna)
        {
            if (avaliacaoExterna.IdAvaliacaoExterna == null)
                modelState.AddModelError("IdAvaliacaoExterna", "Campo é obrigatório");

            if (avaliacaoExterna.DesAvaliacaoExterna == null)
                modelState.AddModelError("DesAvaliacaoExterna", "Campo é obrigatório");

            if (avaliacaoExterna.DesPeriodoAvaliacao == null)
                modelState.AddModelError("DesPeriodoAvaliacao", "Campo é obrigatório");

            return modelState.IsValid;
        }
    }
}
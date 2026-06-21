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
    public class ModalidadeService : BaseService
    {
        private readonly ModelStateDictionary modelState;

        public ModalidadeService(ModelStateDictionary modelState)
        {
            this.modelState = modelState;
        }

        public Modalidade Find(int idModalidade)
        {
            Modalidade modalidade;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                ModalidadeMapper mapper = new ModalidadeMapper();
                mapper.connection = conn;

                modalidade = mapper.Find(idModalidade);
            }

            return modalidade;
        }

        public IList<Modalidade> List()
        {
            IList<Modalidade> result;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                ModalidadeMapper mapper = new ModalidadeMapper();
                mapper.connection = conn;

                result = mapper.List();
            }

            return result;
        }

        public Modalidade Insert(Modalidade modalidade, UserState usuario)
        {
            if (ValidaModalidade(modalidade))
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        ModalidadeMapper mapper = new ModalidadeMapper();
                        mapper.connection = conn;
                        mapper.transaction = trans;

                        if (!mapper.ValidaSigla(modalidade))
                            throw new BusinessException("Sigla já cadastrada");

                        modalidade = mapper.Insert(modalidade);

                        AuditInsert(modalidade, usuario, trans);

                        trans.Commit();
                    }
                }
            }

            return modalidade;
        }

        public void Update(Modalidade modalidade, UserState usuario)
        {
            if (ValidaModalidade(modalidade))
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        ModalidadeMapper mapper = new ModalidadeMapper();
                        mapper.connection = conn;
                        mapper.transaction = trans;

                        if (!mapper.ValidaSigla(modalidade))
                            throw new BusinessException("Sigla já cadastrada");

                        Modalidade modalidadeOld = mapper.Find(modalidade.IdModalidade.Value);

                        mapper.Update(modalidade);

                        AuditUpdate(modalidade, modalidadeOld, usuario, trans);

                        trans.Commit();
                    }
                }
            }
        }

        public void Delete(int idModalidade, UserState usuario)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    ModalidadeMapper mapper = new ModalidadeMapper();
                    mapper.connection = conn;
                    mapper.transaction = trans;

                    Modalidade modalidadeOld = mapper.Find(idModalidade);

                    mapper.Delete(idModalidade);

                    AuditDelete(modalidadeOld, usuario, trans);

                    trans.Commit();
                }
            }
        }

        private bool ValidaModalidade(Modalidade modalidade)
        {
            if (modalidade.DesModalidade == null)
                modelState.AddModelError("DesModalidade", "Campo é obrigatório");

            if (modalidade.SiglaModalidade == null)
                modelState.AddModelError("SiglaModalidade", "Campo é obrigatório");

            return modelState.IsValid;
        }
    }
}
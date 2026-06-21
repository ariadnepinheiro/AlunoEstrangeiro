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
    public class GrupoFuncaoService : BaseService
    {
        private readonly ModelStateDictionary modelState;

        public GrupoFuncaoService(ModelStateDictionary modelState)
        {
            this.modelState = modelState;
        }

        public GrupoFuncao Find(int idGrupoFuncao)
        {
            GrupoFuncao result;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                GrupoFuncaoMapper mapper = new GrupoFuncaoMapper();
                mapper.connection = conn;

                result = mapper.Find(idGrupoFuncao);
            }

            return result;
        }

        public IList<GrupoFuncao> List()
        {
            IList<GrupoFuncao> result;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                GrupoFuncaoMapper mapper = new GrupoFuncaoMapper();
                mapper.connection = conn;

                result = mapper.List();
            }

            return result;
        }

        public GrupoFuncao Insert(GrupoFuncao grupoFuncao, UserState usuario)
        {
            if (ValidaGrupoFuncao(grupoFuncao))
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        GrupoFuncaoMapper mapper = new GrupoFuncaoMapper();
                        mapper.connection = conn;
                        mapper.transaction = trans;

                        grupoFuncao = mapper.Insert(grupoFuncao);

                        AuditInsert(grupoFuncao, usuario, trans);

                        trans.Commit();
                    }
                }
            }

            return grupoFuncao;
        }

        public void Update(GrupoFuncao grupoFuncao, UserState usuario)
        {
            if (ValidaGrupoFuncao(grupoFuncao))
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        GrupoFuncaoMapper mapper = new GrupoFuncaoMapper();
                        mapper.connection = conn;
                        mapper.transaction = trans;

                        GrupoFuncao grupoFuncaoOld = mapper.Find(grupoFuncao.IdGrupoFuncao.Value);

                        mapper.Update(grupoFuncao);

                        AuditUpdate(grupoFuncao, grupoFuncaoOld, usuario, trans);

                        trans.Commit();
                    }
                }
            }
        }

        public void Delete(int idGrupoFuncao, UserState usuario)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    GrupoFuncaoMapper mapper = new GrupoFuncaoMapper();
                    mapper.connection = conn;
                    mapper.transaction = trans;

                    GrupoFuncao grupoFuncaoOld = mapper.Find(idGrupoFuncao);

                    mapper.Delete(idGrupoFuncao);

                    AuditDelete(grupoFuncaoOld, usuario, trans);

                    trans.Commit();
                }
            }
        }

        private bool ValidaGrupoFuncao(GrupoFuncao grupoFuncao)
        {
            if (grupoFuncao.DesGrupoFuncao == null)
                modelState.AddModelError("DesGrupoFuncao", "Campo é obrigatório");

            return modelState.IsValid;
        }
    }
}
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
    public class IndicadorService : BaseService
    {
        private readonly ModelStateDictionary modelState;

        public IndicadorService(ModelStateDictionary modelState)
        {
            this.modelState = modelState;
        }

        public Indicador Find(int idIndicador)
        {
            Indicador indicador;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                IndicadorMapper mapper = new IndicadorMapper();
                mapper.connection = conn;

                indicador = mapper.Find(idIndicador);
            }

            return indicador;
        }

        public IList<Indicador> List()
        {
            IList<Indicador> result;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                IndicadorMapper mapper = new IndicadorMapper();
                mapper.connection = conn;

                result = mapper.List();
            }

            return result;
        }

        public IList<Indicador> ListByTipo(TipoIndicador tipoIndicador)
        {
            IList<Indicador> result;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                IndicadorMapper mapper = new IndicadorMapper();
                mapper.connection = conn;

                result = mapper.ListByTipo(tipoIndicador);
            }

            return result;
        }

        public Indicador Insert(Indicador indicador, UserState usuario)
        {
            if (ValidaIndicador(indicador))
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        IndicadorMapper mapper = new IndicadorMapper();
                        mapper.connection = conn;
                        mapper.transaction = trans;

                        if (mapper.ValidaIndicador(indicador.IdIndicador.Value))
                            throw new BusinessException("Código já cadastrado");

                        indicador = mapper.Insert(indicador);

                        AuditInsert(indicador, usuario, trans);

                        trans.Commit();
                    }
                }
            }

            return indicador;
        }

        public void Update(Indicador indicador, UserState usuario)
        {
            if (ValidaIndicador(indicador))
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        IndicadorMapper mapper = new IndicadorMapper();
                        mapper.connection = conn;
                        mapper.transaction = trans;

                        Indicador indicadorOld = mapper.Find(indicador.IdIndicador.Value);

                        mapper.Update(indicador);

                        AuditUpdate(indicador, indicadorOld, usuario, trans);

                        trans.Commit();
                    }
                }
            }
        }

        public void Delete(int idIndicador, UserState usuario)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    IndicadorMapper mapper = new IndicadorMapper();
                    mapper.connection = conn;
                    mapper.transaction = trans;

                    Indicador indicadorOld = mapper.Find(idIndicador);

                    mapper.Delete(idIndicador);

                    AuditDelete(indicadorOld, usuario, trans);

                    trans.Commit();
                }
            }
        }

        public bool VerificaRelacionamento(int idIndicador)
        {
            bool result;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                IndicadorMapper mapper = new IndicadorMapper();
                mapper.connection = conn;

                result = mapper.VerificaRelacionamento(idIndicador);
            }

            return result;
        }

        private bool ValidaIndicador(Indicador indicador)
        {
            if (indicador.IdIndicador == null)
                modelState.AddModelError("Indicador.IdIndicador", "Campo é obrigatório");

            if (indicador.DesIndicador == null)
                modelState.AddModelError("Indicador.DesIndicador", "Campo é obrigatório");

            if (indicador.TipoIndicador == TipoIndicador.Vazio)
                modelState.AddModelError("Indicador.TipoIndicador", "Campo é obrigatório");

            return modelState.IsValid;
        }
    }
}
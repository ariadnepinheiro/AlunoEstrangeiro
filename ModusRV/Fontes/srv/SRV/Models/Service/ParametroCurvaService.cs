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
    public class ParametroCurvaService : BaseService
    {
        private readonly ModelStateDictionary modelState;

        public ParametroCurvaService(ModelStateDictionary modelState)
        {
            this.modelState = modelState;
        }

        public IList<ParametroCurva> List(int idTipoUnidadeAdm, int idAnoReferencia)
        {
            IList<ParametroCurva> result;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                ParametroCurvaMapper mapper = new ParametroCurvaMapper();
                mapper.connection = conn;

                result = mapper.List(idTipoUnidadeAdm, idAnoReferencia);
            }

            return result;
        }

        public void Save(ParametroCurvaItem[][] parametros, UserState usuario)
        {
            if (ValidaParametrosCurva(parametros))
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        ParametroCurvaMapper mapper = new ParametroCurvaMapper();
                        mapper.connection = conn;
                        mapper.transaction = trans;

                        for (short i = 0; i < parametros.Length; i++)
                        {
                            for (short j = 0; j < parametros[i].Length; j++)
                            {
                                if (parametros[i][j].Existia)
                                {
                                    ParametroCurva parametroCurvaOld = mapper.Find(parametros[i][j].ParametroCurva.AnoReferencia.IdAnoReferencia.Value, parametros[i][j].ParametroCurva.Nota.IdNota.Value, parametros[i][j].ParametroCurva.GrupoFuncao.IdGrupoFuncao.Value, parametros[i][j].ParametroCurva.TipoUnidadeAdministrativa.IdTipoUnidAdm.Value);

                                    // Verifica se houve alguma alteração no registro
                                    if (parametroCurvaOld.QuantidadeVencimento != parametros[i][j].ParametroCurva.QuantidadeVencimento)
                                    {
                                        mapper.Update(parametros[i][j].ParametroCurva);

                                        AuditUpdate(parametros[i][j].ParametroCurva, parametroCurvaOld, usuario, trans);
                                    }
                                }
                                else
                                {
                                    mapper.Insert(parametros[i][j].ParametroCurva);
                                    AuditInsert(parametros[i][j].ParametroCurva, usuario, trans);
                                }
                            }
                        }

                        trans.Commit();
                    }
                }
            }
        }

        private bool ValidaParametrosCurva(ParametroCurvaItem[][] parametros)
        {
            for (short i = 0; i < parametros.Length; i++)
            {
                for (short j = 0; j < parametros[i].Length; j++)
                {
                    if (parametros[i][j].ParametroCurva.QuantidadeVencimento > 3)
                        throw new BusinessException("Não devem ser informados valores maiores que 3,00");
                }
            }

            return modelState.IsValid;
        }
    }
}
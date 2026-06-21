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
    public class ParametroNotaService : BaseService
    {
        private readonly ModelStateDictionary modelState;

        public ParametroNotaService(ModelStateDictionary modelState)
        {
            this.modelState = modelState;
        }

        public IList<ParametroNota> List(int idModalidade, int idAnoReferencia)
        {
            IList<ParametroNota> result;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                ParametroNotaMapper mapper = new ParametroNotaMapper();
                mapper.connection = conn;

                result = mapper.List(idModalidade, idAnoReferencia);
            }

            return result;
        }

        public void Save(ParametroNotaItem[][] parametros, UserState usuario)
        {
            if (ValidaParametrosNota(parametros))
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        ParametroNotaMapper mapper = new ParametroNotaMapper();
                        mapper.connection = conn;
                        mapper.transaction = trans;

                        for (short i = 0; i < parametros.Length; i++)
                        {
                            for (short j = 0; j < parametros[i].Length; j++)
                            {
                                if (parametros[i][j].Existia)
                                {
                                    ParametroNota parametroNotaOld = mapper.Find(parametros[i][j].ParametroNota.Modalidade.IdModalidade.Value, parametros[i][j].ParametroNota.AnoReferencia.IdAnoReferencia.Value, parametros[i][j].ParametroNota.Nota.IdNota.Value, parametros[i][j].ParametroNota.Indicador.IdIndicador.Value);

                                    // Verifica se houve alguma alteração no registro
                                    if (parametroNotaOld.Percentual != parametros[i][j].ParametroNota.Percentual)
                                    {
                                        // Verifica se atingiu a meta (100,00)
                                        if (parametros[i][j].ParametroNota.Percentual == 100)
                                            parametros[i][j].ParametroNota.ValorMeta = true;
                                        else
                                            parametros[i][j].ParametroNota.ValorMeta = false;

                                        mapper.Update(parametros[i][j].ParametroNota);

                                        AuditUpdate(parametros[i][j].ParametroNota, parametroNotaOld, usuario, trans);
                                    }
                                }
                                else
                                {
                                    // Verifica se atingiu a meta (100,00)
                                    if (parametros[i][j].ParametroNota.Percentual == 100)
                                        parametros[i][j].ParametroNota.ValorMeta = true;
                                    else
                                        parametros[i][j].ParametroNota.ValorMeta = false;

                                    mapper.Insert(parametros[i][j].ParametroNota);
                                    AuditInsert(parametros[i][j].ParametroNota, usuario, trans);
                                }
                            }
                        }

                        trans.Commit();
                    }
                }
            }
        }

        private bool ValidaParametrosNota(ParametroNotaItem[][] parametros)
        {
            bool encontrou100 = false;

            for (short i = 0; i < parametros.Length; i++)
            {
                for (short j = 0; j < parametros[i].Length; j++)
                {
                    if (parametros[i][j].ParametroNota.Percentual > 130)
                        throw new BusinessException("Não devem ser informados valores maiores que 130,00");

                    if (parametros[i][j].ParametroNota.Percentual == 100)
                    {
                        if(encontrou100)
                            throw new BusinessException("Não pode existir mais de um valor 100,00 por indicador");

                        encontrou100 = true;
                    }
                }

                encontrou100 = false;
            }

            return modelState.IsValid;
        }
    }
}
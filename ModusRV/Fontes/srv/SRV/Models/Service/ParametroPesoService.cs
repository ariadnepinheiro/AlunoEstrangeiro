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
    public class ParametroPesoService : BaseService
    {
        private readonly ModelStateDictionary modelState;

        public ParametroPesoService(ModelStateDictionary modelState)
        {
            this.modelState = modelState;
        }

        public IList<ParametroPeso> List(int idModalidade, int idTipoUnidadeAdm, int idAnoReferencia)
        {
            IList<ParametroPeso> result;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                ParametroPesoMapper mapper = new ParametroPesoMapper();
                mapper.connection = conn;

                result = mapper.List(idModalidade, idTipoUnidadeAdm, idAnoReferencia);
            }

            return result;
        }

        public void Save(CadastroParametroPeso cadastro, UserState usuario)
        {
            if (ValidaParametrosPeso(cadastro))
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        ParametroPesoMapper mapper = new ParametroPesoMapper();
                        mapper.connection = conn;
                        mapper.transaction = trans;

                        // Controla a soma dos valores da linha com IGE ou sem IGE
                        decimal soma = 0;

                        for (short i = 0; i < cadastro.NewValues.Length; i++)
                        {
                            for (short j = 0; j < cadastro.NewValues[i].Length; j++)
                            {
                                if (cadastro.NewValues[i][j].IdParametroPeso != null)
                                {
                                    ParametroPeso parametroPesoOld = mapper.Find(cadastro.NewValues[i][j].IdParametroPeso.Value);

                                    // Verifica se houve alguma alteração no registro
                                    if (parametroPesoOld.ValorPeso != cadastro.NewValues[i][j].ValorPeso)
                                    {
                                        mapper.Update(cadastro.NewValues[i][j]);

                                        AuditUpdate(cadastro.NewValues[i][j], parametroPesoOld, usuario, trans);
                                    }
                                }
                                else
                                {
                                    if (cadastro.NewValues[i][j].TemIGE)
                                    {
                                        // Verifica se os valores com IGE foram configurados
                                        soma = cadastro.NewValues[i][0].ValorPeso + cadastro.NewValues[i][1].ValorPeso + cadastro.NewValues[i][2].ValorPeso;

                                        if (soma == 100)
                                        {
                                            cadastro.NewValues[i][j] = mapper.Insert(cadastro.NewValues[i][j]);
                                            AuditInsert(cadastro.NewValues[i][j], usuario, trans);
                                        }
                                    }
                                    else
                                    {
                                        // Verifica se os valores sem IGE foram configurados
                                        soma = cadastro.NewValues[i][3].ValorPeso + cadastro.NewValues[i][4].ValorPeso;

                                        if (soma == 100)
                                        {
                                            cadastro.NewValues[i][j] = mapper.Insert(cadastro.NewValues[i][j]);
                                            AuditInsert(cadastro.NewValues[i][j], usuario, trans);
                                        }
                                    }

                                    soma = 0;
                                }
                            }
                        }

                        trans.Commit();
                    }
                }
            }
        }

        private bool ValidaParametrosPeso(CadastroParametroPeso cadastro)
        {
            decimal somaLinhaComIGE = 0;
            decimal somaLinhaSemIGE = 0;
            bool erroIge = false;

            cadastro.Errors = new bool[cadastro.NewValues.Length];

            for (short i = 0; i < cadastro.NewValues.Length; i++)
            {
                for (short j = 0; j < cadastro.NewValues[i].Length; j++)
                {
                    if (cadastro.NewValues[i][j].ValorPeso > 100)
                        throw new BusinessException("Não devem ser informados valores maiores que 100,00");

                    if(cadastro.NewValues[i][j].TemIGE)
                        somaLinhaComIGE = somaLinhaComIGE + cadastro.NewValues[i][j].ValorPeso;
                    else
                        somaLinhaSemIGE = somaLinhaSemIGE + cadastro.NewValues[i][j].ValorPeso;
                }

                if (somaLinhaComIGE != 100 && somaLinhaComIGE != 0)
                {
                    cadastro.Errors[i] = true;
                    erroIge = true;
                }

                if (somaLinhaSemIGE != 100 && somaLinhaSemIGE != 0)
                {
                    cadastro.Errors[i] = true;
                    erroIge = true;
                }

                somaLinhaComIGE = 0;
                somaLinhaSemIGE = 0;
            }

            if (erroIge)
                modelState.AddModelError("", "As somas dos valores com IGE, e a soma dos valores sem IGE, de cada grupo função devem ser iguais a 100,00");

            return modelState.IsValid;
        }
    }
}
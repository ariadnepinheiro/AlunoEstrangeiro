using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.DTO;
using SRV.Models.Domain;
using System.Data.SqlClient;
using SRV.Models.Mapper;
using System.IO;
using System.Configuration;
using SRV.Common.Loader;
using SRV.Common.Exceptions;

namespace SRV.Models.Service
{
    public class CriterioUnidadeAdministrativaService : ArquivoImportacaoService
    {
        public Paging<CriterioUnidadeAdministrativa> List(FiltroCriterioUnidadeAdministrativa filtro, int currentPage, int pageSize)
        {
            Paging<CriterioUnidadeAdministrativa> criterios;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                CriterioUnidadeAdministrativaMapper mapper = new CriterioUnidadeAdministrativaMapper();
                mapper.connection = conn;

                criterios = mapper.List(filtro, currentPage, pageSize);
            }

            return criterios;
        }

        public void Delete(int idUnidadeAdministrativa, int idAnoReferencia, UserState usuario)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    CriterioUnidadeAdministrativaMapper mapper = new CriterioUnidadeAdministrativaMapper();
                    mapper.connection = conn;
                    mapper.transaction = trans;

                    CriterioUnidadeAdministrativa criterioUnidadeAdministrativaOld = mapper.Find(idUnidadeAdministrativa, idAnoReferencia);

                    mapper.Delete(idUnidadeAdministrativa, idAnoReferencia);

                    AuditDelete(criterioUnidadeAdministrativaOld, usuario, trans);

                    trans.Commit();
                }
            }
        }

        public void Import(ArquivoImportacao arquivoImportacao, UserState usuarioLogado)
        {
            CriterioUnidadeAdministrativa criterioUnidadeAdministrativa, criterioUnidadeAdministrativaOld;

            List<string> ErrorRecords = new List<string>();

            try
            {
                //Faz a leitura de processamento do arquivo
                string filename = Path.Combine(ConfigurationManager.AppSettings["PathUpload"], arquivoImportacao.DesArquivo);

                CsvFileLoader<ImportCriterioUnidadeAdministrativa> loader = new CsvFileLoader<ImportCriterioUnidadeAdministrativa>(filename);

                List<ImportCriterioUnidadeAdministrativa> list = loader.Import();

                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        //Limpa log de erros anteriores
                        ArquivoImportacaoLogMapper arquivoImportacaoLogMapper = new ArquivoImportacaoLogMapper();
                        arquivoImportacaoLogMapper.connection = conn;
                        arquivoImportacaoLogMapper.transaction = trans;

                        arquivoImportacaoLogMapper.Delete(arquivoImportacao.IdArquivoImportacao);

                        ArquivoImportacaoMapper arquivoImportacaoMapper = new ArquivoImportacaoMapper();
                        arquivoImportacaoMapper.connection = conn;
                        arquivoImportacaoMapper.transaction = trans;

                        if (loader.HasError)
                        {
                            //Grava log de erros e atualiza status do arquivo
                            foreach (var error in loader.ErrorRecords)
                            {
                                arquivoImportacaoLogMapper.Insert(arquivoImportacao.IdArquivoImportacao, error);
                            }

                            arquivoImportacaoMapper.UpdateStatus(arquivoImportacao, StatusImportacao.Falha);
                        }
                        else
                        {
                            UnidadeAdministrativaMapper unidadeAdministrativaMapper = new UnidadeAdministrativaMapper();
                            unidadeAdministrativaMapper.connection = conn;
                            unidadeAdministrativaMapper.transaction = trans;


                            //Valida chaves estrangeiras
                            for (int i = 0; i < list.Count; i++)
                            {
                                if (unidadeAdministrativaMapper.Find(list[i].CodUnidadeAdministrativa) == null)
                                    ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'CodUnidadeAdministrativa'"));

                                if(list[i].PercCurriculoMinimo >= 1000)
                                    ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'PercCurriculoMinimo'"));

                                if (list[i].PercLancamentoNota>= 1000)
                                    ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'PercLancamentoNota'"));

                                if (list[i].NotaIge >= 1000)
                                    ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'NotaIge'"));

                            }

                            if (ErrorRecords.Count > 0)
                            {
                                //Grava log de erros e atualiza status do arquivo
                                foreach (var error in ErrorRecords)
                                {
                                    arquivoImportacaoLogMapper.Insert(arquivoImportacao.IdArquivoImportacao, error);
                                }

                                arquivoImportacaoMapper.UpdateStatus(arquivoImportacao, StatusImportacao.Falha);
                            }
                            else
                            {
                                //Insere ou atualiza registros da função
                                //Atualiza status do arquivo para CONCLUÍDO
                                CriterioUnidadeAdministrativaMapper criterioUnidadeAdministrativaMapper = new CriterioUnidadeAdministrativaMapper();
                                criterioUnidadeAdministrativaMapper.connection = conn;
                                criterioUnidadeAdministrativaMapper.transaction = trans;

                                foreach (var importCriterioUnidadeAdministrativa in list)
                                {
                                    //Carrega os dados
                                    criterioUnidadeAdministrativa = new CriterioUnidadeAdministrativa();
                                    criterioUnidadeAdministrativa.UnidadeAdministrativa = new UnidadeAdministrativa();
                                    criterioUnidadeAdministrativa.UnidadeAdministrativa.IdUnidadeAdministrativa = importCriterioUnidadeAdministrativa.CodUnidadeAdministrativa;

                                    criterioUnidadeAdministrativa.AnoReferencia = new AnoReferencia() { IdAnoReferencia = usuarioLogado.Ciclo };

                                    if (importCriterioUnidadeAdministrativa.PercCurriculoMinimo != null)
                                        criterioUnidadeAdministrativa.PerCurriculoMinimo = Decimal.Round(importCriterioUnidadeAdministrativa.PercCurriculoMinimo.Value, 2);

                                    if (importCriterioUnidadeAdministrativa.PercLancamentoNota != null)
                                        criterioUnidadeAdministrativa.PercLancamentoNota = Decimal.Round(importCriterioUnidadeAdministrativa.PercLancamentoNota.Value, 2);

                                    if (importCriterioUnidadeAdministrativa.NotaIge != null)
                                        criterioUnidadeAdministrativa.NotaIge = Decimal.Round(importCriterioUnidadeAdministrativa.NotaIge.Value, 2);

                                    if (criterioUnidadeAdministrativaMapper.ExisteCriterioUnidadeAdministrativa(criterioUnidadeAdministrativa.UnidadeAdministrativa.IdUnidadeAdministrativa.Value, criterioUnidadeAdministrativa.AnoReferencia.IdAnoReferencia.Value))
                                    {
                                        //Atualiza o registro
                                        criterioUnidadeAdministrativaOld = criterioUnidadeAdministrativaMapper.Find(criterioUnidadeAdministrativa.UnidadeAdministrativa.IdUnidadeAdministrativa.Value, criterioUnidadeAdministrativa.AnoReferencia.IdAnoReferencia.Value);

                                        criterioUnidadeAdministrativaMapper.Update(criterioUnidadeAdministrativa);
                                        AuditUpdate(criterioUnidadeAdministrativa, criterioUnidadeAdministrativaOld, usuarioLogado, trans);
                                    }
                                    else
                                    {
                                        //Insere o registro
                                        criterioUnidadeAdministrativaMapper.Insert(criterioUnidadeAdministrativa);
                                        AuditInsert(criterioUnidadeAdministrativa, usuarioLogado, trans);
                                    }
                                }

                                arquivoImportacaoMapper.UpdateStatus(arquivoImportacao, StatusImportacao.Concluido);
                            }
                        }

                        trans.Commit();
                    }
                }
            }
            catch (Exception e)
            {
                DeleteLog(arquivoImportacao.IdArquivoImportacao);

                InsertLog(arquivoImportacao.IdArquivoImportacao, e.Message);

                UpdateStatus(arquivoImportacao, StatusImportacao.Falha);
            }
        }
    }
}
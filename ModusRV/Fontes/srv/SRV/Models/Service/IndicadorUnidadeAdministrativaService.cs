using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using System.Data.SqlClient;
using SRV.Models.Mapper;
using SRV.Models.DTO;
using System.IO;
using System.Configuration;
using SRV.Common.Loader;

namespace SRV.Models.Service
{
    public class IndicadorUnidadeAdministrativaService : ArquivoImportacaoService
    {
        public Paging<IndicadorUnidadeAdministrativa> List(FiltroIndicadorUnidadeAdministrativa filtro, int ciclo, int currentPage, int pageSize)
        {
            Paging<IndicadorUnidadeAdministrativa> niveis = null;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                IndicadorUnidadeAdministrativaMapper mapper = new IndicadorUnidadeAdministrativaMapper();
                mapper.connection = conn;

                niveis = mapper.List(filtro, ciclo, currentPage, pageSize);
            }

            return niveis;
        }


        public void Delete(int idUnidadeAdministrativa, int idModalidade, int idNivelEnsino, int idIndicador, UserState usuario)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    IndicadorUnidadeAdministrativaMapper mapper = new IndicadorUnidadeAdministrativaMapper();
                    mapper.connection = conn;
                    mapper.transaction = trans;

                    IndicadorUnidadeAdministrativa indicadorUnidadeAdministrativaOld = mapper.Find(idUnidadeAdministrativa, idModalidade, idNivelEnsino, idIndicador, usuario.Ciclo);

                    mapper.Delete(idUnidadeAdministrativa, idModalidade, idNivelEnsino, idIndicador, usuario.Ciclo);

                    AuditDelete(indicadorUnidadeAdministrativaOld, usuario, trans);

                    trans.Commit();
                }
            }
        }

        public void Import(ArquivoImportacao arquivoImportacao, UserState usuarioLogado)
        {
            List<string> ErrorRecords = new List<string>();

            try
            {
                //Faz a leitura de processamento do arquivo
                string filename = Path.Combine(ConfigurationManager.AppSettings["PathUpload"], arquivoImportacao.DesArquivo);

                CsvFileLoader<ImportIndicadorUnidadeAdministrativa> loader = new CsvFileLoader<ImportIndicadorUnidadeAdministrativa>(filename);

                List<ImportIndicadorUnidadeAdministrativa> list = loader.Import();

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

                            ModalidadeMapper modalidadeMapper = new ModalidadeMapper();
                            modalidadeMapper.connection = conn;
                            modalidadeMapper.transaction = trans;

                            NivelEnsinoMapper nivelEnsinoMapper = new NivelEnsinoMapper();
                            nivelEnsinoMapper.connection = conn;
                            nivelEnsinoMapper.transaction = trans;

                            IndicadorMapper indicadorMapper = new IndicadorMapper();
                            indicadorMapper.connection = conn;
                            indicadorMapper.transaction = trans;


                            //Valida a participação e as chaves estrangeiras
                            for (int i = 0; i < list.Count; i++)
                            {
                                if (list[i].ValorRealizado >= 1000)
                                    ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'ValorRealizado'"));

                                int? idModalidade = modalidadeMapper.FindBySigla(list[i].CodModalidade);

                                if (idModalidade == null)                                
                                    ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'CodModalidade'"));                                
                                else if (nivelEnsinoMapper.FindByModalidadeDesc(idModalidade.Value, list[i].DesNivelEnsino) == null)
                                        ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'DesNivelEnsino'"));                                

                                if (unidadeAdministrativaMapper.FindByCenso(list[i].CodCenso) == null)
                                    ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'CodCenso'"));

                                if (!indicadorMapper.ValidaIndicador(list[i].CodIndicador))
                                    ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'CodIndicador'"));
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
                                //Insere ou atualiza registros de Avaliação Externa Unidade Administrativa
                                //Atualiza status do arquivo para CONCLUÍDO
                                IndicadorUnidadeAdministrativaMapper mapper = new IndicadorUnidadeAdministrativaMapper();
                                mapper.connection = conn;
                                mapper.transaction = trans;

                                IndicadorUnidadeAdministrativa indicadorUnidadeAdministrativa, indicadorUnidadeAdministrativaOld;

                                foreach (var importIndicadorUnidadeAdministrativa in list)
                                {
                                    //Carrega os dados
                                    indicadorUnidadeAdministrativa = new IndicadorUnidadeAdministrativa();

                                    indicadorUnidadeAdministrativa.Modalidade = new Modalidade();
                                    indicadorUnidadeAdministrativa.Modalidade.IdModalidade = modalidadeMapper.FindBySigla(importIndicadorUnidadeAdministrativa.CodModalidade);

                                    indicadorUnidadeAdministrativa.NivelEnsino = new NivelEnsino();
                                    indicadorUnidadeAdministrativa.NivelEnsino.IdNivelEnsino = nivelEnsinoMapper.FindByModalidadeDesc(indicadorUnidadeAdministrativa.Modalidade.IdModalidade.Value, importIndicadorUnidadeAdministrativa.DesNivelEnsino);

                                    indicadorUnidadeAdministrativa.UnidadeAdministrativa = new UnidadeAdministrativa();
                                    indicadorUnidadeAdministrativa.UnidadeAdministrativa.IdUnidadeAdministrativa = unidadeAdministrativaMapper.FindByCenso(importIndicadorUnidadeAdministrativa.CodCenso);

                                    indicadorUnidadeAdministrativa.AnoReferencia = new AnoReferencia();
                                    indicadorUnidadeAdministrativa.AnoReferencia.IdAnoReferencia = usuarioLogado.Ciclo;

                                    indicadorUnidadeAdministrativa.Indicador = new Indicador();
                                    indicadorUnidadeAdministrativa.Indicador.IdIndicador = importIndicadorUnidadeAdministrativa.CodIndicador;

                                    if (importIndicadorUnidadeAdministrativa.ValorRealizado >= 0)
                                    {
                                        indicadorUnidadeAdministrativa.ValorRealizado = Decimal.Round(importIndicadorUnidadeAdministrativa.ValorRealizado.Value, 2);

                                        if (mapper.FindExiste(indicadorUnidadeAdministrativa.UnidadeAdministrativa.IdUnidadeAdministrativa.Value, indicadorUnidadeAdministrativa.Modalidade.IdModalidade.Value, indicadorUnidadeAdministrativa.NivelEnsino.IdNivelEnsino.Value, indicadorUnidadeAdministrativa.Indicador.IdIndicador.Value, indicadorUnidadeAdministrativa.AnoReferencia.IdAnoReferencia.Value))
                                        {
                                            indicadorUnidadeAdministrativaOld = mapper.Find(indicadorUnidadeAdministrativa.UnidadeAdministrativa.IdUnidadeAdministrativa.Value, indicadorUnidadeAdministrativa.Modalidade.IdModalidade.Value, indicadorUnidadeAdministrativa.NivelEnsino.IdNivelEnsino.Value, indicadorUnidadeAdministrativa.Indicador.IdIndicador.Value, indicadorUnidadeAdministrativa.AnoReferencia.IdAnoReferencia.Value);

                                            //Atualiza o indicador Unidade Administrativa
                                            mapper.Update(indicadorUnidadeAdministrativa);

                                            AuditUpdate(indicadorUnidadeAdministrativa, indicadorUnidadeAdministrativaOld, usuarioLogado, trans);

                                        }
                                        else
                                        {
                                            //Insere o indicador Unidade Administrativa
                                            mapper.Insert(indicadorUnidadeAdministrativa);
                                            AuditInsert(indicadorUnidadeAdministrativa, usuarioLogado, trans);

                                        }
                                    }
                                    else
                                    {
                                        //Exclui o indicador unidade administrativa
                                        indicadorUnidadeAdministrativaOld = mapper.Find(indicadorUnidadeAdministrativa.UnidadeAdministrativa.IdUnidadeAdministrativa.Value, indicadorUnidadeAdministrativa.Modalidade.IdModalidade.Value, indicadorUnidadeAdministrativa.NivelEnsino.IdNivelEnsino.Value, indicadorUnidadeAdministrativa.Indicador.IdIndicador.Value, indicadorUnidadeAdministrativa.AnoReferencia.IdAnoReferencia.Value);

                                        if (indicadorUnidadeAdministrativaOld != null)
                                        {
                                            mapper.Delete(indicadorUnidadeAdministrativa.UnidadeAdministrativa.IdUnidadeAdministrativa.Value, indicadorUnidadeAdministrativa.Modalidade.IdModalidade.Value, indicadorUnidadeAdministrativa.NivelEnsino.IdNivelEnsino.Value, indicadorUnidadeAdministrativa.Indicador.IdIndicador.Value, indicadorUnidadeAdministrativa.AnoReferencia.IdAnoReferencia.Value);
                                            AuditDelete(indicadorUnidadeAdministrativaOld, usuarioLogado, trans);
                                        }
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
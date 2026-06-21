using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using SRV.Models.DTO;
using System.Data.SqlClient;
using SRV.Models.Mapper;
using System.IO;
using System.Configuration;
using SRV.Common.Loader;

namespace SRV.Models.Service
{
    public class MetaUnidadeAdministrativaService : ArquivoImportacaoService
    {
        public Paging<MetaUnidadeAdministrativa> List(FiltroMetaUnidadeAdministrativa filtro, int currentPage, int pageSize)
        {
            Paging<MetaUnidadeAdministrativa> metas = null;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                MetaUnidadeAdministrativaMapper mapper = new MetaUnidadeAdministrativaMapper();
                mapper.connection = conn;

                metas = mapper.List(filtro, currentPage, pageSize);
            }

            return metas;
        }

        public void Delete(int idUnidadeAdministrativa, int idNivelEnsino, int idIndicador, int idModalidade, int idAnoReferencia, UserState usuario)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    MetaUnidadeAdministrativaMapper mapper = new MetaUnidadeAdministrativaMapper();
                    mapper.connection = conn;
                    mapper.transaction = trans;

                    MetaUnidadeAdministrativa metaUnidadeAdministrativaOld = mapper.Find(idUnidadeAdministrativa, idNivelEnsino, idIndicador, idModalidade, idAnoReferencia);

                    mapper.Delete(idUnidadeAdministrativa, idNivelEnsino, idIndicador, idModalidade, idAnoReferencia);

                    AuditDelete(metaUnidadeAdministrativaOld, usuario, trans);

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

                CsvFileLoader<ImportMetaUnidadeAdministrativa> loader = new CsvFileLoader<ImportMetaUnidadeAdministrativa>(filename);

                List<ImportMetaUnidadeAdministrativa> list = loader.Import();

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

                            NivelEnsinoUnidadeAdministrativaMapper nivelEnsinoUnidadeAdministrativaMapper = new NivelEnsinoUnidadeAdministrativaMapper();
                            nivelEnsinoUnidadeAdministrativaMapper.connection = conn;
                            nivelEnsinoUnidadeAdministrativaMapper.transaction = trans;

                            IndicadorMapper indicadorMapper = new IndicadorMapper();
                            indicadorMapper.connection = conn;
                            indicadorMapper.transaction = trans;

                            NivelEnsinoMapper nivelEnsinoMapper = new NivelEnsinoMapper();
                            nivelEnsinoMapper.connection = conn;
                            nivelEnsinoMapper.transaction = trans;

                            ModalidadeMapper modalidadeMapper = new ModalidadeMapper();
                            modalidadeMapper.connection = conn;
                            modalidadeMapper.transaction = trans;

                            //Validações
                            for (int i = 0; i < list.Count; i++)
                            {
                                if(list[i].ValorMeta >= 1000)
                                    ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'ValorMeta'"));

                                if (unidadeAdministrativaMapper.FindByCenso(list[i].CodCenso) == null)
                                    ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'CodCenso'"));

                                int? idModalidade = modalidadeMapper.FindBySigla(list[i].CodModalidade);

                                if (idModalidade == null)
                                {
                                    ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'CodModalidade'"));
                                }
                                else
                                {
                                    if (nivelEnsinoMapper.FindByModalidadeDesc(idModalidade.Value, list[i].DesNivelEnsino) == null)
                                        ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'DesNivelEnsino'"));
                                }

                                if (!indicadorMapper.ExisteIndicador(list[i].CodIndicador))
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
                                //Insere ou atualiza registros
                                //Atualiza status do arquivo para CONCLUÍDO

                                MetaUnidadeAdministrativaMapper metaUnidadeAdministrativaMapper = new MetaUnidadeAdministrativaMapper();
                                metaUnidadeAdministrativaMapper.connection = conn;
                                metaUnidadeAdministrativaMapper.transaction = trans;

                                MetaUnidadeAdministrativa metaUnidadeAdministrativa, metaUnidadeAdministrativaOld;
                                NivelEnsinoUnidadeAdministrativa nivelEnsinoUnidadeAdministrativa, nivelEnsinoUnidadeAdministrativaOld;

                                foreach (var importMeta in list)
                                {
                                    //Carrega os dados
                                    metaUnidadeAdministrativa = new MetaUnidadeAdministrativa();
                                    metaUnidadeAdministrativa.UnidadeAdministrativa = new UnidadeAdministrativa() { IdUnidadeAdministrativa = unidadeAdministrativaMapper.FindByCenso(importMeta.CodCenso) };
                                    metaUnidadeAdministrativa.Indicador = new Indicador() { IdIndicador = importMeta.CodIndicador };
                                    metaUnidadeAdministrativa.Modalidade = new Modalidade() { IdModalidade = modalidadeMapper.FindBySigla(importMeta.CodModalidade) };
                                    metaUnidadeAdministrativa.NivelEnsino = new NivelEnsino() { IdNivelEnsino = nivelEnsinoMapper.FindByModalidadeDesc(metaUnidadeAdministrativa.Modalidade.IdModalidade.Value, importMeta.DesNivelEnsino) };
                                    metaUnidadeAdministrativa.AnoReferencia = new AnoReferencia() { IdAnoReferencia = usuarioLogado.Ciclo };

                                    if (importMeta.ValorMeta >= 0)
                                    {
                                        metaUnidadeAdministrativa.ValorMeta = Decimal.Round(importMeta.ValorMeta.Value, 2);

                                        if (!nivelEnsinoUnidadeAdministrativaMapper.ExisteNivelEnsinoUnidadeAdministrativa(metaUnidadeAdministrativa.UnidadeAdministrativa.IdUnidadeAdministrativa.Value, metaUnidadeAdministrativa.Modalidade.IdModalidade.Value, metaUnidadeAdministrativa.NivelEnsino.IdNivelEnsino.Value, usuarioLogado.Ciclo))
                                        {
                                            nivelEnsinoUnidadeAdministrativa = new NivelEnsinoUnidadeAdministrativa();
                                            nivelEnsinoUnidadeAdministrativa.UnidadeAdministrativa = new UnidadeAdministrativa() { IdUnidadeAdministrativa = metaUnidadeAdministrativa.UnidadeAdministrativa.IdUnidadeAdministrativa.Value };
                                            nivelEnsinoUnidadeAdministrativa.Modalidade = new Modalidade() { IdModalidade = metaUnidadeAdministrativa.Modalidade.IdModalidade.Value };
                                            nivelEnsinoUnidadeAdministrativa.NivelEnsino = new NivelEnsino() { IdNivelEnsino = metaUnidadeAdministrativa.NivelEnsino.IdNivelEnsino.Value };
                                            nivelEnsinoUnidadeAdministrativa.AnoReferencia = new AnoReferencia() { IdAnoReferencia = usuarioLogado.Ciclo };

                                            //Insere o nivel de ensino unidade administrativa
                                            nivelEnsinoUnidadeAdministrativaMapper.Insert(nivelEnsinoUnidadeAdministrativa);
                                            AuditInsert(nivelEnsinoUnidadeAdministrativa, usuarioLogado, trans);
                                        }

                                        if (metaUnidadeAdministrativaMapper.ExisteMeta(metaUnidadeAdministrativa.UnidadeAdministrativa.IdUnidadeAdministrativa.Value, metaUnidadeAdministrativa.NivelEnsino.IdNivelEnsino.Value, metaUnidadeAdministrativa.Indicador.IdIndicador.Value, metaUnidadeAdministrativa.Modalidade.IdModalidade.Value, usuarioLogado.Ciclo))
                                        {
                                            metaUnidadeAdministrativaOld = metaUnidadeAdministrativaMapper.Find(metaUnidadeAdministrativa.UnidadeAdministrativa.IdUnidadeAdministrativa.Value, metaUnidadeAdministrativa.NivelEnsino.IdNivelEnsino.Value, metaUnidadeAdministrativa.Indicador.IdIndicador.Value, metaUnidadeAdministrativa.Modalidade.IdModalidade.Value, usuarioLogado.Ciclo);

                                            //Atualiza a meta unidade administrativa
                                            metaUnidadeAdministrativaMapper.Update(metaUnidadeAdministrativa);
                                            AuditUpdate(metaUnidadeAdministrativa, metaUnidadeAdministrativaOld, usuarioLogado, trans);
                                        }
                                        else
                                        {
                                            //Insere a meta unidade administrativa
                                            metaUnidadeAdministrativaMapper.Insert(metaUnidadeAdministrativa);
                                            AuditInsert(metaUnidadeAdministrativa, usuarioLogado, trans);
                                        }
                                    }
                                    else
                                    {
                                        //Exclui a meta unidade administrativa
                                        metaUnidadeAdministrativaOld = metaUnidadeAdministrativaMapper.Find(metaUnidadeAdministrativa.UnidadeAdministrativa.IdUnidadeAdministrativa.Value, metaUnidadeAdministrativa.NivelEnsino.IdNivelEnsino.Value, metaUnidadeAdministrativa.Indicador.IdIndicador.Value, metaUnidadeAdministrativa.Modalidade.IdModalidade.Value, usuarioLogado.Ciclo);

                                        if (metaUnidadeAdministrativaOld != null)
                                        {
                                            metaUnidadeAdministrativaMapper.Delete(metaUnidadeAdministrativa.UnidadeAdministrativa.IdUnidadeAdministrativa.Value, metaUnidadeAdministrativa.NivelEnsino.IdNivelEnsino.Value, metaUnidadeAdministrativa.Indicador.IdIndicador.Value, metaUnidadeAdministrativa.Modalidade.IdModalidade.Value, usuarioLogado.Ciclo);
                                            AuditDelete(metaUnidadeAdministrativaOld, usuarioLogado, trans);
                                        }

                                        //Exclui o nível ensino unidade administrativa
                                        nivelEnsinoUnidadeAdministrativaOld = nivelEnsinoUnidadeAdministrativaMapper.Find(metaUnidadeAdministrativa.UnidadeAdministrativa.IdUnidadeAdministrativa.Value, metaUnidadeAdministrativa.Modalidade.IdModalidade.Value, metaUnidadeAdministrativa.NivelEnsino.IdNivelEnsino.Value, usuarioLogado.Ciclo);

                                        if (nivelEnsinoUnidadeAdministrativaOld != null)
                                        {
                                            nivelEnsinoUnidadeAdministrativaMapper.Delete(metaUnidadeAdministrativa.UnidadeAdministrativa.IdUnidadeAdministrativa.Value, metaUnidadeAdministrativa.Modalidade.IdModalidade.Value, metaUnidadeAdministrativa.NivelEnsino.IdNivelEnsino.Value, usuarioLogado.Ciclo);
                                            AuditDelete(nivelEnsinoUnidadeAdministrativaOld, usuarioLogado, trans);
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
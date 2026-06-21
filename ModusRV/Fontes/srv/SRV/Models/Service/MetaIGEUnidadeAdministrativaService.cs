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
    public class MetaIGEUnidadeAdministrativaService : ArquivoImportacaoService
    {
        public Paging<MetaIGEUnidadeAdministrativa> List(FiltroMetaIGEUnidadeAdministrativa filtro, int currentPage, int pageSize)
        {
            Paging<MetaIGEUnidadeAdministrativa> metas;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                MetaIGEUnidadeAdministrativaMapper mapper = new MetaIGEUnidadeAdministrativaMapper();
                mapper.connection = conn;

                metas = mapper.List(filtro, currentPage, pageSize);
            }

            return metas;
        }

        public void Delete(int idUnidadeAdministrativa, int idAnoReferencia, UserState usuario)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    MetaIGEUnidadeAdministrativaMapper mapper = new MetaIGEUnidadeAdministrativaMapper();
                    mapper.connection = conn;
                    mapper.transaction = trans;

                    MetaIGEUnidadeAdministrativa metaIGEUnidadeAdministrativaOld = mapper.Find(idUnidadeAdministrativa, idAnoReferencia);

                    mapper.Delete(idUnidadeAdministrativa, idAnoReferencia);

                    AuditDelete(metaIGEUnidadeAdministrativaOld, usuario, trans);

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

                CsvFileLoader<ImportMetaIGEUnidadeAdministrativa> loader = new CsvFileLoader<ImportMetaIGEUnidadeAdministrativa>(filename);

                List<ImportMetaIGEUnidadeAdministrativa> list = loader.Import();

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

                            //Validações
                            for (int i = 0; i < list.Count; i++)
                            {
                                if(list[i].MetaIge >= 1000)
                                    ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'MetaIge'"));

                                if (!unidadeAdministrativaMapper.ExisteUnidadeAdministrativa(list[i].CodUnidadeAdministrativa))
                                    ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'CodUnidadeAdministrativa'"));
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
                                MetaIGEUnidadeAdministrativaMapper metaIGEUnidadeAdministrativaMapper = new MetaIGEUnidadeAdministrativaMapper();
                                metaIGEUnidadeAdministrativaMapper.connection = conn;
                                metaIGEUnidadeAdministrativaMapper.transaction = trans;

                                MetaIGEUnidadeAdministrativa metaIGEUnidadeAdministrativa, metaIGEUnidadeAdministrativaOld;

                                foreach (var importMeta in list)
                                {
                                    //Carrega os dados
                                    metaIGEUnidadeAdministrativa = new MetaIGEUnidadeAdministrativa();
                                    metaIGEUnidadeAdministrativa.UnidadeAdministrativa = new UnidadeAdministrativa();
                                    metaIGEUnidadeAdministrativa.UnidadeAdministrativa.IdUnidadeAdministrativa = importMeta.CodUnidadeAdministrativa;
                                    metaIGEUnidadeAdministrativa.AnoReferencia = new AnoReferencia() { IdAnoReferencia = usuarioLogado.Ciclo };
                                    metaIGEUnidadeAdministrativa.MetaIge = Decimal.Round(importMeta.MetaIge, 2);

                                    if (metaIGEUnidadeAdministrativaMapper.ExisteMetaIge(importMeta.CodUnidadeAdministrativa, usuarioLogado.Ciclo))
                                    {
                                        metaIGEUnidadeAdministrativaOld = metaIGEUnidadeAdministrativaMapper.Find(importMeta.CodUnidadeAdministrativa, usuarioLogado.Ciclo);

                                        //Atualiza o registro
                                        metaIGEUnidadeAdministrativaMapper.Update(metaIGEUnidadeAdministrativa);
                                        AuditUpdate(metaIGEUnidadeAdministrativa, metaIGEUnidadeAdministrativaOld, usuarioLogado, trans);
                                    }
                                    else
                                    {
                                        //Insere o registro
                                        metaIGEUnidadeAdministrativaMapper.Insert(metaIGEUnidadeAdministrativa);
                                        AuditInsert(metaIGEUnidadeAdministrativa, usuarioLogado, trans);
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
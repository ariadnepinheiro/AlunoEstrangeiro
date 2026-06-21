using System;
using System.Collections.Generic;
using System.Linq;
using SRV.Models.Domain;
using SRV.Models.DTO;
using System.Data.SqlClient;
using SRV.Models.Mapper;
using SRV.Common.Loader;
using System.IO;
using System.Configuration;

namespace SRV.Models.Service
{
    public class DenunciaAvaliacaoExternaService : ArquivoImportacaoService
    {
        public Paging<DenunciaAvaliacaoExterna> List(FiltroDenunciaAvaliacaoExterna filtro, int currentPage, int pageSize)
        {
            Paging<DenunciaAvaliacaoExterna> denunciasAvaliacoesExternas;
            SqlConnection conn = GetConnection();
            DenunciaAvaliacaoExternaMapper mapper = null;

            try
            {
                conn.Open();

                mapper = new DenunciaAvaliacaoExternaMapper();
                mapper.connection = conn;
                denunciasAvaliacoesExternas = mapper.List(filtro, currentPage, pageSize);
            }
            finally
            {
                conn.Close();
            }

            return denunciasAvaliacoesExternas;
        }

        public void Import(ArquivoImportacao arquivoImportacao, UserState usuarioLogado)
        {
            List<string> errorRecords = new List<string>();
            string fileName = string.Empty;
            CsvFileLoader<ImportDenunciaAvaliacaoExterna> loader;
            List<ImportDenunciaAvaliacaoExterna> list;
            SqlConnection conn = GetConnection();
            SqlTransaction trans = null;

            ArquivoImportacaoLogMapper arquivoImportacaoLogMapper;
            ArquivoImportacaoMapper arquivoImportacaoMapper;
            AnoReferenciaMapper anoReferenciaMapper;
            AvaliacaoExternaMapper avaliacaoExternaMapper;
            ServidorMapper servidorMapper;
            UnidadeAdministrativaMapper unidadeAdministrativaMapper;
            DenunciaAvaliacaoExternaMapper denunciaAvaliacaoExternaMapper;
            DenunciaAvaliacaoExterna denunciaAvaliacaoExterna;

            try
            {
                fileName = Path.Combine(ConfigurationManager.AppSettings["PathUpload"], arquivoImportacao.DesArquivo);
                loader = new CsvFileLoader<ImportDenunciaAvaliacaoExterna>(fileName);
                list = loader.Import();

                conn.Open();
                trans = conn.BeginTransaction();

                arquivoImportacaoLogMapper = new ArquivoImportacaoLogMapper();
                arquivoImportacaoLogMapper.connection = conn;
                arquivoImportacaoLogMapper.transaction = trans;

                arquivoImportacaoLogMapper.Delete(arquivoImportacao.IdArquivoImportacao);

                arquivoImportacaoMapper = new ArquivoImportacaoMapper();
                arquivoImportacaoMapper.connection = conn;
                arquivoImportacaoMapper.transaction = trans;

                if (loader.HasError)
                {
                    foreach (var error in loader.ErrorRecords)
                    {
                        arquivoImportacaoLogMapper.Insert(arquivoImportacao.IdArquivoImportacao, error);
                    }

                    arquivoImportacaoMapper.UpdateStatus(arquivoImportacao, StatusImportacao.Falha);
                }
                else
                {
                    anoReferenciaMapper = new AnoReferenciaMapper();
                    anoReferenciaMapper.connection = conn;
                    anoReferenciaMapper.transaction = trans;

                    avaliacaoExternaMapper = new AvaliacaoExternaMapper();
                    avaliacaoExternaMapper.connection = conn;
                    avaliacaoExternaMapper.transaction = trans;

                    servidorMapper = new ServidorMapper();
                    servidorMapper.connection = conn;
                    servidorMapper.transaction = trans;

                    unidadeAdministrativaMapper = new UnidadeAdministrativaMapper();
                    unidadeAdministrativaMapper.connection = conn;
                    unidadeAdministrativaMapper.transaction = trans;

                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].Ciclo != usuarioLogado.Ciclo)
                            errorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'CICLO'"));

                        if (unidadeAdministrativaMapper.FindByCenso(list[i].CensoUnidade) == null)
                            errorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'CENSO_UNIDADE'"));

                        if (!unidadeAdministrativaMapper.ExisteUnidadeAdministrativa(list[i].UnidadeAdministrativa))
                            errorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'UNIDADE_ADMINISTRATIVA'"));

                        if (servidorMapper.Find(list[i].Matricula) == null)
                            errorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'MATRÍCULA'"));

                        if (!avaliacaoExternaMapper.Valida(list[i].Avaliacao))
                            errorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'AVALIAÇÃO'"));
                    }

                    if (errorRecords.Count() > 0)
                    {
                        foreach (var error in errorRecords)
                        {
                            arquivoImportacaoLogMapper.Insert(arquivoImportacao.IdArquivoImportacao, error);
                        }
                        arquivoImportacaoMapper.UpdateStatus(arquivoImportacao, StatusImportacao.Falha);
                    }
                    else
                    {
                        denunciaAvaliacaoExternaMapper = new DenunciaAvaliacaoExternaMapper();
                        denunciaAvaliacaoExternaMapper.connection = conn;
                        denunciaAvaliacaoExternaMapper.transaction = trans;

                        denunciaAvaliacaoExternaMapper.DeleteAllRows();

                        foreach (var importDenunciaAvaliacaoExternaMapper in list)
                        {
                            denunciaAvaliacaoExterna = new DenunciaAvaliacaoExterna();

                            // Ano Base da ocorrência
                            denunciaAvaliacaoExterna.AnoReferencia = new AnoReferencia();
                            denunciaAvaliacaoExterna.AnoReferencia.IdAnoReferencia = importDenunciaAvaliacaoExternaMapper.Ciclo;

                            // Censo da Unidade
                            // Código da Unidade
                            denunciaAvaliacaoExterna.UnidadeAdministrativa = new UnidadeAdministrativa();
                            denunciaAvaliacaoExterna.UnidadeAdministrativa.IdCenso = importDenunciaAvaliacaoExternaMapper.CensoUnidade;
                            denunciaAvaliacaoExterna.UnidadeAdministrativa.IdUnidadeAdministrativa = importDenunciaAvaliacaoExternaMapper.UnidadeAdministrativa;

                            // Matrícula do Docente
                            denunciaAvaliacaoExterna.Servidor = new Servidor();
                            denunciaAvaliacaoExterna.Servidor.IdServidor = importDenunciaAvaliacaoExternaMapper.Matricula;

                            // Código da Avaliação
                            denunciaAvaliacaoExterna.AvaliacaoExterna = new AvaliacaoExterna();
                            denunciaAvaliacaoExterna.AvaliacaoExterna.IdAvaliacaoExterna = importDenunciaAvaliacaoExternaMapper.Avaliacao;

                            // Motivo denúncia
                            denunciaAvaliacaoExterna.DesMotivoDenuncia = importDenunciaAvaliacaoExternaMapper.Motivo;

                            denunciaAvaliacaoExternaMapper.Insert(denunciaAvaliacaoExterna);
                            AuditInsert(denunciaAvaliacaoExterna, usuarioLogado, trans);
                        }

                        arquivoImportacaoMapper.UpdateStatus(arquivoImportacao, StatusImportacao.Concluido);
                    }
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();

                DeleteLog(arquivoImportacao.IdArquivoImportacao);

                InsertLog(arquivoImportacao.IdArquivoImportacao, ex.Message);

                UpdateStatus(arquivoImportacao, StatusImportacao.Falha);
            }
        }

        public void Delete(int idServidor, UserState usuario)
        {
            SqlConnection conn = GetConnection();
            SqlTransaction trans = null;
            DenunciaAvaliacaoExternaMapper mapper = new DenunciaAvaliacaoExternaMapper();

            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                mapper.connection = conn;
                mapper.transaction = trans;

                DenunciaAvaliacaoExterna denunciaAvaliacaoExternaOld = mapper.Find(idServidor);

                mapper.Delete(idServidor);

                AuditDelete(denunciaAvaliacaoExternaOld, usuario, trans);

                trans.Commit();                    
            }
            catch (Exception ex)
            {
                trans.Rollback();
            }
            finally
            {
                conn.Close();
            }
        }

        public IList<DenunciaAvaliacaoExterna> List(int idAnoReferencia, int idServidor, int idUnidade)
        {
            IList<DenunciaAvaliacaoExterna> denunciasAvaliacoesExternas = null;
            SqlConnection conn = GetConnection();
            DenunciaAvaliacaoExternaMapper denunciaAvaliacaoExternaMapper;

            try
            {
                conn.Open();

                denunciaAvaliacaoExternaMapper = new DenunciaAvaliacaoExternaMapper();
                denunciaAvaliacaoExternaMapper.connection = conn;

                denunciasAvaliacoesExternas = denunciaAvaliacaoExternaMapper.ListBy(idAnoReferencia, idServidor, idUnidade);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return denunciasAvaliacoesExternas;
        }
    }
}
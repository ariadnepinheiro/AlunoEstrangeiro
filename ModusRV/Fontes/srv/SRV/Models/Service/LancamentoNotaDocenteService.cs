using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Mapper;
using SRV.Models.Domain;
using SRV.Models.DTO;
using System.Data.SqlClient;
using SRV.Common.Loader;
using System.Configuration;
using System.IO;

namespace SRV.Models.Service
{
    public class LancamentoNotaDocenteService: ArquivoImportacaoService
    {
        public Paging<LancamentoNotaDocente> List(FiltroLancamentoNotaDocente filtro, int currentPage, int pageSize)
        {
            Paging<LancamentoNotaDocente> lancamentosNotasDocentes;
            SqlConnection conn = GetConnection();
            LancamentoNotaDocenteMapper mapper = null;

            try
            {
                conn.Open();

                mapper = new LancamentoNotaDocenteMapper();
                mapper.connection = conn;
                lancamentosNotasDocentes = mapper.List(filtro, currentPage, pageSize);
            }
            finally
            {
                conn.Close();
            }

            return lancamentosNotasDocentes;
        }

        public void Import(ArquivoImportacao arquivoImportacao, UserState usuarioLogado)
        {
            List<string> errorRecords = new List<string>();
            string fileName = string.Empty;
            CsvFileLoader<ImportLancamentoNotaDocente> loader;
            List<ImportLancamentoNotaDocente> list;
            SqlConnection conn = GetConnection();
            SqlTransaction trans = null;

            ArquivoImportacaoLogMapper arquivoImportacaoLogMapper;
            ArquivoImportacaoMapper arquivoImportacaoMapper;
            AnoReferenciaMapper anoReferenciaMapper;
            ServidorMapper servidorMapper;
            UnidadeAdministrativaMapper unidadeAdministrativaMapper;
            LancamentoNotaDocenteMapper lancamentoNotaDocenteMapper;
            LancamentoNotaDocente lancamentoNotaDocente;

            try
            {
                fileName = Path.Combine(ConfigurationManager.AppSettings["PathUpload"], arquivoImportacao.DesArquivo);
                loader = new CsvFileLoader<ImportLancamentoNotaDocente>(fileName);
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

                        if (list[i].Periodo < 0 || list[i].Periodo > 2)
                            errorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'PERÍODO'"));

                        if (list[i].Bimestre < 1 || list[i].Bimestre > 4)
                            errorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'BIMESTRE'"));
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
                        lancamentoNotaDocenteMapper = new LancamentoNotaDocenteMapper();
                        lancamentoNotaDocenteMapper.connection = conn;
                        lancamentoNotaDocenteMapper.transaction = trans;

                        lancamentoNotaDocenteMapper.DeleteAllRows();

                        foreach (var importLancamentoNotaDocente in list)
                        {
                            lancamentoNotaDocente = new LancamentoNotaDocente();

                            lancamentoNotaDocente.NmBimestre = importLancamentoNotaDocente.Bimestre;
                            lancamentoNotaDocente.DesTurma = importLancamentoNotaDocente.Turma;
                            lancamentoNotaDocente.NmPeriodo = importLancamentoNotaDocente.Periodo;
                            lancamentoNotaDocente.DesDisciplina = importLancamentoNotaDocente.Disciplina;

                            // Ano Base da ocorrência
                            lancamentoNotaDocente.AnoReferencia = new AnoReferencia();
                            lancamentoNotaDocente.AnoReferencia.IdAnoReferencia = importLancamentoNotaDocente.Ciclo;

                            // Censo da Unidade
                            // Código da Unidade
                            lancamentoNotaDocente.UnidadeAdministrativa = new UnidadeAdministrativa();
                            lancamentoNotaDocente.UnidadeAdministrativa.IdCenso = importLancamentoNotaDocente.CensoUnidade;
                            lancamentoNotaDocente.UnidadeAdministrativa.IdUnidadeAdministrativa = importLancamentoNotaDocente.UnidadeAdministrativa;

                            // Matrícula do Docente
                            lancamentoNotaDocente.Servidor = new Servidor();
                            lancamentoNotaDocente.Servidor.IdServidor = importLancamentoNotaDocente.Matricula;

                            lancamentoNotaDocenteMapper.Insert(lancamentoNotaDocente);
                            AuditInsert(lancamentoNotaDocente, usuarioLogado, trans);
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

		public void Delete(int IdLancamentoNotaDocente, UserState usuario)
        {
            SqlConnection conn = GetConnection();
            SqlTransaction trans = null;
            LancamentoNotaDocenteMapper mapper = new LancamentoNotaDocenteMapper();

            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                mapper.connection = conn;
                mapper.transaction = trans;

				LancamentoNotaDocente lancamentoNotaDocenteOld = mapper.Find(IdLancamentoNotaDocente);

				mapper.Delete(IdLancamentoNotaDocente);

                AuditDelete(lancamentoNotaDocenteOld, usuario, trans);

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

        public string PossuiLancamentoNotas(int idAnoReferencia, int idServidor, int Unidade)
        {
            SqlConnection conn = GetConnection();
            LancamentoNotaDocenteMapper lancamentoNotaDocenteMapper;
            MotivoInelegibilidadeMapper motivoInelegibilidadeMapper;

            string retorno = string.Empty;
            
            try
            {
                lancamentoNotaDocenteMapper = new LancamentoNotaDocenteMapper();

                conn.Open();

                lancamentoNotaDocenteMapper.connection = conn;

                if (lancamentoNotaDocenteMapper.FindBy(idServidor) != null)
                {
                    motivoInelegibilidadeMapper = new MotivoInelegibilidadeMapper();
                    motivoInelegibilidadeMapper.connection = conn;

                    retorno = motivoInelegibilidadeMapper.Find((int)MotivoInelegibilidade.TipoMotivo.LancamentoNotaDocente).DesMotivoInelegibilidade;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return retorno;
        }

        public IList<LancamentoNotaDocente> List(int idAnoReferencia, int idServidor, int idUnidade)
        {
            IList<LancamentoNotaDocente> lancamentosNotas = null;
            SqlConnection conn = GetConnection();
            LancamentoNotaDocenteMapper mapper;

            try
            {
                conn.Open();
                mapper = new LancamentoNotaDocenteMapper();
                mapper.connection = conn;

                lancamentosNotas = mapper.ListBy(idAnoReferencia, idServidor, idUnidade);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return lancamentosNotas;
        }
    }
}
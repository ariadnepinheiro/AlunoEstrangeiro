using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using SRV.Models.Mapper;
using SRV.Models.DTO;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;
using SRV.Common.Loader;
using SRV.Common.Exceptions;

namespace SRV.Models.Service
{
    public class AplicacaoProvaAvaliacaoExternaService : ArquivoImportacaoService
    {
        public Paging<AplicacaoProvaAvaliacaoExterna> List(FiltroAplicacaoProvaAvaliacaoExterna filtro, int currentPage, int pageSize)
        {
            Paging<AplicacaoProvaAvaliacaoExterna> aplicacoesProvas;
            SqlConnection conn = GetConnection();
			AplicacaoProvaAvaliacaoExternaMapper mapper = null;

            try
            {
                conn.Open();

                mapper = new AplicacaoProvaAvaliacaoExternaMapper();
                mapper.connection = conn;
                aplicacoesProvas = mapper.List(filtro, currentPage, pageSize);
            }	
            finally
            {
				conn.Close();
            }

			return aplicacoesProvas;
        }

		public void Import(ArquivoImportacao arquivoImportacao, UserState usuarioLogado)
		{
			List<string> ErrorRecords = new List<string>();
            string fileName = string.Empty;
            CsvFileLoader<ImportAplicacaoProvaAvaliacaoExterna> loader;
            List<ImportAplicacaoProvaAvaliacaoExterna> list;
            SqlConnection conn = GetConnection();
            SqlTransaction trans;
            ArquivoImportacaoLogMapper arquivoImportacaoLogMapper;
            ArquivoImportacaoMapper arquivoImportacaoMapper;
            AnoReferenciaMapper anoReferenciaMapper;
            AvaliacaoExternaMapper avaliacaoExternaMapper;
            ServidorMapper servidorMapper;
            UnidadeAdministrativaMapper unidadeAdministrativaMapper;
            AplicacaoProvaAvaliacaoExternaMapper aplicacaoProvaAvaliacaoExternaMapper;
            AplicacaoProvaAvaliacaoExterna aplicacaoProvaAvaliacaoExterna;

			try
			{
                fileName = Path.Combine(ConfigurationManager.AppSettings["PathUpload"], arquivoImportacao.DesArquivo);
                loader = new CsvFileLoader<ImportAplicacaoProvaAvaliacaoExterna>(fileName);
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
                            ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'CICLO'"));

                        if (unidadeAdministrativaMapper.FindByCenso(list[i].CensoUnidade) == null)
                            ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'CENSO_UNIDADE'"));

                        if (!unidadeAdministrativaMapper.ExisteUnidadeAdministrativa(list[i].UnidadeAdministrativa))
                            ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'UNIDADE_ADMINISTRATIVA'"));

                        if (servidorMapper.Find(list[i].Matricula) == null)
                            ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'MATRÍCULA'"));

                        if (list[i].Data.Year != usuarioLogado.Ciclo)
                            ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'DATA'"));

                        if (!avaliacaoExternaMapper.Valida(list[i].Avaliacao))
                            ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'AVALIAÇÃO'"));

                        if (list[i].Periodo < 0 || list[i].Periodo > 2)
                            ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'PERÍODO'"));
                    }

                    if (ErrorRecords.Count > 0)
                    {
                        foreach (var error in ErrorRecords)
                        {
                            arquivoImportacaoLogMapper.Insert(arquivoImportacao.IdArquivoImportacao, error);
                        }

                        arquivoImportacaoMapper.UpdateStatus(arquivoImportacao, StatusImportacao.Falha);
                    }
                    else
                    {
                        aplicacaoProvaAvaliacaoExternaMapper = new AplicacaoProvaAvaliacaoExternaMapper();
                        aplicacaoProvaAvaliacaoExternaMapper.connection = conn;
                        aplicacaoProvaAvaliacaoExternaMapper.transaction = trans;

                        aplicacaoProvaAvaliacaoExternaMapper.DeleteAllRows();

                        foreach (var importAplicacaoProvaAvaliacaoExterna in list)
                        {
                            aplicacaoProvaAvaliacaoExterna = new AplicacaoProvaAvaliacaoExterna();

                            // Ano Base da ocorrência
                            aplicacaoProvaAvaliacaoExterna.AnoReferencia = new AnoReferencia();
                            aplicacaoProvaAvaliacaoExterna.AnoReferencia.IdAnoReferencia = importAplicacaoProvaAvaliacaoExterna.Ciclo;

                            // Censo da Unidade
                            // Código da Unidade
                            aplicacaoProvaAvaliacaoExterna.UnidadeAdministrativa = new UnidadeAdministrativa();
                            aplicacaoProvaAvaliacaoExterna.UnidadeAdministrativa.IdCenso = importAplicacaoProvaAvaliacaoExterna.CensoUnidade;
                            aplicacaoProvaAvaliacaoExterna.UnidadeAdministrativa.IdUnidadeAdministrativa = importAplicacaoProvaAvaliacaoExterna.UnidadeAdministrativa;
                            
                            // Turma
                            aplicacaoProvaAvaliacaoExterna.DesTurma = importAplicacaoProvaAvaliacaoExterna.Turma;

                            // Período da realização da prova
                            aplicacaoProvaAvaliacaoExterna.NmPeriodo = importAplicacaoProvaAvaliacaoExterna.Periodo;

                            // Matrícula do Docente
                            aplicacaoProvaAvaliacaoExterna.Servidor = new Servidor();
                            aplicacaoProvaAvaliacaoExterna.Servidor.IdServidor = importAplicacaoProvaAvaliacaoExterna.Matricula;

                            // Data e hora da Prova
                            aplicacaoProvaAvaliacaoExterna.DtProva = new DateTime(
                                importAplicacaoProvaAvaliacaoExterna.Data.Year, importAplicacaoProvaAvaliacaoExterna.Data.Month, importAplicacaoProvaAvaliacaoExterna.Data.Day,
                                importAplicacaoProvaAvaliacaoExterna.Hora.Hour, importAplicacaoProvaAvaliacaoExterna.Hora.Minute, 0
                            );
                            
                            // Código da Avaliação
                            aplicacaoProvaAvaliacaoExterna.AvaliacaoExterna = new AvaliacaoExterna();
                            aplicacaoProvaAvaliacaoExterna.AvaliacaoExterna.IdAvaliacaoExterna = importAplicacaoProvaAvaliacaoExterna.Avaliacao;

                            aplicacaoProvaAvaliacaoExternaMapper.Insert(aplicacaoProvaAvaliacaoExterna);
                            AuditInsert(aplicacaoProvaAvaliacaoExterna, usuarioLogado, trans);
                        }

                        arquivoImportacaoMapper.UpdateStatus(arquivoImportacao, StatusImportacao.Concluido);                        
                    }
                }

                trans.Commit();
			}
			catch (Exception e)
			{
				DeleteLog(arquivoImportacao.IdArquivoImportacao);

				InsertLog(arquivoImportacao.IdArquivoImportacao, e.Message);

				UpdateStatus(arquivoImportacao, StatusImportacao.Falha);
			}
		}

		public void Delete(int idServidor, UserState usuario)
		{
			SqlConnection conn = GetConnection();
			SqlTransaction trans = null;
			AplicacaoProvaAvaliacaoExternaMapper mapper = new AplicacaoProvaAvaliacaoExternaMapper();

            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                mapper.connection = conn;
                mapper.transaction = trans;

                AplicacaoProvaAvaliacaoExterna aplicacaoProvaAvaliacaoExternaOld = mapper.Find(idServidor);

                mapper.Delete(idServidor);

                AuditDelete(aplicacaoProvaAvaliacaoExternaOld, usuario, trans);

                trans.Commit();
            }
            catch (Exception e)
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
            AplicacaoProvaAvaliacaoExternaMapper aplicacaoProvaAvaliacaoExternaMapper;
            string retorno = string.Empty;

            try
            {
                aplicacaoProvaAvaliacaoExternaMapper = new AplicacaoProvaAvaliacaoExternaMapper();

                conn.Open();

                aplicacaoProvaAvaliacaoExternaMapper.connection = conn;

                if (aplicacaoProvaAvaliacaoExternaMapper.Find(idServidor) != null)
                {
                    retorno = "Docente não fez lançamento de notas";
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

        public IList<AplicacaoProvaAvaliacaoExterna> List(int idAnoReferencia, int idServidor, int idUnidade)
        {
            IList<AplicacaoProvaAvaliacaoExterna> aplicacoesProvasAvaliacoesExternas = null;
            SqlConnection conn = GetConnection();
            AplicacaoProvaAvaliacaoExternaMapper aplicacaoProvaAvaliacaoExternaMapper;

            try
            {
                conn.Open();

                aplicacaoProvaAvaliacaoExternaMapper = new AplicacaoProvaAvaliacaoExternaMapper();
                aplicacaoProvaAvaliacaoExternaMapper.connection = conn;

                aplicacoesProvasAvaliacoesExternas = aplicacaoProvaAvaliacaoExternaMapper.ListBy(idAnoReferencia, idServidor, idUnidade);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return aplicacoesProvasAvaliacoesExternas;
        }
    }
}
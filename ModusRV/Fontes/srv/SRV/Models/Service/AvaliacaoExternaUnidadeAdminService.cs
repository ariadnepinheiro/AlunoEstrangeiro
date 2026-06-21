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
    public class AvaliacaoExternaUnidadeAdminService : ArquivoImportacaoService
    {

        public Paging<AvaliacaoExternaUnidadeAdmin> List(FiltroAvaliacaoExternaUnidadeAdmin filtro, int ciclo, int currentPage, int pageSize)
        {
            Paging<AvaliacaoExternaUnidadeAdmin> avaliacoes;
			
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                AvaliacaoExternaUnidadeAdminMapper mapper = new AvaliacaoExternaUnidadeAdminMapper();
                mapper.connection = conn;

                avaliacoes = mapper.List(filtro, ciclo, currentPage, pageSize);
            }

            return avaliacoes;
        }

        public void Delete(int IdAvaliacaoExterna, int idUnidadeAdministrativa, int ciclo, UserState usuario)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    AvaliacaoExternaUnidadeAdminMapper mapper = new AvaliacaoExternaUnidadeAdminMapper();
                    mapper.connection = conn;
                    mapper.transaction = trans;

                    AvaliacaoExternaUnidadeAdmin avaliacaoExternaUnidadeAdminOld = mapper.Find(IdAvaliacaoExterna, idUnidadeAdministrativa, ciclo);

                    mapper.Delete(IdAvaliacaoExterna, idUnidadeAdministrativa, ciclo);

                    AuditDelete(avaliacaoExternaUnidadeAdminOld, usuario, trans);

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

                CsvFileLoader<ImportAvaliacaoExternaUnidadeAdmin> loader = new CsvFileLoader<ImportAvaliacaoExternaUnidadeAdmin>(filename);

                List<ImportAvaliacaoExternaUnidadeAdmin> list = loader.Import();

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

                            AvaliacaoExternaMapper avaliacaoExternaMapper = new AvaliacaoExternaMapper();
                            avaliacaoExternaMapper.connection = conn;
                            avaliacaoExternaMapper.transaction = trans;

                            //Valida a participação e as chaves estrangeiras
                            for (int i = 0; i < list.Count; i++)
                            {
                                if (list[i].PercParticipacao >= 1000)
                                    ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'PercParticipacao'"));

                                if (!unidadeAdministrativaMapper.ExisteUnidadeAdministrativa(list[i].CodUnidadeAdministrativa))
                                    ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'CodUnidadeAdministrativa'"));

                                if (!avaliacaoExternaMapper.Valida(list[i].CodAvaliacaoExterna))
                                    ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'CodAvaliacaoExterna'"));
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
                                AvaliacaoExternaUnidadeAdminMapper mapper = new AvaliacaoExternaUnidadeAdminMapper();
                                mapper.connection = conn;
                                mapper.transaction = trans;

                                AvaliacaoExternaUnidadeAdmin avaliacaoExternaUnidadeAdmin, avaliacaoExternaUnidadeAdminOld;

                                foreach (var importAvaliacaoExternaUnidadeAdmin in list)
                                {
                                    //Carrega os dados do servidor
                                    avaliacaoExternaUnidadeAdmin = new AvaliacaoExternaUnidadeAdmin();

                                    avaliacaoExternaUnidadeAdmin.AvaliacaoExterna = new AvaliacaoExterna();
                                    avaliacaoExternaUnidadeAdmin.AvaliacaoExterna.IdAvaliacaoExterna = importAvaliacaoExternaUnidadeAdmin.CodAvaliacaoExterna;

                                    avaliacaoExternaUnidadeAdmin.UnidadeAdministrativa = new UnidadeAdministrativa();
                                    avaliacaoExternaUnidadeAdmin.UnidadeAdministrativa.IdUnidadeAdministrativa = importAvaliacaoExternaUnidadeAdmin.CodUnidadeAdministrativa;

                                    avaliacaoExternaUnidadeAdmin.AnoReferencia = new AnoReferencia();
                                    avaliacaoExternaUnidadeAdmin.AnoReferencia.IdAnoReferencia = usuarioLogado.Ciclo;

                                    avaliacaoExternaUnidadeAdmin.PercParticipacao = Decimal.Round(importAvaliacaoExternaUnidadeAdmin.PercParticipacao, 2);

                                    if (mapper.FindExiste(avaliacaoExternaUnidadeAdmin.AvaliacaoExterna.IdAvaliacaoExterna.Value, avaliacaoExternaUnidadeAdmin.UnidadeAdministrativa.IdUnidadeAdministrativa.Value, avaliacaoExternaUnidadeAdmin.AnoReferencia.IdAnoReferencia.Value))
                                    {
                                        avaliacaoExternaUnidadeAdminOld = mapper.Find(avaliacaoExternaUnidadeAdmin.AvaliacaoExterna.IdAvaliacaoExterna.Value, avaliacaoExternaUnidadeAdmin.UnidadeAdministrativa.IdUnidadeAdministrativa.Value, avaliacaoExternaUnidadeAdmin.AnoReferencia.IdAnoReferencia.Value);

                                        //Atualiza a avaliação externa
                                        mapper.Update(avaliacaoExternaUnidadeAdmin);

                                        AuditUpdate(avaliacaoExternaUnidadeAdmin, avaliacaoExternaUnidadeAdminOld, usuarioLogado, trans);

                                    }
                                    else
                                    {
                                        //Insere a avaliação externa
                                        mapper.Insert(avaliacaoExternaUnidadeAdmin);
                                        AuditInsert(avaliacaoExternaUnidadeAdmin, usuarioLogado, trans);

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

		public void Inserir(SqlConnection conn, SqlTransaction trans, AvaliacaoExternaUnidadeAdmin avaliacaoExternaUnidadeAdmin, UserState usuarioLogado)
		{
			//Insere registros de Avaliação Externa Unidade Administrativa
			//Atualiza status do arquivo para CONCLUÍDO
			AvaliacaoExternaUnidadeAdminMapper mapper = new AvaliacaoExternaUnidadeAdminMapper();
			mapper.connection = conn;
			mapper.transaction = trans;

			//Insere a avaliação externa
			mapper.Insert(avaliacaoExternaUnidadeAdmin);
			AuditInsert(avaliacaoExternaUnidadeAdmin, usuarioLogado, trans);			
		}

		public void DeleteAll(SqlConnection conn, SqlTransaction trans, UserState usuarioLogado)
		{
			AvaliacaoExternaUnidadeAdminMapper mapper = new AvaliacaoExternaUnidadeAdminMapper();
			mapper.connection = conn;
			mapper.transaction = trans;

			mapper.DeleteAll(usuarioLogado.Ciclo);

			AuditDeleteAvaliacaoExterna(new AvaliacaoExternaUnidadeAdmin(), usuarioLogado, trans);
		}

		private void AuditDeleteAvaliacaoExterna(AvaliacaoExternaUnidadeAdmin model, UserState usuario, SqlTransaction trans)
		{
			LogAuditoria logAuditoria = new LogAuditoria();
			logAuditoria.DesObjeto = model.GetType().Name;
			logAuditoria.TipoOperacao = OperacaoAuditoria.Exclusao;
			logAuditoria.Usuario = new Usuario() { Id = usuario.Id };
			logAuditoria.IpCliente = usuario.IPCliente;

			LogAuditoriaMapper logMapper = new LogAuditoriaMapper();
			logMapper.connection = trans.Connection;
			logMapper.transaction = trans;

			int? idLogAuditoria = logMapper.Insert(logAuditoria);

			LogAuditoriaItemMapper logItemMapper = new LogAuditoriaItemMapper();
			logItemMapper.connection = trans.Connection;
			logItemMapper.transaction = trans;

			LogAuditoriaItem item = new LogAuditoriaItem();

			item.DesAtributo = "TODOS OS REGISTROS";
			item.VlrAnterior = null;
			item.VlrAtual = null;
			logItemMapper.Insert(item, idLogAuditoria.Value);
		}
    }   
}
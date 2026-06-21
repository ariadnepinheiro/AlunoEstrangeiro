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
using System.Collections;

namespace SRV.Models.Service
{
	public class AvaliacaoExternaUnidadeAdminDetalheService : ArquivoImportacaoService
	{
		internal Paging<AvaliacaoExternaUnidadeAdminDetalhe> List(FiltroAvaliacaoExternaUnidadeAdminDetalhe filtroDetalhes, int currentPage, int pageSize)
		{
			Paging<AvaliacaoExternaUnidadeAdminDetalhe> detalhes;

			using (SqlConnection conn = GetConnection())
			{
				conn.Open();
				AvaliacaoExternaUnidadeAdminDetalheMapper mapper = new AvaliacaoExternaUnidadeAdminDetalheMapper();
				mapper.connection = conn;

				detalhes = mapper.List(filtroDetalhes, currentPage, pageSize);
			}

			return detalhes;
		}

		public void Import(ArquivoImportacao arquivoImportacao, UserState usuarioLogado)
        {
            List<string> ErrorRecords = new List<string>();

            try
            {
                //Faz a leitura de processamento do arquivo
                string filename = Path.Combine(ConfigurationManager.AppSettings["PathUpload"], arquivoImportacao.DesArquivo);

				CsvFileLoader<ImportAvaliacaoExternaUnidadeAdminDetalhe> loader = new CsvFileLoader<ImportAvaliacaoExternaUnidadeAdminDetalhe>(filename);

				List<ImportAvaliacaoExternaUnidadeAdminDetalhe> list = loader.Import();

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

							TurnoMapper turnoMapper = new TurnoMapper();
							turnoMapper.connection = conn;
							turnoMapper.transaction = trans;

                            //Valida a participação e as chaves estrangeiras
                            for (int i = 0; i < list.Count; i++)
                            {
								if (list[i].AnoReferencia != usuarioLogado.Ciclo)
                                    ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'CICLO'"));

                                if (!unidadeAdministrativaMapper.ExisteUnidadeAdministrativa(list[i].IdUnidadeAdministrativa))
									ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'UNIDADE_ADMINISTRATIVA'"));

								if (!unidadeAdministrativaMapper.ExistePor(list[i].IdUnidadeAdministrativa, list[i].CensoUnidadeAdministrativa))
									ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'CENSO_UNIDADE'"));

                                if (!avaliacaoExternaMapper.Valida(list[i].AvaliacaoExterna))
                                    ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'AVALIAÇÃO'"));

								if (!turnoMapper.Valida(list[i].Turno))
									ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'Turno'"));

								if(!Enum.IsDefined(typeof(Periodo), list[i].Periodo))
									ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'Periodo'"));

								if(list[i].Previsto == 0)
									ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'Previsto'"));
								
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
								#region variáveis locais
								AvaliacaoExternaUnidadeAdmin avaliacaoExternaUnidadeAdmin = new AvaliacaoExternaUnidadeAdmin();
								AvaliacaoExternaUnidadeAdminDetalhe avaliacaoExternaUnidadeAdminDetalhe = new AvaliacaoExternaUnidadeAdminDetalhe();
								avaliacaoExternaUnidadeAdminDetalhe.AnoReferencia = new AnoReferencia();
								avaliacaoExternaUnidadeAdminDetalhe.AvaliacaoExterna = new AvaliacaoExterna();
								avaliacaoExternaUnidadeAdminDetalhe.UnidadeAdministrativa = new UnidadeAdministrativa();

								AvaliacaoExternaUnidadeAdminDetalheMapper avaliacaoExternaUnidadeAdminDetalheMapper = new AvaliacaoExternaUnidadeAdminDetalheMapper();
								avaliacaoExternaUnidadeAdminDetalheMapper.connection = conn;
								avaliacaoExternaUnidadeAdminDetalheMapper.transaction = trans;

								AvaliacaoExternaUnidadeAdminService avaliacaoExternaUnidadeAdminService = new AvaliacaoExternaUnidadeAdminService();

								IEnumerable<KeyValuePair<int, int>> idsUnidadesAdministrativasEAvaliacoesExternas = list.Select(x => new KeyValuePair<int, int>(x.IdUnidadeAdministrativa, x.AvaliacaoExterna)).Distinct();
								IEnumerable<int> idsUnidadesAdministrativas = idsUnidadesAdministrativasEAvaliacoesExternas.Select(a => a.Key).Distinct();
								bool atingiuPercentualDiurno = true, atingiuPercentualNoturno = true;
								decimal? percentualDiurno, percentualNoturno;
								decimal percentualGeral;

								TipoCriterioElegibilidadeService tipoCriterioElegibilidadeService = new TipoCriterioElegibilidadeService();
								decimal metaIndividualDiurna = tipoCriterioElegibilidadeService.ObtemValorPor((int) CriteriosAvaliacaoExternaEnum.ParticipacaoDiurnaIndividual)/100;
								decimal metaIndividualNoturna = tipoCriterioElegibilidadeService.ObtemValorPor((int) CriteriosAvaliacaoExternaEnum.ParticipacaoNoturnaIndividual)/100;
								decimal metaGeralDiurna = tipoCriterioElegibilidadeService.ObtemValorPor((int) CriteriosAvaliacaoExternaEnum.ParticipacaoDiurnaMedia)/100;
								decimal metaGeralNoturna = tipoCriterioElegibilidadeService.ObtemValorPor((int) CriteriosAvaliacaoExternaEnum.ParticipacaoNoturnaMedia)/100;

								#endregion

								//Limpa todos os registros de avaliações externas detalhadas que possuem ano de referência igual ao ciclo de gestão selecionado
								avaliacaoExternaUnidadeAdminDetalheMapper.DeleteAll(usuarioLogado.Ciclo);								

								//Limpa os registros da tabela global de avaliação externa e gera auditoria
								avaliacaoExternaUnidadeAdminService.DeleteAll(conn, trans, usuarioLogado);

								foreach (var importAvaliacaoExternaUnidadeAdminDetalhe in list)
                                {									
									avaliacaoExternaUnidadeAdminDetalhe.AnoReferencia.IdAnoReferencia = importAvaliacaoExternaUnidadeAdminDetalhe.AnoReferencia;								
									avaliacaoExternaUnidadeAdminDetalhe.AvaliacaoExterna.IdAvaliacaoExterna = importAvaliacaoExternaUnidadeAdminDetalhe.AvaliacaoExterna;									
									avaliacaoExternaUnidadeAdminDetalhe.UnidadeAdministrativa.IdUnidadeAdministrativa = importAvaliacaoExternaUnidadeAdminDetalhe.IdUnidadeAdministrativa;
									
									avaliacaoExternaUnidadeAdminDetalhe.Turno = turnoMapper.Find(importAvaliacaoExternaUnidadeAdminDetalhe.Turno);
									avaliacaoExternaUnidadeAdminDetalhe.Turma = importAvaliacaoExternaUnidadeAdminDetalhe.Turma;
									avaliacaoExternaUnidadeAdminDetalhe.Periodo = (Periodo) importAvaliacaoExternaUnidadeAdminDetalhe.Periodo;
									avaliacaoExternaUnidadeAdminDetalhe.Previsto = importAvaliacaoExternaUnidadeAdminDetalhe.Previsto;
									avaliacaoExternaUnidadeAdminDetalhe.Realizado = importAvaliacaoExternaUnidadeAdminDetalhe.Realizado;

                                    //Insere a avaliação externa detalhada
									avaliacaoExternaUnidadeAdminDetalheMapper.Insert(avaliacaoExternaUnidadeAdminDetalhe);
								}

								//Verifica os percentuais diurno e noturno e grava na avaliacao externa 
								#region Calculo de percentuais globais
															
								foreach (var idUnidadeAdministrativa in idsUnidadesAdministrativas)
								{
									foreach (int idAvaliacaoExterna in idsUnidadesAdministrativasEAvaliacoesExternas.Where(x => x.Key == idUnidadeAdministrativa).Select(x => x.Value).Distinct())
									{
										atingiuPercentualDiurno = true;
										atingiuPercentualNoturno = true;
										percentualDiurno = null;
										percentualNoturno = null;
										percentualGeral = 0;

                                        // Percentual diurno
										if (avaliacaoExternaUnidadeAdminDetalheMapper.PossuiTurno(idUnidadeAdministrativa, GrupoTurno.D, usuarioLogado.Ciclo))
										{
											percentualDiurno = avaliacaoExternaUnidadeAdminDetalheMapper.CalculoPercentualAtingidoPor(idUnidadeAdministrativa, idAvaliacaoExterna, GrupoTurno.D, usuarioLogado.Ciclo);
											
											if (!avaliacaoExternaUnidadeAdminDetalheMapper.MetasIndividuaisAtingidas(idUnidadeAdministrativa, idAvaliacaoExterna, GrupoTurno.D, usuarioLogado.Ciclo, metaIndividualDiurna))
											{												
												if (percentualDiurno.HasValue && percentualDiurno < metaGeralDiurna)
													atingiuPercentualDiurno = false;
											}
										}

                                        // Percentual noturno
										if (avaliacaoExternaUnidadeAdminDetalheMapper.PossuiTurno(idUnidadeAdministrativa, GrupoTurno.N, usuarioLogado.Ciclo))
										{
											percentualNoturno = avaliacaoExternaUnidadeAdminDetalheMapper.CalculoPercentualAtingidoPor(idUnidadeAdministrativa, idAvaliacaoExterna, GrupoTurno.N, usuarioLogado.Ciclo);
											
											if (!avaliacaoExternaUnidadeAdminDetalheMapper.MetasIndividuaisAtingidas(idUnidadeAdministrativa, idAvaliacaoExterna, GrupoTurno.N, usuarioLogado.Ciclo, metaIndividualNoturna))
											{												
												if (percentualNoturno.HasValue && percentualNoturno < metaGeralNoturna)
													atingiuPercentualNoturno = false;
											}
										}

										if (atingiuPercentualDiurno && atingiuPercentualNoturno)
											percentualGeral = 0.1m;

										avaliacaoExternaUnidadeAdmin = new AvaliacaoExternaUnidadeAdmin
										{
											AnoReferencia = new AnoReferencia { IdAnoReferencia = usuarioLogado.Ciclo },
											AvaliacaoExterna = new AvaliacaoExterna { IdAvaliacaoExterna = idAvaliacaoExterna },
											UnidadeAdministrativa = new UnidadeAdministrativa { IdUnidadeAdministrativa = idUnidadeAdministrativa },
											PercParticipacao = percentualGeral,
											PercParticipacaoDiurno = percentualDiurno,
											PercParticipacaoNoturno = percentualNoturno
										};
										
										avaliacaoExternaUnidadeAdminService.Inserir(conn, trans, avaliacaoExternaUnidadeAdmin, usuarioLogado);
									}
								}

								#endregion

								//Atualiza status do arquivo para CONCLUÍDO
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

		private void AuditDeleteAvaliacaoExternaDetalhe(AvaliacaoExternaUnidadeAdminDetalhe model, UserState usuario, SqlTransaction trans)
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
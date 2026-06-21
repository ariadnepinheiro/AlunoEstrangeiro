using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Proderj.DOL.Domain;
using Proderj.DOL.Exception;
using Proderj.DOL.Repository;
using Proderj.Foundation.Framework;
using Proderj.Foundation.Common;

namespace Proderj.DOL.Service
{
	public class RespostaCurriculoMinimoService : IRespostaCurriculoMinimoService
	{
		private readonly ICompetenciaHabilidadeDocenteRepository repositorioHabilidadeDocente;
		private readonly ICompetenciaHabilidadeGrupoRepository repositorioHabilidadeGrupo;
	
		public RespostaCurriculoMinimoService(ICompetenciaHabilidadeDocenteRepository repositorioHabilidadeDocente, ICompetenciaHabilidadeGrupoRepository repositorioHabilidadeGrupo)
		{
			this.repositorioHabilidadeDocente = repositorioHabilidadeDocente;
			this.repositorioHabilidadeGrupo = repositorioHabilidadeGrupo;
		}

		public DTORespostaCurriculoMinimo_StatusPreenchimentoPorTurma ObtemStatusPreenchimentoPor(DTORespostaCurriculoMinimo_ObtemStatusPreenchimentoPor dtoVerificacaoStatus)
		{
			
			var dtoStatusPreenchimentoCurriculo = new DTORespostaCurriculoMinimo_StatusPreenchimentoPorTurma
			                                      	{
			                                      		StatusPreenchimento =
			                                      			StatusPreenchimentoCurriculoMinimoPorTurmaEnum.NaoSeAplica
			                                      	};

			var parametrosObtemQuantidade = new CompetenciaHabilidadeGrupo
			                                	{
			                                		Ano = dtoVerificacaoStatus.Ano,
			                                		Curso = dtoVerificacaoStatus.CodigoCurso,
			                                		Serie = dtoVerificacaoStatus.Serie,
			                                		Modalidade = dtoVerificacaoStatus.CodigoModalidade,
			                                		SubPeriodo = dtoVerificacaoStatus.Subperiodo,
			                                		Disciplina = dtoVerificacaoStatus.CodigoDisciplina,
			                                		Periodo = dtoVerificacaoStatus.Periodo,
			                                		TipoCurso = dtoVerificacaoStatus.TipoCurso
			                                	};


			int quantidadeItensResposta = repositorioHabilidadeGrupo.QuantidadeItensRespostaPor(parametrosObtemQuantidade);

			if (quantidadeItensResposta > 0)
			{
				//Se existirem repostas para algum dos grupos de competencia desta disciplina
				var parametroObtemCompetenciasDocente = new CompetenciaHabilidadeDocente
				                                        	{
				                                        		Matricula = dtoVerificacaoStatus.MatriculaDocente,
																Turma = dtoVerificacaoStatus.CodigoTurma,
																SubPeriodo = dtoVerificacaoStatus.Subperiodo,
																Disciplina = dtoVerificacaoStatus.CodigoDisciplina,
																Ano = dtoVerificacaoStatus.Ano,
																Periodo = dtoVerificacaoStatus.Periodo

				                                        	};

				List<CompetenciaHabilidadeDocente> listaCompetenciasDocenteTurma =
					repositorioHabilidadeDocente.EnumeraCompetencias(parametroObtemCompetenciasDocente)
					.ToList();

				//Se nao tiver nenhuma resposta
				if (listaCompetenciasDocenteTurma.Count == 0)
				{
					dtoStatusPreenchimentoCurriculo.StatusPreenchimento = StatusPreenchimentoCurriculoMinimoPorTurmaEnum.Pendente;
				}

				//Se a quantidade de respostas for igual a numero de "perguntas".
				else if (listaCompetenciasDocenteTurma.Count >= quantidadeItensResposta)
				{
					dtoStatusPreenchimentoCurriculo.StatusPreenchimento = StatusPreenchimentoCurriculoMinimoPorTurmaEnum.Completo;
					dtoStatusPreenchimentoCurriculo.DataUltimoPreenchimento = listaCompetenciasDocenteTurma.First().DataCadastro;
				}

				//Se a quantidade de resposta for inferior ao numero de "perguntas" (Parcial)
				else
				{
					dtoStatusPreenchimentoCurriculo.StatusPreenchimento = StatusPreenchimentoCurriculoMinimoPorTurmaEnum.Parcial;
					dtoStatusPreenchimentoCurriculo.DataUltimoPreenchimento = listaCompetenciasDocenteTurma.First().DataCadastro;
					dtoStatusPreenchimentoCurriculo.PercentualPreenchimentoParcial = ((decimal)listaCompetenciasDocenteTurma.Count/quantidadeItensResposta)*100;
				}
			}
			return dtoStatusPreenchimentoCurriculo;
		}

		public void VerificaPermissaoParaListar(DTORespostaCurriculoMinimo_VerificaPermissaoParaListar dtoSolicitacao)
		{
			var fabricaServico = new NinjectFactoryBase<NinjectModuloServico>();
			SelecaoTurmasService servicoDeTurma = fabricaServico.Obtem<SelecaoTurmasService>();

			List<DTOSelecaoTurmas> listaTurmasComAcesso = servicoDeTurma.EnumeraSelecaoTurmasPor(dtoSolicitacao.NumeroFuncionarioDocente).ToList();

			//Verifica se os dados de solicitação de lançamento de curriculo minimo existem no escopo deste docente
			DTOSelecaoTurmas turmaComAcesso = listaTurmasComAcesso.FirstOrDefault(turmas =>
												turmas.Serie == dtoSolicitacao.Serie &&
												turmas.Semestre == dtoSolicitacao.Periodo &&
												turmas.Ano == dtoSolicitacao.Ano &&
												turmas.Curso == dtoSolicitacao.CodigoCurso &&
												turmas.Disciplina == dtoSolicitacao.CodigoDisciplina &&
												turmas.Modalidade == dtoSolicitacao.CodigoModalidade &&
												turmas.UnidadeEnsino == dtoSolicitacao.CodigoUnidadeEnsino &&
												turmas.Turma == dtoSolicitacao.CodigoTurma &&
												turmas.Tipo == dtoSolicitacao.TipoCurso
											);

			
			if (turmaComAcesso == null)
			{
				throw new RespostaCurriculoMinimoException(RespostaCurriculoMinimoException.TipoEnum.AcessoNegadoDocente_A_TurmaEDisciplina);
			}

			//Se tiver acesso, verificar se ainda pode lançar curriculo minimo...
			if (!turmaComAcesso.ValidoParaLancamento)
			{
				var excecao = new RespostaCurriculoMinimoException(RespostaCurriculoMinimoException.TipoEnum.TurmaEDisciplinaInvalidaParaLancamento)
				{
					CodigoTurma = dtoSolicitacao.CodigoTurma
				};

				throw excecao;
			}

			//throw new NotImplementedException();
		}

		public void VerificaPermissaoParaSalvar(DTORespostaCurriculoMinimo_VerificaPermissaoParaSalvar dtoSolicitacao)
		{
			//Mapper.CreateMap<DTOCurriculoMinimo_VerificaPermissaoParaSalvarCurriculoMinimo, DTOCurriculoMinimoSolicitacaoLancaRespostas>();
			//DTOCurriculoMinimoSolicitacaoLancaRespostas dtoSolicitacaoLanca =
			//	Mapper.Map<DTOCurriculoMinimo_VerificaPermissaoParaSalvarCurriculoMinimo, DTOCurriculoMinimoSolicitacaoLancaRespostas>(dtoSolicitacao);
			//
			//VerificaPermissaoParaLancarCurriculoMinimo(dtoSolicitacao.NumericoFuncionarioDocente, dtoSolicitacaoLanca);
			
			List<DTORespostaCurriculoMinimo_RespostasPorGrupo> listaRespostasPorGrupoValidas =
				EnumeraRespostasPorGrupoPor(new DTORespostaCurriculoMinimo_EnumeraRespostasPorGrupoPor
				                            	{
				                            		Ano = dtoSolicitacao.Ano,
													CodigoTurma = dtoSolicitacao.CodigoTurma,
													Curso = dtoSolicitacao.CodigoCurso,
													Disciplina = dtoSolicitacao.CodigoDisciplina,
													Matricula = dtoSolicitacao.MatriculaDocente,
													Modalidade = dtoSolicitacao.CodigoModalidade,
													Periodo = dtoSolicitacao.Periodo,
													Serie = dtoSolicitacao.Serie,
													SubPeriodo = dtoSolicitacao.SubPeriodo,
													TipoCurso = dtoSolicitacao.TipoCurso
				                            	}).ToList();

			//Validar se todas as respostas informadas fazem parte do contexto desse deste professor e currículo mínimo
			foreach( DTORespostaCurriculoMinimo_RespostasPorGrupo respostaDada in dtoSolicitacao.ListaResposta)
			{
				if (!listaRespostasPorGrupoValidas.Any(respValida => respValida.CodigoGrupo == respostaDada.CodigoGrupo && respValida.CodigoResposta == respostaDada.CodigoResposta))
				{
					throw new RespostaCurriculoMinimoException(RespostaCurriculoMinimoException.TipoEnum.RespostasInformadasInvalidas);
				}
			}
		}

		public IEnumerable<DTORespostaCurriculoMinimo_RespostasPorGrupo> EnumeraRespostasPorGrupoPor(DTORespostaCurriculoMinimo_EnumeraRespostasPorGrupoPor dtoSolicitacao)
		{
			//Mapper.CreateMap<DTORespostaCurriculoMinimo_EnumeraRespostasPorGrupoPor, TOCompetenciaHabilidadeGrupo_EnumeraPor>();
			TOCompetenciaHabilidadeGrupo_EnumeraPor voSolicitacao =
				Mapper.Map<DTORespostaCurriculoMinimo_EnumeraRespostasPorGrupoPor, TOCompetenciaHabilidadeGrupo_EnumeraPor>(dtoSolicitacao);

			int itens = 0;
			foreach (VOCompetenciaHabilidadeGrupoComResposta respostaPorGrupo in repositorioHabilidadeGrupo.EnumeraComRespostaPor(voSolicitacao))
			{
				itens++;
				yield return new DTORespostaCurriculoMinimo_RespostasPorGrupo
				             	{
									DescricaoGrupo = respostaPorGrupo.DescricaoGrupo,
									DescricaoResposta = respostaPorGrupo.DescricaoCompetenciaHabilidade,
									OrdemGrupo = respostaPorGrupo.OrdemGrupo,
									OrdemResposta = respostaPorGrupo.OrdemResposta,
									CodigoGrupo = respostaPorGrupo.IdCompetenciaHabilidadeGrupo,
									CodigoResposta = respostaPorGrupo.IdCompetenciaHabilidadeItem,
									Respondido = respostaPorGrupo.Resposta,
				             	};
			}

			if (itens == 0)
			{
				throw new RespostaCurriculoMinimoException(RespostaCurriculoMinimoException.TipoEnum.NaoExistemDadosDeGruposERespostasParaLancamento)
				      	{
				      		Subperiodo = dtoSolicitacao.SubPeriodo
				      	};
			}
		}
		
		public void SalvaPor(DTORespostaCurriculoMinimo_SalvaPor dtoCurriculoMinimo)
		{
			var fabricaServico = new NinjectFactoryBase<NinjectModuloServico>();
			LogCompetenciaHabilidadeDocenteService servicoLogCompetenciaHabilidadeDocente = fabricaServico.Obtem<LogCompetenciaHabilidadeDocenteService>();

			//Mapper.CreateMap<DTORespostaCurriculoMinimo_SalvaPor, DTOLogCompetenciaHabilidade>();
			DTOLogCompetenciaHabilidade dtoSolicitacao = Mapper.Map<DTORespostaCurriculoMinimo_SalvaPor, DTOLogCompetenciaHabilidade>(dtoCurriculoMinimo);

			servicoLogCompetenciaHabilidadeDocente.InserePorCompetenciaHabilidadeItemPor(dtoSolicitacao);

			//Mapper.CreateMap<DTORespostaCurriculoMinimo_SalvaPor, TOCompetenciaHabilidadeDocente_RemoveCompetenciasAntigas>();
			TOCompetenciaHabilidadeDocente_RemoveCompetenciasAntigas voRemoveCompetenciasAntigas = Mapper.Map<DTORespostaCurriculoMinimo_SalvaPor, TOCompetenciaHabilidadeDocente_RemoveCompetenciasAntigas>(dtoCurriculoMinimo);

			repositorioHabilidadeDocente.RemoveCompetenciasAntigas(voRemoveCompetenciasAntigas);

			var competenciasHabilidadeDocente = new List<TOCompetenciaHabilidade_Insere>();

			dtoCurriculoMinimo.IdsCompetenciaHabilidadeItem.ExecuteForEach
			( id=>
				{
					var competenciaHabilidadeDocente = new TOCompetenciaHabilidade_Insere
					{
						Ano = dtoCurriculoMinimo.Ano,
						CodigoDisciplina = dtoCurriculoMinimo.CodigoDisciplina,
						CodigoTurma = dtoCurriculoMinimo.CodigoTurma,
						Matricula = dtoCurriculoMinimo.Matricula,
						Periodo = dtoCurriculoMinimo.Periodo,
						SubPeriodo = dtoCurriculoMinimo.Subperiodo,
						IdCompetenciaHabilidadeItem = id
					};
					competenciasHabilidadeDocente.Add(competenciaHabilidadeDocente);
				}
			);

			bool retornoOk = repositorioHabilidadeDocente.Insere(competenciasHabilidadeDocente, true);

			if (!retornoOk)
			{
				throw new RespostaCurriculoMinimoException(RespostaCurriculoMinimoException.TipoEnum.OsItensNaoPuderamSerSalvos);
			}
		}

		public bool ExisteAvaliacaoParaResponderPor(short ano, short periodo, short subperiodo)
		{
			throw new NotImplementedException();
		}
	}
}

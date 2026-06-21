using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proderj.DOL.Service;
using Proderj.DOL.WebApp.Models;
using AutoMapper;
using Resources;
using Proderj.DOL.Exception;

namespace Proderj.DOL.WebApp.Controllers
{
	public class AvaliacaoCurriculoMinimoController : ControllerPadrao
	{
		private readonly IAvaliacaoCurriculoMinimoService avaliacaoCurriculoMinimoServico;
		private readonly ISubPeriodoLetivoService subPeriodoLetivoServico;

		public AvaliacaoCurriculoMinimoController(IAvaliacaoCurriculoMinimoService avaliacaoCurriculoMinimoServico, ISubPeriodoLetivoService subPeriodoLetivoServico)
		{
			this.avaliacaoCurriculoMinimoServico = avaliacaoCurriculoMinimoServico;
			this.subPeriodoLetivoServico = subPeriodoLetivoServico;
		}

		[HttpPost]
		[LogadoComTermoAceito]
		public ActionResult Lista(DocenteLogadoBindModel docenteLogadoModelo, AvaliacaoCurriculoMinimoListaRequestModel solicitacaoModelo)
		{
			//Valida os pre-requisitos.
			PrerequisitoResult preRequisitoLancaRespostas = PreRequisitoLista(docenteLogadoModelo, solicitacaoModelo);
			if (!preRequisitoLancaRespostas.EhValido)
				return preRequisitoLancaRespostas;

			ViewResult viewSaida = preRequisitoLancaRespostas;
			var modeloLista = (AvaliacaoCurriculoMinimoListaViewModel)preRequisitoLancaRespostas.Modelo; 

			if (ModelState.IsValid)
			{
				try
				{
					modeloLista.DadosAvaliacoesEJustificativas =
						avaliacaoCurriculoMinimoServico.ObtemAvaliacoesEJustificativaPor(solicitacaoModelo.Ano, solicitacaoModelo.Periodo,
						                                                                 modeloLista.BimestreSelecionado,
						                                                                 docenteLogadoModelo.Matricula);

					viewSaida = View("Lista", modeloLista);

				}
				catch (AvaliacaoCurriculoMinimoException excecaoAvaliacaoCurriculoMinimo)
				{
                    viewSaida = null;
					ModelState.AddModelError(excecaoAvaliacaoCurriculoMinimo.TipoDeErro, excecaoAvaliacaoCurriculoMinimo.Message);
				}

				catch (System.Exception excecao)
				{
					viewSaida = View("ErroInesperado", excecao);
				}
			}
			if (viewSaida == null)
			{
				var fabricaDeController = (NinjectControllerFactory)ControllerBuilder.Current.GetControllerFactory();
				var selecaoTurmaController = (SelecaoTurmasController)fabricaDeController.CriaController(typeof(SelecaoTurmasController));
				viewSaida = selecaoTurmaController.Lista(docenteLogadoModelo, "RespostaCurriculoMinimo", this, solicitacaoModelo.CodigoTurma);
				((SelecaoTurmasListaViewModel) viewSaida.ViewData.Model).CodigoTurmaErro = solicitacaoModelo.CodigoTurma;
			}
			return viewSaida;
		}

		[HttpPost]
		[LogadoComTermoAceito]
		public ActionResult Salva(DocenteLogadoBindModel modeloDocenteLogado, AvaliacaoCurriculoMinimoSalvaRequestModel modeloSolicitacao)
		{
			//Valida os pre-requisitos.
			PrerequisitoResult preRequisitoLista = PreRequisitoLista(modeloDocenteLogado, modeloSolicitacao);
			if (!preRequisitoLista.EhValido)
				return preRequisitoLista;

			ViewResult viewSaida = preRequisitoLista;
			var modeloLista = (AvaliacaoCurriculoMinimoListaViewModel)preRequisitoLista.Modelo;

			PrerequisitoResult preRequisitoSalva = PreRequisitoSalva(modeloLista, modeloDocenteLogado, modeloSolicitacao);
			if (!preRequisitoSalva.EhValido)
				return preRequisitoSalva;

			//Passou nos pré-requisitos

			if (ModelState.IsValid)
			{
                try
                {
                    var dtoAvaliacoesResposta = new DTOAvaliacaoCurriculoMinimo_SalvaAvaliacoesEJustificativaPor
                                                    {
                                                        Ano = modeloSolicitacao.Ano,
                                                        Periodo = modeloSolicitacao.Periodo,
                                                        SubPeriodo = modeloSolicitacao.Subperiodo.Value,
                                                        DescricaoJustificativa = modeloSolicitacao.DescricaoJustificativa,
                                                        Matricula = modeloDocenteLogado.Matricula,
                                                        AvaliacoesCurriculoMinimo = modeloSolicitacao.ListaAvaliacao.ConvertAll(
                                                            resposta =>
                                                            new DTOAvaliacaoCurriculoMinimo_RespostaAvaliacao
                                                                {
                                                                    IdAvaliacaoCurriculoMinimo = resposta.Codigo,
                                                                    EhAvaliadoPositivamente =

                                                                        //Converte Enum? para Bool?
                                                                        (resposta.Resposta == null)
                                                                            ? null
                                                                            : (bool?)
                                                                              (resposta.Resposta.Value ==
                                                                               AvaliacaoCurriculoMinimoSalvaRequestModel.RespostaAvaliacao.RespostaEnum.Sim)
                                                                })
                                                    };

                    avaliacaoCurriculoMinimoServico.SalvaAvaliacoesEJustificativaPor(dtoAvaliacoesResposta);

                    //Carrega as avaliações persistidas
                    modeloLista.DadosAvaliacoesEJustificativas =
                        avaliacaoCurriculoMinimoServico.ObtemAvaliacoesEJustificativaPor(modeloSolicitacao.Ano, modeloSolicitacao.Periodo,
                                                                                         modeloLista.BimestreSelecionado,
                                                                                         modeloDocenteLogado.Matricula);

                    modeloLista.LancamentoPersistido = true;
                    modeloLista.MensagemLancamentoPersistido =
                        Recurso.AvaliacaoCurriculoMinimoSalva_MensagemLancamentoPersistido;

                }

                catch (AvaliacaoCurriculoMinimoException excecaoAvaliacaoCurriculoMinimo)
                {
                    // persisto a lista preenchida para o View
                    modeloLista.DadosAvaliacoesEJustificativas =
                        avaliacaoCurriculoMinimoServico.ObtemAvaliacoesEJustificativaPor(modeloSolicitacao.Ano, modeloSolicitacao.Periodo,
                                                                                         modeloLista.BimestreSelecionado,
                                                                                         modeloDocenteLogado.Matricula);

                    modeloLista.DadosAvaliacoesEJustificativas.DescricaoJustificativa = modeloSolicitacao.DescricaoJustificativa;
                    modeloLista.DadosAvaliacoesEJustificativas.ListaAvaliacaoCurriculoMinimo = modeloSolicitacao.ListaAvaliacao.ConvertAll(
                        aval => new DTOAvaliacaoCurriculoMinimo_RespostaAvaliacao()
                        {
                            DescricaoAvaliacao =
                                modeloLista.DadosAvaliacoesEJustificativas.ListaAvaliacaoCurriculoMinimo
                                    .Single(resp => resp.IdAvaliacaoCurriculoMinimo == aval.Codigo).DescricaoAvaliacao,

                            IdAvaliacaoCurriculoMinimo = aval.Codigo,
                            EhAvaliadoPositivamente =
                                (aval.Resposta == null) ? null : (bool?)(aval.Resposta.Value == AvaliacaoCurriculoMinimoSalvaRequestModel.RespostaAvaliacao.RespostaEnum.Sim)
                        }
                    );

                    ModelState.AddModelError(excecaoAvaliacaoCurriculoMinimo.TipoDeErro, excecaoAvaliacaoCurriculoMinimo.Message);

                    viewSaida = View("Lista", modeloLista);
                }

                catch (System.Exception ex)
                {
                    viewSaida = View("ErroInesperado", ex);
                }
			}
			else
			{
				//Tentando exibir o erro amigavelmente
				try
				{
					//Carrega as avaliações para responder e exibe o erro.
					modeloLista.DadosAvaliacoesEJustificativas =
						avaliacaoCurriculoMinimoServico.ObtemAvaliacoesEJustificativaPor(modeloSolicitacao.Ano, modeloSolicitacao.Periodo,
						                                                                 modeloLista.BimestreSelecionado,
						                                                                 modeloDocenteLogado.Matricula);

					//Atualiza as opções marcadas pelo usuário
					foreach (var resposta in modeloLista.DadosAvaliacoesEJustificativas.ListaAvaliacaoCurriculoMinimo)
					{
						AvaliacaoCurriculoMinimoSalvaRequestModel.RespostaAvaliacao respostaDada =
							modeloSolicitacao.ListaAvaliacao.FirstOrDefault(
								avaliacao => avaliacao.Codigo == resposta.IdAvaliacaoCurriculoMinimo);

						if (respostaDada != null)
						{
							resposta.EhAvaliadoPositivamente = respostaDada.Resposta.Value ==
							                                   AvaliacaoCurriculoMinimoSalvaRequestModel.RespostaAvaliacao.RespostaEnum.Sim;
						}
					}
				}
				catch (System.Exception ex)
				{
					viewSaida = View("ErroInesperado", ex);
				}
			}

			if (viewSaida == null)
			{
				viewSaida = View("Lista", modeloLista);
			}

			return viewSaida;
		}

		private AvaliacaoCurriculoMinimoListaViewModel CarregaDadosBimestreSelecionadoModelo(AvaliacaoCurriculoMinimoListaViewModel modeloOrigem, ICurriculoMinimoListaPrerequisito solicitacaoModelo)
		{
			modeloOrigem.BimestresHabilitadosGeral = subPeriodoLetivoServico.EnumeraPor(solicitacaoModelo.Ano, solicitacaoModelo.Periodo).ToList();

			//Verifica se existe bimestre informado na solicitação
			if (solicitacaoModelo.Subperiodo.HasValue)
			{
				modeloOrigem.BimestreSelecionado = solicitacaoModelo.Subperiodo.Value;
			}
			else
			{
				//Senao tiver, pega o primeiro ativo (define 0).
				modeloOrigem.BimestreSelecionado = 0;
			}

			return modeloOrigem;
		}

		private AvaliacaoCurriculoMinimoListaViewModel CarregaDadosBimestresValidosParaCurriculoMinimoModelo(AvaliacaoCurriculoMinimoListaViewModel modeloOrigem, ICurriculoMinimoListaPrerequisito solicitacaoModelo)
		{
			modeloOrigem.BimestresHabilitadosCurriculoMinimo = new List<DTOSubPeriodoLetivoAtivo>();

			var dtoSolicitacaoExibicaoCurriculoMinimo = new DTOSubPeriodoLetivo_EnumeraParaExibicaoDeCurriculoMinimoPor
			{
				Ano = solicitacaoModelo.Ano,
				Curso = solicitacaoModelo.CodigoCurso,
				Disciplina = solicitacaoModelo.CodigoDisciplina,
				Modalidade = solicitacaoModelo.CodigoModalidade,
				Periodo = solicitacaoModelo.Periodo,
				Serie = solicitacaoModelo.Serie,
				TipoCurso = solicitacaoModelo.TipoCurso,
				SubPeriodo = modeloOrigem.BimestreSelecionado
			};

			List<DTOSubPeriodoLetivoAtivo> listaBimestresCurriculosMinimoExibicao = subPeriodoLetivoServico.ListaParaExibicaoDeCurriculoMinimoPor(dtoSolicitacaoExibicaoCurriculoMinimo).ToList();
			modeloOrigem.BimestresHabilitadosCurriculoMinimo = listaBimestresCurriculosMinimoExibicao;
			
            if (modeloOrigem.BimestreSelecionado == 0)
			{
				modeloOrigem.BimestreSelecionado = listaBimestresCurriculosMinimoExibicao[0].SubPeriodo;
			}

			return modeloOrigem;
		}

		private PrerequisitoResult PreRequisitoLista(DocenteLogadoBindModel docenteLogadoModelo, ICurriculoMinimoListaPrerequisito dadosPrerequisito)
		{
			//Se correr tudo bem...
			//Carrega modelo..
			var fabricaDeController = (NinjectControllerFactory)ControllerBuilder.Current.GetControllerFactory();
			var selecaoTurmaController = (SelecaoTurmasController)fabricaDeController.CriaController(typeof(SelecaoTurmasController));

			PrerequisitoResult retorno = null;
			try
			{
				avaliacaoCurriculoMinimoServico.VerificaPermissaoParaListar(new DTOAvaliacaoCurriculoMinimo_VerificaPermissaoParaListar()
				{
					Ano = dadosPrerequisito.Ano,
					CodigoCurso = dadosPrerequisito.CodigoCurso,
					CodigoDisciplina = dadosPrerequisito.CodigoDisciplina,
					CodigoModalidade = dadosPrerequisito.CodigoModalidade,
					CodigoTurma = dadosPrerequisito.CodigoTurma,
					CodigoUnidadeEnsino = dadosPrerequisito.CodigoUnidadeEnsino,
					NumeroFuncionarioDocente = docenteLogadoModelo.NumeroFuncionario,
					Periodo = dadosPrerequisito.Periodo,
					Serie = dadosPrerequisito.Serie,
					TipoCurso = dadosPrerequisito.TipoCurso
				});

				var modelo = new AvaliacaoCurriculoMinimoListaViewModel(docenteLogadoModelo)
				{
					TituloDaPagina = Recurso.AvaliacaoCurriculoMinimoLista_TituloPagina,
					TituloLista = Recurso.AvaliacaoCurriculoMinimoLista_TituloLancamentoResposta,
					Ano = dadosPrerequisito.Ano,
					Periodo = dadosPrerequisito.Periodo,
					CodigoCurso = dadosPrerequisito.CodigoCurso,
					CodigoTurma = dadosPrerequisito.CodigoTurma,
					CodigoDisciplina = dadosPrerequisito.CodigoDisciplina,
					CodigoUnidadeEnsino = dadosPrerequisito.CodigoUnidadeEnsino,
					CodigoModalidade = dadosPrerequisito.CodigoModalidade,
					TipoCurso = dadosPrerequisito.TipoCurso,
					Serie = dadosPrerequisito.Serie,
					MensagemSumario = Recurso.AvaliacaoCurriculoMinimoLista_MensagemSumario
				};
				modelo.CabecalhoModelo.TituloCabecalho = Recurso.AvaliacaoCurriculoMinimoLista_TituloPagina;
				modelo.CabecalhoModelo.LinkAjuda = Url.Action("CurriculoMinimo", "Ajuda");

				modelo.TurmaSelecionadaModelo = selecaoTurmaController.ObtemTurmaSelecionadaViewModelPor((ISelecaoTurmasTurmaSelecionadaRequestModel)dadosPrerequisito, docenteLogadoModelo);

				modelo = CarregaDadosBimestreSelecionadoModelo(modelo, dadosPrerequisito);
				modelo = CarregaDadosBimestresValidosParaCurriculoMinimoModelo(modelo, dadosPrerequisito);

				//PreRequisitoa atendidos, manter mesma tela...
				retorno = GeraViewPrerequisitoValido("Lista", modelo);
			}

			//Prerequisito nao atendido, voltar para tela anterior...
			catch (RespostaCurriculoMinimoException curriculoMinimoException)
			{
				ModelState.AddModelError(curriculoMinimoException.TipoDeErro, curriculoMinimoException.Message);
			}
			catch (SubPeriodoLetivoException subperiodoException)
			{
				ModelState.AddModelError(subperiodoException.TipoDeErro, subperiodoException.Message);
			}
			catch (SelecaoTurmasException selecaoTurmasException)
			{
				ModelState.AddModelError(selecaoTurmasException.TipoDeErro, selecaoTurmasException.Message);
			}
			catch (System.Exception excecao)
			{
				retorno = GeraViewPrerequisitoInvalido("ErroInesperado", excecao);
			}

			if (retorno == null)
			{
				var retornoTurma = selecaoTurmaController.Lista(docenteLogadoModelo, "RespostaCurriculoMinimo", this, dadosPrerequisito.CodigoTurma);
				retorno = GeraViewPrerequisitoInvalido("SelecaoTurmas", retornoTurma.ViewData.Model);
			}

			return retorno;
		}

		private PrerequisitoResult PreRequisitoSalva(AvaliacaoCurriculoMinimoListaViewModel modeloLista, DocenteLogadoBindModel docenteLogadoModelo, AvaliacaoCurriculoMinimoSalvaRequestModel modeloSolicitacao)
		{			
			PrerequisitoResult retorno = null;
			try
			{
				avaliacaoCurriculoMinimoServico.VerificaPermissaoParaSalvar(modeloSolicitacao.Ano, modeloSolicitacao.Periodo,
				                                                            modeloSolicitacao.Subperiodo.Value,
				                                                            docenteLogadoModelo.Matricula,
				                                                            modeloSolicitacao.ListaAvaliacao.ConvertAll(
				                                                            	resposta =>
				                                                            	new DTOAvaliacaoCurriculoMinimo_RespostaAvaliacao
				                                                            		{
				                                                            			IdAvaliacaoCurriculoMinimo = resposta.Codigo,
				                                                            			EhAvaliadoPositivamente =
																							//Converte Enum? para Bool?
				                                                            				(resposta.Resposta == null) ? null : (bool?)(resposta.Resposta.Value == AvaliacaoCurriculoMinimoSalvaRequestModel.RespostaAvaliacao.RespostaEnum.Sim)
				                                                            		}));
				retorno = GeraViewPrerequisitoValido("Lista", modeloLista);
			}
			catch(AvaliacaoCurriculoMinimoException excecaoAvaliacao)
			{
				ModelState.AddModelError(excecaoAvaliacao.TipoDeErro, excecaoAvaliacao.Message);
			}
			catch (System.Exception excecao)
			{
				retorno = GeraViewPrerequisitoInvalido("ErroInesperado", excecao);
			}

			if (retorno == null)
			{
				retorno = GeraViewPrerequisitoInvalido("Lista", modeloLista);
			}

			return retorno;
		}
	}
}
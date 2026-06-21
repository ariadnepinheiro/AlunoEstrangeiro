using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Proderj.DOL.Service;
using Proderj.DOL.WebApp.Models;
using Resources;
using Proderj.DOL.Exception;

namespace Proderj.DOL.WebApp.Controllers
{
    public class RespostaCurriculoMinimoController : ControllerPadrao
    {
    	private readonly IRespostaCurriculoMinimoService respostaCurriculoMinimoServico;
    	private readonly ISubPeriodoLetivoService subPeriodoLetivoServico;

		public RespostaCurriculoMinimoController(IRespostaCurriculoMinimoService curriculoMinimoServico, ISubPeriodoLetivoService subPeriodoLetivoServico)
		{
			this.respostaCurriculoMinimoServico = curriculoMinimoServico;
			this.subPeriodoLetivoServico = subPeriodoLetivoServico;
		}

		// GET: /RespostaCurriculoMinimo
		[LogadoComTermoAceito]
		public ActionResult Inicial()
		{
			return RedirectToAction("Lista","SelecaoTurmas", new { nomeController = "RespostaCurriculoMinimo" });
		}

		// GET: /RespostaCurriculoMinimo/SelecaoTurmas
		//[LogadoComTermoAceito]
		//public ActionResult SelecaoTurmas(DocenteLogadoViewModel docenteLogadoModelo)
        //{
		//
		//	
        //}

		// GET: /RespostaCurriculoMinimo/LancaRespostas
		[LogadoComTermoAceito]
		public ActionResult Lista()
		{
			return RedirectToAction("Lista","SelecaoTurmas", new { nomeController = "RespostasCurriculoMinimo"});
		}

		// POST: /RespostaCurriculoMinimo/LancaRespostas
		[HttpPost]
		[LogadoComTermoAceito]
		public ActionResult Lista(DocenteLogadoBindModel docenteLogadoModelo, RespostaCurriculoMinimoListaRequestModel solicitacaoModelo)
		{
			//string codigoTurma = solicitacaoModelo.CodigoTurma;
			
			//Verifica se todos os dados do modelo estão preenchidos...

			//Valida os pre-requisitos.
			PrerequisitoResult preRequisitoLancaRespostas = PreRequisitoLista(docenteLogadoModelo, solicitacaoModelo);
			if (!preRequisitoLancaRespostas.EhValido)
				return preRequisitoLancaRespostas;

			ViewResult viewSaida = preRequisitoLancaRespostas;
			var modeloLancaRespostas = (RespostaCurriculoMinimoListaViewModel) preRequisitoLancaRespostas.Modelo;

			if (ModelState.IsValid)
			{
				//Qualquer erro a partir daqui, mostra na própria tela.
				try
				{
					var dtoSolicitacaoListaRespostas = new DTORespostaCurriculoMinimo_EnumeraRespostasPorGrupoPor
					{
						Ano = solicitacaoModelo.Ano,
						CodigoTurma = solicitacaoModelo.CodigoTurma,
						Curso = solicitacaoModelo.CodigoCurso,
						Disciplina = solicitacaoModelo.CodigoDisciplina,
						Matricula = docenteLogadoModelo.Matricula,
						Modalidade = solicitacaoModelo.CodigoModalidade,
						Periodo = solicitacaoModelo.Periodo,
						Serie = solicitacaoModelo.Serie,
						TipoCurso = solicitacaoModelo.TipoCurso,
						SubPeriodo = modeloLancaRespostas.BimestreSelecionado
					};

					modeloLancaRespostas.ListaRespostasPorGrupo =
						respostaCurriculoMinimoServico.EnumeraRespostasPorGrupoPor(dtoSolicitacaoListaRespostas).ToList();

					modeloLancaRespostas.HabilitarAvaliacao = subPeriodoLetivoServico.EhAtivoParaLancamentoDeAvaliacaoDeCurriculoMinimoPor(modeloLancaRespostas.Ano, modeloLancaRespostas.Periodo, modeloLancaRespostas.BimestreSelecionado);
				}
				catch (RespostaCurriculoMinimoException curriculoMinimoException)
				{
					ModelState.AddModelError(curriculoMinimoException.TipoDeErro, curriculoMinimoException.Message);
				}
				catch (SubPeriodoLetivoException subperiodoException)
				{
					ModelState.AddModelError(subperiodoException.TipoDeErro, subperiodoException.Message);
				}
				catch (System.Exception excecao)
				{
					viewSaida = View("ErroInesperado", excecao);
				}
			}

			return viewSaida;
		}

		// GET: /RespostaCurriculoMinimo/SalvaRespostas
		[HttpGet]
		[LogadoComTermoAceito]
		public ActionResult Salva()
		{
			return RedirectToAction("Lista", "SelecaoTurmas", new { nomeController = "RespostasCurriculoMinimo" });
		}

		// POST: /RespostaCurriculoMinimo/SalvaRespostas
    	[HttpPost]
		[LogadoComTermoAceito]
		public ActionResult Salva(DocenteLogadoBindModel modeloDocenteLogado, RespostaCurriculoMinimoSalvaRequestModel modeloSolicitacao)
		{
			//Valida os pre-requisitos.
			RespostaCurriculoMinimoListaRequestModel solicitacaoLista = new RespostaCurriculoMinimoListaRequestModel
			                                                                     	{
			                                                                     		Ano = modeloSolicitacao.Ano,
																						CodigoCurso = modeloSolicitacao.CodigoCurso,
																						CodigoDisciplina = modeloSolicitacao.CodigoDisciplina,
																						CodigoModalidade = modeloSolicitacao.CodigoModalidade,
																						CodigoTurma = modeloSolicitacao.CodigoTurma,
																						CodigoUnidadeEnsino = modeloSolicitacao.CodigoUnidadeEnsino,
																						NumeroFuncionarioDocente = modeloDocenteLogado.NumeroFuncionario,
																						Periodo = modeloSolicitacao.Periodo,
																						Serie = modeloSolicitacao.Serie,
																						Subperiodo = modeloSolicitacao.Subperiodo,
																						TipoCurso = modeloSolicitacao.TipoCurso
			                                                                     	};
			//Verifica se o professor tem acesso a essa turma/disciplina e se demais informações dadas são pertinentes.
			PrerequisitoResult preRequisitoLista = PreRequisitoLista(modeloDocenteLogado, solicitacaoLista);
			if (!preRequisitoLista.EhValido)
				return preRequisitoLista;

			var modeloLista = (RespostaCurriculoMinimoListaViewModel) preRequisitoLista.Modelo;

			PrerequisitoResult preRequisitoSalva = PreRequisitoSalva(modeloLista, modeloDocenteLogado, modeloSolicitacao);
			if (!preRequisitoSalva.EhValido)
				return preRequisitoSalva;

			ViewResult viewSaida = preRequisitoSalva;
			

			//Verifica se todos os dados do modelo estão preenchidos...
			if (ModelState.IsValid)
			{
				
				try
				{
					respostaCurriculoMinimoServico.SalvaPor(new DTORespostaCurriculoMinimo_SalvaPor
					                                                        	{
					                                                        		Ano = modeloSolicitacao.Ano,
					                                                        		CodigoTurma = modeloSolicitacao.CodigoTurma,
					                                                        		CodigoDisciplina = modeloSolicitacao.CodigoDisciplina,
					                                                        		Matricula = modeloDocenteLogado.Matricula,
					                                                        		Periodo = modeloSolicitacao.Periodo,
					                                                        		Subperiodo = modeloSolicitacao.Subperiodo,
					                                                        		IdsCompetenciaHabilidadeItem =
					                                                        			modeloSolicitacao.ListaResposta
																							.Where(resposta => resposta.Respondido == true)
																							.Select(resposta => resposta.Codigo)
																							.ToList()
					                                                        	});

					modeloLista.ListaRespostasPorGrupo =
						respostaCurriculoMinimoServico.EnumeraRespostasPorGrupoPor(new DTORespostaCurriculoMinimo_EnumeraRespostasPorGrupoPor
						{
							Ano = modeloSolicitacao.Ano,
							CodigoTurma = modeloSolicitacao.CodigoTurma,
							Curso = modeloSolicitacao.CodigoCurso,
							Disciplina = modeloSolicitacao.CodigoDisciplina,
							Matricula = modeloDocenteLogado.Matricula,
							Modalidade = modeloSolicitacao.CodigoModalidade,
							Periodo = modeloSolicitacao.Periodo,
							Serie = modeloSolicitacao.Serie,
							TipoCurso = modeloSolicitacao.TipoCurso,
							SubPeriodo = modeloLista.BimestreSelecionado
						}).ToList();

					modeloLista.HabilitarAvaliacao = subPeriodoLetivoServico.EhAtivoParaLancamentoDeAvaliacaoDeCurriculoMinimoPor(modeloLista.Ano, modeloLista.Periodo, modeloLista.BimestreSelecionado);

					modeloLista.LancamentoPersistido = true;
					modeloLista.MensagemLancamentoPersistido =
						Recurso.RespostaCurriculoMinimoSalva_MensagemLancamentoPersistido;
				}
				catch(RespostaCurriculoMinimoException curriculoException)
				{
					ModelState.AddModelError(curriculoException.TipoDeErro, curriculoException.Message);
				}
				catch(SubPeriodoLetivoException subperiodoException)
				{
					ModelState.AddModelError(subperiodoException.TipoDeErro, subperiodoException.Message);
				}
			}
			return viewSaida;
		}

		/// <param name="lancaRespostasModelo">Aproveita o modelo pre-gerado pela validação de lançamento respostas</param>
    	private PrerequisitoResult PreRequisitoSalva(RespostaCurriculoMinimoListaViewModel lancaRespostasModelo, DocenteLogadoBindModel docenteLogadoModelo, RespostaCurriculoMinimoSalvaRequestModel solicitacaoModelo)
    	{
			PrerequisitoResult retorno = null;
			try
			{
				respostaCurriculoMinimoServico.VerificaPermissaoParaSalvar(new DTORespostaCurriculoMinimo_VerificaPermissaoParaSalvar
				                                                                  	{
				                                                                  		Ano = solicitacaoModelo.Ano,
				                                                                  		CodigoTurma = solicitacaoModelo.CodigoTurma,
				                                                                  		CodigoCurso = solicitacaoModelo.CodigoCurso,
				                                                                  		CodigoDisciplina =
				                                                                  			solicitacaoModelo.CodigoDisciplina,
				                                                                  		MatriculaDocente = docenteLogadoModelo.Matricula,
				                                                                  		CodigoModalidade =
				                                                                  			solicitacaoModelo.CodigoModalidade,
				                                                                  		Periodo = solicitacaoModelo.Periodo,
				                                                                  		SubPeriodo = solicitacaoModelo.Subperiodo,
				                                                                  		Serie = solicitacaoModelo.Serie,
				                                                                  		TipoCurso = solicitacaoModelo.TipoCurso,
				                                                                  		ListaResposta = solicitacaoModelo
				                                                                  			.ListaResposta.ConvertAll(
				                                                                  				p =>
				                                                                  				new DTORespostaCurriculoMinimo_RespostasPorGrupo
				                                                                  					{
				                                                                  						CodigoGrupo = p.CodigoGrupo,
				                                                                  						CodigoResposta = p.Codigo
				                                                                  					})
				                                                                  	});

				retorno = GeraViewPrerequisitoValido("Lista", lancaRespostasModelo);
			}

			//Prerequisito nao atendido, mostrar erros no lançamento
			catch (RespostaCurriculoMinimoException curriculoMinimoException)
			{
				ModelState.AddModelError(curriculoMinimoException.TipoDeErro, curriculoMinimoException.Message);
			}
			catch (SubPeriodoLetivoException subperiodoException)
			{
				ModelState.AddModelError(subperiodoException.TipoDeErro, subperiodoException.Message);
			}
			catch (System.Exception excecao)
			{
				retorno = GeraViewPrerequisitoInvalido("ErroInesperado", excecao);
			}

			if (retorno == null)
			{
				retorno = GeraViewPrerequisitoInvalido("Lista", lancaRespostasModelo);
			}

			return retorno;

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
				respostaCurriculoMinimoServico.VerificaPermissaoParaListar(new DTORespostaCurriculoMinimo_VerificaPermissaoParaListar()
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


				var modelo = new RespostaCurriculoMinimoListaViewModel(docenteLogadoModelo)
				{
					TituloDaPagina = Recurso.RespostaCurriculoMinimoLista_TituloPagina,
					TituloLancamentoResposta = Recurso.RespostaCurriculoMinimoLista_TituloLancamentoResposta,
					MensagemSumario = Recurso.RespostaCurriculoMinimoLista_MensagemSumario,
					Ano = dadosPrerequisito.Ano,
					Periodo = dadosPrerequisito.Periodo,
					CodigoCurso = dadosPrerequisito.CodigoCurso,
					CodigoTurma = dadosPrerequisito.CodigoTurma,
					CodigoDisciplina = dadosPrerequisito.CodigoDisciplina,
					CodigoUnidadeEnsino = dadosPrerequisito.CodigoUnidadeEnsino,
					CodigoModalidade = dadosPrerequisito.CodigoModalidade,
					TipoCurso = dadosPrerequisito.TipoCurso,
					Serie = dadosPrerequisito.Serie
				};
				modelo.CabecalhoModelo.TituloCabecalho = Recurso.RespostaCurriculoMinimoLista_TituloPagina;
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
				retorno = GeraViewPrerequisitoInvalido("../SelecaoTurmas/Lista", retornoTurma.ViewData.Model);
			}

			return retorno;
		}

		private RespostaCurriculoMinimoListaViewModel CarregaDadosBimestreSelecionadoModelo(RespostaCurriculoMinimoListaViewModel modeloOrigem, ICurriculoMinimoListaPrerequisito solicitacaoModelo)
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

		private RespostaCurriculoMinimoListaViewModel CarregaDadosBimestresValidosParaCurriculoMinimoModelo(RespostaCurriculoMinimoListaViewModel modeloOrigem, ICurriculoMinimoListaPrerequisito solicitacaoModelo)
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

    }
}

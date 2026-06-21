using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Repository;
using Proderj.DOL.Exception;
using Proderj.DOL.Domain;
using Proderj.Foundation.Common;
using Proderj.Foundation.Framework;
using AutoMapper;

namespace Proderj.DOL.Service
{
	public class AvaliacaoCurriculoMinimoService : IAvaliacaoCurriculoMinimoService
	{
		private readonly IAvaliacaoCurriculoMinimoRepository repositorioAvaliacaoCurriculoMinimo;
		private readonly IAvaliacaoCurriculoMinimoJustificativaRepository repositorioAvaliacaoCurriculoMinimoJustificativa;
		private readonly ILogAvaliacaoCurriculoMinimoJustificativaRepository repositorioLogAvaliacaoCurriculoMinimoJustificativa;
		private readonly IAvaliacaoCurriculoMinimoDocenteRepository repositorioAvaliacaoCurriculoMinimoDocente;
		private readonly ILogAvaliacaoCurriculoMinimoDocenteRepository repositorioLogAvaliacaoCurriculoMinimoDocente;

		public AvaliacaoCurriculoMinimoService(IAvaliacaoCurriculoMinimoRepository repositorioAvaliacaoCurriculoMinimo,
			 IAvaliacaoCurriculoMinimoJustificativaRepository repositorioAvaliacaoCurriculoMinimoJustificativa,
			 ILogAvaliacaoCurriculoMinimoJustificativaRepository repositorioLogAvaliacaoCurriculoMinimoJustificativa,
			 IAvaliacaoCurriculoMinimoDocenteRepository repositorioAvaliacaoCurriculoMinimoDocente,
			 ILogAvaliacaoCurriculoMinimoDocenteRepository repositorioLogAvaliacaoCurriculoMinimoDocente)
		{
			this.repositorioAvaliacaoCurriculoMinimo = repositorioAvaliacaoCurriculoMinimo;
			this.repositorioAvaliacaoCurriculoMinimoJustificativa = repositorioAvaliacaoCurriculoMinimoJustificativa;
			this.repositorioLogAvaliacaoCurriculoMinimoJustificativa = repositorioLogAvaliacaoCurriculoMinimoJustificativa;
			this.repositorioAvaliacaoCurriculoMinimoDocente = repositorioAvaliacaoCurriculoMinimoDocente;
			this.repositorioLogAvaliacaoCurriculoMinimoDocente = repositorioLogAvaliacaoCurriculoMinimoDocente;
		}

		public DTOAvaliacaoCurriculoMinimo_AvaliacoesEJustificativa ObtemAvaliacoesEJustificativaPor(short ano, short periodo, short subPeriodo, string matricula)
		{
			IEnumerable<TOAvaliacaoCurriculoMinimoListagem> listagemAvaliacaoCurriculoMinimo = repositorioAvaliacaoCurriculoMinimo.EnumeraPor(matricula, ano, periodo, subPeriodo);

			if (!(listagemAvaliacaoCurriculoMinimo.Count() > 0))
			{
				throw new AvaliacaoCurriculoMinimoException(AvaliacaoCurriculoMinimoException.TipoEnum.NaoExisteAvaliacaoParaBimestre);
			}

			AvaliacaoCurriculoMinimoJustificativa avaliacaoJustificativa = repositorioAvaliacaoCurriculoMinimoJustificativa.ObtemPor(ano, periodo, subPeriodo, matricula);

			var dtoAvaliacaoCurriculoMinimo_Avaliacoes = new DTOAvaliacaoCurriculoMinimo_AvaliacoesEJustificativa
			{
                DescricaoJustificativa = (avaliacaoJustificativa != null) ? avaliacaoJustificativa.Justificativa : string.Empty
			};

			listagemAvaliacaoCurriculoMinimo.ExecuteForEach
			(avl => {
				var dtoAvaliacaoCurriculoMinimo = new DTOAvaliacaoCurriculoMinimo_RespostaAvaliacao
				{
					DescricaoAvaliacao = avl.DescricaoAvaliacao,
					EhAvaliadoPositivamente = avl.Resposta,
					Ordem = avl.Ordem,
					IdAvaliacaoCurriculoMinimo = avl.IdAvaliacaoCurriculoMinimo
				};

				dtoAvaliacaoCurriculoMinimo_Avaliacoes.ListaAvaliacaoCurriculoMinimo.Add(dtoAvaliacaoCurriculoMinimo);
			});

			return dtoAvaliacaoCurriculoMinimo_Avaliacoes;
		}

		public void SalvaAvaliacoesEJustificativaPor(DTOAvaliacaoCurriculoMinimo_SalvaAvaliacoesEJustificativaPor dtoSolicitacaoSalvamento)
		{
            if (!(dtoSolicitacaoSalvamento.AvaliacoesCurriculoMinimo.Any(av => av.EhAvaliadoPositivamente != null)) && 
                 (dtoSolicitacaoSalvamento.DescricaoJustificativa.IsNullOrEmpty()))
			{
				throw new AvaliacaoCurriculoMinimoException(AvaliacaoCurriculoMinimoException.TipoEnum.NecessarioPreenchimentodeCampos);
			}

			//atualiza curriculo mínimo docente
            if (dtoSolicitacaoSalvamento.AvaliacoesCurriculoMinimo.Any(av => av.EhAvaliadoPositivamente != null))
            {
                //Mapper.CreateMap<DTOAvaliacaoCurriculoMinimo_SalvaAvaliacoesEJustificativaPor, DTOAvaliacaoCurriculoMinimoDocente_AtualizaCompetenciasPor>();
                DTOAvaliacaoCurriculoMinimoDocente_AtualizaCompetenciasPor dtoAtualizaAvaliacaoCurriculoMinimoDocente = Mapper.Map<DTOAvaliacaoCurriculoMinimo_SalvaAvaliacoesEJustificativaPor, DTOAvaliacaoCurriculoMinimoDocente_AtualizaCompetenciasPor>(dtoSolicitacaoSalvamento);

                AtualizaCompetenciasDocentePor(dtoAtualizaAvaliacaoCurriculoMinimoDocente);
            }			

			//atualiza curriculo mínimo justificativa
            if (!dtoSolicitacaoSalvamento.DescricaoJustificativa.IsNullOrEmpty())
            {
                //Mapper.CreateMap<DTOAvaliacaoCurriculoMinimo_SalvaAvaliacoesEJustificativaPor, DTOAvaliacaoCurriculoMinimoJustificativa_AtualizaCompetenciasPor>();
                DTOAvaliacaoCurriculoMinimoJustificativa_AtualizaCompetenciasPor dtoAtualizaAvaliacaoCurriculoMinimoJustificativa = Mapper.Map<DTOAvaliacaoCurriculoMinimo_SalvaAvaliacoesEJustificativaPor, DTOAvaliacaoCurriculoMinimoJustificativa_AtualizaCompetenciasPor>(dtoSolicitacaoSalvamento);

                AtualizaCompetenciasJustificativaPor(dtoAtualizaAvaliacaoCurriculoMinimoJustificativa);
            }			
		}

		public void AtualizaCompetenciasJustificativaPor(DTOAvaliacaoCurriculoMinimoJustificativa_AtualizaCompetenciasPor dtoAtualizaJustificativa)
		{
			short ano = dtoAtualizaJustificativa.Ano;
			short periodo = dtoAtualizaJustificativa.Periodo;
			short subperiodo = dtoAtualizaJustificativa.SubPeriodo;
			string matricula = dtoAtualizaJustificativa.Matricula;

			//Insere Log
			repositorioLogAvaliacaoCurriculoMinimoJustificativa.InserePor(ano, periodo, subperiodo, matricula);

			//Remove as avaliações anteriores
			repositorioAvaliacaoCurriculoMinimoJustificativa.RemoveCompetenciasAntigas(matricula, ano, periodo, subperiodo);

			//Insere ou reinsere as novas avaliações
			var avaliacaoCurriculoMinimoJustificativa = new AvaliacaoCurriculoMinimoJustificativa
			{
				Ano = ano,
				Periodo = periodo,
				SubPeriodo = subperiodo,
				Matricula = matricula,
				Justificativa = dtoAtualizaJustificativa.DescricaoJustificativa ?? string.Empty
			};
			repositorioAvaliacaoCurriculoMinimoJustificativa.InsereCom(avaliacaoCurriculoMinimoJustificativa);
		}

		public void AtualizaCompetenciasDocentePor(DTOAvaliacaoCurriculoMinimoDocente_AtualizaCompetenciasPor dtoAtualizacao)
		{
			short ano = dtoAtualizacao.Ano;
			short periodo = dtoAtualizacao.Periodo;
			short subperiodo = dtoAtualizacao.SubPeriodo;
			string matricula = dtoAtualizacao.Matricula;

			//Insere Log
			repositorioLogAvaliacaoCurriculoMinimoDocente.InserePor(ano, periodo, subperiodo, matricula);

			//Remove as avaliações anteriores
			repositorioAvaliacaoCurriculoMinimoDocente.RemoveCompetenciasAntigas(matricula, ano, periodo, subperiodo);

			//Insere ou reinsere as novas avaliações
			var listaAvaliacaoCurriculoMinimoDocente = new List<AvaliacaoCurriculoMinimoDocente>();

			dtoAtualizacao.AvaliacoesCurriculoMinimo.ExecuteForEach
			(avaliacao =>
			{
                if (avaliacao.EhAvaliadoPositivamente != null)
                {
                    var dtoAvaliacaoCurriculoMinimo = new AvaliacaoCurriculoMinimoDocente
                    {
                        AvaliacaoCurriculoMinimo = new AvaliacaoCurriculoMinimo { Id = avaliacao.IdAvaliacaoCurriculoMinimo },
                        Resposta = avaliacao.EhAvaliadoPositivamente.Value,
                        Matricula = dtoAtualizacao.Matricula
                    };

                    listaAvaliacaoCurriculoMinimoDocente.Add(dtoAvaliacaoCurriculoMinimo);
                }
			}
			);

			repositorioAvaliacaoCurriculoMinimoDocente.InserePor(listaAvaliacaoCurriculoMinimoDocente, true);
		}

		public void VerificaPermissaoParaListar(short ano, short periodo, short subPeriodo, string matricula, List<DTOAvaliacaoCurriculoMinimo_RespostaAvaliacao> listaRespostaAvaliacao)
		{
			throw new NotImplementedException();
		}

		public void VerificaPermissaoParaListar(DTOAvaliacaoCurriculoMinimo_VerificaPermissaoParaListar dtoSolicitacao)
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
				throw new AvaliacaoCurriculoMinimoException(AvaliacaoCurriculoMinimoException.TipoEnum.AcessoNegadoDocente_A_TurmaEDisciplina);
			}

			//Se tiver acesso, verificar se ainda pode lançar avaliação de Curriculo minimo...
			if (!turmaComAcesso.ValidoParaLancamento)
			{
				var excecao = new AvaliacaoCurriculoMinimoException(AvaliacaoCurriculoMinimoException.TipoEnum.TurmaEDisciplinaInvalidaParaLancamento)
				{
					CodigoTurma = dtoSolicitacao.CodigoTurma
				};

				throw excecao;
			}
		}

		public void VerificaPermissaoParaSalvar(short ano, short periodo, short subPeriodo, string matricula, List<DTOAvaliacaoCurriculoMinimo_RespostaAvaliacao> listaRespostaAvaliacao)
		{
			DTOAvaliacaoCurriculoMinimo_AvaliacoesEJustificativa dadosAvaliacaoEJustificativa = ObtemAvaliacoesEJustificativaPor(ano, periodo, subPeriodo, matricula);
			foreach (var respostaAvaliacao in listaRespostaAvaliacao)
			{
				if (!dadosAvaliacaoEJustificativa.ListaAvaliacaoCurriculoMinimo.Any(avaliacao => avaliacao.IdAvaliacaoCurriculoMinimo == respostaAvaliacao.IdAvaliacaoCurriculoMinimo))
				{
					throw new AvaliacaoCurriculoMinimoException(AvaliacaoCurriculoMinimoException.TipoEnum.RespostasInformadasInvalidas);
				}
			}
		}
	}
}

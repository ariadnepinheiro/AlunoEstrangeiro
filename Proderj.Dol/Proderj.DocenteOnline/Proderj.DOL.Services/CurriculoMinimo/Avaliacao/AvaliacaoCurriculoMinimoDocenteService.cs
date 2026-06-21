using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Repository;
using Proderj.DOL.Domain;
using Proderj.Foundation.Common;

namespace Proderj.DOL.Service
{
	public class AvaliacaoCurriculoMinimoDocenteService : IAvaliacaoCurriculoMinimoDocenteService
	{
		private IAvaliacaoCurriculoMinimoDocenteRepository repositorioAvaliacaoCurriculoMinimoDocente;
		private ILogAvaliacaoCurriculoMinimoDocenteRepository repositorioLogAvaliacaoCurriculoMinimoDocente;

		public AvaliacaoCurriculoMinimoDocenteService(IAvaliacaoCurriculoMinimoDocenteRepository repositorioAvaliacaoCurriculoMinimoDocente,
			ILogAvaliacaoCurriculoMinimoDocenteRepository repositorioLogAvaliacaoCurriculoMinimoDocente)
		{
			this.repositorioAvaliacaoCurriculoMinimoDocente = repositorioAvaliacaoCurriculoMinimoDocente;
			this.repositorioLogAvaliacaoCurriculoMinimoDocente = repositorioLogAvaliacaoCurriculoMinimoDocente;
		}

		public void AtualizaCompetenciasPor(DTOAvaliacaoCurriculoMinimoDocente_AtualizaCompetenciasPor dtoAtualizacao)
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
			(avl =>
				{
					var dtoAvaliacaoCurriculoMinimo = new AvaliacaoCurriculoMinimoDocente
					{
						AvaliacaoCurriculoMinimo = new AvaliacaoCurriculoMinimo { Id = avl.IdAvaliacaoCurriculoMinimo },
						Resposta = avl.EhAvaliadoPositivamente.Value,
						Matricula = dtoAtualizacao.Matricula
					};

					listaAvaliacaoCurriculoMinimoDocente.Add(dtoAvaliacaoCurriculoMinimo);
				}
			);

			repositorioAvaliacaoCurriculoMinimoDocente.InserePor(listaAvaliacaoCurriculoMinimoDocente, true);
		}
	}
}

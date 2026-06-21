using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Repository;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Service
{
	public class AvaliacaoCurriculoMinimoJustificativaService : IAvaliacaoCurriculoMinimoJustificativaService
	{
		private IAvaliacaoCurriculoMinimoJustificativaRepository repositorioAvaliacaoCurriculoMinimoJustificativa;
		private ILogAvaliacaoCurriculoMinimoJustificativaRepository repositorioLogAvaliacaoCurriculoMinimoJustificativa;

		public AvaliacaoCurriculoMinimoJustificativaService(IAvaliacaoCurriculoMinimoJustificativaRepository repositorioAvaliacaoCurriculoMinimoJustificativa,
			ILogAvaliacaoCurriculoMinimoJustificativaRepository repositorioLogAvaliacaoCurriculoMinimoJustificativa)
		{
			this.repositorioAvaliacaoCurriculoMinimoJustificativa = repositorioAvaliacaoCurriculoMinimoJustificativa;
			this.repositorioLogAvaliacaoCurriculoMinimoJustificativa = repositorioLogAvaliacaoCurriculoMinimoJustificativa;
		}

		public void AtualizaCompetenciasPor(DTOAvaliacaoCurriculoMinimoJustificativa_AtualizaCompetenciasPor dtoAtualizaJustificativa)
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
				Justificativa = dtoAtualizaJustificativa.Justificativa ?? string.Empty
			};
			repositorioAvaliacaoCurriculoMinimoJustificativa.InsereCom(avaliacaoCurriculoMinimoJustificativa);
		}
	}
}

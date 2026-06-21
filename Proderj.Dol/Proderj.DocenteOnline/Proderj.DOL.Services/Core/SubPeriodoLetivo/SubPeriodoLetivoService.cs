using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Repository;
using AutoMapper;
using Proderj.DOL.Domain;
using Proderj.DOL.Exception;

namespace Proderj.DOL.Service
{
	public class SubPeriodoLetivoService : ISubPeriodoLetivoService
	{
		private readonly ISubPeriodoLetivoRepository repositorioSubPeriodo;
		
		//Cache de Bimestres por Ano/Periodo
		short? ultimoAnoSolicitado;
		short? ultimoPeriodoSolicitado;
		List<DTOSubPeriodoLetivoAtivo> ultimosSubPeriodosLetivosSolicitados = new List<DTOSubPeriodoLetivoAtivo>();

		public SubPeriodoLetivoService(ISubPeriodoLetivoRepository repositorioSubPeriodo)
		{
			this.repositorioSubPeriodo = repositorioSubPeriodo;
		}

		public IEnumerable<DTOSubPeriodoLetivoAtivo> EnumeraPor(short ano, short periodo)
		{
			//Verifica se esse resultado ja foi fornecido e reaproveita
			if (ultimoAnoSolicitado == ano && ultimoPeriodoSolicitado == periodo)
				foreach (DTOSubPeriodoLetivoAtivo dtoSubPeriodo in ultimosSubPeriodosLetivosSolicitados)
				{
					yield return dtoSubPeriodo;
				}
			else
			{
				int itens = 0;

				//Redefine o cache
				ultimoAnoSolicitado = ano;
				ultimoPeriodoSolicitado = periodo;
				ultimosSubPeriodosLetivosSolicitados.Clear();

				IEnumerable<SubPeriodoLetivo> subPeriodosAtivos = repositorioSubPeriodo.EnumeraPor(ano, periodo);

				//Mapper.CreateMap<SubPeriodoLetivo, DTOSubPeriodoLetivoAtivo>();
				foreach (SubPeriodoLetivo subperiodo in subPeriodosAtivos)
				{
					itens++;
					var dtoSubPeriodo = Mapper.Map(subperiodo, new DTOSubPeriodoLetivoAtivo());

					ultimosSubPeriodosLetivosSolicitados.Add(dtoSubPeriodo);
					yield return dtoSubPeriodo;
				}

				if (itens == 0)
					throw new SubPeriodoLetivoException(ano, periodo, SubPeriodoLetivoException.TipoEnum.NaoExisteSubPeriodoAtivo);
			}
		}

		public IEnumerable<DTOSubPeriodoLetivoAtivo> EnumeraAtivosParaLancamentoDeCurriculoMinimoPor(short ano, short periodo)
		{
			int itens = 0;
			IEnumerable<SubPeriodoLetivo> subPeriodosAtivos =
				repositorioSubPeriodo.EnumeraAtivosParaLancamentoDeCurriculoMinimoPor(ano, periodo);

			foreach (SubPeriodoLetivo subperiodo in subPeriodosAtivos)
			{
				itens++;
				var dtoSubPeriodo = Mapper.Map(subperiodo, new DTOSubPeriodoLetivoAtivo());

				yield return dtoSubPeriodo;
			}

			if (itens == 0)
				throw new SubPeriodoLetivoException(ano, periodo, SubPeriodoLetivoException.TipoEnum.NaoExisteSubPeriodoAtivoParaLancamentoDeCurriculoMinimo);
		}

		public short ObtemAtualParaLancamentoDeNotasPor(short ano, short periodo)
		{
			short? subPeriodoAtual = repositorioSubPeriodo.ObtemAtualParaLancamentoDeNotasPor(ano, periodo);
			if (subPeriodoAtual == null)
			{
				//Se não encontrar o subperiodo ativo pela data, pega o primeiro que vier da lista de opções.
				subPeriodoAtual = repositorioSubPeriodo.EnumeraPor(ano, periodo)
									.First()
									.SubPeriodo;
				
			}

			return subPeriodoAtual.Value;
		}

		public bool EhAtivoParaLancamentoDeNotas(short ano, short periodo, short subPeriodo) {
			return repositorioSubPeriodo.EhAtivoParaLancamentoDeNotasPor(ano, periodo, subPeriodo);
		}

		public bool EhAntigoParaLancamentoDeNotas(short ano, short periodo, short subPeriodo)
		{
			return repositorioSubPeriodo.EhAntigoParaLancamentoDeNotasPor(ano, periodo, subPeriodo);
		}

		
		public IEnumerable<DTOSubPeriodoLetivoAtivo> ListaParaExibicaoDeCurriculoMinimoPor(DTOSubPeriodoLetivo_EnumeraParaExibicaoDeCurriculoMinimoPor dtoSolicitacao)
		{

			//Mapper.CreateMap<DTOSubPeriodoLetivo_EnumeraParaExibicaoDeCurriculoMinimoPor, TOSubPeriodoLetivo_EnumeraAtivosParaLancamentoDeRespostaDeCurriculoMinimoPor>();
			TOSubPeriodoLetivo_EnumeraAtivosParaLancamentoDeRespostaDeCurriculoMinimoPor voSolicitacaoRespostas = Mapper.Map<DTOSubPeriodoLetivo_EnumeraParaExibicaoDeCurriculoMinimoPor, TOSubPeriodoLetivo_EnumeraAtivosParaLancamentoDeRespostaDeCurriculoMinimoPor>(dtoSolicitacao);

			
			//Pega os ativos para curriculo minimo resposta
			List<SubPeriodoLetivo> listaAtivosParaResposta = repositorioSubPeriodo.EnumeraAtivosParaLancamentoDeRespostaDeCurriculoMinimoPor(voSolicitacaoRespostas).ToList();

			//Pega os ativos para avaliação do curriculo minimo
			List<SubPeriodoLetivo> listaAtivosParaAvaliacao = repositorioSubPeriodo.EnumeraAtivosParaLancamentoDeAvaliacaoDeCurriculoMinimoPor(dtoSolicitacao.Ano,
			                                                                                 dtoSolicitacao.Periodo)
																							 .ToList();

			//Faz a interceção
			//IEnumerable<SubPeriodoLetivo> listaAtivosIntercecao = listaAtivosParaAvaliacao.Intersect(listaAtivosParaResposta);

			//Faz a união distinta
			//List<SubPeriodoLetivo> listaAtivosParaExibir = listaAtivosParaAvaliacao.Union(listaAtivosParaResposta.Except(listaAtivosIntercecao)).ToList();
			var listaAtivosParaExibir = listaAtivosParaAvaliacao;
			foreach (SubPeriodoLetivo subPeriodoLetivo in listaAtivosParaAvaliacao)
			{
				if (!listaAtivosParaExibir.Any(s => s.SubPeriodo == subPeriodoLetivo.SubPeriodo))
				{
					listaAtivosParaExibir.Add(subPeriodoLetivo);
				}
			}


			//Se nao existir nenhum, exceção de inexistente.
			if (listaAtivosParaExibir.Count == 0)
			{
				throw new SubPeriodoLetivoException(dtoSolicitacao.Ano, dtoSolicitacao.Periodo,
				                                    SubPeriodoLetivoException.TipoEnum.NaoExistemSubperiodosValidosParaExibicao);
			}
			if (dtoSolicitacao.SubPeriodo != 0 && !listaAtivosParaExibir.Any(subperiodo => subperiodo.SubPeriodo == dtoSolicitacao.SubPeriodo)) 
			{
				throw new SubPeriodoLetivoException(dtoSolicitacao.Ano, dtoSolicitacao.Periodo, SubPeriodoLetivoException.TipoEnum.SubperiodoInvalidoParaExibicao);
			}

			foreach (SubPeriodoLetivo subperiodo in listaAtivosParaExibir)
			{
				var dtoSubPeriodo = Mapper.Map(subperiodo, new DTOSubPeriodoLetivoAtivo());

				yield return dtoSubPeriodo;
			}
		}

		public bool EhAtivoParaLancamentoDeAvaliacaoDeCurriculoMinimoPor(short ano, short periodo, short subperiodo)
		{
			return repositorioSubPeriodo.EhAtivoParaLancamentoDeAvaliacaoDeCurriculoMinimoPor(ano, periodo, subperiodo);
		}

		public bool EhAtivoParaLancamentoDeCurriculoMinimo(short ano, short periodo, short subPeriodo)
		{
			return repositorioSubPeriodo.EhAtivoParaLancamentoDeRespostaDeCurriculoMinimoPor(ano, periodo, subPeriodo);

		}
	}
}

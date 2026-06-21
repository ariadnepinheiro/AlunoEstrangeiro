using System;
using System.Collections.Generic;

namespace Proderj.DOL.Service
{
	public interface ISubPeriodoLetivoService
	{
		IEnumerable<DTOSubPeriodoLetivoAtivo> EnumeraPor(short ano, short periodo);

		IEnumerable<DTOSubPeriodoLetivoAtivo> EnumeraAtivosParaLancamentoDeCurriculoMinimoPor(short ano, short periodo);
		
		short ObtemAtualParaLancamentoDeNotasPor(short ano, short periodo);

		bool EhAtivoParaLancamentoDeNotas(short ano, short periodo, short subPeriodo);

		bool EhAtivoParaLancamentoDeCurriculoMinimo(short ano, short periodo, short subPeriodo);

		bool EhAntigoParaLancamentoDeNotas(short ano, short periodo, short subPeriodo);

		IEnumerable<DTOSubPeriodoLetivoAtivo> ListaParaExibicaoDeCurriculoMinimoPor(DTOSubPeriodoLetivo_EnumeraParaExibicaoDeCurriculoMinimoPor dtoSolicitacaoCurriculoMinimo);

		bool EhAtivoParaLancamentoDeAvaliacaoDeCurriculoMinimoPor(short ano, short periodo, short subperiodo);
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Architecture;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository
{
	public interface ISubPeriodoLetivoRepository : IRepository<SubPeriodoLetivo>
	{
		short? ObtemAtualParaLancamentoDeNotasPor(short ano, short periodo);

		short? ObtemAtualParaCurriculoMinimoPor(short ano, short periodo);

		bool EhAtivoParaLancamentoDeNotasPor(short ano, short periodo, short subPeriodo);

		bool EhAntigoParaLancamentoDeNotasPor(short ano, short periodo, short subPeriodo);

		bool EhAtivoParaLancamentoDeRespostaDeCurriculoMinimoPor(short ano, short periodo, short subPeriodo);

		bool EhAtivoParaLancamentoDeAvaliacaoDeCurriculoMinimoPor(short ano, short periodo, short subPeriodo);

		IEnumerable<SubPeriodoLetivo> EnumeraPor(short ano, short periodo);

		IEnumerable<SubPeriodoLetivo> EnumeraAtivosParaLancamentoDeCurriculoMinimoPor(short ano, short periodo);

		IEnumerable<SubPeriodoLetivo> EnumeraAtivosParaLancamentoDeRespostaDeCurriculoMinimoPor(TOSubPeriodoLetivo_EnumeraAtivosParaLancamentoDeRespostaDeCurriculoMinimoPor voSolicitacao);

		IEnumerable<SubPeriodoLetivo> EnumeraAtivosParaLancamentoDeAvaliacaoDeCurriculoMinimoPor(short ano, short periodo);

		IEnumerable<SubPeriodoLetivo> EnumeraParaAvaliacaoPor(short ano, short periodo);

		IEnumerable<SubPeriodoLetivo> EnumeraParaAvaliacaoPor(short ano, short periodo, bool acrescentaPeriodoFinal);

	}
}

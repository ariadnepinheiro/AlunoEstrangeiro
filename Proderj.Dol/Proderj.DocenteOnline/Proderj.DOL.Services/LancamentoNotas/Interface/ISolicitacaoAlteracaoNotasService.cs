using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Service
{
	public interface ISolicitacaoAlteracaoNotasService
	{
		bool ExisteSolicitacaoAlteracaoNotaValidaEAprovada(DTOSolicitacaoAlteracaoNotas_ConsultaTurma dtoConsultaSolicitacao);
		DateTime? ObtemDataDaSolicitacaoAlteracaoNotaAguardandoAprovacao(DTOSolicitacaoAlteracaoNotas_ConsultaTurma dtoConsultaSolicitacao);

		void InsereSolicitacaoReabertura(DTOSolicitacaoReabertura solicitacaoReabertura);
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Architecture;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository
{
	public interface ISolicitacaoAlteracaoNotasRepository : IRepository<SolicitacaoAlteracaoNotas>
	{
		bool ExisteSolicitacaoAlteracaoNotasValido(SolicitacaoAlteracaoNotas solicitacao);
		bool ExisteSolicitacaoAlteracaoNotas(SolicitacaoAlteracaoNotas solicitacao);
		DateTime? ObtemDataPor(SolicitacaoAlteracaoNotas solicitacao);
		void InsereSolicitacaoReabertura(SolicitacaoAlteracaoNotas solicitacao);
	}
}

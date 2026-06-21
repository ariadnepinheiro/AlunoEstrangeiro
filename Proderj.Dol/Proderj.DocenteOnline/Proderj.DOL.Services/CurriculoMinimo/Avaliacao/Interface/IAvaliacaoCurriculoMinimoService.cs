using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Architecture;

namespace Proderj.DOL.Service
{
	public interface IAvaliacaoCurriculoMinimoService : IService
	{
		DTOAvaliacaoCurriculoMinimo_AvaliacoesEJustificativa ObtemAvaliacoesEJustificativaPor(short ano, short periodo, short subPeriodo, string matricula);

		void SalvaAvaliacoesEJustificativaPor(DTOAvaliacaoCurriculoMinimo_SalvaAvaliacoesEJustificativaPor dtoSolicitacao);

		void AtualizaCompetenciasJustificativaPor(DTOAvaliacaoCurriculoMinimoJustificativa_AtualizaCompetenciasPor dtoAtualizaJustificativa);

		void AtualizaCompetenciasDocentePor(DTOAvaliacaoCurriculoMinimoDocente_AtualizaCompetenciasPor dtoAtualizacao);

		void VerificaPermissaoParaListar(DTOAvaliacaoCurriculoMinimo_VerificaPermissaoParaListar dtoSolicitacao);

		void VerificaPermissaoParaSalvar(short ano, short periodo, short subPeriodo, string matricula, List<DTOAvaliacaoCurriculoMinimo_RespostaAvaliacao> listaRespostaAvaliacao);
	}
}




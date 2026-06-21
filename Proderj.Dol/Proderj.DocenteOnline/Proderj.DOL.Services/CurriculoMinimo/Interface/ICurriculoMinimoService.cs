using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Architecture;

namespace Proderj.DOL.Service
{
	public interface IRespostaCurriculoMinimoService : IService
	{
		DTORespostaCurriculoMinimo_StatusPreenchimentoPorTurma ObtemStatusPreenchimentoPor(DTORespostaCurriculoMinimo_ObtemStatusPreenchimentoPor dtoVerificacaoStatus);
		
		void VerificaPermissaoParaListar(DTORespostaCurriculoMinimo_VerificaPermissaoParaListar dtoSolicitacao);
		
		IEnumerable<DTORespostaCurriculoMinimo_RespostasPorGrupo> EnumeraRespostasPorGrupoPor(DTORespostaCurriculoMinimo_EnumeraRespostasPorGrupoPor dtoSolicitacao);

		void SalvaPor(DTORespostaCurriculoMinimo_SalvaPor dtoCurriculoMinimo);

		void VerificaPermissaoParaSalvar(DTORespostaCurriculoMinimo_VerificaPermissaoParaSalvar dtoSolicitacao);
	}
}

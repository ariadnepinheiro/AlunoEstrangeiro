using System.Collections.Generic;

namespace Proderj.DOL.Service
{
	public class DTORespostaCurriculoMinimo_VerificaPermissaoParaSalvar : DTOCurriculoMinimo_VerificaPermissaoBase	
	{
		public List<DTORespostaCurriculoMinimo_RespostasPorGrupo> ListaResposta { get; set; }

		public string MatriculaDocente { get; set; }

		public short SubPeriodo { get; set; }
	}
}
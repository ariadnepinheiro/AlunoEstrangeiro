using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Architecture;

namespace Proderj.DOL.Service
{
	public interface ICadastroGlpService : IService
	{
		List<DTOCadastroGlp_Disciplina> ListaDisciplinasPor();

        List<DTOCadastroGlp_Disciplina> ListaDisciplinasPor(int num_func);

		void VerificaPermissaoParaInsercaoDocenteDisponivel(DTOCadastroGlp_VerificaPermissaoParaInsercaoDocenteDisponivel dtoSolicitacaoParaLancamento);

		void InsereDocenteDisponivel(DTOCadastroGlp_InsereDocenteDisponivel dtoInsereDocente);

		void RemoveDocenteDisponivel(int docenteDisponivelId, long numeroFuncionario);

		DTOCadastroGlp_DocenteLogadoComTelefone ObtemDocenteComTelefonePor(string matricula);

		List<DTOCadastroGlp_DocenteDisponivel> ListaDocentesDisponiveisPor(long numeroFuncionario);
		
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Architecture;

namespace Proderj.DOL.Service
{
    public interface ISelecaoTurmasService : IService
    {
		IEnumerable<DTOSelecaoTurmas> EnumeraSelecaoTurmasPor(string matricula);
		IEnumerable<DTOSelecaoTurmas> EnumeraSelecaoTurmasPor(long numeroFuncionarioDocente);
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Service
{
	public interface IPessoaService
	{
		void AtualizaTelefone(string telefone, long pessoaId);
	}
}

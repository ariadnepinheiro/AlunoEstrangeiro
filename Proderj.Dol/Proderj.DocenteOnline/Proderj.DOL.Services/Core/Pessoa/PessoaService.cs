using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Repository;
using Proderj.Foundation.Common;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Service
{
	public class PessoaService : IPessoaService
	{
		IPessoaRepository repositorioPessoa;

		public PessoaService(IPessoaRepository repositorioPessoa)
		{
			this.repositorioPessoa = repositorioPessoa;
		}

		public void AtualizaTelefone(string telefone, long pessoaId)
		{
			repositorioPessoa.AtualizaTelefone(telefone.OnlyNumbers(), pessoaId);
		}

   //     public void ObtemIdFuncional(string pessoaId)
     //   {
      //      repositorioPessoa.ObtemIdFuncional(pessoaId);
      //  }
	}
}

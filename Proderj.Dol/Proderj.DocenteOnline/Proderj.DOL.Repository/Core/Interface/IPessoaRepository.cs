using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Domain;
using Proderj.Foundation.Architecture;

namespace Proderj.DOL.Repository
{
	public interface IPessoaRepository : IRepository<Pessoa>
	{
		int AtualizaTelefone(string telefone, long identificador);

        string BuscaMatricula(string vinculo, string idfuncional);

        string BuscaNunFunc(string vinculo, long idfuncional);

        List<string> BuscaIdVinculo(string cpf); 

        string ObtemIdFuncional(string idfuncional);

        Pessoa ObtemPor(long identificador);
	}
}

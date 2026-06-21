using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Service
{
	public interface ILY_PESSOAService
	{
        DTOLY_PESSOA ObtemPor(int? pessoa);
        DTOLY_PESSOA ObtemPor(string idfuncional);
        DTOLY_PESSOA Atualiza(DTOAtualizaPessoa dto);
       // string ObtemIdFuncional(string idfuncional);
	}
}

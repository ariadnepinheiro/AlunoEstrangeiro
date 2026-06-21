using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Architecture;

namespace Proderj.DOL.Service
{
	public interface IApiService : IService
	{
        DTOApi ReceberDados(string chr);
        DTODocenteLogado ValidaUsuario(string crp);
        String EnviaUsuario(string usuario, string senha);
        string EnviaUsuario(string usuario, string senha, int duracaoDoTokenEmSegundos);
	}
}

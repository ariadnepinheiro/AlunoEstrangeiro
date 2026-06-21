using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Proderj.DOL.Service;

namespace Proderj.DOL.WebApp.Models
{
	public class DocenteLogadoBindModel
	{
		public string IdentificacaoUnica { get; private set; }
		public string Nome { get; private set; }
		public string Email { get; private set; }
        public string Senha { get; private set; }
		public string Matricula { get; private set; }
        public string IdFuncional { get; private set; }
        public string Vinculo { get; private set; }  
		public long NumeroFuncionario { get; private set; }
		public bool AceitouTermoDeAceite { get; private set;}

		public DocenteLogadoBindModel(DTODocenteLogadoPrincipal docenteLogadoPrincipal)
		{
			IdentificacaoUnica = docenteLogadoPrincipal.IdentificacaoUnica;
			Nome = docenteLogadoPrincipal.Nome;
			Email = docenteLogadoPrincipal.Email;
            Senha = docenteLogadoPrincipal.Senha;
			Matricula = docenteLogadoPrincipal.Matricula;
            IdFuncional = docenteLogadoPrincipal.IdFuncional;
            Vinculo = docenteLogadoPrincipal.Vinculo;
			NumeroFuncionario = docenteLogadoPrincipal.NumeroFuncionario;
			AceitouTermoDeAceite = docenteLogadoPrincipal.AceitouTermoDeAceite;
		}
	}
}

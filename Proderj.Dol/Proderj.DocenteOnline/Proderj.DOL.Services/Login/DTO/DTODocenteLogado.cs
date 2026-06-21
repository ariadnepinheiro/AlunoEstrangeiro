using System;

namespace Proderj.DOL.Service
{
    public class DTODocenteLogado
	{
		public string Matricula { get; set; }
        public string IdFuncional { get; set; }
        public string Vinculo { get; set; }
        public string Nome { get; set; }
        public string Senha { get; set; }
        public string Email { get; set; }
		public bool AlteracaoDeSenhaNecessaria { get; set; }
		public bool AceitouTermoDeAceite { get; set; }
    	public long NumeroFuncionario { get; set; }
	}
}

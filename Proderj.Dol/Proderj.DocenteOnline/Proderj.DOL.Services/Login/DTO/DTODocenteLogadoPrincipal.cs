using System.Security.Principal;

namespace Proderj.DOL.Service
{
	public class DTODocenteLogadoPrincipal : GenericPrincipal
	{
		public DTODocenteLogadoPrincipal(DTODocenteLogado dtoDocenteLogado)
			: base(new GenericIdentity(dtoDocenteLogado.Matricula), null)
		{
			this.identificacaoUnica = dtoDocenteLogado.Matricula;
			this.Email				= dtoDocenteLogado.Email;
            this.Senha              = dtoDocenteLogado.Senha;
			this.Nome				= dtoDocenteLogado.Nome;
			this.NumeroFuncionario	= dtoDocenteLogado.NumeroFuncionario;
            this.IdFuncional        = dtoDocenteLogado.IdFuncional;
            this.Vinculo            = dtoDocenteLogado.Vinculo;
			this.AceitouTermoDeAceite = dtoDocenteLogado.AceitouTermoDeAceite;
		}

	
		private readonly string identificacaoUnica;
		public string IdentificacaoUnica { get { return identificacaoUnica; } }
		public string Nome { get; set; }
		public string Email { get; set; }
        public string Senha { get; set; }
		public string Matricula { get { return identificacaoUnica; } }
		public bool AceitouTermoDeAceite { get; set; }
        public string IdFuncional { get; set; }
        public string Vinculo { get; set; }
		public long NumeroFuncionario { get; set; }
	}
}
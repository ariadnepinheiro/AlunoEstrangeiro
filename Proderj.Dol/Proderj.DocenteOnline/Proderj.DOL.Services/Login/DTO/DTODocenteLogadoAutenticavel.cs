using System;
using Proderj.Foundation.Framework.Web.Seguranca;

namespace Proderj.DOL.Service
{
	public class DTODocenteLogadoAutenticavel : IAutenticavel
	{
		public DTODocenteLogadoAutenticavel(string identificacaoUnica)
		{
			this.identificacaoUnica = identificacaoUnica;
		}

		private readonly string identificacaoUnica;
		public string IdentificacaoUnica
		{
			get { return identificacaoUnica; }
		}

		public string Nome { get; set; }
		public string Email { get; set; }
        public string Senha { get; set; }
        public string IdFuncional { get; set; }
        public string Vinculo { get; set; }
		public string Matricula { get { return identificacaoUnica; } }
		public bool AceitouTermoDeAceite { get; set; }
		public long NumeroFuncionario { get; set; }
	}
}
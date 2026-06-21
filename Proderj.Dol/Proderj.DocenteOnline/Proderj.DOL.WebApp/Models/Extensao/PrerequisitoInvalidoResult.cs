namespace Proderj.DOL.WebApp.Models
{
	public class PrerequisitoInvalidoResult : PrerequisitoResult
	{
		public PrerequisitoInvalidoResult(string nomeView, object modelo)
			: base(nomeView, modelo)
		{
			ehValido = false;
		}

		public override bool EhValido
		{
			get { return ehValido; }
		}
	}
}
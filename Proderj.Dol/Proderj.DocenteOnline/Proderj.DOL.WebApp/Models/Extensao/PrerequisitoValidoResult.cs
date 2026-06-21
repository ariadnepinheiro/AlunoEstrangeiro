namespace Proderj.DOL.WebApp.Models
{
	public class PrerequisitoValidoResult : PrerequisitoResult
	{
		public PrerequisitoValidoResult(string nomeView, object modelo)
			: base(nomeView, modelo)
		{
			ehValido = true;
		}

		public override bool EhValido
		{
			get { return ehValido; }
		}
	}
}
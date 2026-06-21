using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proderj.DOL.WebApp.Models
{
	public abstract class PrerequisitoResult : ViewResult
	{
		protected bool ehValido;
		public abstract bool EhValido { get; }
		public object Modelo { get { return ViewData.Model; } }

		protected PrerequisitoResult(string nomeView, object modelo)
		{
			ViewName = nomeView;
			ViewData.Model = modelo;
		}
	}


	//Extender para um prequesitoValido novo...

}
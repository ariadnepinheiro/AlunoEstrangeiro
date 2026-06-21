using System.Collections.Generic;
using System.Web.Mvc;

namespace Proderj.DOL.WebApp.Models
{
	public class ViewMontadaResult : ViewResult
	{
		public ViewMontadaResult(string nomeView, object modelo, ModelStateDictionary modelState)
		{
			ViewName = nomeView;
			ViewData.Model = modelo;
			if (modelState != null)
			{
				foreach (KeyValuePair<string, ModelState> item in modelState)	
				{
					ViewData.ModelState.Add(item);
				}
			}
		}
	}
}
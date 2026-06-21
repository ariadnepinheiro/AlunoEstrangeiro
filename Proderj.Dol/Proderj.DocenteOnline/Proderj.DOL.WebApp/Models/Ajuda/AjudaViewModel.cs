using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proderj.DOL.WebApp.Models
{
	public class AjudaViewModel : ViewModelPadrao
	{
		public AjudaViewModel()
		{
			CabecalhoModelo.BotaoAjudaHabilitado = false;
			CabecalhoModelo.BotaoInicioHabilitado = false;
		}
	}
}
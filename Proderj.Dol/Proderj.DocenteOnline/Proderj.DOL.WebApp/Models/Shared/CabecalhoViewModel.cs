using System;
using System.Globalization;

namespace Proderj.DOL.WebApp.Models
{
	public class CabecalhoViewModel
	{
		public bool BotaoSairHabilitado { get; set; }
		public bool BotaoAjudaHabilitado { get; set; }
		public bool BotaoInicioHabilitado { get; set; }

		public string TituloCabecalho { get; set; }

		public string LinkAjuda { get; set; }

		public string DataPorExtenso
		{
			get
			{
				string dataPorExtenso = DateTime.Now.ToString("dddd, dd \\de MMMM \\de yyyy");
				dataPorExtenso = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dataPorExtenso);
				dataPorExtenso = dataPorExtenso.Replace("De", "de");

				return dataPorExtenso;
			}
		}

		public DocenteLogadoBindModel DocenteLogadoModelo { get; set; }
	}
}
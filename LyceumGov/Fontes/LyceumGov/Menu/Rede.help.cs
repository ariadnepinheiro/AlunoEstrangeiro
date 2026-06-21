using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Menu
{
	public partial class Rede
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Divisão do sistema que organiza as funcionalidades de gestão da rede nos seguintes grupos:");
			help.Summary.Add("• Unidades Administrativas");
			help.Summary.Add("• Unidades de Ensino");
			help.Summary.Add("• Unidades Físicas");
			help.Summary.Add("• Quadro de Horários");
			help.Summary.Add("• Relatórios/Documentos");

			help.Oper.Add("Cada grupo é composto por funcionalidades do sistema que podem ser acessadas clicando em seu respectivo link exibido na página.");
		}
	}
}


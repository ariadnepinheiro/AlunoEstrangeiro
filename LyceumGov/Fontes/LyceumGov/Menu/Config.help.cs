using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Menu
{
	public partial class Config
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Divisão do sistema que organiza as funcionalidades de configurações nos seguintes grupos:");
			help.Summary.Add("• Pedagógico");
			help.Summary.Add("• Gestão da Rede");
			help.Summary.Add("• Gestão de Pessoas");
			help.Summary.Add("• Controle de Acesso");
			help.Summary.Add("• Configurações Gerais");

			help.Oper.Add("Cada grupo é composto por funcionalidades do sistema que podem ser acessadas clicando em seu respectivo link exibido na página.");
		}
	}
}

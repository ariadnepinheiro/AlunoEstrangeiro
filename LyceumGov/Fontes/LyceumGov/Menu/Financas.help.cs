using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Menu
{
	public partial class Financas
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Divisão do sistema que organiza as funcionalidades de orçamento e finanças nos seguintes grupos:");
			help.Summary.Add("• Catálogo de Itens e Fornecedores");
			help.Summary.Add("• Cadastros Básicos");
            help.Summary.Add("• Controle de Acesso");

			help.Oper.Add("Cada grupo é composto por funcionalidades do sistema que podem ser acessadas clicando em seu respectivo link exibido na página.");
		}
	}
}

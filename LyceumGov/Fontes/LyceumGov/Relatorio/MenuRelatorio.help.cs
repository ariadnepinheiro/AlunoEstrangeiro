using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Relatorio
{
	public partial class MenuRelatorio
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Consultar relatórios.");

			help.Oper.TitleAdd("Consultando relatórios");
			help.Oper.Add("Ao acessar a página de consultas por relatórios, todos os grupos de relatórios serão exibidos em forma de lista.");
			help.Oper.Add("Para consultar um relatório deve-se selecionar o grupo a que ele pertence, clicando na seta preta.");
			help.Oper.Add("Neste momento, serão exibidos os relatórios associados ao grupo selecionado.");
			help.Oper.Add("Em seguida deve-se clicar no relatório de interesse. Este link abre uma nova página que exibirá os dados do relatório.");
			help.Oper.Add("Obs.: As setas pretas identificam os grupos de relatórios que possuem relatórios a eles associados.");
			help.Oper.Add("Obs.: As setas brancas identificam os relatórios dos grupos que foram selecionados.");
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Curriculo
{
	public partial class VerbaPorCompetencia
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Consultar e editar verbas disponíveis de GLP por competência.");
			help.Oper.Add("Para consultar ou editar verbas é necessário selecionar o ano de interesse.");

			help.Oper.TitleAdd("Consultando verbas por competência");
			help.Oper.Add("A consulta é realizada automaticamente quando for selecionado o ano de interesse.");
			help.Oper.Add("Os registros serão apresentados na grade de verbas por competência.");

			help.Oper.TitleAdd("Alterando verbas de uma competência");
			help.Oper.Add("Para alterar verbas é necessário fazer a consulta de verbas por competência. Ver 'Consultando verbas por competência'.");
			help.Oper.Add("Para alterar os dados de uma verba deve-se clicar no botão ?I.", "~/img/bt_editar.png");
			help.Oper.Add("Os dados da verba serão carregados permitindo alterações nos campos.");
			help.Oper.Add("Para salvar os dados da verba deve-se clicar no botão ?I.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a alteração nos dados da verba deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");
			help.Oper.Add("Obs.: Serão editáveis apenas os meses superiores à data atual.");

			help.Oper.TitleAdd("Descrição dos Campos");
			help.Oper.TitleAdd("Área de Pesquisa");
			help.Oper.Add("• Ano: Ano de referência de referência para a consulta de verbas. (Tabela: Ano Letivo)");

			help.Oper.TitleAdd("Grade de Verbas por Competência");
			help.Oper.Add("• Mês: Mês de referência.");
			help.Oper.Add("• Valor Inicial: Valor inicial da verba no ano e mês de referência.");
			help.Oper.Add("• GLP Utilizada: Verba em reais de GLP já utilizada.");
			help.Oper.Add("• Saldo: Diferença entre o 'Valor Inicial' e a 'GLP Utilizada'.");

			help.Oper.TitleAdd("Botões");
			help.Oper.Add("?I: Salva a inserção/alteração da linha.", "~/img/bt_salvar.png");
			help.Oper.Add("?I: Cancela a inserção/alteração da nova linha.", "~/img/bt_cancelar.png");
			help.Oper.Add("?I: Permite alteração na linha.", "~/img/bt_editar.png");
			help.Oper.Add("?I: Limpa os filtros selecionados.", "~/img/bt_Limpa.png");
			help.Oper.Add("Obs.: A visibilidade dos botões depende da operação em andamento.");
		}
	}
}

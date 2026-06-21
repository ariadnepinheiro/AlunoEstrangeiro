using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Hades
{
	public partial class Relatorios
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Consultar, cadastrar, alterar e remover grupos de relatórios e seus relatórios.");
			help.Oper.Add("Permite a manutenção dos grupos de relatórios e de seus relatórios.");

			help.Oper.TitleAdd("Consultando grupos de relatórios e seus relatórios");
			help.Oper.Add("Ao acessar a página de relatórios, todas os grupos de relatórios cadastrados serão exibidos na grade de grupos e o primeiro registro é automaticamente selecionado de forma a exibir todos os relatórios associados a ele na grade de relatórios.");
			help.Oper.Add("Obs.: O nome do grupo de relatórios selecionado é destacado na tela acima da grade de relatórios.");
			help.Oper.Add("Para consultar os relatórios associados a outro grupo de relatórios deve-se selecionar o grupo desejado clicando na linha em que o grupo aparece na grade de grupos de relatórios.");
			help.Oper.Add("Os dados dos relatórios do grupo selecionado serão exibidos automaticamente na grade de relatórios.");
			help.Oper.Add("Para filtrar a consulta utilize a linha de filtro acima dos registros. Também é possível definir o tipo de filtro clicando no botão ?I.", "~/img/bt_filtro.jpg");
			help.Oper.Add("Para limpar o filtro deve-se clicar no botão ?I.", "~/img/bt_limpa.png");

			help.Oper.TitleAdd("Cadastrando grupos de relatórios");
			help.Oper.Add("Para cadastrar um novo grupo de relatórios deve-se clicar no botão ?I da grade de grupos de relatórios.", "~/img/bt_novo.png");
			help.Oper.Add("Deve-se preencher os campos com os dados do novo grupo de relatórios.");
			help.Oper.Add("Para salvar os dados do novo grupo de relatórios deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a inclusão dos dados do grupo de relatórios deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

			help.Oper.TitleAdd("Alterando grupos de relatórios");
			help.Oper.Add("Para alterar os dados de um grupo de relatórios deve-se clicar no botão ?I da grade de grupos de relatórios.", "~/img/bt_editar.png");
			help.Oper.Add("Os dados do grupo de relatórios serão carregados permitindo alterações nos campos.");
			help.Oper.Add("Para salvar os dados do grupo de relatórios deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a alteração nos dados do grupo de relatórios deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

			help.Oper.TitleAdd("Removendo grupos de relatórios");
			help.Oper.Add("Para remover um grupo de relatórios deve-se clicar no botão ?I da grade de grupos de relatórios e confirmar a remoção do registro.", "~/img/bt_exclui2.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

			help.Oper.TitleAdd("Cadastrando novo relatório de um grupo de relatórios");
			help.Oper.Add("Para cadastrar um novo relatório deve-se selecionar o grupo de relatórios desejado. Ver 'Consultando grupos de relatórios e seus relatórios'.");
			help.Oper.Add("Para cadastrar um relatório associado ao grupo selecionado deve-se clicar no botão ?I da grade de relatórios.", "~/img/bt_novo.png");
			help.Oper.Add("Deve-se preencher os campos com os dados do novo relatório.");
			help.Oper.Add("Para salvar os dados do novo relatório associado ao grupo selecionado deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a inclusão dos dados do novo relatório no grupo selecionado deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

			help.Oper.TitleAdd("Alterando relatório de um grupo de relatórios");
			help.Oper.Add("Para alterar um relatório deve-se selecionar o grupo de relatórios desejado. Ver 'Consultando grupos de relatórios e seus relatórios'.");
			help.Oper.Add("Para alterar os dados de um relatório associado ao grupo selecionado deve-se clicar no botão ?I da grade de relatórios.", "~/img/bt_editar.png");
			help.Oper.Add("Os dados do relatório serão carregados permitindo alterações nos campos.");
			help.Oper.Add("Para salvar os dados do relatório associado ao grupo selecionado deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a alteração nos dados do relatório associado ao grupo selecionado deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

			help.Oper.TitleAdd("Removendo relatório de um grupo de relatórios");
			help.Oper.Add("Para remover um relatório deve-se selecionar o grupo de relatórios desejado. Ver 'Consultando grupos de relatórios e seus relatórios'.");
			help.Oper.Add("Para remover os dados de um relatório associado ao grupo selecionado deve-se clicar no botão ?I da grade de relatórios e confirmar a remoção do registro.", "~/img/bt_exclui2.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

			help.Oper.TitleAdd("Descrição dos Campos");
			help.Oper.TitleAdd("Grupos de Relatórios");
			help.Oper.Add("• Grupo: Código identificador único do grupo de relatórios.");
			help.Oper.Add("• Descrição: Descrição funcional do grupo. É o nome dado ao grupo de relatórios.");

			help.Oper.TitleAdd("Relatórios");
			help.Oper.Add("• Relatório: Código identificador único do relatório.");
			help.Oper.Add("• Descrição: Descrição detalhada do relatório.");
			help.Oper.Add("• Arquivo: Arquivo referente ao relatório.");
			help.Oper.Add("• Função: Função referente ao relatório.");

			help.Oper.TitleAdd("Botões");
			help.Oper.Add("?I: Insere uma nova linha.", "~/img/bt_novo.png");
			help.Oper.Add("?I: Salva a inserção/alteração da linha.", "~/img/bt_salvar.png");
			help.Oper.Add("?I: Cancela a inserção/alteração da nova linha.", "~/img/bt_cancelar.png");
			help.Oper.Add("?I: Permite alteração na linha.", "~/img/bt_editar.png");
			help.Oper.Add("?I: Remove a linha.", "~/img/bt_exclui2.png");
			help.Oper.Add("?I: Limpa os filtros selecionados.", "~/img/bt_Limpa.png");
			help.Oper.Add("Obs.: A visibilidade dos botões depende da operação em andamento.");
		}
	}
}
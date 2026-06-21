using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Curriculo
{
	public partial class OutrasInstituicoes
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Consultar, cadastrar, alterar e remover instituições.");
			help.Summary.Add("Este cadastro será usado, por exemplo, na página de encerramento para registrar uma transferência de um aluno para uma instituição externa.");

			help.Oper.TitleAdd("Informando dados de pesquisa");
			help.Oper.Add("Para consultar, alterar ou remover uma instituição é necessário fazer uma pesquisa pela instituição de interesse clicando no botão ?I.", "~/Images/bt_drop.png");
			help.Oper.Add("Todas as instituições existentes são apresentadas em uma lista suspensa.");
			help.Oper.Add("A pesquisa pode ser filtrada pelo código e/ou descrição da instituição. Após definidas estas informações, deve-se pressionar a tecla ENTER para filtrar os resultados.");
			help.Oper.Add("Deve-se selecionar a instituição de interesse clicando na linha em que a instituição aparece na lista suspensa.");
			help.Oper.Add("Obs.: Utilize os caracteres '%' ou '*' para aplicar um filtro cujo valor está contido no campo pesquisado e não apenas quando estiver no início, conforme o exemplo abaixo:");
			help.Oper.Add("Exemplo: Para filtrar a coluna 'Descrição' da grade de pesquisa de unidades de ensino para que só sejam exibidos registros contendo a palavra 'José', digite %José ou *José na coluna 'Descrição' e pressione a tecla ENTER.");

			help.Oper.TitleAdd("Consultando instituições");
			help.Oper.Add("A consulta é realizada automaticamente quando a instituição de interesse for selecionada.");
			help.Oper.Add("Os dados da instituição serão exibidos em uma aba:");
			help.Oper.Add("1- Informações Gerais: Informações gerais da instituição.");

			help.Oper.TitleAdd("Cadastrando nova instituição");
			help.Oper.Add("Para cadastrar uma nova instituição deve-se clicar no botão ?I.", "~/img/bt_novo.png");
			help.Oper.Add("A aba 'Informações Gerais' deve ser selecionada e deve-se preencher os campos com os dados da nova instituição.");
			help.Oper.Add("Para salvar os dados da nova instituição deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a inclusão dos dados da nova instituição deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

			help.Oper.TitleAdd("Alterando instituição");
			help.Oper.Add("Para alterar uma instituição é necessário fazer a consulta da instituição desejada. Ver 'Consultando instituições'.");
			help.Oper.Add("Para alterar os dados da instituição selecionada deve-se clicar no botão ?I.", "~/img/bt_editar.png");
			help.Oper.Add("Os dados da instituição serão carregados permitindo alterações nos campos.");
			help.Oper.Add("Para salvar os dados da instituição selecionada deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a alteração nos dados da instituição selecionada deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

			help.Oper.TitleAdd("Removendo instituição");
			help.Oper.Add("Para remover uma instituição é necessário fazer a consulta da unidade de ensino desejada. Ver 'Consultando instituições'.");
			help.Oper.Add("Para remover os dados de uma instituição selecionada deve-se clicar no botão ?I e confirmar a remoção do registro.", "~/img/bt_exclui2.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

			help.Oper.TitleAdd("Descrição dos Campos");
			help.Oper.TitleAdd("• Área de Pesquisa");
			help.Oper.Add("• Instituição: Intituição de referência para a consulta.");

			help.Oper.TitleAdd("• Aba: Informações Gerais");
			help.Oper.Add("• Nome Completo: Nome completo da instituição.");
			help.Oper.Add("• CEP: CEP da instituição.");
			help.Oper.Add("• Município: Município no qual a instituição está localizada.");
			help.Oper.Add("• UF: Estado no qual a instituição está localizada.");
			help.Oper.Add("• Endereço: Endereço da instituição.");
			help.Oper.Add("• Número: Número da instituição.");
			help.Oper.Add("• Complemento: Complemento do endereço da instituição.");
			help.Oper.Add("• Bairro: Bairro da instituição.");

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Curriculo
{
	public partial class CHCargos
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Consultar, cadastrar, alterar e remover cargas horárias de cargos.");
			//help.PreReq.Add(typeof(Techne.Lyceum.Net.Basico.Funcoes));
			help.Oper.Add("Para consultar, cadastrar, alterar ou remover uma carga horária é necessário fazer uma pesquisa pelo cargo de interesse clicando no botão ?I.", "~/Images/bt_drop.png");
			help.Oper.Add("Todos os cargos existentes são apresentados em uma lista suspensa.");
			help.Oper.Add("A pesquisa pode ser filtrada pelo nome do cargo. Após definida esta informação, o resultado é apresentado automaticamente na lista suspensa.");
			help.Oper.Add("Deve-se selecionar o cargo de interesse clicando na linha em que o cargo aparece na lista suspensa.");
			help.Oper.Add("Obs.: Nesta pesquisa, utilize o caracter '%' para aplicar um filtro cujo valor está contido no campo pesquisado e não apenas quando estiver no início, conforme o exemplo abaixo:");
			help.Oper.Add("Exemplo: Para filtrar a coluna 'Cargo' para que só sejam exibidos registros contendo a palavra 'DOC', digite %DOC na coluna 'Cargo'.");

			help.Oper.TitleAdd("Consultando cargas horárias de um cargo");
			help.Oper.Add("A consulta é realizada automaticamente quando o cargo de interesse for selecionado.");
			help.Oper.Add("Os dados das cargas horárias associadas ao cargo selecionado serão exibidos na grade de cargas horárias.");
			help.Oper.Add("Para filtrar a consulta utilize a linha de filtro acima dos registros. Também é possível definir o tipo de filtro clicando no botão ?I.", "~/img/bt_filtro.jpg");
			help.Oper.Add("Para limpar o filtro deve-se clicar no botão ?I.", "~/img/bt_limpa.png");

			help.Oper.TitleAdd("Cadastrando nova carga horária de um cargo");
			help.Oper.Add("Para cadastrar uma nova carga horária é necessário fazer a consulta do cargo desejado. Ver 'Consultando cargas horárias de um cargo'.");
			help.Oper.Add("Para cadastrar uma nova carga horária associada ao cargo selecionado deve-se clicar no botão ?I.", "~/img/bt_novo.png");
			help.Oper.Add("Deve-se preencher os campos com os dados da nova carga horária.");
			help.Oper.Add("Para salvar os dados da nova carga horária deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a inclusão dos dados da carga horária deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

			help.Oper.TitleAdd("Alterando carga horária de um cargo");
			help.Oper.Add("Para alterar uma carga horária é necessário fazer a consulta do cargo desejado. Ver 'Consultando cargas horárias de um cargo'.");
			help.Oper.Add("Para alterar os dados de uma carga horária associada ao cargo selecionado deve-se clicar no botão ?I.", "~/img/bt_editar.png");
			help.Oper.Add("Os dados da carga horária serão carregados permitindo alterações nos campos.");
			help.Oper.Add("Para salvar os dados da carga horária deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a alteração nos dados da carga horária deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

			help.Oper.TitleAdd("Removendo carga horária de um cargo");
			help.Oper.Add("Para remover uma carga horária é necessário fazer a consulta do cargo desejado. Ver 'Consultando cargas horárias de um cargo'.");
			help.Oper.Add("Para remover uma carga horária associada ao cargo selecionado deve-se clicar no botão ?I e confirmar a remoção do registro.", "~/img/bt_exclui2.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

			help.Oper.TitleAdd("Descrição dos Campos");
			help.Oper.Add("• Função: Função referente à carga horária. (Tabela: Funções)");
			help.Oper.Add("• Nº Matrículas: Quantidade de matrículas referente à carga horária.");
			help.Oper.Add("• Readaptado: Indica se a função é readaptada.");
			help.Oper.Add("• GLP: Indica se a função possui GLP (Gratificação por Lotação Prioritária).");
			help.Oper.Add("• CH Semanal Total: Carga horária semanal total.");
			help.Oper.Add("• CH Semanal Efetiva: Carga horária semanal efetiva.");

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

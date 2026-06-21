using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Basico
{
	public partial class GrupoHabilitacaoDocente
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Consultar, cadastrar, alterar e remover grupos de habilitações em docentes.");

			help.Oper.Add("Para consultar, cadastrar, alterar ou remover um grupo de habilitações é necessário fazer uma pesquisa pelo docente de interesse clicando no botão ?I.", "~/Images/bt_drop.png");
			help.Oper.Add("A pesquisa pode ser feita pela matrícula, nome, RG, CPF e/ou número do funcionário. Após definidas estas informações, deve-se clicar em 'Buscar'. O resultado da pesquisa aparecerá em uma lista suspensa.");
			help.Oper.Add("Deve-se selecionar o docente de interesse clicando na linha em que o docente aparece na lista suspensa.");

			help.Oper.TitleAdd("Consultando grupos de habilitações em um docente");
			help.Oper.Add("A consulta é realizada automaticamente quando o docente de interesse for selecionado.");
			help.Oper.Add("Os dados dos grupos de habilitações associados ao docente selecionado serão exibidos e divididos em duas abas:");
			help.Oper.Add("1 - Disciplinas Habilitadas: Lista de grupos de habilitações cujas disciplinas são ministradas pelo docente.");
			help.Oper.Add("2 - Disciplinas Habilitadas Provisoriamente: Lista de grupos de habilitações cujas disciplinas são ministradas provisoriamente pelo docente.");
			help.Oper.Add("Para filtrar a consulta utilize a linha de filtro acima dos registros. Também é possível definir o tipo de filtro clicando no botão ?I.", "~/img/bt_filtro.jpg");
			help.Oper.Add("Para limpar o filtro deve-se clicar no botão ?I.", "~/img/bt_limpa.png");

			help.Oper.TitleAdd("Cadastrando novo grupo de habilitações de disciplinas habilitadas em um docente");
			help.Oper.Add("Para cadastrar um novo grupo de habilitações de disciplinas habilitadas é necessário fazer a consulta do docente desejado. Ver 'Consultando grupos de habilitações em um docente'.");
			help.Oper.Add("Para cadastrar um novo grupo de habilitações de disciplinas habilitadas no docente selecionado a aba 'Disciplinas Habilitadas' deve estar selecionada e deve-se clicar no botão ?I.", "~/img/bt_novo.png");
			help.Oper.Add("Deve-se preencher os campos com os dados do novo grupo de habilitações de disciplinas habilitadas.");
			help.Oper.Add("Para salvar os dados do novo grupo de habilitações de disciplinas habilitadas deve-se clicar no botão ?I.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a inclusão dos dados do novo grupo de habilitações de disciplinas habilitadas deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a inclusão.");

			help.Oper.TitleAdd("Alterando grupo de habilitações de disciplinas habilitadas em um docente");
			help.Oper.Add("Para alterar um grupo de habilitações de disciplinas habilitadas é necessário fazer a consulta do docente desejado. Ver 'Consultando grupos de habilitações em um docente'.");
			help.Oper.Add("Para alterar os dados de um grupo de habilitações de disciplinas habilitadas no docente selecionado a aba 'Disciplinas Habilitadas' deve estar selecionada e deve-se clicar no botão ?I.", "~/img/bt_editar.png");
			help.Oper.Add("Os dados do grupo de habilitações de disciplinas habilitadas serão carregados permitindo alterações nos campos.");
			help.Oper.Add("Para salvar os dados do grupo de habilitações de disciplinas habilitadas deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a alteração nos dados do grupo de habilitações de disciplinas habilitadas deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

			help.Oper.TitleAdd("Removendo grupo de habilitações de disciplinas habilitadas em um docente");
			help.Oper.Add("Para remover um grupo de habilitações de disciplinas habilitadas é necessário fazer a consulta do docente desejado. Ver 'Consultando grupos de habilitações em um docente'.");
			help.Oper.Add("Para remover um grupo de habilitações de disciplinas habilitadas associado ao docente selecionado a aba 'Disciplinas Habilitadas' deve estar selecionada e deve-se clicar no botão ?I e confirmar a remoção do registro.", "~/img/bt_exclui2.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

			help.Oper.TitleAdd("Cadastrando novo grupo de habilitações de disciplinas habilitadas provisoriamente em um docente");
			help.Oper.Add("Para cadastrar um novo grupo de habilitações de disciplinas habilitadas provisoriamente é necessário fazer a consulta do docente desejado. Ver 'Consultando grupos de habilitações em um docente'.");
			help.Oper.Add("Para cadastrar um novo grupo de habilitações de disciplinas habilitadas provisoriamente no docente selecionado a aba 'Disciplinas Habilitadas Provisoriamente' deve estar selecionada e deve-se clicar no botão ?I.", "~/img/bt_novo.png");
			help.Oper.Add("Deve-se preencher os campos com os dados do novo grupo de habilitações de disciplinas habilitadas provisoriamente.");
			help.Oper.Add("Para salvar os dados do novo grupo de habilitações de disciplinas habilitadas provisoriamente deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a inclusão dos dados do novo grupo de habilitações de disciplinas habilitadas provisoriamente deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a inclusão.");

			help.Oper.TitleAdd("Alterando grupo de habilitações de disciplinas habilitadas provisoriamente em um docente");
			help.Oper.Add("Para alterar um grupo de habilitações de disciplinas habilitadas provisoriamente é necessário fazer a consulta do docente desejado. Ver 'Consultando grupos de habilitações em um docente'.");
			help.Oper.Add("Para alterar os dados de um grupo de habilitações de disciplinas habilitadas provisoriamente no docente selecionado a aba 'Disciplinas Habilitadas Provisoriamente' deve estar selecionada e deve-se clicar no botão ?I.", "~/img/bt_editar.png");
			help.Oper.Add("Os dados do grupo de habilitações de disciplinas habilitadas provisoriamente serão carregados permitindo alterações nos campos.");
			help.Oper.Add("Para salvar os dados do grupo de habilitações de disciplinas habilitadas provisoriamente deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a alteração nos dados do grupo de habilitações de disciplinas habilitadas provisoriamente deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

			help.Oper.TitleAdd("Removendo grupo de habilitações de disciplinas habilitadas provisoriamente em um docente");
			help.Oper.Add("Para remover um grupo de habilitações de disciplinas habilitadas provisoriamente é necessário fazer a consulta do docente desejado. Ver 'Consultando grupos de habilitações em um docente'.");
			help.Oper.Add("Para remover um grupo de habilitações de disciplinas habilitadas provisoriamente associado ao docente selecionado a aba 'Disciplinas Habilitadas Provisoriamente' deve estar selecionada e deve-se clicar no botão ?I e confirmar a remoção do registro.", "~/img/bt_exclui2.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

			help.Oper.TitleAdd("Descrição dos Campos");
			help.Oper.Add("• Agrupamento: Identificação textual do grupo de habilitações.");
			help.Oper.Add("• Data Limite: Data para a desabilitação do grupo de habilitações de disciplinas habilitadas provisoriamente em um docente.");
			help.Oper.Add("• Grupo de Ingresso: Indica se o grupo é ou não de ingresso.");
            help.Oper.Add("• Habilitação Matrícula: Indica se o grupo é habilitado para matrícula.");
            help.Oper.Add("• Habilitação GLP Indica se o grupo é habilitado para GLP.");
            help.Oper.Add("• Obs.: Habilitação Matrícula/GLP: Pode estar um só marcado ou os dois e indica que o docente só poderá ser alocado em aulas normais ou GLP, ou em ambas nesta disciplina.");

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

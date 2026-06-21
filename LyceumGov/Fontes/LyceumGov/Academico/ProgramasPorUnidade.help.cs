using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Academico
{
	public partial class ProgramasPorUnidade
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Consultar, cadastrar, alterar e remover programas sociais por unidade de ensino.");

			help.Oper.TitleAdd("Informando dados de pesquisa");
			help.Oper.Add("Para consultar, alterar ou remover um programa social é necessário fazer uma pesquisa pela unidade de ensino de interesse clicando no botão ?I.", "~/Images/bt_drop.png");
			help.Oper.Add("Todas as unidades de ensino existentes são apresentadas em uma lista suspensa.");
			help.Oper.Add("A pesquisa pode ser filtrada pelo código, descrição, U.A., CNPJ ou situação da unidade de ensino. Após definidas estas informações, deve-se pressionar a tecla ENTER para filtrar os resultados.");
			help.Oper.Add("Deve-se selecionar a unidade de ensino de interesse clicando na linha em que a unidade aparece na lista suspensa.");
			help.Oper.Add("Obs.: Utilize os caracteres '%' ou '*' para aplicar um filtro cujo valor está contido no campo pesquisado e não apenas quando estiver no início, conforme o exemplo abaixo:");
			help.Oper.Add("Exemplo: Para filtrar a coluna 'Descrição' da grade de pesquisa de unidades de ensino para que só sejam exibidos registros contendo a palavra 'José', digite %José ou *José na coluna 'Descrição' e pressione a tecla ENTER.");

			help.Oper.TitleAdd("Consultando programas sociais de uma unidade de ensino");
			help.Oper.Add("A consulta é realizada automaticamente quando a unidade de ensino de interesse for selecionada.");
			help.Oper.Add("Os dados dos programas sociais associados à unidade de ensino selecionada serão exibidos na grade de programas por unidade.");

			help.Oper.TitleAdd("Cadastrando novo programa social em uma unidade de ensino");
			help.Oper.Add("Para cadastrar um novo programa social é necessário fazer a consulta da unidade de ensino desejada. Ver 'Consultando programas sociais de uma unidade de ensino'.");
			help.Oper.Add("Para cadastrar um novo programa social associado à unidade de ensino selecionada deve-se clicar no botão ?I.", "~/img/bt_novo.png");
			help.Oper.Add("Deve-se preencher os campos com os dados do novo programa social.");
			help.Oper.Add("Para salvar os dados do novo programa social associado à unidade de ensino selecionada deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a inclusão dos dados do novo programa social associado à unidade de ensino selecionada deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

			help.Oper.TitleAdd("Alterando programa social de uma unidade de ensino");
			help.Oper.Add("Para alterar um programa social é necessário fazer a consulta da unidade de ensino desejada. Ver 'Consultando programas sociais de uma unidade de ensino'.");
			help.Oper.Add("Para alterar os dados de um programa social associado à unidade de ensino selecionada deve-se clicar no botão ?I.", "~/img/bt_editar.png");
			help.Oper.Add("Os dados do programa social serão carregados permitindo alterações nos campos.");
			help.Oper.Add("Para salvar os dados do programa social associado à unidade de ensino selecionada deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a alteração nos dados do programa social associado à unidade de ensino selecionada deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

			help.Oper.TitleAdd("Removendo programa social de uma unidade de ensino");
			help.Oper.Add("Para remover um programa social é necessário fazer a consulta da unidade de ensino desejada. Ver 'Consultando programas sociais de uma unidade de ensino'.");
			help.Oper.Add("Para remover os dados de um programa social associado à unidade de ensino selecionada deve-se clicar no botão ?I e confirmar a remoção do registro.", "~/img/bt_exclui2.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

			help.Oper.TitleAdd("Descrição dos Campos");
			help.Oper.Add("• Unidade de Ensino: Unidade de ensino para a qual estão cadastrados os programas sociais.");
			help.Oper.Add("• Agência de Fomento: Código da instituição que concede o programa social.");
			help.Oper.Add("• Programa: Código do programa social.");
			help.Oper.Add("• Tipo de Benefícios: Código do tipo de benefícios.");
			help.Oper.Add("• Ano de Validade: Ano de validade do programa social nesta unidade de ensino.");
			help.Oper.Add("• Data de Início: Data de início do programa social nesta unidade de ensino.");
			help.Oper.Add("• Data Final: Data final do programa social nesta unidade de ensino.");

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

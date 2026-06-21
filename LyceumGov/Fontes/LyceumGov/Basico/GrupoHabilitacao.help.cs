using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Basico
{
	public partial class GrupoHabilitacao
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Consultar, cadastrar, alterar e remover grupos de habilitações.");

			help.Oper.TitleAdd("Consultando grupos de habilitações");
			help.Oper.Add("Ao acessar a página de grupos de habilitações, todos os grupos de habilitações cadastrados serão exibidos.");
			help.Oper.Add("Para filtrar a consulta utilize a linha de filtro acima dos registros. Também é possível definir o tipo de filtro clicando no botão ?I.", "~/img/bt_filtro.jpg");
			help.Oper.Add("Para limpar o filtro deve-se clicar no botão ?I.", "~/img/bt_limpa.png");

			help.Oper.TitleAdd("Cadastrando novo grupo de habilitações");
			help.Oper.Add("Para cadastrar um grupo de habilitações deve-se clicar no botão ?I.", "~/img/bt_novo.png");
			help.Oper.Add("Deve-se preencher os campos com os dados do novo grupo de habilitações.");
			help.Oper.Add("Para salvar os dados do novo grupo de habilitações deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a inclusão dos dados do grupo de habilitações deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

			help.Oper.TitleAdd("Alterando grupo de habilitações");
			help.Oper.Add("Para alterar os dados de um grupo de habilitações deve-se clicar no botão ?I.", "~/img/bt_editar.png");
			help.Oper.Add("Os dados do grupo de habilitações serão carregados permitindo alterações nos campos.");
			help.Oper.Add("Para salvar os dados do grupo de habilitações deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a alteração nos dados do grupo de habilitações deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

			help.Oper.TitleAdd("Removendo grupo de habilitações");
			help.Oper.Add("Para remover os dados do grupo de habilitações deve-se clicar no botão ?I e confirmar a remoção do registro.", "~/img/bt_exclui2.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

			help.Oper.TitleAdd("Descrição dos Campos");
			help.Oper.Add("• Grupo: Código mnemônico do grupo de habilitações.");
			help.Oper.Add("• Descrição: Descrição detalhada do grupo de habilitações.");
            help.Oper.Add("• Disponibilidade GLP – DOL: Opção para que esse grupo tenha disponibilidade GLP no Docente Online.");
            help.Oper.Add("•Atividade Complementar: Indica se o grupo permite realização de atividade complementar.");


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

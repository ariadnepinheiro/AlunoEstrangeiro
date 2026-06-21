using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Basico
{
	public partial class NivelCurso
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Consultar, cadastrar, alterar e remover níveis de ensino.");

			help.Oper.Add("Identifica o nível de ensino existente na instituição.");
			help.Oper.Add("Uma mesma instituição pode oferecer diversos níveis de ensino: fundamental, médio, profissionalizante etc.");
			help.Oper.Add("Obs.: Estas informações serão usadas posteriormente pela transação 'Escolaridades'.");

			help.Oper.TitleAdd("Consultando níveis de ensino");
			help.Oper.Add("Ao acessar a página de níveis de ensino, todos os níveis cadastrados serão exibidos.");
			help.Oper.Add("Para filtrar a consulta utilize a linha de filtro acima dos registros. Também é possível definir o tipo de filtro clicando no botão ?I.", "~/img/bt_filtro.jpg");
			help.Oper.Add("Para limpar o filtro deve-se clicar no botão ?I.", "~/img/bt_limpa.png");

			help.Oper.TitleAdd("Cadastrando novo nível de ensino");
			help.Oper.Add("Para cadastrar um novo nível de ensino deve-se clicar no botão ?I.", "~/img/bt_novo.png");
			help.Oper.Add("Deve-se preencher os campos com os dados do novo nível de ensino.");
			help.Oper.Add("Para salvar os dados do novo nível de ensino deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a inclusão dos dados do nível de ensino deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

			help.Oper.TitleAdd("Alterando nível de ensino");
			help.Oper.Add("Para alterar os dados de um nível de ensino deve-se clicar no botão ?I.", "~/img/bt_editar.png");
			help.Oper.Add("Os dados do nível de ensino serão carregados permitindo alterações nos campos.");
			help.Oper.Add("Para salvar os dados do nível de ensino deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a alteração nos dados do nível de ensino deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

			help.Oper.TitleAdd("Removendo nível de ensino");
			help.Oper.Add("Para remover um nível de ensino deve-se clicar no botão ?I e confirmar a remoção do registro.", "~/img/bt_exclui2.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

			help.Oper.TitleAdd("Descrição dos Campos");
			help.Oper.Add("• Nível: Nível de ensino de uma instituição.");
			help.Oper.Add("• Descrição: Descrição detalhada do nível de ensino.");
            help.Oper.Add("• Detalhe: Detalhe referente ao nível de ensino. (Tabela Geral: DetalheTipoCurso)");

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
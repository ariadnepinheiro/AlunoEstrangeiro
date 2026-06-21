using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Basico
{
	public partial class PeriodoLetivo
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Consultar, cadastrar, alterar e remover anos letivos.");

			help.Oper.TitleAdd("Consultando anos letivos");
			help.Oper.Add("Ao acessar a página de anos letivos, todos os anos letivos cadastrados serão exibidos.");
			help.Oper.Add("Para filtrar a consulta utilize a linha de filtro acima dos registros. Também é possível definir o tipo de filtro clicando no botão ?I.", "~/img/bt_filtro.jpg");
			help.Oper.Add("Para limpar o filtro deve-se clicar no botão ?I.", "~/img/bt_limpa.png");

			help.Oper.TitleAdd("Cadastrando novo ano letivo");
			help.Oper.Add("Para cadastrar um novo ano letivo deve-se clicar no botão ?I.", "~/img/bt_novo.png");
			help.Oper.Add("Deve-se preencher os campos com os dados do novo ano letivo.");
			help.Oper.Add("Para salvar os dados do novo ano letivo deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a inclusão dos dados do ano letivo deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

			help.Oper.TitleAdd("Alterando ano letivo");
			help.Oper.Add("Para alterar os dados de um ano letivo deve-se clicar no botão ?I.", "~/img/bt_editar.png");
			help.Oper.Add("Os dados do ano letivo serão carregados permitindo alterações nos campos.");
			help.Oper.Add("Para salvar os dados do ano letivo deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a alteração nos dados do ano letivo deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

			help.Oper.TitleAdd("Removendo ano letivo");
			help.Oper.Add("Para remover um ano letivo deve-se clicar no botão ?I e confirmar a remoção do registro.", "~/img/bt_exclui2.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

			help.Oper.TitleAdd("Descrição dos Campos");
			help.Oper.Add("• Ano: Ano referente ao ano.");
			help.Oper.Add("• Período: Código do período.");
			help.Oper.Add("• Data Início: Data de início da validade do ano letivo.");
			help.Oper.Add("• Data Fim: Data de fim da validade do ano letivo.");
			help.Oper.Add("• Data Início Aula: Data de início das aulas do ano letivo.");
			help.Oper.Add("• Data Fim Aula: Data de fim das aulas do ano letivo.");
			help.Oper.Add("• Data início Docente: Data de início de trabalho para os docentes.");
			help.Oper.Add("• Data Fim Docente: Data de fim de trabalho para os docentes.");
			help.Oper.Add("• Especificação: Especificação do ano letivo.");
			help.Oper.Add("• Próximo Ano: Campo para relacionar este ano letivo com o próximo.");
			help.Oper.Add("• Próximo Período: Campo para relacionar este Ano Letivo com o próximo.");

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

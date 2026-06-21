using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Hades
{
	public partial class Usuarios
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Consultar, cadastrar, alterar e desabilitar usuários e seus padrões de acesso.");
			help.Oper.Add("Para consultar, alterar ou desabilitar um usuário é necessário fazer uma pesquisa pelo usuário de interesse clicando no botão ?I.", "~/Images/bt_drop.png");
			help.Oper.Add("Todos os usuários existentes são apresentados em uma lista suspensa.");
			help.Oper.Add("A pesquisa pode ser filtrada pelo login e/ou nome do usuário. Após definidas estas informações, o resultado é apresentado automaticamente na lista suspensa.");
			help.Oper.Add("Deve-se selecionar o usuário de interesse clicando na linha em que o usuário aparece na lista suspensa.");
			help.Oper.Add("Obs.: Nesta pesquisa, utilize os caracteres '%' ou '*' para aplicar um filtro cujo valor está contido no campo pesquisado e não apenas quando estiver no início, conforme o exemplo abaixo:");
			help.Oper.Add("Exemplo: Para filtrar a coluna 'Nome' para que só sejam exibidos registros contendo a palavra 'Paula', digite %Paula ou *Paula na coluna 'Nome'.");

			help.Oper.TitleAdd("Consultando um usuário");
			help.Oper.Add("A consulta é realizada automaticamente quando o usuário de interesse for selecionado.");
			help.Oper.Add("Os dados do usuário selecionado serão exibidos e divididos em duas abas:");
			help.Oper.Add("1 - Dados do Usuário: Dados gerais do usuário.");
			help.Oper.Add("2 - Padrões de Acesso: Padrões de acesso definidos para o usuário.");
 
			help.Oper.TitleAdd("Cadastrando novo usuário");
			help.Oper.Add("Para cadastrar um novo usuário deve-se clicar no botão ?I.", "~/Images/SmallNew.png");
			help.Oper.Add("Será carregado um formulário em branco para preenchimento dos dados.");
			help.Oper.Add("Deve-se preencher os campos com os dados do novo usuário na aba 'Dados do Usuário'.");
			help.Oper.Add("Para salvar os dados do novo usuário deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/Images/SmallOk.png");
			help.Oper.Add("Para cancelar a inclusão dos dados do usuário deve-se clicar no botão ?I.", "~/Images/SmallCancel.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");
			help.Oper.Add("Obs.: No modo de cadastro de usuários, a aba 'Padrões de Acesso' fica desabilitada.");

			help.Oper.TitleAdd("Alterando usuário");
			help.Oper.Add("Para alterar um usuário é necessário fazer a consulta do usuário desejado. Ver 'Consultando um usuário'.");
			help.Oper.Add("Para alterar os dados do usuário selecionado a aba 'Dados do Usuário' deve ser selecionada e deve-se clicar no botão ?I.", "~/Images/SmallEdit.png");
			help.Oper.Add("Os dados do usuário serão carregados na abas 'Dados do Usuário' permitindo alterações nos campos.");
			help.Oper.Add("Para salvar os dados do usuário deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/Images/SmallOk.png");
			help.Oper.Add("Para cancelar a alteração nos dados do usuário deve-se clicar no botão ?I.", "~/Images/SmallCancel.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

			help.Oper.TitleAdd("Desabilitando usuário");
			help.Oper.Add("Para desabilitar um usuário é necessário fazer a consulta do usuário desejado. Ver 'Consultando um usuário'.");
			help.Oper.Add("Para desabilitar o usuário selecionado a aba 'Dados do Usuário' deve ser selecionada e deve-se clicar no botão ?I e confirmar a desabilitação do registro.", "~/Images/SmallDelete.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

			help.Oper.TitleAdd("Cadastrando padrão de acesso");
			help.Oper.Add("Para cadastrar um novo padrão de acesso é necessário fazer a consulta do usuário desejado. Ver 'Consultando um usuário'.");
			help.Oper.Add("Para cadastrar um novo padrão de acesso associado ao usuário selecionado a aba 'Padrões de Acesso' deve ser selecionada e deve-se clicar no botão ?I da grade de padrões de acesso.", "~/img/bt_novo.png");
			help.Oper.Add("Deve-se selecionar o padrão de acesso na lista suspensa para incluí-lo no usuário selecionado.");
			help.Oper.Add("Para salvar a inclusão do padrão de acesso no usuário selecionado deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a inclusão do padrão de acesso no usuário selecionado deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

			help.Oper.TitleAdd("Removendo padrão de acesso");
			help.Oper.Add("Para remover um padrão de acesso é necessário fazer a consulta do usuário desejado. Ver 'Consultando um usuário'.");
			help.Oper.Add("Para remover um padrão de acesso associado ao usuário selecionado a aba 'Padrões de Acesso' deve ser selecionada e deve-se clicar no botão ?I da grade de padrões de acesso e confirmar a remoção do registro.", "~/Images/bot_desabil.png");

			help.Oper.TitleAdd("Descrição dos Campos");
			help.Oper.TitleAdd("Aba: Dados do Usuário");
			help.Oper.Add("• Matrícula: matrícula do servidor ou funcionário associado ao novo usuário. Pode ser selecionada através do número de matrícula, nome, cpf, documento e/ou pessoa.");
			help.Oper.Add("• Nome: Nome completo do servidor ou funcionário.");
			help.Oper.Add("• Telefone: Telefone principal de contato do servidor ou funcionário.");
			help.Oper.Add("• Celular: Celular de contato do servidor ou funcionário.");
			help.Oper.Add("• E-mail: Endereço externo de correio eletrônico do servidor ou funcionário.");
			help.Oper.Add("• Unidade Administrativa: Unidade administrativa a que o usuário pertence.");
			help.Oper.Add("• Usuário: Login do usuário.");
			help.Oper.Add("• Senha: Senha utilizada para o controle de acesso do usuário ao sistema.");
			help.Oper.Add("• Privilegiado: Informa se o usuário possui o privilégio de acesso a todas as páginas do sistema independente do seu padrão de acesso e aos dados de todas as unidades físicas independente da configuração de restrição de acesso.");
			help.Oper.Add("• Habilitado: Informa se o usuário está habilitado.");

			help.Oper.TitleAdd("Aba: Padrões de Acesso");
			help.Oper.Add("• Padrão de Acesso: Padrão de acesso habilitado para o usuário. (Tabela: Padrões de Acesso)");

			help.Oper.TitleAdd("Botões");
			help.Oper.TitleAdd("Dados do Usuário");
			help.Oper.Add("?I: Carrega um formulário para novo registro.", "~/Images/SmallNew.png");
			help.Oper.Add("?I: Salva as alterações do registro.", "~/Images/SmallOk.png");
			help.Oper.Add("?I: Cancela a operação corrente e retorna para página inicial.", "~/Images/SmallCancel.png");
			help.Oper.Add("?I: Permite alteração no registro.", "~/Images/SmallEdit.png");
			help.Oper.Add("?I: Desabilita o registro.", "~/Images/bot_desabil.png");

			help.Oper.TitleAdd("Padrões de Acesso");
			help.Oper.Add("?I: Insere uma nova linha.", "~/img/bt_novo.png");
			help.Oper.Add("?I: Salva a inserção da linha.", "~/img/bt_salvar.png");
			help.Oper.Add("?I: Cancela a inserção da nova linha.", "~/img/bt_cancelar.png");
			help.Oper.Add("?I: Remove a linha.", "~/img/bt_exclui2.png");
			help.Oper.Add("?I: Limpa os filtros selecionados.", "~/img/bt_Limpa.png");
			help.Oper.Add("Obs.: A visibilidade dos botões depende da operação em andamento.");
		}
	}
}

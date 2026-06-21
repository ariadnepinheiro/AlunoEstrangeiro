using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Hades
{
	public partial class PadacesRelatorios
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Consultar, cadastrar, alterar e remover padrões de acessos, suas transações, seus usuários e seus relatórios.");

			help.Oper.Add("Os padrões de acesso reúnem as funcionalidades do sistema de acordo com um perfil de usuário.");

			help.Oper.TitleAdd("Consultando padrões de acesso");
			help.Oper.Add("Ao acessar a página de padrões de acesso, a consulta é realizada automaticamente.");
			help.Oper.Add("Todos os padrões de acesso cadastrados serão exibidos na grade de padrões de acesso.");
			help.Oper.Add("Para filtrar a consulta utilize a linha de filtro acima dos registros. Também é possível definir o tipo de filtro clicando no botão ?I.", "~/img/bt_filtro.jpg");
			help.Oper.Add("Para limpar o filtro deve-se clicar no botão ?I.", "~/img/bt_limpa.png");

			help.Oper.TitleAdd("Cadastrando novo padrão de acesso");
			help.Oper.Add("Para cadastrar um novo padrão de acesso deve-se clicar no botão ?I.", "~/img/bt_novo.png");
			help.Oper.Add("Deve-se preencher os campos com os dados do novo padrão de acesso.");
			help.Oper.Add("Para salvar os dados do novo padrão de acesso deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a inclusão dos dados do padrão de acesso deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

			help.Oper.TitleAdd("Alterando padrão de acesso");
			help.Oper.Add("Para alterar os dados de um padrão de acesso deve-se clicar no botão ?I.", "~/img/bt_editar.png");
			help.Oper.Add("Os dados do padrão de acesso serão carregados permitindo alterações nos campos.");
			help.Oper.Add("Para salvar os dados do padrão de acesso deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a alteração nos dados do padrão de acesso deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

			help.Oper.TitleAdd("Removendo padrão de acesso");
			help.Oper.Add("Para remover um padrão de acesso deve-se clicar no botão ?I e confirmar a remoção do registro.", "~/img/bt_exclui2.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

			help.Oper.TitleAdd("Consultando transações de um padrão de acesso");
			help.Oper.Add("Para consultar transações é necessário selecionar o padrão de acesso de interesse em sua respectiva grade.");
			help.Oper.Add("Para consultar as transações associadas ao padrão de acesso selecionado deve-se clicar no botão 'Transações'.");
			help.Oper.Add("Os dados das transações serão exibidos em estrutura de árvore em uma nova página.");
			help.Oper.Add("Obs.: A estrutura de árvore é composta por nós e níveis. Cada nó está vinculado a um nível. Um nó de nível maior pode apresentar um ou mais nós de nível menor.");
			help.Oper.Add("Obs.: Caso não seja exibido nenhum nó, significa que o padrão de acesso selecionado não possui nenhuma transação.");
			help.Oper.Add("Os dados das transações estão organizados da seguinte forma:");
			help.Oper.Add("• O primeiro nível é formado por grupos de funcionalidades. Cada grupo de funcionalidades é classificado de acordo com o Menu a que está associado no sistema.");
			help.Oper.Add("• O segundo nível é formado pelas funcionalidades do grupo correspondente. Assim, este nível corresponde às funcionalidades existentes em um determinado Menu.");
			help.Oper.Add("• O terceiro nível é formado por três nós: Adicionar, Alterar e Remover. Este nível identifica as ações permitidas para uma determinada funcionalidade.");
			help.Oper.Add("Obs.: Cada funcionalidade possui quatro ações: Consultar, Adicionar, Alterar e Remover. Na estrutura de árvore, quando não possuir nenhum nó de terceiro nível habilitado, a funcionalidade terá apenas a ação de consulta.");

			help.Oper.TitleAdd("Alterando transações de um padrão de acesso");
			help.Oper.Add("Para alterar transações é necessário consultar as transações do padrão de acesso de interesse. Ver 'Consultando transações de um padrão de acesso'.");
			help.Oper.Add("Para alterar as transações associadas ao padrão de acesso selecionado deve-se clicar no botão 'Alterar'.");
			help.Oper.Add("Todos os Menus cadastrados no sistema serão exibidos em forma de lista.");
			help.Oper.Add("Deve-se selecionar o Menu que se deseja manter as transações associadas ao padrão de acesso selecionado.");
			help.Oper.Add("Assim que um Menu é selecionado, todas as transações cadastradas no sistema serão exibidas em estrutura de árvore, sendo habilitadas as transações associadas ao padrão de acesso selecionado.");
			help.Oper.Add("Deve-se marcar ou desmarcar as transações referentes ao Menu selecionado.");
			help.Oper.Add("Obs.: Caso outro Menu seja selecionado na lista lateral, os dados alterados das transações do Menu corrente serão perdidos.");
			help.Oper.Add("Para salvar a alteração dos dados das transações associadas ao Menu selecionado deve-se clicar em 'Salvar' e repetir os passos anteriores para alterar outro Menu.");
			help.Oper.Add("Para cancelar a alteração dos dados das transações associadas ao Menu selecionado deve-se clicar em 'Cancelar'.");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

			help.Oper.TitleAdd("Consultando usuários de um padrão de acesso");
			help.Oper.Add("Para consultar usuários é necessário selecionar o padrão de acesso de interesse em sua respectiva grade.");
			help.Oper.Add("Para consultar os usuários associados ao padrão de acesso selecionado deve-se clicar no botão 'Usuários'.");
			help.Oper.Add("Todos os usuários associados ao padrão de acesso selecionado serão exibidos na grade de usuários em uma nova página.");
			help.Oper.Add("Para filtrar a consulta utilize a linha de filtro acima dos registros. Também é possível definir o tipo de filtro clicando no botão ?I.", "~/img/bt_filtro.jpg");
			help.Oper.Add("Para limpar o filtro deve-se clicar no botão ?I.", "~/img/bt_limpa.png");

			help.Oper.TitleAdd("Cadastrando novo usuário em um padrão de acesso");
			help.Oper.Add("Para cadastrar novo usuário é necessário consultar os usuários do padrão de acesso de interesse. Ver 'Consultando usuários de um padrão de acesso'.");
			help.Oper.Add("Para cadastrar um novo usuário no padrão de acesso selecionado deve-se clicar no botão ?I.", "~/img/bt_novo.png");
			help.Oper.Add("Deve-se selecionar o usuário de interesse.");
			help.Oper.Add("Para salvar os dados do novo usuário no padrão de acesso selecionado deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a inclusão do novo usuário no padrão de acesso selecionado deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

			help.Oper.TitleAdd("Removendo usuário de um padrão de acesso");
			help.Oper.Add("Para remover um usuário é necessário consultar os usuários do padrão de acesso de interesse. Ver 'Consultando usuários de um padrão de acesso'.");
			help.Oper.Add("Para remover um usuário associado ao padrão de acesso selecionado deve-se clicar no botão ?I e confirmar a remoção do registro.", "~/img/bt_exclui2.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

			help.Oper.TitleAdd("Consultando relatórios de um padrão de acesso");
			help.Oper.Add("Para consultar relatórios é necessário selecionar o padrão de acesso de interesse em sua respectiva grade.");
			help.Oper.Add("Para consultar os relatórios associados ao padrão de acesso selecionado deve-se clicar no botão 'Relatórios'.");
			help.Oper.Add("Todos os relatórios associados ao padrão de acesso selecionado serão exibidos na grade de relatórios em uma nova página.");
			help.Oper.Add("Para filtrar a consulta utilize a linha de filtro acima dos registros. Também é possível definir o tipo de filtro clicando no botão ?I.", "~/img/bt_filtro.jpg");
			help.Oper.Add("Para limpar o filtro deve-se clicar no botão ?I.", "~/img/bt_limpa.png");

			help.Oper.TitleAdd("Cadastrando novo relatório em um padrão de acesso");
			help.Oper.Add("Para cadastrar novo relatório é necessário consultar os relatórios do padrão de acesso de interesse. Ver 'Consultando relatórios de um padrão de acesso'.");
			help.Oper.Add("Para cadastrar um novo relatório no padrão de acesso selecionado deve-se clicar no botão ?I.", "~/img/bt_novo.png");
			help.Oper.Add("Deve-se preencher os campos com os dados do novo relatório.");
			help.Oper.Add("Para salvar os dados do novo relatório no padrão de acesso selecionado deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a inclusão do novo relatório no padrão de acesso selecionado deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

			help.Oper.TitleAdd("Alterando relatório de um padrão de acesso");
			help.Oper.Add("Para alterar um relatório é necessário consultar os relatórios do padrão de acesso de interesse. Ver 'Consultando relatórios de um padrão de acesso'.");
			help.Oper.Add("Para alterar um relatório associado ao padrão de acesso selecionado deve-se clicar no botão ?I.", "~/img/bt_editar.png");
			help.Oper.Add("Os dados do relatório serão carregados permitindo alterações nos campos.");
			help.Oper.Add("Para salvar os dados do relatório deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a alteração nos dados do relatório deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

			help.Oper.TitleAdd("Removendo relatório de um padrão de acesso");
			help.Oper.Add("Para remover um relatório é necessário consultar os relatórios do padrão de acesso de interesse. Ver 'Consultando relatórios de um padrão de acesso'.");
			help.Oper.Add("Para remover um relatório associado ao padrão de acesso selecionado deve-se clicar no botão ?I e confirmar a remoção do registro.", "~/img/bt_exclui2.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

			help.Oper.TitleAdd("Descrição dos Campos");
			help.Oper.TitleAdd("• Padrões de Acesso");
			help.Oper.Add("• Padrão de Acesso: Código do padrão de acesso.");
			help.Oper.Add("• Nome: Nome descritivo do padrão de acesso.");

			help.Oper.TitleAdd("• Transações");
			help.Oper.Add("• Transação: Transação do sistema.");
			help.Oper.Add("• Alterar: Indica se o padrão de acesso possui permissão para alterar registros na transação.");
			help.Oper.Add("• Cadastrar: Indica se o padrão de acesso possui permissão para cadastrar registros na transação.");
			help.Oper.Add("• Remover: Indica se o padrão de acesso possui permissão para remover registros na transação.");

			help.Oper.TitleAdd("• Usuários");
			help.Oper.Add("• Usuário: Usuário pertencente ao padrão de acesso.");

			help.Oper.TitleAdd("• Relatórios");
			help.Oper.Add("• Padrão de Acesso: Código do padrão de acesso.");
			help.Oper.Add("• Grupo do Relatório: Grupo utilizado para classificar relatórios.");
			help.Oper.Add("• Relatório: Nome descritivo do relatório.");

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Curriculo
{
	public partial class PadraoAcessoTurmas
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Consultar, cadastrar, alterar e remover períodos de permissão para criar, alterar ou remover turmas e para manipular o quadro de horários. Cada período está associado a um padrão de acesso já existente.");
			help.Summary.Add("Obs.: Estando fora do período cadastrado na página de períodos de criação de turmas, a página de turmas permitirá manipulação das turmas somente para os usuários que pertençam aos padrões de acesso registrados nesta página.");
			help.Oper.Add("Para consultar, cadastrar, alterar ou remover um período é necessário fazer uma pesquisa pelo curso de interesse clicando no botão ?I.", "~/Images/bt_drop.png");
			help.Oper.Add("Todos os cursos existentes são apresentados em uma lista suspensa.");
			help.Oper.Add("A pesquisa pode ser filtrada pelo código e/ou descrição do curso. Após definidas estas informações, o resultado é apresentado automaticamente na lista suspensa.");
			help.Oper.Add("Deve-se selecionar o curso de interesse clicando na linha em que o curso aparece na lista suspensa.");
			help.Oper.Add("Obs.: Nesta pesquisa, utilize o caracter '%' para aplicar um filtro cujo valor está contido no campo pesquisado e não apenas quando estiver no início, conforme o exemplo abaixo:");
			help.Oper.Add("Exemplo: Para filtrar a coluna 'Descrição' para que só sejam exibidos registros contendo a palavra 'Ensino', digite %Ensino na coluna 'Descrição'.");

			help.Oper.TitleAdd("Consultando períodos de permissão de um curso");
			help.Oper.Add("A consulta é realizada automaticamente quando o curso de interesse for selecionado.");
			help.Oper.Add("Os dados dos períodos de permissão associados ao curso selecionado serão exibidos na grade de períodos de permissão.");
			help.Oper.Add("Para filtrar a consulta utilize a linha de filtro acima dos registros. Também é possível definir o tipo de filtro clicando no botão ?I.", "~/img/bt_filtro.jpg");
			help.Oper.Add("Para limpar o filtro deve-se clicar no botão ?I.", "~/img/bt_limpa.png");

			help.Oper.TitleAdd("Cadastrando período de permissão em um curso");
			help.Oper.Add("Para cadastrar um novo período de permissão é necessário fazer a consulta do curso desejado. Ver 'Consultando períodos de permissão de um curso.");
			help.Oper.Add("Para cadastrar um período de permissão associado ao curso selecionado deve-se clicar no botão ?I.", "~/img/bt_novo.png");
			help.Oper.Add("Deve-se preencher os campos com os dados do período de permissão.");
			help.Oper.Add("Para salvar os dados do novo período de permissão associado ao curso selecionado deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a inclusão dos dados do período de permissão deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

			help.Oper.TitleAdd("Alterando período de permissão de um curso");
			help.Oper.Add("Para alterar um novo período de permissão é necessário fazer a consulta do curso desejado. Ver 'Consultando períodos de permissão de um curso.");
			help.Oper.Add("Para alterar os dados de um período de permissão associado ao curso selecionado deve-se clicar no botão ?I.", "~/img/bt_editar.png");
			help.Oper.Add("Os dados do período de permissão serão carregados permitindo alterações nos campos.");
			help.Oper.Add("Para salvar os dados do período de permissão associado ao curso selecionado deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a alteração nos dados do período de permissão deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

			help.Oper.TitleAdd("Removendo período de permissão de um curso");
			help.Oper.Add("Para remover um período de permissão é necessário fazer a consulta do curso desejado. Ver 'Consultando períodos de permissão de um curso.");
			help.Oper.Add("Para remover um período de permissão associado ao curso selecionado deve-se clicar no botão ?I e confirmar a remoção do registro.", "~/img/bt_exclui2.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

			help.Oper.TitleAdd("Descrição dos Campos");
			help.Oper.Add("• Curso: Curso para o qual o período de permissão de manipulação de turmas foi informado.");
            help.Oper.Add("• Padrão de Acesso: Padrão de acesso que terá permissão para realizar a operação indicada no campo 'Operação' na página de turmas. (Tabela: Padrões de Acesso)");
			help.Oper.Add("• Operação: Operação permitida para os usuários do padrão de acesso informado. Exemplos:");
			help.Oper.Add("1 - Permite manipulação da turma: Esta operação permite o usuário inserir/alterar/remover uma turma. Seu acesso se restringe a aba Geral da página de turmas.");
			help.Oper.Add("2 - Permite manipulação do quadro de horários: Esta operação permite o usuário modificar o quadro de horários da turma. Seu acesso se restringe à aba 'Quadro de Horários' da página de turmas.");
			help.Oper.Add("• Data Inicial: Data inicial do período de permissão permitido para manipulação de turmas.");
			help.Oper.Add("• Data Final: Data final do período de permissão permitido para manipulação de turmas.");

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


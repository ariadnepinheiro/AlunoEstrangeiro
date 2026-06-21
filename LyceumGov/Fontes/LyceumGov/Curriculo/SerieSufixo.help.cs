using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Curriculo
{
	public partial class SerieSufixo
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Consultar, cadastrar, alterar e remover sufixos de anos de escolaridade.");
			help.Summary.Add("O sufixo de ano de escolaridade é usado pela transação Turmas para definição do código da turma.");

			help.Oper.TitleAdd("Informando dados de pesquisa");
			help.Oper.Add("Para consultar, alterar ou remover um sufixo é necessário selecionar unidade de ensino, escolaridade, turno, matriz curricular e ano de escolaridade obrigatoriamente em conjunto e nesta ordem.");
			help.Oper.Add("Ao clicar no botão ?I ao lado da grade de pesquisa de unidades de ensino, todas as unidades de ensino existentes são apresentadas em uma lista suspensa.", "~/Images/bt_drop.png");
			help.Oper.Add("A pesquisa pode ser filtrada pelo código, descrição, U.A., CNPJ ou situação da unidade de ensino. Após definidas estas informações, deve-se pressionar a tecla ENTER para filtrar os resultados.");
			help.Oper.Add("Deve-se selecionar a unidade de ensino de interesse clicando na linha em que a unidade aparece na lista suspensa.");
			help.Oper.Add("Ao clicar no botão ?I ao lado da grade de pesquisa de escolaridades, todas as escolaridades existentes são apresentadas em uma lista suspensa.", "~/Images/bt_drop.png");
			help.Oper.Add("A pesquisa pode ser filtrada pelo código e/ou descrição da escolaridade. Após definidas estas informações, o resultado é apresentado automaticamente na lista suspensa.");
			help.Oper.Add("Deve-se selecionar a escolaridade de interesse clicando na linha em que a escolaridade aparece na lista suspensa. Neste momento, a escolaridade selecionada é aplicada como filtro à grade de pesquisa de matrizes curriculares.");
			help.Oper.Add("Obs.: Nas pesquisas por unidade de ensino e escolaridade, utilize os caracteres '%' ou '*' para aplicar um filtro cujo valor está contido no campo pesquisado e não apenas quando estiver no início, conforme o exemplo abaixo:");
			help.Oper.Add("Exemplo: Para filtrar a coluna 'Descrição' da grade de pesquisa de unidades de ensino para que só sejam exibidos registros contendo a palavra 'José', digite %José ou *José na coluna 'Descrição'.");

			help.Oper.TitleAdd("Consultando sufixos de um ano de escolaridade");
			help.Oper.Add("A consulta é realizada automaticamente quando for selecionado o conjunto composto por unidade de ensino, escolaridade, matriz curricular e ano de escolaridade.");
			help.Oper.Add("Para filtrar a consulta utilize a linha de filtro acima dos registros. Também é possível definir o tipo de filtro clicando no botão ?I.", "~/img/bt_filtro.jpg");
			help.Oper.Add("Para limpar o filtro deve-se clicar no botão ?I.", "~/img/bt_limpa.png");

			help.Oper.TitleAdd("Cadastrando novo sufixo em um ano de escolaridade");
			help.Oper.Add("Para cadastrar um novo sufixo é necessário fazer a consulta do ano de escolaridade desejado. Ver 'Consultando sufixos de um ano de escolaridade'.");
			help.Oper.Add("Para cadastrar um novo sufixo no ano de escolaridade selecionado deve-se clicar no botão ?I.", "~/img/bt_novo.png");
			help.Oper.Add("Deve-se preencher os campos com os dados do novo sufixo.");
			help.Oper.Add("Para salvar os dados do novo sufixo no ano de escolaridade selecionado deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a inclusão dos dados do novo sufixo no ano de escolaridade selecionado deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

			help.Oper.TitleAdd("Alterando sufixo de um ano de escolaridade");
			help.Oper.Add("Para alterar um sufixo é necessário fazer a consulta do ano de escolaridade desejado. Ver 'Consultando sufixos de um ano de escolaridade'.");
			help.Oper.Add("Para alterar um sufixo do ano de escolaridade selecionado deve-se clicar no botão ?I.", "~/img/bt_editar.png");
			help.Oper.Add("Os dados do sufixo serão carregados permitindo alterações nos campos.");
			help.Oper.Add("Para salvar os dados do sufixo no ano de escolaridade selecionado deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a alteração nos dados do sufixo deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

			help.Oper.TitleAdd("Removendo sufixo de um ano de escolaridade");
			help.Oper.Add("Para remover um sufixo é necessário fazer a consulta do ano de escolaridade desejado. Ver 'Consultando sufixos de um ano de escolaridade'.");
			help.Oper.Add("Para remover um sufixo no ano de escolaridade selecionado deve-se clicar no botão ?I e confirmar a remoção do registro.", "~/img/bt_exclui2.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

			help.Oper.TitleAdd("Descrição dos Campos");
            help.Oper.TitleAdd("• Informações de Pesquisa");
            help.Oper.Add("• Escolaridade: Código de identificação e nome da escolaridade.");
            help.Oper.Add("• Turno: Turno referente à escolaridade. Os valores possíveis para este campo devem ser previamente informados na tela 'Turnos'. (Tabela: Turno, cujo o filtro é Escolaridade)");
            help.Oper.Add("• Matriz Curricular: Matriz curricular. (Tabela Matriz Curricular, cujos dados são filtrados por Escolaridade e Turno)");
            help.Oper.Add("• Ano de Escolaridade: Ano da escolaridade. (Tabela: Ano Letivo, cujos filtros são Escolaridade, Turno e Matriz Curricular)");   

			help.Oper.TitleAdd("• Grade: Sufixo do Ano de Escolaridade");
            help.Oper.Add("• Sufixo: Código do sufixo. Este código será usado pela transação Turmas para definição do código da turma.");
			help.Oper.Add("• Descrição: Descrição do sufixo.");

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
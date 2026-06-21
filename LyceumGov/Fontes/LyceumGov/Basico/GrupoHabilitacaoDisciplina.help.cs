using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Basico
{
	public partial class GrupoHabilitacaoDisciplina
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Consultar, cadastrar e remover disciplinas em grupos de habilitações.");

			help.Oper.Add("Para consultar, cadastrar ou remover uma disciplina é necessário fazer uma pesquisa pelo grupo de habilitações de interesse clicando no botão ?I.", "~/Images/bt_drop.png");
			help.Oper.Add("Todos os grupos de habilitações existentes são apresentados em uma lista suspensa.");
			help.Oper.Add("A pesquisa pode ser filtrada pelo nome e/ou descrição do grupo de habilitações. Após definidas estas informações, o resultado é apresentado automaticamente na lista suspensa.");
			help.Oper.Add("Deve-se selecionar o grupo de habilitações de interesse clicando na linha em que o grupo de habilitações aparece na lista suspensa.");
			help.Oper.Add("Obs.: Nesta pesquisa, utilize os caracteres '%' ou '*' para aplicar um filtro cujo valor está contido no campo pesquisado e não apenas quando estiver no início, conforme o exemplo abaixo:");
			help.Oper.Add("Exemplo: Para filtrar a coluna 'Descrição' para que só sejam exibidos registros contendo a palavra 'Biologia', digite %Biologia ou *Biologia na coluna 'Descrição'.");

			help.Oper.TitleAdd("Consultando disciplinas em um grupo de habilitações");
			help.Oper.Add("A consulta é realizada automaticamente quando o grupo de habilitações de interesse for selecionado.");
			help.Oper.Add("Os dados das disciplinas associadas ao grupo de habilitações selecionado serão exibidos na grade de disciplinas.");
			help.Oper.Add("Para filtrar a consulta utilize a linha de filtro acima dos registros. Também é possível definir o tipo de filtro clicando no botão ?I.", "~/img/bt_filtro.jpg");
			help.Oper.Add("Para limpar o filtro deve-se clicar no botão ?I.", "~/img/bt_limpa.png");
			
			help.Oper.TitleAdd("Cadastrando nova disciplina em um grupo de habilitações");
			help.Oper.Add("Para cadastrar uma nova disciplina é necessário fazer a consulta do grupo de habilitações desejado. Ver 'Consultando disciplinas em um grupo de habilitações'.");
			help.Oper.Add("Para cadastrar uma nova disciplina no grupo de habilitações desejado deve-se clicar no botão ?I.", "~/img/bt_novo.png");
			help.Oper.Add("Deve-se pesquisar a disciplina desejada clicando no botão ?I localizado na grade de pesquisa de disciplinas. Todas as disciplinas existentes são apresentadas em uma lista suspensa.");
			help.Oper.Add("Para facilitar a pesquisa, utilize os filtros de código e/ou nome da disciplina localizados na lista suspensa. Após definidos valores aos filtros, o resultado da pesquisa é apresentado nesta lista automaticamente.");
			help.Oper.Add("Obs.: Nesta pesquisa, utilize o caracter '%' para aplicar um filtro cujo valor está contido no campo pesquisado e não apenas quando estiver no início, conforme o exemplo abaixo:");
			help.Oper.Add("Exemplo: Para filtrar a coluna 'Disciplina' para que só sejam exibidos registros contendo a palavra 'Produção', digite %Produção na coluna 'Disciplina'.");
			help.Oper.Add("Deve-se selecionar a disciplina desejada clicando na linha em que a disciplina aparece na lista suspensa.");
			help.Oper.Add("Para salvar a inclusão da disciplina selecionada no grupo de habilitações deve-se clicar no botão ?I.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a inclusão da nova disciplina no grupo de habilitações deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a inclusão.");

			help.Oper.TitleAdd("Removendo disciplina em um grupo de habilitações");
			help.Oper.Add("Para remover uma disciplina é necessário fazer a consulta do grupo de habilitações desejado. Ver 'Consultando disciplinas em um grupo de habilitações'.");
			help.Oper.Add("Para remover uma disciplina associada ao grupo de habilitações desejado deve-se clicar no botão ?I e confirmar a remoção do registro.", "~/img/bt_exclui2.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

			help.Oper.TitleAdd("Descrição dos Campos");
			help.Oper.Add("• Código: Código da disciplina.");
			help.Oper.Add("• Disciplina: Nome da disciplina.");
			help.Oper.Add("• Grupo: Código mnemônico do grupo de habilitações.");
			help.Oper.Add("• Descrição: Descrição detalhada do grupo de habilitações.");
			
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Academico
{
	public partial class Ocorrencias
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Consultar, cadastrar, alterar, remover e visualizar detalhes das ocorrências de alunos.");

			help.Oper.Add("Para consultar, cadastrar, alterar, remover ou visualizar detalhes de uma ocorrência é necessário fazer uma pesquisa pelo aluno de interesse clicando no botão ?I.", "~/Images/bt_drop.png");
			help.Oper.Add("A pesquisa pode ser feita pelo nome, matrícula, nome da mãe e/ou nome do pai do aluno. Após definidas estas informações, deve-se clicar em ?I.", "~/Images/bot_buscar.png");
			help.Oper.Add("O resultado da pesquisa aparecerá em uma lista suspensa e deve-se selecionar o aluno de interesse clicando na linha em que o aluno aparece nesta lista.");
			help.Oper.Add("Obs.: Os dados escolares do aluno selecionado aparecem destacados na tela.");

			help.Oper.TitleAdd("Consultando ocorrências");
			help.Oper.Add("A consulta é realizada automaticamente quando o aluno de interesse for selecionado.");
			help.Oper.Add("Os dados das ocorrências associadas ao aluno selecionado serão exibidos na grade de ocorrências.");
			help.Oper.Add("Para filtrar a consulta utilize a linha de filtro acima dos registros. Também é possível definir o tipo de filtro clicando no botão ?I.", "~/img/bt_filtro.jpg");
			help.Oper.Add("Para limpar o filtro deve-se clicar no botão ?I.", "~/img/bt_limpa.png");

			help.Oper.TitleAdd("Cadastrando nova ocorrência");
			help.Oper.Add("Para cadastrar uma ocorrência é necessário fazer a consulta do aluno desejado. Ver 'Consultando ocorrências'.");
			help.Oper.Add("Para cadastrar uma ocorrência associada ao aluno selecionado deve-se clicar no botão ?I.", "~/img/bt_novo.png");
			help.Oper.Add("Deve-se preencher os campos com os dados da nova ocorrência.");
			help.Oper.Add("Para salvar os dados da nova ocorrência deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/Images/SmallOk.png");
			help.Oper.Add("Para cancelar a inclusão dos dados da ocorrência deve-se clicar no botão ?I.", "~/images/SmallCancel.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

			help.Oper.TitleAdd("Alterando ocorrência");
			help.Oper.Add("Para alterar uma ocorrência é necessário fazer a consulta do aluno desejado. Ver 'Consultando ocorrências'.");
			help.Oper.Add("Para alterar os dados de uma ocorrência associada ao aluno selecionado deve-se clicar no botão ?I.", "~/img/bt_editar.png");
			help.Oper.Add("Os dados da ocorrência serão carregados permitindo alterações nos campos.");
			help.Oper.Add("Para salvar os dados da ocorrência deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/Images/SmallOk.png");
			help.Oper.Add("Para cancelar a alteração nos dados da ocorrência deve-se clicar no botão ?I.", "~/images/SmallCancel.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");
			help.Oper.Add("Obs.: Todos os campos dos dados da ocorrência selecionada exibidos no modo de remoção ficam desativados, não permitindo sua alteração.");

			help.Oper.TitleAdd("Removendo ocorrência");
			help.Oper.Add("Para remover uma ocorrência é necessário fazer a consulta do aluno desejado. Ver 'Consultando ocorrências'.");
			help.Oper.Add("Para remover os dados de uma ocorrência associada ao aluno selecionado deve-se clicar no botão ?I.", "~/img/bt_exclui2.png");
			help.Oper.Add("Os dados da ocorrência são exibidos na tela e deve-se confirmar a remoção do registro clicando em ?I.", "~/images/SmallOk.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

			help.Oper.TitleAdd("Visualizando detalhes de uma ocorrência");
			help.Oper.Add("Para visualizar os detalhes de uma ocorrência é necessário fazer a consulta do aluno desejado. Ver 'Consultando ocorrências'.");
			help.Oper.Add("Para visualizar os detalhes de uma ocorrência associada ao aluno selecionado deve-se clicar no botão ?I.", "~/img/bt_busca.png");
			help.Oper.Add("Os detalhes da ocorrência selecionada serão exibidos na tela. Todos os campos dos dados exibidos ficam desativados, não permitindo sua alteração.");
			help.Oper.Add("Para retornar à grade das ocorrências associadas ao aluno selecionado deve-se clicar no botão ?I.", "~/images/SmallCancel.png");

			help.Oper.TitleAdd("Descrição dos Campos");
			help.Oper.Add("• Aluno: Aluno referente à ocorrência.");
			help.Oper.Add("• Usuário: Usuário que registrou a ocorrência.");
            help.Oper.Add("• Tipo: Tipo da ocorrência. (Tabela Geral: TIPO OCORRENCIA)");
			help.Oper.Add("• Data: Data da ocorrência.");
			help.Oper.Add("• Descrição: Breve descrição da ocorrência.");
			help.Oper.Add("• Ano Letivo: Ano letivo referente à ocorrência. (Tabela: Ano Letivo, cujo filtro é o ano da Matrícula do aluno)");
			help.Oper.Add("• Período Letivo: Período letivo referente à ocorrência. (Tabela: Período Letivo, Período Letivo da Matrícula cujo filtro é Ano Letivo)");
			help.Oper.Add("• Disciplina: Disciplina referente à ocorrência. (Tabela Disciplina, Disciplina da Matrícula cujo filtros são Ano Letivo e Período Letivo)");
			help.Oper.Add("• Turma: Turma do aluno. (Tabela: Matrícula, cujo filtros são Ano Letivo, Disciplina e Turma)");
			help.Oper.Add("• Docente: Nome do docente responsável pela turma.");

			help.Oper.TitleAdd("Botões");
			help.Oper.TitleAdd("Consulta de Ocorrências");
			help.Oper.Add("?I: Insere uma nova linha.", "~/img/bt_novo.png");
			help.Oper.Add("?I: Permite alteração na linha.", "~/img/bt_editar.png");
			help.Oper.Add("?I: Remove a linha.", "~/img/bt_exclui2.png");
			help.Oper.Add("?I: Limpa os filtros selecionados.", "~/img/bt_Limpa.png");
			help.Oper.Add("?I: Seleciona uma linha para exibição completa.", "~/img/bt_busca.png");

			help.Oper.TitleAdd("Modo de Cadastro/Alteração/Remoção/Visualização");
			help.Oper.Add("?I: Confirma a inserção/alteração/remoção do registro.", "~/Images/SmallOk.png");
			help.Oper.Add("?I: Cancela a inserção/alteração/remoção/visualização do novo registro.", "~/Images/SmallCancel.png");
			help.Oper.Add("Obs.: A visibilidade dos botões depende da operação em andamento.");
		}
	}
}

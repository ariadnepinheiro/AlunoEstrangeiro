using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Academico
{
	public partial class NotasTurma
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Consultar, cadastrar, alterar e remover notas dos alunos de uma turma por disciplina.");

			help.Oper.Add("Cadastro das notas de uma turma.");
			help.Oper.Add("O sistema permite um cadastro de notas por disciplina de uma turma.");
			help.Oper.Add("Obs.: Nesta página é possível verificar a média final do aluno, calculada de acordo com a fórmula informada previamente.");

			help.Oper.TitleAdd("Informando dados de pesquisa");
			help.Oper.Add("Para consultar, cadastrar ou remover notas é necessário selecionar primeiramente a turma de interesse.");
			help.Oper.Add("Para fazer uma pesquisa pela turma de interesse, é necessário selecionar ano, período e coordenadoria obrigatoriamente em conjunto e nesta ordem e clicar no botão ?I.", "~/Images/bot_buscar.png");
			help.Oper.Add("Todas as turmas com as informações definidas serão exibidas na grade de turmas.");
			help.Oper.Add("Obs.: É possível opcionalmente selecionar um município e/ou uma unidade de ensino para filtrar os registros apresentados na grade de turmas.");
            help.Oper.Add("Deve-se selecionar a turma de interesse clicando no botão ?I correspondente à linha em que a turma aparece na lista suspensa.", "~/img/bt_search.gif");
			help.Oper.Add("Obs.: Nas pesquisas por coordenadoria, município e unidade de ensino, utilize os caracteres '%' ou '*' para aplicar um filtro cujo valor está contido no campo pesquisado e não apenas quando estiver no início, conforme o exemplo abaixo:");
			help.Oper.Add("Exemplo: Para filtrar a coluna 'Descrição' para que só sejam exibidos registros contendo a palavra 'Fluminense', digite %Fluminense ou *Fluminense na coluna 'Descrição'.");

            help.Oper.TitleAdd("Consultando notas por disciplina de uma turma.");
            help.Oper.Add("Para consultar notas por disciplina de uma turma é necessário primeiramente entrar com a turma. Vide 'Informando dados de pesquisa').");
            help.Oper.Add("A partir da seleção de uma turma, abrirá uma nova página com os dados da turma, na qual será necessário informar a disciplina desejada juntamente com seu período letivo (opcional).");
            help.Oper.Add("Para fazer uma pesquisa da disciplina, deve-se clicar no botão ?I ao lado de sua grade de pesquisa.", "~/Images/bt_drop.png");
            help.Oper.Add("Após definidas estas informações, deve-se clicar em ?I.", "~/Images/bot_buscar.png");
            help.Oper.Add("Uma página com as notas dos alunos da turma selecionada será iniciada (para a disciplina escolhida).");

            help.Oper.TitleAdd("Cadastrando/Alterando/Removendo notas por disciplina de uma turma.");
            help.Oper.Add("Para cadastrar/alterar/remover notas por disciplina de uma turma é necessário primeiramente entrar com a turma. Vide 'Informando dados de pesquisa').");
            help.Oper.Add("Faça as alterações necessárias na grade de Notas e clique no botão ?I para salvar.", "~/Images/SmallOk.png");
            help.Oper.Add("Caso queira cancelar as alterações, clique no botão ?I.", "~/Images/EditButtonsReturn.png");

            help.Oper.TitleAdd("Descrição dos Campos");
            help.Oper.TitleAdd("• Área de Pesquisa (Turmas)");
            help.Oper.Add("• Ano: Ano da turma selecionada. (Tabela: Ano Letivo)");
            help.Oper.Add("• Período: Período da turma selecionada. (Tabela: Ano Letivo, cujo filtro é Ano");
            help.Oper.Add("• Coordenadoria: coordenadoria da turma selecionada.");
            help.Oper.Add("• Município: Município da turma selecionada.");
            help.Oper.Add("• Unidade de Ensino: Unidade de ensino da turma selecionada.");
            help.Oper.Add("• Turma: Turma selecionada.");

            help.Oper.TitleAdd("• Grade de Turmas");
            help.Oper.Add("• Ano: Ano da turma selecionada.");
            help.Oper.Add("• Período: Período da turma selecionada.");
            help.Oper.Add("• Escolaridade: Escolaridade da turma selecionada.");
            help.Oper.Add("• Turno: Turno da turma selecionada.");
            help.Oper.Add("• Ano de Escolaridade: Ano escolar da turma selecionada.");
            help.Oper.Add("• Turma: Turma selecionada.");
            help.Oper.Add("• Unidade de Ensino: Unidade de ensino da turma selecionada.");
            help.Oper.Add("• Capacidade: Quantidade máxima de alunos da turma.");
            help.Oper.Add("• Sala de Aula: Sala de aula alocada para a turma.");

            help.Oper.TitleAdd("• Dados da Turma");
            help.Oper.Add("• Unidade de Ensino: Unidade de ensino da turma selecionada.");
            help.Oper.Add("• Turma: Turma selecionada.");
            help.Oper.Add("• Ano: Ano da turma selecionada.");
            help.Oper.Add("• Período: Período da turma selecionada.");
            help.Oper.Add("• Escolaridade: Escolaridade da turma selecionada.");
            help.Oper.Add("• Turno: Turno da turma selecionada.");
            help.Oper.Add("• Matriz Curricular: Matriz curricular da turma selecionada.");
            help.Oper.Add("• Ano de Escolaridade: Ano escolar da turma selecionada.");

            help.Oper.TitleAdd("• Área de Pesquisa (Notas)");
            help.Oper.Add("• Disciplina: Disciplina para a qual se deseja consultar ou editar notas.");
            help.Oper.Add("• Período Letivo: Indica a que período o instrumento de avaliação está associada.");

            help.Oper.TitleAdd("• Grade de Notas");
            help.Oper.Add("• Nome: Nome do aluno.");
            help.Oper.Add("• N.º: Número de chamada do aluno.");
            help.Oper.Add("• Nome: Nome completo do aluno.");
            help.Oper.Add("• Foto: Foto do aluno.");
            help.Oper.Add("• Sit. Matrícula: Situação da matrícula do aluno. .");
            help.Oper.Add("• TPF_B1: Valor de TPF_B1.");
            help.Oper.Add("• TPF_B2: Valor de TPF_B2.");
            help.Oper.Add("• TPF_B3: Valor de TPF_B3.");
            help.Oper.Add("• TPF_B4: Valor de TPF_B4.");
            help.Oper.Add("• RF: Valor de RF.");

			help.Oper.TitleAdd("Botões");
            help.Oper.Add("?I: Detalhes do registro selecionado.", "~/img/bt_search.gif");
            help.Oper.Add("?I: Ver fotos.", "~/Images/bt_foto.png");
			help.Oper.Add("?I: Salva a inserção/alteração da grade de notas.", "~/Images/SmallOk.png");
            help.Oper.Add("?I: Voltar para a página anterior.", "~/Images/EditButtonsReturn.png");
            help.Oper.Add("Obs.: A visibilidade dos botões depende da operação em andamento.");
		}
	}
}

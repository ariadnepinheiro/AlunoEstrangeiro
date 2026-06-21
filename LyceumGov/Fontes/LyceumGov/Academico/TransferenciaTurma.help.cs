using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Academico
{
	public partial class TransferenciaTurma
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Transferir turmas.");
			help.Summary.Add("Realiza a transferência de um aluno para outra turma.");
			help.Summary.Add("A transferência será registrada no histórico de transferências de turmas.");
			help.Summary.Add("Se o aluno já possuir notas em instrumentos de avaliação, estas notas serão levadas para a turma destino caso os instrumentos de avaliação existam nesta turma.");
			help.Summary.Add("A freqüências diárias não serão levadas para a turma nova.");

			help.Oper.Add("Para realizar uma transferência é necessário fazer uma pesquisa pelo aluno de interesse clicando no botão ?I.", "~/Images/bt_drop.png");
			help.Oper.Add("A pesquisa pode ser feita pelo nome, matrícula, nome da mãe e/ou nome do pai do aluno. Após definidas estas informações, deve-se clicar em ?I.", "~/Images/bot_buscar.png");
			help.Oper.Add("O resultado da pesquisa aparecerá em uma lista suspensa e deve-se selecionar o aluno de interesse clicando na linha em que o aluno aparece nesta lista.");

			help.Oper.TitleAdd("Transferindo uma turma");
			help.Oper.Add("Os dados do aluno serão destacados na tela quando o aluno de interesse for selecionado.");
			help.Oper.Add("Os dados das matrículas regulares e avulsas do aluno selecionado serão apresentados em suas respectivas grades.");
			help.Oper.Add("Para transferir todas as matrículas regulares do aluno selecionado, deve-se preencher os campos curso, turno, currículo, ano de escolaridade, turma de destino e o motivo da transferência e clicar no botão 'Transferir'.");
            help.Oper.Add("Para transferir uma ou mais matrículas por disciplina do aluno selecionado, deve-se preencher os campos turma de destino para cada matrícula e o motivo da transferência e clicar no botão 'Transferir'.");
			help.Oper.Add("Se algum campo obrigatório não estiver preenchido será exibido um alerta.");
			help.Oper.Add("Para cancelar a transferência e voltar para a tela inicial de pesquisa deve-se clicar no botão ?I.", "~/images/SmallCancel.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a transferência.");

			help.Oper.TitleAdd("Descrição dos Campos");
			help.Oper.TitleAdd("• Dados do Aluno");
			help.Oper.Add("• Aluno: Aluno para o qual se deseja realizar a transferência de turma.");
			help.Oper.Add("• Unidade de Ensino: Unidade de ensino do aluno selecionado.");
			help.Oper.Add("• Unidade Física: Unidade física do aluno selecionado.");
			help.Oper.Add("• Situação: Situação acadêmica do aluno selecionado.");
			help.Oper.Add("• Escolaridade: Escolaridade do aluno selecionado.");
			help.Oper.Add("• Turno: Turno do aluno selecionado.");
			help.Oper.Add("• Ano Escolar: Ano escolar do aluno selecionado.");
			help.Oper.Add("• Turma Atual: Turma atual do aluno.");

			help.Oper.TitleAdd("• Matrículas Regulares");
			help.Oper.Add("• Disciplina: Disciplina da matrícula regular do aluno.");
			help.Oper.Add("• Turma: Turma da matrícula regular do aluno.");
			help.Oper.Add("• Ano: Ano da matrícula regular do aluno.");
			help.Oper.Add("• Período: Período da matrícula regular do aluno.");
			help.Oper.Add("• Situação: Situação da matrícula regular do aluno.");
			help.Oper.Add("• Turma Destino: Indica a turma para o aluno será transferido.");
			help.Oper.Add("• Obs.: Esta operação afetará a matrícula regular do aluno. Ela não afetará as disciplinas avulsas.");
            help.Oper.Add("• Turno: Turno da turma. (Tabela: Turno, cujos filtros são Unidade de Ensino e Escolaridade)");
            help.Oper.Add("• Currículo: Currículo da turma. (Tabela: Matriz Curricular, cujos filtros são Escolaridade e Turno)");
            help.Oper.Add("• Ano de Escolaridade: Ano da escolaridade da turma. (Tabela: Matriz Curricular, cujos filtros são Escolaridade, Turno e Currículo)");
            help.Oper.Add("• Turma de Destino: Turma de destino. (Tabela: Turmas, cujos filtros são Unidade de Ensino, Situação, Escolaridade, Ano de Ingresso, Período de Ingresso, Grade ID, Turno, Currículo e Ano de Escolaridade)");

			help.Oper.TitleAdd("• Matrículas Por Disciplina");
			help.Oper.Add("• Disciplina: Disciplina da matrícula avulsa do aluno.");
			help.Oper.Add("• Turma: Turma da matrícula avulsa do aluno.");
			help.Oper.Add("• Ano: Ano da matrícula avulsa do aluno.");
			help.Oper.Add("• Período: Período da matrícula avulsa do aluno.");
			help.Oper.Add("• Situação: Situação da matrícula avulsa do aluno.");
			help.Oper.Add("• Turma Destino: Indica a turma para o aluno será transferido.");
			help.Oper.Add("• Obs.: Esta operação afetará apenas a disciplina para qual a turma destino foi informada. Ela não afetará a turma da matrícula regular.");

			help.Oper.TitleAdd("• Dados para Transferência");
            help.Oper.Add("• Motivo: Motivo da transferência. (Tabela Fixa: LY_MOTIVO_TRANSF)");

			help.Oper.TitleAdd("Botões");
			help.Oper.Add("?I: Transfere o aluno para outra turma.", "~/images/bot_transferir.png");
			help.Oper.Add("?I: Cancela a operação e retorna para página inicial.", "~/images/SmallCancel.png");
			help.Oper.Add("Obs.: A visibilidade dos botões depende da operação em andamento.");
		}
	}
}

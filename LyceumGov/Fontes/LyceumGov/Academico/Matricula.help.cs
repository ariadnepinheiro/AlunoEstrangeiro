using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Academico
{
	public partial class Matricula
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Consultar, cadastrar e remover matrículas de alunos.");

			help.Oper.Add("Para consultar, cadastrar ou remover uma matrícula é necessário fazer uma pesquisa pela matrícula do aluno de interesse clicando no botão ?I.", "~/Images/bt_drop.png");
			help.Oper.Add("A pesquisa pode ser feita pelo nome, matrícula, nome da mãe e/ou nome do pai do aluno. Após definidas estas informações, deve-se clicar em ?I.", "~/Images/bot_buscar.png");
			help.Oper.Add("O resultado da pesquisa aparecerá em uma lista suspensa e deve-se selecionar o aluno de interesse clicando na linha em que o aluno aparece nesta lista.");
			help.Oper.Add("Obs.: Ao selecionar a matrícula do aluno de interesse, os dados escolares a ele relacionados serão destacados na página.");

			help.Oper.TitleAdd("Consultando matrícula");
			help.Oper.Add("A consulta é realizada automaticamente quando a matrícula do aluno de interesse for selecionada.");
			help.Oper.Add("Caso o aluno selecionado possua matrícula ativa em uma turma os dados das disciplinas a ela associadas serão exibidos na grade de disciplinas.");

			help.Oper.TitleAdd("Cadastrando matrícula");
			help.Oper.Add("Para cadastrar uma matrícula é necessário fazer a consulta da matrícula do aluno desejada. Ver 'Consultando matrícula'.");
			help.Oper.Add("Para cadastrar uma nova matrícula deve-se clicar no botão ?I.", "~/Images/SmallNew.png");
			help.Oper.Add("Deve-se preencher os campos com os dados da nova matrícula.");
			help.Oper.Add("Para salvar os dados da nova matrícula deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/Images/SmallOk.png");
			help.Oper.Add("Para cancelar a inclusão dos dados da matrícula deve-se clicar no botão ?I.", "~/Images/SmallCancel.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

			help.Oper.TitleAdd("Removendo matrícula");
			help.Oper.Add("Para remover uma matrícula é necessário fazer a consulta da matrícula do aluno desejada. Ver 'Consultando matrícula'.");
			help.Oper.Add("Para remover uma matrícula deve-se clicar no botão ?I e confirmar a remoção do registro.", "~/Images/SmallDelete.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

			help.Oper.TitleAdd("Descrição dos Campos");
			help.Oper.TitleAdd("• Dados Escolares");
			help.Oper.Add("• Escolaridade: Escolaridade no qual o aluno está inscrito.");
			help.Oper.Add("• Matriz Curricular: Matriz curricular em andamento da escolaridade.");
			help.Oper.Add("• Turno: Turno no qual o aluno está inscrito.");
			help.Oper.Add("• Ano Escolar: Ano escolar no qual o aluno está inscrito.");
			help.Oper.Add("• Unidade Física: Unidade física na qual o aluno está inscrito.");
			help.Oper.Add("• Coordenadoria: Coordenadoria na qual o aluno está inscrito.");
			help.Oper.Add("• Unidade de Ensino: Unidade de ensino responsável pela unidade física.");

			help.Oper.TitleAdd("• Ano - Período - Turma");
            help.Oper.Add("• Ano: Ano referente à matrícula. (Tabela: Ano Letivo)");
            help.Oper.Add("• Período: Período referente à matrícula. (Tabela: Ano Letivo, cujo filtro é Ano)");
			help.Oper.Add("• Turma: Turma referente à matrícula. (Tabela: Turmas, cujos filtros são Escolaridade, Matriz Curricular, Turno, Ano Escolar, Unidade Física, Coordenadoria, Unidade de Ensino, Ano e Período)");

			help.Oper.TitleAdd("• Disciplinas");
			help.Oper.Add("• Disciplina: Código da disciplina pertencente à turma.");
			help.Oper.Add("• Nome: Descrição da disciplina.");
			help.Oper.Add("• Situação: Situação do aluno na disciplina.");

			help.Oper.TitleAdd("Botões");
			help.Oper.Add("?I: Carrega um formulário para novo registro.", "~/Images/SmallNew.png");
			help.Oper.Add("?I: Salva as alterações do registro.", "~/Images/SmallOk.png");
            help.Oper.Add("?I: Atualiza a matrícula sem verificar vaga na turma", "~/images/bot_atualizar.png");
			help.Oper.Add("?I: Cancela a operação corrente e retorna para página inicial.", "~/Images/SmallCancel.png");
			help.Oper.Add("?I: Remove o registro.", "~/Images/SmallDelete.png");
			help.Oper.Add("Obs.: A visibilidade dos botões depende da operação em andamento.");
		}
	}
}

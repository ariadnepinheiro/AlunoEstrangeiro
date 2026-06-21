using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Academico
{
	public partial class MeusAlunos
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Consultar alunos.");

			help.Oper.Add("A consulta por aluno pode ser realizada de duas maneiras:");
			help.Oper.Add("1 - Pesquisa pelo aluno de interesse, clicando-se no botão ?I ao lado da grade de pesquisa de aluno.", "~/Images/bt_drop.png");
			help.Oper.Add("A pesquisa pode ser feita pelo nome, matrícula, nome da mãe e/ou nome do pai do aluno. Após definidas estas informações, deve-se clicar em ?I.", "~/Images/bot_buscar.png");
			help.Oper.Add("O resultado da pesquisa aparecerá em uma lista suspensa e deve-se selecionar o aluno de interesse clicando na linha em que o aluno aparece nesta lista.");
			help.Oper.Add("2 - Pesquisa por coordenadoria, unidade de ensino, escolaridade, turno, ano escolar e turma obrigatoriamente em conjunto.");
			help.Oper.Add("Após definidas estas informações, deve-se clicar no botão ?I.", "~/Images/bot_buscar.png");

			help.Oper.TitleAdd("Consultando alunos");
			help.Oper.Add("Após realizar uma das consultas anteriores, o resultado da pesquisa será exibido na grade de alunos.");
			help.Oper.Add("Para filtrar a consulta utilize a linha de filtro acima dos registros. Também é possível definir o tipo de filtro clicando no botão ?I.", "~/img/bt_filtro.jpg");
			help.Oper.Add("Para limpar o filtro deve-se clicar no botão ?I.", "~/img/bt_limpa.png");

			help.Oper.TitleAdd("Selecionando aluno");
			help.Oper.Add("Para selecionar um aluno é necessário fazer uma consulta por alunos. Ver 'Consultando alunos'.");
			help.Oper.Add("Para selecionar um aluno deve-se clicar no botão ?I.", "~/img/bt_copiar.png");
			help.Oper.Add("Em seguida, a página é redirecionada para a página 'Alunos' na qual são apresentados os dados do aluno selecionado.");

			help.Oper.TitleAdd("Descrição dos Campos");
			help.Oper.Add("• Matrícula: Matrícula do aluno.");
			help.Oper.Add("• Nome: Nome completo do aluno.");
			//help.Oper.Add("• Coordenadoria: Coordenadoria associada à unidade física pertencente ao aluno.");
			help.Oper.Add("• Cód. Escolaridade: Código de identificação da escolaridade.");
			help.Oper.Add("• Escolaridade: Escolaridade na qual o aluno está matriculado.");
			help.Oper.Add("• Cód. Turno: Código de identificação do turno.");
			help.Oper.Add("• Turno: Turno de atividade escolar. (Tabela: Turno)");
			help.Oper.Add("• Cód. Ano Escolar: Código de identificação do ano escolar. (Tabela: Matriz Curricular, cujos filtros são Escolaridade e Turno)");
			help.Oper.Add("• Ano Escolar: Ano escolar.");
			help.Oper.Add("• Cód. Unid. Ensino: Código de identificação da unidade de ensino.");
			help.Oper.Add("• Unidade Ensino: Unidade de ensino onde o aluno está matriculado.");

			help.Oper.TitleAdd("Botões");
			help.Oper.Add("?I: Seleciona a linha.", "~/img/bt_copiar.png");
			help.Oper.Add("?I: Limpa os filtros selecionados.", "~/img/bt_Limpa.png");
			help.Oper.Add("Obs.: A visibilidade dos botões depende da operação em andamento.");
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Curriculo
{
	public partial class ExclusaoAlocacaoDocente
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Consultar e remover alocações de um docente das suas turmas.");
			help.Summary.Add("A página listará todos os horários de um docente e permitirá a troca dos seus horários por uma carência real ou carência temporária.");
			help.Oper.Add("Para consultar ou remover uma alocação é necessário fazer uma pesquisa pelo docente de interesse clicando no botão ?I.", "~/Images/bt_drop.png");
			help.Oper.Add("A pesquisa pode ser feita pela matrícula, nome, RG, CPF e/ou número do funcionário. Após definidas estas informações, deve-se clicar em 'Buscar'. O resultado da pesquisa aparecerá em uma lista suspensa.");
			help.Oper.Add("Deve-se selecionar o docente de interesse clicando na linha em que o docente aparece na lista suspensa.");

			help.Oper.TitleAdd("Consultando alocações de um docente");
			help.Oper.Add("A consulta é realizada automaticamente quando o docente de interesse for selecionado.");
			help.Oper.Add("Os dados do docente selecionado serão destacados na página.");
			help.Oper.Add("Obs.: Os campos com as informações do docente selecionado não permitem alteração.");
			help.Oper.Add("Os dados das alocações associadas ao docente selecionado serão exibidos na grade de alocações.");

			help.Oper.TitleAdd("Removendo alocações de um docente");
			help.Oper.Add("Para remover uma alocação é necessário fazer a consulta do docente desejado. Ver 'Consultando alocações de um docente'.");
			help.Oper.Add("Para remover uma alocação associada ao docente selecionado deve-se clicar no botão ?I e confirmar a remoção do registro.", "~/img/bt_exclui2.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

			help.Oper.TitleAdd("Descrição dos Campos");
			help.Oper.Add("• CPF: CPF do docente selecionado.");
			help.Oper.Add("• Função: Função na qual o docente selecionado está lotado.");
			help.Oper.Add("• Disciplina de Ingresso: Disciplina de ingresso do docente selecionado.");
			help.Oper.Add("• Cargo: Cargo do docente selecionado.");
            help.Oper.Add("• Ano: Ano da alocação.");
            help.Oper.Add("• Período: período da alocação.");
			help.Oper.Add("• Coordenadoria: Coordenadoria onde o docente está alocado na turma.");
			help.Oper.Add("• Município: Município da unidade de ensino onde o docente está alocado na turma.");
			help.Oper.Add("• Unidade de Ensino: Unidade de ensino onde o docente está alocado na turma.");
			help.Oper.Add("• U.A.: U.A. da unidade de ensino onde o docente está alocado na turma.");
			help.Oper.Add("• Censo: Censo da unidade de ensino onde o docente está alocado na turma.");
			help.Oper.Add("• Turma: Turma onde o docente está alocado.");
			help.Oper.Add("• Turno: Turno da turma onde o docente está alocado.");
			help.Oper.Add("• Dia: Dia da semana em que o docente está alocado.");
			help.Oper.Add("• Hora de Entrada: Hora de início da aula em que o docente está alocado.");
			help.Oper.Add("• Hora de Saída: Hora de fim da aula em que o docente está alocado.");
			help.Oper.Add("• Disciplina: Disciplina em que o docente está alocado.");
			help.Oper.Add("• Matrícula: Matrícula do docente.");

			help.Oper.TitleAdd("Botões");
			help.Oper.Add("?I: Remove a linha.", "~/img/bt_exclui2.png");
			help.Oper.Add("?I: Limpa os filtros selecionados.", "~/img/bt_Limpa.png");
			help.Oper.Add("Obs.: A visibilidade dos botões depende da operação em andamento.");
		}
	}
}

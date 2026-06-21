using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Curriculo
{
	public partial class HorariosDocentePorTurma
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Consultar horários do docente por turma.");
			help.Oper.Add("A página é carregada somente quando o usuário solicitar a consulta dos horários de um docente para a turma selecionada na página de turmas.");
			help.Oper.Add("Os dados da turma selecionada serão destacados na página.");
			help.Oper.Add("Obs.: Os campos com as informações da turma selecionada não permitem alteração.");
			help.Oper.Add("Para consultar os horários é necessário selecionar o docente de interesse dentre os docentes da turma selecionada.");

			help.Oper.TitleAdd("Consultando horários de um docente por turma");
			help.Oper.Add("A consulta é realizada automaticamente quando o docente de interesse for selecionado.");
			help.Oper.Add("Os dados dos horários alocados ao docente selecionado serão exibidos na grade de horários.");

			help.Oper.TitleAdd("Descrição dos Campos");
			help.Oper.Add("• Matrícula: Matrícula do docente.");
			help.Oper.Add("• Nome: Nome completo do docente.");
			help.Oper.Add("• Disciplina: Disciplina em que o docente está alocado.");
			help.Oper.Add("• Dia da Aula: Dia da semana em que o docente está alocado.");
			help.Oper.Add("• Hora Inicial: Hora de início da aula em que o docente está alocado.");
			help.Oper.Add("• Hora Final: Hora de fim da aula em que o docente está alocado.");
			help.Oper.Add("• Data Inicial: Data de início da aula em que o docente está alocado.");
			help.Oper.Add("• Data Final: Data de fim da aula em que o docente está alocado.");

			help.Oper.TitleAdd("Botões");
			help.Oper.Add("?I: Limpa os filtros selecionados.", "~/img/bt_Limpa.png");
			help.Oper.Add("Obs.: A visibilidade dos botões depende da operação em andamento.");
		}
	}
}

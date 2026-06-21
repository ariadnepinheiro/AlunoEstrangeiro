using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Basico
{
	public partial class DocenteFuncaoGLP
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Consultar e cadastrar solicitações de GLP em uma unidade administrativa.");

			help.Oper.TitleAdd("Informando dados de pesquisa");
			help.Oper.Add("Para consultar ou cadastrar solicitações de GLP é necessário fazer uma pesquisa pela unidade administrativa de interesse clicando no botão ?I.", "~/Images/bt_drop.png");
			help.Oper.Add("Todas as unidades administrativas existentes são apresentadas em uma lista suspensa.");
			help.Oper.Add("A pesquisa pode ser filtrada pelo código e/ou nome da unidade administrativa. Após definidas estas informações, deve-se pressionar a tecla ENTER para filtrar os resultados.");
			help.Oper.Add("Deve-se selecionar a unidade administrativa de interesse clicando na linha em que a unidade aparece na lista suspensa.");
			help.Oper.Add("Obs.: Utilize os caracteres '%' ou '*' para aplicar um filtro cujo valor está contido no campo pesquisado e não apenas quando estiver no início, conforme o exemplo abaixo:");
			help.Oper.Add("Exemplo: Para filtrar a coluna 'Nome' da grade de pesquisa de unidade administrativa para que só sejam exibidos registros contendo a palavra 'José', digite %José ou *José na coluna 'Nome' e pressione a tecla ENTER.");

			help.Oper.TitleAdd("Consultando solicitações de GLP de uma unidade administrativa");
			help.Oper.Add("A consulta é realizada automaticamente quando a unidade administrativa de interesse for selecionada.");
			help.Oper.Add("Os dados das solicitações de GLP associadas à unidade administrativa selecionada serão exibidos na grade de solicitações de GLP.");

			help.Oper.TitleAdd("Cadastrando nova solicitação de GLP de uma unidade administrativa");
			help.Oper.Add("Para cadastrar uma nova solicitação de GLP é necessário fazer a consulta da unidade administrativa desejada. Ver 'Consultando solicitações de GLP de uma unidade administrativa'.");
			help.Oper.Add("Um formulário em branco é carregado para o preenchimento dos dados do novo registro.");
			help.Oper.Add("Deve-se preencher os campos com os dados da nova solicitação de GLP.");
			help.Oper.Add("Para salvar os dados da nova solicitação de GLP deve-se clicar no botão 'Incluir Solicitação'. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/Images/SmallOk.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

			help.Oper.TitleAdd("Descrição dos Campos");
			help.Oper.TitleAdd("• Área de Pesquisa");
			help.Oper.Add("• Unidade Administrativa: Unidade administrativa para a qual foram realizadas solicitações de GLP.");

			help.Oper.TitleAdd("• Formulário de Cadastro");
			help.Oper.Add("• Docente: Docente para o qual foi solicitada GLP.");
			help.Oper.Add("• Unidade de Ensino: Unidade de ensino referente à unidade administrativa selecionada para a qual será realizada solicitação de GLP.");
			help.Oper.Add("• Segmento de Atuação: Segmento de atuação da GLP solicitada.");
            help.Oper.Add("• Disciplina: Disciplina da GLP solicitada. (Tabela: Disciplina, cujo filtro é Docente)");
			help.Oper.Add("• CH Solicitada: Carga horária solicitada de aulas para GLP.");
            help.Oper.Add("• Validade da alocação: Dias que o docente ficará alocado em aulas, contando a partir da data de aceite da solicitação, após este prazo as aulas alocadas voltarão para carência temporária.");
			help.Oper.Add("• Obs.: GLP é Gratificação por Lotação Prioritária e indica em qual função o docente pode fazer horas extras.");

			help.Oper.TitleAdd("• Grade de Solicitações de GLP");
			help.Oper.Add("• Unidade de Ensino: Unidade de ensino para a qual será realizada solicitação de GLP.");
			help.Oper.Add("• Matrícula: Matrícula do docente para o qual foi solicitada GLP.");
			help.Oper.Add("• Nome: Nome do docente que receberá solicitação de GLP.");
			help.Oper.Add("• Disciplina: Código da disciplina da GLP solicitada.");
			help.Oper.Add("• Descrição: Descrição da disciplina da GLP solicitada.");
			help.Oper.Add("• Segmento de Atuação: Segmento de atuação da GLP solicitada.");
			help.Oper.Add("• GLP Solicitada: Quantidade de GLP solicitada para o docente.");
			help.Oper.Add("• GLP Usada: Quantidade de GLP usada para o docente.");
			help.Oper.Add("• GLP Cancelada: Quantidade de GLP cancelada para o docente.");
			help.Oper.Add("• Situação: Situação da solicitação de GLP.");
			help.Oper.Add("• Data: Data na qual foi registrada a solicitação de GLP.");

			help.Oper.TitleAdd("Botões");
			help.Oper.Add("?I: Insere uma nova linha.", "~/img/bt_novo.png");
			help.Oper.Add("?I: Limpa os filtros selecionados.", "~/img/bt_Limpa.png");
			help.Oper.Add("Obs.: A visibilidade dos botões depende da operação em andamento.");
		}
	}
}

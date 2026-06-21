using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Basico
{
	public partial class AnaliseGLP
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Consultar, aceitar e reprovar pedidos de GLP em uma coordenadoria.");

			help.Oper.TitleAdd("Informando dados de pesquisa");
			help.Oper.Add("Para consultar, aceitar ou reprovar pedidos de GLP é necessário fazer uma pesquisa pela coordenadoria de interesse clicando no botão ?I.", "~/Images/bt_drop.png");
			help.Oper.Add("Obs.: Opcionalmente, é possível filtrar os pedidos de GLP selecionando uma unidade de ensino na área de pesquisa.");
			help.Oper.Add("Todas as coordenadorias existentes são apresentadas em uma lista suspensa.");
			help.Oper.Add("A pesquisa pode ser filtrada pelo código e/ou nome da coordenadoria. Após definidas estas informações, deve-se pressionar a tecla ENTER para filtrar os resultados.");
			help.Oper.Add("Deve-se selecionar a coordenadoria de interesse clicando na linha em que a coordenadoria aparece na lista suspensa.");
			help.Oper.Add("Obs.: Utilize os caracteres '%' ou '*' para aplicar um filtro cujo valor está contido no campo pesquisado e não apenas quando estiver no início, conforme o exemplo abaixo:");
			help.Oper.Add("Exemplo: Para filtrar a coluna 'Nome' da grade de pesquisa de coordenadoria para que só sejam exibidos registros contendo a palavra 'José', digite %José ou *José na coluna 'Nome' e pressione a tecla ENTER.");

			help.Oper.TitleAdd("Consultando pedidos de GLP de uma coordenadoria");
			help.Oper.Add("A consulta é realizada automaticamente quando a coordenadoria de interesse for selecionada.");
			help.Oper.Add("Os dados dos pedidos de GLP associados à coordenadoria selecionada serão exibidos na grade de análise de pedidos de GLP.");

			help.Oper.TitleAdd("Aceitando pedido de GLP de uma coordenadoria");
			help.Oper.Add("Para aceitar um pedido de GLP é necessário fazer a consulta da coordenadoria desejada. Ver 'Consultando pedidos de GLP de uma coordenadoria'.");
			help.Oper.Add("Para aceitar um pedido de GLP da coordenadoria selecionada deve-se clicar no botão 'Aceitar'.");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o processo de aceitação do pedido.");

			help.Oper.TitleAdd("Recusando pedido de GLP de uma coordenadoria");
			help.Oper.Add("Para recusar um pedido de GLP é necessário fazer a consulta da coordenadoria desejada. Ver 'Consultando pedidos de GLP de uma coordenadoria'.");
			help.Oper.Add("Para recusar um pedido de GLP da coordenadoria selecionada deve-se clicar no botão 'Recusar'.");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o processo de aceitação do pedido.");

			help.Oper.TitleAdd("Descrição dos Campos");
			help.Oper.TitleAdd("• Área de Pesquisa");
			help.Oper.Add("• Coordenadoria: Coordenadoria de referência para a análise dos pedidos de GLP.");
			help.Oper.Add("• Unidade de Ensino: Unidade de ensino de referência para a análise dos pedidos de GLP.");

			help.Oper.TitleAdd("• Grade de Análise de Pedidos de GLP");
			help.Oper.Add("• Unidade de Ensino: Unidade de ensino para a qual foi realizado o pedido de GLP.");
			help.Oper.Add("• Matrícula: Matrícula do docente para o qual foi solicitada GLP.");
			help.Oper.Add("• Nome: Nome do docente que recebeu pedido de GLP.");
			help.Oper.Add("• Descrição: Descrição da disciplina do pedido de GLP.");
			help.Oper.Add("• CH Solicitada: Carga horária do pedido de GLP.");
			help.Oper.Add("• Situação: Situação do pedido de GLP.");
            help.Oper.Add("• CH Livre: Número de tempos livres de docentes lotados na unidade de ensino do pedido, cuja disciplina de ingresso é a disciplina do pedido.");
		}
	}
}

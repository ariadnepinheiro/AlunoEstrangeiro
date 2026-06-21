using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Basico
{
	public partial class ExclusaoGLP
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Consultar e excluir pedidos aceitos de GLP em uma unidade de ensino.");

			help.Oper.TitleAdd("Informando dados de pesquisa");
			help.Oper.Add("Para consultar e excluir pedidos aceitos de GLP é necessário selecionar uma coordenadoria e uma unidade de ensino.");
			help.Oper.Add("Ao clicar no botão ?I ao lado da grade de pesquisa de coordenadorias, todas as coordenadorias existentes são apresentadas em uma lista suspensa.", "~/Images/bt_drop.png");
			help.Oper.Add("A pesquisa pode ser filtrada pelo código e/ou nome da coordenadoria. Após definidas estas informações, deve-se pressionar a tecla ENTER para filtrar os resultados.");
			help.Oper.Add("Deve-se selecionar a coordenadoria de interesse clicando na linha em que a coordenadoria aparece na lista suspensa.");
			help.Oper.Add("Ao clicar no botão ?I ao lado da grade de pesquisa de unidades de ensino, todas as unidades de ensino existentes são apresentadas em uma lista suspensa.", "~/Images/bt_drop.png");
			help.Oper.Add("A pesquisa pode ser filtrada pelo código, descrição, U.A., CNPJ ou situação da unidade de ensino. Após definidas estas informações, deve-se pressionar a tecla ENTER para filtrar os resultados.");
			help.Oper.Add("Deve-se selecionar a unidade de ensino de interesse clicando na linha em que a unidade aparece na lista suspensa.");
			help.Oper.Add("Obs.: Nas grades de pesquisa de coordenadoria e unidade de ensino, utilize os caracteres '%' ou '*' para aplicar um filtro cujo valor está contido no campo pesquisado e não apenas quando estiver no início, conforme o exemplo abaixo:");
			help.Oper.Add("Exemplo: Para filtrar a coluna 'Nome' da grade de pesquisa de coordenadoria para que só sejam exibidos registros contendo a palavra 'José', digite %José ou *José na coluna 'Nome' e pressione a tecla ENTER.");

			help.Oper.TitleAdd("Consultando pedidos aceitos de GLP de uma unidade de ensino");
			help.Oper.Add("A consulta é realizada automaticamente quando a coordenadoria e a unidade de ensino de interesse forem selecionadas.");
			help.Oper.Add("Os dados dos pedidos aceitos de GLP associados à coordenadoria e à unidade de ensino selecionadas serão exibidos na grade de pedidos aceitos de GLP.");

			help.Oper.TitleAdd("Excluindo pedidos aceitos de GLP de uma unidade de ensino");
			help.Oper.Add("Para excluir um pedido aceito de GLP é necessário fazer a consulta da unidade de ensino desejada. Ver 'Consultando pedidos aceitos de GLP de uma coordenadoria'.");
			help.Oper.Add("Para excluir um pedido aceito de GLP associado à unidade de ensino selecionada deve-se clicar no botão 'Excluir' e confirmar a exclusão do registro.");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a exclusão.");

			help.Oper.TitleAdd("Descrição dos Campos");
			help.Oper.TitleAdd("• Área de Pesquisa");
			help.Oper.Add("• Coordenadoria: Coordenadoria de referência para a consulta dos pedidos aceitos de GLP.");
			help.Oper.Add("• Unidade de Ensino: Unidade de ensino de referência para a consulta dos pedidos aceitos de GLP.");

			help.Oper.TitleAdd("• Grade de Pedidos Aceitos de GLP");
			help.Oper.Add("• Unidade de Ensino: Unidade de ensino para a qual foi realizado o pedido de GLP.");
			help.Oper.Add("• Matrícula: Matrícula do docente que recebeu o pedido de GLP.");
			help.Oper.Add("• Nome: Nome do docente que recebeu o pedido de GLP.");
			help.Oper.Add("• Descrição: Descrição da disciplina da GLP solicitada.");
			help.Oper.Add("• CH Solicitada: Carga horária máxima solicitada pelo pedido de GLP.");
			help.Oper.Add("• Situação: Situação do pedido de GLP.");
			help.Oper.Add("• Data: Data na qual foi registrado o pedido de GLP.");
			help.Oper.Add("• Mês: Mês no qual foi registrado o pedido de GLP.");
			help.Oper.Add("• Ano: Ano no qual foi registrado o pedido de GLP.");
		}
	}
}

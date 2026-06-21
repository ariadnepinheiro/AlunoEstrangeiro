using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Curriculo
{
	public partial class ValorGLP
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
            help.Summary.Add("Consultar e alterar os valores GLP.");

            help.Oper.TitleAdd("Consultando valores GLP");
            help.Oper.Add("Ao acessar a página de valores GLP, será exibido a opção para a escolha do ano desejado.");
            help.Oper.Add("Após o ano ser escolhido, os valores da GLP do ano selecionado serão listados.");

			help.Oper.TitleAdd("Alterando valor GLP");
			help.Oper.Add("Para alterar os dados de um valor GLP deve-se clicar no botão ?I na linha da função desejada.", 
                "~/img/bt_editar.png");
			help.Oper.Add("Os dados do valor GLP serão carregados permitindo alterações nos campos.");
            help.Oper.Add("Os meses inferiores ao mês atual não poderão ter seus valores alterados.");
			help.Oper.Add("Para salvar os dados do valor GLP deve-se clicar no botão ?I. Se algum campo obrigatório " +
                "não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a alteração nos dados do valor GLP deve-se clicar no botão ?I.", 
                "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro " +
                "informando os erros ocorridos durante a alteração.");

			help.Oper.TitleAdd("Descrição dos Campos");
            help.Oper.TitleAdd("• Área de Pesquisa");
            help.Oper.Add("• Ano: Ano de referência para a consulta do valor da GLP. (Tabela: Ano Letivo)");
            
            help.Oper.TitleAdd("• Grade de Valores GLP");
			help.Oper.Add("• Função: Relação das funções que são regentes.");
			help.Oper.Add("• Meses: Todos os meses são listados com os valores da GLP.");

			help.Oper.TitleAdd("Botões");
			help.Oper.Add("?I: Salva a alteração da linha.", "~/img/bt_salvar.png");
			help.Oper.Add("?I: Cancela a alteração da linha.", "~/img/bt_cancelar.png");
			help.Oper.Add("?I: Permite alteração na linha.", "~/img/bt_editar.png");
			help.Oper.Add("Obs.: A visibilidade dos botões depende da operação em andamento.");
		}
	}
}

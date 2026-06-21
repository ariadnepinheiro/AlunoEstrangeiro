using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Curriculo
{
	public partial class PadraoAcessoLicencas
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Consultar, cadastrar, alterar e remover licenças de padrões de acesso.");

			help.Oper.Add("Esta transação permite o cadastro das licenças que podem ser manipuladas por um padrão de acesso.");
			help.Oper.Add("Este cadastro será utilizado na página de lotação/situação dos servidores e funcionários.");

			help.Oper.TitleAdd("Consultando licenças de um padrão de acesso");
			help.Oper.Add("A consulta é realizada automaticamente quando um padrão de acesso for selecionado.");
			help.Oper.Add("Os dados das licenças associadas ao padrão de acesso selecionado serão exibidos na grade de licenças.");

			help.Oper.TitleAdd("Cadastrando nova licença em um padrão de acesso");
			help.Oper.Add("Para cadastrar uma nova licença é necessário fazer a consulta do padrão de acesso desejado. Ver 'Consultando licenças de um padrão de acesso'.");
			help.Oper.Add("Para cadastrar uma nova licença no licenças de um padrão de acesso selecionado deve-se clicar no botão ?I.", "~/img/bt_novo.png");
			help.Oper.Add("Deve-se selecionar a licença de interesse.");
			help.Oper.Add("Para salvar os dados da nova licença no padrão de acesso selecionado deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a inclusão da nova licença no padrão de acesso selecionado deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

			help.Oper.TitleAdd("Alterando licença de um padrão de acesso");
			help.Oper.Add("Para alterar uma licença é necessário fazer a consulta do padrão de acesso desejado. Ver 'Consultando licenças de um padrão de acesso'.");
			help.Oper.Add("Para alterar uma licença do padrão de acesso selecionado deve-se clicar no botão ?I.", "~/img/bt_editar.png");
			help.Oper.Add("Os dados da licença serão carregados permitindo alterações nos campos.");
			help.Oper.Add("Para salvar os dados da licença deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a alteração nos dados da licença deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

			help.Oper.TitleAdd("Removendo licença de um padrão de acesso");
			help.Oper.Add("Para remover uma licença é necessário fazer a consulta do padrão de acesso desejado. Ver 'Consultando licenças de um padrão de acesso'.");
			help.Oper.Add("Para remover uma licença do padrão de acesso selecionado deve-se clicar no botão ?I e confirmar a remoção do registro.", "~/img/bt_exclui2.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

			help.Oper.TitleAdd("Descrição dos Campos");
			help.Oper.TitleAdd("• Área de Pesquisa");
			help.Oper.Add("• Padrão de Acesso: Padrão de acesso para licenças.");

			help.Oper.TitleAdd("• Grade de Licenças");
			help.Oper.Add("• Licença: Licença que poderá ser manipulada pelo padrão de acesso. (Tabela: Licenças)");

			help.Oper.TitleAdd("Botões");
			help.Oper.Add("?I: Insere uma nova linha.", "~/img/bt_novo.png");
			help.Oper.Add("?I: Salva a inserção/alteração da linha.", "~/img/bt_salvar.png");
			help.Oper.Add("?I: Cancela a inserção/alteração da nova linha.", "~/img/bt_cancelar.png");
			help.Oper.Add("?I: Permite alteração na linha.", "~/img/bt_editar.png");
			help.Oper.Add("?I: Remove a linha.", "~/img/bt_exclui2.png");
			help.Oper.Add("?I: Limpa os filtros selecionados.", "~/img/bt_Limpa.png");
			help.Oper.Add("Obs.: A visibilidade dos botões depende da operação em andamento.");
		}
	}
}

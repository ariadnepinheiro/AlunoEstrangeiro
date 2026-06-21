using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Hades
{
	public partial class TabelaGeral
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Consultar, cadastrar, alterar e remover tabelas e seus itens.");
			help.Oper.Add("Permite a manutenção das tabelas gerais do sistema.");
			help.Oper.Add("Estas tabelas são utilizadas quando há necessidade de padronizar o preenchimento de certos campos das transações.");
			help.Oper.Add("Nesta página, são cadastrados a tabela, que representa o campo que se deseja padronizar, e os itens associados a ela, que representam os valores possíveis para preenchimento.");
			help.Oper.Add("Exemplos típicos de tabelas de uso geral do sistema: Estado Civil, Unidades da Federação e Tipo de Documento (RG, CPF etc.).");
			
			help.Oper.TitleAdd("Consultando tabelas e seus itens");
			help.Oper.Add("Ao acessar a página de tabelas, todas as tabelas cadastradas serão exibidas na grade de tabelas e o primeiro registro é automaticamente selecionado de forma a exibir todos os itens associados a ele na grade de itens.");
			help.Oper.Add("Para consultar os itens associados a outra tabela deve-se selecionar a tabela desejada clicando na linha em que a tabela aparece na grade de tabelas.");
			help.Oper.Add("Os dados dos itens da tabela selecionada serão exibidos automaticamente na grade de itens.");
			help.Oper.Add("Para filtrar a consulta utilize a linha de filtro acima dos registros. Também é possível definir o tipo de filtro clicando no botão ?I.", "~/img/bt_filtro.jpg");
			help.Oper.Add("Para limpar o filtro deve-se clicar no botão ?I.", "~/img/bt_limpa.png");

			help.Oper.TitleAdd("Cadastrando nova tabela"); 
			help.Oper.Add("Para cadastrar uma nova tabela deve-se clicar no botão ?I da grade de tabelas.", "~/img/bt_novo.png");
			help.Oper.Add("Deve-se preencher os campos com os dados da nova tabela.");
			help.Oper.Add("Para salvar os dados da nova tabela deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a inclusão dos dados da tabela deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

			help.Oper.TitleAdd("Alterando tabela");
			help.Oper.Add("Para alterar os dados de uma tabela deve-se clicar no botão ?I da grade de tabelas.", "~/img/bt_editar.png");
			help.Oper.Add("Os dados da tabela serão carregados permitindo alterações nos campos.");
			help.Oper.Add("Para salvar os dados da tabela deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a alteração nos dados da tabela deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

			help.Oper.TitleAdd("Removendo tabela");
			help.Oper.Add("Para remover uma tabela deve-se clicar no botão ?I da grade de tabelas e confirmar a remoção do registro.", "~/img/bt_exclui2.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

			help.Oper.TitleAdd("Cadastrando novo item em uma tabela");
			help.Oper.Add("Para cadastrar um novo item deve-se selecionar a tabela desejada. Ver 'Consultando tabelas e seus itens'.");
			help.Oper.Add("Para cadastrar um item associado à tabela selecionada deve-se clicar no botão ?I da grade de itens.", "~/img/bt_novo.png");
			help.Oper.Add("Deve-se preencher os campos com os dados do novo item.");
			help.Oper.Add("Para salvar os dados do novo item associado à tabela selecionada deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a inclusão dos dados do novo item na tabela selecionada deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

			help.Oper.TitleAdd("Alterando item de uma tabela");
			help.Oper.Add("Para alterar um item deve-se selecionar a tabela desejada. Ver 'Consultando tabelas e seus itens'.");
			help.Oper.Add("Para alterar os dados de um item associado à tabela selecionada deve-se clicar no botão ?I da grade de itens.", "~/img/bt_editar.png");
			help.Oper.Add("Os dados do item serão carregados permitindo alterações nos campos.");
			help.Oper.Add("Para salvar os dados do item associado à tabela selecionada deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a alteração nos dados do item associado à tabela selecionada deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

			help.Oper.TitleAdd("Removendo item de uma tabela");
			help.Oper.Add("Para remover um item deve-se selecionar a tabela desejada. Ver 'Consultando tabelas e seus itens'.");
			help.Oper.Add("Para remover os dados de um item associado à tabela selecionada deve-se clicar no botão ?I da grade de itens e confirmar a remoção do registro.", "~/img/bt_exclui2.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

			help.Oper.TitleAdd("Descrição dos Campos");
			help.Oper.TitleAdd("Grade de Tabelas");
			help.Oper.Add("• Tabela: Representa o campo que terá valores padronizados.");
			help.Oper.Add("• Descrição: Descrição detalhada do campo.");

			help.Oper.TitleAdd("Grade de Itens");
			help.Oper.Add("• Item: Item que assume valores da tabela.");
			help.Oper.Add("• Descrição: Descrição detalhada do item.");

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
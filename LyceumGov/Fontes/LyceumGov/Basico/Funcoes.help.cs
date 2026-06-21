using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Basico
{
	public partial class Funcoes
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Consultar, cadastrar, alterar e remover funções desempenhadas por pessoas na instituição.");

			help.Oper.TitleAdd("Consultando funções");
			help.Oper.Add("Ao entrar na página de funções, todas as funções cadastradas serão exibidas.");
			help.Oper.Add("Para filtrar a consulta utilize a linha de filtro acima dos registros. Também é possível definir o tipo de filtro clicando no botão ?I.", "~/img/bt_filtro.jpg");
			help.Oper.Add("Para limpar o filtro deve-se clicar no botão ?I.", "~/img/bt_limpa.png");

			help.Oper.TitleAdd("Cadastrando nova função");
			help.Oper.Add("Para cadastrar uma função deve-se clicar no botão ?I.", "~/img/bt_novo.png");
			help.Oper.Add("Deve-se preencher os campos com os dados da nova função.");
			help.Oper.Add("Para salvar os dados da nova função deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a inclusão dos dados da função deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

			help.Oper.TitleAdd("Alterando função");
			help.Oper.Add("Para alterar os dados de uma função deve-se clicar no botão ?I.", "~/img/bt_editar.png");
			help.Oper.Add("Os dados da função serão carregados permitindo alterações nos campos.");
			help.Oper.Add("Para salvar os dados da função deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a alteração nos dados da função deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

			help.Oper.TitleAdd("Removendo função");
			help.Oper.Add("Para remover uma função deve-se clicar no botão ?I e confirmar a remoção do registro.", "~/img/bt_exclui2.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

			help.Oper.TitleAdd("Descrição dos Campos");
			help.Oper.Add("• Descrição: Descrição detalhada da função.");
            help.Oper.Add("• Tipo: Tipo da função. Os possíveis valores para este campo devem ser previamente informados na Tabela Geral. (Tabela Geral: Tipo Função)");
			help.Oper.Add("• Regente: Indica se a função é ou não regente.");
			help.Oper.Add("• Função Extra-Classe: Indica se a função é ou não extra-classe.");
            help.Oper.Add("• Libera GLP na 2ª Matrícula: Indica se a função está liberada para fazer GLP quando é de 2ª Matrícula.");
            help.Oper.Add("• Diretor: Indica se a função é ou não diretor.");
            help.Oper.Add("• Secretário: Indica se a função é ou não secretário.");
            help.Oper.Add("• Desaloca aulas: Indica se a função deve ou não desalocar aulas alocadas.");
            help.Oper.Add("• Permite GLP: Indica se a função permite ou não GLP.");

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


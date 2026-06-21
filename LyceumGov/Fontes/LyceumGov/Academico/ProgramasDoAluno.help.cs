using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Academico
{
	public partial class ProgramasDoAluno
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Consultar, cadastrar, alterar e remover programas sociais dos alunos.");

			help.Oper.TitleAdd("Informando dados de pesquisa");
			help.Oper.Add("Para consultar, alterar ou remover um programa social é necessário fazer uma pesquisa pelo aluno de interesse clicando no botão ?I.", "~/Images/bt_drop.png");
			help.Oper.Add("A pesquisa pode ser feita pelo nome, matrícula, nome da mãe e/ou nome do pai do aluno. Após definidas estas informações, deve-se clicar em ?I.", "~/Images/bot_buscar.png");
			help.Oper.Add("O resultado da pesquisa aparecerá em uma lista suspensa e deve-se selecionar o aluno de interesse clicando na linha em que o aluno aparece nesta lista.");

			help.Oper.TitleAdd("Consultando programas sociais de um aluno");
			help.Oper.Add("A consulta é realizada automaticamente quando o aluno de interesse for selecionado.");
			help.Oper.Add("Os dados dos programas sociais associados ao aluno selecionado serão exibidos na grade de programas sociais.");

			help.Oper.TitleAdd("Cadastrando novo programa social de um aluno");
			help.Oper.Add("Para cadastrar um novo programa social é necessário fazer a consulta do aluno desejado. Ver 'Consultando programas sociais de um aluno'.");
			help.Oper.Add("Para cadastrar um novo programa social associado ao aluno selecionado deve-se clicar no botão ?I.", "~/img/bt_novo.png");
			help.Oper.Add("Deve-se preencher os campos com os dados do novo programa social.");
			help.Oper.Add("Para salvar os dados do novo programa social associado ao aluno selecionado deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a inclusão dos dados do novo programa social associado ao aluno selecionado deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

			help.Oper.TitleAdd("Alterando programa social de um aluno");
			help.Oper.Add("Para alterar um programa social é necessário fazer a consulta do aluno desejado. Ver 'Consultando programas sociais de um aluno'.");
			help.Oper.Add("Para alterar os dados de um programa social associado à unidade de ensino selecionada deve-se clicar no botão ?I.", "~/img/bt_editar.png");
			help.Oper.Add("Os dados do programa social serão carregados permitindo alterações nos campos.");
			help.Oper.Add("Para salvar os dados do programa social associado ao aluno selecionado deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a alteração nos dados do programa social associado ao aluno selecionado deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

			help.Oper.TitleAdd("Removendo programa social de um aluno");
			help.Oper.Add("Para remover um programa social é necessário fazer a consulta do aluno desejado. Ver 'Consultando programas sociais de um aluno'.");
			help.Oper.Add("Para remover os dados de um programa social associado ao aluno selecionado deve-se clicar no botão ?I e confirmar a remoção do registro.", "~/img/bt_exclui2.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

			help.Oper.TitleAdd("Descrição dos Campos");
			help.Oper.Add("• Aluno: Aluno para o qual estão cadastrados os programas sociais.");
			help.Oper.Add("• Agência de Fomento: Código da instituição que concede o programa social.");
			help.Oper.Add("• Programa: Código do programa social.");
			help.Oper.Add("• Tipo de Benefícios: Código do tipo de benefícios.");
			help.Oper.Add("• Ano de Validade: Ano de validade do programa social.");
			help.Oper.Add("• Data de Início: Data de início do programa social para este aluno.");
			help.Oper.Add("• Data Final: Data final do programa social para este aluno.");

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

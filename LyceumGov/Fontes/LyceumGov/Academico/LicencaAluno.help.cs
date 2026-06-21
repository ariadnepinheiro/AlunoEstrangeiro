using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Academico
{
	public partial class LicencaAluno
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Consultar, cadastrar, alterar e remover faltas justificadas.");

			help.Oper.Add("Para consultar, cadastrar, alterar ou remover uma falta justificada é necessário fazer uma pesquisa pelo aluno de interesse clicando no botão ?I.", "~/Images/bt_drop.png");
			help.Oper.Add("A pesquisa pode ser feita pelo nome, matrícula, nome da mãe e/ou nome do pai do aluno. Após definidas estas informações, deve-se clicar em ?I.", "~/Images/bot_buscar.png");
			help.Oper.Add("O resultado da pesquisa aparecerá em uma lista suspensa e deve-se selecionar o aluno de interesse na linha em que o aluno aparece nesta lista.");

			help.Oper.TitleAdd("Consultando faltas justificadas");
			help.Oper.Add("A consulta é realizada automaticamente quando o aluno de interesse for selecionado.");
			help.Oper.Add("Os dados das faltas justificadas associadas ao aluno selecionado serão exibidos na grade de faltas justificadas.");
			help.Oper.Add("Para filtrar a consulta utilize a linha de filtro acima dos registros. Também é possível definir o tipo de filtro clicando no botão ?I.", "~/img/bt_filtro.jpg");
			help.Oper.Add("Para limpar o filtro deve-se clicar no botão ?I.", "~/img/bt_limpa.png");

			help.Oper.TitleAdd("Cadastrando faltas justificadas para todas as disciplinas");
			help.Oper.Add("Para cadastrar faltas justificadas para todas as disciplinas cursadas pelo aluno é necessário fazer a consulta do aluno desejado. Ver 'Consultando faltas justificadas'.");
			help.Oper.Add("Para cadastrar uma falta justificada associada ao aluno selecionado deve-se clicar no botão ?I.", "~/img/bt_novo.png");
			help.Oper.Add("Deve-se preencher os campos com os dados da nova falta justificada. Selecione a opção <Todas> em Disciplina.");
			help.Oper.Add("Obs.: Lembre-se de que, caso haja falta para uma das disciplinas já cadastrada para a mesma data ou intervalo, nenhuma das faltas será incluída e será exibida a mensagem de erro.");
			help.Oper.Add("Para salvar os dados da nova falta justificada deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a inclusão dos dados da falta justificada deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

			help.Oper.TitleAdd("Cadastrando nova falta justificada");
			help.Oper.Add("Para cadastrar uma falta justificada é necessário fazer a consulta do aluno desejado. Ver 'Consultando faltas justificadas'.");
			help.Oper.Add("Para cadastrar uma falta justificada associada ao aluno selecionado deve-se clicar no botão ?I.", "~/img/bt_novo.png");
			help.Oper.Add("Deve-se preencher os campos com os dados da nova falta justificada.");
			help.Oper.Add("Para salvar os dados da nova falta justificada deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a inclusão dos dados da falta justificada deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

			help.Oper.TitleAdd("Alterando falta justificada");
			help.Oper.Add("Para alterar uma falta justificada é necessário fazer a consulta do aluno desejado. Ver 'Consultando faltas justificadas'.");
			help.Oper.Add("Para alterar os dados de uma falta justificada associada ao aluno selecionado deve-se clicar no botão ?I.", "~/img/bt_editar.png");
			help.Oper.Add("Os dados da falta justificada serão carregados permitindo alterações nos campos.");
			help.Oper.Add("Para salvar os dados da falta justificada deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a alteração nos dados da falta justificada deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

			help.Oper.TitleAdd("Removendo falta justificada");
			help.Oper.Add("Para remover uma falta justificada é necessário fazer a consulta do aluno desejado. Ver 'Consultando faltas justificadas'.");
			help.Oper.Add("Para remover os dados de uma falta justificada associada ao aluno selecionado deve-se clicar no botão ?I e confirmar a remoção do registro.", "~/img/bt_exclui2.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

			help.Oper.TitleAdd("Descrição dos Campos");
			help.Oper.Add("• Disciplina: Disciplina na qual o aluno terá falta justificada.");
			help.Oper.Add("• Turma: Turma referente à disciplina. É automaticamente preenchido ao fornecer a disciplina.");
			help.Oper.Add("• Ano Letivo: Ano letivo referente à turma.");
			help.Oper.Add("• Período Letivo: Período letivo referente à turma.");
			help.Oper.Add("• Data Início: Data a partir da qual a falta justificada torna-se válida.");
			help.Oper.Add("• Data Fim: Data de encerramento da validade da falta justificada.");
			help.Oper.Add("• Descrição: Descrição livre sobre a falta justificada.");

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

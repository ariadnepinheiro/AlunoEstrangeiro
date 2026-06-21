using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Academico
{
	public partial class ListarEncerramentoAluno
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Encerrar e reabrir alunos e visualizar detalhes dos encerramentos e reaberturas de alunos.");

			help.Oper.TitleAdd("Informando dados de pesquisa");
			help.Oper.Add("Para consultar, encerrar ou reabrir um aluno é necessário fazer uma pesquisa pelo aluno de interesse clicando no botão ?I.", "~/Images/bt_drop.png");
			help.Oper.Add("A pesquisa pode ser feita pelo nome, matrícula, nome da mãe e/ou nome do pai do aluno. Após definidas estas informações, deve-se clicar em ?I.", "~/Images/bot_buscar.png");
			help.Oper.Add("O resultado da pesquisa aparecerá em uma lista suspensa e deve-se selecionar o aluno de interesse clicando na linha em que o aluno aparece nesta lista.");
			help.Oper.Add("Obs.: Os dados escolares do aluno selecionado aparecerão destacados na tela.");

			help.Oper.TitleAdd("Consultando encerramentos/reaberturas");
			help.Oper.Add("A consulta é realizada automaticamente quando o aluno de interesse for selecionado.");
			help.Oper.Add("Os dados dos encerramentos/reaberturas associadas ao aluno selecionado serão exibidos na grade de encerramentos.");
			help.Oper.Add("Para filtrar a consulta utilize a linha de filtro acima dos registros. Também é possível definir o tipo de filtro clicando no botão ?I.", "~/img/bt_filtro.jpg");
			help.Oper.Add("Para limpar o filtro deve-se clicar no botão ?I.", "~/img/bt_limpa.png");

			help.Oper.TitleAdd("Encerrando aluno");
			help.Oper.Add("Para realizar um encerramento é necessário fazer a consulta do aluno desejado. Ver 'Consultando encerramentos/reaberturas'.");
			help.Oper.Add("Para encerrar o aluno selecionado deve-se clicar no botão 'Novo Encerramento', caso o aluno não possua nenhum encerramento/reabertura anterior. Se o aluno possuir encerramentos/reaberturas anteriores, deve-se clicar no botão ?I da grade de encerramentos.", "~/img/bt_busca.png");
			help.Oper.Add("Deve-se preencher os campos com os dados do encerramento.");
			help.Oper.Add("Para efetivar o encerramento do aluno selecionado deve-se clicar no botão 'Encerrar'.");
			help.Oper.Add("Caso o aluno selecionado possua pré-matrícula, uma confirmação de cancelamento das pré-matrículas existentes será solicitada.");
			help.Oper.Add("Para confirmar o cancelamento das pré-matrículas deve-se clicar no botão ?I.", "~/Images/SmallOk.png");
			help.Oper.Add("Para cancelar a operação de encerramento do aluno selecionado deve-se clicar no botão ?I.", "~/Images/SmallCancel.png");
			help.Oper.Add("Obs.: O encerramento de um aluno requer necessariamente o cancelamento de todas as pré-matrículas, caso existam.");
			help.Oper.Add("Caso o aluno possua disciplinas matriculadas/trancadas, uma confirmação do cancelamento dessas disciplinas será solicitada.");
			help.Oper.Add("Para confirmar o cancelamento das disciplinas cursadas deve-se clicar no botão ?I.", "~/Images/SmallOk.png");
			help.Oper.Add("Para cancelar a operação de encerramento do aluno selecionado deve-se clicar no botão ?I.", "~/Images/SmallCancel.png");
			help.Oper.Add("Obs.: O encerramento de um aluno requer necessariamente o cancelamento de todas as disciplinas cursadas/trancadas, caso existam.");
			help.Oper.Add("Serão exibidos novamente os campos com os dados do encerramento definidos anteriormente.");
			help.Oper.Add("Para concluir o encerramento do aluno selecionado deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/Images/SmallOk.png");
			help.Oper.Add("Para cancelar a operação de encerramento do aluno selecionado deve-se clicar no botão ?I.", "~/Images/SmallCancel.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o encerramento.");

			help.Oper.TitleAdd("Reabrindo aluno");
			help.Oper.Add("Para realizar uma reabertura é necessário fazer a consulta do aluno desejado. Ver 'Consultando encerramentos/reaberturas'.");
			help.Oper.Add("Para realizar uma reabertura do aluno selecionado deve-se clicar no botão ?I da grade de encerramentos.", "~/img/bt_busca.png");
			help.Oper.Add("Os dados do encerramento do aluno selecionado aparecerão destacados na tela.");
			help.Oper.Add("Para efetivar a reabertura do aluno selecionado deve-se clicar no botão 'Reabrir'.");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a reabertura.");

			help.Oper.TitleAdd("Visualizando detalhes de um encerramento/reabertura");
			help.Oper.Add("Para visualizar os detalhes de um encerramento/reabertura é necessário fazer a consulta do aluno desejado. Ver 'Consultando encerramentos/reaberturas'.");
			help.Oper.Add("Para visualizar os detalhes de um encerramento/reabertura associado ao aluno selecionado deve-se clicar no botão ?I.", "~/img/bt_busca.png");
			help.Oper.Add("Os detalhes do encerramento/reabertura selecionado serão exibidos na tela.");
			help.Oper.Add("Para retornar à grade dos encerramentos/reaberturas associados ao aluno selecionado deve-se clicar no botão 'Voltar'.");

			help.Oper.TitleAdd("Descrição dos Campos");
			help.Oper.Add("• Aluno: Aluno selecionado.");
			help.Oper.Add("• Unidade de Ensino: Unidade de ensino.");
			help.Oper.Add("• Situação: Situação do aluno.");
			help.Oper.Add("• Escolaridade: Escolaridade do aluno.");
			help.Oper.Add("• Turno: Turno do aluno.");
			help.Oper.Add("• Ano de Escolaridade: Ano de escolaridade da do aluno.");

            help.Oper.TitleAdd("Encerramentos do Aluno");
            help.Oper.Add("• Escolaridade: Escolaridade do aluno.");
            help.Oper.Add("• Turno: Turno do aluno.");
            help.Oper.Add("• Ano de Ingresso: Ano de ingresso do aluno.");
            help.Oper.Add("• Período de Ingresso: Período de ingresso do aluno.");
            help.Oper.Add("• Data de Encerramento: Data de encerramento do aluno.");
            help.Oper.Add("• Data de Reabertura: Data de reabertura do aluno.");
            help.Oper.Add("• Motivo: Motivo do encerramento. (Tabela Fixa: LY_MOTIVOSAIDA)");
            help.Oper.Add("• Instituição: Instituição para onde o aluno será transferido.");
            help.Oper.Add("• Obs.: Este campo será habilitado apenas se o motivo do encerramento for transferência.");
            help.Oper.Add("• Data da Colação: Data da colação do aluno.");
            help.Oper.Add("• Obs.: Este campo será habilitado apenas se o motivo do encerramento for conclusão.");
            help.Oper.Add("• Data do Diploma: Data do diploma do aluno.");
            help.Oper.Add("• Obs.: Este campo será habilitado apenas se o motivo do encerramento for conclusão.");
            help.Oper.Add("• Ano de Encerramento: Ano de encerramento do aluno. (Tabela: Ano Letivo)");
            help.Oper.Add("• Período de Encerramento: Período de encerramento do aluno. (Tabela: Ano Letivo, cujo filtro é Ano de Encerramento)");
            help.Oper.Add("• Causa: Causa do encerramento. (Tabela Fixa: LY_CAUSA_ENCERR)");

			help.Oper.TitleAdd("Botões");
			help.Oper.TitleAdd("Consulta de Encerramentos");
			help.Oper.Add("?I: Visualiza os detalhes de um registro.", "~/Images/bot_buscar.png");
			help.Oper.Add("?I: Limpa os filtros selecionados.", "~/img/bt_Limpa.png");
			help.Oper.Add("?I: Seleciona uma linha para exibição completa.", "~/img/bt_busca.png");

			help.Oper.TitleAdd("Modo de Encerramento/Reabertura");
			help.Oper.Add("?I: Confirma encerramento/reabertura.", "~/Images/SmallOk.png");
			help.Oper.Add("?I: Cancela encerramento/reabertura.", "~/Images/SmallCancel.png");
			help.Oper.Add("Obs.: A visibilidade dos botões depende da operação em andamento.");
		}
	}
}


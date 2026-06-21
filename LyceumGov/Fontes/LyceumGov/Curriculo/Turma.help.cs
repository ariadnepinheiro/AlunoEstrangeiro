using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Curriculo
{
	public partial class Turma
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Consultar, cadastrar e alterar turmas.");
			help.Oper.Add("Para consultar, cadastrar ou alterar turmas é necessário selecionar um ano, um período e uma coordenadoria obrigatoriamente nesta ordem. É possível opcionalmente selecionar um município e/ou uma unidade de ensino para filtrar os registros apresentados na grade de turmas.");
			help.Oper.Add("Para selecionar uma coordenadoria, um município ou uma unidade de ensino, deve-se clicar no botão ?I ao lado de sua respectiva grade de pesquisa.", "~/img/bt_busca.png");
			help.Oper.Add("Ao clicar no botão ?I ao lado da grade de pesquisa de coordenadorias, todas as coordenadorias existentes são apresentadas em uma lista suspensa.", "~/img/bt_busca.png");
			help.Oper.Add("A pesquisa pode ser filtrada pelo código e/ou descrição da coordenadoria. Após definidas estas informações, o resultado é apresentado automaticamente na lista suspensa.");
			help.Oper.Add("Deve-se selecionar a coordenadoria de interesse clicando na linha em que a coordenadoria aparece na lista suspensa. Neste momento, se o ano e o período de interesse já estiverem selecionados, os registros serão apresentados na grade de turmas. Além disso, o registro selecionado é aplicado como filtro às grades de pesquisa.");
			help.Oper.Add("Ao clicar no botão ?I ao lado da grade de pesquisa de município, todos os municípios existentes são apresentados em uma lista suspensa.", "~/img/bt_busca.png");
			help.Oper.Add("A pesquisa pode ser filtrada pelo código, nome e/ou estado do município. Após definidas estas informações, o resultado é apresentado automaticamente na lista suspensa.");
			help.Oper.Add("Deve-se selecionar o município de interesse clicando na linha em que o município aparece na lista suspensa. Neste momento, o município selecionado é aplicado como filtro à grade de turmas e às grades de pesquisas.");
			help.Oper.Add("Ao clicar no botão ?I ao lado da grade de pesquisa de unidade de ensino, todas as unidades de ensino existentes são apresentadas em uma lista suspensa.", "~/img/bt_busca.png");
			help.Oper.Add("A pesquisa pode ser filtrada pelo código, descrição, código U.A. e/ou unidade administrativa da unidade de ensino. Após definidas estas informações, o resultado é apresentado automaticamente na lista suspensa.");
			help.Oper.Add("Deve-se selecionar a unidade de ensino de interesse clicando na linha em que a unidade de ensino aparece na lista suspensa. Neste momento, a unidade de ensino selecionada é aplicada como filtro à grade de turmas e às grades de pesquisa.");
			help.Oper.Add("Obs.: Em quaisquer destas pesquisas, utilize o caracter '%' para aplicar um filtro cujo valor está contido no campo pesquisado e não apenas quando estiver no início, conforme o exemplo abaixo:");
			help.Oper.Add("Exemplo: Para filtrar a coluna 'Coordenadoria' da grade de pesquisa de coordenadorias para que só sejam exibidos registros contendo a palavra 'Centro', digite %Centro na coluna 'Coordenadoria'.");

			help.Oper.TitleAdd("Consultando turmas");
			help.Oper.Add("Os dados da turma selecionada serão exibidos e divididos em três abas:");
			help.Oper.Add("1 - Geral: Dados gerais referentes à turma.");
			help.Oper.Add("2 - Quadro de Horários: Quadro de horários por disciplina e professor.");
            help.Oper.Add("3 - Matrículas: Lista de alunos da turma.");
			help.Oper.Add("Obs.: Os horários do quadro de horários serão montados de acordo com a escolaridade, turno, série e unidade física informados no cadastro da turma.");
			help.Oper.Add("Na aba 'Quadro de Horários', serão listadas as disciplinas da grade curricular selecionada (escolaridade, turno e série) no cadastro da turma.");
			help.Oper.Add("Serão listados somente os docentes que:");
			help.Oper.Add("• Estejam habilitados para um grupo de disciplinas que contenha a disciplina selecionada.");
			help.Oper.Add("• Cuja matrícula esteja ativa na lotação.");
			help.Oper.Add("• Cuja matrícula esteja lotada na unidade de ensino escolhida para a turma.");
			help.Oper.Add("• Cuja matrícula ativa seja de uma função que permita regência.");
			help.Oper.Add("Além disso, também serão listados os docentes 00000000, 66666666 e 99999999.");

			help.Oper.TitleAdd("Cadastrando nova turma");
			help.Oper.Add("Para cadastrar nova turma deve-se clicar no botão ?I.", "~/img/bt_novo.png");
			help.Oper.Add("Será carregado um formulário em branco para preenchimento dos dados.");
			help.Oper.Add("Deve-se preencher os campos com os dados da nova turma nas abas 'Geral' e 'Quadro de Horários'.");
			help.Oper.Add("Para salvar os dados da nova turma deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/Images/SmallOk.png");
			help.Oper.Add("Para cancelar a inclusão dos dados da turma deve-se clicar no botão ?I.", "~/Images/SmallCancel.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

			help.Oper.TitleAdd("Alterando turma");
			help.Oper.Add("Para alterar os dados de uma turma deve-se clicar no botão ?I.", "~/img/bt_editar.png");
			help.Oper.Add("Os dados da turma serão carregados e exibidos em três abas:");
			help.Oper.Add("1 - Geral: Dados gerais referentes à turma.");
			help.Oper.Add("2 - Quadro de Horários: Quadro de horários por disciplina e professor.");
            help.Oper.Add("3 - Matrículas: Lista de alunos da turma.");
			//help.Oper.Add("Na aba 'Geral' os dados da turma permitem alterações nos campos.");
			//help.Oper.Add("Na aba 'Quadro de Horários' é possível atribuir horários a docentes por disciplina.");
			help.Oper.Add("Os dados da turma permitem alterações nos campos.");
			help.Oper.Add("Para salvar os dados da turma deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/Images/SmallOk.png");
			help.Oper.Add("Para cancelar a alteração nos dados da turma deve-se clicar no botão ?I.", "~/Images/SmallCancel.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

			help.Oper.TitleAdd("Considerações Importantes");
			help.Oper.Add("1 - Abaixo seguem os passos para habilitar um professor para uma turma:");
			help.Oper.Add("• Associar o docente ao grupo de habilitação de disciplinas.");
			help.Oper.Add("• Lotar o docente em uma função de regente e na unidade de ensino da turma. ");
			help.Oper.Add("Obs.: A lotação deve estar sem a data de desativação.");

			help.Oper.Add("2 - Se ao salvar o quadro de horários ocorrer o erro abaixo:");
			help.Oper.Add("• Função da matrícula do docente incompatível para esta turma.");
			help.Oper.Add("Significa que função da turma é diferente da função do docente.");
			help.Oper.Add("Neste caso, aplica-se a seguinte regra:");
			help.Oper.Add("• Se a turma for Ensino Fundamental (1o. ao 5o. ano), então sua função é DOC II.");
			help.Oper.Add("• Se a turma for Ensino Fundamental (6o. ao 9o. ano) ou Ensino Médio, então sua função é DOC I.");
			help.Oper.Add("Obs.: Os docentes que exercem função de regente podem estar lotados nas funções DOC I ou DOC II.");

			help.Oper.Add("3 - Se ao salvar o quadro de horários ocorrer o erro abaixo:");
			help.Oper.Add("• Disciplina <nome disciplina> não foi incluída no quadro de horários.");
			help.Oper.Add("Significa que esta disciplina pertence a esta turma (de acordo com a grade) e que seu horário deve ser cadastrado.");

			help.Oper.Add("4 - Se ao salvar o quadro de horários ocorrer o erro abaixo:");
			help.Oper.Add("• Função em GLP da matrícula do docente incompatível para esta turma.");
			help.Oper.Add("Significa que o docente não foi associado a essa função para cumprir GLP. Ver a página 'Função do Docente em GLP'.");
			help.Oper.Add("Significa que a carga horária para esta categoria do docente pode não estar cadastrada.");
			help.Oper.Add("Deve-se marcar a categoria do docente e cadastrar a carga horária para esta categoria na página 'Carga Horária das Categorias'.");
			help.Oper.Add("Além disso, deve-se cadastrar a função do docente em GLP na página 'Função do Docente em GLP'.");

			help.Oper.Add("5 - Se ao salvar o quadro de horários ocorrer o erro abaixo:");
			help.Oper.Add("• Disciplina <nome disciplina> foi incluída no quadro de horários com um número menor que o permitido.");
			help.Oper.Add("Significa que o número de horários alocados para esta disciplina nesta turma é menor que o informado nos campos de aulas semanais da disciplina (ver página de disciplinas).");

			help.Oper.Add("6 - Se ao salvar o quadro de horários ocorrer o erro abaixo:");
			help.Oper.Add("• Disciplina <nome disciplina> foi incluída no quadro de horários com um número maior que o permitido.");
			help.Oper.Add("Significa que o número de horários alocados para esta disciplina nesta turma é maior que o informado nos campos de aulas semanais da disciplina (ver página de disciplinas).");

			help.Oper.Add("7 - Se ao salvar o quadro de horários ocorrer o erro abaixo:");
			help.Oper.Add("• Carga horária semanal da matrícula do docente ultrapassa carga horária permitida.");
			help.Oper.Add("Significa que o número de horários alocados para este professor está ultrapassando o limite informado para aulas normais e para GLP na página 'Carga Horária das Categorias'.");

			help.Oper.TitleAdd("Descrição dos Campos");
			help.Oper.TitleAdd("Campos de Pesquisa");
			help.Oper.Add("• Ano: Ano referente à turma.");
			help.Oper.Add("• Período: Período referente à turma.");
			help.Oper.Add("• Turno: Turno referente à turma.");
			help.Oper.Add("• Escolaridade: Escolaridade referente à turma.");
			help.Oper.Add("• Unidade de Ensino: Unidade de ensino a que pertence a turma.");
			help.Oper.Add("• Prefixo Unidade: Prefixo da unidade de ensino da turma.");
			help.Oper.Add("• Série: Série referente à turma.");
			help.Oper.Add("• Prefixo Série: Prefixo da série da turma.");
			help.Oper.Add("• Turma: Código de identificação da turma.");

			help.Oper.TitleAdd("Aba: Geral");
			help.Oper.TitleAdd("• Características:");
			help.Oper.Add("• Unidade Física: Unidade física onde se localiza a turma.");
			help.Oper.Add("• Dependência: Dependência da turma.");
			help.Oper.Add("• Número Máximo de Alunos: Número máximo de alunos da turma.");

			help.Oper.TitleAdd("• Período Letivo:");
			help.Oper.Add("• Início das Aulas: Data de início das aulas do período letivo.");
			help.Oper.Add("• Término das Aulas: Data de fim das aulas do período letivo.");

			help.Oper.TitleAdd("Aba: Quadro de Horários");
			help.Oper.Add("• Disciplina: Disciplina cursada pela turma.");
			help.Oper.Add("• Matrícula do Professor: Matrícula do professor.");

            help.Oper.TitleAdd("Aba: Matrículas");
            help.Oper.Add("• Aluno: Matrícula do aluno.");
            help.Oper.Add("• Nome: Nome do aluno.");
            help.Oper.Add("• Nr. Chamada: Número da chamada do aluno.");
            help.Oper.Add("• Situação: Situação do aluno.");

			help.Oper.TitleAdd("Botões");
			help.Oper.Add("?I: Insere uma nova linha.", "~/img/bt_novo.png");
			help.Oper.Add("?I: Permite alteração na linha.", "~/img/bt_editar.png");
			help.Oper.Add("?I: Seleciona a linha.", "~/img/bt_busca.png");
			help.Oper.Add("?I: Salva a inserção/alteração da linha.", "~/Images/SmallOk.png");
			help.Oper.Add("?I: Cancela a inserção/alteração da nova linha.", "~/Images/SmallCancel.png");
			help.Oper.Add("?I: Marca as células selecionadas.", "~/Images/bot_marcar.png");
			help.Oper.Add("?I: Limpa as células selecionadas.", "~/Images/bot_limpar.png");
			help.Oper.Add("Obs.: A visibilidade dos botões depende da operação em andamento.");
		}
	}
}
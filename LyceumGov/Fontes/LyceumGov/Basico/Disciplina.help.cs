using Techne.Web;

namespace Techne.Lyceum.Net.Basico
{
	public partial class Disciplina
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Consultar, cadastrar, alterar e remover disciplinas.");
			help.Oper.Add("Para consultar, alterar ou remover uma disciplina é necessário fazer uma pesquisa pela disciplina de interesse clicando no botão ?I.", "~/Images/bt_drop.png");
			help.Oper.Add("Todas as disciplinas existentes são apresentadas em uma lista suspensa.");
			help.Oper.Add("A pesquisa pode ser filtrada pelo código e/ou descrição da disciplina. Após definidas estas informações, o resultado é apresentado automaticamente na lista suspensa.");
			help.Oper.Add("Deve-se selecionar a disciplina de interesse clicando na linha em que a disciplina aparece na lista suspensa.");
			help.Oper.Add("Obs.: Nesta pesquisa, utilize o caracter '%' para aplicar um filtro cujo valor está contido no campo pesquisado e não apenas quando estiver no início, conforme o exemplo abaixo:");
			help.Oper.Add("Exemplo: Para filtrar a coluna 'Descrição' para que só sejam exibidos registros contendo a palavra 'História', digite %História na coluna 'Descrição'.");

			help.Oper.TitleAdd("Consultando uma disciplina");
			help.Oper.Add("A consulta é realizada automaticamente quando a disciplina de interesse for selecionada.");
			help.Oper.Add("Os dados da disciplina selecionada serão exibidos e divididos em três abas:");
			help.Oper.Add("1 - Dados Gerais: Dados gerais da disciplina, tais como identificação e carga horária.");
			help.Oper.Add("2 - Dados Complementares: Dados complementares da disciplina no que se refere principalmente aos conceitos adotados para avaliação.");
			help.Oper.Add("3 - Grupo de Habilitações: Lista dos grupos de habilitações associados à disciplina selecionada.");
            help.Oper.Add("4 – Disciplinas Múltiplas: Lista as disciplinas múltiplas da disciplina principal.");

			help.Oper.TitleAdd("Cadastrando nova disciplina");
			help.Oper.Add("Para cadastrar uma nova disciplina deve-se clicar no botão ?I.", "~/Images/SmallNew.png");
			help.Oper.Add("Será carregado um formulário em branco para preenchimento dos dados.");
			help.Oper.Add("Deve-se preencher os campos com os dados da nova disciplina nas abas 'Dados Gerais' e 'Dados Complementares'.");
			help.Oper.Add("Obs.: A aba 'Grupo de Habilitações' fica desabilitada em modo de cadastro.");
			help.Oper.Add("Para salvar os dados da nova disciplina deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/Images/SmallOk.png");
			help.Oper.Add("Para cancelar a inclusão dos dados da disciplina deve-se clicar no botão ?I.", "~/Images/SmallCancel.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

			help.Oper.TitleAdd("Alterando disciplina");
			help.Oper.Add("Para alterar uma disciplina é necessário fazer a consulta da disciplina desejada. Ver 'Consultando uma disciplina'.");
			help.Oper.Add("Para alterar os dados da disciplina selecionada deve-se clicar no botão ?I.", "~/Images/SmallEdit.png");
			help.Oper.Add("Os dados da disciplina serão carregados nas abas 'Dados Gerais' e 'Dados Complementares' permitindo alterações nos campos.");
			help.Oper.Add("Para salvar os dados da disciplina deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/Images/SmallOk.png");
			help.Oper.Add("Para cancelar a alteração nos dados da disciplina deve-se clicar no botão ?I.", "~/Images/SmallCancel.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

			help.Oper.TitleAdd("Removendo disciplina");
			help.Oper.Add("Para remover uma disciplina é necessário fazer a consulta da disciplina desejada. Ver 'Consultando uma disciplina'.");
			help.Oper.Add("Para remover a disciplina selecionada deve-se clicar no botão ?I.", "~/Images/SmallDelete.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

			help.Oper.TitleAdd("Descrição dos Campos");
			help.Oper.TitleAdd("Aba: Dados Gerais");
			
            help.Oper.TitleAdd("• Disciplina:");
			help.Oper.Add("• Código: Código da disciplina. Após criado, este campo não permite alteração.");
			help.Oper.Add("• Disciplina: Nome descritivo da disciplina.");
            help.Oper.Add("• Componente: Componente referente à disciplina. (Tabela Geral: ComponenteDisciplina)");
            help.Oper.Add("• Área de Conhecimento: Área de conhecimento referente à disciplina. (Tabela Geral: AreaConhecimentoDisc)");
			help.Oper.Add("• Código SARE: Código SARE.");
			
            help.Oper.TitleAdd("• Detalhes:");
			help.Oper.Add("• Ativa: Indica se o estado da disciplina é ativo ou inativo.");
			help.Oper.Add("• Verifica no Q.H.I.: Indica se, ao matricular-se na disciplina, o horário do aluno será verificado para evitar conflito de horário com outras disciplinas.");
			help.Oper.Add("• Estágio: Indica se a disciplina é estágio.");
            help.Oper.Add("• Disciplina múltipla: Indica se disciplina possui disciplinas múltiplas.");
            help.Oper.Add("• Atividade Complementar: Indica se disciplina é atividade complementar.");

            help.Oper.TitleAdd("• Carga Horária Total:");
			help.Oper.Add("• Hora de Aula Total: Horas de aula teórica previstas.");
			help.Oper.Add("• Hora de Estágio Total: Horas de estágio previstas.");
            help.Oper.Add("• Hora de Atividade Total: Horas de atividades previstas.");
			help.Oper.Add("• Total de Aulas Semanais: Horas semanais de aula previstas.");
			
            help.Oper.TitleAdd("• Tipo de Avaliação:");
			help.Oper.Add("• Pontuação: Indica se pontuação é presente na avaliação.");
			help.Oper.Add("• Frequência: Indica se frequência é presente na avaliação.");
			help.Oper.Add("• Relatório: Indica se relatório é presente na avaliação.");

			help.Oper.TitleAdd("Aba: Dados Complementares");
			help.Oper.TitleAdd("• Nota:");
			help.Oper.Add("• Grupo: Indica o grupo de conceitos utilizado pela disciplina para as notas. (Tabela: Conceitos)");
			help.Oper.Add("• Nota Máxima: Valor máximo da nota.");
			help.Oper.Add("• Casas Decimais: Número de casas decimais consideradas na nota.");

			help.Oper.TitleAdd("Aba: Grupo de Habilitações");
			help.Oper.Add("• Grupo: Código mnemônico do grupo de habilitações.");
			help.Oper.Add("• Descrição: Descrição detalhada do grupo de habilitações.");

            help.Oper.TitleAdd("Aba: Disciplinas Múltiplas");
            help.Oper.Add("• Disciplina Múltipla: Disciplina múltipla da disciplina principal");


			help.Oper.TitleAdd("Botões");
			help.Oper.Add("?I: Carrega um formulário para novo registro.", "~/Images/SmallNew.png");
			help.Oper.Add("?I: Salva as alterações do registro.", "~/Images/SmallOk.png");
			help.Oper.Add("?I: Cancela a operação registro e retorna para página inicial.", "~/Images/SmallCancel.png");
			help.Oper.Add("?I: Permite alteração no registro.", "~/Images/SmallEdit.png");
			help.Oper.Add("?I: Remove o registro.", "~/Images/SmallDelete.png");
			help.Oper.Add("Obs.: A visibilidade dos botões depende da operação em andamento.");
		}
	}
}

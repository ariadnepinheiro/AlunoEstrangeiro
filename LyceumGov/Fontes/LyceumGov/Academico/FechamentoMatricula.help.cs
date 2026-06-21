namespace Techne.Lyceum.Net.Academico
{
    using Techne.Web;

    public partial class FechamentoMatricula
    {
        public override void HelpInit(HelpData help)
        {
            help.ShowDefaultHelp = false;
            help.Summary.Add("Fechamento do ano letivo e matrículas.");

            help.Oper.TitleAdd("Informando dados de pesquisa");
            help.Oper.Add("Para fechar ano letivo e/ou matrículas é necessário selecionar primeiramente a turma de interesse.");
            help.Oper.Add("Para fazer uma pesquisa pela turma de interesse, é necessário selecionar ano, período, unidade de ensino, escolaridade e turno obrigatoriamente em conjunto e nesta ordem e clicar no botão ?I.", "~/Images/bot_buscar.png");
            help.Oper.Add("Ao clicar no botão ?I ao lado da grade de pesquisa de unidade de ensino, todas as unidades de ensino existentes são apresentadas em uma lista suspensa.", "~/Images/bt_drop.png");
            help.Oper.Add("A pesquisa pode ser filtrada pelo código, descrição, código U.A. e/ou unidade administrativa da unidade de ensino. Após definidas estas informações, o resultado é apresentado automaticamente na lista suspensa.");
            help.Oper.Add("Deve-se selecionar a unidade de ensino de interesse clicando na linha em que a unidade de ensino aparece na lista suspensa.");
            help.Oper.Add("Ao clicar no botão ?I ao lado da grade de pesquisa de escolaridades, todas as escolaridades existentes são apresentadas em uma lista suspensa.", "~/Images/bt_drop.png");
            help.Oper.Add("A pesquisa pode ser filtrada pelo código e/ou descrição da escolaridade. Após definidas estas informações, o resultado é apresentado automaticamente na lista suspensa.");
            help.Oper.Add("Deve-se selecionar a escolaridade de interesse clicando na linha em que a escolaridade aparece na lista suspensa.");
            help.Oper.Add("Obs.: Nas pesquisas por unidade de ensino e escolaridade, utilize os caracteres '%' ou '*' para aplicar um filtro cujo valor está contido no campo pesquisado e não apenas quando estiver no início, conforme o exemplo abaixo:");
            help.Oper.Add("Exemplo: Para filtrar a coluna 'Descrição' para que só sejam exibidos registros contendo a palavra 'José', digite %José ou *José na coluna 'Descrição'.");

            help.Oper.TitleAdd("Fechando ano letivo e/ou matrículas dos alunos da turma selecionada");
            help.Oper.Add("Vide 'Informando dados de pesquisa' para entrar com os dados de pesquisa necessários.");
            help.Oper.Add("Todas as turmas com as informações definidas serão exibidas na grade de turmas.");
            help.Oper.Add("Deve-se visualizar detalhes da turma de interesse clique no botão ?I.", "~/img/bt_busca.png");
            help.Oper.Add("Após selecionar a turma de interesse, os alunos matriculados nela serão exibidos na grade Matrículas.");
            help.Oper.Add("Obs.: Os dados da turma selecionada serão exibidos no topo da página.");
            help.Oper.Add("Ao lado da grade Matrículas, é possível identificar duas opções possíveis:");
            help.Oper.Add("• Não efetuar matrícula: Realiza apenas o fechamento de matrículas.");
            help.Oper.Add("• Efetuar nova matrícula: Realiza o fechamento de matrículas e a abertura de novas matrículas para o período seguinte.");
            help.Oper.Add("Caso a opção 'Não efetuar matrícula' estiver marcada, deve-se selecionar os alunos cujas matrículas serão fechadas na grade Matrículas e a situação de fechamento das matrículas selecionadas (Aprovado/Reprovado por nota/Reprovado por falta) e clicar no botão ?I.", "~/Images/bot_confirmar.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o fechamento de matrículas.");
            help.Oper.Add("Caso a opção 'Efetuar nova natrícula' estiver marcada, deve-se selecionar os alunos cujas matrículas serão fechadas na grade de matrículas.");
            help.Oper.Add("Em seguida, deve-se preencher o formulário com os dados das novas matrículas do período seguinte, selecionar a situação de fechamento das matrículas selecionadas (Aprovado/Reprovado por nota/Reprovado por falta) e clicar no botão ?I.", "~/Images/bot_confirmar.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o fechamento e abertura de matrículas.");
            help.Oper.Add("Obs.: Para selicionar todas as matrículas clique no botão 'Marcar Todos'. Caso contrário clique no botão 'Desmarcar Todos'.");

            help.Oper.TitleAdd("Descrição dos Campos");
            help.Oper.TitleAdd("• Área de Pesquisa");
            help.Oper.Add("• Ano: Ano de referência para a pesquisa de turmas.");
            help.Oper.Add("• Período: Período de referência para a pesquisa de turmas.");
            help.Oper.Add("• Unidade de Ensino: Unidade de ensino de referência para a pesquisa de turmas.");
            help.Oper.Add("• U.A.: Sigla da unidade de ensino.");
            help.Oper.Add("• Escolaridade: Escolaridade de referência para a pesquisa de turmas.");
            help.Oper.Add("• Turno: Turno de referência para a pesquisa de turmas.");

            help.Oper.TitleAdd("• Grade: Turmas");
            help.Oper.Add("• Ano: Ano da turma selecionada.");
            help.Oper.Add("• Período: Período de referência da turma selecionada.");
            help.Oper.Add("• Escolaridade: Escolaridade de referência da turma selecionada.");
            help.Oper.Add("• Turno: Turno da turma selecionada.");
            help.Oper.Add("• Ano de Escolaridade: Ano de escolaridade da turma selecionada.");
            help.Oper.Add("• Turma: Código da turma selecionada.");
            help.Oper.Add("• Unidade de Ensino: Unidade de ensino da turma selecionada.");
            help.Oper.Add("• Capacidade: Capacidade númerica da sala de aula da turma selecionada.");
            help.Oper.Add("• Sala de aula: Número da sala de aula da turma selecionada.");

            help.Oper.TitleAdd("• Dados da Turma");
            help.Oper.Add("• Unidade de Ensino: Unidade de ensino da turma selecionada.");
            help.Oper.Add("• Turma: Código da turma selecionada.");
            help.Oper.Add("• Ano: Ano da turma selecionada.");
            help.Oper.Add("• Período: Período da turma selecionada.");
            help.Oper.Add("• Escolaridade: Escolaridade da turma selecionada.");
            help.Oper.Add("• Turno: Turno da turma selecionada.");
            help.Oper.Add("• Matriz Curricular: Matriz curricular da turma selecionada.");
            help.Oper.Add("• Ano de Escolaridade: Ano de escolaridade da turma selecionada.");

            help.Oper.TitleAdd("• Grade: Matrículas");
            help.Oper.Add("• Nº: Número da matrícula.");
            help.Oper.Add("• Aluno: Nome do aluno.");
            help.Oper.Add("• Foto: Foto do aluno.");
            help.Oper.Add("• Situação da matrícula do aluno.");

            help.Oper.TitleAdd("• Grade: Histórico");
            help.Oper.Add("• Nº: Número da matrícula.");
            help.Oper.Add("• Aluno: Nome do aluno.");
            help.Oper.Add("• Foto: Foto do aluno.");
            help.Oper.Add("• Situação da matrícula do aluno.");

            help.Oper.TitleAdd("Botões");
            help.Oper.Add("?I: Visualiza os detalhes da turma selecionada.", "~/img/bt_busca.png");
            help.Oper.Add("?I: Confirma o fechamento das matrículas.", "~/Images/bot_confirmar.png");
            help.Oper.Add("?I: Limpa os filtros selecionados.", "~/img/bt_Limpa.png");
            help.Oper.Add("Obs.: A visibilidade dos botões depende da operação em andamento.");
        }
    }
}
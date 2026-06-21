using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Curriculo
{
    public partial class HorarioOperacional
    {
        public override void HelpInit(HelpData help)
        {
            help.ShowDefaultHelp = false;
            help.Summary.Add("Consultar, cadastrar, alterar e remover horários operacionais.");

            help.Oper.TitleAdd("Consultando horário operacional");
            help.Oper.Add("Para consultar horário operacional é necessário selecionar a unidade de ensino, a unidade física, a escolaridade, o turno, a matriz curricular e o ano de escolaridade desejada e depois clicar no botão ?I.", "~/Images/bot_buscar.png");
            help.Oper.Add("Para fazer uma pesquisa pela unidade de ensino de interesse, deve-se clicar no botão ?I ao lado de sua grade de pesquisa.", "~/Images/bt_drop.png");
            help.Oper.Add("A pesquisa pode ser feita pelo censo, pela unidade de ensino, pela U.A., pelo CGC e/ou pela situação da unidade de ensino. Após definidas estas informações, deve-se pressionar a tecla ENTER.");
            help.Oper.Add("Para fazer uma pesquisa pela unidade física de interesse, deve-se clicar no botão ?I ao lado de sua grade de pesquisa.", "~/Images/bt_drop.png");
            help.Oper.Add("A pesquisa pode ser feita pelo código e/ou pelo nome da unidade física. Após definidas estas informações, deve-se pressionar a tecla ENTER.");
            help.Oper.Add("Para fazer uma pesquisa pela unidade de ensino de interesse, deve-se clicar no botão ?I ao lado de sua grade de pesquisa.", "~/Images/bt_drop.png");
            help.Oper.Add("A pesquisa pode ser feita pelo censo, pela unidade de ensino, pela U.A., pelo CGC e/ou pela situação da unidade física. Após definidas estas informações, deve-se pressionar a tecla ENTER.");
            help.Oper.Add("Para fazer uma pesquisa pela escolaridade de interesse, deve-se clicar no botão ?I ao lado de sua grade de pesquisa.", "~/Images/bt_drop.png");
            help.Oper.Add("A pesquisa pode ser feita pelo código, pela descrição e/ou pelo nível da unidade física. Após definidas estas informações, deve-se pressionar a tecla ENTER.");
            help.Oper.Add("Obs.: É possível opcionalmente selecionar coordenadoria e/ou município para filtrar os registros apresentados na grade de horários.");

            help.Oper.TitleAdd("Cadastrando horário operacional");
            help.Oper.Add("Vide 'Consultando horário operacional' para entrar com os dados necessários para a pesquisa.");
            help.Oper.Add("Selecione a duração da aula desejada e preencha os campos de horários.");
            help.Oper.Add("Para salvar a inserção dos horários, clique no botão 'Salvar'.");

            help.Oper.TitleAdd("Alterando horário operacional");
            help.Oper.Add("Vide 'Consultando horário operacional' para entrar com os dados necessários para a pesquisa.");
            help.Oper.Add("Altere na grade o horário desejado e clique no botão 'Salvar'.");
            help.Oper.Add("Obs.: Não se pode alterar a duração das aulas. Para fazer essa alteração, é preciso apagar todos os horários e clicar no botão 'Salvar'. Depois disso estará disponível a opção de alteração da duração das aulas.");

            help.Oper.TitleAdd("Removendo horário operacional");
            help.Oper.Add("Vide 'Consultando horário operacional' para entrar com os dados necessários para a pesquisa.");
            help.Oper.Add("Para remover um horário em específico (de uma aula) ou todo horário operacional, é necessário apagar na grade os horários desejados e clicar no botão 'Salvar'.");

            help.Oper.TitleAdd("Descrição dos Campos");
            help.Oper.TitleAdd("• Área de Pesquisa");
            help.Oper.Add("• Coordenadoria: Código de identificação e nome da coordenadoria da turma.");
            help.Oper.Add("• Município: Código de identificação e nome do município da turma.");
            help.Oper.Add("• Unidade Ensino: Código de identificação e nome da unidade de ensino da turma.");
            help.Oper.Add("• Unidade Física: Código de identificação e nome da unidade física.");
            help.Oper.Add("• Escolaridade: Escolaridade da turma.");
            help.Oper.Add("• Turno: Turno do horário operacional. (Tabela: Turno, cujo filtro é Escolaridade)");
            help.Oper.Add("• Matriz Curricular: Matriz curricular da turma. (Tabela: Matriz Curricular, cujos filtros são Escolaridade, Turno e Ano de Escolaridade)");
            help.Oper.Add("• Ano de Escolaridade: Série da turma. (Tabela: Matriz Curricular, cujos filtros são Escolaridade, Turno e Matriz Curricular)");

            help.Oper.TitleAdd("• Grade: Horários");
            help.Oper.Add("• Duração: Valor de duração das aulas.");
            help.Oper.Add("• Aula: Aula referente ao horário operacional em questão.");
            help.Oper.Add("• Hora de Início: Hora de início da aula.");
            help.Oper.Add("• Hora de Término: Hora de término da aula.");

            help.Oper.TitleAdd("Botões");
            help.Oper.Add("?I: Busca registro usando os filtros.", "~/Images/bot_buscar.png");
            help.Oper.Add("Obs.: A visibilidade dos botões depende da operação em andamento.");
        }
    }
}

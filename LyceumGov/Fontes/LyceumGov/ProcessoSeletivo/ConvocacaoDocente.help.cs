using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.ProcessoSeletivo
{
    public partial class ConvocacaoDocente
    {
        public override void HelpInit(HelpData help)
        {
            help.ShowDefaultHelp = false;
            help.Summary.Add("Efetuar convocações.");
            help.Oper.Add("Para efetuar convocações, é necessário fazer uma pesquisa pelo processo seletivo, pela coordenadoria, pela habilitação e pela disciplina de ingresso clicando no botão ?I ao lado de sua grade de pesquisa.", "~/Images/bt_drop.png");
            help.Oper.Add("Ao clicar no botão ?I, todos registros de Processos Seletivos/Coordenadoria/Função/Disciplina de Ingresso existentes são apresentados em uma lista suspensa.", "~/Images/bt_drop.png");
			help.Oper.Add("No caso de processo seletivo, a pesquisa pode ser filtrada por Processo Seletivo e/ou Descrição. Após definidas estas informações, o resultado é apresentado automaticamente na lista suspensa.");
			help.Oper.Add("No caso de coordenadoria, a pesquisa pode ser filtrada por Coordenadoria e/ou Descrição. Após definidas estas informações, o resultado é apresentado automaticamente na lista suspensa.");
			help.Oper.Add("No caso de habilitação, a pesquisa pode ser filtrada por Categoria e/ou Descrição. Após definidas estas informações, o resultado é apresentado automaticamente na lista suspensa.");
			help.Oper.Add("No caso de disciplina de ingresso, a pesquisa pode ser filtrada por Disciplina e/ou Descrição. Após definidas estas informações, o resultado é apresentado automaticamente na lista suspensa.");
			help.Oper.Add("Deve-se selecionar o registro de interesse clicando na linha em que o registro aparece na lista suspensa.");
			help.Oper.Add("Obs.: Nesta pesquisa, utilize o caracter '%' para aplicar um filtro cujo valor está contido no campo pesquisado e não apenas quando estiver no início, conforme o exemplo abaixo:");
			help.Oper.Add("Exemplo: Para filtrar a coluna 'Descrição' da grade de pesquisa de processos seletivos para que só sejam exibidos registros contendo a palavra 'História', digite %História na coluna 'Descrição'.");

			help.Oper.TitleAdd("Efetuando convocação");
			help.Oper.Add("Depois de selecionado o Processo Seletivo, a Coordenadoria, a Habilitação e a Disciplina de Ingresso uma nova tela sera carregada abaixo.");
			help.Oper.Add("Preencha os campos e depois clique no botão 'Selecionar'.");
			help.Oper.Add("Uma nova tela será carregada abaixo com os registros de docentes.");
			help.Oper.Add("Selecione os docentes desejados e clique no botão 'Convocar'.");
			help.Oper.Add("Caso deseje cancelar a seleção clique no botão 'Cancelar Seleção'.");

			help.Oper.TitleAdd("Descrição dos Campos");
			help.Oper.TitleAdd("Área de Pesquisa: Processo Seletivo");
			help.Oper.Add("• Processo Seletivo: Código de identificação do processo seletivo.");
			help.Oper.Add("• Descrição: Descrição do processo seletivo.");
			help.Oper.TitleAdd("Área de Pesquisa: Coordenadoria");
			help.Oper.Add("• Coordenadoria: Código de identificação da coordenadoria.");
			help.Oper.Add("• Descrição: Descrição da coordenadoria.");
			help.Oper.TitleAdd("Área de Pesquisa: Função");
			help.Oper.Add("• Categoria: Código de identificação da função.");
			help.Oper.Add("• Descrição: Descrição da função.");
			help.Oper.TitleAdd("Área de Pesquisa: Disciplina de Ingresso");
			help.Oper.Add("• Disciplina: Código de identificação da disciplina.");
			help.Oper.Add("• Descrição: Descrição da disciplina.");

			help.Oper.TitleAdd("Aba: Dados do Processo Seletivo");
			help.Oper.Add("• Disponível: Quantidade de vagas disponíveis.");
			help.Oper.Add("• Quantidade: Número de docentes que serão convocados.");
			help.Oper.Add("• Data de Apresentação: Data de apresentação dos docentes convocados.");
			help.Oper.Add("• Horário de Apresentação: Horário de apresentação dos docentes convocados.");

			help.Oper.TitleAdd("Aba: Seleção");
			help.Oper.Add("• Inscrição: Número de inscrição do docente.");
			help.Oper.Add("• Nome: Nome do docente.");
			help.Oper.Add("• Data de Nascimento: Data de nascimento do docente.");
			help.Oper.Add("• Pontuação: Valor de pontuação do docente.");
            help.Oper.Add("• Situação: Situação do docente.");

			help.Oper.TitleAdd("Botões");
			help.Oper.Add("?I: Limpa os filtros selecionados.", "~/img/bt_Limpa.png");
			help.Oper.Add("Obs.: A visibilidade dos botões depende da operação em andamento.");
        }
    }
}

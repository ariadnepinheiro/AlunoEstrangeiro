using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Basico
{
    public partial class AndamentoGLP
    {
        public override void HelpInit(HelpData help)
        {
            help.ShowDefaultHelp = false;
            help.Summary.Add("Consultar andamento de solicitações de GLP.");

            help.Oper.TitleAdd("Informando dados de pesquisa");
            help.Oper.Add("Para consultar o andamento de solicitações de GLP é necessário selecionar uma unidade de ensino, o ano e o mês.");
            help.Oper.Add("Ao clicar no botão ?I ao lado da grade de pesquisa de unidades de ensino, todas as unidades de ensino existentes são apresentadas em uma lista suspensa.", "~/Images/bt_drop.png");
            help.Oper.Add("A pesquisa pode ser filtrada pelo código, descrição, U.A., CNPJ ou situação da unidade de ensino. Após definidas estas informações, deve-se pressionar a tecla ENTER para filtrar os resultados.");
            help.Oper.Add("Deve-se selecionar a unidade de ensino de interesse clicando na linha em que a unidade aparece na lista suspensa.");
            help.Oper.Add("Obs.: Nas grades de pesquisa de unidade de ensino, utilize os caracteres '%' ou '*' para aplicar um filtro cujo valor está contido no campo pesquisado e não apenas quando estiver no início, conforme o exemplo abaixo:");
            help.Oper.Add("Exemplo: Para filtrar a coluna 'Descrição' da grade de pesquisa de unidade de ensino para que só sejam exibidos registros contendo a palavra 'José', digite %José ou *José na coluna 'Nome' e pressione a tecla ENTER.");

            help.Oper.TitleAdd("Consultando andamento de solicitações de GLP");
            help.Oper.Add("Vide 'Informando dados de pesquisa' para entrar com os dados da pesquisa.");
            help.Oper.Add("A grade Andamento de Solicitações será carregada na página.");

            help.Oper.TitleAdd("Descrição dos Campos");
            help.Oper.TitleAdd("• Área de Pesquisa");
            help.Oper.Add("• Unidade de Ensino: Código de identificação e nome da unidade de ensino.");
            help.Oper.Add("• Ano: Selecione o ano desejado. (Tabela: Ano Letivo)");
            help.Oper.Add("• Mês: Selecione o mês desejado referente ao ano selecionado anteriormente.");

            help.Oper.TitleAdd("• Grade: Andamento de Solicitações");
            help.Oper.Add("• Docente: Nome do docente.");
            help.Oper.Add("• Disciplina: Nome da disciplina solicitada pelo docente.");
            help.Oper.Add("• Status: Status da solicitação do docente.");
            help.Oper.Add("• Quantidade: Quantidade de solicitações.");
            help.Oper.Add("• Data da Última Situação: Data da alteração para última situação da solicitação.");
            help.Oper.Add("• Solicitada: Número de solicitações.");
            help.Oper.Add("• Alocada: Número de alocações.");
            help.Oper.Add("• Cancelada: Número de cancelamentos.");
            help.Oper.Add("• Usúario: Nome do usuário.");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Biblioteca
{
    public partial class Pesquisa
    {
        public override void HelpInit(HelpData help)
        {
            help.ShowDefaultHelp = false;
            help.Summary.Add("Buscar por títulos, autores e/ou editoras no acervo das bibliotecas.");

            help.Oper.TitleAdd("Buscando por títulos, autores e/ou editoras");
            help.Oper.Add("Para buscar um título pelo nome ou pelo(s) autor(es) do mesmo ou pela editora do mesmo no acervo das bibliotecas, é necessário digitar o nome desejado e clicar no botão ?I.", "~/Images/bot_buscar.png");
            help.Oper.Add("Os resultados encontrados serão listados na tela. Caso queira visualizar detalhes do mesmo, clique em 'Consultar'.");
            help.Oper.Add("Caso queira fazer outra busca, digite o nome a ser procurado e clique no botão ?I.", "~/Images/bot_buscar.png");
            help.Oper.Add("Se desejar fazer uma pesquisa com filtros adicionais, clique em 'Busca avançada'.");
            help.Oper.Add("Caso não encontre o título desejado, clique em 'Incluir sugestão de aquisição' para incluir a sugestão de compra do mesmo.");

            help.Oper.TitleAdd("Descrição dos Campos");
            help.Oper.TitleAdd("• Dados de Pesquisa");
            help.Oper.Add("• Pesquisa: Informar nome do título ou nome do(s) autor(es) do mesmo ou nome da editora do mesmo.");

            help.Oper.TitleAdd("• Dados de Resultado");
            help.Oper.Add("• Título: Nome do título.");
            help.Oper.Add("• Autor(es): Nome do(s) autor(es) do título.");
            help.Oper.Add("• Editora: Nome da editora do título.");

            help.Oper.TitleAdd("Botões");
            help.Oper.Add("?I: Buscar: Filtra os registros.", "~/Images/bot_buscar.png");
            help.Oper.Add("Obs.: A visibilidade dos botões depende da operação em andamento.");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Biblioteca
{
    public partial class PesquisaAvancada
    {
        public override void HelpInit(HelpData help)
        {
            help.ShowDefaultHelp = false;
            help.Summary.Add("Buscar por títulos, autores e/ou editoras no acervo das bibliotecas utilizando filtros adicionais.");

            help.Oper.TitleAdd("Buscando por títulos, autores e/ou editoras utilizando filtros adicionais");
            help.Oper.Add("Para buscar um título em bibliotecas no modo avançado, é necessário selecionar os filtros desejados e clicar no botão ?I.", "~/Images/bot_buscar.png");
            help.Oper.Add("Há opções de pesquisar por título, autor(es), editora e/ou assunto.");
            help.Oper.Add("A busca pode ser limitada pelo tipo de material (livro e/ou dvd).");
            help.Oper.Add("Também existe a opção de filtrar por disponibilidade e localização.");
            help.Oper.Add("Os resultados encontrados serão listados na tela. Caso queira visualizar detalhes do mesmo, clique em 'Consultar'.");
            help.Oper.Add("Caso não encontre o título desejado, clique em 'Incluir sugestão de aquisição' para incluir a sugestão de compra do mesmo.");

            help.Oper.TitleAdd("Descrição dos Campos");
            help.Oper.Add("• Título: Nome do título.");
            help.Oper.Add("• Autor: Nome do(s) autor(es) do título.");
            help.Oper.Add("• Editora: Nome da editora do título.");
            help.Oper.Add("• Assunto: Assunto do título.");
            help.Oper.Add("• Biblioteca: Código de identificação e nome da biblioteca.");

            help.Oper.TitleAdd("Botões");
            help.Oper.Add("?I: Buscar: Filtra os registros.", "~/Images/bot_buscar.png");
            help.Oper.Add("Obs.: A visibilidade dos botões depende da operação em andamento.");
        }
    }
}

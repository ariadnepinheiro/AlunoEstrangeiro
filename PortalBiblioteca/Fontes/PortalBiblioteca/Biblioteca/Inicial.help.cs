using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Biblioteca
{
    public partial class Inicial
    {
        public override void HelpInit(HelpData help)
        {
            help.ShowDefaultHelp = false;
            help.Summary.Add("Tela inicial do portal biblioteca.");

            help.Oper.TitleAdd("Descrição das funcionalidades das páginas");
            help.Oper.Add("• Buscar: Buscar por títulos, autores e/ou editoras no acervo da biblioteca.");
            help.Oper.Add("• Busca Avançada: Buscar por títulos, autores e/ou editoras no acervo da biblioteca com opções de filtros adicionais.");
            help.Oper.Add("• Incluir Sugestão de Aquisição: Incluir sugestão de material não encontrado na biblioteca.");
            help.Oper.Add("• Consultar: Consultar situação do título selecionado e fazer a reserva do mesmo.");
        }
    }
}

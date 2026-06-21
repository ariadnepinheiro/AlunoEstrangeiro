using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Biblioteca
{
    public partial class ReservaMaterial
    {
        public override void HelpInit(HelpData help)
        {
            help.ShowDefaultHelp = false;
            help.Summary.Add("Reservar materiais.");

            help.Oper.TitleAdd("Reservando um material");
            help.Oper.Add("Na tela de reservas há detalhes do material selecionado.");
            help.Oper.Add("Para reservar um material é necessário selecionar o exemplar desejado e clicar no botão 'Reservar'.");
            help.Oper.Add("Uma mensagem na tela irá confirmar o sucesso da reserva.");

            help.Oper.TitleAdd("Descrição dos Campos");
            help.Oper.TitleAdd("• Dados do Material");
            help.Oper.Add("• Título: Nome do título.");
            help.Oper.Add("• Autor(es): Nome do(s) autor(es) do título.");
            help.Oper.Add("• Editora: Nome da editora do título.");

            help.Oper.TitleAdd("• Tabela: Exemplares");
            help.Oper.Add("• Código: Código de identificação do material.");
            help.Oper.Add("• Biblioteca: Nome da biblioteca aonde se encontra o material.");
            help.Oper.Add("• Unidade: Nome da unidade aonde se encontra o material.");
            help.Oper.Add("• Situação: Situação do material.");
            help.Oper.Add("• Reservar: Reservar o material.");
        }
    }
}

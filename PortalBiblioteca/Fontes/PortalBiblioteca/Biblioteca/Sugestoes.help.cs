using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Biblioteca
{
    public partial class Sugestoes
    {
        public override void HelpInit(HelpData help)
        {
            help.ShowDefaultHelp = false;
            help.Summary.Add("Incluir sugestão de aquisição de material.");

            help.Oper.TitleAdd("Incluindo sugestão de aquisição");
            help.Oper.Add("Preencha os campos necessários e clique no botão 'Enviar' para incluir a sugestão de aquisição de uma material não encontrado na biblioteca.");
            help.Oper.Add("Uma mensagem na tela irá confirmar o sucesso da inclusão da sugestão.");

            help.Oper.TitleAdd("Descrição dos Campos");
            help.Oper.Add("• Título: Nome do título.");
            help.Oper.Add("• Autor(es): Nome do(s) autor(es) do título.");
            help.Oper.Add("• Editora: Nome da editora do título.");
            help.Oper.Add("• Ano: Ano da edição/publicação do material.");
            help.Oper.Add("• Observações: Observações com relação a aquisição sugerida.");
        }
    }
}

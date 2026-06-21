using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Hades
{
    public partial class AlteraSenhaUsuario
    {
        public override void HelpInit(HelpData help)
        {
            help.ShowDefaultHelp = false;
            help.Summary.Add("Alterar senha de usuários.");

            help.Oper.TitleAdd("Consultando usuário");
            help.Oper.Add("Para alterar a senha do usuário é necessário fazer uma pesquisa pelo mesmo clicando no botão ?I.", "~/Images/bt_drop.png");
            help.Oper.Add("A pesquisa pode ser feita pelo usuário e/ou nome do usuário. Após definidas estas informações, deve-se clicar em ?I.", "~/Images/bot_buscar.png");
            help.Oper.Add("O resultado da pesquisa aparecerá em uma lista suspensa e deve-se selecionar o usuário de interesse clicando na linha em que o usuário aparece nesta lista.");

            help.Oper.TitleAdd("Alterando senha do usuário");
            help.Oper.Add("Digite a nova senha nos campos requisitados e clique no botão ?I para realizar a alteração.", "~/images/bot_confirmar.png");

            help.Oper.TitleAdd("Descrição dos Campos");
            help.Oper.TitleAdd("• Área de Pesquisa:");
            help.Oper.Add("• Usuário: Código de identificação e nome do usuário.");

            help.Oper.TitleAdd("• Senha");
            help.Oper.Add("• Senha: Nova senha do usuário.");
            help.Oper.Add("• Confirmar Senha: Confirmar nova senha do usuário.");

            help.Oper.TitleAdd("Botões");
            help.Oper.Add("?I: Confirma a modificação da senha.", "~/images/bot_confirmar.png");
            help.Oper.Add("Obs.: A visibilidade dos botões depende da operação em andamento.");
        }
    }
}

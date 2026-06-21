using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.ProcessoSeletivo
{
    public partial class LoginCandidato
    {
        public override void HelpInit(HelpData help)
        {
            help.ShowDefaultHelp = false;
            help.Summary.Add("Login em Processo Seletivo.");

            help.Oper.TitleAdd("Logando em Processo Seletivo");
            help.Oper.Add("Para logar em Processo Seletivo é necessário digitar o nome do usuário em Usuário e digitar a senha em Senha.");
            help.Oper.Add("Depois clique no botão ?I para entrar em Processo Seletivo. Uma mensagem de erro irá aparecer na tela caso o Usuário e/ou a Senha estejam incorretos.", "~/Images/bot_entrar.png");

            help.Oper.TitleAdd("Descrição dos Campos");
            help.Oper.Add("• Usuário: Nome do usuário em Processo Seletivo.");
            help.Oper.Add("• Senha: Senha do usuário em Processo Seletivo.");

            help.Oper.TitleAdd("Botões");
            help.Oper.Add("?I: Entra com o login.", "~/Images/bot_entrar.png");
            help.Oper.Add("Obs.: A visibilidade dos botões depende da operação em andamento.");
        }
    }
}

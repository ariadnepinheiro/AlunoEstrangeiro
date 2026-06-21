using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Hades
{
	public partial class AlteracaoSenha
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Alterar senha do usuário logado.");

			help.Oper.TitleAdd("Alterando senha do usuário");
			help.Oper.Add("Deve-se preencher os campos com os dados do usuário logado.");
			help.Oper.Add("Para salvar a alteração da senha do usuário deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/images/bot_confirmar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

			help.Oper.TitleAdd("Descrição dos Campos");
			help.Oper.Add("• Usuário: Login do usuário logado. Este campo não é editável.");
			help.Oper.Add("• Nome: Nome completo do usuário logado. Este campo não é editável.");
			help.Oper.Add("• Senha antiga: Senha atual do usuário logado.");
			help.Oper.Add("• Nova Senha: Nova senha do usuário.");
			help.Oper.Add("• Confirmação Nova Senha: Confirmação da nova senha do usuário.");
			help.Oper.Add("• Data de Alteração da Senha: Data em que a senha do usuário foi alterada. Este campo recebe o valor padrão da data do dia corrente e fica desabilitado para modificações.");

			help.Oper.TitleAdd("Botões");
			help.Oper.Add("?I: Confirma alteração do registro.", "~/images/bot_confirmar.png");
		}
	}
}

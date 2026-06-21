using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Seguranca
{
	public partial class Identificacao
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Obter acesso ao sistema.");

			help.Oper.TitleAdd("Obtendo acesso ao sistema");
            help.Oper.Add("Deve-se preencher os campos de login e senha com os dados do usuário cadastrado no sistema. E preencher o código indicado na imagem no campo próprio.");
			help.Oper.Add("Após definidas estas informações, deve-se clicar em 'Entrar'. Em seguida, o acesso ao sistema será concedido para o usuário.");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o login.");

			help.Oper.TitleAdd("Descrição dos Campos");
			help.Oper.Add("• Usuário: Login do usuário cadastrado no sistema.");
			help.Oper.Add("• Senha: Senha do usuário cadastrado no sistema.");
            help.Oper.Add("• Código da imagem: Código para validação da tentativa de acesso ao sistema.");
		}
	}
}

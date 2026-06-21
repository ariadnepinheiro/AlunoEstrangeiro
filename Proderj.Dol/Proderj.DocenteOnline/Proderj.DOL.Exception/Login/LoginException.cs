using System;
using System.Collections.Generic;
using System.Resources;
using Proderj.DOL.Exception.Login;

namespace Proderj.DOL.Exception
{
	public class LoginException : ApplicationException
	{
		private readonly string mensagem;
		public override string Message
		{
			get
			{
				return mensagem;
			}
		}

		private readonly TipoEnum tipo;
		public enum TipoEnum
		{
			DocenteInexistente,
            MatriculaOuSenhaInvalida,
            PessoaInexistente,
            SenhaAtualIncorreta,
            SenhaCurta,
            SenhaIncorreta,			
			SenhasNaoConferem,
            CaptchaNaoConfere
		}

		public string TipoDeExcecao {
            get { 
                return tipo.ToString();
            } }

		public LoginException(TipoEnum tipo)
		{
			this.tipo = tipo;
			mensagem = GeraMensagem();
		}

		public LoginException(string mensagem) : base (mensagem)
		{
			this.mensagem = mensagem;
		}

		public string GeraMensagem()
		{
			string mensagem = LoginResource.ResourceManager.GetString(tipo.ToString());

			return mensagem;
		}
	}
}

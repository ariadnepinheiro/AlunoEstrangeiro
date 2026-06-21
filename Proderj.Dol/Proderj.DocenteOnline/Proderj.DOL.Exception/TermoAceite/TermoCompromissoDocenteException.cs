using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Exception.TermoAceite
{
    public class TermoCompromissoDocenteException : ApplicationException
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
			TermoCompromissoNaoEncontrado
		}

		public string TipoDeErro { get { return tipo.ToString(); } }

		public TermoCompromissoDocenteException(TipoEnum tipoDeExcecao)
		{
			tipo = tipoDeExcecao;
			mensagem = GeraMensagem();
		}

		protected string GeraMensagem()
		{
			string mensagem = TermoCompromissoResource.ResourceManager.GetString(tipo.ToString());

			return mensagem;
		}
    }
}

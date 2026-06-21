using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Exception
{
	public class UnidadeEnsinoException : ApplicationException
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
			UnidadeEnsinoInexistenteParaCodigoInformado
		}

		public string TipoDeErro { get { return tipo.ToString(); } }

		public string CodigoTurma { get; set; }

		public UnidadeEnsinoException(TipoEnum tipo)
		{
			this.tipo = tipo;
			mensagem = GeraMensagem();

		}

		public UnidadeEnsinoException(string mensagem)
			: base(mensagem)
		{
			this.mensagem = mensagem;
		}

		public string GeraMensagem()
		{
			string mensagem = CoreResource.ResourceManager.GetString(tipo.ToString());

			return mensagem;
		}


	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Exception
{	
	public class SelecaoTurmasException : ApplicationException
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
			MatriculaInvalida,
			NumeroFuncionario
		}

		public string TipoDeErro { get { return tipo.ToString(); } }

		public SelecaoTurmasException(TipoEnum tipo)
		{
			this.tipo = tipo;
			mensagem = GeraMensagem();

		}

		public SelecaoTurmasException(string mensagem)
			: base(mensagem)
		{
			this.mensagem = mensagem;
		}

		public string GeraMensagem()
		{
			string mensagem = SelecaoTurmasResource.ResourceManager.GetString(tipo.ToString());

			return mensagem;
		}
	}
}

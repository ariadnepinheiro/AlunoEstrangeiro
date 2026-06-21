using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;

namespace Proderj.DOL.Exception
{
	public class DocenteException : ExcecaoBase<DocenteMensagensEnum>
	{
		public DocenteException(DocenteMensagensEnum tipo)
			: base(tipo) { }

		public DocenteException(string mensagem)
			: base(mensagem) { }

		public DocenteException(DocenteMensagensEnum tipo, short ano, short periodo)
		{
			this.TipoEnum = tipo;
			Ano = ano;
			Periodo = periodo;
			this.MensagemDeErro = GeraMensagem();
		}

		public short Ano { get; set; }
		public short Periodo { get; set; }

		protected override string GeraMensagem()
		{
			if (MensagemDeErro != null)
				return MensagemDeErro;

			string mensagemNova = DocenteResource.ResourceManager.GetString(TipoDeErro);

			if (TipoEnum == DocenteMensagensEnum.DocenteInexistente)
				mensagemNova = String.Format(mensagemNova, Ano, Periodo);

			return mensagemNova;
		}	
	}

	public enum DocenteMensagensEnum
	{
		DocenteInexistente
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Exception
{
	public class LogCompetenciaHabilidadeDocenteException : ApplicationException
	{
		private readonly string mensagem;
		public override string Message
		{
			get
			{
				return GeraMensagem();
			}
		}

		private readonly TipoEnum tipo;
		public enum TipoEnum
		{
			HaMaisDeUmItemHabilidadeDocenteCadastrado,			
		}

		public string TipoDeErro { get { return tipo.ToString(); } }

		public string CodigoTurma { get; set; }
		public string CodigoAluno { get; set; }

		public string NomeAluno { get; set; }

		public LogCompetenciaHabilidadeDocenteException(TipoEnum tipo)
		{
			this.tipo = tipo;
		}

		public LogCompetenciaHabilidadeDocenteException(string mensagem)
			: base(mensagem)
		{
			this.mensagem = mensagem;
		}

		public string GeraMensagem()
		{
			if (mensagem != null)
			{
				return mensagem;
			}
			else
			{
				string mensagemNova = LogCompetenciaHabilidadeDocenteResource.ResourceManager.GetString(tipo.ToString());

				if (tipo == TipoEnum.HaMaisDeUmItemHabilidadeDocenteCadastrado)
					mensagemNova = String.Format(mensagemNova, CodigoTurma);

				return mensagemNova;
			}
		}
	}
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Exception
{
	public class CadastroGlpException : ApplicationException
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
			DocenteInexistente,
			DisciplinaInvalida,
			MunicipioInvalido,
			MunicipioObrigatorio,
			CoordenadoriaObrigatoria,
			DisciplinaObrigatoria,
			HoraFinalInferiorAHoraInicial,
			DuracaoDeAulaMinima,
			NaoExisteDisponibilidade,
			HorarioOcupado,
			ItemInexistente,
            RegionalObrigatoria

		}
		public string TipoDeErro { get { return tipo.ToString(); } }

		public CadastroGlpException(TipoEnum tipo)
		{
			this.tipo = tipo;
		}

		public CadastroGlpException(string mensagem)
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
				string mensagemNova = CadastroGlpResource.ResourceManager.GetString(tipo.ToString());

				return mensagemNova;
			}
		}
	}
}

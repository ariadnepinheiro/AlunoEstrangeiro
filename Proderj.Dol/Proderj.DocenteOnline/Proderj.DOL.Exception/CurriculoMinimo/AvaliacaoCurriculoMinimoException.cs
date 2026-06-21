using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Exception
{
	public class AvaliacaoCurriculoMinimoException : ApplicationException
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
			NaoExisteAvaliacaoParaBimestre,
			NecessarioPreenchimentodeCampos,
			TurmaEDisciplinaInvalidaParaLancamento,
			AcessoNegadoDocente_A_TurmaEDisciplina,
			RespostasInformadasInvalidas,
            NaoExisteJustificativaParaBimestre
		}

		public string CodigoTurma { get; set; }
		public string TipoDeErro { get { return tipo.ToString(); } }

		public AvaliacaoCurriculoMinimoException(TipoEnum tipo)
		{
			this.tipo = tipo;
		}

		public AvaliacaoCurriculoMinimoException(string mensagem)
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
				string mensagemNova = AvaliacaoCurriculoMinimoResource.ResourceManager.GetString(tipo.ToString());

				return mensagemNova;
			}
		}
	}
}

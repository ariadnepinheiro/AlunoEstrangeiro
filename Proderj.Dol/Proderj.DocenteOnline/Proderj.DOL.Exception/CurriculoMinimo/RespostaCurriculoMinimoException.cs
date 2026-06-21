using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Exception
{
	public class RespostaCurriculoMinimoException : ApplicationException
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
			TurmaEDisciplinaInvalidaParaLancamento,
			AcessoNegadoDocente_A_TurmaEDisciplina,
			NaoExistemDadosDeGruposERespostasParaLancamento,
			NaoExistemBimestresValidosParaLancamento,
			OsItensNaoPuderamSerSalvos,
			NaoExistemBimestresValidosParaExibicao,
			RespostasInformadasInvalidas,
		}

		public string TipoDeErro { get { return tipo.ToString(); } }

		public string CodigoTurma { get; set; }
		public string CodigoAluno { get; set; }

		public string NomeAluno { get; set; }

		public RespostaCurriculoMinimoException(TipoEnum tipo)
		{
			this.tipo = tipo;
		}

		public RespostaCurriculoMinimoException(string mensagem)
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
				string mensagemNova = RespostaCurriculoMinimoResource.ResourceManager.GetString(tipo.ToString());

				if (tipo == TipoEnum.TurmaEDisciplinaInvalidaParaLancamento)
					mensagemNova = String.Format(mensagemNova, CodigoTurma);
					
				return mensagemNova;
			}
		}

		public short Subperiodo { get; set; }
	}
}

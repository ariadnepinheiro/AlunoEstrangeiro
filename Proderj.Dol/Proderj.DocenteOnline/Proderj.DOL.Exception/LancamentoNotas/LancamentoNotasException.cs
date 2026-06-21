using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Exception
{
	public class LancamentoNotasException : ApplicationException
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
			AlunoInvalidoParaLancamento,

			NaoHaAlunosMatriculados,
			NaoHaAulasPrevistas,
			AulasPrevistasMenorQueZero,
			NaoHaAulasDadas,
			AulasDadasMenorQueZero,
			AlunoNaoMatriculado,
			DisciplinaNaoPossuiAulasDadas,
			NotaNaoLancadaATodosOsAlunos,
			JustificativaNaoPreenchido,
			FaltaNaoLancadaATodosOsAlunos,
			FaltasMaiorQueAulasDadas,
			FaltasMenorQueZero,
			AlunoComFrequenciaMaximaSemAvalicao,
			AlunoSemFrequenciaComAvaliacao,
            JustificativaOutrasAcimaPresencaMinima,
			NaoHaNotasEnviadas,
			NaoHaFaltasEnviadas,
			DadosCompementaresNaoAtualizadosCorretamente,
			SemProvaDaTurmaParaLancamento,
            NotaRecuperacaoNaoPreenchido,
            NaoHaRecuperacaoNotaAzul,
            NaoPermitiNotaAcimaNotaMaxima,
            NaoPossuiLancamentoNotas,
            NaoPossuiLancamentoFaltas,
            NaoPossuiLancamentoNotasFaltas,
            SemMatriculaOUProtocolo,
            DisciplinaNaoPossuiFrequenciaNoPeriodo
		}

		public string TipoDeErro { get { return tipo.ToString(); } }

		public string CodigoTurma { get; set; }
		public string CodifgAluno { get; set; }

		public string NomeAluno { get; set; }

		public LancamentoNotasException(TipoEnum tipo)
		{
			this.tipo = tipo;
		}

		public LancamentoNotasException(string mensagem)
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
				string mensagemNova = LancamentoNotasResource.ResourceManager.GetString(tipo.ToString());

				if (tipo == TipoEnum.TurmaEDisciplinaInvalidaParaLancamento)
					mensagemNova = String.Format(mensagemNova, CodigoTurma);

				if (tipo == TipoEnum.AlunoInvalidoParaLancamento ||
					tipo == TipoEnum.FaltasMenorQueZero ||
					tipo == TipoEnum.FaltasMaiorQueAulasDadas ||
					tipo == TipoEnum.AlunoComFrequenciaMaximaSemAvalicao ||
					tipo == TipoEnum.AlunoSemFrequenciaComAvaliacao ||
                    tipo == TipoEnum.JustificativaOutrasAcimaPresencaMinima ||
                    tipo == TipoEnum.JustificativaNaoPreenchido)
				{
					mensagemNova = String.Format(mensagemNova, NomeAluno);
				}

				return mensagemNova;
			}
		}
	}
}

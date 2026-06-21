using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Repository;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Service
{
	public class TurmaService : ITurmaService
	{
		private readonly IProvaRepository repositorioProva;
		private readonly IFrequenciaRepository repositorioFrequencia;

		public TurmaService(IProvaRepository repositorioProva, IFrequenciaRepository repositorioFrequencia)
		{
			this.repositorioProva = repositorioProva;
			this.repositorioFrequencia = repositorioFrequencia;
		}

		public DTOProvaTurma ObtemProvaDaTurmaPor(string disciplina, string turma, short ano, short periodo, short subperiodo)
		{
			Prova prova = repositorioProva.ObtemPor(disciplina, turma, ano, periodo, subperiodo);
			DTOProvaTurma dtoProvaTurma = null;

			if (prova != null)
				dtoProvaTurma = new DTOProvaTurma { TipoProva = prova.TipoProva };

			return dtoProvaTurma;
		}

		public DTOFrequenciaTurma ObtemFrequenciaDaTurmaPor(string disciplina, string turma, short ano, short periodo, short subperiodo)
		{
			DTOFrequenciaTurma dtoFrequenciaDaTurma = null;

			Frequencia frequenciaDaTurma = repositorioFrequencia.ObtemFrequenciaPor(disciplina, turma, ano, periodo, subperiodo);
			if (frequenciaDaTurma != null)
				dtoFrequenciaDaTurma = new DTOFrequenciaTurma
				{
					CodigoFrequencia = frequenciaDaTurma.TipoFrequencia,
					Descricao = frequenciaDaTurma.Descricao,
					AulasDadas = frequenciaDaTurma.AulasDadas,
					AulasPrevistas = frequenciaDaTurma.AulasPrevistas
				};

			return dtoFrequenciaDaTurma;
		}

		public DTOProvaParaLancamento ObtemProvaDaTurmaParaLancamentoPor(string disciplina, string turma, short ano, short periodo, short subperiodo)
		{
			Prova prova = repositorioProva.ObtemPor(disciplina, turma, ano, periodo, subperiodo);
			DTOProvaParaLancamento dtoProvaParaLancamento = null;

			if (prova != null)
				dtoProvaParaLancamento = new DTOProvaParaLancamento
				{
					TipoProva = prova.TipoProva,
					Nome = prova.Nome,
					NotaMaxima = prova.NotaMaxima,
					Ordem = prova.Ordem
				};

			return dtoProvaParaLancamento;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Repository;
using Proderj.DOL.Domain;
using Proderj.DOL.Exception;

namespace Proderj.DOL.Service
{
	public class DisciplinaService : IDisciplinaService
	{
		private IDisciplinaRepository repositorioDisciplina;

		public DisciplinaService(IDisciplinaRepository repositorioDisciplina)
		{
			this.repositorioDisciplina = repositorioDisciplina;
		}

		public DTOConfiguracaoNotaDisciplina ObtemConfiguracaoNotaPor(string codigoDisciplina)
		{
			Disciplina disciplina = repositorioDisciplina.ObtemConceitosPor(codigoDisciplina);

			if (disciplina == null)
				throw new DisciplinaException(DisciplinaException.TipoEnum.DisciplinaInexistenteParaCodigoInformado);

			DTOConfiguracaoNotaDisciplina dtoConfigucacao = new DTOConfiguracaoNotaDisciplina
			{
				CasasDecimais = disciplina.QuantCasasDecimais,
				GrupoNota = disciplina.GrupoNota,
				NotaMaxima = disciplina.NotaMaxima,
                TemNota = disciplina.TemNota,
                TemFrequencia = disciplina.TemFrequencia
			};

			return dtoConfigucacao;
		}

		
		public string ObtemDescricaoDisciplinaPor(string codigoDisciplina)
		{
			Disciplina disciplina = repositorioDisciplina.ObtemDescricaoPor(codigoDisciplina);

			if (disciplina == null)
				throw new DisciplinaException(DisciplinaException.TipoEnum.DisciplinaInexistenteParaCodigoInformado);

			return disciplina.DescricaoCompleta;
		}

        public Disciplina ObtemDisciplina(string codigoDisciplina)
        {
            Disciplina disciplina = repositorioDisciplina.ObtemPorChavePrimaria(codigoDisciplina);

            if (disciplina == null)
                throw new DisciplinaException(DisciplinaException.TipoEnum.DisciplinaInexistenteParaCodigoInformado);

            return disciplina;
        }
	}
}

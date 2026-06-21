using System;

namespace Proderj.DOL.Service
{
	public class DTOSelecaoTurmas
	{
		public string UnidadeEnsino { get; set; }

		public string NomeCompletoUnidadeEnsino { get; set; }

		public string Disciplina { get; set; }

		public string Turma { get; set; }

		public short Ano { get; set; }

		public short Semestre { get; set; }

		public string NomeCompletoDisciplina { get; set; }

		public bool ValidoParaLancamento { get; set; }

		public bool PossuiNotasPendentes { get; set; }

		public string StatusLancamentoLiberado { get; set; }

		public string StatusLancamentoAguardando { get; set; }

		public string StatusLancamentoBloqueado { get; set; }

		public string Curso { get; set; }

		public string Modalidade { get; set; }

		public string Tipo { get; set; }

		public short Serie { get; set; }

        public bool RegistroProva { get; set; }

        public bool RegistroFrequencia { get; set; }

        public bool RegistroProvaFrequencia
        {
            get
            {
                return (RegistroProva || RegistroFrequencia);
            }
        }

        public bool PossuiFaltasPendentes { get; set; }
	}
}

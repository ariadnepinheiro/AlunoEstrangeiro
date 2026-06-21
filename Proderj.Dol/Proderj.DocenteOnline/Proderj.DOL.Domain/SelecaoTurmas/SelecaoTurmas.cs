using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
    public class SelecaoTurmas
    {
        public SelecaoTurmas()
        { 
		}

        public virtual short Ano { get; set; }
        
        public virtual short Semestre { get; set; }
        
        public virtual string NomeCompletoDisciplina { get; set; }
        
        public virtual bool ValidoParaLancamento { get; set; }
        
        public virtual bool PossuiNotasPendentes { get; set; }

        public virtual string StatusLancamentoLiberado { get; set; }

		public virtual string StatusLancamentoAguardando { get; set; }

		public virtual string StatusLancamentoBloqueado { get; set; }

        public virtual string Curso { get; set; }

        public virtual string Modalidade { get; set; }

        public virtual string Tipo { get; set; }

        public virtual short Serie { get; set; }

		public virtual string UnidadeEnsino { get; set; }

		public virtual string NomeCompletoUnidadeEnsino { get; set; }

		public virtual string Disciplina { get; set; }

		public virtual string Turma { get; set; }

        public virtual bool RegistroProva { get; set; }

        public virtual bool RegistroFrequencia { get; set; }

        public virtual bool PossuiFaltasPendentes { get; set; }
    }
}

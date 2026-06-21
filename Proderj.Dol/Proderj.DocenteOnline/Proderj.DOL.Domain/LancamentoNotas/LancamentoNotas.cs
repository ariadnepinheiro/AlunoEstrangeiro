using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
    public class LancamentoNotas
    {
		public LancamentoNotas()
        { 
		}
        public virtual int NotaId { get; set; }

		public virtual bool RecuperacaoPararela { get; set; }

		public virtual bool SemAvaliacao { get; set; }

		public virtual short Ano { get; set; }

		public virtual short? Faltas { get; set; }

		public virtual decimal? MediaNota { get; set; }

		public virtual double? NotaMaxima { get; set; }

		public virtual string NumeroChamada { get; set; }

		public virtual string NomeCompleto { get; set; }

		public virtual string SituacaoMatricula { get; set; }

		public virtual string DescricaoSituacao { get; set; }

		public virtual string CodigoJustificativa { get; set; }

		public virtual string MatriculaAluno { get; set; }

		public virtual string Disciplina { get; set; }

		public virtual string Turma { get; set; }

		public virtual string Semestre { get; set; }

		public virtual string NomeProva { get; set; }

		public virtual string Formula { get; set; }

        public virtual decimal? NotaRecuperacao { get; set; }

        public virtual decimal? NotaProva { get; set; }

        public virtual short? MotivoSemNota { get; set; }

        public virtual bool PossuiLicenca { get; set; }
    }
}

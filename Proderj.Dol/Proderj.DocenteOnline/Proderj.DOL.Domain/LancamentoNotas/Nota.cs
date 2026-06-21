using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Proderj.DOL.Domain
{
    public enum MotivoSemNotaEnum
    {
        [Description("Aluno em Progressão Parcial (Dependência) não apresentou trabalho")]
        AlunoEmProgressaoNaoApresentouTrabalho = 0,
        [Description("Afastamento médico / Maternidade / Serviço  Militar")]
        AfastamentoMedicoMaternidadeServicoMilitar = 1,
        [Description("Outros")]
        Outros = 2
    }

    public class Nota
    {
        public Nota()
        { 
		}

		public virtual int Id { get; set; }
		
		public virtual int? Formulario { get; set; }

		public virtual short Ano { get; set; }

		public virtual short Semestre { get; set; }

		public virtual short? Ordem { get; set; }

		public virtual char? RecuperacaoParalela { get; set; }

		public virtual char? SemAvaliacao { get; set; }

		public virtual char? Compareceu { get; set; }

		public virtual string Turma { get; set; }

		public virtual string Disciplina { get; set; }

        public virtual string Aluno { get; set; }

        public virtual string Conceito { get; set; }

		public virtual string Justificativa { get; set; }

		public virtual string TipoProva { get; set; }

        public virtual DateTime? DataProva { get; set; }

        public virtual decimal? NotaProva { get; set; }

        public virtual decimal? NotaRecuperacao { get; set; }

        public virtual short? MotivoSemNota { get; set; }
    }
}

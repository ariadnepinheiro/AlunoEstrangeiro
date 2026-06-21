using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
	public class LogNota
	{
		public LogNota()
		{ 
		}

		public virtual int Id { get; set; }

		public virtual int Operacao { get; set; }

		public virtual short Ano { get; set; }
		
		public virtual short Semestre { get; set; }
		
		public virtual string Prova { get; set; }
		
		public virtual string ValorAnterior { get; set; }
		
		public virtual string ValorAtual { get; set; }
		
		public virtual string RecuperacaoParalelaAnterior{ get; set; }

		public virtual string RecuperacaoParalelaAtual { get; set; }
		
		public virtual string SemAvaliacaoAnterior { get; set; }

		public virtual string SemAvaliacaoAtual { get; set; }
		
		public virtual string JustificativaAnterior { get; set; }
		
		public virtual string JustificativaAtual { get; set; }
		
		public virtual string Usuario { get; set; }
		
		public virtual string Aluno { get; set; }

		public virtual string Disciplina { get; set; }

		public virtual string Turma { get; set; }
		
		public virtual DateTime? DataOperacao { get; set; }
	
	}	
}

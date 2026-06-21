using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
    public enum TipoDeclaracaoSemNotaEnum
    {
        AvisoEntregaTrabalho = 0,
        AvaliacaoResidencial = 1,
        FichaFaltasAluno = 2
    }

    public class DeclaracaoSemNota
    {
        public DeclaracaoSemNota()
        { 
		}

		public virtual int Id { get; set; }
        public virtual int NotaId { get; set; }
        public virtual short TipoDeclaracaoSemNota { get; set; }
        public virtual String Matricula { get; set; }
        public virtual DateTime DataCadastro { get; set; }
    }
}

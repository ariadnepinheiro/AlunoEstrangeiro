using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Domain;
using Proderj.Foundation.Common;

namespace Proderj.DOL.Service
{
	public class DTONota
	{
        public int Id { get; set; }

		public string CodigoAluno { get; set; }

		public decimal? Nota { get; set; }

		public string CodigoDisciplina { get; set; }

		public string CodigoTurma { get; set; }

		public short Ano { get; set; }

		public short Periodo { get; set; }

		public string TipoProva { get; set; }

		public short Ordem { get; set; }

		public bool RecuperacaoParalela { get; set; }
		
		public bool SemAvaliacao { get; set; }
		
		public String CodigoJustificativa { get; set; }

        public decimal? NotaProva { get; set; }

        public decimal? NotaRecuperacao { get; set; }

        public String DescricaoJustificativa
        {
            get
            {
                return String.IsNullOrEmpty(CodigoJustificativa) ? null : ((MotivoSemNotaEnum)Convert.ToInt16(CodigoJustificativa)).GetDescription();
            }
        }
	}
}

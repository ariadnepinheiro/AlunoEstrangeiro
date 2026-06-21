using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Domain;
using Proderj.Foundation.Common;

namespace Proderj.DOL.Service
{
	public class DTOItemLancamentoNotaFrequenciaAluno
	{
        public int Id { get; set; }
        public bool SomenteLeitura { get; set; }

		public string Codigo { get; set; }
		public string Nome { get; set; }
		public string Situacao { get; set; }
		public string DescricaoSituacao { get; set; }

		public decimal? Nota { get; set; }
		public short? Faltas { get; set; }

		public bool RecuperacaoParalela { get; set; }
		public bool SemAvaliacao { get; set; }


		public string CodigoJustificativa { get; set; }

		public bool SituacaoMatriculado
		{
			get
			{
				return Situacao == "Matriculado";
			}
		}

        public decimal? NotaProva { get; set; }

        public decimal? NotaRecuperacao { get; set; }
        public bool PossuiLicenca { get; set; }
        public bool ExibeMensagemAfastamentoMedico { get; set; }
        public float PresencaMinima { get; set; }

        public String DescricaoJustificativa
        {
            get
            {
                return String.IsNullOrEmpty(CodigoJustificativa) ? null : ((MotivoSemNotaEnum)Convert.ToInt16(CodigoJustificativa)).GetDescription();
            }
        }
	}
}

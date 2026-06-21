using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Service
{
	public class DTOCadastroGlp_VerificaPermissaoParaInsercaoDocenteDisponivel
	{
		public short DiaSemana { get; set; }

		public string Agrupamento { get; set; }

		public DateTime HoraFinal { get; set; }

		public DateTime HoraInicio { get; set; }

		public int DocenteDisponivelGlpId { get; set; }

		public string CodigoMunicipio { get; set; }

		public long NumeroFuncionario { get; set; }

		public string CodigoUnidadeEnsino { get; set; }

        public int CodigoRegional { get; set; }
	}
}

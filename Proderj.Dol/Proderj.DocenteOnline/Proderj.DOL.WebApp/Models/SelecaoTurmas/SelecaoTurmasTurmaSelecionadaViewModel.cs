using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proderj.DOL.WebApp.Models
{
	public class SelecaoTurmasTurmaSelecionadaViewModel
	{
		public string DescricaoUnidadeEnsino { get; set; }
		public short Ano { get; set; }
		public short Periodo { get ; set; }
		public string CodigoTurma { get; set; }
		public string DescricaoDisciplina { get; set; }
		public string NomeDocente { get; set; }
		public string MatriculaDocente { get; set; }
		public string IdFuncional { get; set; }
		public string Vinculo { get; set; }
	}
}

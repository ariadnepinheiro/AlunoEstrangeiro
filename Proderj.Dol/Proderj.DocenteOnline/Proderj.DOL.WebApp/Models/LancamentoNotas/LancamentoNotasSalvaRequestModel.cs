using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Proderj.DOL.Service;
using Proderj.DOL.WebApp.Models;

namespace Proderj.DOL.WebApp
{
	public class LancamentoNotasSalvaRequestModel : ViewModelPadrao
	{
		[Required]
		public string CodigoCurso { get; set; }

		[Required]
		public string TipoCurso { get; set; }

		[Required]
		public string CodigoUnidadeEnsino { get; set; }

		[Required]
		public short Ano { get; set; }

		[Required]
		public short Periodo { get; set; }

		[Required]
		public short Serie { get; set; }

		[Required]
		public string CodigoTurma { get; set; }

		[Required]
		public string CodigoDisciplina { get; set; }

		[Required]
		public string CodigoModalidade { get; set; }

		[Required]
		public short Subperiodo { get; set; }

		[Required]
		public DTOFrequenciaTurma DadosFrequenciaTurma { get; set; }

		public List<DTOItemSalvaNotaFrequenciaAluno> ListaItemLancamentoNotaFrequenciaAluno { get; set; }

        public String ListaMaterialEstudo { get; set; }
	}
}

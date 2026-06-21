using System;
using System.ComponentModel.DataAnnotations;

namespace Proderj.DOL.WebApp.Models
{
	public class RespostaCurriculoMinimoListaRequestModel : ICurriculoMinimoListaPrerequisito, ISelecaoTurmasTurmaSelecionadaRequestModel
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

		public short? Subperiodo { get; set; }

		[Required]
		public short Serie { get; set; }

		[Required]
		public string CodigoTurma { get; set; }

		[Required]
		public string CodigoDisciplina { get; set; }

		[Required]
		public string CodigoModalidade { get; set; }
		
		public long NumeroFuncionarioDocente { get; set; }
	}
}
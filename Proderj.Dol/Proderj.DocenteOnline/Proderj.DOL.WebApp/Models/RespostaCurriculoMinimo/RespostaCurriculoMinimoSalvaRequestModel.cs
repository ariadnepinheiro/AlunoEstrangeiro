using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Proderj.DOL.WebApp.Models
{
	public class RespostaCurriculoMinimoSalvaRequestModel : ISelecaoTurmasTurmaSelecionadaRequestModel
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
		public short Subperiodo { get; set; }

		[Required]
		public short Serie { get; set; }

		[Required]
		public string CodigoTurma { get; set; }

		[Required]
		public string CodigoDisciplina { get; set; }

		[Required]
		public string CodigoModalidade { get; set; }

		[Required]
		public List<Resposta> ListaResposta { get; set; }

		public class Resposta
		{
			[Required]
			public int Codigo { get; set; }

			[Required]
			public int CodigoGrupo { get; set; }

			[Required]
			public bool? Respondido { get; set; }
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Proderj.DOL.WebApp.Models;

namespace Proderj.DOL.WebApp.Controllers
{
	public class AvaliacaoCurriculoMinimoListaRequestModel : ICurriculoMinimoListaPrerequisito, ISelecaoTurmasTurmaSelecionadaRequestModel
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

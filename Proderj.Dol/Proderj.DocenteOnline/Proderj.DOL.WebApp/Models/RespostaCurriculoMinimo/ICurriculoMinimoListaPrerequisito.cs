using System.ComponentModel.DataAnnotations;

namespace Proderj.DOL.WebApp.Models
{
	public interface ICurriculoMinimoListaPrerequisito
	{
		[Required]
		string CodigoCurso { get; set; }

		[Required]
		string TipoCurso { get; set; }

		[Required]
		string CodigoUnidadeEnsino { get; set; }

		[Required]
		short Ano { get; set; }

		[Required]
		short Periodo { get; set; }

		short? Subperiodo { get; set; }

		[Required]
		short Serie { get; set; }

		[Required]
		string CodigoTurma { get; set; }

		[Required]
		string CodigoDisciplina { get; set; }

		[Required]
		string CodigoModalidade { get; set; }

		long NumeroFuncionarioDocente { get; set; }
	}
}
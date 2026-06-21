namespace Proderj.DOL.WebApp.Models
{
	public interface ISelecaoTurmasTurmaSelecionadaRequestModel
	{
		string CodigoUnidadeEnsino { get; set; }
		short Ano { get; set; }
		short Periodo { get; set; }
		short Serie { get; set; }
		string CodigoTurma { get; set; }
		string CodigoDisciplina { get; set; }
	}
}
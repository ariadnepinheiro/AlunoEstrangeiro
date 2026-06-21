namespace Proderj.DOL.Service
{
	public abstract class DTOCurriculoMinimo_VerificaPermissaoBase
	{
		public string CodigoCurso { get; set; }
		public string TipoCurso { get; set; }
		public string CodigoUnidadeEnsino { get; set; }
		public short Ano { get; set; }
		public short Periodo { get; set; }
		public short Subperiodo { get; set; }
		public short Serie { get; set; }
		public string CodigoTurma { get; set; }
		public string CodigoDisciplina { get; set; }
		public string CodigoModalidade { get; set; }
	}
}
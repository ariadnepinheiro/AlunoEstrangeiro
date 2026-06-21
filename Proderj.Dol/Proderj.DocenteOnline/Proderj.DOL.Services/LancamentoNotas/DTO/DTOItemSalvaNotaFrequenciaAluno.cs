namespace Proderj.DOL.Service
{
	public class DTOItemSalvaNotaFrequenciaAluno
	{
        public int Id { get; set; }
		public string Codigo { get; set; }
		public string Nome { get; set; }
		public decimal? Nota { get; set; }
		public short? Faltas { get; set; }
		public bool RecuperacaoParalela { get; set; }
		public bool SemAvaliacao { get; set; }
		public string CodigoJustificativa { get; set; }
        public decimal? NotaProva { get; set; }
        public decimal? NotaRecuperacao { get; set; }
	}
}
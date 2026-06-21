namespace Proderj.DOL.Service
{
	public class DTORespostaCurriculoMinimo_RespostasPorGrupo
	{
		public string DescricaoGrupo { get; set; }
		public string DescricaoResposta { get; set; }
		public int CodigoGrupo { get; set; }
		public int CodigoResposta { get; set; }
		public bool Respondido { get; set; }

		public short OrdemGrupo { get; set; }

		public short OrdemResposta { get; set; }
	}


}
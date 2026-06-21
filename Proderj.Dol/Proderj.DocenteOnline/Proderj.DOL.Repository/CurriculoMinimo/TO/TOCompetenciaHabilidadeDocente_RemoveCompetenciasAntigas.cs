using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Repository
{
	public class TOCompetenciaHabilidadeDocente_RemoveCompetenciasAntigas
	{
		public string Matricula { get; set; } 
		
		public string CodigoTurma { get; set; } 
		
		public string CodigoDisciplina { get; set; } 
		
		public short Ano { get; set; } 
		
		public short Periodo { get; set; } 
		
		public short SubPeriodo { get; set; } 
	}
}

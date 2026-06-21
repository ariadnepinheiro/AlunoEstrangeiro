using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Service
{
	public class DTORespostaCurriculoMinimo_ObtemStatusPreenchimentoPor
	{
		public short Ano { get ; set; }
        public short Periodo { get ; set; }
        public short Subperiodo { get ; set; }
        public short Serie { get ; set; }

        public string CodigoModalidade { get ; set; }
        public string TipoCurso { get ; set; }
        public string CodigoCurso { get ; set; }
        public string CodigoDisciplina { get ; set; }
        public string CodigoTurma { get ; set; }
		public string MatriculaDocente { get; set; }
	}
}

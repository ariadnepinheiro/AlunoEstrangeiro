using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Service
{
    public class DTOTurmaMaterialEstudo
    {
        public int TurmaMaterialEstudoId { get; set; }
        public int MaterialEstudoId { get; set; }
        public decimal Subperiodo { get; set; }
        public string Disciplina { get; set; }
        public string Semestre { get; set; }
        public string Ano { get; set; }
        public string Turma { get; set; }
    }
}

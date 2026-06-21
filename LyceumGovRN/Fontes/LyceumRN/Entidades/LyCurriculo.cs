using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.Entidades
{
    public class LyCurriculo
    {
        public string Curriculo { get; set; }

        public string Curso { get; set; }

        public string Turno { get; set; }

        public int AnoIni { get; set; }

        public int SemIni { get; set; }

        public DateTime DtExtincao { get; set; }        

        public string LinguaEstrangeira { get; set; }

        public string EnsinoReligioso { get; set; }
    }
}

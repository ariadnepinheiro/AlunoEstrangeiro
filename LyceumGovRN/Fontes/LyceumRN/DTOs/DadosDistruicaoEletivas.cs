using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosDistruicaoEletivas
    {
        /// <summary>
        /// Turma regular usada como referencia
        /// </summary>
        public string TurmaReferencia { get; set; }

        public string DescricaoTurmaReferencia { get; set; } 

        public int Ano { get; set; }

        public int Semestre { get; set; }

        public int Serie { get; set; }

        public string DisciplinaGrupo1 { get; set; }

        public string TurmaEletivaGrupo1 { get; set; }

        public string DisciplinaGrupo2 { get; set; }

        public string TurmaEletivaGrupo2 { get; set; }

        public string DisciplinaGrupo3 { get; set; }

        public string TurmaEletivaGrupo3 { get; set; }

        public bool?  Referencia { get; set; }
    }
}

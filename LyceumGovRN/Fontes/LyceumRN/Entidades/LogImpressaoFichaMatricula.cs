using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.Entidades
{
    public class LogImpressaoFichaMatricula
    {
        public int LogImpressaoFichaMatriculaId { get; set; }

        public string AlunoId { get; set; }

        public int ConfirmacaoMatriculaId { get; set; }

        public string Matricula { get; set; }

        public string DataImpressao { get; set; }
    }
}

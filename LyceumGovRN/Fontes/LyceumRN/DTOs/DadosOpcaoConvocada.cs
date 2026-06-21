using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosOpcaoConvocada
    {
        public int InscricaoAlunoId { get; set; }

        public int OpcaoInscricaoId { get; set; }

        public int ControleVagaId { get; set; }

        public string Municipio { get; set; }

        public DateTime PrazoReposta { get; set; }
    }
}

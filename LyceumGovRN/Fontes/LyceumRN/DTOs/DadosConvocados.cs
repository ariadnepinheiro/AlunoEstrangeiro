using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosConvocados
    {
        public int OpcaoInscricaoId { get; set; }

        public int InscricaoAlunoId { get; set; }

        public string Email { get; set; }

        public string Nome { get; set; }       

        public int Serie { get; set; }

        public string Segmento { get; set; }

        public string Escola { get; set; }

        public string MunicipioEscola { get; set; }

        public DateTime PrazoFinal { get; set; }

        public DateTime DataConvocacao { get; set; }

        public DateTime? DataRetorno { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosOpcaoCandidato
    {
        public int OpcaoInscricaoId { get; set; }

        public int InscricaoAlunoId { get; set; }

        public int PreCadastroAlunoId { get; set; }

        public decimal? Pessoa { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public string Celular { get; set; }

        public string FixoCelular { get; set; }

        public DateTime? DataConvocacao { get; set; }
    }
}

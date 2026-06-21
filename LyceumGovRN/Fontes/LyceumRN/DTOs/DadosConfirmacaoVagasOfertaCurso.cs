using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosConfirmacaoVagasOfertaCurso
    {
        public int ConfirmacaoOfertaId { get; set; }

        public string Curso { get; set; }

        public int Serie { get; set; }

        public string NomeCurso { get; set; }

        public int VagasManha { get; set; }

        public bool HabilitaManha { get; set; }

        public int VagasTarde { get; set; }

        public bool HabilitaTarde { get; set; }

        public int VagasNoite { get; set; }

        public bool HabilitaNoite { get; set; }

        public int VagasIntegral { get; set; }

        public bool HabilitaIntegral { get; set; }

        public int QuantidadeOptantes { get; set; }
    }
}

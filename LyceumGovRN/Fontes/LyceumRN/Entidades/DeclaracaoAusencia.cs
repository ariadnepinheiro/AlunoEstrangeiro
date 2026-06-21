using System;

namespace Techne.Lyceum.RN.Entidades
{
    public class DeclaracaoAusencia
    {
        public int DeclaracaoAusenciaId { get; set; }

        public string AlunoId { get; set; }

        public int TipoDeclaracaoAusenciaId { get; set; }

        public string Matricula { get; set; }

        public DateTime DataCadastro { get; set; }

        public string Motivo { get; set; }
    }
}

using System;

namespace Techne.Lyceum.RN.TurnosVagas.Entidades
{
    public class ResultadoAnalise
    {
        public int ResultadoAnaliseId { get; set; }

        public string Descricao { get; set; }

        public bool Ativo { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAlteracao { get; set; }

        public string Matricula { get; set; }
    }
}

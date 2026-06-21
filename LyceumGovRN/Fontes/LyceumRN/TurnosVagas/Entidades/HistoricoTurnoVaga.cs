using System;

namespace Techne.Lyceum.RN.TurnosVagas.Entidades
{
    public class HistoricoTurnoVaga
    {
        public int HistoricoTurnoVagaId { get; set; }

        public int Ano { get; set; }

        public int Periodo { get; set; }

        public string Censo { get; set; }

        public int TipoHistoricoId { get; set; }

        public int PossuiHistoricoTurno { get; set; }

        public int PossuiHistoricoVaga { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAlteracao { get; set; }

        public string Matricula { get; set; }
    }
}

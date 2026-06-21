using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.Agenda.Entidades
{
    public class ParametroTurnoVaga
    {
        public int ParametroTurnoVagaId { get; set; }

        public int AlterarTurnoModalidade { get; set; }

        public int VariacaoTurno { get; set; }

        public decimal PercentualAumentoVaga { get; set; }

        public decimal PercentualDiminuicaoVaga { get; set; }

        public decimal PercentualCriacaoTurma { get; set; }

        public int PerfilId { get; set; }

        public int AgendaId { get; set; }

        public bool RemoveTurnoInteiro { get; set; }

        public bool EditarTurnoFinalizado { get; set; }

        public bool  ConfiguracaoPadrao { get; set; }

        public bool EditarVagaFinalizada { get; set; }

        public bool PodeAnalisar { get; set; }

        public bool PodeTurmaProvisoria { get; set; }

        public bool PossuiLimiteTurmaProvisoria { get; set; }
    }
}

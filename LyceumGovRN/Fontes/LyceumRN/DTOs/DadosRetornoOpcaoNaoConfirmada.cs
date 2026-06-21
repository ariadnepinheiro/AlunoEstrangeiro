using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosRetornoOpcaoNaoConfirmada
    {
        public int InscricaoAlunoId { get; set; }

        public int OpcaoInscricaoId { get; set; }

        public int Fase { get; set; }

        public int ControleVagaId { get; set; }

        public DateTime DataRetorno { get; set; }

        public DateTime NovoPrazoReposta { get; set; }

        public int MotivoRetornoId { get; set; }

        public string UsuarioId { get; set; }
    }
}

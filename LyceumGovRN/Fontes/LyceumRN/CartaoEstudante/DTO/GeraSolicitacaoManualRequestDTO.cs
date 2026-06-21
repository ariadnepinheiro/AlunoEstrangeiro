using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.CartaoEstudante.DTO
{
    [Serializable]
    public class SolicitacaoManualRequestDTO
    {
        public int OperadoraId { get; set; }

        public int TipoSolicitacaoId { get; set; }

        public string Aluno { get; set; }

        public string Motivo { get; set; }

        public string Usuario { get; set; }
    }
}

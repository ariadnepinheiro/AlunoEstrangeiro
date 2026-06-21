using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.Patrimonio.Entidades
{
    public class Transferencia
    {
        public int TransferenciaId { get; set; }

        public string SetorOrigem { get; set; }

        public string SetorDestino { get; set; }

        public string UsuarioSolicitanteId { get; set; }

        public DateTime DataSolicitacao { get; set; }

        public string UsuarioAndamentoId { get; set; }

        public DateTime? DataAndamento { get; set; }

        public DateTime? DataMovimentacao { get; set; }
    }
}
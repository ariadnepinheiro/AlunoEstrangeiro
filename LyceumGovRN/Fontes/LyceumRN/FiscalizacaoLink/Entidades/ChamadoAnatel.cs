using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.FiscalizacaoLink.Entidades
{
    public class ChamadoAnatel
    {
        public int ChamadoAnatelId { get; set; }

        public int CircuitoSetorId { get; set; }

        public string NumeroOperadora { get; set; }

        public DateTime DataOperadora { get; set; }

        public string NumeroAnatel { get; set; }

        public DateTime DataAnatel { get; set; }

        public DateTime? DataResolucao { get; set; }

        public string Severidade { get; set; }

        public string UsuarioId { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAlteracao { get; set; }
    }
}

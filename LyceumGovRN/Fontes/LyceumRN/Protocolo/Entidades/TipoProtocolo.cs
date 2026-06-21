using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.Protocolo.Entidades
{
    public class TipoProtocolo
    {
        public int TipoProtocoloId { get; set; }

        public string Descricao { get; set; }

        public bool Ativo { get; set; }

        public string UsuarioId { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAlteracao { get; set; }
    }
}

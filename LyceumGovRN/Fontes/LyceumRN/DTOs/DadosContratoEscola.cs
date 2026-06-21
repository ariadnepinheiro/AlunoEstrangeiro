using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosContratoEscola
    {
        public int ContratoId { get; set; }

        public int ContratoSetorId { get; set; }

        public int ContratoOperadoraId { get; set; } 

        public string UnidadeAdministrativa { get; set; }

        public int TipoLinkId { get; set; } 

        public string NumeroContrato { get; set; }

        public string Descricao { get; set; }

        public DateTime? DataContratacao { get; set; }

        public DateTime? DataImplantacao { get; set; }

        public DateTime? DataTermino { get; set; }

        public int OperadoraId { get; set; }

        public string UsuarioId { get; set; }
    }
}

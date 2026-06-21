using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.Cadastros.DTOs
{
    public class MaeDadosAnalise
    {
        public int MaeInscricaoId { get; set; }

        public bool Habilitado { get; set; }

        public int? MaeMotivoNaoHabilitadoId { get; set; }

        public string Censo { get; set; }

        public DateTime? DataInicio { get; set; }

        public string UsuarioId { get; set; }
    }
}

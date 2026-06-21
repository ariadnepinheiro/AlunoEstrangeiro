using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.CartaoEstudante.DTO
{          
    public class SolicitacaoManualResponseDTO
    {
        public bool Inseriu{ get; set; }
        
        public bool DadosValidos { get; set; }

        public bool PodeForcarGeracao { get; set; }

        public string MensagemErro { get; set; }
 
    }
}

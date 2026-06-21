using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosBaixaBemPatrimonial
    {
        public int BemId { get; set; }

        public bool Baixa { get; set; }

        public int MotivoBaixaId { get; set; }

        public DateTime DataBaixa { get; set; }

        public string ProcessoBaixa { get; set; }

        public string JustificativaBaixa { get; set; }

        public string BoletimOcorrencia { get; set; }

        public string InstituicaoDestino { get; set; }

        public string CnpjInstituicaoDestino { get; set; }

        public string UsuarioId { get; set; }

        public string PrefixoProcesso { get; set; } 
    }
}

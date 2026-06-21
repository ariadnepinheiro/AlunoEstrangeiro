using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.CartaoEstudante.DTO
{
    public class ControleProcessamentoDTO
    {
        public string NomeProcesso { get; set; }
        public DateTime DataAgenda { get; set; }
        public string Frequencia { get; set; }
        public string Usuario { get; set; }
        public DateTime DtIniMov { get; set; }
        public DateTime DtFimMov { get; set; }
        
        public LogControleProcessamentoDTO LogControleProcessamento { get; set; }

        public ControleProcessamentoDTO()
        {
            LogControleProcessamento = new LogControleProcessamentoDTO();
        }
    }
}

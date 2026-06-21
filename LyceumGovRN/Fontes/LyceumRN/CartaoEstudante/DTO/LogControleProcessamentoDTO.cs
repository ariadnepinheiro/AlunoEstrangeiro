using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.CartaoEstudante.DTO
{
    public class LogControleProcessamentoDTO
    {
        public DateTime DtProc { get; set; }
        public int RegInicial { get; set; }
        public int RegFinal { get; set; }
        public long QtdRegsRetornados { get; set; }
        public long UltRegProc { get; set; }
        public string StProc { get; set; }                
    }
}

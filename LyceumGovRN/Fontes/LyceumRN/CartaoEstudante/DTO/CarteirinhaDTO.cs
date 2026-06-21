using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.CartaoEstudante.DTO
{
    public class CarteirinhaDTO
    {
        public int DUPLICIDADEID { get; set; }
        public int VIA_CARTEIRINHA { get; set; }
        public long COD_BARRAS_CARTEIRINHA { get; set; }
        public string SIT_CARTEIRINHA { get; set; }
        public string MOTIVO { get; set; }
        public DateTime DT_SOLICITACAO { get; set; }
        public string USUARIO { get; set; }
        public DateTime DT_IMPRESSAO { get; set; }
        public DateTime DATA_ALT_SITUACAO { get; set; }
        public string NUMEROCARTAO { get; set; }    
        public string NUMEROLOTE { get; set; }
        public DateTime  DATAUTILIZACAO { get; set; }
        public string LOCALIMPRESSAO { get; set; }  
        public DateTime  DATAENTREGALOTE { get; set; }
        public DateTime  DATACONFIRMACAOENTREGA { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.PrestacaoContas.DTOs
{
    public class DadosXmlItem
    {
        //<xs:element name="cProd">
        public string cProd { get; set; }

        //<xs:element name="cEAN"> //NUMEROITEM, 
        public string cEAN { get; set; }

        //<xs:element name="xProd"> //DESCRICAO
        public string xProd { get; set; }

        //<xs:element name="NCM"> //NCM
        public string NCM { get; set; }

        //<xs:element name="qCom" type="TDec_1104v"> //QUANTIDADE
        public decimal qCom { get; set; }

        //<xs:element name="vUnCom" type="TDec_1110v"> //VALORUNITARIO,
        public decimal vUnCom { get; set; }
    }
}

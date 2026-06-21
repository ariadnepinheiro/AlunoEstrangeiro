using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN
{
    public class EDespesa
    {
        public decimal?     ID                  { get; set; }
        public DateTime?    Data                { get; set; }
        public String       NumeroComprovante   { get; set; }
        public String       TipoComprovante     { get; set; }
        public decimal?     Desconto            { get; set; }
        public decimal?     Valor               { get; set; }
        public decimal?     IDFinalidade        { get; set; }
        public decimal?     IDCompra            { get; set; }

        public decimal      IDPrestacaoContas   { get; set; }  
        public String       TipoOperacao        { get; set; }        

        public void OnValidateFieldValue()
        {
            if (!Data.HasValue)
                throw new Exception("A Data da nota fiscal deve ser informada");

            if (NumeroComprovante.Length == 0 || String.IsNullOrEmpty(NumeroComprovante))
                throw new Exception("O Número do Comprovante deve ser informado");

            if (TipoComprovante.Length == 0  || String.IsNullOrEmpty(TipoComprovante))
                throw new Exception("O Tipo de Comprovante deve ser informado");            
        }
    }
}

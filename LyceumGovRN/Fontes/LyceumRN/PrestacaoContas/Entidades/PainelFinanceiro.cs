using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    
    public class PainelFinanceiro
    {        
        public string Censo { get; set; }
        
        public DateTime? Data { get; set; }

        public String DataInvalida { get; set; }

        public string PeriodoReferencia { get; set; }
        
    }
}

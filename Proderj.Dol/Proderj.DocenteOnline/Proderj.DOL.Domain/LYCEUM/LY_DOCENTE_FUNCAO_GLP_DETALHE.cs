using System;
using System.Text;
using System.Collections.Generic;


namespace Proderj.DOL.Domain {
    
    public class LY_DOCENTE_FUNCAO_GLP_DETALHE {
        public virtual double ID_DOCENTE_FUNCAO_GLP_DETALHE { get; set; }
        public virtual double ID_DOCENTE_FUNCAO_GLP { get; set; }
        public virtual DateTime DATA { get; set; }
        public virtual string STATUS { get; set; }
        public virtual decimal? QTD_GLP { get; set; }
        public virtual string USUARIO { get; set; }
        public virtual string MOTIVO { get; set; }
        
    }
}

using System;
using System.Text;
using System.Collections.Generic;


namespace Proderj.DOL.Domain {
    
    public class LY_CH_CATEGORIA {
        public virtual double ID_CH_FUNCAO_DOCENTE { get; set; }
        public virtual string CATEGORIA_DOCENTE { get; set; }
        public virtual string FUNCAO { get; set; }
        public virtual double NR_MATRICULAS { get; set; }
        public virtual string READAPTADO { get; set; }
        public virtual string GLP { get; set; }
        public virtual decimal CH_SEMANAL_TOTAL { get; set; }
        public virtual decimal? CH_SEMANAL_EFETIVA { get; set; }
        public virtual int? CH_GRUPO { get; set; }
        public virtual string CATEGORIA_DOCENTE_2 { get; set; }
        public virtual int? CH_GRUPO_2 { get; set; }
        public virtual string FUNCAO_2 { get; set; }
        public virtual int? CH_GLP { get; set; }
        public virtual string USUARIOID { get; set; }
        public virtual DateTime? DATACADASTRO { get; set; }
        public virtual DateTime? DATAALTERACAO { get; set; }
    }
}

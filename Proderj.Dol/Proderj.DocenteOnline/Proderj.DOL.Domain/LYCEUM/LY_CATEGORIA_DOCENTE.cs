using System;
using System.Text;
using System.Collections.Generic;


namespace Proderj.DOL.Domain {
    
    public class LY_CATEGORIA_DOCENTE {
        public LY_CATEGORIA_DOCENTE() {
            LY_DOCENTE = new List<LY_DOCENTE>();
        }
        public virtual string CATEGORIA { get; set; }
        public virtual string NOME { get; set; }
        public virtual string FUNCAO { get; set; }
        public virtual string INGRESSO { get; set; }
        public virtual string NECESSITA_SUPERIOR { get; set; }
        public virtual string TIPO { get; set; }
        public virtual string USUARIOID { get; set; }
        public virtual DateTime? DATACADASTRO { get; set; }
        public virtual DateTime? DATAALTERACAO { get; set; }
        public virtual int? AGRUPAMENTOCARGOSID { get; set; }
        public virtual int? CARGAHORARIACOMPLEMENTACAO { get; set; }
        public virtual int? CARGAHORARIAREGENCIA { get; set; }
        public virtual int? CARGAHORARIAPLANEJAMENTO { get; set; }
        public virtual ICollection<LY_DOCENTE> LY_DOCENTE { get; set; }
    }
}

using System;
using System.Text;
using System.Collections.Generic;


namespace Proderj.DOL.Domain {
    
    public class DISPONIBILIDADEGLP {
        public DISPONIBILIDADEGLP()
        {
			DISPONIBILIDADEGLP_DIASEMANA = new List<DISPONIBILIDADEGLP_DIASEMANA>();
			DISPONIBILIDADEGLP_MODALIDADE = new List<DISPONIBILIDADEGLP_MODALIDADE>();
			DISPONIBILIDADEGLP_TURNO = new List<DISPONIBILIDADEGLP_TURNO>();
			DISPONIBILIDADEGLP_UNIDADEENSINO = new List<DISPONIBILIDADEGLP_UNIDADEENSINO>();
        }
        public virtual int DISPONIBILIDADEGLPID { get; set; }
        public virtual LY_GRUPO_HABILITACAO LY_GRUPO_HABILITACAO { get; set; }
        public virtual LY_DOCENTE LY_DOCENTE { get; set; }
        public virtual int ANO { get; set; }
        public virtual string USUARIOID { get; set; }
        public virtual DateTime DATACADASTRO { get; set; }
        public virtual DateTime DATAALTERACAO { get; set; }
        public virtual ICollection<DISPONIBILIDADEGLP_DIASEMANA> DISPONIBILIDADEGLP_DIASEMANA { get; set; }
        public virtual ICollection<DISPONIBILIDADEGLP_MODALIDADE> DISPONIBILIDADEGLP_MODALIDADE { get; set; }
        public virtual ICollection<DISPONIBILIDADEGLP_TURNO> DISPONIBILIDADEGLP_TURNO { get; set; }
        public virtual ICollection<DISPONIBILIDADEGLP_UNIDADEENSINO> DISPONIBILIDADEGLP_UNIDADEENSINO { get; set; }
    }
}

using System;
using System.Text;
using System.Collections.Generic;


namespace Proderj.DOL.Domain {
    
    public class LY_DOCENTE_FUNCAO_GLP {
        public LY_DOCENTE_FUNCAO_GLP() {
            //LY_AULA_DOCENTE_TIPO = new List<LY_AULA_DOCENTE_TIPO>();
			//LY_DOCENTE_FUNCAO_GLP_DETALHE = new List<LY_DOCENTE_FUNCAO_GLP_DETALHE>();
        }
        
        public virtual LY_FUNCAO LY_FUNCAO { get; set; }
        public virtual LY_UNIDADE_ENSINO LY_UNIDADE_ENSINO { get; set; }
        public virtual LY_GRUPO_HABILITACAO LY_GRUPO_HABILITACAO { get; set; }
        public virtual double ID_DOCENTE_FUNCAO_GLP { get; set; }
        public virtual string MATRICULA { get; set; }
		public virtual string IDFUNCIONAL { get; set; }
        public virtual string FUNCAO_GLP { get; set; }
        public virtual double? ANO { get; set; }
        public virtual double? MES { get; set; }
        public virtual string STATUS { get; set; }
        public virtual string UNIDADE_ENS { get; set; }
        public virtual DateTime? DATA { get; set; }
        public virtual decimal? GLP_SOLICITADA { get; set; }
        public virtual decimal? GLP_USADA { get; set; }
        public virtual decimal? GLP_CANCELADA { get; set; }
        public virtual string AGRUPAMENTO { get; set; }
        public virtual double? PRAZO { get; set; }
        public virtual double? CHLIVRE { get; set; }
        public virtual string RESERV01 { get; set; }
        public virtual string USUARIOSOLICITACAOID { get; set; }
        public virtual DateTime? DATASOLICITACAO { get; set; }
        public virtual string USUARIOANALISEID { get; set; }
        public virtual DateTime? DATAANALISE { get; set; }
        public virtual double? CHLIVREMUNICIPIO { get; set; }
        //public virtual ICollection<LY_AULA_DOCENTE_TIPO> LY_AULA_DOCENTE_TIPO { get; set; }
        //public virtual ICollection<LY_DOCENTE_FUNCAO_GLP_DETALHE> LY_DOCENTE_FUNCAO_GLP_DETALHE { get; set; }
    }
}

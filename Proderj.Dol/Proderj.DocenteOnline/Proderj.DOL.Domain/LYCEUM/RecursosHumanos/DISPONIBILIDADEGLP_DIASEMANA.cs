using System;
using System.Text;
using System.Collections.Generic;


namespace Proderj.DOL.Domain {

    public class DISPONIBILIDADEGLP_DIASEMANA
    {
        public virtual int DISPONIBILIDADEGLPID { get; set; }
        public virtual int DIASEMANA { get; set; }
        public virtual DISPONIBILIDADEGLP DISPONIBILIDADEGLP { get; set; }
        
        #region NHibernate Composite Key Requirements
        
        public override bool Equals(object obj) {
			if (obj == null) return false;
			var t = obj as DISPONIBILIDADEGLP_DIASEMANA;
			if (t == null) return false;
			if (DISPONIBILIDADEGLPID == t.DISPONIBILIDADEGLPID
			 && DIASEMANA == t.DIASEMANA)
				return true;

			return false;
        }
        
        public override int GetHashCode() {
			int hash = GetType().GetHashCode();
			hash = (hash * 397) ^ DISPONIBILIDADEGLPID.GetHashCode();
			hash = (hash * 397) ^ DIASEMANA.GetHashCode();

			return hash;
        }

        #endregion

        public virtual string DIASEMANA_DESCRICAO
        {
            get
            {
                switch (DIASEMANA)
                {
                    case 1: return "DOMINGO";
                    case 2: return "SEGUNDA";
                    case 3: return "TERÇA";
                    case 4: return "QUARTA";
                    case 5: return "QUINTA";
                    case 6: return "SEXTA";
                    case 7: return "SABADO";
                    default: return null;
                }
            }
        }
    }
}

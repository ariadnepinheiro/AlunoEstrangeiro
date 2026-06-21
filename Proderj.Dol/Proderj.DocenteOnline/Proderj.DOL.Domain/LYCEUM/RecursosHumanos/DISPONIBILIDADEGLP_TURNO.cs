using System;
using System.Text;
using System.Collections.Generic;


namespace Proderj.DOL.Domain {
    
    public class DISPONIBILIDADEGLP_TURNO {
        public virtual int DISPONIBILIDADEGLPID { get; set; }
        public virtual string TURNO { get; set; }
        public virtual DISPONIBILIDADEGLP DISPONIBILIDADEGLP { get; set; }
        //public virtual LY_TURNO LY_TURNO { get; set; }
        
        #region NHibernate Composite Key Requirements
        
        public override bool Equals(object obj) {
			if (obj == null) return false;
			var t = obj as DISPONIBILIDADEGLP_TURNO;
			if (t == null) return false;
			if (DISPONIBILIDADEGLPID == t.DISPONIBILIDADEGLPID
			 && TURNO == t.TURNO)
				return true;

			return false;
        }
        
        public override int GetHashCode() {
			int hash = GetType().GetHashCode();
			hash = (hash * 397) ^ DISPONIBILIDADEGLPID.GetHashCode();
			hash = (hash * 397) ^ TURNO.GetHashCode();

			return hash;
        }
        
        #endregion

        public virtual string TURNO_DESCRICAO
        {
            get
            {
                switch (TURNO)
                {
                    case "M": return "MANHÃ";
                    case "T": return "TARDE";
                    case "N": return "NOITE";
                    default: return null;
                }
            }
        }
    }
}

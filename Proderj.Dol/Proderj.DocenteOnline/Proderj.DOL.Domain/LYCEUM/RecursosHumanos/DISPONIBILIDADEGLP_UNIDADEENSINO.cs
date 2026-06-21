using System;
using System.Text;
using System.Collections.Generic;


namespace Proderj.DOL.Domain {
    
    public class DISPONIBILIDADEGLP_UNIDADEENSINO {
        public virtual int DISPONIBILIDADEGLPID { get; set; }
        public virtual string UNIDADE_ENS { get; set; }
        public virtual DISPONIBILIDADEGLP DISPONIBILIDADEGLP { get; set; }
        //public virtual LY_UNIDADE_ENSINO LY_UNIDADE_ENSINO { get; set; }
        
        #region NHibernate Composite Key Requirements
        
        public override bool Equals(object obj) {
			if (obj == null) return false;
			var t = obj as DISPONIBILIDADEGLP_UNIDADEENSINO;
			if (t == null) return false;
			if (DISPONIBILIDADEGLPID == t.DISPONIBILIDADEGLPID
			 && UNIDADE_ENS == t.UNIDADE_ENS)
				return true;

			return false;
        }
        
        public override int GetHashCode() {
			int hash = GetType().GetHashCode();
			hash = (hash * 397) ^ DISPONIBILIDADEGLPID.GetHashCode();
			hash = (hash * 397) ^ UNIDADE_ENS.GetHashCode();

			return hash;
        }
        
        #endregion
    }
}

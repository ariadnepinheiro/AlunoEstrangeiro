using System;
using System.Text;
using System.Collections.Generic;


namespace Proderj.DOL.Domain {
    
    public class DISPONIBILIDADEGLP_MODALIDADE {
        public virtual int DISPONIBILIDADEGLPID { get; set; }
        public virtual string MODALIDADE { get; set; }
        public virtual DISPONIBILIDADEGLP DISPONIBILIDADEGLP { get; set; }
        
        #region NHibernate Composite Key Requirements
        
        public override bool Equals(object obj) {
			if (obj == null) return false;
			var t = obj as DISPONIBILIDADEGLP_MODALIDADE;
			if (t == null) return false;
			if (DISPONIBILIDADEGLPID == t.DISPONIBILIDADEGLPID
			 && MODALIDADE == t.MODALIDADE)
				return true;

			return false;
        }
        
        public override int GetHashCode() {
			int hash = GetType().GetHashCode();
			hash = (hash * 397) ^ DISPONIBILIDADEGLPID.GetHashCode();
			hash = (hash * 397) ^ MODALIDADE.GetHashCode();

			return hash;
        }

        #endregion
    }
}

using System;
using System.Text;
using System.Collections.Generic;


namespace Proderj.DOL.Domain {
    
    public class LY_LOTACAO {
        public virtual double PESSOA { get; set; }
        public virtual string MATRICULA { get; set; }
        public virtual double ORDEM { get; set; }
        //public virtual string FUNCAO { get; set; }
        //public virtual string TURNO { get; set; }
        //public virtual DateTime? DATA_DESATIVACAO { get; set; }
        //public virtual string ATO_OFICIAL { get; set; }
        //public virtual string RESP_DOCUMENTACAO { get; set; }
        //public virtual string UNIDADE_FIS { get; set; }
        //public virtual DateTime? DATA_NOMEACAO { get; set; }
        //public virtual DateTime? DATA_NOMEACAO_DO { get; set; }
        //public virtual DateTime? DATA_DESATIVACAO_DO { get; set; }
        //public virtual string TIPO_DESATIVACAO { get; set; }
        //public virtual string UNIDADE_ENS { get; set; }
        //public virtual string NUCLEO { get; set; }
        //public virtual string SETOR { get; set; }
        //public virtual string READAPTADO { get; set; }
        //public virtual DateTime? DT_INICIO_READAPTACAO { get; set; }
        //public virtual DateTime? DT_FIM_READAPTACAO { get; set; }
        //public virtual string MOTIVO_READAPTACAO { get; set; }
        //public virtual string USUARIO { get; set; }
        //public virtual DateTime? DATA_ATUALIZACAO { get; set; }
        #region NHibernate Composite Key Requirements
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var t = obj as LY_LOTACAO;
            if (t == null) return false;
            if (PESSOA == t.PESSOA
             && MATRICULA == t.MATRICULA
             && ORDEM == t.ORDEM)
                return true;

            return false;
        }
        public override int GetHashCode()
        {
            int hash = GetType().GetHashCode();
            hash = (hash * 397) ^ PESSOA.GetHashCode();
            hash = (hash * 397) ^ MATRICULA.GetHashCode();
            hash = (hash * 397) ^ ORDEM.GetHashCode();

            return hash;
        }
        #endregion
    }
}

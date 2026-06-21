using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{   
    public class AceiteTermoCompromissoDocente
    {
        public AceiteTermoCompromissoDocente() 
        {
            TermoCompromissoDocente = new TermoCompromissoDocente();
        }

        #region Propriedades

        public virtual int Id { get; set; }

        public virtual short Ano { get; set; }
        
        public virtual string Num_func { get; set; }
        
        public virtual string IP { get; set; }
        
        public virtual DateTime DataAceite { get; set; }

        public virtual TermoCompromissoDocente TermoCompromissoDocente { get; set; }

        #endregion
       
    }
}

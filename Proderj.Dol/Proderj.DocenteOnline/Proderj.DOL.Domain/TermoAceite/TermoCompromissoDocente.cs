using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
    public class TermoCompromissoDocente
    {
        public TermoCompromissoDocente()
        {
            AceiteTermoCompromissoDocente = new List<AceiteTermoCompromissoDocente>();
        }

        #region Propriedades

        public virtual int Id { get; set; }

		public virtual short Ano { get; set; }

        public virtual IList<AceiteTermoCompromissoDocente> AceiteTermoCompromissoDocente { get; set; }
        
        public virtual DateTime DataInicio { get; set; }
        
        public virtual DateTime DataFim { get; set; }
        
        public virtual DateTime DataCadastro { get; set; }
        
        public virtual DateTime? DataAlteracao { get; set; }

		public virtual string Arquivo { get; set; }

		public virtual string Num_func { get; set; }

        #endregion

    }
}

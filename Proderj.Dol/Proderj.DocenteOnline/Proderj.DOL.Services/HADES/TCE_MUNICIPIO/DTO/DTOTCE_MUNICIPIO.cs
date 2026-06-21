using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Service
{
    public class DTOTCE_MUNICIPIO
    {
        public DTOTCE_MUNICIPIO()
		{
            Logradouros = new HashSet<DTOTCE_LOGRADOURO>();
		}

        public virtual string ID_MUNICIPIO { get; set; }
        public virtual string ID_MUNICIPIO_PRODERJ { get; set; }
        public virtual string UF { get; set; }
        public virtual string ID_IBGE { get; set; }
        public virtual string ID_IBGE_COM_DV { get; set; }
        public virtual string NOME { get; set; }

        public class DTOTCE_LOGRADOURO
        {
            public virtual int ID_LOGRADOURO { get; set; }
            public virtual string ID_MUNICIPIO { get; set; }
            public virtual string CEP { get; set; }
            public virtual string NOME { get; set; }
        }

        public virtual ICollection<DTOTCE_LOGRADOURO> Logradouros { get; set; }
    }
}

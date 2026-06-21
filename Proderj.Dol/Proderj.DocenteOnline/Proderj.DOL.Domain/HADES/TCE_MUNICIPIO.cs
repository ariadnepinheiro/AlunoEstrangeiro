using System.Collections.Generic;

namespace Proderj.DOL.Domain
{
	public class TCE_MUNICIPIO
	{
		public TCE_MUNICIPIO()
		{
            Logradouros = new HashSet<TCE_LOGRADOURO>();
		}

        public virtual string ID_MUNICIPIO { get; set; }
        public virtual string ID_MUNICIPIO_PRODERJ { get; set; }
        public virtual string UF { get; set; }
        public virtual string ID_IBGE { get; set; }
        public virtual string ID_IBGE_COM_DV { get; set; }
        public virtual string NOME { get; set; }

        public virtual ICollection<TCE_LOGRADOURO> Logradouros { get; set; }
	}
}

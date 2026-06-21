using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
    public class Login
    {
        #region Construtores

        public Login()
        { 
			Sistema = new TipoSistema();
		}

        #endregion

        #region Propriedades

        public virtual int Id { get; set; }

        public virtual TipoSistema Sistema { get; set; }
        
        public virtual string Usuario { get; set; }
        
        public virtual DateTime? DataAcesso { get; set; }

        #endregion
    }
}

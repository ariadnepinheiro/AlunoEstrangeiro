using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Architecture;
using Proderj.DOL.Domain;
using Proderj.Foundation.Framework;
using System.Web;

namespace Proderj.DOL.Repository
{
    public class LoginRepository : NHRepositoryBase<Login>, ILoginRepository
    {
        #region ILoginRepository Members

        public void Inclui(TipoSistema sistema, string usuario, DateTime? dataAcesso)
        {
            var login = new Login
            {
                DataAcesso = dataAcesso,
                Sistema = sistema,
                Usuario = usuario
            };

            Inclui(login);
        }

        #endregion
      
    }
}

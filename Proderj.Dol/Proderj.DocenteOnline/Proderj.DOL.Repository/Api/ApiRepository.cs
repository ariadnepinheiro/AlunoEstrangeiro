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
    public class ApiRepository : NHRepositoryBase<DadosAPI>, IApiRepository
    {
        #region IApiRepository Members

        public void ReceberDados(string crp)
        {

        }

        #endregion
      
    }
}

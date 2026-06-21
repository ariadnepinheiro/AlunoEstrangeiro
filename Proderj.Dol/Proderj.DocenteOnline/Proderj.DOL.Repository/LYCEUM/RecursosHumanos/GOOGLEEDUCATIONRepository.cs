using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Transform;
using Proderj.DOL.Domain;
using Proderj.Foundation.Framework;

namespace Proderj.DOL.Repository
{
    public class GOOGLEEDUCATIONRepository : NHRepositoryBase<GOOGLEEDUCATION>, IGOOGLEEDUCATIONRepository
    {
        public override IQueryable<GOOGLEEDUCATION> ListaQueryable()
        {
            return base.ListaQueryable();
        }
    }
}
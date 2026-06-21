using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;
using Proderj.DOL.Domain;
using NHibernate.Transform;
using NHibernate.Criterion;

namespace Proderj.DOL.Repository
{
    public class ProtocoloNotaRepository : NHRepositoryBase<Protocolo>, IProtocoloNotaRepository
    {
        public IEnumerable<Protocolo> EnumeraPor(string idFuncional)
        {
            var protocolos = Sessao.CreateCriteria<Protocolo>()
                .Add(Restrictions.Eq("IdFuncional", idFuncional))
                .AddOrder(Order.Desc("DataCadastro"))
                .List<Protocolo>();

            return protocolos;
        }

        public IEnumerable<Protocolo> EnumeraPor(string idFuncional, string idVinculo, short ano, short periodo)
        {
            var protocolos = Sessao.CreateCriteria<Protocolo>()
                 .Add(Restrictions.Eq("IdFuncional", idVinculo))
                 //.Add(Expression.Or(
                 //         Restrictions.Eq("idFuncional", idVinculo),
                 //         Restrictions.Eq("Matricula", idVinculo)
                 //       ))
                .Add(Restrictions.Eq("Ano", ano))
                .Add(Restrictions.Eq("Periodo", periodo))
                .AddOrder(Order.Desc("DataCadastro"))
                .List<Protocolo>();

            return protocolos;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;
using Proderj.DOL.Domain;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace Proderj.DOL.Repository
{
    public class PeriodoLetivoRepository : NHRepositoryBase<PeriodoLetivo>, IPeriodoLetivoRepository
    {
        public IEnumerable<PeriodoLetivo> Enumera()
        {
            IEnumerable<PeriodoLetivo> anos = Sessao.CreateCriteria<PeriodoLetivo>()
             .SetProjection
             (
                Projections.Distinct(
                    Projections.ProjectionList()
                       .Add(Projections.Property("Ano"), "Ano")
                )
             )
             .SetResultTransformer(Transformers.AliasToBean(typeof(PeriodoLetivo)))
             .List<PeriodoLetivo>();

            return anos;

        }

        public IEnumerable<PeriodoLetivo> EnumeraPor(short ano)
        {
            IEnumerable<PeriodoLetivo> periodos = Sessao.CreateCriteria<PeriodoLetivo>()
             .Add(Restrictions.Eq("Ano", ano))
             .SetProjection
             (
                Projections.Distinct(
                    Projections.ProjectionList()
                       .Add(Projections.Property("Periodo"), "Periodo")
                       .Add(Projections.Property("DescricaoPeriodo"), "DescricaoPeriodo")
                )
             )
             .SetResultTransformer(Transformers.AliasToBean(typeof(PeriodoLetivo)))
             .List<PeriodoLetivo>();

            return periodos;

        }
    }
}

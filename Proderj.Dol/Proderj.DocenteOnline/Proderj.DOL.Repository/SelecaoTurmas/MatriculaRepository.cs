using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;
using Proderj.DOL.Domain;
using System.Collections;

namespace Proderj.DOL.Repository
{
    public class MatriculaRepository : NHRepositoryBase<Matricula>, IMatriculaRepository
    {
        #region IMatriculaRepository Members
    
        public IEnumerable<Matricula> EnumeraMatriculaPor(string matriculaAluno)
        {
            return Sessao.QueryOver<Matricula>()
                    .Where(matricula => matricula.Aluno == matriculaAluno)
                    .List();
        }     

        #endregion

        //TODO:jogar para o teste
        public IEnumerable<Matricula> ListaMatriculas()
        {
            var matriculas = Sessao.CreateCriteria<Matricula>()
              .Add(NHibernate.Criterion.Restrictions.Eq("Disciplina", "152-EMR-1-4"))
           .SetProjection
           (
              NHibernate.Criterion.Projections.ProjectionList()
              .Add(NHibernate.Criterion.Projections.Property("Disciplina"), "Disciplina")
              .Add(NHibernate.Criterion.Projections.Property("Turma"), "Turma")
           )
           .SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(Matricula)));

            return matriculas.List<Matricula>();
        }

    }
}

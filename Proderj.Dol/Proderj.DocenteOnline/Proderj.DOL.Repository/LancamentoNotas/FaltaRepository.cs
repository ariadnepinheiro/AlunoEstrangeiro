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
    public class FaltaRepository : NHRepositoryBase<Falta>, IFaltaRepository
    {
        #region IFaltaRepository Members

        public IEnumerable<Falta> EnumeraPor(short ano, short periodo, string turma, string disciplina, string frequencia)
        {
            IEnumerable<Falta> faltas = Sessao.CreateCriteria<Falta>()
            .Add(Restrictions.Eq("Frequencia", frequencia))
            .Add(Restrictions.Eq("Ano", ano))
            .Add(Restrictions.Eq("Turma", turma))
            .Add(Restrictions.Eq("Disciplina", disciplina))
            .Add(Restrictions.Eq("Semestre", periodo))
            .SetProjection
            (
               Projections.ProjectionList()
                  .Add(Projections.Property("Aluno"), "Aluno")
                  .Add(Projections.Property("QuantFaltas"), "QuantFaltas")
            )
            .SetResultTransformer(Transformers.AliasToBean(typeof(Falta)))
            .List<Falta>();

            return faltas;
        }

        public void Atualiza(double faltas, string aluno, string disciplina, string turma, short ano, short periodo, string frequencia)
        {
            StringBuilder stb = new StringBuilder();
            try
            {

                stb.Append("update Proderj.DOL.Domain.Falta set QuantFaltas = :quantFaltas ");
                stb.Append("where Disciplina = :disciplina ");
                stb.Append("and Turma = :turma ");
                stb.Append("and Ano = :ano ");
                stb.Append("and Semestre = :semestre ");
                stb.Append("and Aluno = :aluno ");
                stb.Append("and Frequencia = :frequencia ");


                var query = SessaoAuditada.CreateQuery(stb.ToString());
                query.SetParameter("disciplina", disciplina);
                query.SetParameter("turma", turma);
                query.SetParameter("ano", ano);
                query.SetParameter("semestre", periodo);
                query.SetParameter("aluno", aluno);
                query.SetParameter("frequencia", frequencia);
                query.SetParameter("quantFaltas", faltas);

                query.ExecuteUpdate();

            }
            catch (Exception)
            {
                throw new Exception("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.");
            }

        }

        public void RemoveFalta(short ano, short periodo, string turma, string disciplina, string frequencia, string aluno)
        {
            StringBuilder stb = new StringBuilder();

            try
            {
                stb.Append("delete from Proderj.DOL.Domain.Falta ");
                stb.Append("where Disciplina = :disciplina ");
                stb.Append("and Aluno = :aluno ");
                stb.Append("and Turma = :turma ");
                stb.Append("and Ano = :ano ");
                stb.Append("and Semestre = :semestre ");
                stb.Append("and Frequencia = :frequencia ");

                var query = SessaoAuditada.CreateQuery(stb.ToString());
                query.SetParameter("aluno", aluno);
                query.SetParameter("disciplina", disciplina);
                query.SetParameter("turma", turma);
                query.SetParameter("ano", ano);
                query.SetParameter("semestre", periodo);
                query.SetParameter("frequencia", frequencia);

                query.ExecuteUpdate();
            }
            catch (Exception)
            {
                throw new Exception("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.");
            }
        }

        #endregion
    }
}

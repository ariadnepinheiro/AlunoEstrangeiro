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
    public class FrequenciaRepository : NHRepositoryBase<Frequencia>, IFrequenciaRepository
    {
        #region IFrequenciaRepository Members

        public Frequencia ObtemFrequenciaPor(string disciplina, string turma, short ano, short periodo, short subperiodo)
        {
            Frequencia freq = Sessao.CreateCriteria<Frequencia>()
             .Add(Restrictions.Eq("Ano", ano))
             .Add(Restrictions.Eq("Turma", turma))
             .Add(Restrictions.Eq("SubPeriodo", subperiodo))
             .Add(Restrictions.Eq("Semestre", periodo))
             .Add(Restrictions.Eq("Disciplina", disciplina))
             .SetProjection
             (
                Projections.ProjectionList()
                   .Add(Projections.Property("TipoFrequencia"), "TipoFrequencia")
                   .Add(Projections.Property("Descricao"), "Descricao")
                   .Add(Projections.Property("AulasPrevistas"), "AulasPrevistas")
                   .Add(Projections.Property("AulasDadas"), "AulasDadas")
             )
             .SetMaxResults(1)
             .SetResultTransformer(Transformers.AliasToBean(typeof(Frequencia)))
             .UniqueResult<Frequencia>();

            return freq;
        }

        public void AtualizaFrequencia(string aulasDadas, string aulasPrevistas, string disciplina, string turma, short ano, short periodo, string frequencia)
        {
            StringBuilder stb = new StringBuilder();
            try
            {

                stb.Append("update Proderj.DOL.Domain.Frequencia set AulasDadas = :aulasDadas, ");
                stb.Append("AulasPrevistas = :aulasPrevistas ");
                stb.Append("where Disciplina = :disciplina ");
                stb.Append("and Turma = :turma ");
                stb.Append("and Ano = :ano ");
                stb.Append("and Semestre = :semestre ");
                stb.Append("and TipoFrequencia = :tipoFrequencia ");

                var query = SessaoAuditada.CreateQuery(stb.ToString());
                query.SetParameter("aulasDadas", aulasDadas);
                query.SetParameter("aulasPrevistas", aulasPrevistas);
                query.SetParameter("disciplina", disciplina);
                query.SetParameter("turma", turma);
                query.SetParameter("ano", ano);
                query.SetParameter("semestre", periodo);
                query.SetParameter("tipoFrequencia", frequencia);

                query.ExecuteUpdate();

            }
            catch (Exception)
            {
                throw new Exception("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.");
            }
        }

        public Frequencia ObtemTotalAulasConsolidado(string disciplina, string turma, short ano, short periodo)
        {
            Frequencia frequencia = Sessao.CreateCriteria<Frequencia>()
                .Add(Restrictions.Eq("Disciplina", disciplina))
                .Add(Restrictions.Eq("Turma", turma))
                .Add(Restrictions.Eq("Ano", ano))
                .Add(Restrictions.Eq("Semestre", periodo))
                .SetProjection(
                    Projections.ProjectionList()
                        .Add(Projections.Sum("AulasPrevistas"), "AulasPrevistas")
                        .Add(Projections.Sum("AulasDadas"), "AulasDadas")
                )
                .SetMaxResults(1)
                .SetResultTransformer(Transformers.AliasToBean(typeof(Frequencia)))
                .UniqueResult<Frequencia>();

            return frequencia;
        }

        #endregion
    }
}

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
    public class NotaRepository : NHRepositoryBase<Nota>, INotaRepository
    {
        #region INotaRepository Members

        public IEnumerable<Nota> EnumeraPor(short ano, short periodo, string turma, string disciplina, string prova)
        {
            IEnumerable<Nota> notas = Sessao.CreateCriteria<Nota>()
            .Add(Restrictions.Eq("TipoProva", prova))
            .Add(Restrictions.Eq("Ano", ano))
            .Add(Restrictions.Eq("Semestre", periodo))
            .Add(Restrictions.Eq("Turma", turma))
            .Add(Restrictions.Eq("Disciplina", disciplina))
            .SetProjection
            (
               Projections.ProjectionList()
                  .Add(Projections.Property("Aluno"), "Aluno")
                  .Add(Projections.Property("Conceito"), "Conceito")
                  .Add(Projections.Property("RecuperacaoParalela"), "RecuperacaoParalela")
                  .Add(Projections.Property("SemAvaliacao"), "SemAvaliacao")
                  .Add(Projections.Property("Justificativa"), "Justificativa")
            )
            .SetResultTransformer(Transformers.AliasToBean(typeof(Nota)))
            .List<Nota>();

            return notas;
        }

        public void RemoveNota(short ano, short periodo, string turma, string disciplina, string prova, string aluno)
        {
            try
            {
                StringBuilder stb = new StringBuilder();
                stb.Append("delete from Proderj.DOL.Domain.Nota ");
                stb.Append("where Disciplina = :disciplina ");
                stb.Append("and Aluno = :aluno ");
                stb.Append("and Turma = :turma ");
                stb.Append("and Ano = :ano ");
                stb.Append("and Semestre = :semestre ");
                stb.Append("and TipoProva = :tipoProva ");

                var query = SessaoAuditada.CreateQuery(stb.ToString());
                query.SetParameter("aluno", aluno);
                query.SetParameter("disciplina", disciplina);
                query.SetParameter("turma", turma);
                query.SetParameter("ano", ano);
                query.SetParameter("semestre", periodo);
                query.SetParameter("tipoProva", prova);

                query.ExecuteUpdate();

            }
            catch (Exception)
            {
                throw new Exception("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.");
            }
         
        }

        public void AtualizaNota(Nota nota)
        {
            StringBuilder stb = new StringBuilder();
            try
            {

                stb.Append("update Proderj.DOL.Domain.Nota set Compareceu = :compareceu, ");
                stb.Append("Conceito = :conceito, ");
                stb.Append("DataProva = :dataProva, ");
                stb.Append("Ordem = :ordem, ");
                stb.Append("Formulario = :formulario, ");
                stb.Append("RecuperacaoParalela = :recuperacaoParalela, ");
                stb.Append("SemAvaliacao = :semAvaliacao, ");
                stb.Append("Justificativa = :justificativa, ");
                stb.Append("NotaRecuperacao = :NotaRecuperacao, ");
                stb.Append("NotaProva = :notaProva, ");
                stb.Append("MotivoSemNota = :motivoSemNota ");
                stb.Append("where Disciplina = :disciplina ");
                stb.Append("and Aluno = :aluno ");
                stb.Append("and Turma = :turma ");
                stb.Append("and Ano = :ano ");
                stb.Append("and Semestre = :semestre ");
                stb.Append("and TipoProva = :tipoProva ");

                var query = SessaoAuditada.CreateQuery(stb.ToString());
                query.SetParameter("compareceu", nota.Compareceu);
                query.SetParameter("conceito", nota.Conceito);
                query.SetParameter("dataProva", nota.DataProva);
                query.SetParameter("ordem", nota.Ordem);
                query.SetParameter("formulario", nota.Formulario);
                query.SetParameter("recuperacaoParalela", nota.RecuperacaoParalela);
                query.SetParameter("semAvaliacao", nota.SemAvaliacao);
                query.SetParameter("justificativa", nota.Justificativa);
                query.SetParameter("aluno", nota.Aluno);
                query.SetParameter("disciplina", nota.Disciplina);
                query.SetParameter("turma", nota.Turma);
                query.SetParameter("ano", nota.Ano);
                query.SetParameter("semestre", nota.Semestre);
                query.SetParameter("tipoProva", nota.TipoProva);
                query.SetParameter("NotaRecuperacao", nota.NotaRecuperacao);
                query.SetParameter("notaProva", nota.NotaProva);
                query.SetParameter("motivoSemNota", nota.MotivoSemNota);

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

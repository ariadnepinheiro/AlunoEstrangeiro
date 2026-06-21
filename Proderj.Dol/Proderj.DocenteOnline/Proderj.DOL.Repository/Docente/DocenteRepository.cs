using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Architecture;
using Proderj.DOL.Domain;
using Proderj.Foundation.Framework;
using NHibernate.Criterion;

namespace Proderj.DOL.Repository
{
    public class DocenteRepository : NHRepositoryBase<Docente>, IDocenteRepository
    {
        #region IDocenteRepository Members

        public Docente ObtemPor(string matricula)
        {
            return Sessao.QueryOver<Docente>()
                .Where(docente => docente.Matricula == matricula)
                .SingleOrDefault<Docente>();
        }

        public Docente ObtemPorPessoa(String pessoa)
        {
            int auxpessoa = Int32.Parse(pessoa);
            var query = Sessao.QueryOver<Docente>()
                .Where(docente => docente.NumeroFuncionario == auxpessoa);//id tava o id tem q fazer a pesquisa pelo codigo unico dai tem q receber o numero
                //.SingleOrDefault<Docente>();                
            return query.Take(1).SingleOrDefault<Docente>();
                
        }        

        public long ObtemNumFuncPor(string matricula)
        {
            IEnumerable<Docente> docente = Sessao.CreateCriteria<Docente>()
            .Add(Restrictions.Eq("Matricula", matricula))
            .SetProjection
            (
               Projections.Distinct(
                   Projections.ProjectionList()
                      .Add(Projections.Property("NumeroFuncionario"), "NumeroFuncionario")
               )
            )
            .SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(Docente)))
            .List<Docente>();

            return docente.First().NumeroFuncionario;
        }

        public Docente ObtemPorPessoaPor(string matricula)
        {
            Sessao.Clear();

            Docente docente = Sessao.CreateCriteria<Docente>()
            .CreateAlias("Pessoa", "p")
            .Add(Restrictions.Eq("Matricula", matricula))
            .SetProjection
            (
               Projections.Distinct(
                   Projections.ProjectionList()
                      .Add(Projections.Property("Pessoa"), "Pessoa")
               )
            )
            .SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(Docente)))
            .SetMaxResults(1)
            .UniqueResult<Docente>();

            return docente;
        }

        public String ObtemEmailPor(string matricula)
        {
            return Sessao.QueryOver<Docente>()
                .Where(docente => docente.Matricula == matricula)
                .JoinQueryOver<Pessoa>(docente => docente.Pessoa)
                .SingleOrDefault()
                    .Pessoa
                        .EmailInterno;
        }        

        public void RedefineSenha(string novasenha, string matricula)
        {
            try
            {
                var docente = Sessao.QueryOver<Docente>()
                    .Where(doc => doc.Matricula == matricula)
                        .SingleOrDefault();

                docente.SenhaAlterada = "S";
                docente.SenhaDocente = novasenha;

                SessaoAuditada.SaveOrUpdate(docente);
                SessaoAuditada.Flush();
            }
            catch (Exception)
            {
                throw new Exception("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.");
            }
        }

        public void AlteraSenha(string novaSenha, string matricula)
        {
            try
            {
                var docente = Sessao.QueryOver<Docente>()
                    .Where(doc => doc.Matricula == matricula)
                        .SingleOrDefault();

                docente.SenhaAlterada = "N";
                docente.SenhaDocente = novaSenha;

                SessaoAuditada.Update(docente);

            }
            catch (Exception)
            {
                throw new Exception("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.");
            }
        }

        #endregion
    }
}

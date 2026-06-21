using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;
using NHibernate;
using NHibernate.Transform;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository
{
    public class PessoaRepository : NHRepositoryBase<Pessoa>, IPessoaRepository
    {
        public Pessoa ObtemPor(long identificador)
        {
            return Sessao.QueryOver<Pessoa>()
                .Where(pessoa => pessoa.Id == identificador)
                .SingleOrDefault<Pessoa>();
        }

        public string BuscaMatricula(string vinculo, string idfuncional)
        {
               try
            {
            ISQLQuery query = Sessao.CreateSQLQuery(
                                 @" select  CONVERT(varchar(10),a.PESSOA) as PESSOA   FROM [LYCEUM].[dbo].[LY_PESSOA] a
	                                inner join [LYCEUM].[dbo].[LY_DOCENTE] b on a.pessoa =b.pessoa
	                                where a.idfuncional= :IDFUNCIONAL and b.VINCULO =:VINCULO"
                                   );

            query.SetString("VINCULO", vinculo);
            query.SetString("IDFUNCIONAL", idfuncional);

            return query.UniqueResult<string>();
            }
               catch (Exception)
               {
                   throw new Exception("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.");
               } 
        }

        public string BuscaNunFunc(string vinculo, long idfuncional)
        {
            try
            {
                ISQLQuery query = Sessao.CreateSQLQuery(
                    @" select d.NUM_FUNC
			           from [LYCEUM].[dbo].[LY_PESSOA] P
					        INNER JOIN [LYCEUM].[dbo].[LY_DOCENTE] D ON P.PESSOA = D.PESSOA
			           where idfuncional = :IDFUNCIONAL
					        and VINCULO = :VINCULO "
                );

                query.SetParameter("VINCULO", vinculo);
                query.SetParameter("IDFUNCIONAL", idfuncional);

                var resultado = query.UniqueResult<object>();
                
                return Convert.ToString (resultado);
            }
            catch (Exception ex)
            {
                // Aqui você pode logar o erro real (ex.Message) para diagnóstico
                throw new Exception("Falha ao buscar o número funcional. Entre em contato com o administrador do sistema.");
            }
        }


        public List<string> BuscaIdVinculo(string cpf)
        {
            try
            {
                ISQLQuery qry = SessaoAuditada.CreateSQLQuery(@" select distinct IDVINCULO
                                        from VW_FUNCIONARIOS f
                                        inner join LY_DOCENTE D on f.PESSOA = d.PESSOA and f.MATRICULA = d.MATRICULA
                                        WHERE DT_DEMISSAO IS NULL
                                        and IDVINCULO is not null
                                        and (f.DATA_DESATIVACAO is null or  f.DATA_DESATIVACAO > GETDATE())
                                        and cpf =:CPF ");

                qry.SetParameter<string>("CPF", cpf);

                return qry.List<string>().ToList();
            }
            catch (Exception)
            {
                throw new Exception("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.");
            }
        }

        public string ObtemIdFuncional(string pessoa)
        {
            ISQLQuery consulta =
                Sessao.CreateSQLQuery(
                    @"select idfuncional
	                  FROM [LYCEUM].[dbo].[LY_PESSOA] a
	  	                inner join [LYCEUM].[dbo].[LY_DOCENTE] b on a.pessoa =b.pessoa
		                where a.pessoa = :pessoa");
            consulta.SetString("pessoa", pessoa);
            var result = consulta.List();

            return "";
        }

        public int AtualizaTelefone(string telefone, long identificador)
        {
            StringBuilder stb = new StringBuilder();

            try
            {
                stb.Append("update Proderj.DOL.Domain.Pessoa set Telefone = :telefone ");
                stb.Append("where id = :identificador ");

                var query = SessaoAuditada.CreateQuery(stb.ToString());
                query.SetParameter("telefone", telefone);
                query.SetParameter("identificador", identificador);

               return query.ExecuteUpdate();
            }
            catch (Exception)
            {
                throw new Exception("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.");
            }          
        }
    }
}

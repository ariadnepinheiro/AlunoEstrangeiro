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
    public class AvaliacaoCurriculoMinimoJustificativaRepository : NHRepositoryBase<AvaliacaoCurriculoMinimoJustificativa>, IAvaliacaoCurriculoMinimoJustificativaRepository
    {
        public AvaliacaoCurriculoMinimoJustificativa ObtemPor(short ano, short periodo, short subperiodo, string matricula)
        {
            AvaliacaoCurriculoMinimoJustificativa subPeriodo = Sessao.CreateCriteria<AvaliacaoCurriculoMinimoJustificativa>()
             .Add(Restrictions.Eq("Ano", ano))
             .Add(Restrictions.Eq("Periodo", periodo))
             .Add(Restrictions.Eq("SubPeriodo", subperiodo))
             .Add(Restrictions.Eq("Matricula", matricula))
             .SetProjection(
                Projections.Distinct(
                    Projections.ProjectionList()
                       .Add(Projections.Property("Justificativa"), "Justificativa")
                       .Add(Projections.Property("Id"), "Id")
                    ))
             .SetMaxResults(1)
             .SetResultTransformer(Transformers.AliasToBean(typeof(AvaliacaoCurriculoMinimoJustificativa)))
             .UniqueResult<AvaliacaoCurriculoMinimoJustificativa>();

            return subPeriodo;
        }

        public int RemoveCompetenciasAntigas(string matricula, short ano, short periodo, short subperiodo)
        {
            StringBuilder stb = new StringBuilder();

            try
            {
                stb.Append("delete from Proderj.DOL.Domain.AvaliacaoCurriculoMinimoJustificativa ");
                stb.Append("where Matricula = :matricula ");
                stb.Append("and Ano = :ano ");
                stb.Append("and Periodo = :periodo ");
                stb.Append("and SubPeriodo = :subperiodo ");

                var query = SessaoAuditada.CreateQuery(stb.ToString());
                query.SetParameter("matricula", matricula);
                query.SetParameter("ano", ano);
                query.SetParameter("periodo", periodo);
                query.SetParameter("subperiodo", subperiodo);

                return query.ExecuteUpdate();
            }
            catch (Exception)
            {
                throw new Exception("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.");
            }           
        }

        public int InsereCom(AvaliacaoCurriculoMinimoJustificativa avaliacaoCurriculoMinimoJustificativa)
        {          
            try
            {
              
                var query = SessaoAuditada.CreateSQLQuery(
                                            @"  INSERT  INTO TCE_AVALIACAO_CM_JUSTIFICATIVA
										(
										  ANO,
										  PERIODO,
										  SUBPERIODO,
										  JUSTIFICATIVA,
										  MATRICULA
										)
										VALUES  
										(
											:ANO,
											:PERIODO,
											:SUBPERIODO,
											:JUSTIFICATIVA,
											:MATRICULA
										)"
                                        );

                query.SetInt16("ANO", avaliacaoCurriculoMinimoJustificativa.Ano);
                query.SetInt16("PERIODO", avaliacaoCurriculoMinimoJustificativa.Periodo);
                query.SetInt16("SUBPERIODO", avaliacaoCurriculoMinimoJustificativa.SubPeriodo);
                query.SetString("JUSTIFICATIVA", avaliacaoCurriculoMinimoJustificativa.Justificativa);
                query.SetString("MATRICULA", avaliacaoCurriculoMinimoJustificativa.Matricula);

                return query.ExecuteUpdate();               
            }
            catch (Exception)
            {
                throw new Exception("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.");
            }           
        }
    }
}

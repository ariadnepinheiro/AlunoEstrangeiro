using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;
using Proderj.DOL.Domain;
using NHibernate.Criterion;
using NHibernate.Transform;
using NHibernate.SqlCommand;
using NHibernate;

namespace Proderj.DOL.Repository
{
    public class DocenteDisponivelGLPRepository : NHRepositoryBase<DocenteDisponivelGLP>, IDocenteDisponivelGLPRepository
    {
        public IEnumerable<DocenteDisponivelGLP> EnumeraPor(long numeroFuncionario)
        {
            var docentesGLP = Sessao.CreateCriteria<DocenteDisponivelGLP>()
             .CreateAlias("GrupoHabilitacao", "gh", JoinType.LeftOuterJoin)
             .CreateAlias("Municipio", "m", JoinType.LeftOuterJoin)
             .CreateAlias("Regional", "r")
             .CreateAlias("Docente", "d")
             .Add(Restrictions.Eq("d.NumeroFuncionario", numeroFuncionario))
             .SetProjection
             (
                Projections.ProjectionList()
                   .Add(Projections.Property("Id"), "Id")
                   .Add(Projections.Property("HoraInicio"), "HoraInicio")
                   .Add(Projections.Property("HoraFinal"), "HoraFinal")
                   .Add(Projections.Property("DiaSemana"), "DiaSemana")
                   .Add(Projections.Property("Municipio"), "Municipio")
                   .Add(Projections.Property("Regional"), "Regional")
                   .Add(Projections.Property("GrupoHabilitacao"), "GrupoHabilitacao")
             )
              .AddOrder(Order.Asc("DiaSemana"))
              .AddOrder(Order.Asc("HoraInicio"))
             .SetResultTransformer(Transformers.AliasToBean(typeof(DocenteDisponivelGLP)))
             .List<DocenteDisponivelGLP>();

            return docentesGLP;
        }

        public bool ExisteDisponibilidade(TODocenteDisponivelGLPExisteDisponibilidade toExisteDisponibilidade)
        {
            ISQLQuery consulta =
                Sessao.CreateSQLQuery(
                    @"	select 1 
						from LY_DOCENTE_DISPONIVEL_GLP 
                        where NUM_FUNC = :numeroFuncionario
							and REGIONALID = :regional
							and MUNICIPIO = :codigoMunicipio
							and DATEPART(hour, HORA_INI) = DATEPART(hour,:horaInicio)
							and DATEPART(minute, HORA_INI) = DATEPART(minute,:horaInicio)
							and DATEPART(hour, HORA_FIM) = DATEPART(hour,:horaFinal)
							and DATEPART(minute, HORA_FIM) = DATEPART(minute,:horaFinal)
							and DIA_SEMANA = :diaSemana");

            consulta.SetInt64("numeroFuncionario", toExisteDisponibilidade.NumeroFuncionario);
            consulta.SetInt32("regional", toExisteDisponibilidade.CodigoRegional);
            consulta.SetString("codigoMunicipio", toExisteDisponibilidade.CodigoMunicipio);
            consulta.SetDateTime("horaInicio", toExisteDisponibilidade.HoraInicio);
            consulta.SetDateTime("horaFinal", toExisteDisponibilidade.HoraFinal);
            consulta.SetInt16("diaSemana", toExisteDisponibilidade.DiaSemana);

            return consulta.List().Count > 0;
        }

        public IEnumerable<DocenteDisponivelGLP> EnumeraDisponibilidadePor(short diaSemana, string codigoMunicipio, int codigoRegional)
        {
            IEnumerable<DocenteDisponivelGLP> disponibilidadesGLP = Sessao.CreateCriteria<DocenteDisponivelGLP>()
             .CreateAlias("Municipio", "m")
             .CreateAlias("Regional", "r")
             .Add(Restrictions.Eq("DiaSemana", diaSemana))
             .Add(Restrictions.Eq("m.Codigo", codigoMunicipio))
             .Add(Restrictions.Eq("r.Codigo", codigoRegional))
             .SetProjection
             (
                Projections.ProjectionList()
                   .Add(Projections.Property("HoraInicio"), "HoraInicio")
                   .Add(Projections.Property("HoraFinal"), "HoraFinal")
             )
             .SetResultTransformer(Transformers.AliasToBean(typeof(DocenteDisponivelGLP)))
             .List<DocenteDisponivelGLP>();

            return disponibilidadesGLP;
        }

        public int Insere(DocenteDisponivelGLP docenteDisponivel)
        {

            try
            {
                var query = SessaoAuditada.CreateSQLQuery(
                                    @"INSERT  INTO LY_DOCENTE_DISPONIVEL_GLP
                                    (
                                        NUM_FUNC,
                                        REGIONALID,
                                        MUNICIPIO,
                                        UNIDADE_ENS,
                                        DIA_SEMANA,
                                        HORA_INI,
                                        HORA_FIM,
                                        GRUPO_HABILITACAO
                                    )
									VALUES  
									(
										:NUM_FUNC,
										:REGIONALID,
										:MUNICIPIO,
										:UNIDADE_ENS,
										:DIA_SEMANA,
										:HORA_INI,
										:HORA_FIM,
										:GRUPO_HABILITACAO
									)");

                query.SetInt64("NUM_FUNC", docenteDisponivel.Docente.NumeroFuncionario);
                query.SetInt32("REGIONALID", docenteDisponivel.Regional.Codigo);
                query.SetString("MUNICIPIO", docenteDisponivel.Municipio.Codigo);
                query.SetString("UNIDADE_ENS", docenteDisponivel.UnidadeEnsino.Codigo);
                query.SetInt16("DIA_SEMANA", docenteDisponivel.DiaSemana);
                query.SetDateTime("HORA_INI", docenteDisponivel.HoraInicio.Value);
                query.SetDateTime("HORA_FIM", docenteDisponivel.HoraFinal.Value);
                query.SetString("GRUPO_HABILITACAO", docenteDisponivel.GrupoHabilitacao.Agrupamento);


                return query.ExecuteUpdate();
            }
            catch (Exception)
            {
                throw new Exception("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.");
            }
        }

        public int RemovePor(int identificador)
        {
            StringBuilder stb = new StringBuilder();

            try
            {

                stb.Append("delete from Proderj.DOL.Domain.DocenteDisponivelGLP ");
                stb.Append("where Id = :identificador ");

                var query = SessaoAuditada.CreateQuery(stb.ToString());
                query.SetParameter("identificador", identificador);

                return query.ExecuteUpdate();

            }
            catch (Exception)
            {
                throw new Exception("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.");
            }
        }


        public bool ConfereItemEhDoDocente(int docenteDisponivelId, long numeroFuncionario)
        {
            var docenteGLP = Sessao.CreateCriteria<DocenteDisponivelGLP>()
             .CreateAlias("Docente", "d")
             .Add(Restrictions.Eq("d.NumeroFuncionario", numeroFuncionario))
             .Add(Restrictions.Eq("Id", docenteDisponivelId))
             .SetProjection
             (
                Projections.ProjectionList()
                   .Add(Projections.Property("Id"), "Id")
             )
             .SetResultTransformer(Transformers.AliasToBean(typeof(DocenteDisponivelGLP)))
             .UniqueResult<DocenteDisponivelGLP>();

            return docenteGLP != null;
        }
    }
}

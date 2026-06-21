using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Transform;
using Proderj.DOL.Domain;
using Proderj.Foundation.Framework;

namespace Proderj.DOL.Repository
{
    public class DISPONIBILIDADEGLPRepository : NHRepositoryBase<DISPONIBILIDADEGLP>, IDISPONIBILIDADEGLPRepository
    {
        public override IQueryable<DISPONIBILIDADEGLP> ListaQueryable()
        {
            return base.ListaQueryable();
        }

        public IList<T> ListaPor<T>(long num_func, int ano)
        {
            try
            {
                string sql = @"
                    select distinct 
                    glp.DISPONIBILIDADEGLPID,  
                    u.UNIDADE_ENS,
                    glp.NUM_FUNC,
                    glp.AGRUPAMENTO,
                    glp.ANO,
                    reg.REGIONAL,
                    mun.NOME as MUNICIPIO,
                    lyue.NOME_COMP as UNIDADE_ESCOLAR,
                    lygh.DESCRICAO as DISCIPLINA, 
                    glp.USUARIOID,
                    glp.DATACADASTRO,
                    glp.DATAALTERACAO,

                    ltrim(STUFF((SELECT ', ' + m.MODALIDADE 
                    FROM RecursosHumanos.DISPONIBILIDADEGLP_MODALIDADE m
                    WHERE m.DISPONIBILIDADEGLPID = glp.DISPONIBILIDADEGLPID
                    FOR XML PATH('')), 1, 1, '')) as MODALIDADE,

                    ltrim(STUFF((SELECT ', ' + (case when d.DIASEMANA = 1 then 'DOM' when d.DIASEMANA = 2 then 'SEG' when d.DIASEMANA = 3 then 'TER' when d.DIASEMANA = 4 then 'QUA' when d.DIASEMANA = 5 then 'QUI' when d.DIASEMANA = 6 then 'SEX' when d.DIASEMANA = 7 then 'SAB' else null end)
                    FROM RecursosHumanos.DISPONIBILIDADEGLP_DIASEMANA d
                    WHERE d.DISPONIBILIDADEGLPID = glp.DISPONIBILIDADEGLPID
                    FOR XML PATH('')), 1, 1, '')) as DIASEMANA,

                    ltrim(STUFF((SELECT ', ' + t.TURNO
                    FROM RecursosHumanos.DISPONIBILIDADEGLP_TURNO t
                    WHERE t.DISPONIBILIDADEGLPID = glp.DISPONIBILIDADEGLPID
                    FOR XML PATH('')), 1, 1, '')) as TURNO

                    from 
                    RecursosHumanos.DISPONIBILIDADEGLP glp
                    inner join RecursosHumanos.DISPONIBILIDADEGLP_UNIDADEENSINO u on u.DISPONIBILIDADEGLPID = glp.DISPONIBILIDADEGLPID
                    inner join LY_GRUPO_HABILITACAO lygh on glp.AGRUPAMENTO = lygh.AGRUPAMENTO
                    inner join LY_UNIDADE_ENSINO lyue on lyue.UNIDADE_ENS = u.UNIDADE_ENS
                    inner join REGIONAL reg on reg.POLO = lyue.ID_REGIONAL
                    inner join MUNICIPIO mun on mun.CODIGO = lyue.MUNICIPIO

                    where glp.NUM_FUNC = :NUM_FUNC
                    and glp.ANO = :ANO

                    group by 
                    glp.DISPONIBILIDADEGLPID,  
                    u.UNIDADE_ENS, 
                    glp.NUM_FUNC,
                    glp.AGRUPAMENTO,
                    glp.ANO,
                    reg.REGIONAL,
                    reg.REGIONAL,
                    mun.NOME,
                    lyue.NOME_COMP,
                    lygh.DESCRICAO,
                    glp.USUARIOID,
                    glp.DATACADASTRO,
                    glp.DATAALTERACAO
                ";

                ISQLQuery qry = Sessao.CreateSQLQuery(sql);

                qry.SetParameter<long>("NUM_FUNC", num_func);
                qry.SetParameter<int>("ANO", ano);

                qry.SetResultTransformer(Transformers.AliasToBean<T>());

                return qry.List<T>();
            }
            catch
            {
                throw;
            }
        }

        public void InsereAuditada(DISPONIBILIDADEGLP entidade)
        {
            using (var transaction = SessaoAuditada.BeginTransaction())
            {
                try
                {
                    var id = InsereAuditadaDISPONIBILIDADEGLP(entidade);

                    foreach (var diaSemana in entidade.DISPONIBILIDADEGLP_DIASEMANA)
                        InsereAuditadaDISPONIBILIDADEGLP_DIASEMANA(id, diaSemana.DIASEMANA);

                    foreach (var modalidade in entidade.DISPONIBILIDADEGLP_MODALIDADE)
                        InsereAuditadaDISPONIBILIDADEGLP_MODALIDADE(id, modalidade.MODALIDADE);

                    foreach (var turno in entidade.DISPONIBILIDADEGLP_TURNO)
                        InsereAuditadaDISPONIBILIDADEGLP_TURNO(id, turno.TURNO);

                    foreach (var unidadeEnsino in entidade.DISPONIBILIDADEGLP_UNIDADEENSINO)
                        InsereAuditadaDISPONIBILIDADEGLP_UNIDADEENSINO(id, unidadeEnsino.UNIDADE_ENS);

                    transaction.Commit();

                    SessaoAuditada.Flush();
                }
                catch
                {
                    transaction.Rollback();
                    throw new Exception("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.");
                }
            }
        }

        public bool ExistePor(double num_func, int ano, string disciplina, string unidadeEscolar, string modalidade, int diaSemana, string turno)
        {
            try
            {
                return ListaQueryable().Any(q => 
                    q.ANO == ano &&
                    q.LY_DOCENTE.NUM_FUNC == num_func &&
                    q.LY_GRUPO_HABILITACAO.AGRUPAMENTO == disciplina &&
                    q.DISPONIBILIDADEGLP_UNIDADEENSINO.Any(r => r.UNIDADE_ENS == unidadeEscolar) &&
                    q.DISPONIBILIDADEGLP_MODALIDADE.Any(r => r.MODALIDADE == modalidade) &&
                    q.DISPONIBILIDADEGLP_DIASEMANA.Any(r => r.DIASEMANA == diaSemana) &&
                    q.DISPONIBILIDADEGLP_TURNO.Any(r => r.TURNO == turno)
                );
            }
            catch
            {
                throw new Exception("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.");
            }
        }

        public bool EhDisciplinaHabilitadaPor(string agrupamento, double num_func) 
        {
            try
            {
                var qry = SessaoAuditada.CreateSQLQuery(@"
                SELECT  COUNT(A.AGRUPAMENTO)
                FROM [dbo].[LY_GRUPO_HABILITACAO_DOC] A
                INNER JOIN LY_GRUPO_HABILITACAO B
                ON A.AGRUPAMENTO = B.AGRUPAMENTO
                WHERE NUM_FUNC = :NUM_FUNC
                AND A.AGRUPAMENTO = :AGRUPAMENTO
                ");

                qry.SetParameter<string>("AGRUPAMENTO", agrupamento);
                qry.SetParameter<double?>("NUM_FUNC", num_func);

                var qtd = qry.UniqueResult();
                return Convert.ToInt32(qtd) > 0;
            }
            catch
            {
                throw new Exception("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.");
            }
        }

        private int InsereAuditadaDISPONIBILIDADEGLP(DISPONIBILIDADEGLP entidade)
        {
            try
            {
                var qry = SessaoAuditada.CreateSQLQuery(@"
                    INSERT INTO RecursosHumanos.DISPONIBILIDADEGLP (AGRUPAMENTO, NUM_FUNC, ANO, USUARIOID, DATACADASTRO, DATAALTERACAO)
                    VALUES (:AGRUPAMENTO, :NUM_FUNC, :ANO, :USUARIOID, :DATACADASTRO, :DATAALTERACAO);

                    SELECT IDENT_CURRENT('RecursosHumanos.DISPONIBILIDADEGLP')
                    ");

                qry.SetParameter<string>("AGRUPAMENTO", entidade.LY_GRUPO_HABILITACAO.AGRUPAMENTO);
                qry.SetParameter<double?>("NUM_FUNC", entidade.LY_DOCENTE.NUM_FUNC);
                qry.SetParameter<int>("ANO", entidade.ANO);
                qry.SetParameter<string>("USUARIOID", entidade.USUARIOID);
                qry.SetParameter<DateTime>("DATACADASTRO", entidade.DATACADASTRO);
                qry.SetParameter<DateTime>("DATAALTERACAO", entidade.DATAALTERACAO);

                var id = qry.UniqueResult();
                entidade.DISPONIBILIDADEGLPID = Convert.ToInt32(id);
                return entidade.DISPONIBILIDADEGLPID;
            }
            catch
            {
                throw new Exception("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.");
            }
        }

        private int InsereAuditadaDISPONIBILIDADEGLP_DIASEMANA(int disponibilidadeGlpId, int value)
        {
            try
            {
                var qry = SessaoAuditada.CreateSQLQuery(@"
                    INSERT INTO RecursosHumanos.DISPONIBILIDADEGLP_DIASEMANA (DISPONIBILIDADEGLPID, DIASEMANA)
                    VALUES (:DISPONIBILIDADEGLPID, :DIASEMANA);
                    ");

                qry.SetParameter<int>("DISPONIBILIDADEGLPID", disponibilidadeGlpId);
                qry.SetParameter<int>("DIASEMANA", value);

                var id = qry.UniqueResult();
                return Convert.ToInt32(id);
            }
            catch
            {
                throw new Exception("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.");
            }
        }

        private int InsereAuditadaDISPONIBILIDADEGLP_MODALIDADE(int disponibilidadeGlpId, string value)
        {
            try
            {
                var qry = SessaoAuditada.CreateSQLQuery(@"
                    INSERT INTO RecursosHumanos.DISPONIBILIDADEGLP_MODALIDADE (DISPONIBILIDADEGLPID, MODALIDADE)
                    VALUES (:DISPONIBILIDADEGLPID, :MODALIDADE);
                    ");

                qry.SetParameter<int>("DISPONIBILIDADEGLPID", disponibilidadeGlpId);
                qry.SetParameter<string>("MODALIDADE", value);

                var id = qry.UniqueResult();
                return Convert.ToInt32(id);
            }
            catch
            {
                throw new Exception("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.");
            }
        }

        private int InsereAuditadaDISPONIBILIDADEGLP_TURNO(int disponibilidadeGlpId, string value)
        {
            try
            {
                var qry = SessaoAuditada.CreateSQLQuery(@"
                    INSERT INTO RecursosHumanos.DISPONIBILIDADEGLP_TURNO (DISPONIBILIDADEGLPID, TURNO)
                    VALUES (:DISPONIBILIDADEGLPID, :TURNO);
                    ");

                qry.SetParameter<int>("DISPONIBILIDADEGLPID", disponibilidadeGlpId);
                qry.SetParameter<string>("TURNO", value);

                var id = qry.UniqueResult();
                return Convert.ToInt32(id);
            }
            catch
            {
                throw new Exception("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.");
            }
        }

        private int InsereAuditadaDISPONIBILIDADEGLP_UNIDADEENSINO(int disponibilidadeGlpId, string value)
        {
            try
            {
                var qry = SessaoAuditada.CreateSQLQuery(@"
                    INSERT INTO RecursosHumanos.DISPONIBILIDADEGLP_UNIDADEENSINO (DISPONIBILIDADEGLPID, UNIDADE_ENS)
                    VALUES (:DISPONIBILIDADEGLPID, :UNIDADEENSINO);
                    ");

                qry.SetParameter<int>("DISPONIBILIDADEGLPID", disponibilidadeGlpId);
                qry.SetParameter<string>("UNIDADEENSINO", value);

                var id = qry.UniqueResult();
                return Convert.ToInt32(id);
            }
            catch
            {
                throw new Exception("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.");
            }
        }

        public void AlteraAuditada(DISPONIBILIDADEGLP entidade)
        {
            try
            {
                SessaoAuditada.SaveOrUpdate(entidade);
                SessaoAuditada.Flush();
            }
            catch
            {
                TransacaoRollback();
                throw;
            }
        }

        public void ExcluiAuditada(int disponibilidadeGlpId, string unidadeEnsino)
        {
            using (var transaction = SessaoAuditada.BeginTransaction())
            {
                try
                {
                    ISQLQuery qry;

                    qry = SessaoAuditada.CreateSQLQuery(@"DELETE FROM RecursosHumanos.DISPONIBILIDADEGLP_UNIDADEENSINO WHERE DISPONIBILIDADEGLPID = :DISPONIBILIDADEGLPID and UNIDADE_ENS = :UNIDADE_ENS;");
                    qry.SetParameter<int>("DISPONIBILIDADEGLPID", disponibilidadeGlpId);
                    qry.SetParameter<string>("UNIDADE_ENS", unidadeEnsino);
                    qry.ExecuteUpdate();

                    qry = SessaoAuditada.CreateSQLQuery(@"SELECT COUNT(0) FROM RecursosHumanos.DISPONIBILIDADEGLP_UNIDADEENSINO WHERE DISPONIBILIDADEGLPID = :DISPONIBILIDADEGLPID;");
                    qry.SetParameter<int>("DISPONIBILIDADEGLPID", disponibilidadeGlpId);
                    bool naoTemMaisEscolas = qry.UniqueResult<int>() == 0;

                    if (naoTemMaisEscolas)
                    {
                        qry = SessaoAuditada.CreateSQLQuery(@"
                        DELETE FROM RecursosHumanos.DISPONIBILIDADEGLP_MODALIDADE WHERE DISPONIBILIDADEGLPID = :DISPONIBILIDADEGLPID;
                        DELETE FROM RecursosHumanos.DISPONIBILIDADEGLP_DIASEMANA WHERE DISPONIBILIDADEGLPID = :DISPONIBILIDADEGLPID;
                        DELETE FROM RecursosHumanos.DISPONIBILIDADEGLP_TURNO WHERE DISPONIBILIDADEGLPID = :DISPONIBILIDADEGLPID;
                        DELETE FROM RecursosHumanos.DISPONIBILIDADEGLP WHERE DISPONIBILIDADEGLPID = :DISPONIBILIDADEGLPID;
                        ");
                        qry.SetParameter<int>("DISPONIBILIDADEGLPID", disponibilidadeGlpId);
                        qry.ExecuteUpdate();
                    }
                    
                    transaction.Commit();

                    SessaoAuditada.Flush();
                }
                catch
                {
                    transaction.Rollback();
                    throw new Exception("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.");
                }
            }
        }
    }
}
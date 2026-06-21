using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data.SqlClient;
using Techne.Lyceum.RN.DTOs;
using System.Data;

namespace Techne.Lyceum.RN.RecursosHumanos
{
    public class CargaHNaoTrabMes : RNBase
    {
        public List<DadosFrequenciaGLP> ListaFrequenciaGLP(int ano, int mes, DateTime dataInicio, DateTime dataFim, decimal? id_regional, string municipio, string faculdade)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();

            try
            {
                return ListaFrequenciaGLP(ctx, ano, mes, dataInicio, dataFim,id_regional, municipio, faculdade);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Empty;
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }
        }

        public List<DadosFrequenciaGLP> ListaFrequenciaGLP(DataContext ctx, int ano, int mes, DateTime dataInicio, DateTime dataFim, decimal? id_regional, string municipio, string faculdade)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            List<DadosFrequenciaGLP> lista = new List<DadosFrequenciaGLP>();

            if (ano < 1900 || !(mes >= 1 && mes <= 12))
                return lista;

            try
            {
                contextQuery.Command = @" declare @MES_EXTENSO varchar(max) = upper(FORMAT(convert(datetime, '1900-' + convert(varchar, @MES) + '-01'), 'MMMM', 'pt-BR'));

                    WITH 

                    CTE(dt) AS
                    (
	                    SELECT @DATAINICIO
	                    UNION ALL
	                    SELECT DATEADD(d, 1, dt) FROM CTE
	                    WHERE dt < @DATAFIM
                    )

                    , q1 as
                    (
	                    select
	                    @ANO as ANO_FILTRADO
	                    , @MES as MES_FILTRADO
	                    , @MES_EXTENSO as MES_EXTENSO
                        , lyad.NUM_FUNC as NUM_FUNC
	                    , reg.ID_REGIONAL as ID_REGIONAL
	                    , reg.REGIONAL as NOME_REGIONAL
	                    , mun.MUNICIPIO as ID_MUNICIPIO
	                    , mun.NOME as NOME_MUNICIPIO
	                    , lyad.FACULDADE as UNIDADE_ENS
	                    , lyue.NOME_COMP as NOME_UNIDADE_ENS
	                    --, lyue.SETOR as SETOR_UNIDADE_ENS
                        , VWS.UA_ATUAL as SETOR_UNIDADE_ENS
	                    , lyp.IDFUNCIONAL as IDFUNCIONAL
	                    , lyd.VINCULO as VINCULO
	                    , lyd.MATRICULA as MATRICULA
                        , lyp.NOME_COMPL AS NOME
	                    , lyadt.DATA_INICIO as DATA_INICIO
	                    , lyadt.DATA_FIM as DATA_FIM
	                    , count(lyad.DIA_SEMANA) * (
		                    select count(0) 
		                    from CTE 
		                    where  dt between lyadt.DATA_INICIO and lyadt.DATA_FIM 
                            --(dt between (case when month(min(lyadt.DATA_INICIO)) = @MES then min(lyadt.DATA_INICIO) else @DATAINICIO end) 
		                    --and @DATAFIM) 
                            and datepart (dw, dt) = lyad.DIA_SEMANA
	                    ) as CH_MENSAL,
                        (CONVERT (VARCHAR, lyp.IDFUNCIONAL) + '/' + CONVERT (VARCHAR, lyd.VINCULO)) AS IDVINCULO

	                    from 
	                    LY_AULA_DOCENTE lyad
	                    INNER JOIN DBO.LY_AULA_DOCENTE_TIPO lyadt (NOLOCK)
		                    on 1=1
		                    and lyadt.ANO = lyad.ANO
		                    and lyadt.AULA = lyad.AULA
		                    and lyadt.DATA_INICIO = lyad.DATA_INICIO
		                    and lyadt.DIA_SEMANA = lyad.DIA_SEMANA
		                    and lyadt.DISCIPLINA = lyad.DISCIPLINA
		                    and lyadt.FACULDADE = lyad.FACULDADE
		                    and lyadt.NUM_FUNC = lyad.NUM_FUNC
		                    and lyadt.SEMESTRE = lyad.SEMESTRE
		                    and lyadt.TURMA = lyad.TURMA
		                    and lyadt.TURNO = lyad.TURNO
		                    and lyadt.TIPO_AULA in ( 'GLP','NGLP')
	                    inner join DBO.LY_DOCENTE lyd (NOLOCK) 
		                    on 1=1
		                    and lyad.NUM_FUNC = lyd.NUM_FUNC 
		                    and lyadt.NUM_FUNC = lyd.NUM_FUNC
	                    inner join LY_PESSOA lyp on lyp.PESSOA = lyd.PESSOA
	                    inner join LY_UNIDADE_ENSINO lyue on lyue.UNIDADE_ENS = lyad.FACULDADE
	                    inner join TCE_REGIONAL reg on reg.ID_REGIONAL = lyue.ID_REGIONAL
	                    inner join HADES..HD_MUNICIPIO mun on mun.MUNICIPIO = lyue.MUNICIPIO
                        INNER JOIN HADES..VW_SETOR VWS ON VWS.SETOR = lyue.SETOR

	                    where 1=1
	                    and (@ANO between datepart(yyyy, lyadt.DATA_INICIO) and datepart(yyyy, lyadt.DATA_FIM) or @ANO is null)
	                    and (@MES between datepart(mm, lyadt.DATA_INICIO) and datepart(mm, lyadt.DATA_FIM) or @MES is null)
	                    and (@ID_REGIONAL = lyue.ID_REGIONAL or @ID_REGIONAL is null)
	                    and (@MUNICIPIO = lyue.MUNICIPIO or @MUNICIPIO is null)
	                    and (@FACULDADE = lyue.UNIDADE_ENS or @FACULDADE is null)
	                    and (lyadt.DATA_FIM > lyadt.DATA_INICIO)

	                    group by
                        lyad.NUM_FUNC
	                    , reg.ID_REGIONAL
	                    , reg.REGIONAL
	                    , mun.MUNICIPIO
	                    , mun.NOME
	                    , lyad.FACULDADE
	                    , lyue.NOME_COMP
	                    --, lyue.SETOR
                        , VWS.UA_ATUAL 
	                    , lyp.IDFUNCIONAL
	                    , lyd.VINCULO
	                    , lyd.MATRICULA
                        , lyp.NOME_COMPL
	                    , lyadt.DATA_INICIO
	                    , lyadt.DATA_FIM
	                    , lyad.DIA_SEMANA

                    )

                    select
                    q1.ANO_FILTRADO
                    , q1.MES_FILTRADO
                    , q1.MES_EXTENSO --as [Mês]
                    , q1.NUM_FUNC
                    , q1.ID_REGIONAL
                    , q1.NOME_REGIONAL --as [Regional GLP]
                    , q1.ID_MUNICIPIO
                    , q1.NOME_MUNICIPIO --as [Muni. GLP]
                    , q1.UNIDADE_ENS
                    , q1.NOME_UNIDADE_ENS --as [U. E. GLP]
                    , q1.SETOR_UNIDADE_ENS --as [U. A. GLP]
                    , q1.IDFUNCIONAL --as [ID]
                    , q1.VINCULO --as [V]
                    , q1.MATRICULA --as [Matrícula]
                    , q1.NOME
                    , min(q1.DATA_INICIO) as DATA_INICIO --as [Início]
                    , max(q1.DATA_FIM) as DATA_FIM --as [Fim]
                    , isnull(chnt.CHMENSAL, sum(q1.CH_MENSAL)) as CH_MENSAL --as [CH Mensal]
                    , chnt.ID_CARGAHNAOTRABMES
                    , chnt.ANO
                    , chnt.MES
                    , isnull(chnt.CHNAOTRABALHADAMES, 0) as CHNAOTRABALHADAMES
                    , isnull(chnt.CHMENSAL, sum(q1.CH_MENSAL)) - isnull(chnt.CHNAOTRABALHADAMES, 0) as CH_MENSAL_TOTAL
                    ,q1.IDVINCULO
                    from q1
                    left join RecursosHumanos.CARGAHNAOTRABMES chnt 
	                    on 1=1
	                    and chnt.NUM_FUNC = q1.NUM_FUNC 
	                    and chnt.UNIDADE_ENS = q1.UNIDADE_ENS 
	                    and chnt.ANO = @ANO 
	                    and chnt.MES = @MES

                    group by
                    q1.ANO_FILTRADO
                    , q1.MES_FILTRADO
                    , q1.MES_EXTENSO
                    , q1.NUM_FUNC
                    , q1.ID_REGIONAL
                    , q1.NOME_REGIONAL
                    , q1.ID_MUNICIPIO
                    , q1.NOME_MUNICIPIO
                    , q1.UNIDADE_ENS
                    , q1.NOME_UNIDADE_ENS
                    , q1.SETOR_UNIDADE_ENS
                    , q1.IDFUNCIONAL
                    , q1.VINCULO
                    , q1.MATRICULA
                    , q1.NOME
                    , chnt.ID_CARGAHNAOTRABMES
                    , chnt.ANO
                    , chnt.MES
                    , chnt.CHNAOTRABALHADAMES
                    ,CHMENSAL
                    ,q1.IDVINCULO

                    order by
                    q1.ANO_FILTRADO
                    , q1.MES_FILTRADO
                    , q1.MES_EXTENSO
                    , q1.NUM_FUNC
                    , q1.ID_REGIONAL
                    , q1.NOME_REGIONAL
                    , q1.ID_MUNICIPIO
                    , q1.NOME_MUNICIPIO
                    , q1.UNIDADE_ENS
                    , q1.NOME_UNIDADE_ENS
                    , q1.SETOR_UNIDADE_ENS
                    , q1.IDFUNCIONAL
                    , q1.VINCULO
                    , q1.MATRICULA
                    , DATA_INICIO
                    , DATA_FIM
                    , CH_MENSAL
                    , chnt.ID_CARGAHNAOTRABMES
                    , chnt.ANO
                    , chnt.MES
                    , CHNAOTRABALHADAMES
                    , CH_MENSAL_TOTAL
                ";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@MES", mes);
                contextQuery.Parameters.Add("@DATAINICIO", dataInicio);
                contextQuery.Parameters.Add("@DATAFIM", dataFim);
                contextQuery.Parameters.Add("@ID_REGIONAL", id_regional);
                contextQuery.Parameters.Add("@MUNICIPIO", municipio);
                contextQuery.Parameters.Add("@FACULDADE", faculdade);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                    lista.Add(new DadosFrequenciaGLP
                    {
                        ANO_FILTRADO = Convert.ToInt32(reader["ANO_FILTRADO"]),
                        MES_FILTRADO = Convert.ToInt32(reader["MES_FILTRADO"]),
                        NUM_FUNC = Convert.ToInt32(reader["NUM_FUNC"]),
                        MES_EXTENSO = Convert.ToString(reader["MES_EXTENSO"]),
                        ID_REGIONAL = Convert.ToInt32(reader["ID_REGIONAL"]),
                        NOME_REGIONAL = Convert.ToString(reader["NOME_REGIONAL"]),
                        ID_MUNICIPIO = Convert.ToString(reader["ID_MUNICIPIO"]),
                        NOME_MUNICIPIO = Convert.ToString(reader["NOME_MUNICIPIO"]),
                        UNIDADE_ENS = Convert.ToString(reader["UNIDADE_ENS"]),
                        NOME_UNIDADE_ENS = Convert.ToString(reader["NOME_UNIDADE_ENS"]),
                        SETOR_UNIDADE_ENS = Convert.ToString(reader["SETOR_UNIDADE_ENS"]),
                        IDFUNCIONAL = reader["IDFUNCIONAL"] != DBNull.Value ? Convert.ToInt32(reader["IDFUNCIONAL"]) as int? : null,
                        VINCULO = reader["VINCULO"] != DBNull.Value ? Convert.ToInt32(reader["VINCULO"]) as int? : null,
                        MATRICULA = Convert.ToString(reader["MATRICULA"]),
                        NOME = Convert.ToString(reader["NOME"]),
                        DATA_INICIO = Convert.ToDateTime(reader["DATA_INICIO"]),
                        DATA_FIM = Convert.ToDateTime(reader["DATA_FIM"]),
                        CH_SEMANAL = null,
                        CH_MENSAL = Convert.ToInt32(reader["CH_MENSAL"]),
                        ID_CARGAHNAOTRABMES = reader["ID_CARGAHNAOTRABMES"] != DBNull.Value ? Convert.ToInt32(reader["ID_CARGAHNAOTRABMES"]) as int? : null,
                        ANO = reader["ANO"] != DBNull.Value ? Convert.ToInt32(reader["ANO"]) as int? : null,
                        MES = reader["MES"] != DBNull.Value ? Convert.ToInt32(reader["MES"]) as int? : null,
                        CHNAOTRABALHADAMES = Convert.ToInt32(reader["CHNAOTRABALHADAMES"]),
                        CH_MENSAL_TOTAL = Convert.ToInt32(reader["CH_MENSAL_TOTAL"]),
                        IDVINCULO = Convert.ToString(reader["IDVINCULO"]),
                    });

                return lista;
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Empty;
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
            }
        }

        public List<DadosTurmaGLP> ListaTurmaGLP(int ano, int mes, string faculdade, decimal num_func)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            List<DadosTurmaGLP> lista = new List<DadosTurmaGLP>();

            if (ano < 1900 || !(mes >= 1 && mes <= 12))
                return lista;

            try
            {
                contextQuery.Command = @"
                    declare @MES_EXTENSO varchar(max) = upper(FORMAT(convert(datetime, '1900-' + convert(varchar, @MES) + '-01'), 'MMMM', 'pt-BR'))
                    declare @FIRST_DATE_OF_MONTH datetime = convert(datetime, convert(varchar, @ANO) + '-' + convert(varchar, @MES) + '-01')
                    declare @LAST_DATE_OF_MONTH datetime = eomonth(@FIRST_DATE_OF_MONTH);

                    WITH 

                    CTE(dt) AS
                    (
	                    SELECT @FIRST_DATE_OF_MONTH
	                    UNION ALL
	                    SELECT DATEADD(d, 1, dt) FROM CTE
	                    WHERE dt < @LAST_DATE_OF_MONTH
                    )

                    , q1 as
                    (
	                    select
	                    @ANO as ANO
	                    , @MES as MES
	                    , lyad.FACULDADE
	                    , lyad.NUM_FUNC
	                    , @MES_EXTENSO as MES_EXTENSO
	                    , lyad.TURMA
	                    , lydisc.DISCIPLINA
	                    , lydisc.NOME as NOME_DISCIPLINA
	                    , lyadt.DATA_INICIO as DATA_INICIO
	                    , lyadt.DATA_FIM as DATA_FIM
	                    , count(lyad.DIA_SEMANA) * (
		                    select count(0) 
		                    from CTE 
		                    where (dt between (case when month(min(lyadt.DATA_INICIO)) = @MES then min(lyadt.DATA_INICIO) else @FIRST_DATE_OF_MONTH end) 
		                    and @LAST_DATE_OF_MONTH) and datepart (dw, dt) = lyad.DIA_SEMANA
		                   
	                    ) as CH_MENSAL

	                    from 
	                    LY_AULA_DOCENTE lyad
	                    INNER JOIN DBO.LY_AULA_DOCENTE_TIPO lyadt (NOLOCK)
		                    on 1=1
		                    and lyadt.ANO = lyad.ANO
		                    and lyadt.AULA = lyad.AULA
		                    and lyadt.DATA_INICIO = lyad.DATA_INICIO
		                    and lyadt.DIA_SEMANA = lyad.DIA_SEMANA
		                    and lyadt.DISCIPLINA = lyad.DISCIPLINA
		                    and lyadt.FACULDADE = lyad.FACULDADE
		                    and lyadt.NUM_FUNC = lyad.NUM_FUNC
		                    and lyadt.SEMESTRE = lyad.SEMESTRE
		                    and lyadt.TURMA = lyad.TURMA
		                    and lyadt.TURNO = lyad.TURNO
		                    and lyadt.TIPO_AULA = 'GLP'
	                    inner join DBO.LY_DOCENTE lyd (NOLOCK) 
		                    on 1=1
		                    and lyad.NUM_FUNC = lyd.NUM_FUNC 
		                    and lyadt.NUM_FUNC = lyd.NUM_FUNC
	                    inner join LY_PESSOA lyp on lyp.PESSOA = lyd.PESSOA
	                    inner join LY_UNIDADE_ENSINO lyue on lyue.UNIDADE_ENS = lyad.FACULDADE
	                    inner join LY_DISCIPLINA lydisc on lydisc.DISCIPLINA = lyad.DISCIPLINA

	                    where 1=1
	                    and (@ANO between datepart(yyyy, lyadt.DATA_INICIO) and datepart(yyyy, lyadt.DATA_FIM) or @ANO is null)
	                    and (@MES between datepart(mm, lyadt.DATA_INICIO) and datepart(mm, lyadt.DATA_FIM) or @MES is null)
	                    and (@FACULDADE = lyad.FACULDADE or @FACULDADE is null)
	                    and (@NUM_FUNC = lyad.NUM_FUNC or @NUM_FUNC is null)
	                    and (lyadt.DATA_FIM > lyadt.DATA_INICIO)

	                    group by
	                    lyad.FACULDADE
	                    , lyad.NUM_FUNC
	                    , lyad.TURMA
	                    , lydisc.DISCIPLINA
	                    , lydisc.NOME
	                    , lyad.NUM_FUNC
	                    , lyadt.DATA_INICIO
	                    , lyadt.DATA_FIM
	                    , lyad.DIA_SEMANA
                    )

                    select
                    q1.ANO
                    , q1.MES
                    , q1.FACULDADE
                    , q1.NUM_FUNC
                    , q1.MES_EXTENSO
                    , q1.TURMA
                    , q1.DISCIPLINA
                    , q1.NOME_DISCIPLINA
                    , q1.DATA_INICIO
                    , q1.DATA_FIM
                    , isnull(chnt.CHMENSAL, sum(q1.CH_MENSAL)) as CH_MENSAL

                    from 
                    q1
                    left join RecursosHumanos.CARGAHNAOTRABMES chnt 
	                    on 1=1
	                    and chnt.NUM_FUNC = q1.NUM_FUNC 
	                    and chnt.UNIDADE_ENS = q1.FACULDADE 
	                    and chnt.ANO = @ANO 
	                    and chnt.MES = @MES

                    group by
                    q1.ANO
                    , q1.MES
                    , q1.FACULDADE
                    , q1.NUM_FUNC
                    , q1.MES_EXTENSO
                    , q1.TURMA
                    , q1.DISCIPLINA
                    , q1.NOME_DISCIPLINA
                    , q1.DATA_INICIO
                    , q1.DATA_FIM
                    , chnt.ID_CARGAHNAOTRABMES
                    ,CHMENSAL
                ";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@MES", mes);
                contextQuery.Parameters.Add("@FACULDADE", faculdade);
                contextQuery.Parameters.Add("@NUM_FUNC", num_func);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                    lista.Add(new DadosTurmaGLP
                    {
                        ANO = Convert.ToInt32(reader["ANO"]),
                        MES = Convert.ToInt32(reader["MES"]),
                        FACULDADE = Convert.ToString(reader["FACULDADE"]),
                        NUM_FUNC = Convert.ToInt32(reader["NUM_FUNC"]),
                        MES_EXTENSO = Convert.ToString(reader["MES_EXTENSO"]),
                        TURMA = Convert.ToString(reader["TURMA"]),
                        DISCIPLINA = Convert.ToString(reader["DISCIPLINA"]),
                        NOME_DISCIPLINA = Convert.ToString(reader["NOME_DISCIPLINA"]),
                        DATA_INICIO = Convert.ToDateTime(reader["DATA_INICIO"]),
                        DATA_FIM = Convert.ToDateTime(reader["DATA_FIM"]),
                        CH_SEMANAL = null,
                        CH_MENSAL = Convert.ToInt32(reader["CH_MENSAL"]),
                    });

                return lista;
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Empty;
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();

                if (ctx != null)
                    ctx.Dispose();
            }
        }

        public int Insere(DataContext ctx, decimal numFunc, string unidadeEns, int ano, int mes, int chMensal, int? chSemanal, int chNaoTrabalhadaMes, string usuarioId)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @"
                    insert into RecursosHumanos.CARGAHNAOTRABMES (NUM_FUNC, UNIDADE_ENS, ANO, MES, CHMENSAL, CH_SEMANAL, CHNAOTRABALHADAMES, USUARIOID, DATACADASTRO, DATAALTERACAO)
                    values (@NUM_FUNC, @UNIDADE_ENS, @ANO, @MES, @CHMENSAL, @CH_SEMANAL, @CHNAOTRABALHADAMES, @USUARIOID, @DATACADASTRO, @DATAALTERACAO)

                    SELECT IDENT_CURRENT('RecursosHumanos.CARGAHNAOTRABMES')
                ";

                contextQuery.Parameters.Add("@NUM_FUNC", numFunc);
                contextQuery.Parameters.Add("@UNIDADE_ENS", unidadeEns);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@MES", mes);
                contextQuery.Parameters.Add("@CHMENSAL", chMensal);
                contextQuery.Parameters.Add("@CH_SEMANAL", chSemanal);
                contextQuery.Parameters.Add("@CHNAOTRABALHADAMES", chNaoTrabalhadaMes);
                contextQuery.Parameters.Add("@USUARIOID", usuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);

                return Convert.ToInt32(ctx.GetReturnValue(contextQuery));
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
        }

        public int Insere(decimal numFunc, string unidadeEns, int ano, int mes, int chMensal, int? chSemanal, int chNaoTrabalhadaMes, string usuarioId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                return Insere(ctx, numFunc, unidadeEns, ano, mes, chMensal, chSemanal, chNaoTrabalhadaMes, usuarioId);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }
        }

        public void Edita(int id, int chNaoTrabalhadaMes, string usuarioId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                this.Atualiza(contexto, id, chNaoTrabalhadaMes, usuarioId);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                    contexto.Dispose();
            }
        }

        public void Atualiza(DataContext contexto, int id, int chNaoTrabalhadaMes, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"
                    update RecursosHumanos.CARGAHNAOTRABMES set
                    CHNAOTRABALHADAMES = @CHNAOTRABALHADAMES
                    , USUARIOID = @USUARIOID 
                    , DATAALTERACAO = @DATAALTERACAO
                    where ID_CARGAHNAOTRABMES = @ID_CARGAHNAOTRABMES
                ";

            contextQuery.Parameters.Add("@ID_CARGAHNAOTRABMES", id);
            contextQuery.Parameters.Add("@CHNAOTRABALHADAMES", chNaoTrabalhadaMes);
            contextQuery.Parameters.Add("@USUARIOID", usuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public bool PossuiAtualizacaoPor(DataContext ctx, int idCargaHNaoTrabMes, int chNaoTrabalhadaMes)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                            FROM   RecursosHumanos.CARGAHNAOTRABMES (NOLOCK)
                                            WHERE  ID_CARGAHNAOTRABMES = @ID_CARGAHNAOTRABMES
												   AND CHNAOTRABALHADAMES <> @CHNAOTRABALHADAMES  ";

            contextQuery.Parameters.Add("@ID_CARGAHNAOTRABMES", SqlDbType.Int, idCargaHNaoTrabMes);
            contextQuery.Parameters.Add("@CHNAOTRABALHADAMES", SqlDbType.Int, chNaoTrabalhadaMes);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }        
    }
}

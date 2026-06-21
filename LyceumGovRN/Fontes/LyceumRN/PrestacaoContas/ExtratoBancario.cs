using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class ExtratoBancario : RNBase
    {
        public enum Satus
        {
            // status = nulo - "Lançamento pela AAE" (ainda sem analise)
            [StringValue("Enviado para Análise")]
            EnviadoAnalise = 1,
            [StringValue("Devolvido para revisão")]
            DevolvidoRevisao = 2,
            [StringValue("Revisado pela AAE")]
            Revisado = 3,
            [StringValue("Aprovado")]
            Aprovado = 4,
            [StringValue("Reprovado")]
            Reprovado = 5
        }

        public bool PossuiPeriodoReferenciaExtratoBancarioPor(Seeduc.Infra.Data.DataContext contexto, int periodoReferenciaExtratoBancarioId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @"   SELECT COUNT(*) 
                                        FROM PrestacaoContas.EXTRATOBANCARIO (NOLOCK)
                                        WHERE PERIODOREFERENCIAEXTRATOBANCARIOID = @PERIODOREFERENCIAEXTRATOBANCARIOID ";

            contextQuery.Parameters.Add("@PERIODOREFERENCIAEXTRATOBANCARIOID", SqlDbType.Int, periodoReferenciaExtratoBancarioId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiEstratoSemAnalisePor(DataContext contexto, string censo, int ano, int mesInicio, int mesFim)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM PRESTACAOCONTAS.EXTRATOBANCARIO E
                                    WHERE CENSO = @CENSO
                                            AND ANO = @ANO
                                            AND MES BETWEEN @MESINICIO AND @MESFIM
                                            AND (STATUS IS NULL OR STATUS <> 4) ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.Int, censo);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@MESINICIO", SqlDbType.Int, mesInicio);
            contextQuery.Parameters.Add("@MESFIM", SqlDbType.Int, mesFim);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public DataTable ObtemExtratoBancario(int mesInicio, int mesFim, int ano, string unidadeEnsino)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                ContextQuery query = new ContextQuery();

                query.Command = @"
                    select * from PrestacaoContas.EXTRATOBANCARIO eb
                    left join PrestacaoContas.EXTRATOBANCARIOARQUIVO eba on eba.EXTRATOBANCARIOID = eb.EXTRATOBANCARIOID
                    where eb.MES BETWEEN @MESINICIO AND @MESFIM
                    and eb.ANO = @ANO
                    and eb.CENSO = @UNIDADEENSINO
                ";

                query.Parameters.Add("@MESINICIO", SqlDbType.Int, mesInicio);
                query.Parameters.Add("@MESFIM", SqlDbType.Int, mesFim);
                query.Parameters.Add("@ANO", SqlDbType.Int, ano);
                query.Parameters.Add("@UNIDADEENSINO", SqlDbType.VarChar, unidadeEnsino);

                return ctx.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable ObtemExtratoBancario(int mes, int ano, string unidadeEnsino)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                ContextQuery query = new ContextQuery();

                query.Command = @"
                    select * from PrestacaoContas.EXTRATOBANCARIO eb
                    left join PrestacaoContas.EXTRATOBANCARIOARQUIVO eba on eba.EXTRATOBANCARIOID = eb.EXTRATOBANCARIOID
                    where eb.MES = @MES
                    and eb.ANO = @ANO
                    and eb.CENSO = @UNIDADEENSINO
                ";

                query.Parameters.Add("@MES", SqlDbType.Int, mes);
                query.Parameters.Add("@ANO", SqlDbType.Int, ano);
                query.Parameters.Add("@UNIDADEENSINO", SqlDbType.VarChar, unidadeEnsino);

                return ctx.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable ObtemExtratoBancario(int extratoBancarioId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.ObtemExtratoBancario(ctx, extratoBancarioId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable ObtemExtratoBancario(DataContext ctx, int extratoBancarioId)
        {
            ContextQuery query = new ContextQuery();

            query.Command = @"
                    select * from PrestacaoContas.EXTRATOBANCARIO eb
                    left join PrestacaoContas.EXTRATOBANCARIOARQUIVO eba on eba.EXTRATOBANCARIOID = eb.EXTRATOBANCARIOID
                    where eb.EXTRATOBANCARIOID = @EXTRATOBANCARIOID
                ";

            query.Parameters.Add("@EXTRATOBANCARIOID", SqlDbType.Int, extratoBancarioId);

            return ctx.GetDataTable(query);
        }

        public DataTable ListaExtratoBancario(int? mes, int? ano, string unidadeEnsino)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                ContextQuery query = new ContextQuery();

                query.Command = @"
                    select 
                    eb.EXTRATOBANCARIOID
                    , eba.EXTRATOBANCARIOARQUIVOID
                    , eb.CENSO
                    , ue.NOME_COMP
                    , eb.MES
                    , eb.ANO
                    , eb.STATUS
                    , case 
	                    when eb.STATUS is null then 'Lançamento pela AAE'
	                    when eb.STATUS = 1 then 'Enviado para análise'
	                    when eb.STATUS = 2 then 'Devolvido para revisão'
	                    when eb.STATUS = 3 then 'Revisado pela AAE'
	                    when eb.STATUS = 4 then 'Aprovado'
	                    when eb.STATUS = 5 then 'Reprovado'
                      end as STATUSDESCRICAO
                    , eb.OBSERVACAO
                    , eba.TIPOARQUIVO
                    , eba.ARQUIVO

                    from PrestacaoContas.EXTRATOBANCARIO eb
                    inner join LY_UNIDADE_ENSINO ue on ue.UNIDADE_ENS = eb.CENSO
                    left join PrestacaoContas.EXTRATOBANCARIOARQUIVO eba on eba.EXTRATOBANCARIOID = eb.EXTRATOBANCARIOID

                    where (eb.MES = @MES or @MES is null)
                    and (eb.ANO = @ANO or @ANO is null)
                    and eb.CENSO = @UNIDADEENSINO
                ";

                query.Parameters.Add("@MES", SqlDbType.Int, (object)mes ?? DBNull.Value);
                query.Parameters.Add("@ANO", SqlDbType.Int, (object)ano ?? DBNull.Value);
                query.Parameters.Add("@UNIDADEENSINO", SqlDbType.VarChar, (object)unidadeEnsino ?? DBNull.Value);

                return ctx.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable ListaExtratoBancarioReprovado(int? mes, int? ano, string unidadeEnsino)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                ContextQuery query = new ContextQuery();

                query.Command = @"
                    select 
                    eb.EXTRATOBANCARIOID
                    , eba.EXTRATOBANCARIOARQUIVOID
                    , eb.CENSO
                    , ue.NOME_COMP
                    , eb.MES
                    , eb.ANO
                    , eb.STATUS
                    , case 
	                    when eb.STATUS is null then 'Lançamento pela AAE'
	                    when eb.STATUS = 1 then 'Enviado para análise'
	                    when eb.STATUS = 2 then 'Devolvido para revisão'
	                    when eb.STATUS = 3 then 'Revisado pela AAE'
	                    when eb.STATUS = 4 then 'Aprovado'
	                    when eb.STATUS = 5 then 'Reprovado'
                      end as STATUSDESCRICAO
                    , eb.OBSERVACAO
                    , eba.TIPOARQUIVO
                    , eba.ARQUIVO

                    from PrestacaoContas.EXTRATOBANCARIO eb
                    inner join LY_UNIDADE_ENSINO ue on ue.UNIDADE_ENS = eb.CENSO
                    left join PrestacaoContas.EXTRATOBANCARIOARQUIVO eba on eba.EXTRATOBANCARIOID = eb.EXTRATOBANCARIOID

                    where eb.STATUS = 5
                    and (eb.MES = @MES or @MES is null)
                    and (eb.ANO = @ANO or @ANO is null)
                    and eb.CENSO = @UNIDADEENSINO
                ";

                query.Parameters.Add("@MES", SqlDbType.Int, (object)mes ?? DBNull.Value);
                query.Parameters.Add("@ANO", SqlDbType.Int, (object)ano ?? DBNull.Value);
                query.Parameters.Add("@UNIDADEENSINO", SqlDbType.VarChar, (object)unidadeEnsino ?? DBNull.Value);

                return ctx.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ValidacaoDados Valida(RN.PrestacaoContas.DTOs.DadosExtratoBancario eb)
        {
            RN.PeriodoLetivo rnPeriodoLetivo = new RN.PeriodoLetivo();
            RN.UnidadeEnsino rnUnidadeEnsino = new RN.UnidadeEnsino();
            RN.PrestacaoContas.PeriodoReferenciaExtratoBancario rnPeriodoReferenciaExtratoBancario = new PeriodoReferenciaExtratoBancario();
            var validacaoDados = new ValidacaoDados();
            var mensagens = new List<string>();
            var ehInsercao = eb.ExtratoBancarioId == 0;

            if (ehInsercao)
            {
                if (eb.Ano == 0)
                    mensagens.Add("ANO: Preenchimento obrigatório");

                if (!new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }.Contains(eb.Mes))
                    mensagens.Add("MÊS: O mês especificado não existe");

                if (eb.Censo.IsNullOrEmptyOrWhiteSpace())
                    mensagens.Add("UNIDADE DE ENSINO: Preenchimento obrigatório");

                if (eb.Status.HasValue)
                    mensagens.Add("STATUS: Para inserção do extrato bancário, é obrigatório ser nulo (Lançamento pela AAE)");
            }

            if (ehInsercao || (!ehInsercao && eb.Arquivo.Length > 0))
            {
                if (eb.Arquivo.Length == 0)
                    mensagens.Add("ARQUIVO DO EXTRATO: Preenchimento obrigatório");
                else
                {
                    if (!new string[] { "image/jpeg", "application/pdf" }.Contains(eb.TipoArquivo))
                        mensagens.Add("TIPO DO ARQUIVO DO EXTRATO: Não pode ser diferente de JPG ou PDF");

                    if (eb.Arquivo.Length > 15728640) // 15mb
                        mensagens.Add("ARQUIVO DO EXTRATO: Não pode ter mais do que 15MB");

                    if (eb.NomeArquivo.Length > 500)
                        mensagens.Add("NOME DO ARQUIVO DO EXTRATO: Não pode ter mais do que 500 caracteres");
                }
            }

            if (eb.Observacao.Length > 500)
                mensagens.Add("OBSERVAÇÃO: Não pode ter mais do que 500 caracteres");

            if (eb.UsuarioId.IsNullOrEmptyOrWhiteSpace())
                mensagens.Add("USUÁRIO ID: Preenchimento obrigatório");

            if (eb.UsuarioId.Length > 15)
                mensagens.Add("USUÁRIO ID: Não pode ter mais do que 15 caracteres");

            if (mensagens.Count == 0)
            {
                DataContext contexto = null;

                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (!rnPeriodoLetivo.EhAnoLetivoPor(contexto, eb.Ano))
                        mensagens.Add("ANO: O ano especificado não é um ano letivo");

                    if (!rnUnidadeEnsino.ExisteUnidadeEnsinoCadastradaPor(contexto, eb.Censo))
                        mensagens.Add("UNIDADE DE ENSINO: O censo informado não existe cadastrado no banco de dados");

                    //busca id periodo de referencia do extrato
                    eb.PeriodoReferenciaExtratoBancarioId = rnPeriodoReferenciaExtratoBancario.ObtemPeriodoReferenciaIdPor(contexto, eb.Ano, eb.Mes);
                    if (eb.PeriodoReferenciaExtratoBancarioId <= 0)
                        mensagens.Add("ANO / MES: Não foi encontrado um período de referência para extrato bancário no primeiro dia do ano / mes selecionados.");

                    if (!RN.Usuarios.VerificaHabilitado(eb.UsuarioId))
                        mensagens.Add("USUÁRIO ID: Não está habilitado no sistema");

                }
                catch (Exception ex)
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (contexto != null)
                    {
                        contexto.Dispose();
                    }
                }
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public void Insere(RN.PrestacaoContas.DTOs.DadosExtratoBancario eb)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    ContextQuery query = new ContextQuery();

                    query.Command = @"
                        declare @EXTRATOBANCARIOID int
                        declare @EXTRATOBANCARIOARQUIVOID int
    
                        insert into PrestacaoContas.EXTRATOBANCARIO (
                            PERIODOREFERENCIAEXTRATOBANCARIOID
                            ,CENSO
                            ,MES
                            ,ANO
                            ,OBSERVACAO
                            ,USUARIOID
                            ,DATACADASTRO
                            ,DATAALTERACAO
                        ) values (
                            @PERIODOREFERENCIAEXTRATOBANCARIOID
                            ,@CENSO
                            ,@MES
                            ,@ANO
                            ,@OBSERVACAO
                            ,@USUARIOID
                            ,@DATACADASTRO
                            ,@DATACADASTRO
                        )
    
                        select @EXTRATOBANCARIOID = EXTRATOBANCARIOID
                        from PrestacaoContas.EXTRATOBANCARIO
                        where PERIODOREFERENCIAEXTRATOBANCARIOID = @PERIODOREFERENCIAEXTRATOBANCARIOID
                        and CENSO = @CENSO
                        and MES = @MES
                        and ANO = @ANO
    
                        insert into PrestacaoContas.EXTRATOBANCARIOARQUIVO (
                            EXTRATOBANCARIOID
                            ,CHAVEARQUIVO
                            ,ARQUIVO
                            ,TIPOARQUIVO
                            ,NOMEARQUIVO
                            ,USUARIOID
                            ,DATACADASTRO
                            ,DATAALTERACAO
                        ) values (
                            @EXTRATOBANCARIOID
                            ,NEWID()
                            ,@ARQUIVO
                            ,@TIPOARQUIVO
                            ,@NOMEARQUIVO
                            ,@USUARIOID
                            ,@DATACADASTRO
                            ,@DATACADASTRO
                        )
    
                        select @EXTRATOBANCARIOARQUIVOID = EXTRATOBANCARIOARQUIVOID
                        from PrestacaoContas.EXTRATOBANCARIOARQUIVO
                        where EXTRATOBANCARIOID = @EXTRATOBANCARIOID
                        and ARQUIVO = @ARQUIVO
                        and TIPOARQUIVO = @TIPOARQUIVO
                        and NOMEARQUIVO = @NOMEARQUIVO
    
                        insert into Poseidon.PrestacaoContas.EXTRATOBANCARIOARQUIVO (
                            EXTRATOBANCARIOARQUIVOID
                            ,EXTRATOBANCARIOID
                            ,CHAVEARQUIVO
                            ,ARQUIVO
                            ,TIPOARQUIVO
                            ,NOMEARQUIVO
                            ,USUARIOID
                            ,DATACADASTRO
                            ,DATAALTERACAO
                            ,DATAAUDITORIA
                            ,OPERACAO
                            ,ESTACAO
                        ) values (
                            @EXTRATOBANCARIOARQUIVOID
                            ,@EXTRATOBANCARIOID
                            ,NEWID()
                            ,@ARQUIVO
                            ,@TIPOARQUIVO
                            ,@NOMEARQUIVO
                            ,@USUARIOID
                            ,@DATACADASTRO
                            ,@DATACADASTRO
                            ,@DATACADASTRO
                            ,@OPERACAO
                            ,@ESTACAO
                        )
                    ";

                    query.Parameters.Add("@ANO", SqlDbType.Int, eb.Ano);
                    query.Parameters.Add("@PERIODOREFERENCIAEXTRATOBANCARIOID", SqlDbType.Int, eb.PeriodoReferenciaExtratoBancarioId);
                    query.Parameters.Add("@MES", SqlDbType.Int, eb.Mes);
                    query.Parameters.Add("@CENSO", SqlDbType.VarChar, eb.Censo);
                    query.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, eb.Arquivo);
                    query.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, eb.TipoArquivo);
                    query.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, eb.NomeArquivo);
                    query.Parameters.Add("@OBSERVACAO", SqlDbType.VarChar, eb.Observacao);
                    query.Parameters.Add("@USUARIOID", SqlDbType.VarChar, eb.UsuarioId);
                    query.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                    query.Parameters.Add("@OPERACAO", SqlDbType.VarChar, "INSERE");
                    query.Parameters.Add("@ESTACAO", SqlDbType.VarChar, System.Web.HttpContext.Current.Request.UserHostAddress);

                    ctx.ApplyModifications(query);
                }
            }
            catch (Exception ex)
            {
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
        }

        public void Atualiza(RN.PrestacaoContas.DTOs.DadosExtratoBancario eb)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    ContextQuery query = new ContextQuery();

                    query.Command = @"
                        update PrestacaoContas.EXTRATOBANCARIO set
                            OBSERVACAO = @OBSERVACAO
                            ,USUARIOID = @USUARIOID
                            ,DATAALTERACAO = @DATAALTERACAO
                        where EXTRATOBANCARIOID = @EXTRATOBANCARIOID
    
                        if (DATALENGTH(@ARQUIVO) > 0)
                        begin
                            update PrestacaoContas.EXTRATOBANCARIOARQUIVO set
                                CHAVEARQUIVO = @CHAVEARQUIVO
                                ,ARQUIVO = @ARQUIVO
                                ,TIPOARQUIVO = @TIPOARQUIVO
                                ,NOMEARQUIVO = @NOMEARQUIVO
                                ,USUARIOID = @USUARIOID
                                ,DATAALTERACAO = @DATAALTERACAO
                            where EXTRATOBANCARIOID = @EXTRATOBANCARIOID
                        end

                        declare @EXTRATOBANCARIOARQUIVOID int
                        declare @DATACADASTRO DateTime
    
                        select 
                        @EXTRATOBANCARIOARQUIVOID = EXTRATOBANCARIOARQUIVOID
                        ,@DATACADASTRO = DATACADASTRO
                        from PrestacaoContas.EXTRATOBANCARIOARQUIVO
                        where EXTRATOBANCARIOID = @EXTRATOBANCARIOID
                        --and CHAVEARQUIVO = @CHAVEARQUIVO
    
                        insert into Poseidon.PrestacaoContas.EXTRATOBANCARIOARQUIVO (
                            EXTRATOBANCARIOARQUIVOID
                            ,EXTRATOBANCARIOID
                            ,CHAVEARQUIVO
                            ,ARQUIVO
                            ,TIPOARQUIVO
                            ,NOMEARQUIVO
                            ,USUARIOID
                            ,DATACADASTRO
                            ,DATAALTERACAO
                            ,DATAAUDITORIA
                            ,OPERACAO
                            ,ESTACAO
                        ) values (
                            @EXTRATOBANCARIOARQUIVOID
                            ,@EXTRATOBANCARIOID
                            ,NEWID()
                            ,@ARQUIVO
                            ,@TIPOARQUIVO
                            ,@NOMEARQUIVO
                            ,@USUARIOID
                            ,@DATACADASTRO
                            ,@DATAALTERACAO
                            ,@DATAALTERACAO
                            ,@OPERACAO
                            ,@ESTACAO
                        )
                    ";

                    query.Parameters.Add("@EXTRATOBANCARIOID", SqlDbType.Int, eb.ExtratoBancarioId);
                    query.Parameters.Add("@CHAVEARQUIVO", SqlDbType.UniqueIdentifier, Guid.NewGuid());
                    query.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, eb.Arquivo);
                    query.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, eb.TipoArquivo);
                    query.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, eb.NomeArquivo);
                    query.Parameters.Add("@OBSERVACAO", SqlDbType.VarChar, eb.Observacao);
                    query.Parameters.Add("@USUARIOID", SqlDbType.VarChar, eb.UsuarioId);
                    query.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                    query.Parameters.Add("@OPERACAO", SqlDbType.VarChar, "ATUALIZA");
                    query.Parameters.Add("@ESTACAO", SqlDbType.VarChar, System.Web.HttpContext.Current.Request.UserHostAddress);

                    ctx.ApplyModifications(query);
                }
            }
            catch (Exception ex)
            {
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
        }

        public void AtualizaStatus(int extratoBancarioId, int? status)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    ContextQuery query = new ContextQuery();

                    query.Command = @"
                        update PrestacaoContas.EXTRATOBANCARIO set
                            STATUS = @STATUS
                            ,USUARIOID = @USUARIOID
                            ,DATAALTERACAO = @DATAALTERACAO
                        where EXTRATOBANCARIOID = @EXTRATOBANCARIOID
                    ";

                    query.Parameters.Add("@EXTRATOBANCARIOID", SqlDbType.Int, extratoBancarioId);
                    query.Parameters.Add("@STATUS", SqlDbType.Int, (object)status ?? DBNull.Value);
                    query.Parameters.Add("@USUARIOID", SqlDbType.VarChar, System.Web.HttpContext.Current.User.Identity.Name);
                    query.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                    ctx.ApplyModifications(query);
                }
            }
            catch (Exception ex)
            {
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
        }

        public bool EstaComJustificativasEmBranco(DataContext ctx, int extratoBancarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT COUNT(*) 
                                      FROM   PrestacaoContas.EXIGENCIAEXTRATO (NOLOCK) 
                                      WHERE  EXTRATOBANCARIOID = @EXTRATOBANCARIOID 
                                      AND isnull(JUSTIFICATIVA, '') = '' ";

            contextQuery.Parameters.Add("@EXTRATOBANCARIOID", SqlDbType.Int, extratoBancarioId);

            return ctx.GetReturnValue<int>(contextQuery) > 0;
        }

        public ValidacaoDados ValidaAtualizacaoStatus(int extratoBancarioId, int? status)
        {
            var validacaoDados = new ValidacaoDados();
            var mensagens = new List<string>();

            if (mensagens.Count == 0)
            {
                DataContext contexto = null;

                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (status.HasValue && EstaComJustificativasEmBranco(contexto, extratoBancarioId))
                        mensagens.Add("EXIGÊNCIAS DO EXTRATO: Existem exigências que estão com a justificativa em branco. É necessário preenchê-las");
                }
                catch (Exception ex)
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (contexto != null)
                    {
                        contexto.Dispose();
                    }
                }
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public int? ObtemStatus(int extratoBancarioId)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                    return ObtemStatus(ctx, extratoBancarioId);
            }
            catch (Exception ex)
            {
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
        }

        public int? ObtemStatus(DataContext ctx, int extratoBancarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT STATUS 
                                      FROM   PrestacaoContas.EXTRATOBANCARIO (NOLOCK) 
                                      WHERE  EXTRATOBANCARIOID = @EXTRATOBANCARIOID ";

            contextQuery.Parameters.Add("@EXTRATOBANCARIOID", SqlDbType.Int, extratoBancarioId);

            return ctx.GetReturnValue<int?>(contextQuery);
        }

        public bool Existe(DataContext ctx, int extratoBancarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT COUNT(*) 
                                      FROM   PrestacaoContas.EXTRATOBANCARIO (NOLOCK) 
                                      WHERE  EXTRATOBANCARIOID = @EXTRATOBANCARIOID ";

            contextQuery.Parameters.Add("@EXTRATOBANCARIOID", SqlDbType.Int, extratoBancarioId);

            return ctx.GetReturnValue<int>(contextQuery) > 0;
        }

        public bool PossuiExigenciasNaoAnalisadas(int extratoBancarioId)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                    return PossuiExigenciasNaoAnalisadas(ctx, extratoBancarioId);
            }
            catch (Exception ex)
            {
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
        }

        public bool PossuiExigenciasNaoAnalisadas(DataContext ctx, int extratoBancarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT COUNT(*) 
                                      FROM   PrestacaoContas.EXIGENCIAEXTRATO (NOLOCK) 
                                      WHERE  EXTRATOBANCARIOID = @EXTRATOBANCARIOID 
                                      AND APROVADO IS NULL ";

            contextQuery.Parameters.Add("@EXTRATOBANCARIOID", SqlDbType.Int, extratoBancarioId);

            return ctx.GetReturnValue<int>(contextQuery) > 0;
        }

        public bool PossuiExigenciasReprovadas(int extratoBancarioId)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                    return PossuiExigenciasReprovadas(ctx, extratoBancarioId);
            }
            catch (Exception ex)
            {
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
        }

        public bool PossuiExigenciasReprovadas(DataContext ctx, int extratoBancarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT COUNT(*) 
                                      FROM   PrestacaoContas.EXIGENCIAEXTRATO (NOLOCK) 
                                      WHERE  EXTRATOBANCARIOID = @EXTRATOBANCARIOID 
                                      AND APROVADO = 0 ";

            contextQuery.Parameters.Add("@EXTRATOBANCARIOID", SqlDbType.Int, extratoBancarioId);

            return ctx.GetReturnValue<int>(contextQuery) > 0;
        }

        public bool VerificaEnvioSEI(int extratoBancarioId)
        {

            var extratoBancario = ObtemExtratoBancario(extratoBancarioId);
            var row = extratoBancario.Rows[0];
            var periodoreferencia = Convert.ToInt32(row["PERIODOREFERENCIAEXTRATOBANCARIOID"]);
            var censo = Convert.ToString(row["CENSO"]);



            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;

            contextQuery.Command = @"SELECT count([IMPORTACAOSEIID]) as existe
                                     FROM [LYCEUM].[PrestacaoContas].[IMPORTACAOSEI]
                                     where [CENSO] = @CENSO and
		                                    [PERIODOREFERENCIAID] = @PERIODOREFERENCIAID ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@PERIODOREFERENCIAID", SqlDbType.Int, periodoreferencia);

            reader = ctx.GetDataReader(contextQuery);

            while (reader.Read())
            {
                if (Convert.ToInt32(reader["existe"]) != 0)
                    return true;
                else
                    return false;
            }

            return true;
        }

        public bool VerificaEnvioSEI(int periodoreferencia, string censo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;

            contextQuery.Command = @"SELECT count([IMPORTACAOSEIID]) as existe
                                     FROM [LYCEUM].[PrestacaoContas].[IMPORTACAOSEI]
                                     where [CENSO] = @CENSO and
		                                    [PERIODOREFERENCIAID] = @PERIODOREFERENCIAID ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@PERIODOREFERENCIAID", SqlDbType.Int, periodoreferencia);

            reader = ctx.GetDataReader(contextQuery);

            while (reader.Read())
            {
                if (Convert.ToInt32(reader["existe"]) != 0)
                    return true;
                else
                    return false;
            }

            return true;
        }
        public ValidacaoDados ValidaRemocao(int extratoBancarioId, int ano, int mes, string censo)
        {
            RN.PrestacaoContas.PeriodoReferencia rnPeriodoReferencia = new PeriodoReferencia();
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (extratoBancarioId <= 0)
            {
                mensagens.Add(" O campo obrigatório CODIGO não foi preenchido");
            }
            if (ano <= 0)
            {
                mensagens.Add(" O campo obrigatório ANO não foi preenchido");
            }
            if (mes <= 0)
            {
                mensagens.Add(" O campo obrigatório MÊS não foi preenchido");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    var periodoReferenciaId = rnPeriodoReferencia.ObtemPeriodoReferenciaPor(contexto, mes, ano);

                    if (VerificaEnvioSEI(periodoReferenciaId, censo))
                    {
                        mensagens.Add("Esse EXTRATO não pode ser excluído, pois já foi enviado ao SEI!");
                    }

                }
                catch
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw;
                }
                finally
                {
                    if (contexto != null)
                    {
                        contexto.Dispose();
                    }
                }
            }
            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;


        }

        public void Remove(int extratoBancarioId)
        {
            var rnExigenciaExtrato = new RN.PrestacaoContas.ExigenciaExtrato();

            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {

                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" 
                    insert into PrestacaoContas.EXTRATOBANCARIOARQUIVO_EXCLUIDO
					SELECT *,GETDATE() FROM  PrestacaoContas.EXTRATOBANCARIOARQUIVO
					where EXTRATOBANCARIOID = @EXTRATOBANCARIOID

				    delete from PrestacaoContas.EXTRATOBANCARIOARQUIVO
                    where EXTRATOBANCARIOID = @EXTRATOBANCARIOID

					INSERT INTO PrestacaoContas.EXIGENCIAEXTRATOARQUIVO_EXCLUIDO
					SELECT EEA.*,GETDATE() FROM PrestacaoContas.EXIGENCIAEXTRATOARQUIVO EEA
					INNER JOIN PrestacaoContas.EXIGENCIAEXTRATO EE ON EEA.EXIGENCIAEXTRATOID = EE.EXIGENCIAEXTRATOID
                    where EXTRATOBANCARIOID = @EXTRATOBANCARIOID

                    delete EEA from PrestacaoContas.EXIGENCIAEXTRATOARQUIVO EEA
					INNER JOIN PrestacaoContas.EXIGENCIAEXTRATO EE ON EEA.EXIGENCIAEXTRATOID = EE.EXIGENCIAEXTRATOID
                    where EXTRATOBANCARIOID = @EXTRATOBANCARIOID

					INSERT INTO  PrestacaoContas.EXIGENCIAEXTRATO_EXCLUIDO
					SELECT *,GETDATE() FROM PrestacaoContas.EXIGENCIAEXTRATO
                    where EXTRATOBANCARIOID = @EXTRATOBANCARIOID

                    delete from PrestacaoContas.EXIGENCIAEXTRATO
                    where EXTRATOBANCARIOID = @EXTRATOBANCARIOID


					INSERT INTO  PrestacaoContas.APLICACAOFINANCEIRACOMPROVANTEARQUIVO_EXCLUIDO
					SELECT AFA.*,GETDATE() FROM PrestacaoContas.APLICACAOFINANCEIRACOMPROVANTEARQUIVO AFA
                    INNER JOIN PrestacaoContas.APLICACAOFINANCEIRA AF ON AF.APLICACAOFINANCEIRAID = AFA.APLICACAOFINANCEIRAID
                    where EXTRATOBANCARIOID = @EXTRATOBANCARIOID

                    delete AFA from PrestacaoContas.APLICACAOFINANCEIRACOMPROVANTEARQUIVO AFA
                    INNER JOIN PrestacaoContas.APLICACAOFINANCEIRA AF ON AF.APLICACAOFINANCEIRAID = AFA.APLICACAOFINANCEIRAID
                    where EXTRATOBANCARIOID = @EXTRATOBANCARIOID

					INSERT INTO  PrestacaoContas.APLICACAOFINANCEIRA_EXCLUIDO
					SELECT *,GETDATE() FROM PrestacaoContas.APLICACAOFINANCEIRA
                    where EXTRATOBANCARIOID = @EXTRATOBANCARIOID

                    delete from PrestacaoContas.APLICACAOFINANCEIRA
                    where EXTRATOBANCARIOID = @EXTRATOBANCARIOID

					INSERT INTO  PrestacaoContas.EXTRATOBANCARIO_EXCLUIDO
					SELECT *,GETDATE() FROM PrestacaoContas.EXTRATOBANCARIO
                    WHERE  EXTRATOBANCARIOID = @EXTRATOBANCARIOID

                    DELETE from PrestacaoContas.EXTRATOBANCARIO
                    WHERE  EXTRATOBANCARIOID = @EXTRATOBANCARIOID
                ";

                contextQuery.Parameters.Add("@EXTRATOBANCARIOID", SqlDbType.Int, extratoBancarioId);

                ctx.ApplyModifications(contextQuery);
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
                ctx.Dispose();
            }
        }
    }
}
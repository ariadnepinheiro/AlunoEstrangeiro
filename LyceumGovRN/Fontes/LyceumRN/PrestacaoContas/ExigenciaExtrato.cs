using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public  class ExigenciaExtrato
    {
        RN.PrestacaoContas.TipoExigenciaExtrato rnTipoExigenciaExtrato = new RN.PrestacaoContas.TipoExigenciaExtrato();
        RN.PrestacaoContas.ExtratoBancario rnExtratoBancario = new RN.PrestacaoContas.ExtratoBancario();
        RN.Usuarios rnUsuarios = new RN.Usuarios();

        public bool PossuiTipoExigenciaExtratoPor(DataContext contexto, int tipoExigenciaExtratoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM PrestacaoContas.EXIGENCIAEXTRATO (NOLOCK)
                                    WHERE TIPOEXIGENCIAEXTRATOID = @TIPOEXIGENCIAEXTRATOID ";

            contextQuery.Parameters.Add("@TIPOEXIGENCIAEXTRATOID", SqlDbType.Int, tipoExigenciaExtratoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
              existe = true;
            }

            return existe;
        }

        public DataTable ListaPor(int extratoBancarioId)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                {
                    ContextQuery query = new ContextQuery();

                    query.Command = @"
                        select
                        ee.EXIGENCIAEXTRATOID
                        ,ee.TIPOEXIGENCIAEXTRATOID
                        ,tee.DESCRICAO as DESCRICAO_TIPOEXIGENCIAEXTRATO
                        ,tee.DATAINICIO as DATAINICIO_TIPOEXIGENCIAEXTRATO
                        ,tee.DATAFIM as DATAFIM_TIPOEXIGENCIAEXTRATO
                        ,ee.EXTRATOBANCARIOID
                        ,ee.NOTAEXPLICATIVA
                        ,ee.JUSTIFICATIVA
                        ,ee.APROVADO
                        ,ee.USUARIOID
                        ,ee.DATACADASTRO
                        ,ee.DATAALTERACAO
                        ,eea.EXIGENCIAEXTRATOARQUIVOID
                        ,eea.CHAVEARQUIVO
                        ,eea.ARQUIVO
                        ,eea.TIPOARQUIVO
                        ,eea.NOMEARQUIVO
                        ,eea.USUARIOID as ARQUIVO_USUARIOID
                        ,eea.DATACADASTRO as ARQUIVO_DATACADASTRO
                        ,eea.DATAALTERACAO as ARQUIVO_DATAALTERACAO

                        from
                        PrestacaoContas.EXIGENCIAEXTRATO ee
                        inner join PrestacaoContas.TIPOEXIGENCIAEXTRATO tee on tee.TIPOEXIGENCIAEXTRATOID = ee.TIPOEXIGENCIAEXTRATOID
                        left join PrestacaoContas.EXIGENCIAEXTRATOARQUIVO eea on eea.EXIGENCIAEXTRATOID = ee.EXIGENCIAEXTRATOID

                        where
                        ee.EXTRATOBANCARIOID = @EXTRATOBANCARIOID
                    ";

                    query.Parameters.Add("@EXTRATOBANCARIOID", SqlDbType.Int, extratoBancarioId);

                    return ctx.GetDataTable(query);
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

        public ValidacaoDados Valida(Entidades.ExigenciaExtrato ee)
        {
            var validacaoDados = new ValidacaoDados();
            var mensagens = new List<string>();
            var ehInsercao = ee.ExigenciaExtratoId == 0;

            if (ehInsercao && ee.ExtratoBancarioId <= 0)
                mensagens.Add("EXTRATO BANCÁRIO: Preenchimento obrigatório");

            if (ee.TipoExigenciaExtratoId <= 0)
                mensagens.Add("TIPO DE EXIGÊNCIA: Preenchimento obrigatório");

            if (ee.NotaExplicativa.IsNullOrEmptyOrWhiteSpace())
                mensagens.Add("NOTA EXPLICATIVA: Preenchimento obrigatório");

            if (ee.NotaExplicativa.Length > 1000)
                mensagens.Add("NOTA EXPLICATIVA: Não pode ter mais do que 1000 caracteres");

            if (ee.UsuarioId.IsNullOrEmptyOrWhiteSpace())
                mensagens.Add("USUÁRIO ID: Preenchimento obrigatório");

            if (ee.UsuarioId.Length > 15)
                mensagens.Add("USUÁRIO ID: Não pode ter mais do que 15 caracteres");

            if (mensagens.Count == 0)
            {
                DataContext contexto = null;

                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (!ehInsercao && !Existe(contexto, ee.ExigenciaExtratoId))
                        mensagens.Add("EXIGÊNCIA DE EXTRATO: Não existe cadastrado no banco de dados");

                    if (ehInsercao && !rnExtratoBancario.Existe(contexto, ee.ExtratoBancarioId))
                        mensagens.Add("EXTRATO BANCÁRIO: Não existe cadastrado no banco de dados");

                    if (!rnTipoExigenciaExtrato.Existe(contexto, ee.TipoExigenciaExtratoId))
                        mensagens.Add("TIPO DE EXIGÊNCIA: Não existe cadastrado no banco de dados");

                    if (!RN.Usuarios.VerificaHabilitado(ee.UsuarioId))
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
        public DataTable ObtemExtratoBancario(int extratoBancarioId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
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
            catch (Exception ex)
            {
                throw ex;
            }
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

        public ValidacaoDados ValidaJustificativa(Entidades.ExigenciaExtrato ee)
        {
            var validacaoDados = new ValidacaoDados();
            var mensagens = new List<string>();

            if (ee.Justificativa.IsNullOrEmptyOrWhiteSpace())
                mensagens.Add("JUSTIFICATIVA: Preenchimento obrigatório");

            if (ee.Justificativa.Length > 500)
                mensagens.Add("JUSTIFICATIVA: Não pode ter mais do que 500 caracteres");

            if (ee.UsuarioId.IsNullOrEmptyOrWhiteSpace())
                mensagens.Add("USUÁRIO ID: Preenchimento obrigatório");

            if (ee.UsuarioId.Length > 15)
                mensagens.Add("USUÁRIO ID: Não pode ter mais do que 15 caracteres");

            if (mensagens.Count == 0)
            {
                DataContext contexto = null;

                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (rnExtratoBancario.ObtemStatus(ee.ExtratoBancarioId) == 2 && ee.Justificativa.IsNullOrEmptyOrWhiteSpace())
                        mensagens.Add("JUSTIFICATIVA: Preenchimento obrigatório");

                    if (!Existe(contexto, ee.ExigenciaExtratoId))
                        mensagens.Add("EXIGÊNCIA DE EXTRATO: Não existe cadastrado no banco de dados");

                    if (!RN.Usuarios.VerificaHabilitado(ee.UsuarioId))
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

        public ValidacaoDados ValidaRemocao(int exigenciaExtratoId)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados();

            if (exigenciaExtratoId <= 0)
                mensagens.Add("EXIGÊNCIA DE EXTRATO: Preenchimento obrigatório");

            if (mensagens.Count == 0)
            {
                DataContext contexto = null;

                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (!Existe(contexto, exigenciaExtratoId))
                        mensagens.Add("EXIGÊNCIA DE EXTRATO: Não existe cadastrado no banco de dados");
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
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public void Insere(Entidades.ExigenciaExtrato ee)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    ContextQuery contextQuery = new ContextQuery();

                    contextQuery.Command = @"
                        insert into PrestacaoContas.EXIGENCIAEXTRATO (
                            TIPOEXIGENCIAEXTRATOID
                            ,EXTRATOBANCARIOID
                            ,NOTAEXPLICATIVA
                            ,JUSTIFICATIVA
                            --,APROVADO
                            ,USUARIOID
                            ,DATACADASTRO
                            ,DATAALTERACAO
                        ) values (
                            @TIPOEXIGENCIAEXTRATOID
                            ,@EXTRATOBANCARIOID
                            ,@NOTAEXPLICATIVA
                            ,''
                            --,0
                            ,@USUARIOID
                            ,@DATACADASTRO
                            ,@DATACADASTRO
                        )
                    ";

                    contextQuery.Parameters.Add("@TIPOEXIGENCIAEXTRATOID", SqlDbType.Int, ee.TipoExigenciaExtratoId);
                    contextQuery.Parameters.Add("@EXTRATOBANCARIOID", SqlDbType.Int, ee.ExtratoBancarioId);
                    contextQuery.Parameters.Add("@NOTAEXPLICATIVA", SqlDbType.VarChar, ee.NotaExplicativa);
                    contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, ee.UsuarioId);
                    contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);

                    ctx.ApplyModifications(contextQuery);
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

        public void AtualizaJustificativa(Entidades.ExigenciaExtrato ee)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    ContextQuery contextQuery = new ContextQuery();

                    contextQuery.Command = @"
                        update PrestacaoContas.EXIGENCIAEXTRATO set
                            JUSTIFICATIVA = @JUSTIFICATIVA
                            ,USUARIOID = @USUARIOID
                            ,DATAALTERACAO = @DATAALTERACAO
                        where EXIGENCIAEXTRATOID = @EXIGENCIAEXTRATOID
                    ";

                    contextQuery.Parameters.Add("@EXIGENCIAEXTRATOID", SqlDbType.Int, ee.ExigenciaExtratoId);
                    contextQuery.Parameters.Add("@JUSTIFICATIVA", SqlDbType.VarChar, ee.Justificativa);
                    contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, ee.UsuarioId);
                    contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                    ctx.ApplyModifications(contextQuery);
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

        public void Atualiza(Entidades.ExigenciaExtrato ee)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    ContextQuery contextQuery = new ContextQuery();

                    contextQuery.Command = @"
                        update PrestacaoContas.EXIGENCIAEXTRATO set
                            TIPOEXIGENCIAEXTRATOID = @TIPOEXIGENCIAEXTRATOID
                            ,NOTAEXPLICATIVA = @NOTAEXPLICATIVA
                            --,APROVADO = @APROVADO
                            ,USUARIOID = @USUARIOID
                            ,DATAALTERACAO = @DATAALTERACAO
                        where EXIGENCIAEXTRATOID = @EXIGENCIAEXTRATOID
                    ";

                    contextQuery.Parameters.Add("@EXIGENCIAEXTRATOID", SqlDbType.Int, ee.ExigenciaExtratoId);
                    contextQuery.Parameters.Add("@TIPOEXIGENCIAEXTRATOID", SqlDbType.Int, ee.TipoExigenciaExtratoId);
                    contextQuery.Parameters.Add("@NOTAEXPLICATIVA", SqlDbType.VarChar, ee.NotaExplicativa);
                    //contextQuery.Parameters.Add("@APROVADO", SqlDbType.Bit, ee.Aprovado);
                    contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, ee.UsuarioId);
                    contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                    ctx.ApplyModifications(contextQuery);
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

        public void AtualizaAprovacao(int exigenciaExtratoId, bool aprovado)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    ContextQuery contextQuery = new ContextQuery();

                    contextQuery.Command = @"
                        update PrestacaoContas.EXIGENCIAEXTRATO set
                            APROVADO = @APROVADO
                            ,USUARIOID = @USUARIOID
                            ,DATAALTERACAO = @DATAALTERACAO
                        where EXIGENCIAEXTRATOID = @EXIGENCIAEXTRATOID
                    ";

                    contextQuery.Parameters.Add("@EXIGENCIAEXTRATOID", SqlDbType.Int, exigenciaExtratoId);
                    contextQuery.Parameters.Add("@APROVADO", SqlDbType.Bit, aprovado);
                    contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, System.Web.HttpContext.Current.User.Identity.Name);
                    contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                    ctx.ApplyModifications(contextQuery);
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

        public bool? EstaAprovado(int exigenciaExtratoId)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                    return EstaAprovado(ctx, exigenciaExtratoId);
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

        public bool? EstaAprovado(DataContext ctx, int exigenciaExtratoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT APROVADO 
                                      FROM   PrestacaoContas.EXIGENCIAEXTRATO (NOLOCK) 
                                      WHERE  EXIGENCIAEXTRATOID = @EXIGENCIAEXTRATOID ";

            contextQuery.Parameters.Add("@EXIGENCIAEXTRATOID", SqlDbType.Int, exigenciaExtratoId);

            return ctx.GetReturnValue<bool?>(contextQuery);
        }

        public void Remove(int exigenciaExtratoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" 
                        DELETE PrestacaoContas.EXIGENCIAEXTRATOARQUIVO
                        WHERE  EXIGENCIAEXTRATOID = @EXIGENCIAEXTRATOID
 
                        DELETE PRESTACAOCONTAS.EXIGENCIAEXTRATO 
                        WHERE  EXIGENCIAEXTRATOID = @EXIGENCIAEXTRATOID
                    ";

                contextQuery.Parameters.Add("@EXIGENCIAEXTRATOID", SqlDbType.Int, exigenciaExtratoId);

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
                if (ctx != null)
                    ctx.Dispose();
            }
        }

        public bool Existe(DataContext ctx, int exigenciaExtratoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT COUNT(*) 
                                      FROM   PrestacaoContas.EXIGENCIAEXTRATO (NOLOCK) 
                                      WHERE  EXIGENCIAEXTRATOID = @EXIGENCIAEXTRATOID ";

            contextQuery.Parameters.Add("@EXIGENCIAEXTRATOID", SqlDbType.Int, exigenciaExtratoId);

            return ctx.GetReturnValue<int>(contextQuery) > 0;
        }
    }
}
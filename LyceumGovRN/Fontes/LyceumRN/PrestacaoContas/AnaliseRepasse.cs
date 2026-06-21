using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class AnaliseRepasse
    {
        public bool PossuiMotivoReprovacaoLancamentoRepassePor(DataContext contexto, int motivoReprovacaoLancamentoRepasseId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM PrestacaoContas.ANALISEREPASSE (NOLOCK)
                                    WHERE MOTIVOREPROVACAOLANCAMENTOREPASSEID = @MOTIVOREPROVACAOLANCAMENTOREPASSEID ";

            contextQuery.Parameters.Add("@MOTIVOREPROVACAOLANCAMENTOREPASSEID", SqlDbType.Int, motivoReprovacaoLancamentoRepasseId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool EhAprovadoPor(DataContext ctx, int lancamentoRepasseId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @"SELECT COUNT(*) 
                                     FROM prestacaocontas.ANALISEREPASSE 
                                     WHERE LANCAMENTOREPASSEID = @LANCAMENTOREPASSEID
	                                    AND APROVADO = 1 ";

            contextQuery.Parameters.Add("@LANCAMENTOREPASSEID", SqlDbType.Int, lancamentoRepasseId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool EhRepasseIntegradoComSEFAZ(DataContext ctx, int lancamentoRepasseId)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" 
                                    select COUNT(0)
                                    from PrestacaoContas.LANCAMENTOREPASSE lr (nolock)
                                    where
                                    lr.LANCAMENTOREPASSEID = @LANCAMENTOREPASSEID
                                    and WSREPASSESEFAZID is not null
                                    ";

                contextQuery.Parameters.Add("@LANCAMENTOREPASSEID", SqlDbType.Int, lancamentoRepasseId);

                return ctx.GetReturnValue<int>(contextQuery) > 0;
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

        public ValidacaoDados Valida(Entidades.AnaliseRepasse ar)
        {
            return Valida(null, ar);
        }

        public ValidacaoDados Valida(DataContext ctx, Entidades.AnaliseRepasse ar)
        {
            var validacaoDados = new ValidacaoDados();
            var mensagens = new List<string>();
            var ehInsercao = ar.AnaliseRepasseId == 0;
            var ctxIsNull = ctx == null;

            if (!ar.Aprovado && (!ar.MotivoReprovacaoLancamentoRepasseId.HasValue || ar.MotivoReprovacaoLancamentoRepasseId.Value == 0))
                mensagens.Add("MOTIVO DA REPROVAÇÃO: Preenchimento obrigatório");

            if (ar.UsuarioId.IsNullOrEmptyOrWhiteSpace())
                mensagens.Add("USUÁRIO ID: Preenchimento obrigatório");

            if (ar.UsuarioId.Length > 15)
                mensagens.Add("USUÁRIO ID: Não pode ter mais do que 15 caracteres");

            /*
            Alterado por Felipe R. Gomes em 03/08/2023
            
            Conforme orientação do Rodrigo via Skype nesse mesmo dia (msg particular),
            desabilitar essa regra específica para repasses importados do SEFAZ, pois
            no integrador os repasses já deveriam vir com APROVADO = 1, e não vieram.
            */
            //if (!mensagens.Any())
            //{
            //    try
            //    {
            //        if (ctxIsNull)
            //            ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            //        if (EhRepasseIntegradoComSEFAZ(ctx, ar.LancamentoRepasseId))
            //            mensagens.Add("LANÇAMENTO DE REPASSE: Não pode ser alterado porque está integrado com o Web Service do SEFAZ");
            //    }
            //    catch (Exception ex)
            //    {
            //        mensagens.Add(ex.Message);
            //    }
            //    finally
            //    {
            //        if (ctxIsNull)
            //            ctx.Dispose();
            //    }
            //}

            if (mensagens.Any())
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            else
                validacaoDados.Valido = true;

            return validacaoDados;
        }

        public void Salva(Entidades.AnaliseRepasse ar)
        {
            try 
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                    Salva(ctx, ar);
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

        public void Salva(DataContext ctx, Entidades.AnaliseRepasse ar)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @"
                    if (@ANALISEREPASSEID = 0)
                    begin
                        insert into PrestacaoContas.ANALISEREPASSE (
                            LANCAMENTOREPASSEID
                            ,MOTIVOREPROVACAOLANCAMENTOREPASSEID
                            ,APROVADO
                            ,USUARIOID
                            ,DATAAPROVACAO
                            ,DATAALTERACAO
                        )
                        values (
                            @LANCAMENTOREPASSEID
                            ,@MOTIVOREPROVACAOLANCAMENTOREPASSEID
                            ,@APROVADO
                            ,@USUARIOID
                            ,getdate()
                            ,getdate()
                        )
                    end
                    else
                    begin
                        update PrestacaoContas.ANALISEREPASSE set
                            MOTIVOREPROVACAOLANCAMENTOREPASSEID = @MOTIVOREPROVACAOLANCAMENTOREPASSEID
                            ,APROVADO = @APROVADO
                            ,USUARIOID = @USUARIOID
                            ,DATAAPROVACAO = case when @APROVADO <> APROVADO then getdate() else DATAAPROVACAO end
                            ,DATAALTERACAO = getdate()
                        where ANALISEREPASSEID = @ANALISEREPASSEID
                    end
                ";

                contextQuery.Parameters.Add("@ANALISEREPASSEID", SqlDbType.Int, ar.AnaliseRepasseId);
                contextQuery.Parameters.Add("@LANCAMENTOREPASSEID", SqlDbType.Int, ar.LancamentoRepasseId);
                contextQuery.Parameters.Add("@APROVADO", SqlDbType.Bit, ar.Aprovado);
                contextQuery.Parameters.Add("@MOTIVOREPROVACAOLANCAMENTOREPASSEID", SqlDbType.Int, ar.MotivoReprovacaoLancamentoRepasseId);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, ar.UsuarioId);

                ctx.ApplyModifications(contextQuery);
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

        public void SalvaLista(List<Entidades.AnaliseRepasse> ars)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                    foreach (var ar in ars)
                        Salva(ctx, ar);
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
    }
}

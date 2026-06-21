using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;


namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class OperacaoExigencia
    {
        public DataTable ListaAtivo()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"  SELECT OPERACAOEXIGENCIAID
                                                  ,OPERACAOID
                                                  ,TIPOEXIGENCIAOPERACAOID
                                                  ,NOTAEXPLICATIVA
                                                  ,JUSTIFICATIVA
                                                  ,APROVADO
                                                  ,USUARIOID
                                                  ,DATACADASTRO
                                                  ,DATAALTERACAO
                                              FROM PrestacaoContas.OPERACAOEXIGENCIA                                 
                                            ORDER BY TIPOEXIGENCIAOPERACAOID ";

                dt = contexto.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                contexto.Abandon();
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
                contexto.Dispose();
            }

            return dt;
        }

        public interface IEOperacaoArquivo
        {
            byte[] ObtemArquivoPor(int id);
        }
        public void AtualizaAprovacao(int exigenciaExtratoId, bool aprovado)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    ContextQuery contextQuery = new ContextQuery();

                    contextQuery.Command = @"
                        update PrestacaoContas.OPERACAOEXIGENCIA set
                            APROVADO = @APROVADO
                            ,USUARIOID = @USUARIOID
                            ,DATAALTERACAO = @DATAALTERACAO
                        where OPERACAOEXIGENCIAID = @EXIGENCIAEXTRATOID
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
                                      FROM   PrestacaoContas.OPERACAOEXIGENCIA (NOLOCK) 
                                      WHERE  OPERACAOEXIGENCIAID = @EXIGENCIAEXTRATOID ";

            contextQuery.Parameters.Add("@EXIGENCIAEXTRATOID", SqlDbType.Int, exigenciaExtratoId);

            return ctx.GetReturnValue<bool?>(contextQuery);
        }
        public ValidacaoDados Valida(Entidades.OperacaoExigencia ee, int tipo)
        {
            var validacaoDados = new ValidacaoDados();
            var mensagens = new List<string>();
            var ehInsercao = ee.OperacaoExigenciaId == 0;

            //     if (ehInsercao && ee.OperacaoExigenciaId <= 0)
            //        mensagens.Add("Operacao: Preenchimento obrigatório");

            if (ee.TipoExigenciaOperacaoId <= 0)
                mensagens.Add("TIPO DE EXIGÊNCIA: Preenchimento obrigatório");



            if (tipo != 1)
            {
                if (ee.Justificativa.IsNullOrEmptyOrWhiteSpace())
                    mensagens.Add("JUSTIFICATIVA: Preenchimento obrigatório");
                else
                    if (ee.Justificativa.Length > 501)
                        mensagens.Add("JUSTIFICATIVA: Não pode ter mais do que 500 caracteres");
            }
            else
            {
                if (ee.NotaExplicativa.IsNullOrEmptyOrWhiteSpace())
                    mensagens.Add("NOTA EXPLICATIVA: Preenchimento obrigatório");
                else
                    if (ee.NotaExplicativa.Length > 501)
                        mensagens.Add("NOTA EXPLICATIVA: Não pode ter mais do que 500 caracteres");
            }
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

                    //if (!ehInsercao && !Existe(contexto, ee.OperacaoExigenciaId))
                    //    mensagens.Add("EXIGÊNCIA DE EXTRATO: Não existe cadastrado no banco de dados");

                    //if (ehInsercao && !rnExtratoBancario.Existe(contexto, ee.OperacaoExigenciaId))
                    //    mensagens.Add("EXTRATO BANCÁRIO: Não existe cadastrado no banco de dados");

                    // if (!rnTipoExigenciaExtrato.Existe(contexto, ee.TipoExigenciaExtratoId))
                    //     mensagens.Add("TIPO DE EXIGÊNCIA: Não existe cadastrado no banco de dados");

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
        public DataTable Lista()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT OPERACAOEXIGENCIAID
                                                  ,OPERACAOID
                                                  ,TIPOEXIGENCIAOPERACAOID
                                                  ,NOTAEXPLICATIVA
                                                  ,JUSTIFICATIVA
                                                  ,APROVADO
                                                  ,USUARIOID
                                                  ,DATACADASTRO
                                                  ,DATAALTERACAO
                                              FROM PrestacaoContas.OPERACAOEXIGENCIA(NOLOCK)
                                       ";

                dt = contexto.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                contexto.Abandon();
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
                contexto.Dispose();
            }

            return dt;
        }

        /*   public ValidacaoDados Valida(Entidades.OperacaoExigencia DocumentoNecCredDeb, bool cadastro)
           {
               List<string> mensagens = new List<string>();
               DataContext contexto = null;
               ValidacaoDados validacaoDados = new ValidacaoDados
               {
                   Valido = false
               };

               if (DocumentoNecCredDeb == null)
               {
                   return validacaoDados;
               }

               //Verifica se é alteração
               if (!cadastro)
               {
                   if (DocumentoNecCredDeb.OperacaoId <= 0)
                   {
                       mensagens.Add("Campo CÓDIGO é obrigatório.");
                   }
               }

               if (DocumentoNecCredDeb.Justificativa.IsNullOrEmptyOrWhiteSpace())
               {
                   mensagens.Add("Campo JUSTIFICATIVA é obrigatório.");
               }

               if (DocumentoNecCredDeb.NotaExplicativa.IsNullOrEmptyOrWhiteSpace())
               {
                   mensagens.Add("Campo nota explicativa é obrigatório.");
               }

               if (mensagens.Count == 0)
               {
                   try
                   {
                       contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                       //Verifica se já existe a descrição cadastrada
                       //if (this.PossuiOutraDescricaoCadastradaPor(contexto, DocumentoNecCredDeb.Descricao, DocumentoNecCredDeb.TipoExigenciaOperacaoId))
                      // {
                      //     mensagens.Add("Esta DESCRIÇÃO já foi utilizada.");
                      // }
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
           */
        private bool PossuiOutraDescricaoCadastradaPor(DataContext ctx, string descricao, int TipoTransporteId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM PrestacaoContas.TIPOEXIGENCIAOPERACAO (NOLOCK)
                                WHERE DESCRICAO = @DESCRICAO
	                                AND TIPOEXIGENCIAOPERACAOID <> @TIPOTRANSPORTEID ";

            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, descricao);
            contextQuery.Parameters.Add("@TIPOTRANSPORTEID", SqlDbType.Int, TipoTransporteId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.OperacaoExigencia TipoTransporte)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO PrestacaoContas.OPERACAOEXIGENCIA
                                                        (OPERACAOID, 
                                                         TIPOEXIGENCIAOPERACAOID, 
                                                         NOTAEXPLICATIVA, 
                                                         JUSTIFICATIVA,
                                                        
                                                         USUARIOID, 
                                                         DATACADASTRO, 
                                                         DATAALTERACAO) 
                                            VALUES      (@OPERACAOID, 
                                                         @TIPOEXIGENCIAOPERACAOID, 
                                                         @NOTAEXPLICATIVA, 
                                                         @JUSTIFICATIVA,
                                           
                                                         @USUARIOID, 
                                                         @DATACADASTRO, 
                                                         @DATAALTERACAO) ";

                contextQuery.Parameters.Add("@OPERACAOID", SqlDbType.Int, TipoTransporte.OperacaoId);
                contextQuery.Parameters.Add("@TIPOEXIGENCIAOPERACAOID", SqlDbType.Int, TipoTransporte.TipoExigenciaOperacaoId);
                contextQuery.Parameters.Add("@NOTAEXPLICATIVA", SqlDbType.VarChar, TipoTransporte.NotaExplicativa);
                contextQuery.Parameters.Add("@JUSTIFICATIVA", SqlDbType.VarChar, TipoTransporte.Justificativa);
                // contextQuery.Parameters.Add("@APROVADO", SqlDbType.Bit, TipoTransporte.Aprovado);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, TipoTransporte.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

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

        public void AtualizaArquivoExigencia(Entidades.OperacaoExigenciaArquivo exigenciaExtratoArquivo)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    ContextQuery contextQuery = new ContextQuery();

                    contextQuery.Command = @"
                    if not exists (select top 1 1 from PrestacaoContas.OPERACAOEXIGENCIAARQUIVO where OPERACAOEXIGENCIAARQUIVOID = @EXIGENCIAEXTRATOID)
                    begin
	                    insert into PrestacaoContas.OPERACAOEXIGENCIAARQUIVO (
		                    OPERACAOEXIGENCIAID
		                    ,CHAVEARQUIVO
		                    ,ARQUIVO
		                    ,TIPOARQUIVO
		                    ,NOMEARQUIVO
		                    ,USUARIOID
		                    ,DATACADASTRO
		                    ,DATAALTERACAO
	                    ) values (
		                    @EXIGENCIAEXTRATOID
		                    ,@CHAVEARQUIVO
		                    ,@ARQUIVO
		                    ,@TIPOARQUIVO
		                    ,@NOMEARQUIVO
		                    ,@USUARIOID
		                    ,@DATACADASTRO
		                    ,@DATAALTERACAO
	                    )
                    end
                    else
                    begin
	                    update PrestacaoContas.OPERACAOEXIGENCIAARQUIVO set
	                     ARQUIVO = @ARQUIVO
	                    ,TIPOARQUIVO = @TIPOARQUIVO
	                    ,NOMEARQUIVO = @NOMEARQUIVO
	                    ,USUARIOID = @USUARIOID
	                    ,DATAALTERACAO = @DATAALTERACAO
	                    where OPERACAOEXIGENCIAARQUIVOID = @EXIGENCIAEXTRATOID
                    end

                    select OperacaoExigenciaArquivoId from PrestacaoContas.OPERACAOEXIGENCIAARQUIVO where OPERACAOEXIGENCIAARQUIVOID = @EXIGENCIAEXTRATOID
                    ";

                    contextQuery.Parameters.Add("@EXIGENCIAEXTRATOID", SqlDbType.Int, exigenciaExtratoArquivo.OperacaoExigenciaId);
                    contextQuery.Parameters.Add("@CHAVEARQUIVO", SqlDbType.UniqueIdentifier, new Guid(exigenciaExtratoArquivo.ChaveArquivo));
                    contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.Binary, exigenciaExtratoArquivo.Arquivo);
                    contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, exigenciaExtratoArquivo.TipoArquivo);
                    contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, exigenciaExtratoArquivo.NomeArquivo);
                    contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, exigenciaExtratoArquivo.UsuarioId);
                    contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, exigenciaExtratoArquivo.DataCadastro);
                    contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, exigenciaExtratoArquivo.DataAlteracao);

                    exigenciaExtratoArquivo.OperacaoExigenciaArquivoId = Convert.ToInt32(ctx.GetReturnValue(contextQuery));

                    //InsereAuditoria(ctx, exigenciaExtratoArquivo, "ATUALIZA", System.Web.HttpContext.Current.Request.UserHostAddress);
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

        public void AtualizaJustificativa(Entidades.OperacaoExigencia ee)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    ContextQuery contextQuery = new ContextQuery();

                    contextQuery.Command = @"
                        update PrestacaoContas.OPERACAOEXIGENCIA set
                            JUSTIFICATIVA = @JUSTIFICATIVA
                            ,USUARIOID = @USUARIOID
                            ,DATAALTERACAO = @DATAALTERACAO
                        where OPERACAOEXIGENCIAID = @EXIGENCIAEXTRATOID
                    ";

                    contextQuery.Parameters.Add("@EXIGENCIAEXTRATOID", SqlDbType.Int, ee.OperacaoExigenciaId);
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
        public ValidacaoDados ValidaArquvoExigencia(Entidades.OperacaoExigenciaArquivo exigenciaExtratoArquivo)
        {
            var validacaoDados = new ValidacaoDados();
            var mensagens = new List<string>();

            if (exigenciaExtratoArquivo.Arquivo.Length <= 1)
                mensagens.Add("ARQUIVO: preenchimento obrigatório");

            if (!new string[] { "image/jpeg", "application/pdf" }.Contains(exigenciaExtratoArquivo.TipoArquivo))
                mensagens.Add("ARQUIVO: suporta somente arquivos JPG e PDF");

            if (exigenciaExtratoArquivo.Arquivo.Length > 15728640)
                mensagens.Add("ARQUIVO: suporta somente arquivos de 15 MB");

            if (exigenciaExtratoArquivo.UsuarioId.IsNullOrEmptyOrWhiteSpace())
                mensagens.Add("USUÁRIO ID: Preenchimento obrigatório");

            if (exigenciaExtratoArquivo.UsuarioId.Length > 15)
                mensagens.Add("USUÁRIO ID: Não pode ter mais do que 15 caracteres");

            //if (mensagens.Count == 0)
            //{
            //    DataContext contexto = null;

            //    try
            //    {
            //        contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            //        //escreva a sua validação aqui!
            //    }
            //    catch (Exception ex)
            //    {
            //        if (contexto != null)
            //        {
            //            contexto.Abandon();
            //        }
            //        throw new Exception(ex.Message);
            //    }
            //    finally
            //    {
            //        if (contexto != null)
            //        {
            //            contexto.Dispose();
            //        }
            //    }
            //}

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

        public ValidacaoDados ValidaAprovacao(int exigenciaEventoId, string usuarioId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            Evento rnEvento = new Evento();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (exigenciaEventoId <= 0)
            {
                mensagens.Add("Campo CÓDIGO DA EXIGÊNCIA é obrigatório.");
            }

            if (usuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se a exigencia foi aprovada
                    if (EhExigenciaAprovadoPor(contexto, exigenciaEventoId))
                    {
                        mensagens.Add("Esta exigência já foi aprovada.");
                    }

                    //Verifica se o evento foi corrigido
                    // if (!EhExigenciaCorrigidaPor(contexto, exigenciaEventoId))
                    // {
                    //     mensagens.Add("Esta exigência não pode ser aprovada pois ainda não respondida / corrigida.");
                    // }
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
        public void Aprova(int exigenciaEventoId, string usuarioId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE PrestacaoContas.OPERACAOEXIGENCIA
                                           SET APROVADO = @APROVADO, 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO
                                         WHERE OPERACAOEXIGENCIAID = @EXIGENCIAEVENTOID ";

                contextQuery.Parameters.Add("@EXIGENCIAEVENTOID", SqlDbType.Int, exigenciaEventoId);
                contextQuery.Parameters.Add("@APROVADO", SqlDbType.Bit, true);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

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
        public void Rejeita(int exigenciaEventoId, string usuarioId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE PrestacaoContas.OPERACAOEXIGENCIA
                                           SET APROVADO = @APROVADO, 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO
                                         WHERE OPERACAOEXIGENCIAID = @EXIGENCIAEVENTOID ";

                contextQuery.Parameters.Add("@EXIGENCIAEVENTOID", SqlDbType.Int, exigenciaEventoId);
                contextQuery.Parameters.Add("@APROVADO", SqlDbType.Bit, false);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

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
        public ValidacaoDados ValidaRejeicao(int exigenciaEventoId, string usuarioId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            Evento rnEvento = new Evento();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (exigenciaEventoId <= 0)
            {
                mensagens.Add("Campo CÓDIGO DA EXIGÊNCIA é obrigatório.");
            }

            if (usuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //       //Verifica se a exigencia foi corrigido
                    //       if (!EhExigenciaCorrigidaPor(contexto, exigenciaEventoId))
                    //       {
                    //           mensagens.Add("Esta exigência não pode ser aprovada pois ainda não respondida / corrigida.");
                    //       }

                    //Verifica se a exigencia foi aprovada
                    if (EhExigenciaAprovadoPor(contexto, exigenciaEventoId))
                    {
                        mensagens.Add("Esta exigência não pode ser pois rejeitada pois já foi aprovada.");
                    }
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
        private bool EhExigenciaAprovadoPor(DataContext contexto, int exigenciaEventoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                    FROM PrestacaoContas.OPERACAOEXIGENCIA (NOLOCK)
                                    WHERE OPERACAOEXIGENCIAID = @EXIGENCIAEVENTOID
                                          AND APROVADO = 1 ";

            contextQuery.Parameters.Add("@EXIGENCIAEVENTOID", SqlDbType.Int, exigenciaEventoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }
        public ValidacaoDados ValidaJustificativa(Entidades.OperacaoExigencia ee)
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

                    //if (rnExtratoBancario.ObtemStatus(ee.ExtratoBancarioId) == 2 && ee.Justificativa.IsNullOrEmptyOrWhiteSpace())
                    //mensagens.Add("JUSTIFICATIVA: Preenchimento obrigatório");

                    //if (!Existe(contexto, ee.ExigenciaExtratoId))
                    //mensagens.Add("EXIGÊNCIA DE EXTRATO: Não existe cadastrado no banco de dados");

                    //if (!RN.Usuarios.VerificaHabilitado(ee.UsuarioId))
                    //mensagens.Add("USUÁRIO ID: Não está habilitado no sistema");
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
        public void Atualiza(Entidades.OperacaoExigencia TipoTransporte)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE PrestacaoContas.OPERACAOEXIGENCIA
                                        SET    TIPOEXIGENCIAOPERACAOID = @TIPOEXIGENCIAOPERACAOID, 
                                               NOTAEXPLICATIVA = @NOTAEXPLICATIVA, 
                                               JUSTIFICATIVA = @JUSTIFICATIVA, 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  OPERACAOEXIGENCIAID = @OPERACAOEXIGENCIAID ";

                contextQuery.Parameters.Add("@TIPOEXIGENCIAOPERACAOID", SqlDbType.VarChar, TipoTransporte.TipoExigenciaOperacaoId);
                contextQuery.Parameters.Add("@NOTAEXPLICATIVA", SqlDbType.VarChar, TipoTransporte.NotaExplicativa);
                contextQuery.Parameters.Add("@JUSTIFICATIVA", SqlDbType.VarChar, TipoTransporte.Justificativa);
                //contextQuery.Parameters.Add("@APROVADO", SqlDbType.Bit, TipoTransporte.Aprovado);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, TipoTransporte.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@TIPOTRANSPORTEID", SqlDbType.Int, TipoTransporte.TipoExigenciaOperacaoId);
                contextQuery.Parameters.Add("@OPERACAOEXIGENCIAID", SqlDbType.Int, TipoTransporte.OperacaoExigenciaId);


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
        public void AtualizaExigencia(Entidades.OperacaoExigencia TipoTransporte)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE PrestacaoContas.OPERACAOEXIGENCIA
                                        SET    TIPOEXIGENCIAOPERACAOID = @TIPOEXIGENCIAOPERACAOID, 
                                               NOTAEXPLICATIVA = @NOTAEXPLICATIVA, 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  OPERACAOEXIGENCIAID = @OPERACAOEXIGENCIAID ";

                contextQuery.Parameters.Add("@TIPOEXIGENCIAOPERACAOID", SqlDbType.VarChar, TipoTransporte.TipoExigenciaOperacaoId);
                contextQuery.Parameters.Add("@NOTAEXPLICATIVA", SqlDbType.VarChar, TipoTransporte.NotaExplicativa);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, TipoTransporte.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@TIPOTRANSPORTEID", SqlDbType.Int, TipoTransporte.TipoExigenciaOperacaoId);
                contextQuery.Parameters.Add("@OPERACAOEXIGENCIAID", SqlDbType.Int, TipoTransporte.OperacaoExigenciaId);


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
        public ValidacaoDados ValidaRemocao(int TipoTransporteId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            PequenaDespesa rnPequenaDespesa = new PequenaDespesa();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (TipoTransporteId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se motivo ja foi utilizado
                    if (rnPequenaDespesa.PossuiTipoTransportePor(contexto, TipoTransporteId))
                    {
                        mensagens.Add("Este motivo não pode ser excluído, pois já foi utilizado por uma despesa.");
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

        public void Remove(int TipoTransporteId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE PrestacaoContas.OPERACAOEXIGENCIA
                            WHERE  OPERACAOEXIGENCIAID = @TIPOTRANSPORTEID  ";

                contextQuery.Parameters.Add("@TIPOTRANSPORTEID", SqlDbType.Int, TipoTransporteId);

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

        public void RemoveExigencia(DataContext contexto, int operacaoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  delete [LYCEUM].[PrestacaoContas].[OPERACAOEXIGENCIA]
                          where OPERACAOID = @OPERACAOID ";

            contextQuery.Parameters.Add("@OPERACAOID", SqlDbType.Int, operacaoId);

            contexto.ApplyModifications(contextQuery);
        }
    }
}
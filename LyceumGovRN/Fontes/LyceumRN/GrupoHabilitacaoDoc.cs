using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Entidades;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Data;
using System.Web;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN
{
    public class GrupoHabilitacaoDoc
    {
        public bool PodeAgrupamentoPor(DataContext contexto, decimal numFunc)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM   LY_GRUPO_HABILITACAO_DOC (NOLOCK)
                                        WHERE  NUM_FUNC = @NUM_FUNC 
                                               AND AGRUPAMENTO_INGRESSO = 'S' 
                                               AND PROVISORIO = 'N'  ";

            contextQuery.Parameters.Add("@NUM_FUNC", TechneDbType.T_NUMFUNC, numFunc);

            if (contexto.GetReturnValue<int>(contextQuery) == 0)
            {
                retorno = true;
            }

            return retorno;
        }

        public bool ExisteAgrupamentoDocentePor(decimal numFunc, string agrupamento, string provisorio)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM LY_GRUPO_HABILITACAO_DOC (NOLOCK)
                                        WHERE NUM_FUNC = @NUM_FUNC 
	                                        AND AGRUPAMENTO = @AGRUPAMENTO 
	                                        AND PROVISORIO = @PROVISORIO ";

                contextQuery.Parameters.Add("@NUM_FUNC", TechneDbType.T_NUMFUNC, numFunc);
                contextQuery.Parameters.Add("@AGRUPAMENTO", TechneDbType.T_ALFAMEDIUM, agrupamento);
                contextQuery.Parameters.Add("@PROVISORIO", TechneDbType.T_SIMNAO, provisorio);

                if (contexto.GetReturnValue<int>(contextQuery) > 0)
                {
                    retorno = true;
                }

                return retorno;
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
                contexto.Dispose();
            }
        }

        public static int IncluirGrupoDeHabilitacaoDocentePorSolicitacao(TConnectionWritable connection, IEnumerable<int> idSolicitacaoHabilitacaoDocentes)
        {
            if (connection == null
                || idSolicitacaoHabilitacaoDocentes == null
                || idSolicitacaoHabilitacaoDocentes.Count() == 0)
            {
                return 0;
            }

            var sql = string.Format(
                                    @"INSERT INTO dbo.LY_GRUPO_HABILITACAO_DOC
                                            (
                                              NUM_FUNC,
                                              AGRUPAMENTO,
                                              PROVISORIO,
                                              DT_LIMITE,
                                              AGRUPAMENTO_INGRESSO,
                                              CAMPO_01,
                                              CAMPO_02,
                                              STAMP_ATUALIZACAO,
                                              DATACADASTRO,
                                              USUARIOID
                                            )
                                    SELECT  NUM_FUNC, 
                                            AGRUPAMENTO, 
                                            'N' AS 'PROVISORIO', 
                                            NULL AS 'DT_LIMITE', 
                                            'N' AS 'AGRUPAMENTO_INGRESSO',  
                                            HABILITACAO_MATRICULA,   
                                            HABILITACAO_GLP,  
                                            GETDATE(),  
                                            GETDATE(),
                                            {0}
                                    FROM dbo.TCE_SOLICITACAO_HABILITACAO_DOCENTE
                                    WHERE ID_SOLICITACAO_HABILITACAO_DOCENTE IN ({1})",
                                            HttpContext.Current.User.Identity.Name,
                                            string.Join(", ", idSolicitacaoHabilitacaoDocentes.Select(x => x.ToString()).ToArray()));

            return TCommand.ExecuteNonQuery(connection, sql);
        }

        public ValidacaoDados Valida(LyGrupoHabilitacaoDoc grupoHabilitacaoDoc, decimal pessoa, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (grupoHabilitacaoDoc == null)
            {
                return validacaoDados;
            }

            if (grupoHabilitacaoDoc.NumFunc <= 0 || pessoa <= 0)
            {
                mensagens.Add("Campo DOCENTE é obrigatório.");
            }

            if (grupoHabilitacaoDoc.Agrupamento.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo AGRUPAMENTO é obrigatório.");
            }

            if (grupoHabilitacaoDoc.Provisorio.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo PROVISORIO é obrigatório.");
            }

            if (grupoHabilitacaoDoc.AgrupamentoIngresso.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo AGRUPAMENTO INGRESSO é obrigatório.");
            }
            else if (grupoHabilitacaoDoc.AgrupamentoIngresso != "S" && grupoHabilitacaoDoc.AgrupamentoIngresso != "N")
            {
                mensagens.Add("Campo AGRUPAMENTO INGRESSO inválido.");
            }

            if (grupoHabilitacaoDoc.Campo01.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo HABILITAÇÃO MATRÍCULA é obrigatório.");
            }
            else if (grupoHabilitacaoDoc.Campo01 != "S" && grupoHabilitacaoDoc.Campo01 != "N")
            {
                mensagens.Add("Campo HABILITAÇÃO MATRÍCULA inválido.");
            }

            if (grupoHabilitacaoDoc.Campo02.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo HABILITAÇÃO GLP é obrigatório.");
            }
            else if (grupoHabilitacaoDoc.Campo02 != "S" && grupoHabilitacaoDoc.Campo02 != "N")
            {
                mensagens.Add("Campo HABILITAÇÃO GLP inválido.");
            }

            if (grupoHabilitacaoDoc.Campo01 == "N" && grupoHabilitacaoDoc.Campo02 == "N")
            {
                mensagens.Add("HABILITAÇÃO MATRÍCULA e/ou HABILITAÇÃO GLP devem estar marcadas obrigatoriamente.");
            }

            if (grupoHabilitacaoDoc.Provisorio == "S")
            {
                if (grupoHabilitacaoDoc.DtLimite == null || grupoHabilitacaoDoc.DtLimite == DateTime.MinValue)
                {
                    mensagens.Add("Campo DATA é obrigatório");
                }
                else
                {
                    if (Convert.ToDateTime(grupoHabilitacaoDoc.DtLimite).Date < DateTime.Now.Date)
                    {
                        mensagens.Add("DATA não pode ser menor que data atual.");
                    }

                    if (Convert.ToDateTime(grupoHabilitacaoDoc.DtLimite).Date < new DateTime(1900, 1, 1))
                    {
                        mensagens.Add("DATA não pode ser menor que 1900.");
                    }
                }
            }

            if (grupoHabilitacaoDoc.Documentacao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TIPO TAD/DOCUMENTAÇÃO são obrigatórios.");
            }
            else if (grupoHabilitacaoDoc.Documentacao.Length > 500)
            {
                mensagens.Add("Campo DOCUMENTAÇÃO deve conter no máximo 500 caracteres");
            }

            if (grupoHabilitacaoDoc.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUÁRIO RESPONSÁVEL é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (cadastro)
                    {
                        //Verifica se ja existe outro agrupamento/docente/provisorio
                        if (this.PossuiHabilitacaoPor(contexto, grupoHabilitacaoDoc.NumFunc, grupoHabilitacaoDoc.Agrupamento, grupoHabilitacaoDoc.Provisorio))
                        {
                            mensagens.Add("AGRUPAMENTO já cadastrado para o docente.");
                        }

                        //Ver se no altera tem possibilidade de trocar agrupamento/docente/provisorio
                        if (grupoHabilitacaoDoc.AgrupamentoIngresso == "S")
                        {
                            if (!this.PodeAgrupamentoPor(contexto, grupoHabilitacaoDoc.NumFunc))
                            {
                                mensagens.Add("Não pode haver mais de uma disciplina de 'Grupo de Ingresso' por docente.");
                            }
                        }
                    }

                    if (grupoHabilitacaoDoc.Agrupamento == "Q033")
                    {
                        if (!RN.Capacitacao.PossuiCapacitacaoEdEspecial(Convert.ToString(pessoa)))
                        {
                            mensagens.Add("Somente professores capacitados em Educação Escpecial poderão ser habilitados.");
                        }
                    }

                    if (this.PossuiHabilitacaoProvisoriaVigentePor(contexto, grupoHabilitacaoDoc.NumFunc, grupoHabilitacaoDoc.Agrupamento))
                    {
                        mensagens.Add("Existe uma habilitação provisória vigente para este grupo de disciplina.");
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
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public void Insere(LyGrupoHabilitacaoDoc grupoHabilitacaoDoc)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                this.Insere(contexto, grupoHabilitacaoDoc);
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
                contexto.Dispose();
            }
        }

        public void Atualiza(LyGrupoHabilitacaoDoc grupoHabilitacaoDoc)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE DBO.LY_GRUPO_HABILITACAO_DOC 
	                                        SET DOCUMENTACAO = @DOCUMENTACAO,
                                                CAMPO_01 = @CAMPO_01,
                                                CAMPO_02 = @CAMPO_02,
                                                AGRUPAMENTO_INGRESSO = @AGRUPAMENTO_INGRESSO,
		                                        DATAALTERACAO = @DATAALTERACAO,
		                                        USUARIOID = @USUARIOID
                                        WHERE NUM_FUNC = @NUM_FUNC
	                                        AND AGRUPAMENTO = @AGRUPAMENTO
	                                        AND PROVISORIO = @PROVISORIO  ";

                contextQuery.Parameters.Add("@NUM_FUNC", grupoHabilitacaoDoc.NumFunc);
                contextQuery.Parameters.Add("@AGRUPAMENTO", grupoHabilitacaoDoc.Agrupamento);
                contextQuery.Parameters.Add("@PROVISORIO", grupoHabilitacaoDoc.Provisorio);
                contextQuery.Parameters.Add("@DOCUMENTACAO", grupoHabilitacaoDoc.Documentacao);
                contextQuery.Parameters.Add("@CAMPO_01", grupoHabilitacaoDoc.Campo01);
                contextQuery.Parameters.Add("@CAMPO_02", grupoHabilitacaoDoc.Campo02);
                contextQuery.Parameters.Add("@AGRUPAMENTO_INGRESSO", grupoHabilitacaoDoc.AgrupamentoIngresso);
                contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);
                contextQuery.Parameters.Add("@USUARIOID", grupoHabilitacaoDoc.UsuarioId);

                contexto.ApplyModifications(contextQuery);
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
                contexto.Dispose();
            }
        }

        public void Remove(decimal numFunc, string agrupamento, string provisorio)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE DBO.LY_GRUPO_HABILITACAO_DOC
                                        WHERE NUM_FUNC = @NUM_FUNC
	                                        AND AGRUPAMENTO = @AGRUPAMENTO
	                                        AND PROVISORIO = @PROVISORIO ";

                contextQuery.Parameters.Add("@NUM_FUNC", numFunc);
                contextQuery.Parameters.Add("@AGRUPAMENTO", agrupamento);
                contextQuery.Parameters.Add("@PROVISORIO", provisorio);

                contexto.ApplyModifications(contextQuery);
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
                contexto.Dispose();
            }
        }

        public void Insere(DataContext ctx, LyGrupoHabilitacaoDoc grupoHabilitacaoDoc)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO DBO.LY_GRUPO_HABILITACAO_DOC 
                                                    (NUM_FUNC, 
                                                     AGRUPAMENTO, 
                                                     PROVISORIO, 
                                                     CAMPO_01,
                                                     CAMPO_02,
                                                     STAMP_ATUALIZACAO, 
                                                     AGRUPAMENTO_INGRESSO,
                                                     DOCUMENTACAO,
                                                     DATACADASTRO,
                                                     USUARIOID) 
                                        VALUES      (@NUM_FUNC, 
                                                     @AGRUPAMENTO, 
                                                     @PROVISORIO, 
                                                     @CAMPO_01,
                                                     @CAMPO_02,
                                                     @STAMP_ATUALIZACAO,             
                                                     @AGRUPAMENTO_INGRESSO,
                                                     @DOCUMENTACAO,
                                                     @DATACADASTRO,
                                                     @USUARIOID)  ";

            contextQuery.Parameters.Add("@NUM_FUNC", grupoHabilitacaoDoc.NumFunc);
            contextQuery.Parameters.Add("@AGRUPAMENTO", grupoHabilitacaoDoc.Agrupamento);
            contextQuery.Parameters.Add("@PROVISORIO", grupoHabilitacaoDoc.Provisorio);
            contextQuery.Parameters.Add("@CAMPO_01", grupoHabilitacaoDoc.Campo01);
            contextQuery.Parameters.Add("@CAMPO_02", grupoHabilitacaoDoc.Campo02);
            contextQuery.Parameters.Add("@STAMP_ATUALIZACAO", DateTime.Now);
            contextQuery.Parameters.Add("@AGRUPAMENTO_INGRESSO", grupoHabilitacaoDoc.AgrupamentoIngresso);
            contextQuery.Parameters.Add("@DOCUMENTACAO", grupoHabilitacaoDoc.Documentacao);
            contextQuery.Parameters.Add("@DATACADASTRO", DateTime.Now);
            contextQuery.Parameters.Add("@USUARIOID", grupoHabilitacaoDoc.UsuarioId ?? HttpContext.Current.User.Identity.Name);

            ctx.ApplyModifications(contextQuery);
        }

        public bool PossuiHabilitacaoProvisoriaVigentePor(decimal numFunc, string agrupamento)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool possui = false;

            try
            {
                possui = this.PossuiHabilitacaoProvisoriaVigentePor(ctx, numFunc, agrupamento);
                return possui;
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

        public bool PossuiHabilitacaoProvisoriaVigentePor(DataContext ctx, decimal numFunc, string agrupamento)
        {
            bool possui = false;

            ContextQuery contextQuery = new ContextQuery
            {
                Command = @" SELECT   COUNT(*)
                            FROM    LY_GRUPO_HABILITACAO_DOC                                     
                            WHERE  NUM_FUNC= @NUM_FUNC
                                   AND AGRUPAMENTO = @AGRUPAMENTO
                                   AND PROVISORIO = 'S' and DT_LIMITE >= convert(date,GETDATE()) "
            };

            contextQuery.Parameters.Add("@NUM_FUNC", numFunc);
            contextQuery.Parameters.Add("@AGRUPAMENTO", agrupamento);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public bool PossuiHabilitacaoAlocacaoNormalPor(decimal numFunc, string agrupamento)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool possui = false;

            try
            {
                possui = this.PossuiHabilitacaoAlocacaoNormalPor(ctx, numFunc, agrupamento);
                return possui;
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

        public bool PossuiHabilitacaoAlocacaoNormalPor(DataContext ctx, decimal numFunc, string agrupamento)
        {
            bool possui = false;

            ContextQuery contextQuery = new ContextQuery
            {
                Command = @" SELECT   COUNT(*)
                            FROM    LY_GRUPO_HABILITACAO_DOC                                     
                            WHERE  NUM_FUNC= @NUM_FUNC
                                   AND AGRUPAMENTO = @AGRUPAMENTO
                                   AND CAMPO_01 = 'S'
                                   AND (PROVISORIO = 'N' or PROVISORIO = 'S' and DT_LIMITE >= convert(date,GETDATE())) "

            };

            contextQuery.Parameters.Add("@NUM_FUNC", numFunc);
            contextQuery.Parameters.Add("@AGRUPAMENTO", agrupamento);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public bool PossuiHabilitacaoPor(DataContext ctx, decimal numFunc, string agrupamento, string provisorio)
        {
            bool possui = false;
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT   COUNT(*)
                            FROM    LY_GRUPO_HABILITACAO_DOC                                     
                            WHERE  NUM_FUNC= @NUM_FUNC
                                   AND AGRUPAMENTO = @AGRUPAMENTO
                                   AND PROVISORIO = @PROVISORIO ";

            contextQuery.Parameters.Add("@NUM_FUNC", numFunc);
            contextQuery.Parameters.Add("@AGRUPAMENTO", agrupamento);
            contextQuery.Parameters.Add("@PROVISORIO", provisorio);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public string ObtemDisciplinadeIngressoPor(decimal numFunc)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            try
            {
                contextQuery.Command = @" SELECT DESCRICAO
                                         FROM ly_grupo_habilitacao_doc ghdoc 
                                         inner JOIN ly_grupo_habilitacao gh ON ghdoc.agrupamento = gh.agrupamento 
                                         where NUM_FUNC = @NUM_FUNC
                                         and AGRUPAMENTO_INGRESSO = 'S' ";

                contextQuery.Parameters.Add("@NUM_FUNC", TechneDbType.T_NUMFUNC, numFunc);

                resultado = contexto.GetReturnValue<string>(contextQuery);

                return resultado;
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
                contexto.Dispose();
            }
        }


        public DataTable ObtemHabilitacaoPor(decimal numFunc, string agrupamento)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT  *
                            FROM    LY_GRUPO_HABILITACAO_DOC                                     
                            WHERE  NUM_FUNC= @NUM_FUNC
                                   AND AGRUPAMENTO = @AGRUPAMENTO
                                   AND (PROVISORIO = 'N' or PROVISORIO = 'S' and DT_LIMITE >= convert(date,GETDATE()))";

                contextQuery.Parameters.Add("@NUM_FUNC", numFunc);
                contextQuery.Parameters.Add("@AGRUPAMENTO", agrupamento);

                dt = ctx.GetDataTable(contextQuery);
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
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }

            return dt;
        }

        public DataTable ListaGrupoHabilitacao(string[] componentes)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"
                    select distinct lygh.AGRUPAMENTO, lygh.DESCRICAO from LY_GRUPO_HABILITACAO_DOC lyghd (nolock)
                    inner join LY_GRUPO_HABILITACAO lygh (nolock) on lyghd.AGRUPAMENTO = lygh.AGRUPAMENTO
                    where (lygh.AGRUPAMENTO in (
	                      select distinct lygd.AGRUPAMENTO from 
	                      LY_GRUPO_HABILITACAO_DISC lygd (nolock)
	                      inner join LY_DISCIPLINA lyd (nolock) on lygd.DISCIPLINA = lyd.DISCIPLINA
	                      where lyd.COMPONENTE in (select value from string_split(@COMPONENTE, ',')
                    ))
                    or @COMPONENTE is null)
                    order by lygh.DESCRICAO
                ";

                contextQuery.Parameters.Add("@COMPONENTE", componentes != null && componentes.Length > 0 ? string.Join(",", componentes) : null);

                dt = ctx.GetDataTable(contextQuery);
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
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }

            return dt;
        }
    }
}

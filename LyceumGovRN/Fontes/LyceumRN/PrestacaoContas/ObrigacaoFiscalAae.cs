using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class ObrigacaoFiscalAae
    {
        public bool PossuiDeclaracaoAaePor(DataContext contexto, int declaracaoAaeId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM PrestacaoContas.OBRIGACAOFISCALAAE (NOLOCK)
                                    WHERE DECLARACAOAAEID = @DECLARACAOAAEID ";

            contextQuery.Parameters.Add("@DECLARACAOAAEID", SqlDbType.Int, declaracaoAaeId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public DataTable ListaDeclaracaoAaePor(string censo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT D.DECLARACAOAAEID,
	                                         O.ANOBASE,
	                                         CASE
												WHEN O.MES = 1 THEN 'Janeiro'
												WHEN O.MES = 2 THEN 'Fevereiro'
												WHEN O.MES = 3 THEN 'Março'
												WHEN O.MES = 4 THEN 'Abril'
												WHEN O.MES = 5 THEN 'Maio'
												WHEN O.MES = 6 THEN 'Junho'
												WHEN O.MES = 7 THEN 'Julho'
												WHEN O.MES = 8 THEN 'Agosto'
												WHEN O.MES = 9 THEN 'Setembro'
												WHEN O.MES = 10 THEN 'Outubro'
												WHEN O.MES = 11 THEN 'Novembro'
												WHEN O.MES = 12 THEN 'Dezembro'
											 END MES,
	                                         D.DESCRICAO,
	                                         CASE
                                                 WHEN D.PERIODICIDADE = 0 THEN 'Sem periodiciade'
                                                 WHEN D.PERIODICIDADE = 1 THEN 'Mensal'
                                                 WHEN D.PERIODICIDADE = 2 THEN 'Bimestral'
                                                 WHEN D.PERIODICIDADE = 3 THEN 'Trimestral'
                                                 WHEN D.PERIODICIDADE = 6 THEN 'Semestral'                       
                                                 WHEN D.PERIODICIDADE = 12 THEN 'Anual'
                                                 ELSE 'Sim'
                                              END PERIODICIDADE,
	                                          CASE
                                                 WHEN DECLARACAOFISCALARQUIVOID IS NULL THEN 'Não'
                                                 ELSE 'Sim'
                                               END ENVIADO, 
	                                           O.OBRIGACAOFISCALAAEID,
	                                           A.DECLARACAOFISCALARQUIVOID,
	                                           CASE
                                                 WHEN D.OBRIGATORIO = 0 THEN 'Não'
                                                 ELSE 'Sim'
                                               END OBRIGATORIO,
											   A.ARQUIVO,
											   A.TIPOARQUIVO,
											   A.NOMEARQUIVO
                                        from [PrestacaoContas].DECLARACAOAAE D
	                                        LEFT JOIN [PrestacaoContas].[OBRIGACAOFISCALAAE] O 
			                                        ON D.DECLARACAOAAEID = O.DECLARACAOAAEID
			                                        AND O.CENSO = @CENSO
	                                        LEFT JOIN PRESTACAOCONTAS.DECLARACAOFISCALARQUIVO A
                                                      ON A.OBRIGACAOFISCALAAEID = O.OBRIGACAOFISCALAAEID
                                        WHERE  D.DATAINICIO <= GETDATE()
                                               AND ( D.DATAFIM IS NULL
                                                      OR D.DATAFIM >= GETDATE() )  ";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);

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

        public ValidacaoDados Valida(Entidades.ObrigacaoFiscalAae obrigacaoFiscalAae, Entidades.DeclaracaoFiscalArquivo declaracaoFiscalArquivo, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (obrigacaoFiscalAae == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (obrigacaoFiscalAae.ObrigacaoFiscalAaeId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
                else
                {
                    declaracaoFiscalArquivo.ObrigacaoFiscalAaeId = obrigacaoFiscalAae.ObrigacaoFiscalAaeId;
                }
            }

            if (obrigacaoFiscalAae.DeclaracaoAaeId <= 0)
            {
                mensagens.Add("Campo DECLARAÇÃO é obrigatório.");
            }

            if (obrigacaoFiscalAae.Censo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo UNIDADE DE ENSINO é obrigatório.");
            }

            if (obrigacaoFiscalAae.AnoBase <= 0)
            {
                mensagens.Add("Campo ANO BASE é obrigatório.");
            }

            if (obrigacaoFiscalAae.Mes <= 0)
            {
                mensagens.Add("Campo MÊS é obrigatório.");
            }

            if (declaracaoFiscalArquivo.Arquivo == null || declaracaoFiscalArquivo.Arquivo.Count() <= 0)
            {
                mensagens.Add("Campo ARQUIVO é obrigatório.");
            }

            if (declaracaoFiscalArquivo.TipoArquivo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TIPO ARQUIVO é obrigatório.");
            }
            else
            {
                //Apenas aceitar pdf e imagem 
                if (declaracaoFiscalArquivo.TipoArquivo.ToUpper() != "IMAGE/JPEG"
                    && declaracaoFiscalArquivo.TipoArquivo.ToUpper() != "APPLICATION/PDF")
                {
                    mensagens.Add("Apenas serão aceitos arquivos dos tipos .jpeg e .pdf .");
                }
            }

            if (declaracaoFiscalArquivo.NomeArquivo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NOME ARQUIVO é obrigatório.");
            }

            if (obrigacaoFiscalAae.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUÁRIO é obrigatório.");
            }
            else
            {
                declaracaoFiscalArquivo.UsuarioId = obrigacaoFiscalAae.UsuarioId;
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

        public void Insere(Entidades.ObrigacaoFiscalAae obrigacaoFiscalAae, Entidades.DeclaracaoFiscalArquivo declaracaoFiscalArquivo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            DeclaracaoFiscalArquivo rnDeclaracaoFiscalArquivo = new DeclaracaoFiscalArquivo();

            try
            {
                //Insere ObrigacaoFiscalAae
                this.Insere(ctx, obrigacaoFiscalAae);

                //Insere declaracaoFiscalArquivo
                declaracaoFiscalArquivo.ObrigacaoFiscalAaeId = obrigacaoFiscalAae.ObrigacaoFiscalAaeId;
                rnDeclaracaoFiscalArquivo.Insere(ctx, declaracaoFiscalArquivo);

                //Insere auditoria declaracaoFiscalArquivo
                rnDeclaracaoFiscalArquivo.InsereAuditoria(ctx, declaracaoFiscalArquivo, "CADASTRADO", System.Web.HttpContext.Current.Request.UserHostName);
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

        private void Insere(DataContext contexto, Entidades.ObrigacaoFiscalAae obrigacaoFiscalAae)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  INSERT INTO PrestacaoContas.OBRIGACAOFISCALAAE
                                                       (DECLARACAOAAEID
                                                       ,CENSO
                                                       ,ANOBASE
                                                       ,MES
                                                       ,USUARIOID
                                                       ,DATACADASTRO
                                                       ,DATAALTERACAO)
                                                 VALUES
                                                       (@DECLARACAOAAEID, 
                                                       @CENSO, 
                                                       @ANOBASE, 
                                                       @MES, 
                                                       @USUARIOID,
                                                       @DATACADASTRO,
                                                       @DATAALTERACAO) 

                                        SELECT IDENT_CURRENT('PRESTACAOCONTAS.OBRIGACAOFISCALAAE') ";

            contextQuery.Parameters.Add("@DECLARACAOAAEID", SqlDbType.Int, obrigacaoFiscalAae.DeclaracaoAaeId);
            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, obrigacaoFiscalAae.Censo);
            contextQuery.Parameters.Add("@ANOBASE", SqlDbType.Int, obrigacaoFiscalAae.AnoBase);
            contextQuery.Parameters.Add("@MES", SqlDbType.Int, obrigacaoFiscalAae.Mes);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, obrigacaoFiscalAae.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            obrigacaoFiscalAae.ObrigacaoFiscalAaeId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        public void Atualiza(Entidades.ObrigacaoFiscalAae obrigacaoFiscalAae, Entidades.DeclaracaoFiscalArquivo declaracaoFiscalArquivo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            DeclaracaoFiscalArquivo rnDeclaracaoFiscalArquivo = new DeclaracaoFiscalArquivo();

            try
            {
                //Atualiza  ObrigacaoFiscalAae
                this.Atualiza(ctx, obrigacaoFiscalAae);

                //Atualiza declaracaoFiscalArquivo
                rnDeclaracaoFiscalArquivo.Atualiza(ctx, declaracaoFiscalArquivo);

                //Insere auditoria declaracaoFiscalArquivo
                rnDeclaracaoFiscalArquivo.InsereAuditoria(ctx, declaracaoFiscalArquivo, "ALTERADO", System.Web.HttpContext.Current.Request.UserHostName);
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

        public void Atualiza(DataContext contexto, Entidades.ObrigacaoFiscalAae obrigacaoFiscalAae)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE PrestacaoContas.OBRIGACAOFISCALAAE
                                       SET CENSO = @CENSO, 
                                          ANOBASE = @ANOBASE, 
                                          MES = @MES, 
                                          USUARIOID = @USUARIOID, 
                                          DATAALTERACAO = @DATAALTERACAO 
                                     WHERE OBRIGACAOFISCALAAEID = @OBRIGACAOFISCALAAEID ";

            contextQuery.Parameters.Add("@OBRIGACAOFISCALAAEID", SqlDbType.Int, obrigacaoFiscalAae.ObrigacaoFiscalAaeId);
            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, obrigacaoFiscalAae.Censo);
            contextQuery.Parameters.Add("@ANOBASE", SqlDbType.Int, obrigacaoFiscalAae.AnoBase);
            contextQuery.Parameters.Add("@MES", SqlDbType.Int, obrigacaoFiscalAae.Mes);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, obrigacaoFiscalAae.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }
    }
}

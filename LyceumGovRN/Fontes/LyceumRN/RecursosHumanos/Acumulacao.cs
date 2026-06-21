using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data.SqlClient;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.RecursosHumanos
{
    public class Acumulacao
    {
        public RecursosHumanos.Entidades.Acumulacao ObtemPor(decimal docenteId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            RecursosHumanos.Entidades.Acumulacao acumulacao = new RecursosHumanos.Entidades.Acumulacao();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT   * 
                                            FROM   RECURSOSHUMANOS.ACUMULACAO 
                                            WHERE  DOCENTEID = @DOCENTEID  ";

                contextQuery.Parameters.Add("@DOCENTEID", docenteId);

                acumulacao = ctx.TryToBindEntity<RecursosHumanos.Entidades.Acumulacao>(contextQuery);

                return acumulacao;
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

        public RecursosHumanos.Entidades.Acumulacao ObtemPor(string processoSeletivo, string numeroInscricao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            RecursosHumanos.Entidades.Acumulacao acumulacao = new RecursosHumanos.Entidades.Acumulacao();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT A.* 
                                        FROM   RECURSOSHUMANOS.ACUMULACAO A (NOLOCK)
                                                INNER JOIN LY_DOCENTE DO (NOLOCK)
                                                        ON A.DOCENTEID = DO.NUM_FUNC 
                                        WHERE  DO.CONCURSO = @CONCURSO 
                                                AND DO.CANDIDATO = @CANDIDATO ";

                contextQuery.Parameters.Add("@CONCURSO", processoSeletivo);
                contextQuery.Parameters.Add("@CANDIDATO", numeroInscricao);

                acumulacao = ctx.TryToBindEntity<RecursosHumanos.Entidades.Acumulacao>(contextQuery);

                return acumulacao;
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

        public void Salva(DataContext ctx, RecursosHumanos.Entidades.Acumulacao acumulacao)
        {
             bool existe = this.PossuiAcumulacaoPor(ctx, acumulacao.DocenteId);

            //Verifica se o docente já possui acumulação
            if (existe)
            {
                //Verifica se a acumulação esta sendo retirada
                if (acumulacao.MatriculaOrgao.IsNullOrEmptyOrWhiteSpace())
                {
                    Remove(ctx, acumulacao.DocenteId);
                }
                else
                {
                    this.Atualiza(ctx, acumulacao);
                }
            }
            else
            {
                //Verifica se a acumulação esta sendo incluida
                if (!acumulacao.MatriculaOrgao.IsNullOrEmptyOrWhiteSpace())
                {
                    this.Insere(ctx, acumulacao);
                }
            }
        }

        public bool PossuiAcumulacaoPor(DataContext ctx, decimal docenteId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT   COUNT(*) 
                                            FROM   RECURSOSHUMANOS.ACUMULACAO 
                                            WHERE  DOCENTEID = @DOCENTEID  ";

            contextQuery.Parameters.Add("@DOCENTEID", docenteId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(DataContext ctx, RecursosHumanos.Entidades.Acumulacao acumulacao)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO RECURSOSHUMANOS.ACUMULACAO 
                                        (DOCENTEID, 
                                         ORGAO, 
                                         MATRICULAORGAO, 
                                         NUMEROPROCESSO, 
                                         USUARIOID, 
                                         DATACADASTRO, 
                                         DATAALTERACAO) 
                                    VALUES      
	                                    (@DOCENTEID, 
                                        @ORGAO, 
                                        @MATRICULAORGAO, 
                                        @NUMEROPROCESSO, 
                                        @USUARIOID, 
                                        @DATACADASTRO, 
                                        @DATAALTERACAO)  ";

                contextQuery.Parameters.Add("@DOCENTEID", acumulacao.DocenteId);
                contextQuery.Parameters.Add("@ORGAO", acumulacao.Orgao);
                contextQuery.Parameters.Add("@MATRICULAORGAO", acumulacao.MatriculaOrgao);
                contextQuery.Parameters.Add("@NUMEROPROCESSO", acumulacao.NumeroProcesso);
                contextQuery.Parameters.Add("@USUARIOID", acumulacao.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);

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
        }

        public void Atualiza(DataContext ctx, RecursosHumanos.Entidades.Acumulacao acumulacao)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE RECURSOSHUMANOS.ACUMULACAO 
                                            SET    ORGAO = @ORGAO, 
                                                   MATRICULAORGAO = @MATRICULAORGAO, 
                                                   NUMEROPROCESSO = @NUMEROPROCESSO, 
                                                   USUARIOID = @USUARIOID, 
                                                   DATAALTERACAO = @DATAALTERACAO 
                                           WHERE  DOCENTEID = @DOCENTEID  ";

                contextQuery.Parameters.Add("@DOCENTEID", acumulacao.DocenteId);
                contextQuery.Parameters.Add("@ORGAO", acumulacao.Orgao);
                contextQuery.Parameters.Add("@MATRICULAORGAO", acumulacao.MatriculaOrgao);
                contextQuery.Parameters.Add("@NUMEROPROCESSO", acumulacao.NumeroProcesso);
                contextQuery.Parameters.Add("@USUARIOID", acumulacao.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);

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
        }

        public void Remove(DataContext ctx, decimal docenteId)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE RECURSOSHUMANOS.ACUMULACAO 
                                        WHERE  DOCENTEID = @DOCENTEID  ";

                contextQuery.Parameters.Add("@DOCENTEID", docenteId);

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
        }
    }
}

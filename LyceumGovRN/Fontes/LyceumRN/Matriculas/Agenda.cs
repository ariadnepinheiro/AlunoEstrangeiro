using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.Matriculas
{
    public class Agenda
    {

        public bool EhPeriodoFase1VigentePor(int ano)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool periodo = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                                        FROM     MATRICULA.AGENDA
                                        WHERE  FASE = 1
                                               AND ANO = @ANO
                                               AND GETDATE() BETWEEN DATAINICIO AND DATAFIM ";

                contextQuery.Parameters.Add("@ANO", ano);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    periodo = true;
                }

                return periodo;
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

        public DateTime RetornaDataBaseMatricula(DataContext contexto, int ano)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" 
            select top 1 DATABASEMATRICULA
            from MATRICULA.AGENDA A (NOLOCK)
            where ANO = @ANO
            and getdate() between A.DATAINICIO and A.DATAFIM
            ";

            contextQuery.Parameters.Add("@ANO", ano);

            return contexto.GetReturnValue<DateTime>(contextQuery);
        }

        public int RetornaAnoCorrenteMatricula(DataContext contexto)
        {
            int ano = 0;

            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" 
            select top 1 ANO
            from MATRICULA.AGENDA A (NOLOCK)
            where getdate() between A.DATAINICIO and A.DATAFIM
            ";

            ano = contexto.GetReturnValue(contextQuery) == null ? 0 : contexto.GetReturnValue<int>(contextQuery);

            return ano;
        }

        public int RetornaAnoCorrenteMatricula()
        {
            var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return RetornaAnoCorrenteMatricula(ctx);
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

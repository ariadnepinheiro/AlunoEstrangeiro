using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;

namespace Techne.Lyceum.RN
{
   public class HistoricoDocente
    {
       public bool PossuiHistoricoCadastradoPor(string aluno, decimal ordem, decimal ano, decimal semestre, string disciplina, decimal numFunc)
       {
           DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
           bool possui = false;

           try
           {
               ContextQuery contextQuery = new ContextQuery
               {
                   Command = @"SELECT  COUNT(*)
                        FROM    LY_HISTORICO_DOCENTE
                        WHERE   ALUNO = @ALUNO
                                AND ORDEM = @ORDEM
                                AND ANO = @ANO
                                AND PERIODO = @PERIODO
                                AND DISCIPLINA = @DISCIPLINA
                                AND NUM_FUNC = @NUM_FUNC  "
               };

               contextQuery.Parameters.Add("@ALUNO", aluno);
               contextQuery.Parameters.Add("@ORDEM", ordem);
               contextQuery.Parameters.Add("@ANO", ano);
               contextQuery.Parameters.Add("@PERIODO", semestre);
               contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
               contextQuery.Parameters.Add("@NUM_FUNC", numFunc);          

               if (ctx.GetReturnValue<int>(contextQuery) > 0)
               {
                   possui = true;
               }

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

       public void Insere(DataContext ctx, LyHistoricoDocente historicoDocente)
       {
           try
           {
               ContextQuery contextQuery = new ContextQuery();

               contextQuery.Command = @" INSERT  INTO dbo.LY_HISTORICO_DOCENTE
                                ( ALUNO ,
                                  ORDEM ,
                                  ANO ,
                                  PERIODO ,
                                  DISCIPLINA ,
                                  NUM_FUNC
                                )
                        VALUES  ( @ALUNO ,
                                  @ORDEM ,
                                  @ANO ,
                                  @PERIODO ,
                                  @DISCIPLINA ,
                                  @NUM_FUNC
                                ) ";

               contextQuery.Parameters.Add("@ALUNO", historicoDocente.Aluno);
               contextQuery.Parameters.Add("@ORDEM", historicoDocente.Ordem);
               contextQuery.Parameters.Add("@ANO", historicoDocente.Ano);
               contextQuery.Parameters.Add("@PERIODO", historicoDocente.Periodo);
               contextQuery.Parameters.Add("@DISCIPLINA", historicoDocente.Disciplina);
               contextQuery.Parameters.Add("@NUM_FUNC", historicoDocente.NumFunc);

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

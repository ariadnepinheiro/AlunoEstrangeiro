using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Util;
using System.Data;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN.Agenda
{
    public class TipoEventoCorrelato : RNBase
    {
        public static DataTable ListaTipoEventoCorrelato()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable agenda = null;

            try
            {
                contextQuery.Command = @" SELECT TIPOEVENTOCORRELATOID,
                                                 TIPOEVENTOID         ,
                                                 TIPOEVENTOPAIID          
                                            FROM agenda.TIPOEVENTOCORRELATO     
                                        ORDER BY TIPOEVENTOCORRELATOID ";

                agenda = ctx.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }

            return agenda;
        }

        public static void InsereTipoEventoCorrelato(Entidades.TipoEventoCorrelato TipoEventoCorrelato)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @"INSERT INTO agenda.TIPOEVENTOCORRELATO( 
                                        TIPOEVENTOID   ,
                                        TIPOEVENTOPAIID       
                                    ) VALUES ( @TIPOEVENTOID   ,
                                               @TIPOEVENTOPAIID
                                             )"
                };

                contextQuery.Parameters.Add("@TIPOEVENTOID", TipoEventoCorrelato.TipoEventoId);
                contextQuery.Parameters.Add("@TIPOEVENTOPAIID", TipoEventoCorrelato.TipoEventoPaiId);

                ctx.ApplyModifications(contextQuery);
            }

            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Format(@"Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0} Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public static void AlteraTipoEventoCorrelato(Entidades.TipoEventoCorrelato TipoEventoCorrelato)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @"UPDATE agenda.TIPOEVENTOCORRELATO
                                   SET TIPOEVENTOID    = @TIPOEVENTOID   ,
                                       TIPOEVENTOPAIID = @TIPOEVENTOPAIID       
                                 WHERE TIPOEVENTOCORRELATOID  = @TIPOEVENTOCORRELATOID "
                };

                contextQuery.Parameters.Add("@TIPOEVENTOCORRELATOID", TipoEventoCorrelato.TipoEventoCorrelatoId);
                contextQuery.Parameters.Add("@TIPOEVENTOID", TipoEventoCorrelato.TipoEventoId);
                contextQuery.Parameters.Add("@TIPOEVENTOPAIID", TipoEventoCorrelato.TipoEventoPaiId);

                ctx.ApplyModifications(contextQuery);
            }

            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Format(@"Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0} Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public static void RemoveTipoEventoCorrelato(int TipoEventoCorrelatoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" DELETE agenda.TIPOEVENTOCORRELATO
                                  WHERE TIPOEVENTOCORRELATOID = @TIPOEVENTOCORRELATOID "
                };

                contextQuery.Parameters.Add("@TIPOEVENTOCORRELATOID", TipoEventoCorrelatoId);

                ctx.ApplyModifications(contextQuery);
            }

            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Format(@"Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0} Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

    }
}

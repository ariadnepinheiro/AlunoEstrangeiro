using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Web;

namespace Techne.Lyceum.RN.TurnosVagas
{
    public class HistoricoJustificativa : RNBase
    {

        public void InsereHistoricoJustificativa(DataContext ctx, int ano, string periodos, string censo, bool turno, bool vaga, int tipoHistorico)
        {
            ContextQuery contextQuery = new ContextQuery();
            StringBuilder sql = new StringBuilder();
            string usuarioResponsavel = HttpContext.Current.User.Identity.Name;

            try
            {
                sql.Append(@" INSERT  INTO TurnosVagas.HISTORICOJUSTIFICATIVA
                            ( ID_AGENDA_CONF_TURNO_VAGA ,
                              HISTORICOTURNOVAGAID ,
                              CENSO ,
                              JUSTIFICATIVACONTINUIDADE ,
                              JUSTIFICATIVANOVO ,
                              TURNO ,
                              VAGA ,
                              VAGASNOVAS ,
                              VAGASCONTINUIDADE ,
                              DATACADASTRO ,
                              DATAALTERACAO ,
                              MATRICULA                                
                            ) ");

                if (tipoHistorico == (int)RN.TurnosVagas.TipoHistorico.TiposHistorico.Diretor)
                {
                    sql.Append(string.Format(@"   SELECT  DISTINCT
                                    T.ID_AGENDA_CONF_TURNO_VAGA ,
                                    H.HISTORICOTURNOVAGAID,
                                    T.CENSO ,
                                    T.JUSTIFICATIVA_CONTINUIDADE ,
                                    T.JUSTIFICATIVA_NOVO ,
                                    T.TURNO ,
                                    T.VAGA ,
                                    T.VAGAS_NOVAS ,
                                    T.VAGAS_CONTINUIDADE ,
                                    T.DT_CADASTRO ,
                                    T.DT_ALTERACAO ,
                                    T.MATRICULA
                          FROM      DBO.TCE_CTV_JUSTIFICATIVA T
                                    INNER JOIN DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA A ON T.ID_AGENDA_CONF_TURNO_VAGA = A.ID_AGENDA_CONF_TURNO_VAGA
                                    INNER JOIN TurnosVagas.HISTORICOTURNOVAGA H ON A.ANO = H.ANO
                                                                               AND A.PERIODO = H.PERIODO
                                                                               AND T.CENSO = H.CENSO
                          WHERE     T.CENSO = @CENSO
                                    AND A.ANO = @ANO
                                    AND A.PERIODO IN ( {0} )
                                    AND T.TURNO = @TURNO
                                    AND T.VAGA = @VAGA
                                    AND h.TIPOHISTORICOID = @TIPOHISTORICOID ", periodos));
                }

                if (tipoHistorico == (int)RN.TurnosVagas.TipoHistorico.TiposHistorico.Seeduc)
                {
                    sql.Append(string.Format(@"   SELECT  DISTINCT
                                        T.ID_AGENDA_CONF_TURNO_VAGA ,
                                        H.HISTORICOTURNOVAGAID ,
                                        T.CENSO ,
                                        T.JUSTIFICATIVA_CONTINUIDADE ,
                                        T.JUSTIFICATIVA_NOVO ,
                                        T.TURNO ,
                                        T.VAGA ,
                                        T.VAGAS_NOVAS ,
                                        T.VAGAS_CONTINUIDADE ,
                                        T.DT_CADASTRO ,
                                        GETDATE() ,
                                        @MATRICULA
                              FROM      DBO.TCE_CTV_JUSTIFICATIVA T
                                        INNER JOIN DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA A ON T.ID_AGENDA_CONF_TURNO_VAGA = A.ID_AGENDA_CONF_TURNO_VAGA
                                        INNER JOIN TurnosVagas.HISTORICOTURNOVAGA H ON A.ANO = H.ANO
                                                                                       AND A.PERIODO = H.PERIODO
                                                                                       AND T.CENSO = H.CENSO
                              WHERE     T.CENSO = @CENSO
                                        AND A.ANO = @ANO
                                        AND A.PERIODO IN ( {0} )
                                        AND T.TURNO = @TURNO
                                        AND T.VAGA = @VAGA
                                        AND h.TIPOHISTORICOID = @TIPOHISTORICOID ", periodos));
                }

                contextQuery.Command = sql.ToString();

                contextQuery.Parameters.Add("@TIPOHISTORICOID", tipoHistorico);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@VAGA", vaga);
                contextQuery.Parameters.Add("@MATRICULA", usuarioResponsavel);

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

        public void DeletaHistoricoJustificativa(DataContext ctx, int ano, string periodos, string censo, bool turno, bool vaga, int tipoHistorico)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = string.Format(@" DELETE  J
                            FROM    TurnosVagas.HISTORICOJUSTIFICATIVA J
                                    INNER JOIN TurnosVagas.HISTORICOTURNOVAGA H ON J.HISTORICOTURNOVAGAID = H.HISTORICOTURNOVAGAID
                            WHERE   H.TIPOHISTORICOID = @TIPOHISTORICOID
                                    AND H.CENSO = @CENSO
                                    AND H.ANO = @ANO
                                    AND H.PERIODO IN ( {0} )
                                    AND TURNO = @TURNO
                                    AND VAGA = @VAGA ", periodos);

                contextQuery.Parameters.Add("@TIPOHISTORICOID", tipoHistorico);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@VAGA", vaga);

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
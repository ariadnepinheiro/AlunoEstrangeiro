using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Web;

namespace Techne.Lyceum.RN.TurnosVagas
{
    public class HistoricoSalaAula
    {
        public void InsereHistoricoSala(DataContext ctx, int ano, string periodos, string censo)
        {

            ContextQuery contextQuery = new ContextQuery();
            StringBuilder sql = new StringBuilder();
            string usuarioResponsavel = HttpContext.Current.User.Identity.Name;

            try
            {
                //Cria historico de salas ativas
                sql.Append(@" INSERT INTO TURNOSVAGAS.HISTORICOSALAAULA
                                            (CENSO, 
                                             ANO, 
                                             PERIODO, 
                                             SALA, 
                                             NUMEROALUNO, 
                                             USUARIOID, 
                                             DATACADASTRO, 
                                             DATAALTERACAO) 
                                SELECT DISTINCT D.FACULDADE AS CENSO, 
                                                @ANO, 
                                                @PERIODO, 
                                                D.DEPENDENCIA AS SALA, 
                                                D.NUM_ALUNOS  AS NUMEROALUNO, 
                                                @MATRICULA    AS USUARIOID, 
                                                GETDATE()     AS DATACADASTRO, 
                                                GETDATE()     AS DATAALTERACAO
                                FROM   LY_DEPENDENCIA D 
								WHERE  D.FACULDADE = @CENSO
                                    AND D.ATIVA = 'S'
                                    AND ( D.DEPENDENCIA LIKE 'SL-%'
                                          OR D.DEPENDENCIA LIKE 'SR-%'
                                        ) 
									AND NOT EXISTS (SELECT TOP 1 1 FROM TURNOSVAGAS.HISTORICOSALAAULA HS
							                                WHERE HS.CENSO = @CENSO
							                                    AND HS.SALA = D.DEPENDENCIA
							                                    AND HS.ANO = @ANO
							                                    AND HS.PERIODO = @PERIODO)

                            ");

                //Cria salas utilizadas que ainda não existam
                sql.Append(string.Format(@" INSERT INTO TURNOSVAGAS.HISTORICOSALAAULA
                                            (CENSO, 
                                             ANO, 
                                             PERIODO, 
                                             SALA, 
                                             NUMEROALUNO, 
                                             USUARIOID, 
                                             DATACADASTRO, 
                                             DATAALTERACAO) 
                                SELECT DISTINCT D.FACULDADE AS CENSO, 
                                                A.ANO, 
                                                CASE 
                                                  WHEN A.PERIODO = 2 THEN 2 
                                                  ELSE 0 
                                                END           PERIODO, 
                                                D.DEPENDENCIA AS SALA, 
                                                D.NUM_ALUNOS  AS NUMEROALUNO, 
                                                @MATRICULA    AS USUARIOID, 
                                                GETDATE()     AS DATACADASTRO, 
                                                GETDATE()     AS DATAALTERACAO
                                FROM   TURNOSVAGAS.HISTORICOVAGA V 
                                       INNER JOIN TURNOSVAGAS.HISTORICOTURNOVAGA H 
                                               ON V.HISTORICOTURNOVAGAID = H.HISTORICOTURNOVAGAID 
                                       INNER JOIN DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA A 
                                               ON V.ID_AGENDA_CONF_TURNO_VAGA = A.ID_AGENDA_CONF_TURNO_VAGA 
                                       INNER JOIN LY_DEPENDENCIA D 
                                               ON D.FACULDADE = V.CENSO 
                                                  AND D.DEPENDENCIA = V.SALA 
	                                 WHERE   V.CENSO = @CENSO
                                             AND A.ANO = @ANO
                                             AND A.PERIODO IN ( {0} )
			                                 AND NOT EXISTS (SELECT TOP 1 1 FROM TURNOSVAGAS.HISTORICOSALAAULA HS
							                                WHERE HS.CENSO = @CENSO
							                                    AND HS.SALA = D.DEPENDENCIA
							                                    AND HS.ANO = @ANO
							                                    AND HS.PERIODO IN ( {0} )) 
                                 
                        ", periodos));
                
                contextQuery.Command = sql.ToString();

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@MATRICULA", usuarioResponsavel);

                if (periodos.Contains("2"))
                {
                    contextQuery.Parameters.Add("@PERIODO", 2);
                }
                else
                {
                    contextQuery.Parameters.Add("@PERIODO", 0);
                }

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

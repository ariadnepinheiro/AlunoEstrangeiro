using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;

namespace Techne.Lyceum.RN
{
    public class CtvLogEncerramento : RNBase
    {
        public static void Inserir(DataContext context, TceCtvLogEncerramento logEncerramento)
        {
            var contextQuery = new ContextQuery
            {
                Command = @" INSERT INTO dbo.TCE_CTV_LOG_ENCERRAMENTO
                                ( ID_AGENDA_CONF_TURNO_VAGA ,
                                  CENSO ,
                                  TURMA ,
                                  RESTRICAO ,
                                  MATRICULA 
                                )
                        VALUES  ( @ID_AGENDA_CONF_TURNO_VAGA ,
                                  @CENSO ,
                                  @TURMA ,
                                  @RESTRICAO ,
                                  @MATRICULA 
                                ) "
            };

            contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", logEncerramento.IdAgendaConfTurnoVaga);
            contextQuery.Parameters.Add("@CENSO", logEncerramento.Censo);
            contextQuery.Parameters.Add("@TURMA", logEncerramento.Turma);
            contextQuery.Parameters.Add("@RESTRICAO", logEncerramento.Restricao);
            contextQuery.Parameters.Add("@MATRICULA", logEncerramento.Matricula);

            context.ApplyModifications(contextQuery);
        }
    }
}

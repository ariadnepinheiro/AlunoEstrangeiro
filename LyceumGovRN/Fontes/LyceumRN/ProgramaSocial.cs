using System.Data;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN
{
    public class ProgramaSocial: RNBase
    {
        public static DataTable Listar(string aluno)
        {
            var contextQuery = new ContextQuery
                        (@" SELECT  ID_PROGRAMA_SOCIAL, ALUNO, PROGRAMA, ELEGIVEL, BENEFICIARIO,
                                INICIO_VIGENCIA, FIM_VIGENCIA, MATRICULA, DT_CADASTRO
                        FROM    TCE_PROGRAMA_SOCIAL
                        WHERE   ALUNO = @ALUNO "
                            );
            contextQuery.Parameters.Add("@ALUNO", aluno);

            return Consultar(contextQuery);
        }
    }
}

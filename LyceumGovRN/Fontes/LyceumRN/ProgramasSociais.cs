using System.Data;
using System.Globalization;

using Techne.Data;
using Techne.Library;
using Techne.Lyceum.CR;

namespace Techne.Lyceum.RN
{
    public class ProgramasSociais : RNBase
    {
        public static bool ExisteCodigoAgenciaPrograma(string codigoAgencia, string codigoPrograma)
        {
            string sql = "select top 1 1 from ly_agencia_programa where agencia = ? and programa =?";
            int retorno = ExecutarFuncao(sql, codigoAgencia, codigoPrograma);
            if (retorno == 1)
                return true;
            return false;
        }

        public static bool ExisteProgramaUnidade(string codigoAgencia, string codigoPrograma)
        {
            string sql = "select top 1 1 from LY_UNIDADE_ENSINO_PROGRAMAS where agencia = ? and programa = ?";
            int retorno = ExecutarFuncao(sql, codigoAgencia, codigoPrograma);
            if (retorno == 1)
                return true;
            return false;
        }
    }
}

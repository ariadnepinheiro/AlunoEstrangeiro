using System.Data;
using System.Globalization;

using Techne.Data;
using Techne.Library;
using Techne.Lyceum.CR;

namespace Techne.Lyceum.RN
{
	public class TipoBeneficio : RNBase
	{
		public static bool ExisteCodigo(string codigo)
		{
			string sql = "select top 1 1 from ly_tipo_beneficio where tipo_beneficio = ?";
			int retorno = ExecutarFuncao(sql, codigo);
			if (retorno == 1)
				return true;
			return false;
		}

        public static bool ExisteProgramaBeneficio(string codigo)
        {
            string sql = "select top 1 1 from LY_UNIDADE_ENSINO_PROGRAMAS where tipo_beneficio = ?";
            int retorno = ExecutarFuncao(sql, codigo);
            if (retorno == 1)
                return true;
            return false;
        }
	}
}

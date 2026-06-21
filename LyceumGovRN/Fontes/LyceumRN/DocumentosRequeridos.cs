using System.Data;
using System.Globalization;

using Techne.Data;
using Techne.Library;
using Techne.Lyceum.CR;

namespace Techne.Lyceum.RN
{
	public class DocumentosRequeridos : RNBase
	{
		public static bool ExisteDescricao(string descricao, string doc)
		{
			string sql = "select top 1 1 from ly_documentos_ingresso where nome = ? and doc <> ?";
			int retorno = ExecutarFuncao(sql, descricao, doc);
			if (retorno == 1)
				return true;
			return false;
		}

		public static string GeraNumeroDocumento()
		{
			return ExecutarFuncao("select isnull(max(convert(int, doc)),0) + 1 from ly_documentos_ingresso").ToString();
		}
	}
}

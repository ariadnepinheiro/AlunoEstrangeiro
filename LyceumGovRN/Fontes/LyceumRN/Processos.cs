using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Text;
using Techne.Controls;
using Techne.Lyceum.RN;
using Techne.Data;
using Techne.Web;
using Techne.Lyceum.CR;
using Techne.Library.Sql.Structure;

namespace Techne.Lyceum.RN
{
	public class Processos
	{
		/// <summary>
		/// Verifica se existe Processo cadastrado no sistema a partir de seu identificador.
		/// </summary>
		/// <param name="processoId">Identificador do Processo</param>
		public static bool ExisteGrupoProcessos(string processoId)
		{
			if (string.IsNullOrEmpty(processoId))
				return false;
			TConnection connection = Techne.HadesLyc.Config.CreateConnection();
			connection.Open();
			QueryTable qt = null;
			try
			{
				string sql = "SELECT 1 FROM hd_processo WHERE processo = ? ";
				qt = new QueryTable(sql);
				qt.Query(connection, processoId);
			}
			finally
			{
				connection.Close();
			}
			return qt.Rows.Count > 0;
		}

		/// <summary>
		/// Verifica se existem Parâmetros cadastrados no Processo a partir de seu identificador.
		/// </summary>
		/// <param name="processoId">Identificador do Processo</param>
		public static bool ExisteParametro(string parametroId)
		{
			if (!string.IsNullOrEmpty(parametroId))
				return false;
			TConnection connection = Techne.HadesLyc.Config.CreateConnection();
			connection.Open();
			QueryTable qt = null;
			try
			{
				string sql = "SELECT 1 FROM hd_parametro_processo WHERE processo = ? ";
				qt = new QueryTable(sql);
				qt.Query(connection, parametroId);
			}
			finally
			{
				connection.Close();
			}
			return qt.Rows.Count > 0;
		}
	}
}

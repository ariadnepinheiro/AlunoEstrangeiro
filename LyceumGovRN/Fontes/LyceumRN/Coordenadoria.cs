using System;
using System.Collections.Generic;
using Techne.Data;
using Techne.Lyceum.CR;

namespace Techne.Lyceum.RN
{
	public class Coordenadoria : RNBase
	{
		// Consultar
		public static Ly_nucleo.Row Consultar(string unidade)
		{
			var connection = Config.CreateConnection();

			connection.Open();

			try
			{
				var parametros = new object[] { unidade };
				var consulta = Ly_nucleo.QueryFirstRow(connection, "nucleo = ?", parametros);

				return consulta;
			}
			finally
			{
				connection.Close();
			}
		}

        public static bool IsUsuarioNucleoOuUnidade(string setor)
        {
            int retorno;
            string sql = string.Empty;
            sql = @"select 1 from LY_UNIDADE_ENSINO where SETOR = ?
                    union
                    select 1 from LY_nucleo where SETOR = ?";
            retorno = ExecutarFuncao(sql, setor, setor);

            if (retorno == 1)
                return true;//pertence ao nucleo ou a unidade
            else
                return false; //não pertence a nenhum dos dois
        }

		/// <summary>
		/// Obtém a descrição de uma Coordenadoria a partir de seu identificador.
		/// </summary>
		/// <param name="connection">Conexão</param>
		/// <param name="idCoordenadoria">Identificador da Coordenadoria</param>
		public static string ObtemDescricaoCoordenadoria(TConnection connection, string idCoordenadoria)
		{
			var descricaoCoordenadoria = Convert.ToString(TCommand.ExecuteScalar(connection, "SELECT descricao FROM ly_nucleo WHERE nucleo = " + idCoordenadoria));

			return descricaoCoordenadoria;
		}

		//Excluir
		public static RetValue Excluir(string unidade)
		{
			var connection = Config.CreateWritableConnection();
			RetValue retorno = null;

			connection.Open(true);

			try
			{
				var parametros = new object[] { unidade };
				var dtCurso = Ly_nucleo.Query(connection, "nucleo = ?", parametros);

				if (dtCurso != null)
				{
					if (dtCurso.Rows != null)
					{
						foreach (Ly_nucleo.Row linha in dtCurso.Rows)
						{
							linha.Delete();
						}

						dtCurso.Put(connection);
						retorno = VerificarErro(dtCurso);

						if (retorno != null && !retorno.Ok)
						{
							connection.Rollback();
							return retorno;
						}

						retorno = new RetValue(true, "Coordenadoria removida com sucesso.", null);
					}
				}
			}
			finally
			{
				connection.Close();
			}

			return retorno;
		}

		//Incluir
		public static RetValue Incluir(Ly_nucleo dtUnidade)
		{
			TConnectionWritable connection = Config.CreateWritableConnection();
			connection.Open(true);

			RetValue retorno = null;

			try
			{
				if (dtUnidade != null)
				{
					if (dtUnidade.Rows != null)
					{
						dtUnidade.Put(connection);

						retorno = VerificarErro(dtUnidade);

						if (retorno != null && !retorno.Ok)
						{
							connection.Rollback();
							return retorno;
						}

						retorno = new RetValue(true, "Coordenadoria incluída com sucesso.", null);
					}
				}
			}
			finally
			{
				connection.Close();
			}

			return retorno;
		}

		//Alterar
		public static RetValue Alterar(Ly_nucleo dtNucleo)
		{
			RetValue retorno = null;

			TConnectionWritable connection = Config.CreateWritableConnection();
			connection.Open(true);

			try
			{
				if (dtNucleo != null)
				{
					if (dtNucleo.Rows != null)
					{
						var colunas = " descricao, end_compl, end_num, endereco, cep, " +
									  " bairro, municipio, setor, pessoa, fone, e_mail, classificacao";

						var listaObjeto = new List<object>();

						listaObjeto.Add(dtNucleo.Rows[0].Descricao);
						listaObjeto.Add(dtNucleo.Rows[0].End_compl);
						listaObjeto.Add(dtNucleo.Rows[0].End_num);
						listaObjeto.Add(dtNucleo.Rows[0].Endereco);

						listaObjeto.Add(dtNucleo.Rows[0].Cep);
						listaObjeto.Add(dtNucleo.Rows[0].Bairro);
						listaObjeto.Add(dtNucleo.Rows[0].Municipio);
						listaObjeto.Add(dtNucleo.Rows[0].Setor);
						listaObjeto.Add(dtNucleo.Rows[0].Pessoa);

						listaObjeto.Add(dtNucleo.Rows[0].Fone);
						listaObjeto.Add(dtNucleo.Rows[0].E_mail);
						listaObjeto.Add(dtNucleo.Rows[0].Classificacao);

						// ColunasTable colunas = MontarParametros(dtUnidade);
						Ly_nucleo.Row.Update(connection, dtNucleo.Rows[0].Nucleo, colunas, listaObjeto.ToArray());

						retorno = VerificarErro(connection.GetErrors());

						if (retorno != null && !retorno.Ok)
						{
							connection.Rollback();
							return retorno;
						}

						retorno = new RetValue(true, "Coordenadoria alterada com sucesso.", null);
					}
				}
			}
			finally
			{
				connection.Close();
			}

			return retorno;
		}

		public static QueryTable ConsultarContato(string nucleo)
		{
			var sql = new System.Text.StringBuilder();

			sql.Append(" select L.MATRICULA, P.NOME_COMPL, P.FONE, P.CELULAR, P.E_MAIL, F.descricao from ly_lotacao L ");
			sql.Append(" inner join ly_funcao F ON F.FUNCAO = L.FUNCAO ");
			sql.Append(" INNER JOIN ly_pessoa P ON P.PESSOA = L.PESSOA ");
			sql.Append(" WHERE ");
			sql.Append(" F.CAMPO_03 = 'S' ");
			sql.Append(" AND L.DATA_NOMEACAO <= convert(date,GetDate()) AND (L.DATA_DESATIVACAO is null OR convert(date,L.data_desativacao) > convert(date,GetDate()))");
			sql.Append(" AND L.NUCLEO = ? ");

			return Consultar(sql.ToString(), nucleo);
		}

		internal static void GeraNumeroCoordenadoria(Ly_nucleo.Row row)
		{
			row.Nucleo = ExecutarFuncao("select isnull(max(convert(int, nucleo)),0) + 1 from ly_nucleo").ToString();
		}

		public static string ObterCoordenadoriaUsuario(string usuario)
		{
			var sql = @"SELECT  ISNULL(nucleo, '') + '|' + ISNULL(unidade_ens, '') valores
                        FROM    vw_funcionarios f
                                INNER JOIN USUARIO u
                                ON f.MATRICULA = u.MATRICULA
                        WHERE   usuario = ?
                        AND (DATA_DESATIVACAO IS NULL OR DATA_DESATIVACAO >= CONVERT(DATE,GETDATE()))
                        AND DATA_NOMEACAO < CONVERT(DATE,GETDATE())";
			var qt = new QueryTable(sql);
			var connection = Config.CreateConnection();

			qt.Query(connection, usuario);

			if (qt.Rows.Count > 0)
			{
				return qt.Rows[0]["valores"].ToString();
			}

			return " | ";
		}

		public static String ObterCoordenadoria(string nucleo)
		{
			var retorno = string.Empty;
			var connection = Config.CreateConnection();

			try
			{
				connection.Open();

				var sql = @" 
                    SELECT * 
                    FROM ly_nucleo
                    WHERE nucleo = ?";

				var row = SimpleRow.QueryFirstRow(connection, sql, nucleo);

				if (!row.IsNull("DESCRICAO"))
				{
					retorno = Convert.ToString(row["DESCRICAO"]);
				}
			}
			finally
			{
				connection.Close();
			}

			return retorno;
		}

		public static String ObterCoordenadoriaMunic(string municipio, string nucleo)
		{
			var retorno = string.Empty;
			var connection = Config.CreateConnection();

			try
			{
				connection.Open();

				var sql = @" 
                    SELECT DISTINCT NUCLEO
                    FROM lY_unidade_ensino
                    WHERE MUNICIPIO = ? AND NUCLEO = ?"; // is not null";

				var row = SimpleRow.QueryFirstRow(connection, sql, municipio, nucleo);

				if (row != null)
				{
					if (!row.IsNull("NUCLEO"))
					{
						retorno = Convert.ToString(row["NUCLEO"]);
					}
				}
			}
			finally
			{
				connection.Close();
			}

			return retorno;
		}

		public static String ObterCoordenadoriaMunic(string municipio)
		{
			var retorno = string.Empty;
			var connection = Config.CreateConnection();

			try
			{
				connection.Open();

				var sql = @" 
                    SELECT DISTINCT NUCLEO
                    FROM lY_unidade_ensino
                    WHERE MUNICIPIO = ? AND NUCLEO is not null";

				var row = SimpleRow.QueryFirstRow(connection, sql, municipio);

				if (!row.IsNull("NUCLEO"))
				{
					retorno = Convert.ToString(row["NUCLEO"]);
				}
			}
			finally
			{
				connection.Close();
			}

			return retorno;
		}
	}
}

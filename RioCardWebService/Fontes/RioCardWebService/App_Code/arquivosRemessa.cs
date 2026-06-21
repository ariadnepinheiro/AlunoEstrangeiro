using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Services;

[WebService(Namespace = "http://200.222.27.185/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class arquivosRemessa : WebService
{
    [WebMethod(Description = "arquivos_Remessa")]
    public arquivosRemessaResponse arquivos_Remessa()
    {
        var arquivo = new arquivosRemessaResponse();

        var conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["conexaoSQL"].ConnectionString);

        var sql = "SELECT arquivo, quantidaderegistros, data FROM RIOCARD_ARQUIVOREMESSA WHERE arquivo like 'SEEDUC%'";

        var sql1 = "SELECT count(*) as quantidadeArquivos FROM RIOCARD_ARQUIVOREMESSA WHERE arquivo like 'SEEDUC%'";

        try
        {
            conexao.Open();

            var command1 = new SqlCommand(sql1, conexao);
            var reader1 = command1.ExecuteReader();

            while (reader1.Read())
            {
                if (reader1["quantidadeArquivos"] != null && !(reader1["quantidadeArquivos"] is DBNull))
                {
                    arquivo.quantidadeArquivos = Convert.ToInt32(reader1["quantidadeArquivos"]);
                }
            }

            reader1.Close();

            var command = new SqlCommand(sql, conexao);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                if (reader["arquivo"] != null && !(reader["arquivo"] is DBNull))
                {
                    if (arquivo.arquivos == null)
                    {
                        arquivo.arquivos = new List<arquivosRemessaResponse.arquivo>();
                    }

                    var arquivoAux = new arquivosRemessaResponse.arquivo();

                    if (reader["quantidaderegistros"] != null && !(reader["quantidadeRegistros"] is DBNull))
                    {
                        arquivoAux.quantidadeRegistros = Convert.ToInt32(reader["quantidadeRegistros"]);
                    }

                    arquivoAux.nome = Convert.ToString(reader["arquivo"]);

                    if (reader["data"] != null && !(reader["data"] is DBNull))
                    {
                        var dataArquivo = DateTime.MinValue;

                        if (DateTime.TryParse(Convert.ToString(reader["data"]), out dataArquivo))
                        {
                            arquivoAux.data = dataArquivo;
                        }
                    }

                    // arquivoAux.quantidadeRegistros = Convert.ToInt32(reader["quantidaderegistros"]); //TODO: ver na consulta onde obter esse valor
                    arquivo.arquivos.Add(arquivoAux);
                }
            }

            reader.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (conexao.State == System.Data.ConnectionState.Open)
            {
                conexao.Close();
            }
        }

        return arquivo;
    }
}
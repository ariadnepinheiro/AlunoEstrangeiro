using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for Remessa
/// </summary>
[WebService(Namespace = "http://200.222.27.185/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class arquivosRemessa : System.Web.Services.WebService
{

    public arquivosRemessa()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod(Description = "arquivos_Remessa")]
    public arquivosRemessaResponse arquivos_Remessa()
    {
        arquivosRemessaResponse arquivo = new arquivosRemessaResponse();

        SqlConnection conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["conexaoSQL"].ConnectionString);

        string sql = "SELECT arquivo, quantidaderegistros, data FROM RIOCARD_ARQUIVOREMESSA WHERE arquivo like 'SLNSEE%'";

        string sql1 = "SELECT count(*) as quantidadeArquivos FROM RIOCARD_ARQUIVOREMESSA WHERE arquivo like 'SLNSEE%'";

        try
        {
            conexao.Open();


            SqlCommand command1 = new SqlCommand(sql1, conexao);
            SqlDataReader reader1 = command1.ExecuteReader();

            while (reader1.Read())
            {
                if (reader1["quantidadeArquivos"] != null && !(reader1["quantidadeArquivos"] is DBNull))
                    arquivo.quantidadeArquivos = Convert.ToInt32(reader1["quantidadeArquivos"]);

            }

            reader1.Close();


            SqlCommand command = new SqlCommand(sql, conexao);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {

                if (reader["arquivo"] != null && !(reader["arquivo"] is DBNull))
                {
                    if (arquivo.arquivos == null)
                        arquivo.arquivos = new List<arquivosRemessaResponse.arquivo>();

                    arquivosRemessaResponse.arquivo arquivoAux = new arquivosRemessaResponse.arquivo();

                    if (reader["quantidaderegistros"] != null && !(reader["quantidadeRegistros"] is DBNull))
                        arquivoAux.quantidadeRegistros = Convert.ToInt32(reader["quantidadeRegistros"]);

                    arquivoAux.nome = Convert.ToString(reader["arquivo"]);

                    if (reader["data"] != null && !(reader["data"] is DBNull))
                    {
                        DateTime dataArquivo = DateTime.MinValue;

                        if (DateTime.TryParse(Convert.ToString(reader["data"]), out dataArquivo))
                            arquivoAux.data = dataArquivo;
                    }

                    //         arquivoAux.quantidadeRegistros = Convert.ToInt32(reader["quantidaderegistros"]); //TODO: ver na consulta onde obter esse valor

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
                conexao.Close();
        }

        return arquivo;
    }

}


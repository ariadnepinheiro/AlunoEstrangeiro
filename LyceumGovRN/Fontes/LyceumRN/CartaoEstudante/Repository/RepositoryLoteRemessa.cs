using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using Techne.Lyceum.RN.CartaoEstudante.Entities;
using Techne.Lyceum.RN.CartaoEstudante.Util;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.CartaoEstudante.Repository
{
    public class RepositoryLoteRemessa : Conexao
    {


        /// <summary>
        /// Método que a lista dos lotes de remessa
        /// </summary>
        /// <returns></returns>
        public List<LoteRemessa> ListaLoteremessa(int operadoraID)
        {
            List<LoteRemessa> lstLoteRemessa = new List<LoteRemessa>();

            ///conexão para persiti o log
            var conn2 = new SqlConnection(System.Text.RegularExpressions.Regex.Replace(Techne.Data.ConnectionList.GetConnectionString("Lyceum"), "Provider=SQLOLEDB.1;", string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase));

            try
            {
                conn2.Open();

                var cmd = new SqlCommand(ListaLoteremessaQuery(operadoraID), openConn());
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if (reader["nomeloteremessa"] != null && !(reader["nomeloteremessa"] is DBNull))
                    {
                        if (lstLoteRemessa == null)
                        {
                            lstLoteRemessa = new List<LoteRemessa>();
                        }

                        var arquivoAux = new LoteRemessa();

                        if (reader["quantidaderegistros"] != null && !(reader["quantidadeRegistros"] is DBNull))
                        {
                            arquivoAux.quantidadeRegistros = Convert.ToInt32(reader["quantidadeRegistros"]);
                        }

                        arquivoAux.nome = Convert.ToString(reader["nomeloteremessa"]);
                        arquivoAux.codOperadora = operadoraID;

                        ///Metodo que persite o log
                        LogLoteremessa(Convert.ToString(reader["loteremessaid"]), conn2);


                        if (reader["datageracao"] != null && !(reader["datageracao"] is DBNull))
                        {
                            var dataArquivo = DateTime.MinValue;

                            if (DateTime.TryParse(Convert.ToString(reader["datageracao"]), out dataArquivo))
                            {
                                arquivoAux.data = dataArquivo;
                            }
                        }

                        lstLoteRemessa.Add(arquivoAux);
                    }
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Log.Adicionar(ex.ToString().Replace("\n", ". ")); 
                throw ex;
            }
            finally
            {
                     closeConn();
                     conn2.Close();  
            }

            return lstLoteRemessa;
        }

        /// <summary>
        /// Método que retorna o total dos lotes de remessa 
        /// </summary>
        /// <returns></returns>
        public int TotalLoteremessa(int operadoraID)
        {
            Int32 totalArquivos = 0;

            try
            {
                var cmd = new SqlCommand(TotalLoteremessaQuery(operadoraID), openConn());
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if (reader["quantidadeArquivos"] != null && !(reader["quantidadeArquivos"] is DBNull))
                    {
                        totalArquivos = Convert.ToInt32(reader["quantidadeArquivos"]);
                    }
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Log.Adicionar(ex.ToString().Replace("\n", ". ")); 
                throw ex;
            }
            finally
            {
                
                    closeConn();
            }


            return totalArquivos;
        }

       /// <summary>
       /// Mátodo que busca o id do lote 
       /// </summary>
       /// <param name="nomeArquivo">nome do arquivo do lote desejado</param>
       /// <returns></returns>
        public int BuscaPorID(string nomeArquivo)
        {
            Int32 idLoteRemessa = 0;

            try
            {
                var cmd = new SqlCommand(IDLoteremessaQuery(nomeArquivo), openConn());
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if (reader["loteremessaid"] != null && !(reader["loteremessaid"] is DBNull))
                    {
                        idLoteRemessa = Convert.ToInt32(reader["loteremessaid"]);
                    }
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Log.Adicionar(ex.ToString().Replace("\n", ". ")); 
                throw ex;
            }
            finally
            {

                closeConn();
            }


            return idLoteRemessa;
        }

        /// <summary>
        /// Grava os logs dos Lotes de remessa
        /// </summary>
        /// <returns></returns>
        public void LogLoteremessa(string loteremessaid, SqlConnection conn2)
        {
             try
            {
                var cmd = new SqlCommand(LogLoteremessaQuery(loteremessaid), conn2);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Log.Adicionar(ex.ToString().Replace("\n", ". ")); 
                throw ex;
            }
        }

        /// <summary>
        /// Query com o total dos lotes de remessas
        /// </summary>
        /// <returns></returns>
        public string TotalLoteremessaQuery(int operadoraID)
        {
            return @"SELECT count(*) as quantidadeArquivos 
                    FROM CartaoEstudante.LOTEREMESSA 
                    WHERE  SITUACAOPROCESSAMENTO = 'N'
                        AND DATEDIFF(M,DATAGERACAO,GETDATE()) <= 3		
                        AND OPERADORAID = " + operadoraID;
        }


         /// <summary>
        /// Query com o ID do lote remessa
        /// </summary>
        /// <returns></returns>
        public string IDLoteremessaQuery(string nomeArquivo)
        {
            return "select loteremessaid from CartaoEstudante.LOTEREMESSA where nomeloteremessa = '" + nomeArquivo + "'";
        }

        /// <summary>
        /// Query Com a lista dos Lotes de remessa
        /// </summary>
        /// <returns></returns>
        public string ListaLoteremessaQuery(int operadoraID)
        {
            return @" SELECT loteremessaid, 
                            nomeloteremessa, 
                            quantidaderegistros, 
                            datageracao 
                    FROM CartaoEstudante.LOTEREMESSA 
                    WHERE SITUACAOPROCESSAMENTO = 'N' 
                            AND DATEDIFF(M,DATAGERACAO,GETDATE()) <= 3		
                            AND OPERADORAID = " + operadoraID;
        }

        /// <summary>
        /// Query que grava os logs dos Lotes de remessa
        /// </summary>
        /// <returns></returns>
        public string LogLoteremessaQuery(string loteremessaid)
        {
            return "INSERT INTO CartaoEstudante.LOGLOTEREMESSA (LOTEREMESSAID, DATAENVIO) VALUES(" + loteremessaid + ", GETDATE())";
        }
    }
}

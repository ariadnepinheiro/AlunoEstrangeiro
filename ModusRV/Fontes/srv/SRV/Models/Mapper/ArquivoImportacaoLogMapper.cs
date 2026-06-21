using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;

namespace SRV.Models.Mapper
{
    public class ArquivoImportacaoLogMapper : BaseMapper<string>
    {

        protected override string QueryFindObject()
        {
            throw new NotImplementedException();
        }

        protected override string QueryListObjects()
        {
            return @"SELECT * 
                       FROM rv_arquivo_importacao_log
                      WHERE id_arquivo_importacao = @idArquivoImportacao";
        }

        private string QueryInsert()
        {
            return @"INSERT INTO rv_arquivo_importacao_log
                                 (id_arquivo_importacao,
                                  des_log)
                          VALUES (@idArquivoImportacao,
                                  @desLog)";
        }

        private string QueryDelete()
        {
            return @"DELETE FROM rv_arquivo_importacao_log
                           WHERE id_arquivo_importacao = @idArquivoImportacao";
        }

        public override string LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            return (string)reader["des_log"];
        }

        public IList<string> List(int idArquivoImportacao)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("idArquivoImportacao", idArquivoImportacao);

            return ListObjects(param);
        }

        public void Insert(int idArquivoImportacao, string log)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("idArquivoImportacao", idArquivoImportacao);
            param.Add("desLog", log);

            InsertObject(QueryInsert(), param);
        }

        public void Delete(int idArquivoImportacao)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("idArquivoImportacao", idArquivoImportacao);

            DeleteObject(QueryDelete(), param);
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using SRV.Models.Mapper;

namespace SRV.Models.Service
{
    public class ArquivoImportacaoLogService : BaseService
    {

        public IList<string> List(int idArquivoImportacao)
        {
            IList<string> result = null;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                ArquivoImportacaoLogMapper mapper = new ArquivoImportacaoLogMapper();
                mapper.connection = conn;

                result = mapper.List(idArquivoImportacao);
            }

            return result;
        }


    }
}
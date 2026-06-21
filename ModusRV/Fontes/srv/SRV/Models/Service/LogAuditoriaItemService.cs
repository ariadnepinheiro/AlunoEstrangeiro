using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using System.Data.SqlClient;
using SRV.Models.Mapper;

namespace SRV.Models.Service
{
    public class LogAuditoriaItemService : BaseService
    {
        public IList<LogAuditoriaItem> List(int idLogAuditoria)
        {
            IList<LogAuditoriaItem> itens;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                LogAuditoriaItemMapper mapper = new LogAuditoriaItemMapper();
                mapper.connection = conn;

                itens = mapper.List(idLogAuditoria);
            }

            return itens;
        }
    }
}
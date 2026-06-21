using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using SRV.Models.DTO;
using System.Data.SqlClient;
using SRV.Models.Mapper;

namespace SRV.Models.Service
{
    public class LogAuditoriaService : BaseService
    {
        public Paging<LogAuditoria> List(FiltroLogAuditoria filtro, int currentPage, int pageSize)
        {
            Paging<LogAuditoria> logs;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                LogAuditoriaMapper mapper = new LogAuditoriaMapper();
                mapper.connection = conn;

                logs = mapper.List(filtro, currentPage, pageSize);
            }

            return logs;
        }

		public List<LogAuditoria> ListRecurso(FiltroLogAuditoria filtro)
		{
			List<LogAuditoria> logs;

			using (SqlConnection conn = GetConnection())
			{
				conn.Open();
				LogAuditoriaMapper mapper = new LogAuditoriaMapper();
				mapper.connection = conn;

				logs = mapper.ListRecurso(filtro).ToList();
			}

			return logs;
		}
    }
}
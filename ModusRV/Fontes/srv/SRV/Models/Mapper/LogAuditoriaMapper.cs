using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using SRV.Models.DTO;
using System.Text;

namespace SRV.Models.Mapper
{
    public class LogAuditoriaMapper : BaseMapper<LogAuditoria>
    {
        protected override string QueryFindObject()
        {
            throw new NotImplementedException();
        }

        protected override string QueryListObjects()
        {
            throw new NotImplementedException();
        }

        private string QueryList(FiltroLogAuditoria filtro)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"SELECT l.id_log_auditoria, l.dt_log, u.des_nome_usuario,
                                l.tipo_operacao, l.des_objeto, l.ip_cliente
                           FROM rv_log_auditoria l,
                                rv_usuario u
                          WHERE l.id_usuario = u.id_usuario");

            if (filtro.TipoOperacao != null)
                sql.Append(" AND l.tipo_operacao = @tipoOperacao");

            if (filtro.DataIni.HasValue && filtro.DataFin.HasValue)
            {
                sql.Append(" AND l.dt_log BETWEEN @dataIni AND @dataFin");
            }

            if (filtro.ObjetoAuditoria != null)
                sql.Append(" AND l.des_objeto LIKE @objeto");

            if (filtro.IdUsuario != null)
                sql.Append(" AND u.id_usuario = @idUsuario");

            sql.Append("  ORDER BY l.dt_log, u.des_nome_usuario, l.tipo_operacao, l.des_objeto");

            return sql.ToString();
        }

        private string QueryInsert()
        {
            return @"INSERT INTO rv_log_auditoria
                                (id_usuario,
                                 dt_log,
                                 tipo_operacao,
                                 des_objeto,
                                 ip_cliente)
                          VALUES (@idUsuario,
                                  GETDATE(),
                                  @tipoOperacao,
                                  @desObjeto,
                                  @ipCliente)";
        }

        public override LogAuditoria LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            LogAuditoria logAuditoria = new LogAuditoria();

            logAuditoria.IdLogAuditoria = (int)reader["id_log_auditoria"];
            logAuditoria.DataLog = (DateTime)reader["dt_log"];
            
            logAuditoria.Usuario = new Usuario();
            logAuditoria.Usuario.Nome = (string)reader["des_nome_usuario"];

            logAuditoria.TipoOperacao = (OperacaoAuditoria)Enum.ToObject(typeof(OperacaoAuditoria), Convert.ToInt32(reader["tipo_operacao"]));

            logAuditoria.DesObjeto = (string)reader["des_objeto"];

            if (reader["ip_cliente"] != DBNull.Value)
                logAuditoria.IpCliente = (string)reader["ip_cliente"];

            return logAuditoria;
        }

        public Paging<LogAuditoria> List(FiltroLogAuditoria filtro, int currentPage, int pageSize)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            if (filtro.TipoOperacao != null)
                param.Add("tipoOperacao", filtro.TipoOperacao.Value);

            if (filtro.DataIni.HasValue || filtro.DataFin.HasValue)
            {
                if (!filtro.DataIni.HasValue)
                    filtro.DataIni = filtro.DataFin;
                else if (!filtro.DataFin.HasValue)
                    filtro.DataFin = filtro.DataIni;

                param.Add("dataIni", filtro.DataIni.Value.AddHours(filtro.HoraIni.Hour).AddMinutes(filtro.HoraIni.Minute));
                param.Add("dataFin", filtro.DataFin.Value.AddHours(filtro.HoraFin.Hour).AddMinutes(filtro.HoraFin.Minute));
            }

            if (filtro.ObjetoAuditoria != null)
                param.Add("objeto", filtro.ObjetoAuditoria.Value.ToString().ToUpper());

            if (filtro.IdUsuario != null)
                param.Add("idUsuario", filtro.IdUsuario);

            return ListPagingObjects(QueryList(filtro), param, LoadObject, currentPage, pageSize);
        }

        public int? Insert(LogAuditoria logAuditoria)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idUsuario", logAuditoria.Usuario.Id);
            param.Add("tipoOperacao", (int)logAuditoria.TipoOperacao);
            param.Add("desObjeto", StringToUpper(logAuditoria.DesObjeto));
            param.Add("ipCliente", logAuditoria.IpCliente);

            return InsertObjectWithIdentity(QueryInsert(), param);
        }

		public IList<LogAuditoria> ListRecurso(FiltroLogAuditoria filtro)
		{
			Dictionary<string, object> param = new Dictionary<string, object>();

			if (filtro.TipoOperacao != null)
				param.Add("tipoOperacao", filtro.TipoOperacao.Value);

			if (filtro.DataIni.HasValue || filtro.DataFin.HasValue)
			{
				if (!filtro.DataIni.HasValue)
					filtro.DataIni = filtro.DataFin;
				else if (!filtro.DataFin.HasValue)
					filtro.DataFin = filtro.DataIni;

				param.Add("dataIni", filtro.DataIni.Value.AddHours(filtro.HoraIni.Hour).AddMinutes(filtro.HoraIni.Minute));
				param.Add("dataFin", filtro.DataFin.Value.AddHours(filtro.HoraFin.Hour).AddMinutes(filtro.HoraFin.Minute));
			}

			if (filtro.ObjetoAuditoria != null)
				param.Add("objeto", filtro.ObjetoAuditoria.Value.ToString().ToUpper());

			param.Add("idUsuario", filtro.IdUsuario);

			return ListObjects(QueryListRecurso(filtro), param);
		}

		private string QueryListRecurso(FiltroLogAuditoria filtro)
		{
			StringBuilder sql = new StringBuilder();

			sql.Append(@"SELECT DISTINCT AI.id_log_auditoria, AI.dt_log, u.des_nome_usuario,
                                AI.tipo_operacao, AI.des_objeto, AI.ip_cliente FROM RV_LOG_AUDITORIA AI
                                INNER JOIN 
                                (SELECT DISTINCT ID_LOG_AUDITORIA FROM RV_LOG_AUDITORIA_ITEM AI
                                WHERE AI.DES_ATRIBUTO = 'SERVIDOR' AND (AI.VLR_ATUAL = @idUsuario OR AI.VLR_ANTERIOR = @idUsuario)) LOG_AUDITORIA
                                ON AI.ID_LOG_AUDITORIA = LOG_AUDITORIA.ID_LOG_AUDITORIA
                                INNER JOIN RV_USUARIO U ON U.ID_USUARIO = AI.ID_USUARIO
                                INNER JOIN RV_LOG_AUDITORIA_ITEM A ON A.ID_LOG_AUDITORIA = AI.ID_LOG_AUDITORIA AND A.DES_ATRIBUTO = 'RECURSO'
                                WHERE 1=1");

			if (filtro.TipoOperacao != null)
				sql.Append(" AND AI.tipo_operacao = @tipoOperacao");

			if (filtro.DataIni.HasValue && filtro.DataFin.HasValue)
			{
				sql.Append(" AND AI.dt_log BETWEEN @dataIni AND @dataFin");
			}

			if (filtro.ObjetoAuditoria != null)
				sql.Append(" AND AI.des_objeto LIKE @objeto");

			sql.Append("  ORDER BY AI.DT_LOG desc");


			return sql.ToString();
		}

    }
}
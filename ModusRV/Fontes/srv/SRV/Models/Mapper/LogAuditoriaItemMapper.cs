using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;

namespace SRV.Models.Mapper
{
    public class LogAuditoriaItemMapper : BaseMapper<LogAuditoriaItem>
    {
        protected override string QueryFindObject()
        {
            throw new NotImplementedException();
        }

        protected override string QueryListObjects()
        {
            return @"SELECT id_log_auditoria, des_atributo,
                            vlr_anterior, vlr_atual
                       FROM rv_log_auditoria_item
                      WHERE id_log_auditoria = @idLogAuditoria";
        }

        private string QueryInsert()
        {
            return @"INSERT INTO rv_log_auditoria_item
                                (id_log_auditoria,
                                 des_atributo,
                                 vlr_anterior,
                                 vlr_atual)
                          VALUES (@idLogAuditoria,
                                  @desAtributo,
                                  @vlrAnterior,
                                  @vlrAtual)";
        }

        public override LogAuditoriaItem LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            LogAuditoriaItem item = new LogAuditoriaItem();

            item.IdLogAuditoria = (int)reader["id_log_auditoria"];
            item.DesAtributo = (string)reader["des_atributo"];

            if(reader["vlr_anterior"] != DBNull.Value)
                item.VlrAnterior = (string)reader["vlr_anterior"];

            if (reader["vlr_atual"] != DBNull.Value)
                item.VlrAtual = (string)reader["vlr_atual"];

            return item;
        }

        public IList<LogAuditoriaItem> List(int idLogAuditoria)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idLogAuditoria", idLogAuditoria);

            return ListObjects(QueryListObjects(), param);
        }

        public void Insert(LogAuditoriaItem logAuditoriaItem, int idLogAuditoria)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idLogAuditoria", idLogAuditoria);
            param.Add("desAtributo", StringToUpper(logAuditoriaItem.DesAtributo));
            param.Add("vlrAnterior", StringToUpper(logAuditoriaItem.VlrAnterior));
            param.Add("vlrAtual", StringToUpper(logAuditoriaItem.VlrAtual));

            InsertObject(QueryInsert(), param);
        }

    }
}
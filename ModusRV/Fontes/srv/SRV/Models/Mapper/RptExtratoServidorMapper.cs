using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.DTO;

namespace SRV.Models.Mapper
{
    public class RptExtratoServidorMapper : BaseMapper<RptExtratoServidor>
    {
        protected override string QueryFindObject()
        {
            return @" SELECT s.id_servidor,
                             s.des_nome_servidor, s.fl_elegivel,
                             dbo.fc_grid_formatar_cpf(cast(s.des_cpf_servidor as varchar)) as cpf,
                             (SELECT DISTINCT id_servidor FROM rv_motivo_ineleg_docente WHERE id_servidor = s.id_servidor and id_ano_referencia = @anoReferencia) as servidor_ineleg
                        FROM rv_servidor s LEFT JOIN rv_cargo_servidor cs 
                                                        ON cs.id_servidor = s.id_servidor 
                                           LEFT JOIN rv_cargo c 
                                                        ON c.id_cargo = cs.id_cargo
                       WHERE s.id_servidor = @idServidor";
        }

        protected override string QueryListObjects()
        {
            throw new NotImplementedException();
        }

        public override RptExtratoServidor LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            RptExtratoServidor rptExtratoServidor = new RptExtratoServidor();

            rptExtratoServidor.MatriculaServidor = Convert.ToInt32(reader["id_servidor"]);
            rptExtratoServidor.ElegivelUnidade = reader["fl_elegivel"].ToString() == "S" ? true : false;
            rptExtratoServidor.Nome = (string)reader["des_nome_servidor"];
            rptExtratoServidor.CPF = (string)reader["cpf"];
            rptExtratoServidor.ElegivelServidor = reader["servidor_ineleg"] == DBNull.Value;

            return rptExtratoServidor;
        }

        public RptExtratoServidor Find(int idServidor, int anoReferencia)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>();

            parametros.Add("idServidor", idServidor);
            parametros.Add("anoReferencia", anoReferencia);

            return FindObject(parametros);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.DTO;
using System.Text;
using SRV.Common;
using SRV.Models.Domain;

namespace SRV.Models.Mapper
{
    public class UnidadeConsolidadoMapper : BaseMapper<UnidadeConsolidado>
    {
        protected override string QueryFindObject()
        {
            throw new NotImplementedException();
        }

        protected override string QueryListObjects()
        {
            throw new NotImplementedException();
        }

        private string QueryListUnidades(FiltroRptConsolidado filtro, UserState usuario)
        {
            StringBuilder sql = new StringBuilder();
            
            sql.Append(@"SELECT * 
                           FROM vw_unidade_administrativa
                          WHERE id_ano_referencia = @idAnoReferencia");

            if (filtro.IdUnidadeAdministrativa != null)
                sql.Append("    AND (id_unidade = @idUnidade or id_unidade = @idRegional) ");
            else
            {
                sql.Append("    AND (id_unidade = @idUnidade or id_regional = @idRegional) ");

                if (usuario.Perfil == Perfil.Escola)
                    sql.Append(@" AND id_unidade IN (SELECT id_unidade_administrativa 
                                                       FROM rv_funcao_servidor 
                                                      WHERE id_servidor = @idServidor
                                                      UNION
                                                     SELECT @idRegional)");
            }

            sql.Append("  ORDER BY ordem, des_unidade");

            return sql.ToString();
        }

        public override UnidadeConsolidado LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            UnidadeConsolidado unidade = new UnidadeConsolidado();

            unidade.IdUnidade = Convert.ToInt32(reader["id_unidade"]);
            unidade.DesUnidade = (string)reader["des_unidade"];
            unidade.Elegivel = reader["elegivel"].ToString().Equals("S") ? true : false;

            if (((int)reader["id_tipo_unidadm"]) == Constants.TipoUnidAdmRegional)
                unidade.Regional = true;

            return unidade;
        }

        public IList<UnidadeConsolidado> ListUnidade(FiltroRptConsolidado filtro, UserState usuario)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("idAnoReferencia", filtro.IdAnoReferencia);
            param.Add("idRegional", filtro.IdRegional);

            if (filtro.IdUnidadeAdministrativa != null)
                param.Add("idUnidade", filtro.IdUnidadeAdministrativa);
            else
                param.Add("idUnidade", filtro.IdRegional);

            if (usuario.Perfil == Perfil.Escola)
                param.Add("idServidor", Convert.ToInt32(usuario.Login));

            return ListObjects(QueryListUnidades(filtro, usuario), param);
        }


    }
}
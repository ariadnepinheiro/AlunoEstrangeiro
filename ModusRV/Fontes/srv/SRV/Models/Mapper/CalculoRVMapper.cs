using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.DTO;

namespace SRV.Models.Mapper
{
    public class CalculoRVMapper : BaseMapper<CoeficienteServidor>
    {
        protected override string QueryFindObject()
        {
            throw new NotImplementedException();
        }

        protected override string QueryListObjects()
        {
			return @"SELECT id_ano_referencia, 
							RIGHT(('00000000' + CONVERT(VARCHAR(8), als.id_servidor)),8) id_servidor, 
							REPLACE(CONVERT(VARCHAR(10),SUM(nm_qtd_salario)),'.',',') nm_qtd_salario,
							s.des_id_funcional, s.nm_vinculo
	  				  FROM rv_alocacao_servidor als
						   inner join rv_servidor s on als.ID_SERVIDOR = s.ID_SERVIDOR
					 WHERE nm_qtd_salario > 0 
					   AND id_ano_referencia = @anoReferencia
					 GROUP BY als.id_ano_referencia, als.id_servidor, s.des_id_funcional, s.nm_vinculo
					 ORDER BY als.id_servidor";
        }

        public override CoeficienteServidor LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            CoeficienteServidor coeficienteServidor = new CoeficienteServidor();

            coeficienteServidor.AnoReferencia = (int)reader["id_ano_referencia"];
            coeficienteServidor.IdServidor = (string)reader["id_servidor"];
            coeficienteServidor.Coeficiente = (string)reader["nm_qtd_salario"];
			coeficienteServidor.IdFuncional = Convert.IsDBNull(reader["des_id_funcional"]) ? default(string) : (string)reader["des_id_funcional"];
			coeficienteServidor.Vinculo = Convert.IsDBNull(reader["nm_vinculo"]) ? default(int?) : Convert.ToInt32(reader["nm_vinculo"]);
            return coeficienteServidor;
        }

        public void ExecutarCalculo(int anoReferencia)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            
            param.Add("id_ano_referencia", anoReferencia);

            CallProcedure("sp_calcula_bonificacao", param);
        }

        public void ExecutarSobreposicaoData(int anoReferencia, int tipo)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            
            param.Add("id_ano_referencia", anoReferencia);

            param.Add("tipo", tipo);

            CallProcedure("sp_sobreposicao_data", param);
        }

        public IList<CoeficienteServidor> ListCoeficienteServidor(int anoReferencia)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("anoReferencia", anoReferencia);

            return ListObjects(param);
        }

		public CoeficienteServidor ObterCoeficienteServidor(int idServidor)
		{
			Dictionary<string, object> param = new Dictionary<string, object>();
			param.Add("idServidor", idServidor);

			CoeficienteServidor resultado = FindObject(QueryObterCoeficienteServidor(), param, PreencheCoeficienteServidor);

			return resultado;

		}

		private CoeficienteServidor PreencheCoeficienteServidor(System.Data.SqlClient.SqlDataReader reader)
		{
			CoeficienteServidor coeficienteServidor = new CoeficienteServidor();

			coeficienteServidor.Coeficiente = reader["Coeficiente"].ToString();

			return coeficienteServidor;
		}

		private string QueryObterCoeficienteServidor()
		{
			return @"SELECT REPLACE(CAST(SUM(NM_QTD_SALARIO) as varchar), '.', ',') AS Coeficiente					
						from RV_ALOCACAO_SERVIDOR
						where ID_SERVIDOR = @idServidor
						group by ID_SERVIDOR";
		}
	}
}
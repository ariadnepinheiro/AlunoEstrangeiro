using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;

namespace SRV.Models.Mapper
{
    public class AlocacaoServidorMapper : BaseMapper<AlocacaoServidor>
    {
        protected override string QueryFindObject()
        {
            throw new NotImplementedException();
        }

        private string QueryDeleteAll()
        {
            return @"DELETE FROM rv_alocacao_servidor
                           WHERE id_ano_referencia = @idAnoReferencia";
        }

		private string QueryDeletePorFuncaoServidor()
        {
			return @"DELETE FROM rv_alocacao_servidor
                           WHERE id_funcao_servidor = @idFuncaoServidor";
        }
		

        protected override string QueryListObjects()
        {
            throw new NotImplementedException();
        }

        public override AlocacaoServidor LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(int idAnoReferencia)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idAnoReferencia", idAnoReferencia);

            DeleteObject(QueryDeleteAll(), param);
        }

		public void DeletePor(int idFuncaoServidor)
		{
			Dictionary<string, object> param = new Dictionary<string, object>();

			param.Add("idFuncaoServidor", idFuncaoServidor);

			DeleteObject(QueryDeletePorFuncaoServidor(), param);
		}

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;

namespace SRV.Models.Mapper
{
    public class IndicadorMapper : BaseMapper<Indicador>
    {
        protected override string QueryFindObject()
        {
            return @"SELECT id_indicador, des_indicador, tipo_indicador
                       FROM rv_indicador
                      WHERE id_indicador = @idIndicador";
        }

        protected override string QueryListObjects()
        {
            return @"SELECT id_indicador, des_indicador, tipo_indicador
                       FROM rv_indicador
                      ORDER BY des_indicador";
        }

        private string QueryListByTipo()
        {
            return @"SELECT id_indicador, des_indicador, tipo_indicador
                       FROM rv_indicador
                      WHERE tipo_indicador = @tipoIndicador
                      ORDER BY des_indicador";
        }

        private string QueryInsert()
        {
            return @"INSERT INTO rv_indicador
                               (id_indicador,
                                des_indicador,
                                tipo_indicador)
                        VALUES (@idIndicador,
                                @desIndicador,
                                @tipoIndicador)";
        }

        private string QueryUpdate()
        {
            return @"UPDATE rv_indicador
                        SET des_indicador = @desIndicador,
                            tipo_indicador = @tipoIndicador
                      WHERE id_indicador = @idIndicador";
        }

        private string QueryDelete()
        {
            return @"DELETE FROM rv_indicador
                           WHERE id_indicador = @idIndicador";
        }

        private string QueryValidaInsert()
        {
            return @"SELECT COUNT(*)
                       FROM rv_indicador
                      WHERE id_indicador = @idIndicador";
        }

        private string QueryVerificaRelacionamento()
        {
            return @"SELECT 1
	                   FROM rv_indicador i
                      WHERE id_indicador = @idIndicador
                        AND NOT EXISTS (SELECT id_indicador
		   				                  FROM rv_indicador_unidadm iu
		 	 		                     WHERE iu.id_indicador = i.id_indicador)
                        AND NOT EXISTS (SELECT id_indicador
						                  FROM rv_meta_unidadm mu
					                     WHERE mu.id_indicador = i.id_indicador)
                        AND NOT EXISTS (SELECT id_indicador
						                  FROM rv_parametro_nota pn
					                     WHERE pn.id_indicador = i.id_indicador)
	                    AND NOT EXISTS (SELECT id_indicador
						                  FROM rv_parametro_peso pp
					                     WHERE pp.id_indicador = i.id_indicador)";
        }

        private string QueryExisteIndicador()
        {
            return @"SELECT COUNT(*)
                       FROM rv_indicador
                      WHERE id_indicador = @idIndicador";
        }

        public override Indicador LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            Indicador indicador = new Indicador();

            indicador.IdIndicador = (int)reader["id_indicador"];
            indicador.DesIndicador = (string)reader["des_indicador"];

            if (Convert.ToInt16(reader["tipo_indicador"]) == Convert.ToInt16(TipoIndicador.Parametro))
                indicador.TipoIndicador = TipoIndicador.Parametro;
            else
                indicador.TipoIndicador = TipoIndicador.Elegibilidade;

            return indicador;
        }

        public Indicador Find(int idIndicador)
        {
            return FindObject("idIndicador", idIndicador);
        }

        public IList<Indicador> List()
        {
            return ListObjects();
        }

        public IList<Indicador> ListByTipo(TipoIndicador tipoIndicador)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("tipoIndicador", (short)tipoIndicador);

            return ListObjects(QueryListByTipo(), param);
        }

        public Indicador Insert(Indicador indicador)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idIndicador", indicador.IdIndicador);
            param.Add("desIndicador", indicador.DesIndicador.ToUpper());
            param.Add("tipoIndicador", (short)indicador.TipoIndicador);

            InsertObject(QueryInsert(), param);

            return indicador;
        }

        public void Update(Indicador indicador)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idIndicador", indicador.IdIndicador);
            param.Add("desIndicador", indicador.DesIndicador.ToUpper());
            param.Add("tipoIndicador", (short)indicador.TipoIndicador);

            UpdateObject(QueryUpdate(), param);
        }

        public void Delete(int idIndicador)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idIndicador", idIndicador);

            DeleteObject(QueryDelete(), param);
        }

        public bool ValidaIndicador(int idIndicador)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idIndicador", idIndicador);

            short cont = Convert.ToInt16(ExecuteScalarQuery(QueryValidaInsert(), param));

            return cont > 0 ? true : false;
        }

        public bool VerificaRelacionamento(int idIndicador)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idIndicador", idIndicador);

            short cont = Convert.ToInt16(ExecuteScalarQuery(QueryVerificaRelacionamento(), param));

            return cont > 0 ? false : true;
        }

        public bool ExisteIndicador(int idIndicador)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idIndicador", idIndicador);

            short cont = Convert.ToInt16(ExecuteScalarQuery(QueryExisteIndicador(), param));

            return cont > 0 ? true : false;
        }
    }
}
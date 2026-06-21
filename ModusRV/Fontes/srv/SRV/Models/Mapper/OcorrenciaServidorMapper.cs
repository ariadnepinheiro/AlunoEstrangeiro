using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using SRV.Models.DTO;
using System.Text;

namespace SRV.Models.Mapper
{
    public class OcorrenciaServidorMapper : BaseMapper<OcorrenciaServidor>
    {
        protected override string QueryFindObject()
        {
			return @"SELECT id_ocorrencia_servidor, os.id_servidor id_servidor,
                            id_ocorrencia, dt_inicio_ocorrencia,
                            dt_fim_ocorrencia, id_unidade_administrativa,
                            dt_fim_original,
							s.DES_NOME_SERVIDOR,
							RECURSO
                       FROM rv_ocorrencia_servidor os
								inner join rv_servidor s
									on s.ID_SERVIDOR = os.ID_SERVIDOR
                      WHERE id_ocorrencia_servidor = @idOcorrenciaServidor";
        }

        protected override string QueryListObjects()
        {
            throw new NotImplementedException();
        }

        private string QueryList(FiltroOcorrenciaServidor filtro)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"SELECT os.id_ocorrencia_servidor, s.id_servidor, s.des_nome_servidor, o.id_ocorrencia, o.des_ocorrencia,
                                os.dt_inicio_ocorrencia, os.dt_fim_ocorrencia,
                                u.des_unidade_administrativa,
								os.RECURSO
                           FROM rv_ocorrencia_servidor os,
                                rv_servidor s,
                                rv_ocorrencia o,
                                rv_unidade_administrativa u
                          WHERE os.id_servidor = s.id_servidor
                            AND os.id_ocorrencia = o.id_ocorrencia
                            AND os.id_unidade_administrativa = u.id_unidade_administrativa");

            if (filtro.IdServidor != null)
                sql.Append(" AND s.id_servidor = @idServidor");

            if (filtro.IdUnidadeAdministrativa != null)
                sql.Append(" AND u.id_unidade_administrativa = @idUnidadeAdministrativa");

            if (filtro.IdOcorrencia != null)
                sql.Append(" AND o.id_ocorrencia = @idOcorrencia");

            sql.Append(" ORDER BY s.des_nome_servidor, o.des_ocorrencia");

            return sql.ToString();
        }

        private string QueryInsert()
        {
            return @"INSERT INTO rv_ocorrencia_servidor
                               (id_servidor,
                                id_ocorrencia,
                                dt_inicio_ocorrencia,
                                dt_fim_ocorrencia,
                                id_unidade_administrativa,
								RECURSO)
                        VALUES (@idServidor,
                                @idOcorrencia,
                                @dataInicio,
                                @dataFim,
                                @idUnidadeAdministrativa,
								@recurso)";
        }

        private string QueryDelete()
        {
            return @"DELETE FROM rv_ocorrencia_servidor
                           WHERE id_ocorrencia_servidor = @idOcorrenciaServidor";
        }

        private string QueryDeleteAll()
        {
            return @"DELETE FROM rv_ocorrencia_servidor
                           WHERE DATEPART(YEAR, dt_inicio_ocorrencia) = @idAnoReferencia
                             AND DATEPART(YEAR, dt_fim_ocorrencia) = @idAnoReferencia";
        }

        public override OcorrenciaServidor LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            OcorrenciaServidor ocorrenciaServidor = new OcorrenciaServidor();

            ocorrenciaServidor.IdOcorrenciaServidor = (int)reader["id_ocorrencia_servidor"];
            ocorrenciaServidor.DataInicioOcorrencia = (DateTime)reader["dt_inicio_ocorrencia"];

            if (reader["dt_fim_ocorrencia"] != DBNull.Value)
                ocorrenciaServidor.DataFimOcorrencia = (DateTime?)reader["dt_fim_ocorrencia"];

            if(reader["dt_fim_original"] != DBNull.Value)
                ocorrenciaServidor.DataFimOriginal = (DateTime?)reader["dt_fim_original"];

            ocorrenciaServidor.Servidor = new Servidor();
            ocorrenciaServidor.Servidor.IdServidor = Convert.ToInt32(reader["id_servidor"]);
			ocorrenciaServidor.Servidor.DesNomeServidor = (string)reader["des_nome_servidor"];

            ocorrenciaServidor.Ocorrencia = new Ocorrencia();
            ocorrenciaServidor.Ocorrencia.IdOcorrencia = (int)reader["id_ocorrencia"];

            ocorrenciaServidor.UnidadeAdministrativa = new UnidadeAdministrativa();
            ocorrenciaServidor.UnidadeAdministrativa.IdUnidadeAdministrativa= Convert.ToInt32(reader["id_unidade_administrativa"]);

			ocorrenciaServidor.Recurso = reader["RECURSO"].ToString();

            return ocorrenciaServidor;
        }

        public OcorrenciaServidor LoadObjectGrid(System.Data.SqlClient.SqlDataReader reader)
        {
            OcorrenciaServidor ocorrenciaServidor = new OcorrenciaServidor();

            ocorrenciaServidor.IdOcorrenciaServidor = (int)reader["id_ocorrencia_servidor"];
            ocorrenciaServidor.DataInicioOcorrencia = (DateTime)reader["dt_inicio_ocorrencia"];
            ocorrenciaServidor.DataFimOcorrencia = (DateTime?)reader["dt_fim_ocorrencia"];

            ocorrenciaServidor.Servidor = new Servidor();
			ocorrenciaServidor.Servidor.IdServidor = Convert.ToInt32(reader["id_servidor"]);
            ocorrenciaServidor.Servidor.DesNomeServidor = (string)reader["des_nome_servidor"];

            ocorrenciaServidor.Ocorrencia = new Ocorrencia();
			ocorrenciaServidor.Ocorrencia.IdOcorrencia = (int)reader["id_ocorrencia"];
            ocorrenciaServidor.Ocorrencia.DesOcorrencia = (string)reader["des_ocorrencia"];

            ocorrenciaServidor.UnidadeAdministrativa = new UnidadeAdministrativa();
            ocorrenciaServidor.UnidadeAdministrativa.DesUnidadeAdministrativa = (string)reader["des_unidade_administrativa"];

			ocorrenciaServidor.Recurso = reader["RECURSO"].ToString();

            return ocorrenciaServidor;
        }

        public OcorrenciaServidor Find(int idOcorrenciaServidor)
        {
            return FindObject("idOcorrenciaServidor", idOcorrenciaServidor);
        }

        public Paging<OcorrenciaServidor> List(FiltroOcorrenciaServidor filtro, int currentPage, int pageSize)
        {
            Dictionary<string, object> param = new Dictionary<string,object>();

            if (filtro.IdServidor != null)
                param.Add("idServidor", filtro.IdServidor);

            if (filtro.IdUnidadeAdministrativa != null)
                param.Add("idUnidadeAdministrativa", filtro.IdUnidadeAdministrativa);

            if (filtro.IdOcorrencia != null)
                param.Add("idOcorrencia", filtro.IdOcorrencia);

            return ListPagingObjects(QueryList(filtro), param, LoadObjectGrid, currentPage, pageSize);
        }

        public OcorrenciaServidor Insert(OcorrenciaServidor ocorrenciaServidor)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idServidor", ocorrenciaServidor.Servidor.IdServidor);
            param.Add("idOcorrencia", ocorrenciaServidor.Ocorrencia.IdOcorrencia);
            param.Add("dataInicio", ocorrenciaServidor.DataInicioOcorrencia);
            param.Add("dataFim", ocorrenciaServidor.DataFimOcorrencia);
            param.Add("idUnidadeAdministrativa", ocorrenciaServidor.UnidadeAdministrativa.IdUnidadeAdministrativa);
			param.Add("recurso", ocorrenciaServidor.Recurso);

            ocorrenciaServidor.IdOcorrenciaServidor = (int)InsertObjectWithIdentity(QueryInsert(), param);

            return ocorrenciaServidor;
        }

        public void Delete(int idOcorrenciaServidor)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idOcorrenciaServidor", idOcorrenciaServidor);

            DeleteObject(QueryDelete(), param);
        }

        public void DeleteAll(int idAnoReferencia)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idAnoReferencia", idAnoReferencia);

            DeleteObject(QueryDeleteAll(), param);
        }

		private string QueryUpdate()
		{
			return @"UPDATE rv_ocorrencia_servidor
                        SET dt_inicio_ocorrencia = @dataInicio,
                            dt_fim_ocorrencia = @dataFim,
                            id_ocorrencia = @idOcorrencia,
							id_unidade_administrativa = @idUnidadeAdministrativa,
							RECURSO = @recurso
                      WHERE id_ocorrencia_servidor = @idOcorrenciaServidor";
		}

		public void Update(OcorrenciaServidor ocorrenciaServidor)
		{
			Dictionary<string, object> param = new Dictionary<string, object>();

			param.Add("idOcorrenciaServidor", ocorrenciaServidor.IdOcorrenciaServidor);
			param.Add("idServidor", ocorrenciaServidor.Servidor.IdServidor);
			param.Add("idOcorrencia", ocorrenciaServidor.Ocorrencia.IdOcorrencia);
			param.Add("dataInicio", ocorrenciaServidor.DataInicioOcorrencia);
			param.Add("dataFim", ocorrenciaServidor.DataFimOcorrencia);
			param.Add("idUnidadeAdministrativa", ocorrenciaServidor.UnidadeAdministrativa.IdUnidadeAdministrativa);
			param.Add("recurso", ocorrenciaServidor.Recurso);

			UpdateObject(QueryUpdate(), param);
		}

		public OcorrenciaServidor CalculaTotalAfastamento(int idServidor, int idAnoReferencia)
		{
			Dictionary<string, object> param = new Dictionary<string, object>();

			param.Add("idServidor", idServidor);
			param.Add("idAnoReferencia", idAnoReferencia);

			OcorrenciaServidor resultado = FindObject(QueryTotalAfastamentoServidor(), param, PreencheTotalAfastamento);

			return resultado;
		}

		private OcorrenciaServidor PreencheTotalAfastamento(System.Data.SqlClient.SqlDataReader reader)
		{
			OcorrenciaServidor ocorrenciaServidor = new OcorrenciaServidor();

			ocorrenciaServidor.TotalDiasAlocado = Convert.ToInt32(reader["DIAS"].ToString());
			ocorrenciaServidor.PercentualTotalAlocado = Convert.ToDecimal(reader["PERC"].ToString());

			return ocorrenciaServidor;

		}

		private string QueryTotalAfastamentoServidor()
		{
			return @"SELECT tab.DIAS, Round(((CAST(tab.DIAS as float)/ TotalDias) * 100),2) as PERC
 					 FROM
					 (SELECT 
						dbo.[FC_ELEGIBILIDADE_SERVIDOR_DIAS_OCORRENCIA](ID_SERVIDOR, AR.ID_ANO_REFERENCIA) AS DIAS, 
						DATEDIFF(day, DT_INICIO_PERIODOLETIVO, DT_FIM_PERIODOLETIVO) + 1 AS TotalDias
					 FROM RV_SERVIDOR S
							inner join RV_ANO_REFERENCIA AR on AR.ID_ANO_REFERENCIA = @idAnoReferencia
					 where ID_SERVIDOR = @idServidor) tab";
		}


		/// <summary>
		/// Regra para preenchimento do campo ID_UNIDADE_ADMINISTRATIVA na criação/edição de Ocorrência Servidor na fase de recurso.
		/// deve-se pegar um MAX da unidade administrativa na tabela rv_ocorrencia_servidor, caso exista alguma ocorrência cadastrada do servidor.
		/// </summary>
		/// <param name="idServidor">Código do servidor</param>
		/// <returns></returns>
		public int? FindMaxUnidadeAdministrativaByServidor(int idServidor)
		{
			Dictionary<string, object> param = new Dictionary<string, object>();

			param.Add("idServidor", idServidor);

			int idUnidadeAdministrativa = Convert.ToInt32(ExecuteScalarQuery(QueryFindMaxUnidadeAdministrativaByServidor(), param));

			if (idUnidadeAdministrativa == 0)
				return null;

			return idUnidadeAdministrativa;
		}

		private string QueryFindMaxUnidadeAdministrativaByServidor()
		{
			return @"SELECT MAX(id_unidade_administrativa)
                       FROM rv_ocorrencia_servidor
                      WHERE id_servidor = @idServidor";
		}
    }
}
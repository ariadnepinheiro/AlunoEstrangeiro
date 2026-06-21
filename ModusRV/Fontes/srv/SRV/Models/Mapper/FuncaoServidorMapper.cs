using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using SRV.Models.DTO;
using System.Text;

namespace SRV.Models.Mapper
{
    public class FuncaoServidorMapper : BaseMapper<FuncaoServidor>
    {
        protected override string QueryFindObject()
        {
			return @"SELECT id_funcao_servidor, fs.id_servidor id_servidor,
                            id_funcao, id_ano_referencia,
                            dt_inicio_funcao, dt_fim_funcao,
                            nm_proporcionalidade, id_unidade_administrativa,
                            dt_fim_original,
							CARGAHORARIAALOCADA, CARGAHORARIALIVRE, CARGAHORARIATOTAL, RECURSO,
							s.DES_NOME_SERVIDOR
                       FROM rv_funcao_servidor fs
								inner join rv_servidor s
									on s.ID_SERVIDOR = fs.ID_SERVIDOR
                      WHERE id_funcao_servidor = @idFuncaoServidor";
        }

        private string QueryFind()
        {
            return @"SELECT id_funcao_servidor, id_servidor,
                            id_funcao, id_ano_referencia,
                            dt_inicio_funcao, dt_fim_funcao,
                            nm_proporcionalidade, id_unidade_administrativa,
                            dt_fim_original,
							CARGAHORARIAALOCADA, CARGAHORARIALIVRE, CARGAHORARIATOTAL, RECURSO
                       FROM rv_funcao_servidor
                      WHERE id_servidor = @idServidor
                        AND id_funcao = @idFuncao
                        AND id_ano_referencia = @idAnoReferencia
                        AND id_unidade_administrativa = @idUnidadeAdministrativa";
        }

        protected override string QueryListObjects()
        {
            throw new NotImplementedException();
        }

        private string QueryList(FiltroFuncaoServidor filtro)
        {
            StringBuilder sql = new StringBuilder();

			sql.Append(@"select fs.id_funcao_servidor, s.id_servidor, s.des_nome_servidor, f.des_funcao, fs.id_ano_referencia,
								fs.dt_inicio_funcao, fs.dt_fim_funcao,
								fs.nm_proporcionalidade, u.id_unidade_administrativa, u.des_unidade_administrativa,
								eu.fl_elegivel, fu.nm_qtd_vencimento,
								fs.CARGAHORARIAALOCADA ,fs.CARGAHORARIALIVRE, fs.CARGAHORARIATOTAL, fs.RECURSO
						from rv_funcao_servidor fs
								inner join rv_servidor s
									on fs.id_servidor = s.id_servidor
								inner join rv_funcao f 
									on fs.id_funcao = f.id_funcao
								inner join rv_unidade_administrativa u
									on fs.id_unidade_administrativa = u.id_unidade_administrativa
								inner join rv_elegibilidade_unidadm eu 
									on eu.id_unidade_administrativa = u.id_unidade_administrativa
								left join rv_nota_funcao_unidadm fu
									on fu.id_unidade_administrativa = u.id_unidade_administrativa
									and fu.id_grupo_funcao = f.id_grupo_funcao 
						where fs.id_ano_referencia = @idAnoReferencia");

            if (filtro.IdServidor != null)
                sql.Append(" AND s.id_servidor = @idServidor");

            if (filtro.IdUnidadeAdministrativa != null)
                sql.Append(" AND u.id_unidade_administrativa = @idUnidadeAdministrativa");

            if (filtro.IdFuncao != null)
                sql.Append(" AND f.id_funcao = @idFuncao");

            sql.Append(@" ORDER BY s.des_nome_servidor, f.des_funcao, u.des_unidade_administrativa");

            return sql.ToString();
        }

        private string QueryFindMaxUnidadeAdministrativaByServidor()
        {
            return @"SELECT MAX(id_unidade_administrativa)
                       FROM rv_funcao_servidor
                      WHERE id_servidor = @idServidor";
        }

        private string QueryInsert()
        {
            return @"INSERT INTO rv_funcao_servidor
                               (id_servidor,
                                id_funcao,
                                id_ano_referencia,
                                dt_inicio_funcao,
                                dt_fim_funcao,
                                nm_proporcionalidade,
                                id_unidade_administrativa,
                                dt_fim_original,
								CARGAHORARIAALOCADA,
								CARGAHORARIALIVRE,
								CARGAHORARIATOTAL,
								RECURSO
								)
                        VALUES (@idServidor,
                                @idFuncao,
                                @idAnoReferencia,
                                @dataInicio,
                                @dataFim,
                                @proporcionalidade,
                                @idUnidadeAdministrativa,
                                @dataFimOriginal,
								@cargaHorariaAlocada,
								@cargaHorariaLivre,
								@cargaHorariaTotal,
								@recurso)";
        }

        private string QueryUpdate()
        {
            return @"UPDATE rv_funcao_servidor
                        SET dt_inicio_funcao = @dataInicio,
                            dt_fim_funcao = @dataFim,
                            nm_proporcionalidade = @proporcionalidade,
							id_unidade_administrativa = @idUnidadeAdministrativa,
							id_funcao = @idFuncao,
							CARGAHORARIAALOCADA = @cargaHorariaAlocada,
							CARGAHORARIALIVRE =	@cargaHorariaLivre,
							CARGAHORARIATOTAL =	@cargaHorariaTotal,
							RECURSO = @recurso
                      WHERE id_funcao_servidor = @idFuncaoServidor";
        }

        private string QueryDelete()
        {
			return @"DELETE FROM rv_funcao_servidor
                           WHERE id_funcao_servidor = @idFuncaoServidor";
        }

        private string QueryDeleteAll()
        {
            return @"DELETE FROM rv_funcao_servidor
                           WHERE id_ano_referencia = @idAnoReferencia";
        }

        private string QueryCountFuncaoByServidor()
        {
            return @"SELECT COUNT(*)
                       FROM rv_funcao_servidor
                      WHERE id_servidor = @idServidor";
        }

		/// <summary>
		/// Load usado pelo metodo QueryList
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
        public override FuncaoServidor LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            FuncaoServidor funcaoServidor = new FuncaoServidor();

            funcaoServidor.IdFuncaoServidor = (int)reader["id_funcao_servidor"];

            funcaoServidor.Servidor = new Servidor();
            funcaoServidor.Servidor.IdServidor = Convert.ToInt32(reader["id_servidor"]);
            funcaoServidor.Servidor.DesNomeServidor = (string)reader["des_nome_servidor"];

            funcaoServidor.Funcao = new Funcao();
            funcaoServidor.Funcao.DesFuncao = (string)reader["des_funcao"];

            funcaoServidor.AnoReferencia = new AnoReferencia();
            funcaoServidor.AnoReferencia.IdAnoReferencia = (int)reader["id_ano_referencia"];

            funcaoServidor.DataInicioFuncao = (DateTime)reader["dt_inicio_funcao"];

            if(reader["dt_fim_funcao"] != DBNull.Value)
                funcaoServidor.DataFimFuncao = (DateTime)reader["dt_fim_funcao"];

            if (reader["nm_proporcionalidade"] != DBNull.Value)
                funcaoServidor.Proporcionalidade = (decimal)reader["nm_proporcionalidade"];

            funcaoServidor.UnidadeAdministrativa = new UnidadeAdministrativa();
            funcaoServidor.UnidadeAdministrativa.IdUnidadeAdministrativa = Convert.ToInt32(reader["id_unidade_administrativa"]);
            funcaoServidor.UnidadeAdministrativa.DesUnidadeAdministrativa = (string)reader["des_unidade_administrativa"];

			if (reader["nm_qtd_vencimento"] != DBNull.Value)
				funcaoServidor.Nota = Convert.ToDecimal(reader["nm_qtd_vencimento"]);

			funcaoServidor.Elegivel = (string)reader["fl_elegivel"];

			if (reader["CARGAHORARIAALOCADA"] != DBNull.Value)
                funcaoServidor.CargaHorariaAlocada= (decimal)reader["CARGAHORARIAALOCADA"];

			if (reader["CARGAHORARIALIVRE"] != DBNull.Value)
                funcaoServidor.CargaHorariaLivre = (decimal)reader["CARGAHORARIALIVRE"];

			if (reader["CARGAHORARIATOTAL"] != DBNull.Value)
                funcaoServidor.CargaHorariaTotal = (decimal)reader["CARGAHORARIATOTAL"];

			funcaoServidor.Recurso = reader["RECURSO"].ToString();

            return funcaoServidor;
        }

		/// <summary>
		/// Load usado pelo método Find
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
        public FuncaoServidor LoadObjectSimple(System.Data.SqlClient.SqlDataReader reader)
        {
            FuncaoServidor funcaoServidor = new FuncaoServidor();

            funcaoServidor.IdFuncaoServidor = (int)reader["id_funcao_servidor"];

            funcaoServidor.Servidor = new Servidor();
            funcaoServidor.Servidor.IdServidor = Convert.ToInt32(reader["id_servidor"]);
			funcaoServidor.Servidor.DesNomeServidor = (string)reader["DES_NOME_SERVIDOR"]; 

            funcaoServidor.Funcao = new Funcao();
            funcaoServidor.Funcao.IdFuncao = (string)reader["id_funcao"];

            funcaoServidor.AnoReferencia = new AnoReferencia();
            funcaoServidor.AnoReferencia.IdAnoReferencia = (int)reader["id_ano_referencia"];

            funcaoServidor.DataInicioFuncao = (DateTime)reader["dt_inicio_funcao"];

            if (reader["dt_fim_funcao"] != DBNull.Value)
                funcaoServidor.DataFimFuncao = (DateTime)reader["dt_fim_funcao"];

            if (reader["nm_proporcionalidade"] != DBNull.Value)
                funcaoServidor.Proporcionalidade = (decimal)reader["nm_proporcionalidade"];

            funcaoServidor.UnidadeAdministrativa = new UnidadeAdministrativa();
            funcaoServidor.UnidadeAdministrativa.IdUnidadeAdministrativa = Convert.ToInt32(reader["id_unidade_administrativa"]);

			if (reader["CARGAHORARIAALOCADA"] != DBNull.Value)
				funcaoServidor.CargaHorariaAlocada = (decimal)reader["CARGAHORARIAALOCADA"];

			if (reader["CARGAHORARIALIVRE"] != DBNull.Value)
				funcaoServidor.CargaHorariaLivre = (decimal)reader["CARGAHORARIALIVRE"];

			if (reader["CARGAHORARIATOTAL"] != DBNull.Value)
				funcaoServidor.CargaHorariaTotal = (decimal)reader["CARGAHORARIATOTAL"];

			funcaoServidor.Recurso = reader["RECURSO"].ToString();		

            return funcaoServidor;
        }

        public FuncaoServidor Find(int idFuncaoServidor)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idFuncaoServidor", idFuncaoServidor);

            return FindObject(QueryFindObject(), param, LoadObjectSimple);
        }

        public FuncaoServidor Find(int idServidor, string idFuncao, int idAnoReferencia, int idUnidadeAdministrativa)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idServidor", idServidor);
            param.Add("idFuncao", idFuncao);
            param.Add("idAnoReferencia", idAnoReferencia);
            param.Add("idUnidadeAdministrativa", idUnidadeAdministrativa);

            return FindObject(QueryFind(), param, LoadObjectSimple);
        }

        /// <summary>
        /// Regra para preenchimento do campo ID_UNIDADE_ADMINISTRATIVA na importação de Ocorrência Servidor,
        /// deve-se pegar um MAX da unidade administrativa na tabela RV_FUNCAO_SERVIDOR para cada servidor
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

        public Paging<FuncaoServidor> List(FiltroFuncaoServidor filtro, int currentPage, int pageSize)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            if(filtro.IdServidor != null)
                param.Add("idServidor", filtro.IdServidor);

            if (filtro.IdUnidadeAdministrativa != null)
                param.Add("idUnidadeAdministrativa", filtro.IdUnidadeAdministrativa);

            if (filtro.IdFuncao != null)
                param.Add("idFuncao", filtro.IdFuncao);

            param.Add("idAnoReferencia", filtro.IdAnoReferencia);

            return ListPagingObjects(QueryList(filtro), param, currentPage, pageSize);
        }

        public FuncaoServidor Insert(FuncaoServidor funcaoServidor)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idServidor", funcaoServidor.Servidor.IdServidor);
            param.Add("idFuncao", funcaoServidor.Funcao.IdFuncao);
            param.Add("idAnoReferencia", funcaoServidor.AnoReferencia.IdAnoReferencia);
            param.Add("dataInicio", funcaoServidor.DataInicioFuncao);
            param.Add("dataFim", funcaoServidor.DataFimFuncao);
            param.Add("proporcionalidade", funcaoServidor.Proporcionalidade);
            param.Add("idUnidadeAdministrativa", funcaoServidor.UnidadeAdministrativa.IdUnidadeAdministrativa);
            param.Add("dataFimOriginal", funcaoServidor.DataFimOriginal);
			param.Add("cargaHorariaAlocada", funcaoServidor.CargaHorariaAlocada);
			param.Add("cargaHorariaLivre", funcaoServidor.CargaHorariaLivre);
			param.Add("cargaHorariaTotal", funcaoServidor.CargaHorariaTotal);
			param.Add("recurso", funcaoServidor.Recurso);

            funcaoServidor.IdFuncaoServidor = (int)InsertObjectWithIdentity(QueryInsert(), param);

            return funcaoServidor;
        }

        public void Update(FuncaoServidor funcaoServidor)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

			param.Add("idFuncaoServidor", funcaoServidor.IdFuncaoServidor);
			param.Add("idServidor", funcaoServidor.Servidor.IdServidor);
            param.Add("idFuncao", funcaoServidor.Funcao.IdFuncao);
            param.Add("idAnoReferencia", funcaoServidor.AnoReferencia.IdAnoReferencia);
            param.Add("dataInicio", funcaoServidor.DataInicioFuncao);
            param.Add("dataFim", funcaoServidor.DataFimFuncao);
            param.Add("proporcionalidade", funcaoServidor.Proporcionalidade);
            param.Add("idUnidadeAdministrativa", funcaoServidor.UnidadeAdministrativa.IdUnidadeAdministrativa);
			param.Add("cargaHorariaAlocada", funcaoServidor.CargaHorariaAlocada);
			param.Add("cargaHorariaLivre", funcaoServidor.CargaHorariaLivre);
			param.Add("cargaHorariaTotal", funcaoServidor.CargaHorariaTotal);
			param.Add("recurso", funcaoServidor.Recurso);

            UpdateObject(QueryUpdate(), param);
        }

        public void Delete(int idFuncaoServidor)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idFuncaoServidor", idFuncaoServidor);

            DeleteObject(QueryDelete(), param);
        }

        public void DeleteAll(int idAnoReferencia)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idAnoReferencia", idAnoReferencia);

            DeleteObject(QueryDeleteAll(), param);
        }

        public short CountFuncaoByServidor(int idServidor)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idServidor", idServidor);

            short cont = Convert.ToInt16(ExecuteScalarQuery(QueryCountFuncaoByServidor(), param));

            return cont;
        }

		public FuncaoServidor CalculaTotalAlocacao(FuncaoServidor funcaoServidor)
		{
			Dictionary<string, object> param = new Dictionary<string, object>();

			param.Add("idServidor", funcaoServidor.Servidor.IdServidor);
			param.Add("idAnoReferencia", funcaoServidor.AnoReferencia.IdAnoReferencia);

			FuncaoServidor resultado = FindObject(QueryTotalAlocacaoServidor(), param, PreencheTotalAlocacao);

			funcaoServidor.PercentualTotalAlocado = resultado.PercentualTotalAlocado;
			funcaoServidor.TotalDiasAlocado = resultado.TotalDiasAlocado;

			return funcaoServidor;
		}

		private FuncaoServidor PreencheTotalAlocacao(System.Data.SqlClient.SqlDataReader reader)
		{
			FuncaoServidor funcaoServidor = new FuncaoServidor();

			funcaoServidor.TotalDiasAlocado = Convert.ToInt32(reader["DIAS"].ToString());
			funcaoServidor.PercentualTotalAlocado = Convert.ToDecimal(reader["PERC"].ToString());

			return funcaoServidor;

		}

		private string QueryTotalAlocacaoServidor()
		{
			return @"SELECT tab.DIAS, 
							Round(((CAST(tab.DIAS as float)/ TotalDias) * 100),2) as PERC
					FROM
						(SELECT 
							dbo.[FC_ELEGIBILIDADE_SERVIDOR_DIAS_FUNCAO](ID_SERVIDOR,  AR.ID_ANO_REFERENCIA) AS DIAS, 
							DATEDIFF(day, DT_INICIO_PERIODOLETIVO, DT_FIM_PERIODOLETIVO) + 1 AS TotalDias
						 FROM RV_SERVIDOR S
								inner join RV_ANO_REFERENCIA AR on AR.ID_ANO_REFERENCIA = @idAnoReferencia
						 where ID_SERVIDOR = @idServidor) tab";
		}

    }
}
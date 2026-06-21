using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using SRV.Models.DTO;
using System.Text;

namespace SRV.Models.Mapper
{
	public class AvaliacaoExternaUnidadeAdminDetalheMapper : BaseMapper<AvaliacaoExternaUnidadeAdminDetalhe>
	{
		protected override string QueryFindObject()
		{
			throw new NotImplementedException();
		}

		protected override string QueryListObjects()
		{
			throw new NotImplementedException();
		}

		public override AvaliacaoExternaUnidadeAdminDetalhe LoadObject(System.Data.SqlClient.SqlDataReader reader)
		{
			AvaliacaoExternaUnidadeAdminDetalhe avaliacaoExternaUnidadeAdminDetalhe = new AvaliacaoExternaUnidadeAdminDetalhe();

			avaliacaoExternaUnidadeAdminDetalhe.AvaliacaoExterna = new AvaliacaoExterna();
			avaliacaoExternaUnidadeAdminDetalhe.AvaliacaoExterna.IdAvaliacaoExterna = Convert.ToInt32(reader["id_avaliacao_externa"]);
			avaliacaoExternaUnidadeAdminDetalhe.AvaliacaoExterna.DesAvaliacaoExterna = reader["des_avaliacao_externa"].ToString();

			avaliacaoExternaUnidadeAdminDetalhe.UnidadeAdministrativa = new UnidadeAdministrativa();
			avaliacaoExternaUnidadeAdminDetalhe.UnidadeAdministrativa.IdUnidadeAdministrativa = Convert.ToInt32(reader["id_unidade_administrativa"]);

			avaliacaoExternaUnidadeAdminDetalhe.AnoReferencia = new AnoReferencia();
			avaliacaoExternaUnidadeAdminDetalhe.AnoReferencia.IdAnoReferencia = Convert.ToInt32(reader["id_ano_referencia"]);

			avaliacaoExternaUnidadeAdminDetalhe.Turno = new Turno();
			avaliacaoExternaUnidadeAdminDetalhe.Turno.IdTurno = Convert.ToInt32(reader["id_turno"]);
            avaliacaoExternaUnidadeAdminDetalhe.Turno.Codigo = (CodigoTurno)Enum.Parse(typeof(CodigoTurno), reader["fl_codigo_turno"].ToString());

			avaliacaoExternaUnidadeAdminDetalhe.Turma = reader["des_turma"].ToString();
			avaliacaoExternaUnidadeAdminDetalhe.Periodo = (Periodo)Convert.ToInt32(reader["nm_periodo"]);
			avaliacaoExternaUnidadeAdminDetalhe.Previsto = Convert.ToInt32(reader["nm_previsto"]);
			avaliacaoExternaUnidadeAdminDetalhe.Realizado = Convert.ToInt32(reader["nm_realizado"]);

			return avaliacaoExternaUnidadeAdminDetalhe;
		}

		public Paging<AvaliacaoExternaUnidadeAdminDetalhe> List(FiltroAvaliacaoExternaUnidadeAdminDetalhe filtro, int currentPage, int pageSize)
		{
			Dictionary<string, object> param = new Dictionary<string, object>();

			if (filtro.IdAvaliacaoExterna != null)
				param.Add("idAvaliacaoExterna", filtro.IdAvaliacaoExterna);

			if (filtro.IdUnidadeAdministrativa != null)
				param.Add("idUnidadeAdministrativa", filtro.IdUnidadeAdministrativa);

			param.Add("ciclo", filtro.IdAnoReferencia);

			return ListPagingObjects(QueryList(filtro), param, LoadObject, currentPage, pageSize);
		}

		private string QueryList(FiltroAvaliacaoExternaUnidadeAdminDetalhe filtro)
		{
			StringBuilder sql = new StringBuilder();

			sql.Append(@" SELECT daeu.id_ano_referencia 
								,daeu.id_unidade_administrativa 
								,daeu.id_turno 
								,daeu.id_avaliacao_externa 
								,ae.des_avaliacao_externa
								,daeu.des_turma 
								,daeu.nm_periodo 
								,daeu.nm_previsto 
								,daeu.nm_realizado 
								,t.fl_codigo_turno 
						   FROM rv_detalhe_avaliacao_externa_unidadm daeu
							    INNER JOIN rv_avaliacao_externa ae 
								  ON daeu.id_avaliacao_externa = ae.id_avaliacao_externa
							    INNER JOIN rv_turno t 
								  ON t.id_turno = daeu.id_turno

						  WHERE DAEU.id_ano_referencia = @ciclo ");

			if (filtro.IdUnidadeAdministrativa != null)
				sql.Append(" AND daeu.id_unidade_administrativa = @idUnidadeAdministrativa");

			if (filtro.IdAvaliacaoExterna != null)
				sql.Append(" AND ae.id_avaliacao_externa = @idAvaliacaoExterna");

			sql.Append(" ORDER BY daeu.des_turma, ae.des_avaliacao_externa ");

			return sql.ToString();
		}

		private string QueryInsert()
		{
			return @"INSERT INTO RV_DETALHE_AVALIACAO_EXTERNA_UNIDADM
							(ID_ANO_REFERENCIA,
							ID_UNIDADE_ADMINISTRATIVA, 
							ID_AVALIACAO_EXTERNA,
							ID_TURNO,
							DES_TURMA,
							NM_PERIODO,
							NM_PREVISTO,
							NM_REALIZADO)
					VALUES (@IdAnoReferencia, 
							@IdUnidadeAdministrativa, 
							@IdAvaliacaoExterna, 
							@IdTurno, 
							@Turma, 
							@Periodo, 
							@Previsto, 
							@Realizado)";
		}

		internal void Insert(AvaliacaoExternaUnidadeAdminDetalhe avaliacaoExternaUnidadeAdminDetalhe)
		{
			Dictionary<string, object> param = new Dictionary<string, object>();

			param.Add("IdAnoReferencia", avaliacaoExternaUnidadeAdminDetalhe.AnoReferencia.IdAnoReferencia);
			param.Add("IdUnidadeAdministrativa", avaliacaoExternaUnidadeAdminDetalhe.UnidadeAdministrativa.IdUnidadeAdministrativa);
			param.Add("IdAvaliacaoExterna", avaliacaoExternaUnidadeAdminDetalhe.AvaliacaoExterna.IdAvaliacaoExterna);
			param.Add("IdTurno", avaliacaoExternaUnidadeAdminDetalhe.Turno.IdTurno);
			param.Add("Turma", avaliacaoExternaUnidadeAdminDetalhe.Turma);
			param.Add("Periodo", (int)avaliacaoExternaUnidadeAdminDetalhe.Periodo);
			param.Add("Previsto", avaliacaoExternaUnidadeAdminDetalhe.Previsto);
			param.Add("Realizado", avaliacaoExternaUnidadeAdminDetalhe.Realizado);

			InsertObject(QueryInsert(), param);
		}

		private string QueryDeleteAll()
		{
			return @"DELETE FROM rv_detalhe_avaliacao_externa_unidadm WHERE id_ano_referencia = @idAnoReferencia";
		}

		public void DeleteAll(int idAnoReferencia)
		{
			Dictionary<string, object> param = new Dictionary<string, object>();

			param.Add("idAnoReferencia", idAnoReferencia);

			DeleteObject(QueryDeleteAll(), param);
		}
		
		internal bool PossuiTurno(int idUnidadeAdministrativa, GrupoTurno grupoTurno, int idAnoReferencia)
		{
			Dictionary<string, object> param = new Dictionary<string, object>();
			
			param.Add("grupoTurno", grupoTurno.ToString());
			param.Add("idUnidadeAdministrativa", idUnidadeAdministrativa);
			param.Add("idAnoReferencia", idAnoReferencia);

			return (int) base.ExecuteScalarQuery(QueryPossuiTurno(), param) > 0 ? true : false;
		}

		private string QueryPossuiTurno()
		{
			return @"SELECT COUNT(DAEU.ID_UNIDADE_ADMINISTRATIVA) 
					  FROM RV_DETALHE_AVALIACAO_EXTERNA_UNIDADM DAEU
					 INNER JOIN RV_TURNO T ON DAEU.ID_TURNO = T.ID_TURNO
					 WHERE DAEU.ID_UNIDADE_ADMINISTRATIVA = @idUnidadeAdministrativa 
					   AND T.FL_GRUPO_TURNO = @grupoTurno
					   AND DAEU.ID_ANO_REFERENCIA = @idAnoReferencia";
		}

		internal bool MetasIndividuaisAtingidas(int idUnidadeAdministrativa, int idAvaliacaoExterna, GrupoTurno grupoTurno, int idAnoReferencia, decimal metaIndividual)
		{
			Dictionary<string, object> param = new Dictionary<string, object>();

			param.Add("grupoTurno", grupoTurno.ToString());
			param.Add("idUnidadeAdministrativa", idUnidadeAdministrativa);
			param.Add("idAnoReferencia", idAnoReferencia);
			param.Add("idAvaliacaoExterna", idAvaliacaoExterna);
			param.Add("metaIndivitual", metaIndividual);

			return (int)base.ExecuteScalarQuery(QueryMetasIndividuaisAtingidas(), param) == 0 ? true : false;
		}

		private string QueryMetasIndividuaisAtingidas()
		{
			return @"SELECT COUNT(DAEU.ID_UNIDADE_ADMINISTRATIVA) 
					  FROM RV_DETALHE_AVALIACAO_EXTERNA_UNIDADM DAEU
					 INNER JOIN RV_TURNO T ON DAEU.ID_TURNO = T.ID_TURNO
					 WHERE DAEU.ID_UNIDADE_ADMINISTRATIVA = @idUnidadeAdministrativa 
					   AND DAEU.ID_AVALIACAO_EXTERNA = @idAvaliacaoExterna
					   AND T.FL_GRUPO_TURNO = @grupoTurno
					   AND DAEU.ID_ANO_REFERENCIA = @idAnoReferencia
					   AND CONVERT(DECIMAL(6,2), DAEU.NM_REALIZADO) / CONVERT(DECIMAL(6,2), DAEU.NM_PREVISTO) < @metaIndivitual";
		}
		
		internal decimal? CalculoPercentualAtingidoPor(int idUnidadeAdministrativa, int idAvaliacaoExterna, GrupoTurno grupoTurno, int idAnoReferencia)
		{
			Dictionary<string, object> param = new Dictionary<string, object>();

			param.Add("grupoTurno", grupoTurno.ToString());
			param.Add("idUnidadeAdministrativa", idUnidadeAdministrativa);
			param.Add("idAnoReferencia", idAnoReferencia);
			param.Add("idAvaliacaoExterna", idAvaliacaoExterna);

			return (decimal?) base.ExecuteScalarQuery(QueryCalculoPercentualAtingido(), param);
		}

		private string QueryCalculoPercentualAtingido()
		{
            return @"SELECT 
	                    ROUND(CONVERT(DECIMAL(6,2), sum(AE.NM_REALIZADO)) / CONVERT(DECIMAL(6,2), sum(AE.NM_PREVISTO)), 2)
                    FROM
                        RV_DETALHE_AVALIACAO_EXTERNA_UNIDADM AE INNER JOIN RV_TURNO TU
                            ON AE.ID_TURNO = TU.ID_TURNO
                    WHERE
                        ID_UNIDADE_ADMINISTRATIVA = @idUnidadeAdministrativa
                        AND AE.ID_AVALIACAO_EXTERNA = @idAvaliacaoExterna
                        AND TU.FL_GRUPO_TURNO = @grupoTurno
                        AND AE.ID_ANO_REFERENCIA = @idAnoReferencia";
		}
	}
}
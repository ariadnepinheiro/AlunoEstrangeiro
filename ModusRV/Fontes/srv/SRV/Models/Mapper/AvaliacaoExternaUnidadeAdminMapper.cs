using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using SRV.Models.DTO;
using System.Text;

namespace SRV.Models.Mapper
{
    public class AvaliacaoExternaUnidadeAdminMapper : BaseMapper<AvaliacaoExternaUnidadeAdmin>
    {
        protected override string QueryFindObject()
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"SELECT au.id_avaliacao_externa, 
                                ae.des_avaliacao_externa,
	                            au.id_unidade_administrativa, 
                                ua.des_unidade_administrativa,
                                au.nm_perc_participacao,
                                au.nm_perc_participacao_diurno,								 
								au.nm_perc_participacao_noturno,
	                            au.id_ano_referencia 
                           FROM rv_avaliacao_externa_unidadm au, 
                                rv_avaliacao_externa ae, 
                                rv_unidade_administrativa ua
                          WHERE id_ano_referencia = @ciclo
                            AND au.id_avaliacao_externa = ae.id_avaliacao_externa
                            AND au.id_unidade_administrativa = ua.id_unidade_administrativa
                            AND au.id_unidade_administrativa = @idUnidadeAdministrativa
                            AND au.id_avaliacao_externa = @idAvaliacaoExterna");

            return sql.ToString();
        }

        protected override string QueryListObjects()
        {
            throw new NotImplementedException();
        }

        private string QueryList(FiltroAvaliacaoExternaUnidadeAdmin filtro)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"SELECT au.id_avaliacao_externa, 
                                ae.des_avaliacao_externa,
	                            au.id_unidade_administrativa, 
                                ua.des_unidade_administrativa,
                                au.nm_perc_participacao,
                                au.nm_perc_participacao_diurno,								 
								au.nm_perc_participacao_noturno,
	                            au.id_ano_referencia 
                           FROM rv_avaliacao_externa_unidadm au, 
                                rv_avaliacao_externa ae, 
                                rv_unidade_administrativa ua
                          WHERE id_ano_referencia = @ciclo
                            AND au.id_avaliacao_externa = ae.id_avaliacao_externa
                            AND au.id_unidade_administrativa = ua.id_unidade_administrativa");

            if (filtro.IdUnidadeAdministrativa != null)
                sql.Append(" AND au.id_unidade_administrativa = @idUnidadeAdministrativa");

            if (filtro.IdAvaliacaoExterna != null)
                sql.Append(" AND au.id_avaliacao_externa = @idAvaliacaoExterna");

            sql.Append(" ORDER BY ae.des_avaliacao_externa, ua.des_unidade_administrativa");

            return sql.ToString();
        }

        private string QueryExiste()
        {
            return @"SELECT count(*)
                       FROM rv_avaliacao_externa_unidadm
                      WHERE id_unidade_administrativa = @idUnidadeAdministrativa
                        AND id_avaliacao_externa = @idAvaliacaoExterna
                        AND id_ano_referencia = @ciclo";
        }

        private string QueryInsert()
        {
            return @"INSERT INTO rv_avaliacao_externa_unidadm
                               ( id_avaliacao_externa,
                                 id_unidade_administrativa,
                                 id_ano_referencia,
                                 nm_perc_participacao,
								 nm_perc_participacao_diurno,
								 nm_perc_participacao_noturno)
                        VALUES ( @idAvaliacaoExterna,
                                 @idUnidadeAdministrativa,
                                 @ciclo,
                                 @participacao,
								 @participacaoDiurna,
								 @participacaoNoturna)";
        }

        private string QueryUpdate()
        {
            return @"UPDATE rv_avaliacao_externa_unidadm
                        SET nm_perc_participacao = @participacao
                      WHERE id_unidade_administrativa = @idUnidadeAdministrativa
                        AND id_avaliacao_externa = @idAvaliacaoExterna
                        AND id_ano_referencia = @ciclo";
        }

        private string QueryDelete()
        {
            return @"DELETE FROM rv_avaliacao_externa_unidadm
                           WHERE id_unidade_administrativa = @idUnidadeAdministrativa
                             AND id_avaliacao_externa = @idAvaliacaoExterna
                             AND id_ano_referencia = @ciclo";
        }

        public override AvaliacaoExternaUnidadeAdmin LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            AvaliacaoExternaUnidadeAdmin avaliacaoExternaUnidadeAdmin = new AvaliacaoExternaUnidadeAdmin();

            avaliacaoExternaUnidadeAdmin.AvaliacaoExterna = new AvaliacaoExterna();
            avaliacaoExternaUnidadeAdmin.AvaliacaoExterna.IdAvaliacaoExterna = Convert.ToInt32(reader["id_avaliacao_externa"]);
            avaliacaoExternaUnidadeAdmin.AvaliacaoExterna.DesAvaliacaoExterna = reader["des_avaliacao_externa"].ToString();

            avaliacaoExternaUnidadeAdmin.UnidadeAdministrativa = new UnidadeAdministrativa();
            avaliacaoExternaUnidadeAdmin.UnidadeAdministrativa.IdUnidadeAdministrativa = Convert.ToInt32(reader["id_unidade_administrativa"]);
            avaliacaoExternaUnidadeAdmin.UnidadeAdministrativa.DesUnidadeAdministrativa = reader["des_unidade_administrativa"].ToString();

            avaliacaoExternaUnidadeAdmin.AnoReferencia = new AnoReferencia();
            avaliacaoExternaUnidadeAdmin.AnoReferencia.IdAnoReferencia = Convert.ToInt32(reader["id_ano_referencia"]);

			avaliacaoExternaUnidadeAdmin.PercParticipacao = (decimal)reader["nm_perc_participacao"];

			if (reader["nm_perc_participacao_diurno"] != DBNull.Value) 
				avaliacaoExternaUnidadeAdmin.PercParticipacaoDiurno = (decimal?)reader["nm_perc_participacao_diurno"];

			if (reader["nm_perc_participacao_noturno"] != DBNull.Value) 
				avaliacaoExternaUnidadeAdmin.PercParticipacaoNoturno = (decimal?)reader["nm_perc_participacao_noturno"];

            return avaliacaoExternaUnidadeAdmin;
        }

        public AvaliacaoExternaUnidadeAdmin Find(int IdAvaliacaoExterna, int idUnidadeAdministrativa, int ciclo)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idAvaliacaoExterna", IdAvaliacaoExterna);
            param.Add("idUnidadeAdministrativa", idUnidadeAdministrativa);
            param.Add("ciclo", ciclo);

            return FindObject(QueryFindObject(), param, LoadObject);
        }

        public Paging<AvaliacaoExternaUnidadeAdmin> List(FiltroAvaliacaoExternaUnidadeAdmin filtro, int ciclo, int currentPage, int pageSize)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            if (filtro.IdAvaliacaoExterna != null)
                param.Add("idAvaliacaoExterna", filtro.IdAvaliacaoExterna);

            if (filtro.IdUnidadeAdministrativa != null)
                param.Add("idUnidadeAdministrativa", filtro.IdUnidadeAdministrativa);

            param.Add("ciclo", ciclo);

            return ListPagingObjects(QueryList(filtro), param, LoadObject, currentPage, pageSize);
        }

        public void Insert(AvaliacaoExternaUnidadeAdmin AvaliacaoExternaUnidadeAdmin)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idAvaliacaoExterna", AvaliacaoExternaUnidadeAdmin.AvaliacaoExterna.IdAvaliacaoExterna);
            param.Add("idUnidadeAdministrativa", AvaliacaoExternaUnidadeAdmin.UnidadeAdministrativa.IdUnidadeAdministrativa);
            param.Add("ciclo", AvaliacaoExternaUnidadeAdmin.AnoReferencia.IdAnoReferencia);
            param.Add("participacao", AvaliacaoExternaUnidadeAdmin.PercParticipacao);
			param.Add("participacaoDiurna", AvaliacaoExternaUnidadeAdmin.PercParticipacaoDiurno);
			param.Add("participacaoNoturna", AvaliacaoExternaUnidadeAdmin.PercParticipacaoNoturno);

            InsertObject(QueryInsert(), param);
        }

        public void Update(AvaliacaoExternaUnidadeAdmin AvaliacaoExternaUnidadeAdmin)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idAvaliacaoExterna", AvaliacaoExternaUnidadeAdmin.AvaliacaoExterna.IdAvaliacaoExterna);
            param.Add("idUnidadeAdministrativa", AvaliacaoExternaUnidadeAdmin.UnidadeAdministrativa.IdUnidadeAdministrativa);
            param.Add("ciclo", AvaliacaoExternaUnidadeAdmin.AnoReferencia.IdAnoReferencia);
            param.Add("participacao", AvaliacaoExternaUnidadeAdmin.PercParticipacao);

            UpdateObject(QueryUpdate(), param);
        }

        public void Delete(int IdAvaliacaoExterna, int idUnidadeAdministrativa, int ciclo)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idAvaliacaoExterna", IdAvaliacaoExterna);
            param.Add("idUnidadeAdministrativa", idUnidadeAdministrativa);
            param.Add("ciclo", ciclo);

            DeleteObject(QueryDelete(), param);
        }

        public bool FindExiste(int IdAvaliacaoExterna, int idUnidadeAdministrativa, int ciclo)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            
            param.Add("idAvaliacaoExterna", IdAvaliacaoExterna);
            param.Add("idUnidadeAdministrativa", idUnidadeAdministrativa);
            param.Add("ciclo", ciclo);

            short count = Convert.ToInt16(ExecuteScalarQuery(QueryExiste(), param));

            return count > 0 ? true : false;
        }

		private string QueryDeleteAll()
		{
			return @"DELETE FROM rv_avaliacao_externa_unidadm WHERE id_ano_referencia = @idAnoReferencia";
		}

		public void DeleteAll(int idAnoReferencia)
		{
			Dictionary<string, object> param = new Dictionary<string, object>();

			param.Add("idAnoReferencia", idAnoReferencia);

			DeleteObject(QueryDeleteAll(), param);
		}
    }
}
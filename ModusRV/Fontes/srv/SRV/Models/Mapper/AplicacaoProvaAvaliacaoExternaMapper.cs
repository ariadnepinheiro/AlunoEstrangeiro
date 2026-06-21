using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using SRV.Models.DTO;
using System.Text;

namespace SRV.Models.Mapper
{
    public class AplicacaoProvaAvaliacaoExternaMapper : BaseMapper<AplicacaoProvaAvaliacaoExterna>
    {
		private string QueryList(FiltroAplicacaoProvaAvaliacaoExterna filtro)
		{
			StringBuilder sql = new StringBuilder();

			sql.Append(@"SELECT 
                            s.id_servidor, s.des_nome_servidor
                        FROM [rv_aplicacao_prova_avaliacao_externa] ap
                            INNER JOIN [rv_servidor] s 
                                ON ap.id_servidor = s.id_servidor");

			if (filtro.IdServidor != null)
			{
				sql.Append(" WHERE ap.id_servidor = @idServidor");

				if (filtro.DesNomeServidor != null)
					sql.Append(" AND s.des_nome_servidor LIKE @desNomeServidor");
			}
			else if (filtro.DesNomeServidor != null)
			{
				sql.Append(" WHERE s.des_nome_servidor LIKE @desNomeServidor");
			}

			return sql.ToString();
		}

		private string QueryInsert()
		{
			return @"INSERT INTO rv_aplicacao_prova_avaliacao_externa(
						id_ano_referencia, 
						id_unidade_administrativa, 
						id_servidor, 
						id_avaliacao_externa,
						des_turma, 
						nm_periodo,
						dt_prova
					)
					VALUES (
						@idAnoReferencia, 
						@idUnidadeAdministrativa, 
						@idServidor, 
						@idAvaliacaoExterna,
						@desTurma, 
						@nmPeriodo,
						@dtProva
					)";
		}

		private string QueryDelete()
		{
            return @"DELETE FROM rv_aplicacao_prova_avaliacao_externa WHERE id_servidor = @idServidor";
		}

        private string QueryDeleteAllRows()
        {
            return @"DELETE FROM rv_aplicacao_prova_avaliacao_externa";
        }

        protected override string QueryFindObject()
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"SELECT 
                            s.id_servidor, s.des_nome_servidor
                        FROM [rv_aplicacao_prova_avaliacao_externa] ap
                            INNER JOIN [rv_servidor] s 
                                ON ap.id_servidor = s.id_servidor
                        WHERE s.des_nome_servidor LIKE @desNomeServidor
                            AND s.id_servidor = @idServidor");

            return sql.ToString();
        }

        private string QueryFindServidor()
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"SELECT 
                            s.id_servidor, s.des_nome_servidor
                        FROM [rv_aplicacao_prova_avaliacao_externa] ap
                            INNER JOIN [rv_servidor] s 
                                ON ap.id_servidor = s.id_servidor
                        WHERE s.id_servidor = @idServidor");

            return sql.ToString();
        }

        protected override string QueryListObjects()
        {
            throw new NotImplementedException();
        }

        private string QueryListObjectsByAnoServidorUnidade()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"SELECT apae.id_rv_aplicacao_prova_avaliacao_externa, apae.id_ano_referencia, 
                           apae.id_unidade_administrativa, apae.id_servidor, apae.id_avaliacao_externa, 
                           ua.des_unidade_administrativa, ae.des_avaliacao_externa, 
                           s.des_nome_servidor, apae.des_turma, apae.nm_periodo, apae.dt_prova
                         FROM rv_aplicacao_prova_avaliacao_externa apae
                            INNER JOIN rv_unidade_administrativa ua ON
                                ua.id_unidade_administrativa = apae.id_unidade_administrativa
                            INNER JOIN rv_avaliacao_externa ae ON
                                ae.id_avaliacao_externa = apae.id_avaliacao_externa
                            INNER JOIN rv_servidor s ON
                                s.id_servidor = apae.id_servidor
                         WHERE
                            apae.id_ano_referencia = @idAnoReferencia AND 
                            apae.id_servidor = @idServidor AND 
                            apae.id_unidade_administrativa = @idUnidade");

            return sql.ToString();
        }

        public override AplicacaoProvaAvaliacaoExterna LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            AplicacaoProvaAvaliacaoExterna aplicacaoProvaAvaliacaoExterna = new AplicacaoProvaAvaliacaoExterna();

            aplicacaoProvaAvaliacaoExterna.Servidor = new Servidor();
            aplicacaoProvaAvaliacaoExterna.Servidor.IdServidor = Convert.ToInt32(reader["id_servidor"]);
            aplicacaoProvaAvaliacaoExterna.Servidor.DesNomeServidor = reader["des_nome_servidor"].ToString();

            return aplicacaoProvaAvaliacaoExterna;
        }

        public AplicacaoProvaAvaliacaoExterna LoadObjectByAnoServidorUnidade(System.Data.SqlClient.SqlDataReader reader)
        {
            AplicacaoProvaAvaliacaoExterna aplicacaoProvaAvaliacaoExterna = new AplicacaoProvaAvaliacaoExterna();

            aplicacaoProvaAvaliacaoExterna.IdAplicacaoProvaAvaliacaoExterna = Convert.ToInt32(reader["id_rv_aplicacao_prova_avaliacao_externa"]);
            aplicacaoProvaAvaliacaoExterna.DesTurma = reader["des_turma"].ToString();
            aplicacaoProvaAvaliacaoExterna.NmPeriodo = Convert.ToInt32(reader["nm_periodo"]);

            if (reader["dt_prova"] != DBNull.Value)
                aplicacaoProvaAvaliacaoExterna.DtProva = (DateTime)reader["dt_prova"];

            aplicacaoProvaAvaliacaoExterna.AnoReferencia = new AnoReferencia();
            aplicacaoProvaAvaliacaoExterna.AnoReferencia.IdAnoReferencia = Convert.ToInt32(reader["id_ano_referencia"]);

            aplicacaoProvaAvaliacaoExterna.UnidadeAdministrativa = new UnidadeAdministrativa();
            aplicacaoProvaAvaliacaoExterna.UnidadeAdministrativa.IdUnidadeAdministrativa = Convert.ToInt32(reader["id_unidade_administrativa"]);
            aplicacaoProvaAvaliacaoExterna.UnidadeAdministrativa.DesUnidadeAdministrativa = reader["des_unidade_administrativa"].ToString();

            aplicacaoProvaAvaliacaoExterna.Servidor = new Servidor();
            aplicacaoProvaAvaliacaoExterna.Servidor.IdServidor = Convert.ToInt32(reader["id_servidor"]);
            aplicacaoProvaAvaliacaoExterna.Servidor.DesNomeServidor = reader["des_nome_servidor"].ToString();

            aplicacaoProvaAvaliacaoExterna.AvaliacaoExterna = new AvaliacaoExterna();
            aplicacaoProvaAvaliacaoExterna.AvaliacaoExterna.IdAvaliacaoExterna = Convert.ToInt32(reader["id_avaliacao_externa"]);
            aplicacaoProvaAvaliacaoExterna.AvaliacaoExterna.DesAvaliacaoExterna = reader["des_avaliacao_externa"].ToString();

            return aplicacaoProvaAvaliacaoExterna;
        }

		public AplicacaoProvaAvaliacaoExterna Find(int idServidor)
		{
			Dictionary<string, object> param = new Dictionary<string, object>();

			param.Add("idServidor", idServidor);

            return FindObject(QueryFindServidor(), param, LoadObject);
		}

		public AplicacaoProvaAvaliacaoExterna Find(int idServidor, string desNomeServidor)
        {
			Dictionary<string, object> param = new Dictionary<string, object>();

			param.Add("idServidor", idServidor);
			param.Add("desNomeServidor", "%" + desNomeServidor + "%");

            return FindObject(QueryFindObject(), param, LoadObject);
        }

        public IList<AplicacaoProvaAvaliacaoExterna> List()
        {
            return ListObjects();
        }

        public void Insert(AplicacaoProvaAvaliacaoExterna aplicacaoProvaAvaliacaoExterna)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
			
			param.Add("idAnoReferencia", aplicacaoProvaAvaliacaoExterna.AnoReferencia.IdAnoReferencia);
            param.Add("idUnidadeAdministrativa", aplicacaoProvaAvaliacaoExterna.UnidadeAdministrativa.IdUnidadeAdministrativa);
            param.Add("idServidor", aplicacaoProvaAvaliacaoExterna.Servidor.IdServidor);
            param.Add("idAvaliacaoExterna", aplicacaoProvaAvaliacaoExterna.AvaliacaoExterna.IdAvaliacaoExterna);
            param.Add("desTurma", aplicacaoProvaAvaliacaoExterna.DesTurma);
            param.Add("nmPeriodo", aplicacaoProvaAvaliacaoExterna.NmPeriodo);
			param.Add("dtProva", aplicacaoProvaAvaliacaoExterna.DtProva);

            InsertObject(QueryInsert(), param);
        }

        public Paging<AplicacaoProvaAvaliacaoExterna> List(FiltroAplicacaoProvaAvaliacaoExterna filtro, int currentPage, int pageSize)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            if (filtro.IdServidor != null)
                param.Add("idServidor", filtro.IdServidor);

            if (filtro.DesNomeServidor != null)
                param.Add("desNomeServidor", "%" + filtro.DesNomeServidor + "%");

			return ListPagingObjects(QueryList(filtro), param, LoadObject, currentPage, pageSize);
        }

        public IList<AplicacaoProvaAvaliacaoExterna> ListBy(int idAnoReferencia, int idServidor, int idUnidade)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idAnoReferencia", idAnoReferencia);
            param.Add("idServidor", idServidor);
            param.Add("idUnidade", idUnidade);

            return ListObjects(QueryListObjectsByAnoServidorUnidade(), param, LoadObjectByAnoServidorUnidade);
        }

		public void Delete(int idServidor)
		{
			Dictionary<string, int> param = new Dictionary<string,int>();

			param.Add("idServidor", idServidor);

			DeleteObject(QueryDelete(), param);
		}

        public void DeleteAllRows()
        {
            DeleteObject(QueryDeleteAllRows(), null);
        }
    }
}
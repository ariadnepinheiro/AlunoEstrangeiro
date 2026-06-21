using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using System.Text;
using SRV.Models.DTO;

namespace SRV.Models.Mapper
{
    public class LancamentoNotaDocenteMapper: BaseMapper<LancamentoNotaDocente>
    {
        protected override string QueryFindObject()
        {
            StringBuilder sql = new StringBuilder();

			sql.Append(@"SELECT s.id_servidor, s.des_nome_servidor,ua.ID_CENSO, ua.des_unidade_administrativa 
								,lnd.nm_bimestre, lnd.des_turma, lnd.nm_periodo, lnd.des_disciplina
								,lnd.id_ano_referencia, lnd.id_unidade_administrativa, lnd.id_lancamento_nota_docente
						FROM [rv_lancamento_nota_docente] lnd
								INNER JOIN [rv_servidor] s 
									ON lnd.id_servidor = s.id_servidor
								INNER JOIN [rv_unidade_administrativa] ua
									ON lnd.id_unidade_administrativa = ua.id_unidade_administrativa
                        WHERE lnd.id_lancamento_nota_docente = @idLancamentoNotaDocente");

            return sql.ToString();
        }

        protected override string QueryListObjects()
        {
            throw new NotImplementedException();
        }

        private string QueryListObjectsByAnoServidorUnidade()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"SELECT lnd.id_lancamento_nota_docente, lnd.nm_bimestre, 
                            lnd.des_turma, nm_periodo, lnd.des_disciplina, 
                            lnd.id_ano_referencia, lnd.id_unidade_administrativa, lnd.id_servidor,
                            ua.des_unidade_administrativa, s.des_nome_servidor
                         FROM rv_lancamento_nota_docente lnd
                            INNER JOIN rv_unidade_administrativa ua ON
                                ua.id_unidade_administrativa = lnd.id_unidade_administrativa
                            INNER JOIN rv_servidor s ON
                                s.id_servidor = lnd.id_servidor
                         WHERE
                            lnd.id_ano_referencia = @idAnoReferencia AND 
                            lnd.id_servidor = @idServidor AND 
                            lnd.id_unidade_administrativa = @idUnidade");

            return sql.ToString();
        }

        public LancamentoNotaDocente LoadObjectByAnoServidorUnidade(System.Data.SqlClient.SqlDataReader reader)
        {
            LancamentoNotaDocente lancamentoNotaDocente = new LancamentoNotaDocente();

            lancamentoNotaDocente.IdLancamentoNotaDocente = Convert.ToInt32(reader["id_lancamento_nota_docente"]);
            lancamentoNotaDocente.NmBimestre = Convert.ToInt32(reader["nm_bimestre"]);
            lancamentoNotaDocente.DesTurma = reader["des_turma"].ToString();
            lancamentoNotaDocente.NmPeriodo = Convert.ToInt32(reader["nm_periodo"]);
            lancamentoNotaDocente.DesDisciplina = reader["des_disciplina"].ToString();

            lancamentoNotaDocente.AnoReferencia = new AnoReferencia();
            lancamentoNotaDocente.AnoReferencia.IdAnoReferencia = Convert.ToInt32(reader["id_ano_referencia"]);

            lancamentoNotaDocente.UnidadeAdministrativa = new UnidadeAdministrativa();
            lancamentoNotaDocente.UnidadeAdministrativa.IdUnidadeAdministrativa = Convert.ToInt32(reader["id_unidade_administrativa"]);
            lancamentoNotaDocente.UnidadeAdministrativa.DesUnidadeAdministrativa = reader["des_unidade_administrativa"].ToString();

            lancamentoNotaDocente.Servidor = new Servidor();
            lancamentoNotaDocente.Servidor.IdServidor = Convert.ToInt32(reader["id_servidor"]);
            lancamentoNotaDocente.Servidor.DesNomeServidor = reader["des_nome_servidor"].ToString();

            return lancamentoNotaDocente;
        }

        public override LancamentoNotaDocente LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
			LancamentoNotaDocente lancamentoNotaDocente = new LancamentoNotaDocente();
			lancamentoNotaDocente.Servidor = new Servidor();
			lancamentoNotaDocente.Servidor.IdServidor = Convert.ToInt32(reader["id_servidor"]);
			lancamentoNotaDocente.Servidor.DesNomeServidor = reader["des_nome_servidor"].ToString();

			lancamentoNotaDocente.AnoReferencia = new AnoReferencia();
			lancamentoNotaDocente.AnoReferencia.IdAnoReferencia = (int)reader["id_ano_referencia"];

			lancamentoNotaDocente.UnidadeAdministrativa = new UnidadeAdministrativa();
			lancamentoNotaDocente.UnidadeAdministrativa.IdCenso = (string)reader["id_censo"];
			lancamentoNotaDocente.UnidadeAdministrativa.DesUnidadeAdministrativa = (string)reader["des_unidade_administrativa"];

			lancamentoNotaDocente.IdLancamentoNotaDocente = (int)reader["id_lancamento_nota_docente"];
			lancamentoNotaDocente.DesDisciplina = (string)reader["des_disciplina"];
			lancamentoNotaDocente.DesTurma = (string)reader["des_turma"];
			lancamentoNotaDocente.NmBimestre = (int)reader["nm_bimestre"];
			lancamentoNotaDocente.NmPeriodo = (int)reader["nm_periodo"];

            return lancamentoNotaDocente;
        }

		public LancamentoNotaDocente LoadObjectServidor(System.Data.SqlClient.SqlDataReader reader)
		{
			LancamentoNotaDocente lancamentoNotaDocente = new LancamentoNotaDocente();

			lancamentoNotaDocente.Servidor = new Servidor();
			lancamentoNotaDocente.Servidor.IdServidor = Convert.ToInt32(reader["id_servidor"]);
			lancamentoNotaDocente.Servidor.DesNomeServidor = reader["des_nome_servidor"].ToString();

			return lancamentoNotaDocente;
		}

        private string QueryList(FiltroLancamentoNotaDocente filtro)
        {
            StringBuilder sql = new StringBuilder();

			sql.Append(@"SELECT s.id_servidor, s.des_nome_servidor,ua.ID_CENSO, ua.des_unidade_administrativa 
								,lnd.nm_bimestre, lnd.des_turma, lnd.nm_periodo, lnd.des_disciplina
								,lnd.id_ano_referencia, lnd.id_unidade_administrativa, lnd.id_lancamento_nota_docente
						FROM [rv_lancamento_nota_docente] lnd
								INNER JOIN [rv_servidor] s 
									ON lnd.id_servidor = s.id_servidor
								INNER JOIN [rv_unidade_administrativa] ua
									ON lnd.id_unidade_administrativa = ua.id_unidade_administrativa	
						WHERE lnd.id_ano_referencia = @idAnoReferencia");

            if (filtro.IdServidor != null)
            {
                sql.Append(" AND lnd.id_servidor = @idServidor");
			}

			if (filtro.DesNomeServidor != null)
			{
				sql.Append(" AND UPPER(s.des_nome_servidor) LIKE @desNomeServidor");
            }

            return sql.ToString();
        }

        private string QueryFindServidor()
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"SELECT 
                            s.id_servidor, s.des_nome_servidor
                        FROM [rv_lancamento_nota_docente] ln
                            INNER JOIN [rv_servidor] s 
                                ON ln.id_servidor = s.id_servidor
                        WHERE ln.id_servidor = @idServidor");

            return sql.ToString();
        }

        public IList<LancamentoNotaDocente> List()
        {
            return ListObjects();
        }

        public Paging<LancamentoNotaDocente> List(FiltroLancamentoNotaDocente filtro, int currentPage, int pageSize)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

			param.Add("idAnoReferencia", filtro.IdAnoReferencia);

            if (filtro.IdServidor != null)
                param.Add("idServidor", filtro.IdServidor);

			if (filtro.DesNomeServidor != null)
				param.Add("desNomeServidor", String.Concat("%", filtro.DesNomeServidor.ToUpper(), "%"));

            return ListPagingObjects(QueryList(filtro), param, LoadObject, currentPage, pageSize);
        }

        public IList<LancamentoNotaDocente> ListBy(int idAnoReferencia, int idServidor, int idUnidade)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idAnoReferencia", idAnoReferencia);
            param.Add("idServidor", idServidor);
            param.Add("idUnidade", idUnidade);

            return ListObjects(QueryListObjectsByAnoServidorUnidade(), param, LoadObjectByAnoServidorUnidade);
        }

		public LancamentoNotaDocente Find(int IdLancamentoNotaDocente)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

			param.Add("IdLancamentoNotaDocente", IdLancamentoNotaDocente);

            return FindObject(QueryFindObject(), param, LoadObject);
        }

		public LancamentoNotaDocente FindBy(int IdServidor)
		{
			Dictionary<string, object> param = new Dictionary<string, object>();

			param.Add("IdServidor", IdServidor);

			return FindObject(QueryFindServidor(), param, LoadObjectServidor);
		}

        private string QueryInsert()
        {
            return @"INSERT INTO rv_lancamento_nota_docente(
						id_ano_referencia, 
						id_unidade_administrativa, 
						id_servidor, 
						nm_bimestre,
						des_turma,
                        nm_periodo,
                        des_disciplina
					)
					VALUES (
						@idAnoReferencia, 
						@idUnidadeAdministrativa, 
						@idServidor, 
						@nmBimestre,
						@desTurma,
						@nmPeriodo,
						@desDisciplina
					)";
        }

        private string QueryDelete()
        {
			return @"DELETE FROM rv_lancamento_nota_docente WHERE id_lancamento_nota_docente = @IdLancamentoNotaDocente";
        }

        private string QueryDeleteAllRows()
        {
            return @"DELETE FROM rv_lancamento_nota_docente";
        }

        public void Insert(LancamentoNotaDocente lancamentoNotaDocente)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idAnoReferencia", lancamentoNotaDocente.AnoReferencia.IdAnoReferencia);
            param.Add("idUnidadeAdministrativa", lancamentoNotaDocente.UnidadeAdministrativa.IdUnidadeAdministrativa);
            param.Add("idServidor", lancamentoNotaDocente.Servidor.IdServidor);
            param.Add("nmBimestre", lancamentoNotaDocente.NmBimestre);
            param.Add("desTurma", lancamentoNotaDocente.DesTurma);
            param.Add("nmPeriodo", lancamentoNotaDocente.NmPeriodo);
            param.Add("desDisciplina", lancamentoNotaDocente.DesDisciplina);

            InsertObject(QueryInsert(), param);
        }

		public void Delete(int IdLancamentoNotaDocente)
        {
            Dictionary<string, int> param = new Dictionary<string, int>();

			param.Add("IdLancamentoNotaDocente", IdLancamentoNotaDocente);

            DeleteObject(QueryDelete(), param);
        }

        public void DeleteAllRows()
        {
            DeleteObject(QueryDeleteAllRows(), null);
        }
    }
}
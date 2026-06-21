using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using SRV.Models.DTO;
using System.Text;

namespace SRV.Models.Mapper
{
    public class NivelEnsinoUnidadeAdministrativaMapper : BaseMapper<NivelEnsinoUnidadeAdministrativa>
    {
        protected override string QueryFindObject()
        {
            return @"SELECT id_unidade_administrativa, id_modalidade,
                            id_nivel_ensino, id_ano_referencia
                       FROM rv_nivel_ensino_unidadm
                      WHERE id_unidade_administrativa = @idUnidadeAdministrativa
                        AND id_modalidade = @idModalidade
                        AND id_nivel_ensino = @idNivelEnsino
                        AND id_ano_referencia = @idAnoReferencia";
        }

        protected override string QueryListObjects()
        {
            throw new NotImplementedException();
        }

        private string QueryList(FiltroNivelEnsinoUnidadeAdministrativa filtro)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"SELECT ua.id_unidade_administrativa, ua.des_unidade_administrativa,
                                m.id_modalidade, m.des_modalidade,
                                ne.id_nivel_ensino, ne.des_nivel_ensino,
                                ar.id_ano_referencia
                           FROM rv_nivel_ensino_unidadm neua,
		                        rv_nivel_ensino ne,
                                rv_unidade_administrativa ua,
                                rv_modalidade m,
                                rv_ano_referencia ar
                          WHERE neua.id_unidade_administrativa = ua.id_unidade_administrativa
                            AND neua.id_modalidade = m.id_modalidade
                            AND neua.id_nivel_ensino = ne.id_nivel_ensino
                            AND ne.id_modalidade = m.id_modalidade
                            AND neua.id_ano_referencia = ar.id_ano_referencia");

            if (filtro.IdUnidadeAdministrativa != null)
                sql.Append(" AND ua.id_unidade_administrativa = @idUnidadeAdm");

            if (filtro.IdModalidade != null)
                sql.Append(" AND m.id_modalidade = @idModalidade");

            if (filtro.IdNivelEnsino != null)
                sql.Append(" AND ne.id_nivel_ensino = @idNivelEnsino");

            sql.Append(@"   AND ar.id_ano_referencia = @idAnoReferencia
                          ORDER BY ua.des_unidade_administrativa, ne.des_nivel_ensino, m.des_modalidade, id_ano_referencia");

            return sql.ToString();
        }

        private string QueryInsert()
        {
            return @"INSERT INTO rv_nivel_ensino_unidadm
                                   (id_unidade_administrativa,
                                    id_modalidade,
                                    id_nivel_ensino,
                                    id_ano_referencia)
                            VALUES (@idUnidadeAdministrativa,
                                    @idModalidade,
                                    @idNivelEnsino,
                                    @idAnoReferencia)";
        }

        private string QueryDelete()
        {
            return @"DELETE FROM rv_nivel_ensino_unidadm
                           WHERE id_unidade_administrativa = @idUnidadeAdministrativa
                             AND id_modalidade = @idModalidade
                             AND id_nivel_ensino = @idNivelEnsino
                             AND id_ano_referencia = @idAnoReferencia";
        }

        private string QueryExisteNivelEnsinoUnidadeAdministrativa()
        {
            return @"SELECT COUNT(*)
                       FROM rv_nivel_ensino_unidadm
                      WHERE id_unidade_administrativa = @idUnidadeAdministrativa
                        AND id_modalidade = @idModalidade
                        AND id_nivel_ensino = @idNivelEnsino
                        AND id_ano_referencia = @idAnoReferencia";
        }

        public override NivelEnsinoUnidadeAdministrativa LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            NivelEnsinoUnidadeAdministrativa nivelEnsinoUnidadeAdm = new NivelEnsinoUnidadeAdministrativa();

            nivelEnsinoUnidadeAdm.UnidadeAdministrativa = new UnidadeAdministrativa();
            nivelEnsinoUnidadeAdm.UnidadeAdministrativa.IdUnidadeAdministrativa = Convert.ToInt32(reader["id_unidade_administrativa"]);
            nivelEnsinoUnidadeAdm.UnidadeAdministrativa.DesUnidadeAdministrativa = (string)reader["des_unidade_administrativa"];

            nivelEnsinoUnidadeAdm.Modalidade = new Modalidade();
            nivelEnsinoUnidadeAdm.Modalidade.IdModalidade = (int)reader["id_modalidade"];
            nivelEnsinoUnidadeAdm.Modalidade.DesModalidade = (string)reader["des_modalidade"];

            nivelEnsinoUnidadeAdm.NivelEnsino = new NivelEnsino();
            nivelEnsinoUnidadeAdm.NivelEnsino.IdNivelEnsino = (int)reader["id_nivel_ensino"];
            nivelEnsinoUnidadeAdm.NivelEnsino.DesNivelEnsino = (string)reader["des_nivel_ensino"];

            nivelEnsinoUnidadeAdm.AnoReferencia = new AnoReferencia();
            nivelEnsinoUnidadeAdm.AnoReferencia.IdAnoReferencia = (int)reader["id_ano_referencia"];

            return nivelEnsinoUnidadeAdm;
        }

        public NivelEnsinoUnidadeAdministrativa LoadObjectSimple(System.Data.SqlClient.SqlDataReader reader)
        {
            NivelEnsinoUnidadeAdministrativa nivelEnsinoUnidadeAdm = new NivelEnsinoUnidadeAdministrativa();

            nivelEnsinoUnidadeAdm.UnidadeAdministrativa = new UnidadeAdministrativa();
            nivelEnsinoUnidadeAdm.UnidadeAdministrativa.IdUnidadeAdministrativa = Convert.ToInt32(reader["id_unidade_administrativa"]);

            nivelEnsinoUnidadeAdm.Modalidade = new Modalidade();
            nivelEnsinoUnidadeAdm.Modalidade.IdModalidade = (int)reader["id_modalidade"];

            nivelEnsinoUnidadeAdm.NivelEnsino = new NivelEnsino();
            nivelEnsinoUnidadeAdm.NivelEnsino.IdNivelEnsino = (int)reader["id_nivel_ensino"];

            nivelEnsinoUnidadeAdm.AnoReferencia = new AnoReferencia();
            nivelEnsinoUnidadeAdm.AnoReferencia.IdAnoReferencia = (int)reader["id_ano_referencia"];

            return nivelEnsinoUnidadeAdm;
        }

        public NivelEnsinoUnidadeAdministrativa Find(int idUnidadeAdministrativa, int idModalidade, int idNivelEnsino, int idAnoReferencia)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idUnidadeAdministrativa", idUnidadeAdministrativa);
            param.Add("idModalidade", idModalidade);
            param.Add("idNivelEnsino", idNivelEnsino);
            param.Add("idAnoReferencia", idAnoReferencia);

            return FindObject(QueryFindObject(), param, LoadObjectSimple);
        }

        public Paging<NivelEnsinoUnidadeAdministrativa> List(FiltroNivelEnsinoUnidadeAdministrativa filtro, int currentPage, int pageSize)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            if (filtro.IdNivelEnsino != null)
                param.Add("idNivelEnsino", filtro.IdNivelEnsino);

            if (filtro.IdUnidadeAdministrativa != null)
                param.Add("idUnidadeAdm", filtro.IdUnidadeAdministrativa);

            if (filtro.IdModalidade != null)
                param.Add("idModalidade", filtro.IdModalidade);

            param.Add("idAnoReferencia", filtro.IdAnoReferencia);

            return ListPagingObjects(QueryList(filtro), param, currentPage, pageSize);
        }

        public void Insert(NivelEnsinoUnidadeAdministrativa nivelEnsinoUnidadeAdministrativa)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idUnidadeAdministrativa", nivelEnsinoUnidadeAdministrativa.UnidadeAdministrativa.IdUnidadeAdministrativa);
            param.Add("idModalidade", nivelEnsinoUnidadeAdministrativa.Modalidade.IdModalidade);
            param.Add("idNivelEnsino", nivelEnsinoUnidadeAdministrativa.NivelEnsino.IdNivelEnsino);
            param.Add("idAnoReferencia", nivelEnsinoUnidadeAdministrativa.AnoReferencia.IdAnoReferencia);

            InsertObject(QueryInsert(), param);
        }

        public void Delete(int idUnidadeAdministrativa, int idModalidade, int idNivelEnsino, int idAnoReferencia)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idUnidadeAdministrativa", idUnidadeAdministrativa);
            param.Add("idModalidade", idModalidade);
            param.Add("idNivelEnsino", idNivelEnsino);
            param.Add("idAnoReferencia", idAnoReferencia);

            DeleteObject(QueryDelete(), param);
        }

        public bool ExisteNivelEnsinoUnidadeAdministrativa(int idUnidadeAdministrativa, int idModalidade, int idNivelEnsino, int idAnoReferencia)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idUnidadeAdministrativa", idUnidadeAdministrativa);
            param.Add("idModalidade", idModalidade);
            param.Add("idNivelEnsino", idNivelEnsino);
            param.Add("idAnoReferencia", idAnoReferencia);

            short cont = Convert.ToInt16(ExecuteScalarQuery(QueryExisteNivelEnsinoUnidadeAdministrativa(), param));

            return cont > 0 ? true : false;
        }
    }
}
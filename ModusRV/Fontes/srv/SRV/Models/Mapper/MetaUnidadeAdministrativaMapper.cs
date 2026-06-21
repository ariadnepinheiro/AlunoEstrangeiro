using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using System.Text;
using SRV.Models.DTO;

namespace SRV.Models.Mapper
{
    public class MetaUnidadeAdministrativaMapper : BaseMapper<MetaUnidadeAdministrativa>
    {
        protected override string QueryFindObject()
        {
            return @"SELECT id_unidade_administrativa, id_nivel_ensino,
                            id_indicador, id_modalidade,
                            id_ano_referencia, nm_valor_meta
                       FROM rv_meta_unidadm
                      WHERE id_unidade_administrativa = @idUnidadeAdministrativa
                        AND id_nivel_ensino = @idNivelEnsino
                        AND id_ano_referencia = @idAnoReferencia
                        AND id_indicador = @idIndicador
                        AND id_modalidade = @idModalidade";
        }

        protected override string QueryListObjects()
        {
            throw new NotImplementedException();
        }

        private string QueryList(FiltroMetaUnidadeAdministrativa filtro)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"SELECT ua.id_unidade_administrativa, ua.des_unidade_administrativa,
                                ne.id_nivel_ensino, ne.des_nivel_ensino,
                                i.id_indicador, i.des_indicador,
                                m.id_modalidade, m.des_modalidade,
                                mua.id_ano_referencia, mua.nm_valor_meta
                           FROM rv_meta_unidadm mua,
                                rv_unidade_administrativa ua,
                                rv_nivel_ensino ne,
                                rv_indicador i,
                                rv_modalidade m
                          WHERE mua.id_unidade_administrativa = ua.id_unidade_administrativa
                            AND mua.id_nivel_ensino = ne.id_nivel_ensino
                            AND mua.id_modalidade = ne.id_modalidade
                            AND mua.id_indicador = i.id_indicador
                            AND mua.id_modalidade = m.id_modalidade");

            if (filtro.IdUnidadeAdministrativa != null)
                sql.Append(" AND ua.id_unidade_administrativa = @idUnidadeAdministrativa");

            if (filtro.IdNivelEnsino != null)
                sql.Append(" AND ne.id_nivel_ensino = @idNivelEnsino");

            if (filtro.IdIndicador != null)
                sql.Append(" AND i.id_indicador = @idIndicador");

            if (filtro.IdModalidade != null)
                sql.Append(" AND m.id_modalidade = @idModalidade");

            sql.Append(@"   AND mua.id_ano_referencia = @idAnoReferencia
                          ORDER BY ua.des_unidade_administrativa, ne.des_nivel_ensino,
                                i.des_indicador, m.des_modalidade");

            return sql.ToString();
        }

        private string QueryInsert()
        {
            return @"INSERT INTO rv_meta_unidadm
                               (id_unidade_administrativa,
                                id_nivel_ensino,
                                id_ano_referencia,
                                id_indicador,
                                id_modalidade,
                                nm_valor_meta)
                        VALUES (@idUnidadeAdministrativa,
                                @idNivelEnsino,
                                @idAnoReferencia,
                                @idIndicador,
                                @idModalidade,
                                @valorMeta)";
        }

        private string QueryUpdate()
        {
            return @"UPDATE rv_meta_unidadm
                        SET nm_valor_meta = @valorMeta
                      WHERE id_unidade_administrativa = @idUnidadeAdministrativa
                        AND id_nivel_ensino = @idNivelEnsino
                        AND id_ano_referencia = @idAnoReferencia
                        AND id_indicador = @idIndicador
                        AND id_modalidade = @idModalidade";
        }

        private string QueryDelete()
        {
            return @"DELETE FROM rv_meta_unidadm
                           WHERE id_unidade_administrativa = @idUnidadeAdministrativa
                             AND id_nivel_ensino = @idNivelEnsino
                             AND id_ano_referencia = @idAnoReferencia
                             AND id_indicador = @idIndicador
                             AND id_modalidade = @idModalidade";
        }

        private string QueryExisteMeta()
        {
            return @"SELECT COUNT(*)
                       FROM rv_meta_unidadm
                      WHERE id_unidade_administrativa = @idUnidadeAdministrativa
                        AND id_nivel_ensino = @idNivelEnsino
                        AND id_ano_referencia = @idAnoReferencia
                        AND id_indicador = @idIndicador
                        AND id_modalidade = @idModalidade";
        }

        public override MetaUnidadeAdministrativa LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            MetaUnidadeAdministrativa metaUnidadeAdministrativa = new MetaUnidadeAdministrativa();

            metaUnidadeAdministrativa.UnidadeAdministrativa = new UnidadeAdministrativa();
            metaUnidadeAdministrativa.UnidadeAdministrativa.IdUnidadeAdministrativa = Convert.ToInt32(reader["id_unidade_administrativa"]);
            metaUnidadeAdministrativa.UnidadeAdministrativa.DesUnidadeAdministrativa = (string)reader["des_unidade_administrativa"];

            metaUnidadeAdministrativa.NivelEnsino = new NivelEnsino();
            metaUnidadeAdministrativa.NivelEnsino.IdNivelEnsino = (int)reader["id_nivel_ensino"];
            metaUnidadeAdministrativa.NivelEnsino.DesNivelEnsino = (string)reader["des_nivel_ensino"];

            metaUnidadeAdministrativa.Indicador = new Indicador();
            metaUnidadeAdministrativa.Indicador.IdIndicador = (int)reader["id_indicador"];
            metaUnidadeAdministrativa.Indicador.DesIndicador = (string)reader["des_indicador"];

            metaUnidadeAdministrativa.Modalidade = new Modalidade();
            metaUnidadeAdministrativa.Modalidade.IdModalidade = (int)reader["id_modalidade"];
            metaUnidadeAdministrativa.Modalidade.DesModalidade = (string)reader["des_modalidade"];

            metaUnidadeAdministrativa.AnoReferencia = new AnoReferencia();
            metaUnidadeAdministrativa.AnoReferencia.IdAnoReferencia = (int)reader["id_ano_referencia"];

            return metaUnidadeAdministrativa;
        }

        public MetaUnidadeAdministrativa LoadObjectSimple(System.Data.SqlClient.SqlDataReader reader)
        {
            MetaUnidadeAdministrativa metaUnidadeAdministrativa = new MetaUnidadeAdministrativa();

            metaUnidadeAdministrativa.UnidadeAdministrativa = new UnidadeAdministrativa();
            metaUnidadeAdministrativa.UnidadeAdministrativa.IdUnidadeAdministrativa = Convert.ToInt32(reader["id_unidade_administrativa"]);

            metaUnidadeAdministrativa.NivelEnsino = new NivelEnsino();
            metaUnidadeAdministrativa.NivelEnsino.IdNivelEnsino = (int)reader["id_nivel_ensino"];

            metaUnidadeAdministrativa.Indicador = new Indicador();
            metaUnidadeAdministrativa.Indicador.IdIndicador = (int)reader["id_indicador"];

            metaUnidadeAdministrativa.Modalidade = new Modalidade();
            metaUnidadeAdministrativa.Modalidade.IdModalidade = (int)reader["id_modalidade"];

            metaUnidadeAdministrativa.AnoReferencia = new AnoReferencia();
            metaUnidadeAdministrativa.AnoReferencia.IdAnoReferencia = (int)reader["id_ano_referencia"];

            metaUnidadeAdministrativa.ValorMeta = (decimal)reader["nm_valor_meta"];

            return metaUnidadeAdministrativa;
        }

        public MetaUnidadeAdministrativa Find(int idUnidadeAdministrativa, int idNivelEnsino, int idIndicador, int idModalidade, int idAnoReferencia)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idUnidadeAdministrativa", idUnidadeAdministrativa);
            param.Add("idNivelEnsino", idNivelEnsino);
            param.Add("idIndicador", idIndicador);
            param.Add("idModalidade", idModalidade);
            param.Add("idAnoReferencia", idAnoReferencia);

            return FindObject(QueryFindObject(), param, LoadObjectSimple);
        }

        public Paging<MetaUnidadeAdministrativa> List(FiltroMetaUnidadeAdministrativa filtro, int currentPage, int pageSize)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            if (filtro.IdUnidadeAdministrativa != null)
                param.Add("idUnidadeAdministrativa", filtro.IdUnidadeAdministrativa);

            if (filtro.IdNivelEnsino != null)
                param.Add("idNivelEnsino", filtro.IdNivelEnsino);

            if (filtro.IdIndicador!= null)
                param.Add("idIndicador", filtro.IdIndicador);

            if (filtro.IdModalidade != null)
                param.Add("idModalidade", filtro.IdModalidade);

            param.Add("idAnoReferencia", filtro.IdAnoReferencia);

            return ListPagingObjects(QueryList(filtro), param, LoadObject, currentPage, pageSize);
        }

        public void Insert(MetaUnidadeAdministrativa metaUnidadeAdministrativa)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idUnidadeAdministrativa", metaUnidadeAdministrativa.UnidadeAdministrativa.IdUnidadeAdministrativa);
            param.Add("idNivelEnsino", metaUnidadeAdministrativa.NivelEnsino.IdNivelEnsino);
            param.Add("idIndicador", metaUnidadeAdministrativa.Indicador.IdIndicador);
            param.Add("idModalidade", metaUnidadeAdministrativa.Modalidade.IdModalidade);
            param.Add("idAnoReferencia", metaUnidadeAdministrativa.AnoReferencia.IdAnoReferencia);
            param.Add("valorMeta", metaUnidadeAdministrativa.ValorMeta);

            InsertObject(QueryInsert(), param);
        }

        public void Update(MetaUnidadeAdministrativa metaUnidadeAdministrativa)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idUnidadeAdministrativa", metaUnidadeAdministrativa.UnidadeAdministrativa.IdUnidadeAdministrativa);
            param.Add("idNivelEnsino", metaUnidadeAdministrativa.NivelEnsino.IdNivelEnsino);
            param.Add("idIndicador", metaUnidadeAdministrativa.Indicador.IdIndicador);
            param.Add("idModalidade", metaUnidadeAdministrativa.Modalidade.IdModalidade);
            param.Add("idAnoReferencia", metaUnidadeAdministrativa.AnoReferencia.IdAnoReferencia);
            param.Add("valorMeta", metaUnidadeAdministrativa.ValorMeta);

            UpdateObject(QueryUpdate(), param);
        }

        public void Delete(int idUnidadeAdministrativa, int idNivelEnsino, int idIndicador, int idModalidade, int idAnoReferencia)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idUnidadeAdministrativa", idUnidadeAdministrativa);
            param.Add("idNivelEnsino", idNivelEnsino);
            param.Add("idIndicador", idIndicador);
            param.Add("idModalidade", idModalidade);
            param.Add("idAnoReferencia", idAnoReferencia);

            DeleteObject(QueryDelete(), param);
        }

        public bool ExisteMeta(int idUnidadeAdministrativa, int idNivelEnsino, int idIndicador, int idModalidade, int idAnoReferencia)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idUnidadeAdministrativa", idUnidadeAdministrativa);
            param.Add("idNivelEnsino", idNivelEnsino);
            param.Add("idIndicador", idIndicador);
            param.Add("idModalidade", idModalidade);
            param.Add("idAnoReferencia", idAnoReferencia);

            short cont = Convert.ToInt16(ExecuteScalarQuery(QueryExisteMeta(), param));

            return cont > 0 ? true : false;
        }
    }
}
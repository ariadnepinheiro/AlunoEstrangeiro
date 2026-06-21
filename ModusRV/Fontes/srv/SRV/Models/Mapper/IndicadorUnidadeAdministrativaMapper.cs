using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using SRV.Models.DTO;
using SRV.Models.Domain;

namespace SRV.Models.Mapper
{
    public class IndicadorUnidadeAdministrativaMapper : BaseMapper<IndicadorUnidadeAdministrativa>
    {
        protected override string QueryFindObject()
        {
            return @"SELECT ua.id_unidade_administrativa, ua.des_unidade_administrativa,                                
                                ne.id_nivel_ensino, ne.des_nivel_ensino,
                                i.id_indicador, i.des_indicador,
                                m.id_modalidade, m.des_modalidade,
                                iua.nm_valor_realizado,
                                iua.id_ano_referencia
                           FROM rv_indicador_unidadm iua,
		                        rv_nivel_ensino ne,
                                rv_unidade_administrativa ua,
                                rv_modalidade m,
                                rv_indicador i
                          WHERE iua.id_unidade_administrativa = ua.id_unidade_administrativa
                            AND iua.id_modalidade = m.id_modalidade
                            AND iua.id_nivel_ensino = ne.id_nivel_ensino
                            AND iua.id_indicador = i.id_indicador
                            AND ne.id_modalidade = m.id_modalidade
                            AND ua.id_unidade_administrativa = @idUnidadeAdm
                            AND m.id_modalidade = @idModalidade
                            AND ne.id_nivel_ensino = @idNivelEnsino
                            AND i.id_indicador = @idIndicador
                            AND iua.id_ano_referencia = @idAnoReferencia";
        }

        protected override string QueryListObjects()
        {
            throw new NotImplementedException();
        }


        private string QueryList(FiltroIndicadorUnidadeAdministrativa filtro)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"SELECT ua.id_unidade_administrativa, ua.des_unidade_administrativa,                                
                                ne.id_nivel_ensino, ne.des_nivel_ensino,
                                i.id_indicador, i.des_indicador,
                                m.id_modalidade, m.des_modalidade,
                                iua.nm_valor_realizado,
                                iua.id_ano_referencia
                           FROM rv_indicador_unidadm iua,
		                        rv_nivel_ensino ne,
                                rv_unidade_administrativa ua,
                                rv_modalidade m,
                                rv_indicador i
                          WHERE iua.id_unidade_administrativa = ua.id_unidade_administrativa
                            AND iua.id_modalidade = m.id_modalidade
                            AND iua.id_nivel_ensino = ne.id_nivel_ensino
                            AND iua.id_indicador = i.id_indicador
                            AND ne.id_modalidade = m.id_modalidade");

            if (filtro.IdUnidadeAdministrativa != null)
                sql.Append(" AND ua.id_unidade_administrativa = @idUnidadeAdm");

            if (filtro.IdModalidade != null)
                sql.Append(" AND m.id_modalidade = @idModalidade");

            if (filtro.IdNivelEnsino != null)
                sql.Append(" AND ne.id_nivel_ensino = @idNivelEnsino");

            if (filtro.IdIndicador != null)
                sql.Append(" AND i.id_indicador = @idIndicador");

            sql.Append(@"   AND iua.id_ano_referencia = @idAnoReferencia
                          ORDER BY ua.des_unidade_administrativa, ne.des_nivel_ensino, i.des_indicador, m.des_modalidade");

            return sql.ToString();
        }

        private string QueryInsert()
        {
            return @"INSERT INTO rv_indicador_unidadm
                               ( id_unidade_administrativa,
                                 id_modalidade,
                                 id_nivel_ensino,
                                 id_indicador,
                                 id_ano_referencia,
                                 nm_valor_realizado)
                        VALUES ( @idUnidadeAdm,
                                 @idModalidade,
                                 @idNivelEnsino,
                                 @idIndicador,
                                 @idAnoReferencia,
                                 @valorRealizado)";
        }

        private string QueryUpdate()
        {
            return @"UPDATE rv_indicador_unidadm
                        SET nm_valor_realizado = @valorRealizado
                      WHERE id_unidade_administrativa = @idUnidadeAdm
                        AND id_modalidade = @idModalidade
                        AND id_nivel_ensino = @idNivelEnsino
                        AND id_indicador = @idIndicador
                        AND id_ano_referencia = @idAnoReferencia";
        }


        private string QueryDelete()
        {
            return @"DELETE FROM rv_indicador_unidadm
                           WHERE id_unidade_administrativa = @idUnidadeAdm
                             AND id_modalidade = @idModalidade
                             AND id_nivel_ensino = @idNivelEnsino
                             AND id_indicador = @idIndicador
                             AND id_ano_referencia = @idAnoReferencia";
        }

        protected string QueryExiste()
        {
            return @"SELECT count(*)
                           FROM rv_indicador_unidadm iua,
		                        rv_nivel_ensino ne,
                                rv_unidade_administrativa ua,
                                rv_modalidade m,
                                rv_indicador i
                          WHERE iua.id_unidade_administrativa = ua.id_unidade_administrativa
                            AND iua.id_modalidade = m.id_modalidade
                            AND iua.id_nivel_ensino = ne.id_nivel_ensino
                            AND iua.id_indicador = i.id_indicador
                            AND ne.id_modalidade = m.id_modalidade
                            AND ua.id_unidade_administrativa = @idUnidadeAdm
                            AND m.id_modalidade = @idModalidade
                            AND ne.id_nivel_ensino = @idNivelEnsino
                            AND i.id_indicador = @idIndicador
                            AND iua.id_ano_referencia = @idAnoReferencia";
        }

        public override IndicadorUnidadeAdministrativa LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            IndicadorUnidadeAdministrativa indicadorUnidadeAdministrativa = new IndicadorUnidadeAdministrativa();

            indicadorUnidadeAdministrativa.UnidadeAdministrativa = new UnidadeAdministrativa();
            indicadorUnidadeAdministrativa.UnidadeAdministrativa.IdUnidadeAdministrativa = Convert.ToInt32(reader["id_unidade_administrativa"]);
            indicadorUnidadeAdministrativa.UnidadeAdministrativa.DesUnidadeAdministrativa = (string)reader["des_unidade_administrativa"];

            indicadorUnidadeAdministrativa.Modalidade = new Modalidade();
            indicadorUnidadeAdministrativa.Modalidade.IdModalidade = (int)reader["id_modalidade"];
            indicadorUnidadeAdministrativa.Modalidade.DesModalidade = (string)reader["des_modalidade"];

            indicadorUnidadeAdministrativa.NivelEnsino = new NivelEnsino();
            indicadorUnidadeAdministrativa.NivelEnsino.IdNivelEnsino = (int)reader["id_nivel_ensino"];
            indicadorUnidadeAdministrativa.NivelEnsino.DesNivelEnsino = (string)reader["des_nivel_ensino"];

            indicadorUnidadeAdministrativa.Indicador = new Indicador();
            indicadorUnidadeAdministrativa.Indicador.IdIndicador = (int)reader["id_indicador"];
            indicadorUnidadeAdministrativa.Indicador.DesIndicador = (string)reader["des_indicador"];

            indicadorUnidadeAdministrativa.AnoReferencia = new AnoReferencia();
            indicadorUnidadeAdministrativa.AnoReferencia.IdAnoReferencia = (int)reader["id_ano_referencia"];

            indicadorUnidadeAdministrativa.ValorRealizado = (decimal)reader["nm_valor_realizado"];

            return indicadorUnidadeAdministrativa;
        }


        public Paging<IndicadorUnidadeAdministrativa> List(FiltroIndicadorUnidadeAdministrativa filtro, int ciclo, int currentPage, int pageSize)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            if (filtro.IdNivelEnsino != null)
                param.Add("idNivelEnsino", filtro.IdNivelEnsino);

            if (filtro.IdUnidadeAdministrativa != null)
                param.Add("idUnidadeAdm", filtro.IdUnidadeAdministrativa);

            if (filtro.IdModalidade != null)
                param.Add("idModalidade", filtro.IdModalidade);

            if (filtro.IdIndicador != null)
                param.Add("idIndicador", filtro.IdIndicador);

            param.Add("idAnoReferencia", ciclo);

            return ListPagingObjects(QueryList(filtro), param, currentPage, pageSize);
        }

        public IndicadorUnidadeAdministrativa Find(int idUnidadeAdministrativa, int idModalidade, int idNivelEnsino, int idIndicador, int ciclo)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idUnidadeAdm", idUnidadeAdministrativa);
            param.Add("idModalidade", idModalidade);
            param.Add("idNivelEnsino", idNivelEnsino);
            param.Add("idIndicador", idIndicador);
            param.Add("idAnoReferencia", ciclo);

            return FindObject(QueryFindObject(), param, LoadObject);
        }

        public void Insert(IndicadorUnidadeAdministrativa IndicadorUnidadeAdministrativa)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idUnidadeAdm", IndicadorUnidadeAdministrativa.UnidadeAdministrativa.IdUnidadeAdministrativa);
            param.Add("idModalidade", IndicadorUnidadeAdministrativa.Modalidade.IdModalidade);
            param.Add("idNivelEnsino", IndicadorUnidadeAdministrativa.NivelEnsino.IdNivelEnsino);
            param.Add("idIndicador", IndicadorUnidadeAdministrativa.Indicador.IdIndicador);
            param.Add("idAnoReferencia", IndicadorUnidadeAdministrativa.AnoReferencia.IdAnoReferencia);
            param.Add("valorRealizado", IndicadorUnidadeAdministrativa.ValorRealizado);

            InsertObject(QueryInsert(), param);
        }

        public void Update(IndicadorUnidadeAdministrativa IndicadorUnidadeAdministrativa)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idUnidadeAdm", IndicadorUnidadeAdministrativa.UnidadeAdministrativa.IdUnidadeAdministrativa);
            param.Add("idModalidade", IndicadorUnidadeAdministrativa.Modalidade.IdModalidade);
            param.Add("idNivelEnsino", IndicadorUnidadeAdministrativa.NivelEnsino.IdNivelEnsino);
            param.Add("idIndicador", IndicadorUnidadeAdministrativa.Indicador.IdIndicador);
            param.Add("idAnoReferencia", IndicadorUnidadeAdministrativa.AnoReferencia.IdAnoReferencia);
            param.Add("valorRealizado", IndicadorUnidadeAdministrativa.ValorRealizado);

            UpdateObject(QueryUpdate(), param);
        }

        public void Delete(int idUnidadeAdministrativa, int idModalidade, int idNivelEnsino, int idIndicador, int ciclo)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idUnidadeAdm", idUnidadeAdministrativa);
            param.Add("idModalidade", idModalidade);
            param.Add("idNivelEnsino", idNivelEnsino);
            param.Add("idIndicador", idIndicador);
            param.Add("idAnoReferencia", ciclo);

            DeleteObject(QueryDelete(), param);
        }

        public bool FindExiste(int idUnidadeAdministrativa, int idModalidade, int idNivelEnsino, int idIndicador, int ciclo)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idUnidadeAdm", idUnidadeAdministrativa);
            param.Add("idModalidade", idModalidade);
            param.Add("idNivelEnsino", idNivelEnsino);
            param.Add("idIndicador", idIndicador);
            param.Add("idAnoReferencia", ciclo);

            short count = Convert.ToInt16(ExecuteScalarQuery(QueryExiste(), param));

            return count > 0 ? true : false;
        }
    }
}
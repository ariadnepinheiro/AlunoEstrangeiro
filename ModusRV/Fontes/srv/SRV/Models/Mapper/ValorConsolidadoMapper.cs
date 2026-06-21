using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.DTO;
using System.Text;
using SRV.Common;
using SRV.Models.Domain;

namespace SRV.Models.Mapper
{
    public class ValorConsolidadoMapper : BaseMapper<ValorConsolidado>
    {
        protected override string QueryFindObject()
        {
            throw new NotImplementedException();
        }

        protected override string QueryListObjects()
        {
            throw new NotImplementedException();
        }

        private string QueryListIndicadores(FiltroRptConsolidado filtro, UserState usuario)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"SELECT u.id_unidade,
                                m.nm_valor_meta AS previsto,
                                i.nm_valor_realizado AS realizado
                       FROM (vw_unidade_administrativa u  
                            LEFT OUTER JOIN rv_meta_unidadm m 
                              ON u.id_ano_referencia = m.id_ano_referencia 
                             AND u.id_unidade = m.id_unidade_administrativa
                             AND m.id_modalidade         = @idModalidade 
                             AND m.id_indicador          = @idIndicador 
                             AND m.id_nivel_ensino       = @idNivelEnsino) 
                            LEFT OUTER JOIN rv_indicador_unidadm i 
                              ON i.id_unidade_administrativa = m.id_unidade_administrativa
                             AND i.id_modalidade     = m.id_modalidade 
                             AND i.id_indicador      = m.id_indicador 
                             AND i.id_nivel_ensino   = m.id_nivel_ensino 
                             AND i.id_ano_referencia = m.id_ano_referencia
                      WHERE u.id_ano_referencia = @idAnoReferencia");

            if (filtro.IdUnidadeAdministrativa != null)
                sql.Append("    AND (id_unidade = @idUnidade or id_unidade = @idRegional) ");
            else
            {
                sql.Append("    AND (id_unidade = @idUnidade or id_regional = @idRegional) ");

                if (usuario.Perfil == Perfil.Escola)
                    sql.Append(@" AND id_unidade IN (SELECT id_unidade_administrativa 
                                                       FROM rv_funcao_servidor 
                                                      WHERE id_servidor = @idServidor
                                                      UNION
                                                     SELECT @idRegional)");
            }

            sql.Append("  ORDER BY ordem, des_unidade");

            return sql.ToString();
        }

        private string QueryListIGE(FiltroRptConsolidado filtro, UserState usuario)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"SELECT u.id_unidade, 
                                m.nm_meta_ige AS previsto,
                                c.nm_nota_ige AS realizado
                           FROM (vw_unidade_administrativa u 
                                LEFT JOIN rv_meta_ige_unidadm m
                                  ON m.id_unidade_administrativa = u.id_unidade
                                 AND m.id_ano_referencia = u.id_ano_referencia)
                                LEFT JOIN rv_criterio_unidadm c
                                  ON c.id_unidade_administrativa = u.id_unidade
                                 AND c.id_ano_referencia = u.id_ano_referencia
                          WHERE u.id_ano_referencia = @idAnoReferencia");

            if (filtro.IdUnidadeAdministrativa != null)
                sql.Append("    AND (id_unidade = @idUnidade or id_unidade = @idRegional) ");
            else
            {
                sql.Append("    AND (id_unidade = @idUnidade or id_regional = @idRegional) ");

                if (usuario.Perfil == Perfil.Escola)
                    sql.Append(@" AND id_unidade IN (SELECT id_unidade_administrativa 
                                                       FROM rv_funcao_servidor 
                                                      WHERE id_servidor = @idServidor
                                                      UNION
                                                     SELECT @idRegional)");
            }

            sql.Append("  ORDER BY ordem, des_unidade");

            return sql.ToString();
        }

        private string QueryListAvaliacaoExterna(FiltroRptConsolidado filtro, UserState usuario)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"SELECT u.id_unidade, a.nm_perc_participacao AS percentual
                           FROM vw_unidade_administrativa u 
                               LEFT JOIN rv_avaliacao_externa_unidadm a
                                 ON a.id_unidade_administrativa = u.id_unidade
                                AND a.id_ano_referencia = u.id_ano_referencia
                                AND a.id_avaliacao_externa = @idAvaliacaoExterna
                          WHERE u.id_ano_referencia = @idAnoReferencia");

            if (filtro.IdUnidadeAdministrativa != null)
                sql.Append("    AND (id_unidade = @idUnidade or id_unidade = @idRegional) ");
            else
            {
                sql.Append("    AND (id_unidade = @idUnidade or id_regional = @idRegional) ");

                if (usuario.Perfil == Perfil.Escola)
                    sql.Append(@" AND id_unidade IN (SELECT id_unidade_administrativa 
                                                       FROM rv_funcao_servidor 
                                                      WHERE id_servidor = @idServidor
                                                      UNION
                                                     SELECT @idRegional)");
            }

            sql.Append("  ORDER BY ordem, des_unidade");

            return sql.ToString();
        }

        private string QueryListLancamentoNotas(FiltroRptConsolidado filtro, UserState usuario)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"SELECT u.id_unidade, c.nm_perc_lancamento_nota AS percentual
                           FROM vw_unidade_administrativa u 
                               LEFT JOIN rv_criterio_unidadm c
                                 ON c.id_unidade_administrativa = u.id_unidade
                                AND c.id_ano_referencia = u.id_ano_referencia
                          WHERE u.id_ano_referencia = @idAnoReferencia");

            if (filtro.IdUnidadeAdministrativa != null)
                sql.Append("    AND (id_unidade = @idUnidade or id_unidade = @idRegional) ");
            else
            {
                sql.Append("    AND (id_unidade = @idUnidade or id_regional = @idRegional) ");

                if (usuario.Perfil == Perfil.Escola)
                    sql.Append(@" AND id_unidade IN (SELECT id_unidade_administrativa 
                                                       FROM rv_funcao_servidor 
                                                      WHERE id_servidor = @idServidor
                                                      UNION
                                                     SELECT @idRegional)");
            }

            sql.Append("  ORDER BY ordem, des_unidade");

            return sql.ToString();
        }

        private string QueryListCurriculoMinimo(FiltroRptConsolidado filtro, UserState usuario)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"SELECT u.id_unidade, c.nm_perc_curriculo_minimo AS percentual
                           FROM vw_unidade_administrativa u 
                               LEFT JOIN rv_criterio_unidadm c
                                 ON c.id_unidade_administrativa = u.id_unidade
                                AND c.id_ano_referencia = u.id_ano_referencia
                          WHERE u.id_ano_referencia = @idAnoReferencia");

            if (filtro.IdUnidadeAdministrativa != null)
                sql.Append("    AND (id_unidade = @idUnidade or id_unidade = @idRegional) ");
            else
            {
                sql.Append("    AND (id_unidade = @idUnidade or id_regional = @idRegional) ");

                if (usuario.Perfil == Perfil.Escola)
                    sql.Append(@" AND id_unidade IN (SELECT id_unidade_administrativa 
                                                       FROM rv_funcao_servidor 
                                                      WHERE id_servidor = @idServidor
                                                      UNION
                                                     SELECT @idRegional)");
            }

            sql.Append("  ORDER BY ordem, des_unidade");

            return sql.ToString();
        }

        private string QueryFindCurriculoMinRegional()
        {
            return @"SELECT @idRegional AS id_unidade,
                            CAST(ISNULL(((SUM(tab.a) * 100.00) / COUNT(tab.id_unidade_administrativa)),0) AS DECIMAL(5,2)) percentual
                       FROM (SELECT CASE WHEN nm_perc_curriculo_minimo >= 
                                             (SELECT nm_valor_criterio 
                                                FROM rv_tipo_criterio_elegibilidade  
                                               WHERE id_criterio_elegibilidade = @idCriterioCurriculoMin
                                                 AND id_ano_referencia = @idAnoReferencia)
                                        THEN 1  ELSE 0  END a, ua.id_unidade_administrativa  
                              FROM rv_unidade_administrativa ua 
                              LEFT JOIN rv_criterio_unidadm cua 
                                ON (ua.id_unidade_administrativa = cua.id_unidade_administrativa)  
                             WHERE ua.id_unidade_regional = @idRegional
                               AND id_ano_referencia = @idAnoReferencia)tab";
        }


        public override ValorConsolidado LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            ValorConsolidado valor = new ValorConsolidado();

            valor.IdUnidade = Convert.ToInt32(reader["id_unidade"]);

            if (reader["previsto"] != DBNull.Value)
                valor.Previsto = (decimal)reader["previsto"];

            if (reader["realizado"] != DBNull.Value)
                valor.Realizado = (decimal)reader["realizado"];

            valor.Percentual = CalculaPercentualRealizado(valor.Previsto, valor.Realizado);

            return valor;
        }

        private ValorConsolidado LoadPercentual(System.Data.SqlClient.SqlDataReader reader)
        {
            ValorConsolidado valor = new ValorConsolidado();

            valor.IdUnidade = Convert.ToInt32(reader["id_unidade"]);

            if (reader["percentual"] != DBNull.Value)
                valor.Percentual = (decimal)reader["percentual"];

            return valor;
        }

        public IList<ValorConsolidado> ListIndicadores(FiltroRptConsolidado filtro, UserState usuario, int idModalidade, int idNivelEnsino, int idIndicador)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("idAnoReferencia", filtro.IdAnoReferencia);
            param.Add("idRegional", filtro.IdRegional);
            param.Add("idModalidade", idModalidade);
            param.Add("idNivelEnsino", idNivelEnsino);
            param.Add("idIndicador", idIndicador);

            if (filtro.IdUnidadeAdministrativa != null)
                param.Add("idUnidade", filtro.IdUnidadeAdministrativa);
            else
                param.Add("idUnidade", filtro.IdRegional);

            if (usuario.Perfil == Perfil.Escola)
                param.Add("idServidor", Convert.ToInt32(usuario.Login));

            return ListObjects(QueryListIndicadores(filtro, usuario), param);
        }

        public IList<ValorConsolidado> ListIGE(FiltroRptConsolidado filtro, UserState usuario)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("idAnoReferencia", filtro.IdAnoReferencia);
            param.Add("idRegional", filtro.IdRegional);

            if (filtro.IdUnidadeAdministrativa != null)
                param.Add("idUnidade", filtro.IdUnidadeAdministrativa);
            else
                param.Add("idUnidade", filtro.IdRegional);

            if (usuario.Perfil == Perfil.Escola)
                param.Add("idServidor", Convert.ToInt32(usuario.Login));

            return ListObjects(QueryListIGE(filtro, usuario), param);
        }

        public IList<ValorConsolidado> ListAvaliacaoExterna(FiltroRptConsolidado filtro, UserState usuario, int idAvaliacaoExterna)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("idAnoReferencia", filtro.IdAnoReferencia);
            param.Add("idRegional", filtro.IdRegional);
            param.Add("idAvaliacaoExterna", idAvaliacaoExterna);

            if (filtro.IdUnidadeAdministrativa != null)
                param.Add("idUnidade", filtro.IdUnidadeAdministrativa);
            else
                param.Add("idUnidade", filtro.IdRegional);

            if (usuario.Perfil == Perfil.Escola)
                param.Add("idServidor", Convert.ToInt32(usuario.Login));

            return ListObjects(QueryListAvaliacaoExterna(filtro, usuario), param, LoadPercentual);
        }

        public IList<ValorConsolidado> ListLancamentoNotas(FiltroRptConsolidado filtro, UserState usuario)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("idAnoReferencia", filtro.IdAnoReferencia);
            param.Add("idRegional", filtro.IdRegional);

            if (filtro.IdUnidadeAdministrativa != null)
                param.Add("idUnidade", filtro.IdUnidadeAdministrativa);
            else
                param.Add("idUnidade", filtro.IdRegional);

            if (usuario.Perfil == Perfil.Escola)
                param.Add("idServidor", Convert.ToInt32(usuario.Login));

            return ListObjects(QueryListLancamentoNotas(filtro, usuario), param, LoadPercentual);
        }

        public IList<ValorConsolidado> ListCurriculoMinimo(FiltroRptConsolidado filtro, UserState usuario)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("idAnoReferencia", filtro.IdAnoReferencia);
            param.Add("idRegional", filtro.IdRegional);

            if (filtro.IdUnidadeAdministrativa != null)
                param.Add("idUnidade", filtro.IdUnidadeAdministrativa);
            else
                param.Add("idUnidade", filtro.IdRegional);

            if (usuario.Perfil == Perfil.Escola)
                param.Add("idServidor", Convert.ToInt32(usuario.Login));

            return ListObjects(QueryListCurriculoMinimo(filtro, usuario), param, LoadPercentual);
        }

        public ValorConsolidado FindCurriculoMinRegional(FiltroRptConsolidado filtro)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("idAnoReferencia", filtro.IdAnoReferencia);
            param.Add("idRegional", filtro.IdRegional);
            param.Add("idCriterioCurriculoMin", Constants.IdCriterioCurriculoMin);

            return FindObject(QueryFindCurriculoMinRegional(), param, LoadPercentual);
        }

        private decimal? CalculaPercentualRealizado(decimal? previsto, decimal? realizado)
        {
            if (!previsto.HasValue || !realizado.HasValue)
                return null;

            if (previsto <= 0)
                return null;

            return  (realizado.Value / previsto.Value) * 100;
        }

    }
}
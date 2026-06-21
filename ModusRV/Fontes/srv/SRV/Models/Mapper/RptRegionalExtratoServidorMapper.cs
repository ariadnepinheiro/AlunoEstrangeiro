using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.DTO;
using System.Text;
using SRV.Common;

namespace SRV.Models.Mapper
{
    public class RptRegionalExtratoServidorMapper : BaseMapper<RptRegionalExtratoServidor>
    {
        protected override string QueryFindObject()
        {
            throw new NotImplementedException();
        }

        protected override string QueryListObjects()
        {
            throw new NotImplementedException();
        }

        protected string QueryListRegionalExtrato(int? idRegional)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"SELECT n.id_unidade_administrativa,
                                CASE WHEN n.id_tipo_unidadm = 1 THEN NULL ELSE n.des_unidade_administrativa END  AS nome_escola,
                                CASE WHEN n.id_tipo_unidadm = 1 THEN n.des_unidade_administrativa ELSE dbo.fc_grid_retorna_ds_unidade(n.id_unidade_regional) END AS nome_regional,
                                g.des_grupo_funcao AS funcao_bonific,
                                u.des_funcao AS funcao_atividade,
                                (ISNULL(f.nm_proporcionalidade, 0.00) * 100) AS proporcionalidade,
                                CONVERT(VARCHAR, f.dt_inicio_funcao, 103) AS dt_inicio,
                                CONVERT(VARCHAR, f.dt_fim_funcao, 103) AS dt_fim,
                                o.nm_qtd_vencimento AS coeficiente_bonificacao,
                                dbo.fc_grid_retorna_percent_alocacao(s.id_servidor, f.id_ano_referencia, n.id_unidade_administrativa, f.id_funcao_servidor) as perc_alocacao,
                                e.fl_elegivel,
                                a.nm_qtd_salario AS resultado,
                                mid.id_servidor AS servidor_ineleg
                           FROM rv_servidor s INNER JOIN rv_funcao_servidor f
                                                      ON f.id_servidor = s.id_servidor
                                              INNER JOIN rv_funcao u
                                                      ON u.id_funcao = f.id_funcao
                                              INNER JOIN rv_grupo_funcao g
                                                      ON g.id_grupo_funcao = u.id_grupo_funcao
                                              INNER JOIN rv_elegibilidade_unidadm e 
                                                      ON e.id_unidade_administrativa = f.id_unidade_administrativa
                                                     AND e.id_ano_referencia = f.id_ano_referencia
                                              INNER JOIN rv_unidade_administrativa n 
                                                      ON n.id_unidade_administrativa = f.id_unidade_administrativa
                                              LEFT JOIN rv_nota_funcao_unidadm o 
                                                      ON o.id_unidade_administrativa = f.id_unidade_administrativa 
                                                     AND o.id_ano_referencia = f.id_ano_referencia 
                                                     AND o.id_grupo_funcao = g.id_grupo_funcao  
                                              LEFT JOIN rv_alocacao_servidor a 
                                                      ON a.id_servidor = f.id_servidor 
                                                     AND a.id_unidade_administrativa = f.id_unidade_administrativa 
                                                     AND a.id_funcao_servidor = f.id_funcao_servidor 
                                                     AND a.id_ano_referencia = f.id_ano_referencia  
                                              LEFT JOIN rv_motivo_ineleg_docente mid 
                                                      ON mid.id_servidor = f.id_servidor 
                                                     AND mid.id_unidade_administrativa = f.id_unidade_administrativa 
                                                     AND mid.id_ano_referencia = f.id_ano_referencia  
                          WHERE s.id_servidor = @idServidor
                            AND f.id_ano_referencia = @ciclo
                            AND n.id_tipo_unidadm IN (" + Constants.TipoUnidAdmRegional + "," + Constants.TipoUnidAdmEscolar + "," + Constants.TipoUnidAdmRegionalPedagogica + "," + Constants.TipoUnidAdmRegionalAdministrativa + "," + Constants.TipoUnidAdmRegionalGestaoPessoas + "," + Constants.TipoUnidAdmRegionalPedagogica_Administrativa + ")");

                if (idRegional != null)
                    sql.Append(" AND n.id_unidade_regional = @idRegional");

                sql.Append(" ORDER BY f.dt_inicio_funcao");

                return sql.ToString();
        }

        public IList<RptRegionalExtratoServidor> List(int? idRegional, int MatServidor, int ciclo)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idServidor", MatServidor);
            param.Add("ciclo", ciclo);

            if (idRegional != null)
                param.Add("idRegional", idRegional);

            return ListObjects(QueryListRegionalExtrato(idRegional), param);
        }

        public override RptRegionalExtratoServidor LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            RptRegionalExtratoServidor rptRegionalExtratoServidor = new RptRegionalExtratoServidor();

            rptRegionalExtratoServidor.IdUnidadeAdministrativa = Convert.ToInt32(reader["id_unidade_administrativa"]);
            rptRegionalExtratoServidor.Regional = (string)reader["nome_regional"];
            rptRegionalExtratoServidor.Unidade = (string)reader["nome_escola"];
            rptRegionalExtratoServidor.FuncaoBonificacao = (string)reader["funcao_bonific"];
            rptRegionalExtratoServidor.FuncaoAtividade = (string)reader["funcao_atividade"];            
            rptRegionalExtratoServidor.Periodo = String.Concat((string)reader["dt_inicio"], " à ", (string)reader["dt_fim"]);

            if (reader["coeficiente_bonificacao"] != DBNull.Value)
                rptRegionalExtratoServidor.CoeficienteBonificacao = (decimal)reader["coeficiente_bonificacao"];

            if (reader["proporcionalidade"] != DBNull.Value)
                rptRegionalExtratoServidor.Proporcionalidade = (decimal)reader["proporcionalidade"];

            if (reader["perc_alocacao"] != DBNull.Value)
                rptRegionalExtratoServidor.Alocacao = (decimal)reader["perc_alocacao"];

            if (reader["resultado"] != DBNull.Value)
                rptRegionalExtratoServidor.Resultado = (decimal)reader["resultado"];

            rptRegionalExtratoServidor.Eligibilidade = reader["fl_elegivel"].ToString() == "S" ? true : false;
            rptRegionalExtratoServidor.EligibilidadeUnidadeServidor = (reader["servidor_ineleg"] == DBNull.Value);

            return rptRegionalExtratoServidor;
        }
    }
}
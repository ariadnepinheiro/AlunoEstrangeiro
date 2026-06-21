using System;
using System.Data;
using Techne.Data;
using Techne.Library;
using Techne.Lyceum.CR;
using System.Collections.Generic;
using System.Linq;

namespace Techne.Lyceum.RN
{
    using Seeduc.Infra.Data;
    using System.Data.SqlClient;

    public class Frequencia : RNBase
    {
        public static QueryTable CarregaDataAula(decimal ano, decimal semestre, string turma, string faculdade, string turno, string disciplina)
        {
            TConnection cn = Config.CreateConnection();

            QueryTable qt = new QueryTable("SELECT convert(varchar(10), dt_aula,103) as dt_aula, " +
                                "convert(varchar(10), dt_aula,103) +  isnull(' - aula: ' + convert(varchar(5),AULA),'') as descricao " +
                                "FROM LY_LISTA_FREQ " +
                                "WHERE ano = ? " +
                                "AND semestre = ? " +
                                "AND turno = ? " +
                                "AND turma = ? " +
                                "AND faculdade = ? " +
                                "AND disciplina = ?");

            DbObject[] dbobj = new DbObject[] { ano, semestre, turno, turma, faculdade, disciplina };

            qt.Query(cn, dbobj);

            return qt;
        }

        public static QueryTable ListaFrequencias(string turma, string ano, string semestre, string faculdade, string turno, string data, string disciplina, string aula)
        {
            if (string.IsNullOrEmpty(ano)) ano = "0";
            if (string.IsNullOrEmpty(semestre)) semestre = "0";

            string query = "SELECT CL.LISTA, CL.DISCIPLINA, CL.TURMA, CL.ANO, CL.SEMESTRE, PE.NOME_COMPL, CL.OBSERVACAO, " +
                                "(case cl.ocorrencia when 'Falta' then 'S' else 'N' end) FALTA, " +
                                "(case cl.ocorrencia when 'Cancelamento' then 'S' else 'N' end) CANCELAMENTO, " +
                                "(case cl.ocorrencia when 'Abonada' then 'S' else 'N' end) ABONADA " +
                                "FROM ly_comp_lista cl inner join ly_aluno a on a.aluno = cl.aluno inner join ly_lista_freq lf on cl.lista = lf.lista INNER JOIN LY_PESSOA PE (NOLOCK)  ON PE.PESSOA = A.PESSOA" +
                                "WHERE cl.ano = ? " +
                                "AND cl.semestre = ? " +
                                "AND cl.turma = ? " +
                                "AND lf.faculdade = ? " +
                                "AND lf.turno = ? ";
            if (data != "-1")
            {
                query += "AND lf.dt_aula = convert(datetime, ? ,103) ";
                if (!string.IsNullOrEmpty(aula))
                {
                    query += "AND lf.aula = ? ";
                }
            }
            if (disciplina != "-1")
                query += "AND cl.disciplina = ? ";
            query += "Order by PE.NOME_COMPL ";

            TConnection cn = Config.CreateConnection();

            QueryTable qt = new QueryTable(query);

            if (data != "-1" && !string.IsNullOrEmpty(aula) && disciplina != "-1")
                qt.Query(cn, ano, semestre, turma, faculdade, turno, data, aula, disciplina);
            else if (data != "-1" && disciplina != "-1")
                qt.Query(cn, ano, semestre, turma, faculdade, turno, data, disciplina);
            else if (data == "-1" && disciplina == "-1")
                qt.Query(cn, ano, semestre, turma, faculdade, turno);
            else
            {
                if (data != "-1" && !string.IsNullOrEmpty(aula))
                    qt.Query(cn, ano, semestre, turma, faculdade, turno, data, aula);
                else if (data != "-1")
                    qt.Query(cn, ano, semestre, turma, faculdade, turno, data);
                else
                    qt.Query(cn, ano, semestre, turma, faculdade, turno, disciplina);
            }

            return qt;
        }

        public static string ValidarDataLista(
            string disciplina, string turma, decimal? ano, decimal? periodo, DateTime? data)
        {
            string ret = string.Empty;

            if (data != null)
            {
                QueryTable qt = ObterDadosTurma(disciplina, turma, ano, periodo);
                if (qt.Rows.Count > 0)
                {
                    if (Convert.ToDateTime(data).Date < Convert.ToDateTime(qt.Rows[0]["dt_inicio"]).Date
                            || Convert.ToDateTime(data).Date > Convert.ToDateTime(qt.Rows[0]["dt_fim"]).Date)
                    {
                        ret = "A data selecionada não faz parte da turma.";
                    }
                }
            }
            return ret;
        }

        public static QueryTable ObterDadosTurma(string disciplina, string turma, decimal? ano, decimal? periodo)
        {
            TConnectionWritable tconnw = Config.CreateWritableConnection();
            QueryTable qt = null;

            try
            {
                tconnw.Open(true);

                if (!string.IsNullOrEmpty(disciplina) && !string.IsNullOrEmpty(turma) && ano != null && periodo != null)                        
                {
                    qt = new QueryTable(
                        " SELECT * " +
                        " FROM ly_turma " +
                        " WHERE disciplina = ? " +
                            " AND turma = ? " +
                            " AND ano = ? " +
                            " AND semestre = ? ");
                    qt.Query(tconnw, disciplina, turma, ano, periodo);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                tconnw.Close();
            }
            return qt;
        }

        /// <summary>
        /// Obtém as listas de frequência de determinada turma.
        /// </summary>        
        /// <param name="turma">Código da turma.</param>
        /// <param name="ano">Ano da turma.</param>
        /// <param name="semestre">Semestre da turma.</param>
        /// <returns>QueryTable contendo freq, subperiodo e aulas_dadas da Ly_freq.</returns>
        public static QueryTable ConsultarFrequenciasTurma(string turma, string ano, string semestre)
        {
            return Consultar(@"
                SELECT  disciplina, freq, subperiodo, aulas_dadas 
                FROM	ly_freq (NOLOCK)
                WHERE	turma = ? AND ano = ? AND periodo = ?",
                turma, ano, semestre);
        }

        /// <summary>
        /// Obtém as faltas de determinada turma.
        /// </summary>
        /// <param name="turma">Turma.</param>
        /// <param name="ano">Ano da turma.</param>
        /// <param name="semestre">Semestre da turma.</param>
        /// <returns>QueryTable contendo aluno, disciplina, freq e faltas de Ly_falta.</returns>
        public static QueryTable ConsultarFaltasTurma(string turma, string ano, string semestre)
        {
            return Consultar(@"
                SELECT	aluno, disciplina, freq, faltas, faltas - ISNULL(faltas_compensadas,0) faltas_efetivas
                FROM	ly_falta (NOLOCK)
                WHERE	turma = ? AND ano = ? AND periodo = ?",
                turma, ano, semestre);
        }

        #region Lançamento de notas do Histórico
        /// <summary>
        /// Consulta as frequências de determinada turma.
        /// </summary>
        /// <param name="turma">Turma.</param>
        /// <param name="ano">Ano.</param>
        /// <param name="semestre">Semestre</param>
        /// <returns>Informações das frequencias da turma.</returns>
        public static InfoFreq[] ConsultarFreqsHistorico(string turma, decimal? ano, decimal? semestre)
        {          
            QueryTable qt = Consultar(
               @"SELECT disciplina, freq,descricao, CONVERT(INT, aulas_dadas) aulas_dadas, subperiodo FROM 
                ly_freq WHERE turma = ? AND ano = ? AND periodo = ? ORDER BY disciplina, subperiodo",
               turma, ano, semestre);

            if (qt != null && qt.Rows.Count > 0)
            {
                return qt.Rows.Cast<SimpleRow>()
                    .Select(p => new InfoFreq
                    {
                        Disciplina = Convert.ToString(p["disciplina"]),
                        Freq = Convert.ToString(p["freq"]),
                        Subperiodo = Convert.ToDecimal(p["subperiodo"]),
                        AulasDadas = Util.Utils.ToNullableDecimal(p["aulas_dadas"]),
                         Titulo = Convert.ToString(p["descricao"])
                    }).ToArray();
            }
            else
                return new InfoFreq[] { };
        }

        /// <summary>
        /// Consulta as frequências de determinada turma.
        /// </summary>
        /// <param name="turma">Turma.</param>
        /// <param name="ano">Ano.</param>
        /// <param name="disciplina">Disciplina.</param>
        /// <param name="semestre">Semestre</param>
        /// <returns>Informações das frequencias da turma.</returns>
        public static InfoFreq[] ConsultarFreqsHistorico(string turma, decimal? ano, decimal? semestre, string disciplina)
        {
            QueryTable qt = Consultar(
               @"SELECT disciplina, descricao,freq, CONVERT(INT, aulas_dadas) aulas_dadas, subperiodo FROM 
                ly_freq WHERE turma = ? AND ano = ? AND periodo = ? AND disciplina = ? ORDER BY disciplina, subperiodo",
               turma, ano, semestre, disciplina);

            if (qt != null && qt.Rows.Count > 0)
            {
                return qt.Rows.Cast<SimpleRow>()
                    .Select(p => new InfoFreq
                    {
                        Disciplina = Convert.ToString(p["disciplina"]),
                        Freq = Convert.ToString(p["freq"]),
                        Subperiodo = Convert.ToDecimal(p["subperiodo"]),
                        AulasDadas = Util.Utils.ToNullableDecimal(p["aulas_dadas"]),
                        Titulo = Convert.ToString(p["descricao"])

                    }).ToArray();
            }
            //                        Freq = Convert.ToString(p["freq"]),

            else
                return new InfoFreq[] { };
        }

        [Serializable]
        public class InfoFreq
        {
            public string Freq { get; set; }
            public string Disciplina { get; set; }
            public string Titulo { get; set; }
            public decimal? Subperiodo { get; set; }
            public decimal? AulasDadas { get; set; }
        }
      
        #endregion

        internal static ContextQuery AtualizarAulas(string aulasDadas, string aulasPrevistas, string disciplina, string turma, int ano, int periodo, string frequencia)
        {
            var contextQuery = new ContextQuery
                               {
                                   Command = @"UPDATE  LY_FREQ
                                               SET     AULAS_DADAS = @AULAS_DADAS,
                                                       AULAS_PREVISTAS = @AULAS_PREVISTAS
                                               WHERE   DISCIPLINA = @DISCIPLINA
                                                       AND TURMA = @TURMA
                                                       AND ANO = @ANO
                                                       AND PERIODO = @PERIODO
                                                       AND FREQ = @FREQ"
                               };

            contextQuery.Parameters.Add("@AULAS_DADAS", TechneDbType.T_DECIMAL_MEDIO_PRECISO, aulasDadas);
            contextQuery.Parameters.Add("@AULAS_PREVISTAS", TechneDbType.T_DECIMAL_MEDIO_PRECISO, aulasPrevistas);
            contextQuery.Parameters.Add("@DISCIPLINA", TechneDbType.T_CODIGO, disciplina);
            contextQuery.Parameters.Add("@TURMA", turma);
            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
            contextQuery.Parameters.Add("@PERIODO", TechneDbType.T_SEMESTRE2, periodo);
            contextQuery.Parameters.Add("@FREQ", TechneDbType.T_FALTA, frequencia);

            return contextQuery;
        }

        public void ObtemConsolidadoBimestralPor(DataContext ctx, int ano, int periodo, string turma, string disciplina, out DTOs.DadosConsolidadoBimestralTurma consolidadoBimestral)
        {
            consolidadoBimestral = new DTOs.DadosConsolidadoBimestralTurma();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT  SUM(AULAS_PREVISTAS) AS TOTALAULASPREVISTAS ,
                                        SUM(AULAS_DADAS) AS TOTALAULASDADAS
                                FROM    VW_LY_FREQ
                                WHERE   DISCIPLINA = @DISCIPLINA
                                        AND TURMA = @TURMA
                                        AND ANO = @ANO
                                        AND PERIODO = @PERIODO ";

                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    if (reader["TOTALAULASPREVISTAS"] != DBNull.Value)
                    {
                        consolidadoBimestral.TotalAulasPrevistas = Convert.ToInt32(reader["TOTALAULASPREVISTAS"]);
                    }
                    if (reader["TOTALAULASDADAS"] != DBNull.Value)
                    {
                        consolidadoBimestral.TotalAulasDadas = Convert.ToInt32(reader["TOTALAULASDADAS"]);
                    }
                }
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }
    }
}
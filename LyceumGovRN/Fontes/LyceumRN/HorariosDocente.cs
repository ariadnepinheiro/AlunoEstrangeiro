using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Data;
using System.Data;
using Seeduc.Infra.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN
{
    public class HorariosDocente : RNBase
    {
        public static QueryTable ConsultarHorarioAulas(String matriculaDocente)
        {
            QueryTable qt = Consultar(@"
                SELECT (CASE WHEN ha.DIA_SEMANA = 1
			                THEN (CASE ha.dia_semana 
				                WHEN 1 THEN 'DOMINGO'
				                WHEN 2 THEN 'SEGUNDA'
				                WHEN 3 THEN 'TERÇA'
				                WHEN 4 THEN 'QUARTA'
				                WHEN 5 THEN 'QUINTA'
				                WHEN 6 THEN 'SEXTA'
				                WHEN 7 THEN 'SÁBADO' end)
			                ELSE (CASE WHEN ha.DIA_SEMANA = 7 
				                THEN (CASE ha.dia_semana 
					                WHEN 1 THEN 'DOMINGO'
					                WHEN 2 THEN 'SEGUNDA'
					                WHEN 3 THEN 'TERÇA'
					                WHEN 4 THEN 'QUARTA'
					                WHEN 5 THEN 'QUINTA'
					                WHEN 6 THEN 'SEXTA'
					                WHEN 7 THEN 'SÁBADO' end)
				                ELSE CAST(ha.dia_semana AS varchar(MAX)) + 'ª - ' + 
					                (CASE ha.dia_semana 
					                WHEN 1 THEN 'DOMINGO'
					                WHEN 2 THEN 'SEGUNDA'
					                WHEN 3 THEN 'TERÇA'
					                WHEN 4 THEN 'QUARTA'
					                WHEN 5 THEN 'QUINTA'
					                WHEN 6 THEN 'SEXTA'
					                WHEN 7 THEN 'SÁBADO' end)
				                END)
			                END) AS dia_semana_descr,
                    ha.dia_semana AS dia_semana,
                    t.disciplina,
                    d.nome AS disciplina_descr, 
                    d.nome, 
                    t.turma,
                    ha.faculdade,
                    ha.dependencia, 
                    ha.aula, 
                    dep.descricao, 
                    ha.horaini_aula, 
                    ha.horafim_aula, 
                    t.ano, 
                    t.semestre, 
                    pl.id_reduzida, 
                    fac.nome_abrev,
                    ha.faculdade + '<br/>' + fac.nome_abrev AS faculdade_descr,
                    (CASE WHEN dep.descricao <> NULL 
                        THEN
		                    ha.dependencia + '<br/>' + dep.tipo_depend + '  -  ' + dep.descricao
		                ELSE
                            ha.dependencia + '<br/>' + dep.tipo_depend 
                        END) AS dependencia_descr
                FROM ly_turma t, 
                    ly_aula_docente ad, 
                    ly_hor_aula ha, 
                    ly_disciplina d, 
                    ly_periodo_letivo pl, 
                    ly_faculdade fac,
                    ly_dependencia dep,
                    ly_docente doc
                WHERE ad.disciplina = ha.disciplina AND
                    ad.turma = ha.turma AND
                    ad.ano = ha.ano AND
                    ad.Semestre = ha.Semestre AND
                    ad.turno = ha.turno AND
                    ad.faculdade = ha.faculdade AND
                    ad.dia_semana = ha.dia_semana AND
                    ad.aula = ha.aula AND
                    t.disciplina = ha.disciplina AND
                    t.Turma = ha.Turma AND
                    t.ano = ha.ano AND
                    t.semestre = ha.semestre AND                    
	                t.sit_turma = 'Aberta' AND 
                    t.ano = pl.ano AND
                    t.semestre = pl.periodo AND
                    t.disciplina = d.disciplina AND                    
                    ha.faculdade = dep.faculdade AND
                    ha.dependencia = dep.dependencia AND
                    t.faculdade = fac.faculdade AND
                    doc.num_func = ad.num_func AND
                    ad.data_fim = t.dt_fim AND
                    doc.matricula = ?
                ORDER BY ha.dia_semana ASC,
                    ha.horaini_aula ASC,
                    ha.aula ASC, 
                    t.ano ASC,   
                    t.semestre ASC", matriculaDocente);
            return qt;
        }

        public DataTable ListaHorariosPor(int ano, int semestre, string turma, string disciplina, int diaSemana, DateTime dataLancamento)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            DataTable dt = null;

            try
            {
                dt = this.ListaHorariosPor(ctx, ano, semestre, turma, disciplina, diaSemana, dataLancamento);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Empty;
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
                ctx.Dispose();
            }

            return dt;
        }

        public DataTable ListaHorariosPor(DataContext ctx, int ano, int semestre, string turma, string disciplina, int diaSemana, DateTime dataLancamento)
        {
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            contextQuery.Command = @" SELECT DISTINCT CONVERT(CHAR(5),H.HORAINI_AULA,108) AS ENTRADA,                
		                                        CONVERT(CHAR(5),H.HORAFIM_AULA,108) AS SAIDA ,    
		                                        CONVERT(CHAR(5),H.HORAINI_AULA,108) + ' às ' + CONVERT(CHAR(5),HORAFIM_AULA,108) as  HORARIO,  
		                                        H.HORAINI_AULA, 
		                                        H.HORAFIM_AULA,
		                                        a.AULA
                                        FROM   LY_AULA_DOCENTE A
                                               INNER JOIN LY_HOR_AULA H
                                                 ON A.TURNO = H.TURNO
                                                    AND A.FACULDADE = H.FACULDADE
                                                    AND A.DIA_SEMANA = H.DIA_SEMANA
                                                    AND A.AULA = H.AULA
                                                    AND A.DISCIPLINA = H.DISCIPLINA
                                                    AND A.TURMA = H.TURMA
                                                    AND A.ANO = H.ANO
                                                    AND A.SEMESTRE = H.SEMESTRE 
                                        WHERE A.DISCIPLINA = @DISCIPLINA
                                                AND A.TURMA = @TURMA
                                                AND A.ANO = @ANO
                                                AND A.SEMESTRE = @SEMESTRE
                                                AND A.DIA_SEMANA = @DIASEMANA
		                                        AND A.DATA_INICIO <> A.DATA_FIM
                                                AND @DATALANCAMENTO BETWEEN  A.DATA_INICIO AND A.DATA_FIM
                                                AND NOT EXISTS (SELECT TOP 1 1 
                                                                FROM LY_AULA_DOCENTE A2 
                                                                WHERE A.NUM_FUNC = A2.NUM_FUNC 
                                                                    AND A.ANO = A2.ANO 
                                                                    AND A.SEMESTRE = A2.SEMESTRE 
                                                                    AND A.TURMA = A2.TURMA 
                                                                    AND A.DISCIPLINA = A2.DISCIPLINA 						
                                                                    AND A.DIA_SEMANA = A2.DIA_SEMANA
                                                                    AND A.DATA_INICIO = A2.DATA_FIM
                                                                    AND @DATALANCAMENTO BETWEEN A2.DATA_INICIO AND A2.DATA_FIM 
                                                                    AND A2.DATA_INICIO <> A2.DATA_FIM )
                                        ORDER BY HORAINI_AULA ASC   ";

            contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", semestre);
            contextQuery.Parameters.Add("@TURMA", turma);
            contextQuery.Parameters.Add("@DIASEMANA", diaSemana);
            contextQuery.Parameters.Add("@DATALANCAMENTO", dataLancamento);


            dt = ctx.GetDataTable(contextQuery);

            return dt;
        }

        public DataTable ListaDataFrequenciaPor(DataContext ctx, int ano, int semestre, string turma, string disciplina)
        {
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            contextQuery.Command = @" SELECT *
                                        FROM Turma.FN_DATAS_AULA (@DISCIPLINA, @TURMA, @ANO, @PERIODO, NULL)  ";

            contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", semestre);
            contextQuery.Parameters.Add("@TURMA", turma);

            dt = ctx.GetDataTable(contextQuery);

            return dt;
        }

        public DataTable ListaDataFrequenciaPor(int ano, int semestre, string turma, string disciplina, int mes)
        {
            DataTable horarios = null;
            ContextQuery contextQuery = new ContextQuery();


            try
            {
                contextQuery.Command = @"SELECT DATA as DATAHORA, CONVERT(varchar, DATA, 103) AS DATA
                                        FROM Turma.FN_DATAS_AULA (@DISCIPLINA, @TURMA, @ANO, @PERIODO, NULL)
                                        WHERE MONTH(DATA) = @MES
                                        ORDER BY DATAHORA ASC  ";

                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", semestre);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@MES", mes);

                horarios = Consultar(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }

            return horarios;
        }

        public DateTime ObtemMenorDataFrequenciaPor(int ano, int semestre, string turma, string disciplina)
        {
            DateTime data = DateTime.MinValue;
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                contextQuery.Command = @" SELECT MIN(DATA) MINDATA
                                        FROM Turma.FN_DATAS_AULA (@DISCIPLINA, @TURMA, @ANO, @PERIODO, NULL)  ";

                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", semestre);
                contextQuery.Parameters.Add("@TURMA", turma);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    data = reader["MINDATA"] != DBNull.Value ? Convert.ToDateTime(reader["MINDATA"]) : DateTime.MinValue;
                }

            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return data;
        }
    }
}

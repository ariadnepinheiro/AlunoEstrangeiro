using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Data;

namespace Techne.Lyceum.RN
{
    public class HorariosAluno : RNBase
    {
        public static QueryTable ObterHorarioAluno(string aluno)
        {
            TConnectionWritable tconnw = Config.CreateWritableConnection();
            QueryTable qtHorario = null;

            try
            {
                tconnw.Open(true);

                qtHorario = new QueryTable(@"
                     SELECT ha.dia_semana, 
	                    (CASE WHEN ha.dia_semana = 1 THEN
		                    (CASE ha.dia_semana 
			                    WHEN 1 THEN 'Domingo' 
			                    WHEN 2 THEN 'Segunda' 
			                    WHEN 3 THEN 'Terça' 
			                    WHEN 4 THEN 'Quarta' 
			                    WHEN 5 THEN 'Quinta' 
			                    WHEN 6 THEN 'Sexta' 
			                    WHEN 7 THEN 'Sábado' END) 
	                    ELSE 
		                    (CASE WHEN ha.dia_semana = 7 THEN
			                    (CASE ha.dia_semana 
				                     WHEN 1 THEN 'Domingo' 
				                     WHEN 2 THEN 'Segunda' 
				                     WHEN 3 THEN 'Terça' 
				                     WHEN 4 THEN 'Quarta' 
				                     WHEN 5 THEN 'Quinta' 
				                     WHEN 6 THEN 'Sexta' 
				                     WHEN 7 THEN 'Sábado' END)    
		                    ELSE CONVERT(VARCHAR,ha.DIA_SEMANA) + 'ª - ' + 
			                    (CASE ha.dia_semana 
				                     WHEN 1 THEN 'Domingo' 
				                     WHEN 2 THEN 'Segunda' 
				                     WHEN 3 THEN 'Terça' 
				                     WHEN 4 THEN 'Quarta' 
				                     WHEN 5 THEN 'Quinta' 
				                     WHEN 6 THEN 'Sexta' 
				                     WHEN 7 THEN 'Sábado' 
			                    ELSE CONVERT(VARCHAR,ha.DIA_SEMANA) END)
		                    END)
	                     END) AS dia_semana_descr, 
                         ha.faculdade, 
                         ha.dependencia, 
                         ha.aula, 
                         ha.horaini_aula, 
                         ha.horafim_aula, 
                         (CONVERT(VARCHAR(5),ha.horaini_aula, 108)) + ' as ' +
                             CONVERT(VARCHAR(5),ha.horafim_aula, 108) AS horario_descr, 
                         ha.frequencia AS frequencia, 
                         t.disciplina, 
                         t.turma, 
                         t.ano , 
                         t.semestre, 
                         t.dt_inicio, 
                         t.dt_fim, 
                         t.turma AS turma_descr, 
                         d.disciplina,
                         d.nome,
                         d.nome_compl AS nome_compl_discip,
                         d.nome_compl AS disciplina_descr,
                         pl.id_reduzida,
                         m.subturma1,
                         m.subturma2,
                         (Case when doc.MATRICULA not in('00000000','11111111','22222222','44444444','66666666','88888888','55555551','55555555','99999999') 
											Then 'Prof. ' + PE.nome_compl
                         else PE.nome_compl end) as docente,
                         dep.descricao,
                         (ISNULL(ue.nome_comp + '<br />' + dep.descricao, ue.nome_comp)) AS local_descr 
                     FROM ly_matricula m
                     INNER JOIN ly_turma t ON m.disciplina = t.disciplina
                         AND (m.turma = t.turma OR m.subturma1 = t.turma OR m.subturma2 = t.turma)
                         AND m.ano = t.ano 
                         AND m.semestre = t.semestre
                     INNER JOIN ly_hor_aula ha ON t.disciplina = ha.disciplina
                         AND t.turma = ha.turma
                         AND t.ano = ha.ano 
                         AND t.semestre = ha.semestre
                     INNER JOIN LY_AULA_DOCENTE ad ON ha.TURNO = ad.TURNO
	                    AND ha.FACULDADE = ad.FACULDADE
	                    AND ha.DIA_SEMANA = ad.DIA_SEMANA
	                    AND ha.AULA = ad.AULA
	                    AND ha.DISCIPLINA = ad.DISCIPLINA
	                    AND ha.TURMA = ad.TURMA
	                    AND ha.ANO = ad.ANO
	                    AND ha.SEMESTRE = ad.SEMESTRE
	                    AND ad.DATA_FIM <> ad.DATA_INICIO
	                    AND ad.DATA_FIM = t.DT_FIM
                     INNER JOIN ly_disciplina d ON t.disciplina = d.disciplina
                     LEFT OUTER JOIN ly_periodo_letivo pl ON pl.ano = t.ano
                         AND pl.periodo = t.semestre
                     LEFT OUTER JOIN LY_DOCENTE doc ON doc.NUM_FUNC = ad.NUM_FUNC
                     LEFT JOIN LY_PESSOA PE ON PE.PESSOA = DOC.PESSOA
                     INNER JOIN ly_dependencia dep ON ha.faculdade = dep.faculdade
                         AND ha.dependencia = dep.dependencia
                     INNER JOIN LY_UNIDADE_ENSINO ue ON ha.FACULDADE = ue.UNIDADE_ENS
                     WHERE m.aluno = ? and t.sit_turma = 'Aberta'
                     ORDER BY ha.dia_semana, CONVERT(VARCHAR(5),ha.horaini_aula, 108)");
                qtHorario.Query(tconnw, aluno);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally 
            {
                tconnw.Close();
            }
            return qtHorario;
        }
    }
}

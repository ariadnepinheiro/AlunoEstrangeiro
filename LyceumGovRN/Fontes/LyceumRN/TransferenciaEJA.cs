using System;
using System.Text;
using Techne.Data;
using Techne.Library;
using Techne.Lyceum.CR;

namespace Techne.Lyceum.RN
{
    public class TransferenciaEJA : RNBase
    {
        public static QueryTable ConsultarProvasUnidadeCurso(string aluno)
        {
            String sql =
                @"SELECT DISTINCT p.prova, 
                    p.NOME,
                    MAX(p.nota_max) AS nota_max,
                    CASE WHEN p.nome = 'Total de Pontos Final' THEN 3 ELSE  
		                CASE WHEN p.nome = 'Recuperação Paralela' THEN 2 ELSE 		  
			                CASE WHEN p.nome = 'Total de Pontos' THEN 1 ELSE 0 END 
		                END 
	                END AS ordenacao      
                FROM ly_prova p 
                INNER JOIN ly_matricula m ON p.DISCIPLINA = m.DISCIPLINA
	                AND p.TURMA = m.TURMA
	                AND p.ANO = m.ANO
	                AND p.SEMESTRE = m.SEMESTRE
                INNER JOIN ly_aluno a ON m.aluno = a.aluno
                WHERE a.aluno = ?
                GROUP BY p.prova, p.nome
                ORDER BY ordenacao, p.prova";

            QueryTable qtProvasUC = Consultar(sql, aluno);
            return qtProvasUC;
        }

        public static QueryTable ConsultarMatriculaNotaFalta(DbObject aluno)
        {
            if (aluno.IsNull)
                return null;

            StringBuilder sql = new StringBuilder(
            @"declare @aluno as varchar(20)
                set @aluno = ? 

SELECT mat.disciplina, 

mat.turma, 
                
d.nome AS nome_disciplina,

(select top 1 case when isnull(AULAS_DADAS,0) = 0 then -1 else 
round((1-(isnull(FALTAS,0)/isnull(AULAS_DADAS,0)))*100,1) end as freq 
from LY_FALTA fa inner join LY_FREQ fr
on fa.DISCIPLINA = fr.DISCIPLINA
and fa.TURMA = fr.TURMA
and fa.ANO = fr.ANO
and fa.PERIODO = fr.PERIODO
and fa.FREQ = fr.FREQ
where fa.TURMA = mat.TURMA
and fa.ANO = mat.ANO
and fa.ALUNO = mat.ALUNO
and fa.DISCIPLINA = mat.DISCIPLINA
and fa.PERIODO = mat.SEMESTRE
--and fr.subperiodo = ?
) as faltas ");

            QueryTable qtProvasUC = ConsultarProvasUnidadeCurso(aluno.ToString());
            if (qtProvasUC == null) return null;

            foreach (SimpleRow rowProvaUC in qtProvasUC.Rows)
            {
                String provaUC = rowProvaUC["prova"].ToString();

                sql.Append(@", 
                    CASE WHEN EXISTS (SELECT 1 FROM LY_PROVA pv WHERE pv.PROVA = '" + MudarAspas(provaUC) + @"'
                        and pv.DISCIPLINA = mat.DISCIPLINA  and pv.TURMA = mat.TURMA 
		                and pv.ANO = mat.ANO and pv.SEMESTRE = mat.SEMESTRE)
                    THEN ISNULL((SELECT conceito FROM ly_nota WHERE 
                        aluno = @aluno AND disciplina = d.disciplina AND 
                        ano = mat.ano AND semestre = mat.semestre AND mat.turma = turma AND 
                        prova = '" + MudarAspas(provaUC) + @"'), '')                    
                    ELSE '-' END AS " + provaUC + " ");
            }

            sql.Append(
            @" FROM ly_matricula mat
                inner join LY_TURMA t on mat.DISCIPLINA = t.DISCIPLINA and mat.TURMA = t.TURMA and mat.ANO = t.ANO and mat.SEMESTRE = t.SEMESTRE
                INNER JOIN ly_disciplina d ON d.disciplina = mat.disciplina
                WHERE mat.aluno = @aluno and (mat.sit_matricula = 'Matriculado' or mat.sit_matricula = 'Trancado')
                GROUP by mat.disciplina, d.nome, d.disciplina, mat.ano, mat.semestre, mat.turma, mat.aluno
                ORDER BY d.nome");

            return Consultar(sql.ToString(), aluno.ToString());
        }

        public static QueryTable ConsultarDadosAluno(string aluno)
        {
            QueryTable qt = null;

            string sql = @"SELECT top 1 a.aluno, 
            a.unidade_ensino, 
            c.curso,           
            c.nome as nomecurso,
            t.turno, 
            t.descricao as nometurno, 
            a.curriculo, 
            a.serie, 
            s.descricao as nomeserie, 
            ue.nome_comp as nomefaculdade, 
            n.descricao as nomenucleo, 
            ue.nucleo, 
            a.unidade_fisica, 
            uf.nome_comp as nomeUnidadeFisica,
            m.ano,
            m.semestre,
            m.turma
            from ly_matgrade mg 
            join ly_grade_turma gt on mg.grade_id = gt.grade_id 
            join ly_grade_serie gs on gs.GRADE_ID = mg.GRADE_ID and gs.GRADE = gt.TURMA and gs.GRADE = gt.TURMA 
            join ly_matricula m on m.ALUNO = mg.ALUNO and gt.disciplina = m.disciplina and m.turma = gt.turma and m.ANO = gt.ANO and m.SEMESTRE = gt.SEMESTRE 
            join Ly_Aluno a on mg.ALUNO = a.ALUNO 
            INNER JOIN Ly_Curso c on gs.curso = c.curso 
            left JOIN Ly_unidade_fisica uf on a.unidade_fisica = uf.unidade_fis 
            INNER JOIN Ly_unidade_ensino ue on a.UNIDADE_ENSINO = ue.unidade_ens 
            INNER JOIN ly_turno t on gs.turno = t.turno 
            INNER JOIN ly_serie s on gs.curso = s.curso AND gs.curriculo = s.curriculo AND t.turno = s.turno AND a.SERIE = s.serie 
            LEFT JOIN ly_nucleo n on n.nucleo = ue.nucleo 
            where mg.sit_matgrade = 'Matriculado' 
            and mg.aluno = ? ";

            qt = Consultar(sql, aluno);

            return qt;
        }

        public static string ConsultarModalidade(string curso)
        {
            return ConsultarCampo("select top 1 modalidade from ly_curso where curso = ?", curso);
        }
    }
}
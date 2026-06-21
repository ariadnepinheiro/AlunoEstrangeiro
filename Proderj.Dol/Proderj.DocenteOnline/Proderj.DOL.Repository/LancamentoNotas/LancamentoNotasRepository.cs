using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;
using Proderj.DOL.Domain;
using Proderj.Foundation.Common;
using System.Collections;
using NHibernate.Linq;

namespace Proderj.DOL.Repository
{
    public class LancamentoNotasRepository : NHRepositoryBase<LancamentoNotas>, ILancamentoNotasRepository
    {
        #region ILancamentoNotasRepository Members

        public IEnumerable<LancamentoNotas> EnumeraLancamentosPor(string disciplina, string turma, short ano, short periodo, short subperiodo)
        {
            //var query = Sessao.CreateSQLQuery("exec SP_LANCAMENTONOTAS_DOL :disciplina,:turma,:ano,:periodo,:subperiodo");
            //query.SetString("disciplina", disciplina);
            //query.SetString("turma", turma);
            //query.SetInt16("ano", ano);
            //query.SetInt16("periodo", periodo);
            //query.SetInt16("subperiodo", subperiodo);

            var query = Sessao.CreateSQLQuery(@"  
            SELECT pe.nome_compl  
     , m.aluno  
     , m.sit_matricula  
     , ISNULL((SELECT TOP 1  
                      [DESCRICAO]   + ' - ' + CONVERT(VARCHAR, [DT_INICIO], 103) + ' a ' + CONVERT(VARCHAR, [DT_FIM], 103)  
                 FROM [dbo].[LY_ALUNO_LICENCA] WITH ( NOLOCK )  
                WHERE [ALUNO]      = m.aluno  
                  AND [DISCIPLINA] = m.disciplina  
                  AND [TURMA]      = m.turma  
                  AND [ANO]        = m.ano  
                  AND [SEMESTRE]   = m.semestre  
                  AND DT_FIM       = (SELECT MAX(DT_FIM)  
                                       FROM [LY_ALUNO_LICENCA] WITH ( NOLOCK )  
                                      WHERE [ALUNO]      = m.aluno  
                                        AND [DISCIPLINA] = m.disciplina  
                                        AND [TURMA]      = m.turma  
                                        AND [ANO]        = m.ano  
                                        AND [SEMESTRE]   = m.semestre)  
             ), '') +   
       ISNULL((SELECT TOP 1  
                      'Remanejado para a turma ' + TURMA_DESTINO + ' em ' + CONVERT(VARCHAR, DATA, 103)  
                 FROM dbo.LY_TURMA_TRANSF WITH ( NOLOCK )  
                WHERE ALUNO     = m.ALUNO  
                  AND ANO       = m.ANO  
                  AND PERIODO   = m.SEMESTRE  
                  AND TURMA_ANT = m.TURMA), '') +   
       ISNULL((SELECT TOP 1  
                      LY_MOTIVOSAIDA.DESCRICAO + ' em ' + CONVERT(VARCHAR, DT_ENCERRAMENTO, 103)  
                 FROM dbo.LY_H_CURSOS_CONCL WITH ( NOLOCK )  
                INNER JOIN dbo.LY_MOTIVOSAIDA WITH ( NOLOCK ) ON LY_H_CURSOS_CONCL.MOTIVO = LY_MOTIVOSAIDA.MOTIVOSAIDA  
                WHERE ALUNO          = m.ALUNO  
                  AND DT_REABERTURA IS NULL), '') +   
       ISNULL((SELECT TOP 1  
                      ' Transferido para outra unidade da rede estadual em ' + CONVERT(VARCHAR, DT_TRANS, 103)  
                 FROM dbo.LY_H_CURR_ALUNO WITH ( NOLOCK )  
                INNER JOIN dbo.LY_UNIDADE_ENSINO WITH ( NOLOCK ) ON LY_H_CURR_ALUNO.UNIDADE_ENSINO = LY_UNIDADE_ENSINO.UNIDADE_ENS  
                WHERE ALUNO   = m.ALUNO  
                  AND ANO     = m.ANO  
                  AND PERIODO = m.SEMESTRE  
             ), '') descricao_situacao  
     , m.num_chamada  
     , m.disciplina  
     , m.turma  
     , m.ano  
     , m.semestre  
     , CASE m.sit_matricula  
          WHEN 'Matriculado' THEN n.conceito  
          ELSE NULL  
       END AS MÉDIA  
      , CASE
                              WHEN  m.sit_matricula = 'Matriculado' and f.FALTAS is not null THEN CONVERT(INT, f.faltas)
							  when m.SIT_MATRICULA = 'Matriculado' and f.FALTAS is null then (	select count(*) 
																								from Turma.FREQUENCIADIARIA fd
																								inner join Turma.FREQUENCIADIARIA_ALUNOFALTA fda on fd.FREQUENCIADIARIAID=fda.FREQUENCIADIARIAID
																								inner join LY_SUBPERIODO_LETIVO SL on sl.ANO = fd.ANO and sl.PERIODO = fd.SEMESTRE and sl.SUBPERIODO =  :SUBPERIODO
																								where  m.DISCIPLINA = fd.disciplina
																									AND m.turma = fd.TURMA
																									AND m.ANO = fd.ANO
																									AND m.SEMESTRE = fd.SEMESTRE                                                                 
																									AND m.ALUNO = fda.ALUNO                                                                
																									AND fda.ATIVO = 1
																									and fd.DATAFREQUENCIA between sl.DT_INICIO and sl.DT_FIM)
                              ELSE NULL
                            END AS faltas  
     , prova.nome AS nome_prova  
     , prova.nota_max  
     , prova.formula  
     , CASE m.sit_matricula  
          WHEN 'Matriculado' THEN n.recuperacao_paralela  
          ELSE 'N'  
       END AS recuperacao_paralela  
     , CASE m.sit_matricula  
          WHEN 'Matriculado' THEN n.sem_avaliacao  
          ELSE 'N'  
       END AS sem_avaliacao  
     , CASE m.sit_matricula  
          WHEN 'Matriculado' THEN n.justificativa  
          ELSE ''  
       END AS justificativa  
     , n.notaProva  
     , n.notaRecuperacao    
     , n.motivoSemNotaID  
     , (SELECT TOP 1 ID_ALUNO_LICENCA  
          FROM [dbo].[LY_ALUNO_LICENCA] WITH ( NOLOCK )  
         WHERE [ALUNO]      = m.aluno  
           AND [DISCIPLINA] = m.disciplina  
           AND [TURMA]      = m.turma  
           AND [ANO]        = m.ano  
           AND [SEMESTRE]   = m.semestre  
           AND DT_FIM       = (SELECT MAX(DT_FIM)  
                                 FROM [LY_ALUNO_LICENCA] WITH ( NOLOCK )  
                                WHERE [ALUNO]      = m.aluno  
                                  AND [DISCIPLINA] = m.disciplina  
                                  AND [TURMA]      = m.turma  
                                  AND [ANO]        = m.ano  
                                  AND [SEMESTRE]   = m.semestre)) AS ID_ALUNO_LICENCA  
    , n.notaID  
  
  FROM ly_matricula m WITH ( NOLOCK )  
   
  JOIN ly_aluno a WITH ( NOLOCK ) ON m.aluno = a.aluno  
   JOIN ly_pessoa pe WITH ( NOLOCK ) ON pe.pessoa = a.pessoa
  LEFT JOIN ly_freq freq WITH ( NOLOCK ) ON freq.disciplina = m.disciplina  
                                        AND freq.turma      = m.turma  
                                        AND freq.ano        = m.ano  
                                        AND freq.periodo    = m.semestre  
                                        AND freq.subperiodo = :SUBPERIODO  
    
  LEFT JOIN LY_FALTA f WITH ( NOLOCK ) ON f.ALUNO      = m.ALUNO  
                                      AND f.ANO        = m.ANO  
                                      AND f.DISCIPLINA = m.DISCIPLINA  
                                      AND f.PERIODO    = m.SEMESTRE  
                                      AND f.TURMA      = m.TURMA  
                                      AND freq.freq    = f.freq  
    
  LEFT JOIN ly_prova prova WITH ( NOLOCK ) ON prova.disciplina = m.disciplina  
                                          AND prova.turma      = m.turma  
                                          AND prova.ano        = m.ano  
                                          AND prova.semestre   = m.semestre  
                                          AND prova.subperiodo = :SUBPERIODO  
    
  LEFT JOIN LY_NOTA n WITH ( NOLOCK ) ON n.DISCIPLINA = prova.disciplina  
                                     AND n.turma      = prova.TURMA  
                                     AND n.ANO        = prova.ANO  
                                     AND n.SEMESTRE   = prova.SEMESTRE  
                                     AND n.PROVA      = prova.PROVA  
                                     AND n.ALUNO      = m.ALUNO  
  
WHERE m.disciplina = :DISCIPLINA  
   AND m.turma      = :TURMA  
   AND m.ano        = :ANO  
   AND m.semestre   = :PERIODO  
 ORDER   
    BY  pe.nome_compl ");


            query.SetString("DISCIPLINA", disciplina);
            query.SetString("TURMA", turma);
            query.SetInt16("ANO", ano);
            query.SetInt16("PERIODO", periodo);
            query.SetInt16("SUBPERIODO", subperiodo);
            query.SetTimeout(60);

            var listaLancamentos = query.List();

            IEnumerable<LancamentoNotas> turmas = MapeiaLancamentoNotas(listaLancamentos);
            return turmas;
        }

        private IEnumerable<LancamentoNotas> MapeiaLancamentoNotas(IList listaLancamentos)
        {
            var lista = new List<LancamentoNotas>();

            foreach (object[] turma in listaLancamentos)
            {
                var itemLancamentoNotas = new LancamentoNotas
                                            {
                                                NomeCompleto = turma[0] != null ? turma[0].ToString() : String.Empty,
                                                MatriculaAluno = turma[1] != null ? turma[1].ToString() : String.Empty,
                                                SituacaoMatricula = turma[2] != null ? turma[2].ToString() : String.Empty,
                                                DescricaoSituacao = turma[3] != null ? turma[3].ToString() : String.Empty,
                                                NumeroChamada = turma[4] != null ? turma[4].ToString() : String.Empty,
                                                NotaProva = !String.IsNullOrEmpty((string)turma[9]) ? turma[9].To<Decimal>() : default(decimal?),
                                                Faltas = turma[10] != null ? turma[10].To<short>() : default(short?),
                                                NomeProva = turma[11] != null ? turma[11].ToString() : String.Empty,
                                                NotaMaxima = !String.IsNullOrEmpty((string)turma[12]) ? turma[12].To<Double>() : default(double?),
                                                Formula = turma[13] != null ? turma[13].ToString() : String.Empty,
                                                RecuperacaoPararela = turma[14] == null ? false : (turma[14].ToString() == "S"),
                                                SemAvaliacao = turma[15] == null ? false : (turma[15].ToString() == "S"),
                                                CodigoJustificativa = turma[16] != null ? turma[16].ToString() : String.Empty,
                                                MediaNota = turma[17] != null ? turma[17].To<Decimal>() : default(decimal?),
                                                NotaRecuperacao = turma[18] != null ? turma[18].To<Decimal>() : default(decimal?),
                                                MotivoSemNota = turma[19] != null ? turma[19].To<short>() : default(short?),
                                                PossuiLicenca = turma[20] != null ? true : false,
                                                NotaId = turma[21].To<int>()
                                            };

                lista.Add(itemLancamentoNotas);
            }
            return lista;
        }

        public bool ExisteNotaPendenteParaLancamentoEmBimestreAnteriorAoAtualPor(short ano, short periodo, short subperiodoAtual, string disciplina, string turma)
        {
            //throw new NotImplementedException();

            //var imat = Sessao.Query<Matricula>();
            //var iprov = Sessao.Query<Prova>();
            //var inota = Sessao.Query<Nota>();

            #region leftjoin em LINQ Nao suportado pelo Hibernate, infelizmente
            /*
			var existe = from matricula in Sessao.Query<Matricula>()
						 join prova in Sessao.Query<Prova>()
							on new
							{
								matricula.Disciplina,
								matricula.Turma,
								matricula.Ano,
								matricula.Semestre
							}
							equals
								new
								{
									prova.Disciplina,
									prova.Turma,
									prova.Ano,
									prova.Semestre
								}
						  where 
							matricula.Situacao == "Matriculado" &&
							matricula.Ano == ano &&
							matricula.Semestre == periodo &&
							matricula.Disciplina == disciplina &&
							matricula.Turma == turma &&
							prova.SubPeriodo < subperiodoAtual

						 select new { 
							 matricula.Aluno, 
							 matricula.Disciplina, 
							 matricula.Turma, 
							 matricula.Ano, 
							 matricula.Semestre, 
							 prova.TipoProva 
						};


			var existe2 = from e in existe
						  join nota in Sessao.Query<Nota>()
							on new
							{
								e.Aluno,
								e.Disciplina,
								e.Turma,
								e.Ano,
								e.Semestre,
								e.TipoProva
							} 
							equals new 
							{ 
								nota.Aluno,
								nota.Disciplina,
								nota.Turma,
								nota.Ano,
								nota.Semestre,
								nota.TipoProva
							}
						  into left1
						  from left2 in left1.DefaultIfEmpty()
						  where 
							left2.Aluno == null

						  select left2.Aluno;

			var a = existe2.ToArray();

						  //from mbox in matPorProva
						 // select matPorProva;

			return a.Length > 0;
			*/
            #endregion
            /*
            var query = Sessao.CreateSQLQuery(
							@"SELECT TOP 1 1 
							FROM 
								LY_MATRICULA m (NOLOCK) 
								INNER JOIN LY_PROVA P (NOLOCK) 
									on p.DISCIPLINA = m.DISCIPLINA 
									and p.TURMA = m.TURMA
									and p.ANO = m.ANO and p.SEMESTRE = m.SEMESTRE
							WHERE 
								  NOT EXISTS(
									SELECT TOP 1 1 
									FROM 
										LY_NOTA n (NOLOCK) 
									WHERE 
										n.ALUNO = m.ALUNO 
										AND n.DISCIPLINA = m.DISCIPLINA 
										AND n.TURMA = m.TURMA 
										AND n.ANO = m.ANO 
										AND n.SEMESTRE = m.SEMESTRE 
										AND n.PROVA = p.PROVA
								)
								AND m.ANO = :ano
								AND m.SEMESTRE = :periodo
								AND m.DISCIPLINA = :disciplina
								AND m.TURMA = :turma
								AND m.SIT_MATRICULA = 'Matriculado'
								AND p.SUBPERIODO < :subperiodo");

			query.SetString("disciplina", disciplina);
			query.SetString("turma", turma);
			query.SetInt16("ano", ano);
			query.SetInt16("periodo", periodo);
			query.SetInt16("subperiodo", subperiodoAtual); 
             
             return false;

			*/
            var query = Sessao.CreateSQLQuery("exec SP_BUSCAPENDENCIABIMESTREANTERIOR :disciplina,:turma,:ano,:periodo,:subperiodo");
            query.SetString("disciplina", disciplina);
            query.SetString("turma", turma);
            query.SetInt16("ano", ano);
            query.SetInt16("periodo", periodo);
            query.SetInt16("subperiodo", subperiodoAtual);


            var resultado = query.List();

            return resultado.Count > 0;

        }

        public bool AtualizaFlagLancamentoCompletoPor(short ano, short periodo, string turma, string disciplina, string prova)
        {
            int resultado = 0;
            try
            {
                var query = SessaoAuditada.CreateSQLQuery(
                                @"UPDATE  PROVA
                                SET     PROVA.COMPLEMENTO = PROVA.LANCAMENTO_COMPLETO
                                FROM    (
                                        SELECT P.COMPLEMENTO,
                                                (
                                                SELECT CASE WHEN EXISTS (  -- QUANDO EXISTE MATRÍCULA SEM NOTA, LANCAMENTO_COMPLETO = 'N'
                                                                    SELECT TOP 1
                                                                            1
                                                                    FROM      LY_MATRICULA M (NOLOCK)
                                                                    WHERE     M.DISCIPLINA = P.DISCIPLINA
                                                                            AND M.TURMA = P.TURMA
                                                                            AND M.ANO = P.ANO
                                                                            AND M.SEMESTRE = P.SEMESTRE
                                                                            AND M.SIT_MATRICULA = 'Matriculado'
                                                                            AND NOT EXISTS ( SELECT TOP 1
                                                                                                    1
                                                                                            FROM   LY_NOTA N (NOLOCK)
                                                                                            WHERE  N.DISCIPLINA = M.DISCIPLINA
                                                                                                    AND N.TURMA = M.TURMA
                                                                                                    AND N.ANO = M.ANO
                                                                                                    AND N.SEMESTRE = M.SEMESTRE
                                                                                                    AND N.PROVA = P.PROVA
                                                                                                    AND N.ALUNO = M.ALUNO
																									
																									UNION   
															
																							SELECT TOP 1 DATA_OPERACAO FROM LY_LOG_NOTA LN
																							WHERE LN.DISCIPLINA = M.DISCIPLINA
																							   AND LN.TURMA = M.TURMA
																							   AND LN.ANO = M.ANO
																							   AND LN.SEMESTRE = M.SEMESTRE
																							   AND LN.PROVA = P.PROVA
																							   AND LN.ALUNO = M.ALUNO 
																								ORDER BY DATA_OPERACAO DESC ) ) THEN 'N'
                                                            ELSE 'S'
                                                        END
                                                ) LANCAMENTO_COMPLETO
                                        FROM   LY_PROVA P (NOLOCK)
                                        WHERE  P.DISCIPLINA = :DISCIPLINA
                                                AND P.TURMA = :TURMA
                                                AND P.ANO = :ANO
                                                AND P.SEMESTRE = :PERIODO
                                                AND P.PROVA = :PROVA
                                        ) PROVA");

                query.SetString("DISCIPLINA", disciplina);
                query.SetString("TURMA", turma);
                query.SetString("PROVA", prova);
                query.SetInt16("ANO", ano);
                query.SetInt16("PERIODO", periodo);

                resultado = query.ExecuteUpdate();

                return resultado > 0;

            }
            catch (Exception)
            {
                throw new Exception("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.");
            }           
        }

        #endregion
    }
}

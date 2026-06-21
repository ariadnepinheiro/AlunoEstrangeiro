using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository
{
	public class LogCompetenciaHabilidadeDocenteRepository : NHRepositoryBase<LogCompetenciaHabilidadeDocente>, ILogCompetenciaHabilidadeDocenteRepository
	{
		public int InserePorCompetenciaHabilidadeItemPor(string matricula, string turma, string disciplina, short ano, short periodo, short subperiodo)
		{
            try
            {
                var query = SessaoAuditada.CreateSQLQuery(
                                        @"INSERT INTO dbo.TCE_LOG_COMPETENCIA_HABILIDADE_DOCENTE
									 (
									  ID_COMPETENCIA_HABILIDADE_ITEM,
									  DISCIPLINA,
									  TURMA,
									  ANO,
									  PERIODO,
									  SUBPERIODO,
									  MATRICULA,
									  DT_CADASTRO
									 )
									 SELECT  ID_COMPETENCIA_HABILIDADE_ITEM,
											 DISCIPLINA,
											 TURMA,
											 ANO,
											 PERIODO,
											SUBPERIODO,
											 MATRICULA,
											 DT_CADASTRO
									 FROM    dbo.TCE_COMPETENCIA_HABILIDADE_DOCENTE
									 WHERE   MATRICULA = :MATRICULA
											 AND TURMA = :TURMA
											 AND DISCIPLINA = :DISCIPLINA
											 AND ANO = :ANO
											 AND PERIODO = :PERIODO
											 AND SUBPERIODO = :SUBPERIODO
								");

                query.SetString("MATRICULA", matricula);
                query.SetString("TURMA", turma);
                query.SetString("DISCIPLINA", disciplina);
                query.SetInt16("ANO", ano);
                query.SetInt16("PERIODO", periodo);
                query.SetInt16("SUBPERIODO", subperiodo);

                return query.ExecuteUpdate();
            }
            catch (Exception)
            {
                throw new Exception("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.");
            }       
		}	
	}
}

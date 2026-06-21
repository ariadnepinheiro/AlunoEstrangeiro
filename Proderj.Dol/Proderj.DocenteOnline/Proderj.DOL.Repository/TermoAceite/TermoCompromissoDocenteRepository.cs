using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Architecture;
using Proderj.DOL.Domain;
using Proderj.Foundation.Framework;
using System.Collections;

namespace Proderj.DOL.Repository
{
    public class TermoCompromissoDocenteRepository : NHRepositoryBase<TermoCompromissoDocente>, ITermoCompromissoDocenteRepository
    {
        #region ITermoCompromissoDocenteRepository Members

		public TermoCompromissoDocente ObtemTermoNaoAceitoMaisRecentePor(string matricula)
		{
			string sql = @"SELECT TOP 1
									T.ID_TERMO_DOCENTE, T.ANO, DT_INICIO, T.DT_FIM, T.ARQUIVO, T.DT_CADASTRO, T.DT_ALTERACAO, T.MATRICULA
							FROM    DBO.VW_TCE_TERMO_COMPROMISSO_DOCENTE T
								LEFT JOIN DBO.VW_TCE_ACEITE_TERMO_COMPROMISSO_DOCENTE A
								ON A.ID_TERMO_DOCENTE = T.ID_TERMO_DOCENTE
								AND A.MATRICULA = :matricula
							WHERE   
								T.DT_FIM >= CONVERT(DATE, GETDATE()) 
								AND A.MATRICULA Is Null
							ORDER BY T.ANO";

            var query = Sessao.CreateSQLQuery(sql);

			query.AddEntity(typeof (TermoCompromissoDocente));

			query.SetParameter("matricula", matricula);

			TermoCompromissoDocente termoDeCompromisso = query.UniqueResult<TermoCompromissoDocente>();

			return termoDeCompromisso;
		}

        public TermoCompromissoDocente ObtemTermoNaoAceitoMaisRecentePorIdFuncional(string idfuncional)
        {
            string sql = @"SELECT TOP 1
									T.ID_TERMO_DOCENTE, T.ANO, DT_INICIO, T.DT_FIM, T.ARQUIVO, T.DT_CADASTRO, T.DT_ALTERACAO, T.MATRICULA
							FROM    DBO.VW_TCE_TERMO_COMPROMISSO_DOCENTE T
								LEFT JOIN DBO.VW_TCE_ACEITE_TERMO_COMPROMISSO_DOCENTE A
								ON A.ID_TERMO_DOCENTE = T.ID_TERMO_DOCENTE
								AND A.IDFUNCIONAL = :idfuncional
							WHERE   
								T.DT_FIM >= CONVERT(DATE, GETDATE()) 
								AND A.MATRICULA Is Null
							ORDER BY T.ANO";

            var query = Sessao.CreateSQLQuery(sql);

            query.AddEntity(typeof(TermoCompromissoDocente));

            query.SetParameter("idfuncional", idfuncional);

            TermoCompromissoDocente termoDeCompromisso = query.UniqueResult<TermoCompromissoDocente>();

            return termoDeCompromisso;
        }

		#endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;
using Proderj.Foundation.Common;
using Proderj.DOL.Domain;
using System.Collections;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace Proderj.DOL.Repository
{
	public class AvaliacaoCurriculoMinimoRepository : NHRepositoryBase<AvaliacaoCurriculoMinimo>, IAvaliacaoCurriculoMinimoRepository
	{
		public IEnumerable<TOAvaliacaoCurriculoMinimoListagem> EnumeraPor(string matricula, short ano, short periodo, short subperiodo)
		{
			var query = Sessao.CreateSQLQuery(
                                    @"SELECT  C.ID_AVALIACAO_CM,
											ORDEM,
											AVALIACAO,
											(
												SELECT  TOP 1  resposta
												FROM      TCE_AVALIACAO_CM_DOCENTE d
												WHERE     D.ID_AVALIACAO_CM = C.ID_AVALIACAO_CM
														AND MATRICULA = :MATRICULA
                                                ORDER BY DT_CADASTRO DESC 
											) AS RESPOSTA
									 FROM   TCE_AVALIACAO_CM c
									 WHERE  ANO = :ANO
											AND PERIODO = :PERIODO
											AND SUBPERIODO = :SUBPERIODO
											AND HABILITADO = 1
									 ORDER BY ORDEM");

			query.SetInt16("ANO", ano);
			query.SetInt16("PERIODO", periodo);
			query.SetInt16("SUBPERIODO", subperiodo);
			query.SetString("MATRICULA", matricula);

			var retorno = query.List();

			foreach (object[] item in retorno)
			{
				yield return new TOAvaliacaoCurriculoMinimoListagem
						{
							IdAvaliacaoCurriculoMinimo = item[0].To<int>(),
							Ordem = item[1].To<short>(),
							DescricaoAvaliacao = item[2] != null ? item[2].ToString() : string.Empty,
							Resposta = item[3] != null ? item[3].To<bool>() : default(bool?)
						};
			}
		}
	}
}

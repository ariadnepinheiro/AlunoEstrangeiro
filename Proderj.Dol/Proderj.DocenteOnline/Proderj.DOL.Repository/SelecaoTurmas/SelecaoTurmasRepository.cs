using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;
using Proderj.Foundation.Common;
using Proderj.DOL.Domain;
using System.Collections;

namespace Proderj.DOL.Repository
{
	public class SelecaoTurmasRepository : NHRepositoryBase<SelecaoTurmas>, ISelecaoTurmasRepository
	{
		public IEnumerable<SelecaoTurmas> EnumeraTurmasPor(long numeroFuncionario)
		{
			var query = Sessao.CreateSQLQuery("exec SP_SELECAOTURMAS :numeroFunc");
			query.SetString("numeroFunc", numeroFuncionario.ToString());

			var listaTurmas = query.List();

			IEnumerable<SelecaoTurmas> turmas = MapeiaSelecaoTurmas(listaTurmas);
			return turmas;
		}

		private IList<SelecaoTurmas> MapeiaSelecaoTurmas(IList listaTurmas)
		{
			var listaSelecaoTurmas = new List<SelecaoTurmas>();

			foreach (object[] turma in listaTurmas)
			{
				var selecao = new SelecaoTurmas
				{
					UnidadeEnsino = turma[0] != null ? turma[0].ToString() : String.Empty,
					NomeCompletoUnidadeEnsino = turma[1] != null ? turma[1].ToString() : String.Empty,
					Disciplina = turma[2] != null ? turma[2].ToString() : String.Empty,
					Turma = turma[3] != null ? turma[3].ToString() : String.Empty,
					Ano = turma[4] != null ? turma[4].To<short>() : default(short),
					Semestre = turma[5] != null ? turma[5].To<short>() : default(short),
					NomeCompletoDisciplina = turma[6] != null ? turma[6].ToString() : String.Empty,
					ValidoParaLancamento = turma[7] == null ? false : turma[7].ToBoolean(),
					PossuiNotasPendentes = turma[8] == null ? false : turma[8].ToBoolean(),
                    PossuiFaltasPendentes = turma[9] == null ? false : turma[9].ToBoolean(),
					StatusLancamentoLiberado = turma[10] != null ? turma[10].ToString() : String.Empty,
					StatusLancamentoAguardando = turma[11] != null ? turma[11].ToString() : String.Empty,
					StatusLancamentoBloqueado = turma[12] != null ? turma[12].ToString() : String.Empty,
					Curso = turma[13] != null ? turma[13].ToString() : String.Empty,
					Modalidade = turma[14] != null ? turma[14].ToString() : String.Empty,
					Tipo = turma[15] != null ? turma[15].ToString() : String.Empty,
					Serie = turma[16] != null ? turma[16].To<short>() : default(short),
                    RegistroProva = turma[17] == null ? false : turma[17].ToBoolean(),
                    RegistroFrequencia = turma[18] == null ? false : turma[18].ToBoolean()
				};

				listaSelecaoTurmas.Add(selecao);
			}

			return listaSelecaoTurmas;
		}
	}
}

using System.Collections.Generic;
using Proderj.Foundation.Framework;
using Proderj.Foundation.Common;
using Proderj.DOL.Domain;
using Proderj.DOL.Repository;
using System.Collections;
using System;

namespace Proderj.DOL.Repository
{
    public class LancamentoNotasConsolidadoRepository : NHRepositoryBase<LancamentoNotasConsolidado>, ILancamentoNotasConsolidadoRepository
    {
        #region ILancamentoNotasConsolidadoRepository Members

        public IEnumerable<LancamentoNotasConsolidado> EnumeraLancamentosNotasConsolidado(string disciplina, string turma, short ano, short periodo)
        {
            var query = Sessao.CreateSQLQuery("exec SP_CONSOLIDADOBIMESTRAL :disciplina, :turma, :ano, :periodo");
            query.SetString("disciplina", disciplina);
            query.SetString("turma", turma);
            query.SetInt16("ano", ano);
            query.SetInt16("periodo", periodo);
            query.SetTimeout(60);

            var listaLancamentos = query.List();

            IEnumerable<LancamentoNotasConsolidado> notasEFaltas = MapeiaLancamentoNotasConsolidado(listaLancamentos, periodo, ano);

            return notasEFaltas;
        }

        private IEnumerable<LancamentoNotasConsolidado> MapeiaLancamentoNotasConsolidado(IList listaLancamentos, short periodo, int ano)
        {
            List<LancamentoNotasConsolidado> lista = new List<LancamentoNotasConsolidado>();            

            foreach (object[] aluno in listaLancamentos)
            {

                LancamentoNotasConsolidado notasEFaltas = new LancamentoNotasConsolidado
                {
                    NomeCompleto = aluno[1] != null ? aluno[1].ToString() : String.Empty,
                    SituacaoMatricula = aluno[2] != null ? aluno[2].ToString() : String.Empty,
                    Nota1 = !String.IsNullOrEmpty(Convert.ToString(aluno[3])) ? aluno[3].To<decimal>() : default(decimal?),
                    Nota2 = !String.IsNullOrEmpty(Convert.ToString(aluno[4])) ? aluno[4].To<decimal>() : default(decimal?)
                };

                // Devido às colunas serem dinâmicas, e segundo os períodos, os valores possíveis são:
                // periodo = 0 --> irá possuir todas as 4 colunas de bimetres
                // período 1 ou 2, terão somente 2 columas de bimestres (1 e 2)

                if (periodo == 0)
                {
                    if (ano >= 2025)
                    {
                        notasEFaltas.Nota3 = !String.IsNullOrEmpty((string)aluno[5]) ? aluno[5].To<decimal>() : default(decimal?);

                        notasEFaltas.Falta1 = aluno[6] != null ? aluno[6].To<short>() : default(short?);
                        notasEFaltas.Falta2 = aluno[7] != null ? aluno[7].To<short>() : default(short?);
                        notasEFaltas.Falta3 = !String.IsNullOrEmpty(Convert.ToString(aluno[8])) ? aluno[8].To<short>() : default(short?);

                        notasEFaltas.NotasAcumuladas = !String.IsNullOrEmpty(Convert.ToString(aluno[9])) ? aluno[9].To<decimal>() : default(decimal?);
                        notasEFaltas.FaltasAcumuladas = !String.IsNullOrEmpty(Convert.ToString(aluno[10])) ? aluno[10].To<short>() : default(short?);
                        notasEFaltas.PercentualFrequenciaAcumulada = !String.IsNullOrEmpty(Convert.ToString(aluno[11])) ? aluno[11].To<decimal>() : default(decimal?);
                    }
                    else
                    {
                        notasEFaltas.Nota3 = !String.IsNullOrEmpty((string)aluno[5]) ? aluno[5].To<decimal>() : default(decimal?);
                        notasEFaltas.Nota4 = !String.IsNullOrEmpty((string)aluno[6]) ? aluno[6].To<decimal>() : default(decimal?);

                        notasEFaltas.Falta1 = aluno[7] != null ? aluno[7].To<short>() : default(short?);
                        notasEFaltas.Falta2 = aluno[8] != null ? aluno[8].To<short>() : default(short?);
                        notasEFaltas.Falta3 = !String.IsNullOrEmpty(Convert.ToString(aluno[9])) ? aluno[9].To<short>() : default(short?);
                        notasEFaltas.Falta4 = !String.IsNullOrEmpty(Convert.ToString(aluno[10])) ? aluno[10].To<short>() : default(short?);

                        notasEFaltas.NotasAcumuladas = !String.IsNullOrEmpty(Convert.ToString(aluno[11])) ? aluno[11].To<decimal>() : default(decimal?);
                        notasEFaltas.FaltasAcumuladas = !String.IsNullOrEmpty(Convert.ToString(aluno[12])) ? aluno[12].To<short>() : default(short?);
                        notasEFaltas.PercentualFrequenciaAcumulada = !String.IsNullOrEmpty(Convert.ToString(aluno[13])) ? aluno[13].To<decimal>() : default(decimal?);
                    }
                }
                else if (periodo == 1 || periodo == 2)
                {
                    notasEFaltas.Falta1 = aluno[5] != null ? aluno[5].To<short>() : default(short?);
                    notasEFaltas.Falta2 = aluno[6] != null ? aluno[6].To<short>() : default(short?);

                    notasEFaltas.NotasAcumuladas = !String.IsNullOrEmpty(Convert.ToString(aluno[7])) ? aluno[7].To<decimal>() : default(decimal?);
                    notasEFaltas.FaltasAcumuladas = !String.IsNullOrEmpty(Convert.ToString(aluno[8])) ? aluno[8].To<short>() : default(short?);
                    notasEFaltas.PercentualFrequenciaAcumulada = !String.IsNullOrEmpty(Convert.ToString(aluno[9])) ? aluno[9].To<decimal>() : default(decimal?);
                }

                lista.Add(notasEFaltas);
            }

            return lista;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Domain;
using Proderj.Foundation.Framework;
using Proderj.Foundation.Common;
using System.Collections;

namespace Proderj.DOL.Repository
{
    public class NotaConsolidadoRepository: NHRepositoryBase<NotaConsolidado>, INotaConsolidadoRepository
    {
        #region INotaConsolidadoRepository Members

        public IEnumerable<NotaConsolidado> EnumeraPor(string codigoDisciplina, string codigoTurma, short ano, short periodo)
        {
            var query = Sessao.CreateSQLQuery("exec SP_MEDIA_POR_BIMESTRE_TURMA :disciplina, :turma, :ano, :periodo");
            query.SetString("disciplina", codigoDisciplina);
            query.SetString("turma", codigoTurma);
            query.SetInt16("ano", ano);
            query.SetInt16("periodo", periodo);

            var notas = query.List();

            IEnumerable<NotaConsolidado> notasConsolidadas = MapeiaNotasConsolidadas(notas);

            return notasConsolidadas;
        }

        private IEnumerable<NotaConsolidado> MapeiaNotasConsolidadas(IList notas)
        {
            List<NotaConsolidado> lista = new List<NotaConsolidado>();

            foreach (object[] n in notas)
            {
                NotaConsolidado nota = new NotaConsolidado
                {
                    SubPeriodo = (n[0] != null) ? n[0].To<short?>() : null,
                    Media = (n[1] != null) ? n[1].To<decimal?>() : null
                };

                lista.Add(nota);
            }

            return lista;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Repository;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Service
{
    public class PeriodoLetivoService : IPeriodoLetivoService
    {
        IPeriodoLetivoRepository repositorioPeriodoLetivo;

        public PeriodoLetivoService(IPeriodoLetivoRepository repositorioPeriodoLetivo)
		{
			this.repositorioPeriodoLetivo = repositorioPeriodoLetivo;
		}


        public List<DTOPeriodoLetivo> Lista()
        {
            List<PeriodoLetivo> listaPeriodo = repositorioPeriodoLetivo.Enumera().ToList();

            List<DTOPeriodoLetivo> listaDtoPeriodoLetivo = listaPeriodo.ConvertAll(periodoLetivo => new DTOPeriodoLetivo
            {
                Ano = periodoLetivo.Ano

            });

            return listaDtoPeriodoLetivo;
        }

        public List<DTOPeriodoLetivo> ListaPor(short ano)
        {
            List<PeriodoLetivo> listaPeriodo = repositorioPeriodoLetivo.EnumeraPor(ano).ToList();

            List<DTOPeriodoLetivo> listaDtoPeriodoLetivo = listaPeriodo.ConvertAll(periodoLetivo => new DTOPeriodoLetivo
            {
                Ano = periodoLetivo.Ano,
                Periodo = periodoLetivo.Periodo,
                DescricaoPeriodo = periodoLetivo.DescricaoPeriodo

            });

            return listaDtoPeriodoLetivo;
        }
    }
}

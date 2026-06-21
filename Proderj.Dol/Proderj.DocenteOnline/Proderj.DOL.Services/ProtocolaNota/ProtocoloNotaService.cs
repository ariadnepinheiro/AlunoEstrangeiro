using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Repository;
using Proderj.DOL.Domain;
using Proderj.Foundation.Common;

namespace Proderj.DOL.Service
{
	public class ProtocoloNotaService : IProtocoloNotaService
	{
		private IProtocoloNotaRepository repositorioProtocoloNota;
		private IDisciplinaRepository repositorioDisciplina;

		public ProtocoloNotaService(IProtocoloNotaRepository repositorioProtocoloNota, IDisciplinaRepository repositorioDisciplina)
		{
			this.repositorioProtocoloNota = repositorioProtocoloNota;
			this.repositorioDisciplina = repositorioDisciplina;
		}

        public List<DTOProtocoloNotaComData> ListaPor(string idFuncional)
		{
            List<Protocolo> listaProtocolos = repositorioProtocoloNota.EnumeraPor(idFuncional).ToList();

			List<DTOProtocoloNotaComData> listaDtoProtocoloNota = listaProtocolos.ConvertAll(
				protocolo => new DTOProtocoloNotaComData
				{
					Id = protocolo.Id,
					Ano = protocolo.Ano,
					CodigoDisciplina = protocolo.Disciplina,
					NomeDisciplina = repositorioDisciplina.ObtemDescricaoPor(protocolo.Disciplina).DescricaoCompleta,
					CodigoTurma = protocolo.Turma,
                    IdFuncional = protocolo.IdFuncional,
					Periodo = protocolo.Periodo,
					SubPeriodo = protocolo.SubPeriodo,
					Tipo = protocolo.Tipo,
					DataCadastro = protocolo.DataCadastro,
				}
			);

			return listaDtoProtocoloNota;				
		}

        public List<DTOProtocoloNotaComData> ListaPor(string idFuncional, string idVinculo, short ano, short periodo)
        {
            List<Protocolo> listaProtocolos = repositorioProtocoloNota.EnumeraPor(idFuncional, idVinculo, ano, periodo).ToList();

            List<DTOProtocoloNotaComData> listaDtoProtocoloNota = listaProtocolos.ConvertAll(
                protocolo => new DTOProtocoloNotaComData
                {
                    Id = protocolo.Id,
                    Ano = protocolo.Ano,
                    CodigoDisciplina = protocolo.Disciplina,
                    NomeDisciplina = repositorioDisciplina.ObtemDescricaoPor(protocolo.Disciplina).DescricaoCompleta,
                    CodigoTurma = protocolo.Turma,
                    IdFuncional = protocolo.IdFuncional,
                    Periodo = protocolo.Periodo,
                    SubPeriodo = protocolo.SubPeriodo,
                    Tipo = protocolo.Tipo,
                    DataCadastro = protocolo.DataCadastro,
                }
            );

            return listaDtoProtocoloNota;
        }

	}
}

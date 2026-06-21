using System;
using Proderj.DOL.Repository;
using Proderj.DOL.Domain;
using Proderj.DOL.Exception;

namespace Proderj.DOL.Service
{
	public class TermoCompromissoDocenteService : ITermoCompromissoDocenteService
    {
		private ITermoCompromissoDocenteRepository repositorioDeTermoDocente;
		private IAceiteTermoCompromissoDocenteRepository repositorioAceiteDeTermo;

		public TermoCompromissoDocenteService(ITermoCompromissoDocenteRepository repositorioDeTermoAceite, IAceiteTermoCompromissoDocenteRepository repositorioAceiteDeTermo)
        {
			this.repositorioDeTermoDocente = repositorioDeTermoAceite;
			this.repositorioAceiteDeTermo = repositorioAceiteDeTermo;
        }

		public DTOTermoAceiteExibicao ObtemTermoNaoAceitoMaisRecentePor(string matricula)
		{
			TermoCompromissoDocente termoAceite = repositorioDeTermoDocente.ObtemTermoNaoAceitoMaisRecentePor(matricula);
			if (termoAceite != null)
			{
				var dtoTermoAceite = new DTOTermoAceiteExibicao
				                     	{
				                     		Ano = termoAceite.Ano,
				                     		Arquivo = termoAceite.Arquivo,
				                     		Codigo = termoAceite.Id
				                     	};
				return dtoTermoAceite;
			}
			return null;
			//throw new TermoCompromissoDocenteException(TermoCompromissoDocenteException.TipoEnum.TermoCompromissoNaoEncontrado);
		}

        public DTOTermoAceiteExibicao ObtemTermoNaoAceitoMaisRecentePorIdFuncional(string idfuncional)
        {
            TermoCompromissoDocente termoAceite = repositorioDeTermoDocente.ObtemTermoNaoAceitoMaisRecentePorIdFuncional(idfuncional);
            if (termoAceite != null)
            {
                var dtoTermoAceite = new DTOTermoAceiteExibicao
                {
                    Ano = termoAceite.Ano,
                    Arquivo = termoAceite.Arquivo,
                    Codigo = termoAceite.Id
                };
                return dtoTermoAceite;
            }
            return null;
            //throw new TermoCompromissoDocenteException(TermoCompromissoDocenteException.TipoEnum.TermoCompromissoNaoEncontrado);
        }

		public int IncluiAceiteDeTermo(DTOTermoAceiteInclusao dtoTermoAceite)
		{
			AceiteTermoCompromissoDocente aceiteTermo = new AceiteTermoCompromissoDocente
			                                            	{
			                                            		Ano = dtoTermoAceite.Ano,
			                                            		DataAceite = dtoTermoAceite.DataAceite,
			                                            		IP = dtoTermoAceite.IP,
			                                            		Num_func = dtoTermoAceite.Matricula,
			                                            		TermoCompromissoDocente =
			                                            			new TermoCompromissoDocente {Id = dtoTermoAceite.IdTermo}
			                  
							                            	};

			return repositorioAceiteDeTermo.Insere(aceiteTermo).Id;
		}
    }
}

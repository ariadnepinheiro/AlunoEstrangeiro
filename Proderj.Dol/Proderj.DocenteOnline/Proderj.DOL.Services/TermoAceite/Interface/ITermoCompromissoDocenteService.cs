using Proderj.Foundation.Architecture;

namespace Proderj.DOL.Service
{
    public interface ITermoCompromissoDocenteService : IService
    {
    	DTOTermoAceiteExibicao ObtemTermoNaoAceitoMaisRecentePor(string matricula);
        DTOTermoAceiteExibicao ObtemTermoNaoAceitoMaisRecentePorIdFuncional(string idfuncional);
        
    	int IncluiAceiteDeTermo(DTOTermoAceiteInclusao dtoTermoAceite);
    }
}

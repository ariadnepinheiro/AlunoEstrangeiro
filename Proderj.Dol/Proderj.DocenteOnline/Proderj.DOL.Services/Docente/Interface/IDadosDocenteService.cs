using System.Collections.Generic;
using Proderj.DOL.Domain;
using Proderj.Foundation.Architecture;

namespace Proderj.DOL.Service
{
    public interface IDadosDocenteService: IService
    {
        DadosGeraisDocenteDTO ObtemPor(string matricula);
        List<DadosFormacaoDocenteDTO> ListaFormacaoPor(string matricula, TipoFormacaoEnum tipoFormacao);
        List<DadosCapacitacaoDocenteDTO> ListaCapacitacaoPor(string matricula);
        List<DadosTurmaDocenteDTO> ListaTurmaPor(string matricula);
        List<DadosAcessoDocenteDTO> ListaAcessoPor(string matricula);
    }
}

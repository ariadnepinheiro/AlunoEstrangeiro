using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Domain;
using Proderj.Foundation.Architecture;

namespace Proderj.DOL.Repository
{
    public interface IDadosFormacaoDocenteRepository: IRepository<DadosFormacaoDocente>
    {
        IList<DadosFormacaoDocente> ListaFormacaoPor(string matricula, TipoFormacaoEnum tipoFormacao);
    }
}

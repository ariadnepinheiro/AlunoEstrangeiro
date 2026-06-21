using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Architecture;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository
{
    public interface IDadosGeraisDocenteRepository: IRepository<DadosGeraisDocente>
    {
        DadosGeraisDocente ObtemPor(string matricula);

        DadosGeraisDocente ObtemPorPessoa(string pessoa);

        List<string> ObtemEmailsPor(string cpf, out string nome);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Architecture;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository
{
    public interface IUltimoResetRepository : IRepository<UltimoReset>
    {
        UltimoReset VerificaDadosUltimoReset(String matricula);

        void AtualizaUltimoResetPorMatricula(String matricula);
    }
}

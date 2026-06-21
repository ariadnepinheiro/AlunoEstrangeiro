using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Service
{
    public interface IMaterialEstudoService
    {
        List<DTOMaterialEstudo> ObtemPor(string identificador);
        void Gravar(string identificador, List<DTOMaterialEstudo> lstMaterialEstudo);
        List<DTOMaterialEstudo> ObtemIds();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Service
{
    public interface IDISPONIBILIDADEGLPService
	{
        IEnumerable<DTOListaDISPONIBILIDADEGLP> ListaPor(long num_func, int ano);
        DISPONIBILIDADEGLP Inclui(DTOIncluiDISPONIBILIDADEGLP dto);
        void Exclui(int disponibilidadeGlpId, string unidadeEnsino);
	}
}

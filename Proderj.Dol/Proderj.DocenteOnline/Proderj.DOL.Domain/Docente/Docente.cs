using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
	public class Docente
    {
        public Docente()
        {
            Pessoa = new Pessoa();
        }

        #region Propriedades

        public virtual long NumeroFuncionario { get; set; }

        public virtual string Matricula { get; set; }

        public virtual string IdFuncional { get; set; }

        public virtual string Vinculo { get; set; }

        public virtual string SenhaDocente { get; set; }

        public virtual string SenhaAlterada { get; set; }

        public virtual string IdVinculo { get; set; }

        public virtual Pessoa Pessoa { get; set; }

        #endregion

    }
}

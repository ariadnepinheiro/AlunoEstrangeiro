using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
    public class Pessoa
    {
        public Pessoa()
        {}
        
        #region Propriedades

        public virtual long Id { get; set; }

        public virtual string NomeCompleto { get; set; }

        public virtual string PreNomeSocial { get; set; }

        public virtual string Telefone { get; set; }

        public virtual string EmailInterno { get; set; }

        //Campo criado por Felipe Ribeiro Gomes em 22/06/2022.
        //Para este campo funcionar, é preciso mapeá-lo na Domain.ClassMap.
        //Não vou mexer na VW_LY_PESSOA porque não sei quais sistemas usam ela.
        //public virtual string EmailGoogle { get; set; }

        #endregion
    }
}

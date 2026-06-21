using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
    public class MaterialEstudo
    {
        public MaterialEstudo() 
        {
		}
        

        public virtual int MaterialEstudoId { get; set; }

        public virtual string Descricao { get; set; }

        public virtual bool Ativo { get; set; }

        public virtual string UsuarioId { get; set; }

        public virtual DateTime? DataCadastro { get; set; }

        public virtual DateTime? DataAlteracao { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeduc.Infra.MapeamentoAtributos
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AtributoTabela
     : Attribute{
        private string nome;
        public string Nome
		{
			get { return nome; }
			set { nome = value; }
		}

        public AtributoTabela(string nome)
		{
			this.nome = nome;
		}
    }
}

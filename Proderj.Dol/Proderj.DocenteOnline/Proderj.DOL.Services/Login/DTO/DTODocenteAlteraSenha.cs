using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Service
{
    public class DTODocenteAlteraSenha
    {
        public string Matricula { get; set; }
		public string IdFuncional { get; set; }
		public string Vinculo { get; set; }
        public string SenhaAtual { get; set; }
        public string SenhaNova { get; set; }
        public string SenhaNovaConfirmacao { get; set; }
    }
}

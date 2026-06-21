using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SRV.Models.Domain
{
    public class TokenAlteracaoSenha
    {
        public int IdTokenAlteracaoSenha { get; set; }

        public string DesToken { get; set; }

        public DateTime DtValidade { get; set; }

        public Usuario Usuario { get; set; }
    }
}
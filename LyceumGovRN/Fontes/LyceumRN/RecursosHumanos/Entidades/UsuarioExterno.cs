using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.RecursosHumanos.Entidades
{
    public class UsuarioExterno
    {
        public int UsuarioExternoId { get; set; }

        public int TipoUsuarioExternoId { get; set; }

        public decimal PessoaId { get; set; }

        public bool Ativo { get; set; }

        public string UsuarioId { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAlteracao { get; set; }
    }
}

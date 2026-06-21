using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.NecessidadeEspecial.Entidades
{
    public class NecessidadeEspecial
    {
        public int NecessidadeEspecialId { get; set; }

        public string Item { get; set; }

        public string Descricao { get; set; }

        public bool Ativo { get; set; }

        public bool Ledor { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAlteracao { get; set; }

        public string UsuarioId { get; set; }
    }
}

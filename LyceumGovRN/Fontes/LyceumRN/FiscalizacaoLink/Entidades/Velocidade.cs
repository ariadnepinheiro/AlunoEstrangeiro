using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.FiscalizacaoLink.Entidades
{
    public class Velocidade
    {
        public int VelocidadeId { get; set; }

        public int UnidadeVelocidadeId { get; set; } 

        public int Valor { get; set; }

        public bool Ativo { get; set; }

        public string UsuarioId { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAlteracao { get; set; }
    }
}

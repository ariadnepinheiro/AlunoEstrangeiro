using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.Pedagogico.Entidades
{
    public class ItinerarioFormativo
    {
        public int ItinerarioFormativoId { get; set; }

        public int CategoriaItinerarioFormativoId { get; set; }

        public string Descricao { get; set; }

        public string Objetivo { get; set; }

        public bool Oferta { get; set; }

        public bool Ativo { get; set; }

        public string UsuarioId { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAlteracao { get; set; }
    }
}

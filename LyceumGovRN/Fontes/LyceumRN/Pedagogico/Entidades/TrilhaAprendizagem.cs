using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.Pedagogico.Entidades
{
    public class TrilhaAprendizagem
    {
        public int TrilhaAprendizagemId { get; set; }

        public int ItinerarioFormativoId { get; set; }

        public string Descricao { get; set; }

        public string Tipo { get; set; }

        public string Objetivo { get; set; }

        public bool Oferta { get; set; }

        public bool Ativo { get; set; }

        public string UsuarioId { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAlteracao { get; set; }
    }
}

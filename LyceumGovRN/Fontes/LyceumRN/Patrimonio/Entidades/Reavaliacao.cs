using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.Patrimonio.Entidades
{
    public class Reavaliacao
    {
        public int ReavaliacaoId { get; set; }

        public bool Inservivel { get; set; }

        public int BemValorId { get; set; }

        public int? MoedaId { get; set; }

        public int? VidaAdicional { get; set; }

        public decimal? ValorMercado { get; set; }

        public DateTime DataReavaliacao { get; set; }

        public string UsuarioId { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAlteracao { get; set; }
    }
}

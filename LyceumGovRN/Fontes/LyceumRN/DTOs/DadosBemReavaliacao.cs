using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosBemReavaliacao
    {
        public int BemId { get; set; }

        public DateTime DataAquisicao { get; set; }

        public bool? Inservivel { get; set; }

        public string Processo { get; set; }

        public int? EstadoconservacaoId { get; set; }

        public int? MoedaId { get; set; }

        public int? VidaAdicional { get; set; }

        public decimal? ValorMercado { get; set; }

        public decimal? ValorCalculado { get; set; }

        public DateTime DataReavaliacao { get; set; }

        public string UsuarioId { get; set; }

        public decimal? UltimoValorAtualizado { get; set; }

        public int ClassificacaoId { get; set; }

    }
}

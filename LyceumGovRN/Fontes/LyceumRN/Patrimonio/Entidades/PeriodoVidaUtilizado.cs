using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.Patrimonio.Entidades
{
    public class PeriodoVidaUtilizado
    {
        public int PeriodoVidaUtilizadoId { get; set; }

        public string Conceito { get; set; }

        public int QuantidadeAnos { get; set; }

        public int Pontuacao { get; set; }

        public bool Ativo { get; set; }

        public string UsuarioId { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAlteracao { get; set; }
    }
}

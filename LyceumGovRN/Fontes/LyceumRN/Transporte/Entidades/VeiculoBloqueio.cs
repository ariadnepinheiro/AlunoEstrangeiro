using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Transporte.Entidades
{
    [AtributoTabela("Transporte.VEICULOBLOQUEIO", Nome = "Transporte.VEICULOBLOQUEIO")]
    public class VeiculoBloqueio
    {
        [AtributoCampo(Nome = "VEICULOBLOQUEIOID")]
        public int VeiculoBloqueioId { get; set; }

        [AtributoCampo(Nome = "CONDUTORID")]
        public int VeiculoId { get; set; }

        [AtributoCampo(Nome = "MOTIVOBLOQUEIOID")]
        public int MotivoBloqueioId { get; set; }

        public string Observacao { get; set; }

        [AtributoCampo(Nome = "USUARIOBLOQUEIOID")]
        public string UsuarioBloqueioId { get; set; }

        [AtributoCampo(Nome = "DATABLOQUEIO")]
        public DateTime DataBloqueio { get; set; }

        [AtributoCampo(Nome = "USUARIODESBLOQUEIOID")]
        public string UsuarioDesbloqueioId { get; set; }

        [AtributoCampo(Nome = "datadesbloqueio")]
        public DateTime? DataDesbloqueio { get; set; }
    }
}

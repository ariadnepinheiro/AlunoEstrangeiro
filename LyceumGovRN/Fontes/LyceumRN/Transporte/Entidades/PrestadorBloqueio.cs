using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Transporte.Entidades
{
    [AtributoTabela("Transporte.PRESTADORBLOQUEIO", Nome = "Transporte.PRESTADORBLOQUEIO")]
    public class PrestadorBloqueio : IEntity
    {
        [AtributoCampo(Nome = "PRESTADORBLOQUEIOID")]
        public int PrestadorBloqueioId { get; set; }

        [AtributoCampo(Nome = "PRESTADORID")]
        public int PrestadorId { get; set; }

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

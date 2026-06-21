using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Transporte.Entidades
{
    [AtributoTabela("Transporte.CONDUTORBLOQUEIO", Nome = "Transporte.CONDUTORBLOQUEIO")]
    public class CondutorBloqueio : IEntity
    {
        [AtributoCampo(Nome = "CONDUTORBLOQUEIOID")]
        public int CondutorBloqueioId { get; set; }

        [AtributoCampo(Nome = "CONDUTORID")]
        public int CondutorId { get; set; }

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

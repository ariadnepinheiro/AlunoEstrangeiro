using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Common.Logging;
using System.ComponentModel.DataAnnotations;

namespace SRV.Models.Domain
{
    public class DenunciaAvaliacaoExterna
    {
        [PrimaryKey]
        [Display(Name = "Código")]
        public int IdDenunciaAvaliacaoExterna { get; set; }

        [PrimaryKey]
        public AnoReferencia AnoReferencia { get; set; }

        [PrimaryKey]
        public AvaliacaoExterna AvaliacaoExterna { get; set; }

        [PrimaryKey]
        public Servidor Servidor { get; set; }

        [PrimaryKey]
        public UnidadeAdministrativa UnidadeAdministrativa { get; set; }

        public string DesMotivoDenuncia { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.RecursosHumanos.Entidades
{
    [AtributoTabela("RecursosHumanos.CARGAHNAOTRABMESFINALIZADA", Nome = "RecursosHumanos.CARGAHNAOTRABMESFINALIZADA")]
    public class CargaHNaoTrabMesFinalizada
    {
        [AtributoCampo(Nome = "CARGAHNAOTRABMESFINALIZADAID")]
        public int CargaHNaoTrabMesFinalizadaId { get; set; }

        [AtributoCampo(Nome = "CENSO")]
        public string Censo { get; set; }

        [AtributoCampo(Nome = "CENSO")]
        public int Ano { get; set; }

        [AtributoCampo(Nome = "CENSO")]
        public int Mes { get; set; }

        [AtributoCampo(Nome = "DATAFINALIZACAO")]
        public DateTime DataFinalizacao { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }        
    }
}

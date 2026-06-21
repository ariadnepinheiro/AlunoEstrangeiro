using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Pedagogico.Entidades
{
    [AtributoTabela("Pedagogico.TRILHAAPRENDIZAGEMESCOLAFINALIZADA", Nome = "Pedagogico.TrilhaAprendizagemEscolaFinalizada")]
    public class TrilhaAprendizagemEscolaFinalizada
    {
        [AtributoCampo(Nome = "TrilhaAprendizagemEscolaFinalizadaID")]
        public int TrilhaAprendizagemEscolaFinalizadaId { get; set; }

        [AtributoCampo(Nome = "CENSO")]
        public string Censo { get; set; }

        [AtributoCampo(Nome = "ANO")]
        public int Ano { get; set; }        

        [AtributoCampo(Nome = "DATAFINALIZACAO")]
        public DateTime DataFinalizacao { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }        
    }
}

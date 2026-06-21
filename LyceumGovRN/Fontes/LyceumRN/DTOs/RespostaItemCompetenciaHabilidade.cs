using System;

namespace Techne.Lyceum.RN.DTOs
{
    public class RespostaItemCompetenciaHabilidade
    {
        public string CompetenciaHabilidade { get; set; } 
       
        public int IdCompetenciaHabilidadeGrupo { get; set; }

        public int IdCompetenciaHabilidadeItem { get; set; }

        public bool Resposta { get; set; }

        public DateTime DtCadastro  { get; set; }
    }
}
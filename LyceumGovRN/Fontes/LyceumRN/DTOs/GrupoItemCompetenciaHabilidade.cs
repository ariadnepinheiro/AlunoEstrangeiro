namespace Techne.Lyceum.RN.DTOs
{
    using System.Collections.Generic;

    public class GrupoItemCompetenciaHabilidade
    {
        public GrupoItemCompetenciaHabilidade()
        {
            this.Itens = new List<RespostaItemCompetenciaHabilidade>();
        }

        public string Grupo { get; set; }

        public int IdCompetenciaHabilidadeGrupo { get; set; }

        public ICollection<RespostaItemCompetenciaHabilidade> Itens { get; set; }
    }
}

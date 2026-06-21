namespace Techne.Lyceum.RN.Entidades
{
    using Seeduc.Infra.Entities;

    public class TceCompetenciaHabilidadeItem : IEntity
    {
        public string CompetenciaHabilidade { get; set; }

        public string DtAlteracao { get; set; }

        public string DtCadastro { get; set; }

        public int IdCompetenciaHabilidadeGrupo { get; set; }

        public int IdCompetenciaHabilidadeItem { get; set; }

        public string Matricula { get; set; }

        public int Ordem { get; set; }
    }
}
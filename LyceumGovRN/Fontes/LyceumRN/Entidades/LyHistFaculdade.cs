namespace Techne.Lyceum.RN.Entidades
{
    using Seeduc.Infra.Entities;

    public class LyHistFaculdade : IEntity
    {
        public string Aluno { get; set; }

        public decimal Ordem { get; set; }

        public string OutraFaculdade { get; set; }
    }
}
